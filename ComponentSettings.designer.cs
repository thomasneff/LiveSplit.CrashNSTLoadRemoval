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
			this.scalingLabel = new System.Windows.Forms.Label();
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.croppedPreviewPictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
			this.SuspendLayout();
			// 
			// lblVersion
			// 
			this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblVersion.AutoSize = true;
			this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblVersion.ForeColor = System.Drawing.SystemColors.ControlDark;
			this.lblVersion.Location = new System.Drawing.Point(588, 611);
			this.lblVersion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(47, 17);
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
			this.processListComboBox.Location = new System.Drawing.Point(83, 9);
			this.processListComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.processListComboBox.Name = "processListComboBox";
			this.processListComboBox.Size = new System.Drawing.Size(539, 24);
			this.processListComboBox.TabIndex = 22;
			this.processListComboBox.DropDown += new System.EventHandler(this.processListComboBox_DropDown);
			this.processListComboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 9);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(62, 17);
			this.label1.TabIndex = 23;
			this.label1.Text = "Capture:";
			// 
			// previewPictureBox
			// 
			this.previewPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.previewPictureBox.Location = new System.Drawing.Point(47, 128);
			this.previewPictureBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.previewPictureBox.Name = "previewPictureBox";
			this.previewPictureBox.Size = new System.Drawing.Size(533, 221);
			this.previewPictureBox.TabIndex = 24;
			this.previewPictureBox.TabStop = false;
			this.previewPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.previewPictureBox_MouseClick);
			this.previewPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.previewPictureBox_MouseDown);
			this.previewPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.previewPictureBox_MouseMove);
			this.previewPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.previewPictureBox_MouseUp);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(40, 48);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(215, 31);
			this.label2.TabIndex = 25;
			this.label2.Text = "Capture Preview";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(43, 104);
			this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(470, 17);
			this.label11.TabIndex = 28;
			this.label11.Text = "Left Click sets top-left corner, right click sets bottom-right corner of region";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(40, 356);
			this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(326, 31);
			this.label4.TabIndex = 30;
			this.label4.Text = "Cropped Capture Preview";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// croppedPreviewPictureBox
			// 
			this.croppedPreviewPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.croppedPreviewPictureBox.Location = new System.Drawing.Point(47, 391);
			this.croppedPreviewPictureBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.croppedPreviewPictureBox.Name = "croppedPreviewPictureBox";
			this.croppedPreviewPictureBox.Size = new System.Drawing.Size(533, 221);
			this.croppedPreviewPictureBox.TabIndex = 29;
			this.croppedPreviewPictureBox.TabStop = false;
			// 
			// scalingLabel
			// 
			this.scalingLabel.AutoSize = true;
			this.scalingLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.scalingLabel.Location = new System.Drawing.Point(470, 37);
			this.scalingLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.scalingLabel.Name = "scalingLabel";
			this.scalingLabel.Size = new System.Drawing.Size(152, 26);
			this.scalingLabel.TabIndex = 32;
			this.scalingLabel.Text = "Scaling: 100%";
			// 
			// trackBar1
			// 
			this.trackBar1.LargeChange = 25;
			this.trackBar1.Location = new System.Drawing.Point(518, 65);
			this.trackBar1.Maximum = 201;
			this.trackBar1.Minimum = 100;
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Size = new System.Drawing.Size(104, 56);
			this.trackBar1.SmallChange = 25;
			this.trackBar1.TabIndex = 31;
			this.trackBar1.TickFrequency = 25;
			this.trackBar1.Value = 100;
			this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
			// 
			// CrashNSTLoadRemovalSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.scalingLabel);
			this.Controls.Add(this.trackBar1);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.croppedPreviewPictureBox);
			this.Controls.Add(this.label11);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.previewPictureBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.processListComboBox);
			this.Controls.Add(this.lblVersion);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "CrashNSTLoadRemovalSettings";
			this.Padding = new System.Windows.Forms.Padding(9, 9, 9, 9);
			this.Size = new System.Drawing.Size(635, 628);
			((System.ComponentModel.ISupportInitialize)(this.previewPictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.croppedPreviewPictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
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
		private System.Windows.Forms.Label scalingLabel;
		private System.Windows.Forms.TrackBar trackBar1;
	}
}
