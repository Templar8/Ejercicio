
namespace Scheduler.Windows
{
    partial class SchedulerForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DtpCurrentDate = new System.Windows.Forms.DateTimePicker();
            this.BttCalculateNextDate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ChkEnabled = new System.Windows.Forms.CheckBox();
            this.NUDRecurrency = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.CbxOccurs = new System.Windows.Forms.ComboBox();
            this.CbxType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.DtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.DtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.TxtDescription = new System.Windows.Forms.TextBox();
            this.TxtNextExecution = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.DtpConfigurationDate = new System.Windows.Forms.DateTimePicker();
            this.DtpConfigurationTime = new System.Windows.Forms.DateTimePicker();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUDRecurrency)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DtpCurrentDate);
            this.groupBox1.Controls.Add(this.BttCalculateNextDate);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(504, 55);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input";
            // 
            // DtpCurrentDate
            // 
            this.DtpCurrentDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpCurrentDate.Location = new System.Drawing.Point(148, 19);
            this.DtpCurrentDate.Name = "DtpCurrentDate";
            this.DtpCurrentDate.Size = new System.Drawing.Size(106, 23);
            this.DtpCurrentDate.TabIndex = 4;
            // 
            // BttCalculateNextDate
            // 
            this.BttCalculateNextDate.Location = new System.Drawing.Point(274, 19);
            this.BttCalculateNextDate.Name = "BttCalculateNextDate";
            this.BttCalculateNextDate.Size = new System.Drawing.Size(214, 23);
            this.BttCalculateNextDate.TabIndex = 2;
            this.BttCalculateNextDate.Text = "Calculate Next Date";
            this.BttCalculateNextDate.UseVisualStyleBackColor = true;
            this.BttCalculateNextDate.Click += new System.EventHandler(this.BttCalculateNextDate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Current date";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.DtpConfigurationTime);
            this.groupBox2.Controls.Add(this.DtpConfigurationDate);
            this.groupBox2.Controls.Add(this.ChkEnabled);
            this.groupBox2.Controls.Add(this.NUDRecurrency);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.CbxOccurs);
            this.groupBox2.Controls.Add(this.CbxType);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(13, 95);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(504, 113);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Configuration";
            // 
            // ChkEnabled
            // 
            this.ChkEnabled.AutoSize = true;
            this.ChkEnabled.Location = new System.Drawing.Point(276, 23);
            this.ChkEnabled.Name = "ChkEnabled";
            this.ChkEnabled.Size = new System.Drawing.Size(68, 19);
            this.ChkEnabled.TabIndex = 7;
            this.ChkEnabled.Text = "Enabled";
            this.ChkEnabled.UseVisualStyleBackColor = true;
            // 
            // NUDRecurrency
            // 
            this.NUDRecurrency.Enabled = false;
            this.NUDRecurrency.Location = new System.Drawing.Point(388, 76);
            this.NUDRecurrency.Name = "NUDRecurrency";
            this.NUDRecurrency.Size = new System.Drawing.Size(37, 23);
            this.NUDRecurrency.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(275, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 15);
            this.label5.TabIndex = 5;
            this.label5.Text = "Every";
            // 
            // CbxOccurs
            // 
            this.CbxOccurs.Enabled = false;
            this.CbxOccurs.FormattingEnabled = true;
            this.CbxOccurs.Items.AddRange(new object[] {
            "Daily",
            "Monthly",
            "Yearly"});
            this.CbxOccurs.Location = new System.Drawing.Point(148, 76);
            this.CbxOccurs.Name = "CbxOccurs";
            this.CbxOccurs.Size = new System.Drawing.Size(121, 23);
            this.CbxOccurs.TabIndex = 4;
            // 
            // CbxType
            // 
            this.CbxType.FormattingEnabled = true;
            this.CbxType.Items.AddRange(new object[] {
            "Once",
            "Recurring"});
            this.CbxType.Location = new System.Drawing.Point(148, 19);
            this.CbxType.Name = "CbxType";
            this.CbxType.Size = new System.Drawing.Size(121, 23);
            this.CbxType.TabIndex = 3;
            this.CbxType.SelectedIndexChanged += new System.EventHandler(this.CbxType_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 15);
            this.label4.TabIndex = 2;
            this.label4.Text = "Occurs";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 15);
            this.label3.TabIndex = 1;
            this.label3.Text = "DateTime";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Type";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.DtpEndDate);
            this.groupBox3.Controls.Add(this.DtpStartDate);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Location = new System.Drawing.Point(12, 232);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(505, 56);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Limits";
            // 
            // DtpEndDate
            // 
            this.DtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpEndDate.Location = new System.Drawing.Point(389, 22);
            this.DtpEndDate.Name = "DtpEndDate";
            this.DtpEndDate.Size = new System.Drawing.Size(100, 23);
            this.DtpEndDate.TabIndex = 4;
            // 
            // DtpStartDate
            // 
            this.DtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpStartDate.Location = new System.Drawing.Point(149, 23);
            this.DtpStartDate.Name = "DtpStartDate";
            this.DtpStartDate.Size = new System.Drawing.Size(106, 23);
            this.DtpStartDate.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(255, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 15);
            this.label7.TabIndex = 2;
            this.label7.Text = "End date";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 15);
            this.label6.TabIndex = 0;
            this.label6.Text = "Start date";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.TxtDescription);
            this.groupBox4.Controls.Add(this.TxtNextExecution);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Location = new System.Drawing.Point(13, 310);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(504, 130);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Output";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 58);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 15);
            this.label9.TabIndex = 3;
            this.label9.Text = "Description";
            // 
            // TxtDescription
            // 
            this.TxtDescription.Enabled = false;
            this.TxtDescription.Location = new System.Drawing.Point(7, 79);
            this.TxtDescription.Multiline = true;
            this.TxtDescription.Name = "TxtDescription";
            this.TxtDescription.Size = new System.Drawing.Size(481, 45);
            this.TxtDescription.TabIndex = 2;
            // 
            // TxtNextExecution
            // 
            this.TxtNextExecution.Enabled = false;
            this.TxtNextExecution.Location = new System.Drawing.Point(148, 26);
            this.TxtNextExecution.Name = "TxtNextExecution";
            this.TxtNextExecution.Size = new System.Drawing.Size(340, 23);
            this.TxtNextExecution.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 29);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(114, 15);
            this.label8.TabIndex = 0;
            this.label8.Text = "Next execution time";
            // 
            // DtpConfigurationDate
            // 
            this.DtpConfigurationDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DtpConfigurationDate.Location = new System.Drawing.Point(148, 49);
            this.DtpConfigurationDate.Name = "DtpConfigurationDate";
            this.DtpConfigurationDate.Size = new System.Drawing.Size(121, 23);
            this.DtpConfigurationDate.TabIndex = 8;
            // 
            // DtpConfigurationTime
            // 
            this.DtpConfigurationTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.DtpConfigurationTime.Location = new System.Drawing.Point(276, 48);
            this.DtpConfigurationTime.Name = "DtpConfigurationTime";
            this.DtpConfigurationTime.Size = new System.Drawing.Size(112, 23);
            this.DtpConfigurationTime.TabIndex = 9;
            // 
            // SchedulerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 475);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SchedulerForm";
            this.Text = "Scheduler";
            this.Load += new System.EventHandler(this.SchedulerForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUDRecurrency)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button BttCalculateNextDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox ChkEnabled;
        private System.Windows.Forms.NumericUpDown NUDRecurrency;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox CbxOccurs;
        private System.Windows.Forms.ComboBox CbxType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox TxtDescription;
        private System.Windows.Forms.TextBox TxtNextExecution;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker DtpCurrentDate;
        private System.Windows.Forms.DateTimePicker DtpEndDate;
        private System.Windows.Forms.DateTimePicker DtpStartDate;
        private System.Windows.Forms.DateTimePicker DtpConfigurationTime;
        private System.Windows.Forms.DateTimePicker DtpConfigurationDate;
    }
}

