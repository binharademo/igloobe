using System;
using System.Windows.Forms;
using System.Threading;
using System.Resources;
using Br.Com.IGloobe.Connector.Mote;

namespace Br.Com.IGloobe.Connector.Gui {

    public partial class FormFindAndConnect:Form, IGloobeStateListener, IRListener {

        private readonly Connector _connector;
        private static FormCalibration _formCalibration;

        private ThreadStart _command;

        private bool _lightsOn;
        private bool _isReadyToUse;
        public bool IsReadyToUse { get { return _isReadyToUse; } }

        private DateTime _timeout;
        private const int TimeOutDelta = 20;

        private readonly ResourceManager _resourceManager = new ResourceManager(typeof(Localization).FullName,
                                                             System.Reflection.Assembly.GetExecutingAssembly());
       
        private static FormFindAndConnect _singleton;
        public static FormFindAndConnect Singleton() {
            if (_singleton == null || _singleton.IsDisposed)
                _singleton = new FormFindAndConnect();
            return _singleton;
        }

        private FormFindAndConnect() {
            Localization.LoadResources();
            InitializeComponent();
            _connector = Connector.NewInstance();
            ShowInTaskbar = false;
        }

        private void UpdateGui(string btnText, string msg, 
                                            bool enableBtn = false, 
                                            bool visibleBtn = false, 
                                            ThreadStart cmd = null) {

            BeginInvoke((MethodInvoker) delegate {
                waitGif.Visible = Localization.BtnMsgPleaseWait == msg;
                btnConnect.Text = btnText;
                txtWellcome.Text = msg;
                btnConnect.Enabled = enableBtn;
                btnConnect.Visible = visibleBtn;
                _command = cmd;
            });
        }

        public void StateChanged(bool lightsOn, IrState irState) {
            if(controlCursor.Checked) {
                _lightsOn = false;
                return;
            }
            _lightsOn = lightsOn;
        }

        public void StateChanched(ConnectionState state) {

            if (state == null) return;
            if (IsDisposed) return;

            BeginInvoke((MethodInvoker)delegate {
                lbStatus.Text = _resourceManager.GetString(state.GetType().Name);
            });
            
            switch (state.Id) {
                case ConnectionState.Success.NotInitialized:
                    UpdateGui("", Localization.BtnMsgPleaseWait);
                    break;

                case ConnectionState.Success.BluetoothClientFound:
                    UpdateGui(Localization.BtnMsgPleaseWait,
                                    Localization.MsgBluetoothOkSearchingIgloobe);
                    break;

                case ConnectionState.Success.ReadyToUse:
                    if (_isReadyToUse) return;

                    _isReadyToUse = true;
                    UpdateGui(Localization.BtnMsgCalibrate,
                                    Localization.MsgReadyToUse,
                                    true, true, Calibrate);

                    BeginInvoke((MethodInvoker)delegate {
                                   controlCursor.Visible = true;
                                   lbSuavisar.Visible = true;
                                   slideSmooth.Visible = true;

                                   RestoreCalibrationData();
                                   connectionHeartbeat.Start();
                               });
                    break;

                case ConnectionState.Fail.BluetoothClientNotFound:
                    UpdateGui(Localization.BtnMsgSearchBluetoothAgain,
                                     Localization.MsgNotInitialized, true, true,
                                     Retry);
                    break;

                case ConnectionState.Fail.HasNoDevices:
                case ConnectionState.Fail.NotFoundIgloobeDevice:
                    UpdateGui(Localization.BtnMsgSearchIgloobeAgain,
                                     Localization.MsgDeviceNotFound, true, true,
                                     Retry);
                    break;

                case ConnectionState.Fail.InvalidLicense:
                    UpdateGui(Localization.BtnMsgExit,
                                    Localization.MsgInvalidLicense,
                                    true, true, Exit);
                    break;

                case ConnectionState.Fail.ConnectionProblem:
                    LookAtMe();
                    UpdateGui(Localization.BtnMsgExit,
                                    Localization.MsgConnectionProblem,
                                    true, true, Exit);

                    BeginInvoke((MethodInvoker)delegate {
                        lightTimer.Stop();
                        _lightsOn = false;
                        imgLightsOn.Visible = false;

                        connectionHeartbeat.Stop();
                        Console.Beep();
                        controlCursor.Visible = false;
                        lbSuavisar.Visible = false;
                        slideSmooth.Visible = false;
                        viewTest.Visible = false;
                        FormCalibration.Singleton().Visible = false;
                    });
                    break;

                default:
                    UpdateGui("", Localization.BtnMsgPleaseWait);
                    break;
            }   
        }

