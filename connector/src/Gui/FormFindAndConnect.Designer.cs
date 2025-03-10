namespace Br.Com.IGloobe.Connector.Gui {

    partial class FormFindAndConnect {

        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFindAndConnect));
            this.txtWellcome = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.status = new System.Windows.Forms.StatusStrip();
            this.lbStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.connectionHeartbeat = new System.Windows.Forms.Timer();
            this.lightTimer = new System.Windows.Forms.Timer();
            this.imgLightsOn = new System.Windows.Forms.PictureBox();
            this.imgLogo = new System.Windows.Forms.PictureBox();
            this.slideSmooth = new System.Windows.Forms.TrackBar();
            this.lbSuavisar = new System.Windows.Forms.Label();
            this.beepTimer = new System.Windows.Forms.Timer();
            this.controlCursor = new System.Windows.Forms.RadioButton();
            this.viewTest = new System.Windows.Forms.RadioButton();
            this.waitGif = new System.Windows.Forms.PictureBox();
            this.status.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgLightsOn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.slideSmooth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.waitGif)).BeginInit();
            this.SuspendLayout();
            // 
            // txtWellcome
            // 
            this.txtWellcome.BackColor = System.Drawing.Color.Transparent;
            this.txtWellcome.Cursor = System.Windows.Forms.Cursors.Arrow;
            resources.ApplyResources(this.txtWellcome, "txtWellcome");
            this.txtWellcome.ForeColor = System.Drawing.Color.Navy;
            this.txtWellcome.Name = "txtWellcome";
            // 
            // btnConnect
            // 
            resources.ApplyResources(this.btnConnect, "btnConnect");
            this.btnConnect.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.btnConnect.ForeColor = System.Drawing.Color.Black;
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.OnBtnClick);
            // 
            // status
            // 
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbStatus});
            resources.ApplyResources(this.status, "status");
            this.status.Name = "status";
            this.status.DoubleClick += new System.EventHandler(this.OnStatusDoubleClick);
            // 
            // lbStatus
            // 
            this.lbStatus.BackColor = System.Drawing.SystemColors.Control;
            this.lbStatus.Name = "lbStatus";
            resources.ApplyResources(this.lbStatus, "lbStatus");
            // 
            // connectionHeartbeat
            // 
            this.connectionHeartbeat.Interval = 10000;
            this.connectionHeartbeat.Tick += new System.EventHandler(this.OnConnectionHeartbeat);
            // 
            // lightTimer
            // 
            this.lightTimer.Interval = 10;
            this.lightTimer.Tick += new System.EventHandler(this.OnLightTick);
            // 
            // imgLightsOn
            // 
            this.imgLightsOn.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.imgLightsOn, "imgLightsOn");
            this.imgLightsOn.Name = "imgLightsOn";
            this.imgLightsOn.TabStop = false;
            // 
            // imgLogo
            // 
            this.imgLogo.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.imgLogo, "imgLogo");
            this.imgLogo.Name = "imgLogo";
            this.imgLogo.TabStop = false;
            // 
            // slideSmooth
            // 
            resources.ApplyResources(this.slideSmooth, "slideSmooth");
            this.slideSmooth.Maximum = 20;
            this.slideSmooth.Name = "slideSmooth";
            this.slideSmooth.Value = 5;
            this.slideSmooth.Scroll += new System.EventHandler(this.OnSmoothChangeValue);
            // 
            // lbSuavisar
            // 
            resources.ApplyResources(this.lbSuavisar, "lbSuavisar");
            this.lbSuavisar.BackColor = System.Drawing.Color.Transparent;
            this.lbSuavisar.Name = "lbSuavisar";
            // 
            // beepTimer
            // 
            this.beepTimer.Interval = 500;
            this.beepTimer.Tick += new System.EventHandler(this.OnBeepTick);
            // 
            // controlCursor
            // 
            resources.ApplyResources(this.controlCursor, "controlCursor");
            this.controlCursor.BackColor = System.Drawing.Color.Transparent;
            this.controlCursor.Name = "controlCursor";
            this.controlCursor.UseVisualStyleBackColor = false;
            this.controlCursor.CheckedChanged += new System.EventHandler(this.OnControlCursorValueChanged);
            // 
            // viewTest
            // 
            resources.ApplyResources(this.viewTest, "viewTest");
            this.viewTest.BackColor = System.Drawing.Color.Transparent;
            this.viewTest.Checked = true;
            this.viewTest.Name = "viewTest";
            this.viewTest.TabStop = true;
            this.viewTest.UseVisualStyleBackColor = false;
            this.viewTest.CheckedChanged += new System.EventHandler(this.OnViewTestValueChanged);
            // 
            // waitGif
            // 
            resources.ApplyResources(this.waitGif, "waitGif");
            this.waitGif.BackColor = System.Drawing.Color.Transparent;
            this.waitGif.Name = "waitGif";
            this.waitGif.TabStop = false;
            // 
            // FormFindAndConnect
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.status);
            this.Controls.Add(this.lbSuavisar);
            this.Controls.Add(this.controlCursor);
            this.Controls.Add(this.slideSmooth);
            this.Controls.Add(this.waitGif);
            this.Controls.Add(this.imgLightsOn);
            this.Controls.Add(this.txtWellcome);
            this.Controls.Add(this.viewTest);
            this.Controls.Add(this.imgLogo);
            this.Controls.Add(this.btnConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormFindAndConnect";
            this.Deactivate += new System.EventHandler(this.FormFindAndConnect_Deactivate);
            this.Shown += new System.EventHandler(this.FormFindAndConnect_Shown);
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgLightsOn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imgLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.slideSmooth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.waitGif)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox imgLogo;
        private System.Windows.Forms.Label txtWellcome;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.StatusStrip status;
        private System.Windows.Forms.ToolStripStatusLabel lbStatus;
        private System.Windows.Forms.Timer connectionHeartbeat;
        private System.Windows.Forms.PictureBox imgLightsOn;
        private System.Windows.Forms.Timer lightTimer;
        private System.Windows.Forms.TrackBar slideSmooth;
        private System.Windows.Forms.Label lbSuavisar;
        private System.Windows.Forms.Timer beepTimer;
        private System.Windows.Forms.RadioButton controlCursor;
        private System.Windows.Forms.RadioButton viewTest;
        private System.Windows.Forms.PictureBox waitGif;
    }
}