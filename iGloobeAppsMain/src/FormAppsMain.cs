using System;
using System.Windows.Forms;
using Br.Com.IGloobe.Connector.Commands;
using Br.Com.IGloobe.Connector.Gui;
using System.Diagnostics;

namespace Br.Com.IGloobe.Apps.Main.Gui {
    public partial class FormAppsMain : DraggableForm {

        private static FormAppsMain _singleton;

        private FormAppsMain() {
            InitializeComponent();
            MakeTrasparent();
            MoveToBottomRightCorner();
            InitializeDragSupport(IgloobeImage);
        }

        private void BtnCloseWindow_Click(object sender, EventArgs e) {
            FormAppsPresentation form = FormAppsPresentation.Singleton();
            form.ShowWindowsToolbar();
            form.CloseFile();
            Environment.Exit(1);
        }

        public static FormAppsMain Singleton() {
            _singleton = _singleton ?? (_singleton = new FormAppsMain());
            _singleton.Visible = false;
            FormFindAndConnect.Singleton().Show(_singleton);
            _singleton.IsReadyToUse.Start();
            return _singleton;
        }

        private void BtnCalibrateClick(object sender, EventArgs e) {
            FormFindAndConnect frm = FormFindAndConnect.Singleton();
            frm.Visible = true;
            frm.WindowState = FormWindowState.Normal;
            frm.Activate();

            try { frm.Show(this); }
            // ReSharper disable EmptyGeneralCatchClause            
            catch { /* ignore */ }
            // ReSharper restore EmptyGeneralCatchClause
        }

        private void BtnPaintClick(object sender, EventArgs e) {
            FormAppsDraw frm = FormAppsDraw.Singleton();
            frm.WindowState = FormWindowState.Normal;

            try { frm.Show(this); }    
            catch { frm.Visible = true; }
        }

        private void BtnShowClick(object sender, EventArgs e) {
            FormAppsPresentation frm = FormAppsPresentation.Singleton();
            frm.WindowState = FormWindowState.Normal;

            try { frm.Show(this); }
            catch { frm.Visible = true; }
        }

        private void BtnMouseClick(object sender, EventArgs e) {
            RightClick.PrepareIt();
        }

        private void BtnKeyboardClick(object sender, EventArgs e) {
            Process.Start("osk.exe");
        }

        private void IsReadyToUseTick(object sender, EventArgs e) {
            if (!FormFindAndConnect.Singleton().IsReadyToUse) return;

            IsReadyToUse.Stop();
            BtnPaint.Enabled = true;
            BtnShow.Enabled = true;
            BtnMouse.Enabled = true;
        }
    }
}
