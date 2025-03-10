using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Br.Com.IGloobe.Connector.Mote;

namespace Br.Com.IGloobe.Connector.Gui {

    public partial class FormCalibration: Form, IRListener{

        private const int Offset = 30;
        private const int CrosshairSize = 26;

        private readonly float[] _dstX = new float[4];
       	private readonly float[] _dstY = new float[4];

        private readonly float[] _srcX = new float[4];
        private readonly float[] _srcY = new float[4];

        private readonly Bitmap _bCalibration;
        private readonly Graphics _gCalibration;
        
        private int _counter;
        private Rectangle _screenSize;

        private static FormCalibration _singleton;
        public static FormCalibration Singleton() {
            if (_singleton == null || _singleton.IsDisposed)
                _singleton = new FormCalibration();
            return _singleton;
        }

        private FormCalibration() {
            _screenSize = Screen.GetBounds(this);
            KeyDown += OnKeyPress;

            InitializeComponent();
            Text = "Calibração - Área de Trabalho:" + Screen.GetWorkingArea(this) + 
                       " || Área Real: " + Screen.GetBounds(this);
            FormBorderStyle = FormBorderStyle.None;

            Left = 0;
            Top = 0;
            Size = new Size(_screenSize.Width, _screenSize.Height);

            pbCalibrate.Left = 0;
            pbCalibrate.Top = 0;
            pbCalibrate.Size = new Size(_screenSize.Width, _screenSize.Height);

            _bCalibration = new Bitmap(_screenSize.Width, _screenSize.Height, PixelFormat.Format24bppRgb);
            _gCalibration = Graphics.FromImage(_bCalibration);          
        }

        private void OnKeyPress(object sender, KeyEventArgs e) {
            if(e.KeyCode != Keys.Escape) return;
            Dispose();
        }

        private void DrawCrosshair(int x, int y) {
            Pen p = new Pen(Color.Red);
            _gCalibration.DrawEllipse(p, x - CrosshairSize / 2, y - CrosshairSize / 2, CrosshairSize, CrosshairSize);
            _gCalibration.DrawLine(p, x - CrosshairSize, y, x + CrosshairSize, y);
            _gCalibration.DrawLine(p, x, y - CrosshairSize, x, y + CrosshairSize);
        }

        public void ShowCalibrationPoint(int counter, int x, int y) {
            _dstX[counter] = x;
            _dstY[counter] = y;
            System.Console.WriteLine(x + @", " + y);

            _gCalibration.Clear(Color.White);
            DrawCrosshair(x, y);
            BeginInvoke((MethodInvoker)delegate {
                pbCalibrate.Image = _bCalibration;
            });
        }
        
        public void Calibrate() {
            Show();
            ShowCalibrationPoint(0, Offset, Offset);     
        }

        public void StateChanged(bool lightsOn, IrState irState) {
            if(!lightsOn) return;

            System.Console.Beep();
            _srcX[_counter] = irState.RawX1;
            _srcY[_counter] = irState.RawY1;
            _counter++;

            if(_counter == 1) {
                ShowCalibrationPoint(_counter, pbCalibrate.Size.Width - Offset, Offset);
                return;
            }

            if(_counter == 2) {
                ShowCalibrationPoint(_counter, pbCalibrate.Size.Width - Offset, pbCalibrate.Size.Height - Offset);
                return;
            }

            if(_counter == 3) {
                ShowCalibrationPoint(_counter, Offset, pbCalibrate.Size.Height - Offset);
                return;
            }

            _gCalibration.Clear(Color.White);
            FormFindAndConnect frm = FormFindAndConnect.Singleton();
            frm .ExitCalibration();
            frm.ActivateControlCursor();

            CalibrationData data = new CalibrationData(_screenSize.Width, _screenSize.Height, 
                                                                           _dstX, _dstY, _srcX, _srcY);

            frm.ComputeWarp(data);
            BeginInvoke((MethodInvoker)delegate {
                frm.WindowState = FormWindowState.Minimized;
                frm.Visible = false;
                pbCalibrate.Image = _bCalibration;
                Dispose();
            });
        }
    }
}
