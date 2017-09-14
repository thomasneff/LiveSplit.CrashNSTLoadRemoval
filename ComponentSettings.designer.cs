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
			this.chkAutoReset = new System.Windows.Forms.CheckBox();
			this.lblVersion = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
			this.SuspendLayout();
			// 
			// chkAutoReset
			// 
			this.chkAutoReset.AutoSize = true;
			this.chkAutoReset.Location = new System.Drawing.Point(16, 490);
			this.chkAutoReset.Name = "chkAutoReset";
			this.chkAutoReset.Size = new System.Drawing.Size(379, 17);
			this.chkAutoReset.TabIndex = 16;
			this.chkAutoReset.TabStop = false;
			this.chkAutoReset.Text = "Automatically reset timer after hard resetting the emulator  without a savefile";
			this.chkAutoReset.UseVisualStyleBackColor = true;
			this.chkAutoReset.CheckedChanged += new System.EventHandler(this.checkAutoReset_CheckedChanged);
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
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Items.AddRange(new object[] {
			"Any% No Save Corruption",
			"Reverse Badge Acquisition",
			"Catch \'em All",
			"Any% Glitchless"});
			this.comboBox1.Location = new System.Drawing.Point(71, 17);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(395, 21);
			this.comboBox1.TabIndex = 22;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(52, 13);
			this.label1.TabIndex = 23;
			this.label1.Text = "Category:";
			// 
			// checkedListBox1
			// 
			this.checkedListBox1.CheckOnClick = true;
			this.checkedListBox1.FormattingEnabled = true;
			this.checkedListBox1.Location = new System.Drawing.Point(10, 45);
			this.checkedListBox1.Name = "checkedListBox1";
			this.checkedListBox1.Size = new System.Drawing.Size(456, 439);
			this.checkedListBox1.TabIndex = 24;
			this.checkedListBox1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox1_ItemCheck);
			// 
			// PokemonRBSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.chkAutoReset);
			this.Controls.Add(this.checkedListBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.lblVersion);
			this.Name = "PokemonRBSettings";
			this.Padding = new System.Windows.Forms.Padding(7);
			this.Size = new System.Drawing.Size(476, 510);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.CheckBox chkAutoReset;
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckedListBox checkedListBox1;
	}
}
