namespace Gravite
{
    partial class OptionsScreen
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
            this.portsBox = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.statusLbl = new System.Windows.Forms.Label();
            this.disconnectBtn = new System.Windows.Forms.Button();
            this.connectBtn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.fullscreenChk = new System.Windows.Forms.CheckBox();
            this.sizeCombobox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.runBtn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // portsBox
            // 
            this.portsBox.FormattingEnabled = true;
            this.portsBox.Location = new System.Drawing.Point(6, 19);
            this.portsBox.Name = "portsBox";
            this.portsBox.Size = new System.Drawing.Size(218, 82);
            this.portsBox.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.statusLbl);
            this.groupBox1.Controls.Add(this.disconnectBtn);
            this.groupBox1.Controls.Add(this.connectBtn);
            this.groupBox1.Controls.Add(this.portsBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(230, 180);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CPController Settings";
            // 
            // statusLbl
            // 
            this.statusLbl.AutoSize = true;
            this.statusLbl.Location = new System.Drawing.Point(6, 160);
            this.statusLbl.Name = "statusLbl";
            this.statusLbl.Size = new System.Drawing.Size(43, 13);
            this.statusLbl.TabIndex = 3;
            this.statusLbl.Text = "Waiting";
            // 
            // disconnectBtn
            // 
            this.disconnectBtn.Location = new System.Drawing.Point(67, 108);
            this.disconnectBtn.Name = "disconnectBtn";
            this.disconnectBtn.Size = new System.Drawing.Size(77, 24);
            this.disconnectBtn.TabIndex = 2;
            this.disconnectBtn.Text = "Disconnect";
            this.disconnectBtn.UseVisualStyleBackColor = true;
            // 
            // connectBtn
            // 
            this.connectBtn.Location = new System.Drawing.Point(150, 107);
            this.connectBtn.Name = "connectBtn";
            this.connectBtn.Size = new System.Drawing.Size(74, 25);
            this.connectBtn.TabIndex = 1;
            this.connectBtn.Text = "Connect";
            this.connectBtn.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.fullscreenChk);
            this.groupBox2.Controls.Add(this.sizeCombobox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(255, 11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(229, 142);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Display Settings";
            // 
            // fullscreenChk
            // 
            this.fullscreenChk.AutoSize = true;
            this.fullscreenChk.Checked = true;
            this.fullscreenChk.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fullscreenChk.Location = new System.Drawing.Point(9, 69);
            this.fullscreenChk.Name = "fullscreenChk";
            this.fullscreenChk.Size = new System.Drawing.Size(74, 17);
            this.fullscreenChk.TabIndex = 3;
            this.fullscreenChk.Text = "Fullscreen";
            this.fullscreenChk.UseVisualStyleBackColor = true;
            // 
            // sizeCombobox
            // 
            this.sizeCombobox.FormattingEnabled = true;
            this.sizeCombobox.Location = new System.Drawing.Point(9, 42);
            this.sizeCombobox.Name = "sizeCombobox";
            this.sizeCombobox.Size = new System.Drawing.Size(214, 21);
            this.sizeCombobox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Screen Size";
            // 
            // runBtn
            // 
            this.runBtn.Location = new System.Drawing.Point(255, 159);
            this.runBtn.Name = "runBtn";
            this.runBtn.Size = new System.Drawing.Size(229, 38);
            this.runBtn.TabIndex = 3;
            this.runBtn.Text = "Run Game";
            this.runBtn.UseVisualStyleBackColor = true;
            // 
            // OptionsScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 204);
            this.Controls.Add(this.runBtn);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "OptionsScreen";
            this.Text = "Cyborg Physicals | Options";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListBox portsBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button disconnectBtn;
        private System.Windows.Forms.Button connectBtn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox fullscreenChk;
        private System.Windows.Forms.ComboBox sizeCombobox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button runBtn;
        private System.Windows.Forms.Label statusLbl;
    }
}