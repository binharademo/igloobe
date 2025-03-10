namespace Br.Com.IGloobe.Apps.Main.Gui
{
    partial class FormAppsPresentation {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAppsPresentation));
            this.BtnPlay = new System.Windows.Forms.Button();
            this.BtnRight = new System.Windows.Forms.Button();
            this.ShowImage = new System.Windows.Forms.PictureBox();
            this.BtnCloseWindow = new System.Windows.Forms.Button();
            this.BtnOpen = new System.Windows.Forms.Button();
            this.BtnRec = new System.Windows.Forms.Button();
            this.BtnLeft = new System.Windows.Forms.Button();
            this._fileDialog = new System.Windows.Forms.OpenFileDialog();
            this._checkPresentation = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ShowImage)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnPlay
            // 
            this.BtnPlay.CausesValidation = false;
            this.BtnPlay.Enabled = false;
            this.BtnPlay.FlatAppearance.BorderSize = 0;
            this.BtnPlay.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnPlay.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPlay.ForeColor = System.Drawing.Color.Transparent;
            this.BtnPlay.Image = ((System.Drawing.Image)(resources.GetObject("BtnPlay.Image")));
            this.BtnPlay.Location = new System.Drawing.Point(113, 18);
            this.BtnPlay.Margin = new System.Windows.Forms.Padding(0);
            this.BtnPlay.Name = "BtnPlay";
            this.BtnPlay.Size = new System.Drawing.Size(49, 49);
            this.BtnPlay.TabIndex = 1;
            this.BtnPlay.TabStop = false;
            this.BtnPlay.UseVisualStyleBackColor = true;
            // 
            // BtnRight
            // 
            this.BtnRight.CausesValidation = false;
            this.BtnRight.Enabled = false;
            this.BtnRight.FlatAppearance.BorderSize = 0;
            this.BtnRight.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnRight.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnRight.ForeColor = System.Drawing.Color.Transparent;
            this.BtnRight.Image = ((System.Drawing.Image)(resources.GetObject("BtnRight.Image")));
            this.BtnRight.Location = new System.Drawing.Point(131, 66);
            this.BtnRight.Margin = new System.Windows.Forms.Padding(0);
            this.BtnRight.Name = "BtnRight";
            this.BtnRight.Size = new System.Drawing.Size(49, 49);
            this.BtnRight.TabIndex = 2;
            this.BtnRight.TabStop = false;
            this.BtnRight.UseVisualStyleBackColor = true;
            this.BtnRight.Click += new System.EventHandler(this.BtnRight_Click);
            // 
            // ShowImage
            // 
            this.ShowImage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ShowImage.BackgroundImage")));
            this.ShowImage.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.ShowImage.ErrorImage = null;
            this.ShowImage.InitialImage = null;
            this.ShowImage.Location = new System.Drawing.Point(57, 57);
            this.ShowImage.Name = "ShowImage";
            this.ShowImage.Size = new System.Drawing.Size(64, 64);
            this.ShowImage.TabIndex = 4;
            this.ShowImage.TabStop = false;
            this.ShowImage.WaitOnLoad = true;
            // 
            // BtnCloseWindow
            // 
            this.BtnCloseWindow.BackColor = System.Drawing.Color.White;
            this.BtnCloseWindow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.BtnCloseWindow.CausesValidation = false;
            this.BtnCloseWindow.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.BtnCloseWindow.FlatAppearance.BorderSize = 0;
            this.BtnCloseWindow.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnCloseWindow.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnCloseWindow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCloseWindow.ForeColor = System.Drawing.Color.Transparent;
            this.BtnCloseWindow.Image = ((System.Drawing.Image)(resources.GetObject("BtnCloseWindow.Image")));
            this.BtnCloseWindow.Location = new System.Drawing.Point(103, 102);
            this.BtnCloseWindow.Margin = new System.Windows.Forms.Padding(0);
            this.BtnCloseWindow.Name = "BtnCloseWindow";
            this.BtnCloseWindow.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.BtnCloseWindow.Size = new System.Drawing.Size(29, 29);
            this.BtnCloseWindow.TabIndex = 5;
            this.BtnCloseWindow.TabStop = false;
            this.BtnCloseWindow.UseVisualStyleBackColor = false;
            this.BtnCloseWindow.Click += new System.EventHandler(this.BtnCloseWindow_Click);
            // 
            // BtnOpen
            // 
            this.BtnOpen.CausesValidation = false;
            this.BtnOpen.FlatAppearance.BorderSize = 0;
            this.BtnOpen.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnOpen.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnOpen.ForeColor = System.Drawing.Color.Transparent;
            this.BtnOpen.Image = ((System.Drawing.Image)(resources.GetObject("BtnOpen.Image")));
            this.BtnOpen.Location = new System.Drawing.Point(64, 0);
            this.BtnOpen.Margin = new System.Windows.Forms.Padding(0);
            this.BtnOpen.Name = "BtnOpen";
            this.BtnOpen.Size = new System.Drawing.Size(49, 49);
            this.BtnOpen.TabIndex = 3;
            this.BtnOpen.TabStop = false;
            this.BtnOpen.UseVisualStyleBackColor = true;
            this.BtnOpen.Click += new System.EventHandler(this.BtnOpen_Click);
            // 
            // BtnRec
            // 
            this.BtnRec.CausesValidation = false;
            this.BtnRec.Enabled = false;
            this.BtnRec.FlatAppearance.BorderSize = 0;
            this.BtnRec.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnRec.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnRec.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnRec.ForeColor = System.Drawing.Color.Transparent;
            this.BtnRec.Image = ((System.Drawing.Image)(resources.GetObject("BtnRec.Image")));
            this.BtnRec.Location = new System.Drawing.Point(15, 18);
            this.BtnRec.Margin = new System.Windows.Forms.Padding(0);
            this.BtnRec.Name = "BtnRec";
            this.BtnRec.Size = new System.Drawing.Size(49, 49);
            this.BtnRec.TabIndex = 6;
            this.BtnRec.TabStop = false;
            this.BtnRec.UseVisualStyleBackColor = true;
            // 
            // BtnLeft
            // 
            this.BtnLeft.CausesValidation = false;
            this.BtnLeft.Enabled = false;
            this.BtnLeft.FlatAppearance.BorderSize = 0;
            this.BtnLeft.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnLeft.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnLeft.ForeColor = System.Drawing.Color.Transparent;
            this.BtnLeft.Image = ((System.Drawing.Image)(resources.GetObject("BtnLeft.Image")));
            this.BtnLeft.Location = new System.Drawing.Point(0, 67);
            this.BtnLeft.Margin = new System.Windows.Forms.Padding(0);
            this.BtnLeft.Name = "BtnLeft";
            this.BtnLeft.Size = new System.Drawing.Size(49, 49);
            this.BtnLeft.TabIndex = 7;
            this.BtnLeft.TabStop = false;
            this.BtnLeft.UseVisualStyleBackColor = true;
            this.BtnLeft.Click += new System.EventHandler(this.BtnLeft_Click);
            // 
            // _fileDialog
            // 
            this._fileDialog.Filter = "ppt|*.ppt|pptx|*.pptx";
            // 
            // _checkPresentation
            // 
            this._checkPresentation.Interval = 2000;
            this._checkPresentation.Tick += new System.EventHandler(this.CheckPresentationTick);
            // 
            // FormAppsPresentation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(183, 133);
            this.Controls.Add(this.BtnLeft);
            this.Controls.Add(this.BtnRec);
            this.Controls.Add(this.BtnCloseWindow);
            this.Controls.Add(this.BtnOpen);
            this.Controls.Add(this.BtnRight);
            this.Controls.Add(this.BtnPlay);
            this.Controls.Add(this.ShowImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormAppsPresentation";
            this.Text = "FormAppsPresentation";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.ShowImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnPlay;
        private System.Windows.Forms.Button BtnRight;
        private System.Windows.Forms.PictureBox ShowImage;
        private System.Windows.Forms.Button BtnCloseWindow;
        private System.Windows.Forms.Button BtnOpen;
        private System.Windows.Forms.Button BtnRec;
        private System.Windows.Forms.Button BtnLeft;
        private System.Windows.Forms.OpenFileDialog _fileDialog;
        private System.Windows.Forms.Timer _checkPresentation;

    }
}

