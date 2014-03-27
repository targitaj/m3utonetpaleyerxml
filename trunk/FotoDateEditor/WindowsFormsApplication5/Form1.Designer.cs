namespace WindowsFormsApplication5
{
    partial class Form1
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
            this.btnSplit = new System.Windows.Forms.Button();
            this.lblDir1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbxOffset1 = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.nudMinutes1 = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nudHours1 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nudDay1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nudMonth1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.nudYaer1 = new System.Windows.Forms.NumericUpDown();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cbxOffset2 = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.nudMinutes2 = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.nudHours2 = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.nudDay2 = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.nudMonth2 = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.nudYaer2 = new System.Windows.Forms.NumericUpDown();
            this.button4 = new System.Windows.Forms.Button();
            this.lblDir2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.lblDestDir = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblProgress = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinutes1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHours1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDay1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMonth1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYaer1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinutes2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHours2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDay2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMonth2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYaer2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSplit
            // 
            this.btnSplit.Enabled = false;
            this.btnSplit.Location = new System.Drawing.Point(12, 260);
            this.btnSplit.Name = "btnSplit";
            this.btnSplit.Size = new System.Drawing.Size(170, 23);
            this.btnSplit.TabIndex = 0;
            this.btnSplit.Text = "Соединить и отсортировать";
            this.btnSplit.UseVisualStyleBackColor = true;
            this.btnSplit.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblDir1
            // 
            this.lblDir1.AutoSize = true;
            this.lblDir1.Location = new System.Drawing.Point(144, 12);
            this.lblDir1.Name = "lblDir1";
            this.lblDir1.Size = new System.Drawing.Size(0, 13);
            this.lblDir1.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(3, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(135, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Первая директория";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbxOffset1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.nudMinutes1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.nudHours1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.nudDay1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.nudMonth1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.nudYaer1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.lblDir1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(713, 92);
            this.panel1.TabIndex = 5;
            // 
            // cbxOffset1
            // 
            this.cbxOffset1.AutoSize = true;
            this.cbxOffset1.Location = new System.Drawing.Point(375, 54);
            this.cbxOffset1.Name = "cbxOffset1";
            this.cbxOffset1.Size = new System.Drawing.Size(147, 17);
            this.cbxOffset1.TabIndex = 14;
            this.cbxOffset1.Text = "Сдвиг по дате, времени";
            this.cbxOffset1.UseVisualStyleBackColor = true;
            this.cbxOffset1.CheckedChanged += new System.EventHandler(this.cbxOffset1_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(297, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Минуты";
            // 
            // nudMinutes1
            // 
            this.nudMinutes1.Enabled = false;
            this.nudMinutes1.Location = new System.Drawing.Point(298, 54);
            this.nudMinutes1.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.nudMinutes1.Minimum = new decimal(new int[] {
            59,
            0,
            0,
            -2147483648});
            this.nudMinutes1.Name = "nudMinutes1";
            this.nudMinutes1.Size = new System.Drawing.Size(54, 20);
            this.nudMinutes1.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(237, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Часы";
            // 
            // nudHours1
            // 
            this.nudHours1.Enabled = false;
            this.nudHours1.Location = new System.Drawing.Point(238, 54);
            this.nudHours1.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.nudHours1.Minimum = new decimal(new int[] {
            23,
            0,
            0,
            -2147483648});
            this.nudHours1.Name = "nudHours1";
            this.nudHours1.Size = new System.Drawing.Size(54, 20);
            this.nudHours1.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(161, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "День";
            // 
            // nudDay1
            // 
            this.nudDay1.Enabled = false;
            this.nudDay1.Location = new System.Drawing.Point(164, 54);
            this.nudDay1.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.nudDay1.Minimum = new decimal(new int[] {
            31,
            0,
            0,
            -2147483648});
            this.nudDay1.Name = "nudDay1";
            this.nudDay1.Size = new System.Drawing.Size(54, 20);
            this.nudDay1.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(101, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Месяц";
            // 
            // nudMonth1
            // 
            this.nudMonth1.Enabled = false;
            this.nudMonth1.Location = new System.Drawing.Point(102, 54);
            this.nudMonth1.Maximum = new decimal(new int[] {
            11,
            0,
            0,
            0});
            this.nudMonth1.Minimum = new decimal(new int[] {
            11,
            0,
            0,
            -2147483648});
            this.nudMonth1.Name = "nudMonth1";
            this.nudMonth1.Size = new System.Drawing.Size(54, 20);
            this.nudMonth1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Год";
            // 
            // nudYaer1
            // 
            this.nudYaer1.Enabled = false;
            this.nudYaer1.Location = new System.Drawing.Point(42, 54);
            this.nudYaer1.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.nudYaer1.Name = "nudYaer1";
            this.nudYaer1.Size = new System.Drawing.Size(54, 20);
            this.nudYaer1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cbxOffset2);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.nudMinutes2);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.nudHours2);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.nudDay2);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.nudMonth2);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.nudYaer2);
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.lblDir2);
            this.panel2.Location = new System.Drawing.Point(12, 110);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(713, 102);
            this.panel2.TabIndex = 6;
            // 
            // cbxOffset2
            // 
            this.cbxOffset2.AutoSize = true;
            this.cbxOffset2.Location = new System.Drawing.Point(375, 67);
            this.cbxOffset2.Name = "cbxOffset2";
            this.cbxOffset2.Size = new System.Drawing.Size(147, 17);
            this.cbxOffset2.TabIndex = 23;
            this.cbxOffset2.Text = "Сдвиг по дате, времени";
            this.cbxOffset2.UseVisualStyleBackColor = true;
            this.cbxOffset2.CheckedChanged += new System.EventHandler(this.cbxOffset2_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(297, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Минуты";
            // 
            // nudMinutes2
            // 
            this.nudMinutes2.Enabled = false;
            this.nudMinutes2.Location = new System.Drawing.Point(298, 67);
            this.nudMinutes2.Name = "nudMinutes2";
            this.nudMinutes2.Size = new System.Drawing.Size(54, 20);
            this.nudMinutes2.TabIndex = 21;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(237, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "Часы";
            // 
            // nudHours2
            // 
            this.nudHours2.Enabled = false;
            this.nudHours2.Location = new System.Drawing.Point(238, 67);
            this.nudHours2.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.nudHours2.Minimum = new decimal(new int[] {
            23,
            0,
            0,
            -2147483648});
            this.nudHours2.Name = "nudHours2";
            this.nudHours2.Size = new System.Drawing.Size(54, 20);
            this.nudHours2.TabIndex = 19;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(161, 45);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(34, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "День";
            // 
            // nudDay2
            // 
            this.nudDay2.Enabled = false;
            this.nudDay2.Location = new System.Drawing.Point(164, 67);
            this.nudDay2.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.nudDay2.Minimum = new decimal(new int[] {
            31,
            0,
            0,
            -2147483648});
            this.nudDay2.Name = "nudDay2";
            this.nudDay2.Size = new System.Drawing.Size(54, 20);
            this.nudDay2.TabIndex = 8;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(101, 45);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Месяц";
            // 
            // nudMonth2
            // 
            this.nudMonth2.Enabled = false;
            this.nudMonth2.Location = new System.Drawing.Point(102, 67);
            this.nudMonth2.Maximum = new decimal(new int[] {
            11,
            0,
            0,
            0});
            this.nudMonth2.Minimum = new decimal(new int[] {
            11,
            0,
            0,
            -2147483648});
            this.nudMonth2.Name = "nudMonth2";
            this.nudMonth2.Size = new System.Drawing.Size(54, 20);
            this.nudMonth2.TabIndex = 16;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(41, 45);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(25, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Год";
            // 
            // nudYaer2
            // 
            this.nudYaer2.Enabled = false;
            this.nudYaer2.Location = new System.Drawing.Point(42, 67);
            this.nudYaer2.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.nudYaer2.Name = "nudYaer2";
            this.nudYaer2.Size = new System.Drawing.Size(54, 20);
            this.nudYaer2.TabIndex = 14;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(3, 6);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(135, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "Вторая директория";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // lblDir2
            // 
            this.lblDir2.AutoSize = true;
            this.lblDir2.Location = new System.Drawing.Point(144, 12);
            this.lblDir2.Name = "lblDir2";
            this.lblDir2.Size = new System.Drawing.Size(0, 13);
            this.lblDir2.TabIndex = 0;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 218);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(157, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "Директория назначения";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // lblDestDir
            // 
            this.lblDestDir.AutoSize = true;
            this.lblDestDir.Location = new System.Drawing.Point(176, 224);
            this.lblDestDir.Name = "lblDestDir";
            this.lblDestDir.Size = new System.Drawing.Size(0, 13);
            this.lblDestDir.TabIndex = 7;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(15, 344);
            this.progressBar1.MarqueeAnimationSpeed = 1000;
            this.progressBar1.Maximum = 1000;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(710, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 9;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(12, 325);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(24, 13);
            this.lblProgress.TabIndex = 10;
            this.lblProgress.Text = "0/0";
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Location = new System.Drawing.Point(23, 299);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(0, 13);
            this.lblError.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 375);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.lblDestDir);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnSplit);
            this.Name = "Form1";
            this.Text = "Соединение папок с картинками";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinutes1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHours1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDay1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMonth1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYaer1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinutes2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHours2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDay2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMonth2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYaer2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSplit;
        private System.Windows.Forms.Label lblDir1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label lblDir2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label lblDestDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudMonth1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudYaer1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudMinutes1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudHours1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudDay1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudMinutes2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudHours2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nudDay2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nudMonth2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown nudYaer2;
        private System.Windows.Forms.CheckBox cbxOffset1;
        private System.Windows.Forms.CheckBox cbxOffset2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.Label lblError;
    }
}

