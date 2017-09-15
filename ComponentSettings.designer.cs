namespace LiveSplit.UI.Components
{
	partial class CrashNSTLoadRemovalSettings
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblVersion = new System.Windows.Forms.Label();
			this.processListComboBox = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.previewPictureBox = new System.Windows.Forms.PictureBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.croppedPreviewPictureBox = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.croppedPreviewPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// lblVersion
			// 
			this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVersion.AutoSize = true;
			this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblVersion.ForeColor = System.Drawing.SystemColors.ControlDark;
			this.lblVersion.Location = new System.Drawing.Point(432, 1);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(37, 13);
			this.lblVersion.TabIndex = 21;
			this.lblVersion.Text = "v0.0.0";
			this.lblVersion.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// processListComboBox
			// 
			this.processListComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.processListComboBox.FormattingEnabled = true;
			this.processListComboBox.Items.AddRange(new object[] {
            "Full Display (Primary)"});
			this.processListComboBox.Location = new System.Drawing.Point(71, 17);
			this.processListComboBox.Name = "processListComboBox";
			this.processListComboBox.Size = new System.Drawing.Size(395, 21);
			this.processListComboBox.TabIndex = 22;
			this.processListComboBox.DropDown += new System.EventHandler(this.processListComboBox_DropDown);
			this.processListComboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(7, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(47, 13);
			this.label1.TabIndex = 23;
			this.label1.Text = "Capture:";
			// 
			// previewPictureBox
			// 
			this.previewPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.previewPictureBox.Location = new System.Drawing.Point(35, 104);
			this.previewPictureBox.Name = "previewPictureBox";
			this.previewPictureBox.Size = new System.Drawing.Size(400, 180);
			this.previewPictureBox.TabIndex = 24;
			this.previewPictureBox.TabStop = false;
			this.previewPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.previewPictureBox_MouseClick);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(30, 51);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(173, 26);
			this.label2.TabIndex = 25;
			this.label2.Text = "Capture Preview";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(32, 77);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(349, 13);
			this.label11.TabIndex = 28;
			this.label11.Text = "Left Click sets top-left corner, right click sets bottom-right corner of region";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(30, 289);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(262, 26);
			this.label4.TabIndex = 30;
			this.label4.Text = "Cropped Capture Preview";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// croppedPreviewPictureBox
			// 
			this.croppedPreviewPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.croppedPreviewPictureBox.Location = new System.Drawing.Point(35, 318);
			this.croppedPreviewPictureBox.Name = "croppedPreviewPictureBox";
			this.croppedPreviewPictureBox.Size = new System.Drawing.Size(400, 180);
			this.croppedPreviewPictureBox.TabIndex = 29;
			this.croppedPreviewPictureBox.TabStop = false;
			// 
			// CrashNSTLoadRemovalSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label4);
			this.Controls.Add(this.croppedPreviewPictureBox);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.previewPictureBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.processListComboBox);
			this.Controls.Add(this.lblVersion);
			this.Name = "CrashNSTLoadRemovalSettings";
			this.Padding = new System.Windows.Forms.Padding(7);
			this.Size = new System.Drawing.Size(476, 510);
			((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.croppedPreviewPictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.ComboBox processListComboBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.PictureBox previewPictureBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.PictureBox croppedPreviewPictureBox;
	}
}
