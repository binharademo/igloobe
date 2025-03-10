using System;
using System.Collections;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Br.Com.IGloobe.Apps.Main.Gui {

    public partial class FormAppsPresentation : DraggableForm {

        private static FormAppsPresentation _singleton;
        private string _simpleFileName;
        private Process _presentationProcess;
        private readonly int _windowsToolbar;

        [DllImport("user32.dll")] private static extern int FindWindow(string className, string windowText);
        [DllImport("user32.dll")] private static extern int ShowWindow(int hwnd, int command);

        private const string LeftKey = "{LEFT}";
        private const string RightKey = "{RIGHT}";

        private const int SwHide = 0;
        private const int SwShow = 1;

        private FormAppsPresentation() {
            InitializeComponent();
            MakeTrasparent();
            MoveToBottomLeftCorner();
            InitializeDragSupport(ShowImage);
            _windowsToolbar = FindWindow("Shell_TrayWnd", "");
        }

        private void BtnCloseWindow_Click(object sender, EventArgs e) { CloseFile(); }

        public void CloseFile() {
            ShowWindowsToolbar();

            if (_presentationProcess != null && !_presentationProcess.HasExited)
                _presentationProcess.Kill();

            Visible = false;
            BtnLeft.Enabled = false;
            BtnRight.Enabled = false;
            BtnOpen.Enabled = true;

            _checkPresentation.Stop();
        }

        public void ShowWindowsToolbar() {
            ShowWindow(_windowsToolbar, SwShow);
        }

        public static FormAppsPresentation Singleton() {
            if (_singleton == null || _singleton.IsDisposed)
                _singleton = new FormAppsPresentation();
            return _singleton;
        }

        private void BtnOpen_Click(object sender, EventArgs e)  {

            DialogResult result = _fileDialog.ShowDialog(this);
            if (result != DialogResult.OK) {
                _simpleFileName = null;
                return;
            }

            _simpleFileName = _fileDialog.SafeFileName;
            if (_simpleFileName == null) return;

            FormAppsDraw.Singleton().SetLocationToPointer();

            string suffix = "";
 
            if (_simpleFileName.EndsWith(".ppt")){
                _simpleFileName = _simpleFileName.Substring(0, _simpleFileName.Length - 4);
                suffix = ".pps";
            }

            if (_simpleFileName.EndsWith(".pptx")){
                _simpleFileName = _simpleFileName.Substring(0, _simpleFileName.Length - 5);
                suffix = ".pps";
            }

            string destFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "iGloobePresentation" + suffix);

            System.IO.File.Delete(destFile);
            System.IO.File.Copy(_fileDialog.FileName, destFile);

            _simpleFileName = destFile;
            _presentationProcess = Process.Start(_simpleFileName);

            BtnRight.Enabled = true;
            BtnLeft.Enabled = true;
            BtnOpen.Enabled = false;

            HideWidowsToolbar();
            _checkPresentation.Start();
        }

        private void HideWidowsToolbar() {
            ShowWindow(_windowsToolbar, SwHide);
        }

        private bool CanSendKey() {
            return _presentationProcess != null ;
        }

        private void BtnLeft_Click(object sender, EventArgs e) {
            FormAppsDraw.Singleton().SetLocationToPointer();
            new Thread(new ThreadStart(BtnLeft_Click)).Start(); 
        }
        private void BtnLeft_Click() {
            TrySendKeys(LeftKey);
        }

        private void BtnRight_Click(object sender, EventArgs e) {
            FormAppsDraw.Singleton().SetLocationToPointer();
            new Thread(new ThreadStart(BtnRight_Click)).Start(); 
        }
        private void BtnRight_Click() {
            TrySendKeys(RightKey);
        }

        private void TrySendKeys(string cmd) {
            IntPtr lastWindow = Window.GetForegroundWindow();

            while ((int)_presentationProcess.MainWindowHandle == 0)
                Thread.Sleep(10);

            Window.SetForegroundWindow(_presentationProcess.MainWindowHandle);
            Thread.Sleep(50);
            SendKeys.SendWait(cmd);
            Window.SetForegroundWindow(lastWindow);
        }

        private void CheckPresentationTick(object sender, EventArgs e) {
            try {
                if (!_presentationProcess.HasExited) return;
                CloseFile();
            } catch { 
                //bye-bye
            }
        }
    }
}
