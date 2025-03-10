namespace Br.Com.IGloobe.Apps.Main.Gui
{
    partial class FormAppsMain
    {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAppsMain));
            this.IgloobeImage = new System.Windows.Forms.PictureBox();
            this.BtnCloseWindow = new System.Windows.Forms.Button();
            this.BtnPaint = new System.Windows.Forms.Button();
            this.BtnShow = new System.Windows.Forms.Button();
            this.BtnMouse = new System.Windows.Forms.Button();
            this.BtnCalibrate = new System.Windows.Forms.Button();
            this.BtnKeyboard = new System.Windows.Forms.Button();
            this.IsReadyToUse = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.IgloobeImage)).BeginInit();
            this.SuspendLayout();
            // 
            // IgloobeImage
            // 
            this.IgloobeImage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("IgloobeImage.BackgroundImage")));
            this.IgloobeImage.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.IgloobeImage.ErrorImage = null;
            this.IgloobeImage.InitialImage = null;
            this.IgloobeImage.Location = new System.Drawing.Point(57, 57);
            this.IgloobeImage.Name = "IgloobeImage";
            this.IgloobeImage.Size = new System.Drawing.Size(64, 64);
            this.IgloobeImage.TabIndex = 4;
            this.IgloobeImage.TabStop = false;
            this.IgloobeImage.WaitOnLoad = true;
            // 
            // BtnCloseWindow
            // 
            this.BtnCloseWindow.BackColor = System.Drawing.Color.Transparent;
            this.BtnCloseWindow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnCloseWindow.CausesValidation = false;
            this.BtnCloseWindow.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.BtnCloseWindow.FlatAppearance.BorderSize = 0;
            this.BtnCloseWindow.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            this.BtnCloseWindow.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            this.BtnCloseWindow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCloseWindow.ForeColor = System.Drawing.Color.White;
            this.BtnCloseWindow.Image = ((System.Drawing.Image)(resources.GetObject("BtnCloseWindow.Image")));
            this.BtnCloseWindow.Location = new System.Drawing.Point(103, 102);
            this.BtnCloseWindow.Margin = new System.Windows.Forms.Padding(0);
            this.BtnCloseWindow.Name = "BtnCloseWindow";
            this.BtnCloseWindow.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.BtnCloseWindow.Size = new System.Drawing.Size(29, 29);
            this.BtnCloseWindow.TabIndex = 5;
            this.BtnCloseWindow.TabStop = false;
            this.BtnCloseWindow.UseVisualStyleBackColor = true;
            this.BtnCloseWindow.Click += new System.EventHandler(this.BtnCloseWindow_Click);
            // 
            // BtnPaint
            // 
            this.BtnPaint.CausesValidation = false;
            this.BtnPaint.Enabled = false;
            this.BtnPaint.FlatAppearance.BorderSize = 0;
            this.BtnPaint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnPaint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnPaint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPaint.ForeColor = System.Drawing.Color.Transparent;
            this.BtnPaint.Image = ((System.Drawing.Image)(resources.GetObject("BtnPaint.Image")));
            this.BtnPaint.Location = new System.Drawing.Point(64, 1);
            this.BtnPaint.Margin = new System.Windows.Forms.Padding(0);
            this.BtnPaint.Name = "BtnPaint";
            this.BtnPaint.Size = new System.Drawing.Size(50, 49);
            this.BtnPaint.TabIndex = 3;
            this.BtnPaint.TabStop = false;
            this.BtnPaint.UseVisualStyleBackColor = true;
            this.BtnPaint.Click += new System.EventHandler(this.BtnPaintClick);
            // 
            // BtnShow
            // 
            this.BtnShow.CausesValidation = false;
            this.BtnShow.Enabled = false;
            this.BtnShow.FlatAppearance.BorderSize = 0;
            this.BtnShow.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnShow.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnShow.ForeColor = System.Drawing.Color.Transparent;
            this.BtnShow.Image = ((System.Drawing.Image)(resources.GetObject("BtnShow.Image")));
            this.BtnShow.Location = new System.Drawing.Point(15, 18);
            this.BtnShow.Margin = new System.Windows.Forms.Padding(0);
            this.BtnShow.Name = "BtnShow";
            this.BtnShow.Size = new System.Drawing.Size(49, 49);
            this.BtnShow.TabIndex = 1;
            this.BtnShow.TabStop = false;
            this.BtnShow.UseVisualStyleBackColor = true;
            this.BtnShow.Click += new System.EventHandler(this.BtnShowClick);
            // 
            // BtnMouse
            // 
            this.BtnMouse.CausesValidation = false;
            this.BtnMouse.Enabled = false;
            this.BtnMouse.FlatAppearance.BorderSize = 0;
            this.BtnMouse.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnMouse.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnMouse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnMouse.ForeColor = System.Drawing.Color.Transparent;
            this.BtnMouse.Image = ((System.Drawing.Image)(resources.GetObject("BtnMouse.Image")));
            this.BtnMouse.Location = new System.Drawing.Point(0, 67);
            this.BtnMouse.Margin = new System.Windows.Forms.Padding(0);
            this.BtnMouse.Name = "BtnMouse";
            this.BtnMouse.Size = new System.Drawing.Size(49, 49);
            this.BtnMouse.TabIndex = 6;
            this.BtnMouse.TabStop = false;
            this.BtnMouse.UseVisualStyleBackColor = true;
            this.BtnMouse.Click += new System.EventHandler(this.BtnMouseClick);
            // 
            // BtnCalibrate
            // 
            this.BtnCalibrate.CausesValidation = false;
            this.BtnCalibrate.FlatAppearance.BorderSize = 0;
            this.BtnCalibrate.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnCalibrate.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnCalibrate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCalibrate.ForeColor = System.Drawing.Color.Transparent;
            this.BtnCalibrate.Image = ((System.Drawing.Image)(resources.GetObject("BtnCalibrate.Image")));
            this.BtnCalibrate.Location = new System.Drawing.Point(113, 18);
            this.BtnCalibrate.Margin = new System.Windows.Forms.Padding(0);
            this.BtnCalibrate.Name = "BtnCalibrate";
            this.BtnCalibrate.Size = new System.Drawing.Size(49, 49);
            this.BtnCalibrate.TabIndex = 2;
            this.BtnCalibrate.TabStop = false;
            this.BtnCalibrate.UseVisualStyleBackColor = true;
            this.BtnCalibrate.Click += new System.EventHandler(this.BtnCalibrateClick);
            // 
            // BtnKeyboard
            // 
            this.BtnKeyboard.CausesValidation = false;
            this.BtnKeyboard.FlatAppearance.BorderSize = 0;
            this.BtnKeyboard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BtnKeyboard.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BtnKeyboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnKeyboard.ForeColor = System.Drawing.Color.Transparent;
            this.BtnKeyboard.Image = ((System.Drawing.Image)(resources.GetObject("BtnKeyboard.Image")));
            this.BtnKeyboard.Location = new System.Drawing.Point(131, 67);
            this.BtnKeyboard.Margin = new System.Windows.Forms.Padding(0);
            this.BtnKeyboard.Name = "BtnKeyboard";
            this.BtnKeyboard.Size = new System.Drawing.Size(49, 49);
            this.BtnKeyboard.TabIndex = 7;
            this.BtnKeyboard.TabStop = false;
            this.BtnKeyboard.UseVisualStyleBackColor = true;
            this.BtnKeyboard.Visible = false;
            this.BtnKeyboard.Click += new System.EventHandler(this.BtnKeyboardClick);
            // 
            // IsReadyToUse
            // 
            this.IsReadyToUse.Interval = 1000;
            this.IsReadyToUse.Tick += new System.EventHandler(this.IsReadyToUseTick);
            // 
            // FormAppsMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(164, 133);
            this.Controls.Add(this.BtnKeyboard);
            this.Controls.Add(this.BtnShow);
            this.Controls.Add(this.BtnCloseWindow);
            this.Controls.Add(this.IgloobeImage);
            this.Controls.Add(this.BtnMouse);
            this.Controls.Add(this.BtnPaint);
            this.Controls.Add(this.BtnCalibrate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormAppsMain";
            this.Text = "iGloobe";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.IgloobeImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox IgloobeImage;
        private System.Windows.Forms.Button BtnCloseWindow;
        private System.Windows.Forms.Button BtnPaint;
        private System.Windows.Forms.Button BtnShow;
        private System.Windows.Forms.Button BtnMouse;
        private System.Windows.Forms.Button BtnCalibrate;
        private System.Windows.Forms.Button BtnKeyboard;
        private System.Windows.Forms.Timer IsReadyToUse;

    }
}