        private void Retry() {
            UpdateGui("", Localization.BtnMsgPleaseWait);
            BeginInvoke((MethodInvoker)delegate {
                lbStatus.Text = @"...";
            });
            new Thread(() => _connector.Retry()).Start();
        }

        private static void LookAtMe() {
            Singleton().Visible = true;
            Singleton().WindowState = FormWindowState.Normal;
            Singleton().Activate();
        }

        private void RestoreCalibrationData() {
            _connector.IRListener = this;
            ActivateControlCursor();
            if (!CalibrationData.HasStoredData()) {
                ViewTest();
                return;
            }

            try {
                _connector.ComputeWarp(CalibrationData.Restore());
                DialogResult dlgResult = MessageBox.Show(this, Localization.MsgDlgTxt, Localization.MsgDlgTitle,
                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dlgResult == DialogResult.Yes) {
                    WindowState = FormWindowState.Minimized;
                    Singleton().Visible = false;
                    return;
                }
            } catch {
                Console.WriteLine(@"Can't Restore Saved Calibration Data.");
            }

            ViewTest();
        }

        public void ExitCalibration() { _connector.IRListener = this; }

        private void Calibrate() {
            _connector.SmoothingAmount = slideSmooth.Value;
            controlCursor.Checked = false;
            imgLightsOn.Visible = false;

            if(_formCalibration != null && !_formCalibration.IsDisposed)
                _formCalibration.Dispose();
            
            _formCalibration = FormCalibration.Singleton();
            _connector.IRListener = _formCalibration;
            _formCalibration.Calibrate();
        }

        public void ActivateControlCursor() {
            BeginInvoke((MethodInvoker)delegate {
                controlCursor.Visible = true;
                controlCursor.Checked = true;
                viewTest.Visible = true;
                lbStatus.Visible = true;
                slideSmooth.Visible = true;
            });
        }

        public void ViewTest() {
            BeginInvoke((MethodInvoker)delegate {
                _connector.CursorControl = false;
                controlCursor.Checked = false;
            });
        }

        #region form events

        private void OnLightTick(object sender, EventArgs e) { imgLightsOn.Visible = _lightsOn; }
        private void OnBeepTick(object sender, EventArgs e) { if (_lightsOn) Console.Beep(); }
        private void OnViewTestValueChanged(object sender, EventArgs e) { controlCursor.Checked = !viewTest.Checked; }
        private void OnSmoothChangeValue(object sender, EventArgs e) { _connector.SmoothingAmount = slideSmooth.Value; }

        private void OnBtnClick(object sender, EventArgs e) {
            if(_command == null) return;
            _command.Invoke();
        }

        private void OnControlCursorValueChanged(object sender, EventArgs e) {
            _connector.CursorControl = controlCursor.Checked;
            viewTest.Checked = !controlCursor.Checked;
            slideSmooth.Enabled = controlCursor.Checked;
            lbSuavisar.Enabled = controlCursor.Checked;

            if (controlCursor.Checked) {
                lightTimer.Stop();
                beepTimer.Stop();
                return;
            }

            lightTimer.Start();
            beepTimer.Start();
        }

        private static void Exit() {
            Environment.Exit(1);
        }

        private void OnConnectionHeartbeat(object sender, EventArgs e) {
            connectionHeartbeat.Stop();

            try {
                Connector.Log("On Connection Heartbeat...");
                if (!Hardware.CheckConnectionHeartbeats) return;

                DateTime tmp = _connector.LastHardwareTickTime.AddSeconds(TimeOutDelta);

                if (_timeout.CompareTo(DateTime.Now) >= 0)
                    return;

                if (tmp > _timeout) {
                    _timeout = tmp;
                    return;
                }

                if (IsConnectionWorking()) return;

                _connector.CurrentState = _connector.CurrentState.State(ConnectionState.Fail.ConnectionProblem);             
            } finally {
                connectionHeartbeat.Start();
            }
        }

        #endregion

        private bool IsConnectionWorking() {
            try {
                _connector.Write(0, 1);
                return _connector.Read(0, 1)[0] == 1;
            } catch {
                return false;
            }
        }

        public void ComputeWarp(CalibrationData data) {
            _connector.ComputeWarp(data);
        }

        private void FormFindAndConnect_Shown(object sender, EventArgs e) {
            _connector.StateListener = this;
            _connector.CurrentState = _connector.CurrentState.State(ConnectionState.Success.SearchingBluetooth);
        }

        private void OnStatusDoubleClick(object sender, EventArgs e) {
            ConsoleManager.Show();
        }

        private void FormFindAndConnect_Deactivate(object sender, EventArgs e) {
            if(WindowState == FormWindowState.Minimized)
            Visible = false;
        }
    }
}