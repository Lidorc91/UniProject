namespace OxyDUI
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title7 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.Title title8 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.Title title9 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series9 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series10 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series11 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series12 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title10 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.Title title11 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.Title title12 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Dashboard = new System.Windows.Forms.TabPage();
            this.Developer = new System.Windows.Forms.TabPage();
            this.Play = new System.Windows.Forms.Button();
            this.Test = new System.Windows.Forms.Button();
            this.Record = new System.Windows.Forms.Button();
            this.Save_Recording = new System.Windows.Forms.Button();
            this.chart2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.Current = new System.Windows.Forms.Label();
            this.Recording_time = new System.Windows.Forms.Label();
            this.Sensor_version = new System.Windows.Forms.Label();
            this.Current_box = new System.Windows.Forms.TextBox();
            this.Recording_time_box = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.LED_color_box = new System.Windows.Forms.GroupBox();
            this.LED_Red_select = new System.Windows.Forms.RadioButton();
            this.LED_Green_select = new System.Windows.Forms.RadioButton();
            this.RTIA = new System.Windows.Forms.ComboBox();
            this.Rint = new System.Windows.Forms.ComboBox();
            this.RInt_labal = new System.Windows.Forms.Label();
            this.RTIA_labal = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.Dashboard.SuspendLayout();
            this.Developer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).BeginInit();
            this.LED_color_box.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea3.BackSecondaryColor = System.Drawing.Color.Cyan;
            chartArea3.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea3);
            this.chart1.Location = new System.Drawing.Point(266, 112);
            this.chart1.Name = "chart1";
            series7.ChartArea = "ChartArea1";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series7.Name = "PPG";
            this.chart1.Series.Add(series7);
            this.chart1.Size = new System.Drawing.Size(669, 536);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            title7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            title7.Name = "PPG";
            title7.Text = "PPG";
            title8.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
            title8.Name = "Left_Side";
            title8.Text = "Absorption Coefficient [1/m]\\";
            title9.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            title9.Name = "Bottem";
            title9.Text = "Time [Sec]";
            this.chart1.Titles.Add(title7);
            this.chart1.Titles.Add(title8);
            this.chart1.Titles.Add(title9);
            this.chart1.Click += new System.EventHandler(this.chart1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(245, 182);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(51, 385);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "HR [bpm]";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(51, 412);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "RR [bpm]";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(51, 438);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "HRV [ms]";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(51, 464);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "SpO2 [%]";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(51, 490);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "BP [mmHg]";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(118, 385);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(13, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "0";
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(118, 412);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(13, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "0";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(118, 438);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(13, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(118, 464);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(13, 13);
            this.label11.TabIndex = 23;
            this.label11.Text = "0";
            this.label11.Click += new System.EventHandler(this.label11_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(118, 490);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(13, 13);
            this.label12.TabIndex = 24;
            this.label12.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 55F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label6.ForeColor = System.Drawing.Color.DarkBlue;
            this.label6.Location = new System.Drawing.Point(251, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(222, 85);
            this.label6.TabIndex = 25;
            this.label6.Text = "OxyD";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(54, 345);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(77, 23);
            this.button1.TabIndex = 26;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Dashboard);
            this.tabControl1.Controls.Add(this.Developer);
            this.tabControl1.Location = new System.Drawing.Point(-4, -1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(969, 742);
            this.tabControl1.TabIndex = 27;
            // 
            // Dashboard
            // 
            this.Dashboard.Controls.Add(this.pictureBox1);
            this.Dashboard.Controls.Add(this.chart1);
            this.Dashboard.Controls.Add(this.button1);
            this.Dashboard.Controls.Add(this.label12);
            this.Dashboard.Controls.Add(this.label6);
            this.Dashboard.Controls.Add(this.label11);
            this.Dashboard.Controls.Add(this.label1);
            this.Dashboard.Controls.Add(this.label10);
            this.Dashboard.Controls.Add(this.label2);
            this.Dashboard.Controls.Add(this.label9);
            this.Dashboard.Controls.Add(this.label3);
            this.Dashboard.Controls.Add(this.label8);
            this.Dashboard.Controls.Add(this.label4);
            this.Dashboard.Controls.Add(this.label5);
            this.Dashboard.Location = new System.Drawing.Point(4, 22);
            this.Dashboard.Name = "Dashboard";
            this.Dashboard.Padding = new System.Windows.Forms.Padding(3);
            this.Dashboard.Size = new System.Drawing.Size(961, 716);
            this.Dashboard.TabIndex = 0;
            this.Dashboard.Text = "Dashboard";
            this.Dashboard.UseVisualStyleBackColor = true;
            // 
            // Developer
            // 
            this.Developer.Controls.Add(this.RTIA_labal);
            this.Developer.Controls.Add(this.RInt_labal);
            this.Developer.Controls.Add(this.Rint);
            this.Developer.Controls.Add(this.RTIA);
            this.Developer.Controls.Add(this.LED_color_box);
            this.Developer.Controls.Add(this.comboBox1);
            this.Developer.Controls.Add(this.Recording_time_box);
            this.Developer.Controls.Add(this.Current_box);
            this.Developer.Controls.Add(this.Sensor_version);
            this.Developer.Controls.Add(this.Recording_time);
            this.Developer.Controls.Add(this.Current);
            this.Developer.Controls.Add(this.chart2);
            this.Developer.Controls.Add(this.Save_Recording);
            this.Developer.Controls.Add(this.Record);
            this.Developer.Controls.Add(this.Test);
            this.Developer.Controls.Add(this.Play);
            this.Developer.Location = new System.Drawing.Point(4, 22);
            this.Developer.Name = "Developer";
            this.Developer.Padding = new System.Windows.Forms.Padding(3);
            this.Developer.Size = new System.Drawing.Size(961, 716);
            this.Developer.TabIndex = 1;
            this.Developer.Text = "Developer";
            this.Developer.UseVisualStyleBackColor = true;
            // 
            // Play
            // 
            this.Play.Location = new System.Drawing.Point(81, 225);
            this.Play.Name = "Play";
            this.Play.Size = new System.Drawing.Size(94, 23);
            this.Play.TabIndex = 0;
            this.Play.Text = "Play";
            this.Play.UseVisualStyleBackColor = true;
            // 
            // Test
            // 
            this.Test.Location = new System.Drawing.Point(81, 260);
            this.Test.Name = "Test";
            this.Test.Size = new System.Drawing.Size(94, 23);
            this.Test.TabIndex = 1;
            this.Test.Text = "Test";
            this.Test.UseVisualStyleBackColor = true;
            // 
            // Record
            // 
            this.Record.Location = new System.Drawing.Point(81, 295);
            this.Record.Name = "Record";
            this.Record.Size = new System.Drawing.Size(94, 23);
            this.Record.TabIndex = 2;
            this.Record.Text = "Record";
            this.Record.UseVisualStyleBackColor = true;
            // 
            // Save_Recording
            // 
            this.Save_Recording.AutoSize = true;
            this.Save_Recording.Location = new System.Drawing.Point(81, 330);
            this.Save_Recording.Name = "Save_Recording";
            this.Save_Recording.Size = new System.Drawing.Size(94, 23);
            this.Save_Recording.TabIndex = 3;
            this.Save_Recording.Text = "Save Recording";
            this.Save_Recording.UseVisualStyleBackColor = true;
            // 
            // chart2
            // 
            chartArea4.Name = "ChartArea1";
            this.chart2.ChartAreas.Add(chartArea4);
            legend2.Name = "Legend1";
            this.chart2.Legends.Add(legend2);
            this.chart2.Location = new System.Drawing.Point(353, 77);
            this.chart2.Name = "chart2";
            series8.ChartArea = "ChartArea1";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series8.Legend = "Legend1";
            series8.Name = "PD1";
            series9.ChartArea = "ChartArea1";
            series9.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series9.Legend = "Legend1";
            series9.Name = "PD2";
            series10.ChartArea = "ChartArea1";
            series10.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series10.Legend = "Legend1";
            series10.Name = "PD3";
            series11.ChartArea = "ChartArea1";
            series11.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series11.Legend = "Legend1";
            series11.Name = "PD4";
            series12.ChartArea = "ChartArea1";
            series12.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series12.Legend = "Legend1";
            series12.Name = "PD5";
            this.chart2.Series.Add(series8);
            this.chart2.Series.Add(series9);
            this.chart2.Series.Add(series10);
            this.chart2.Series.Add(series11);
            this.chart2.Series.Add(series12);
            this.chart2.Size = new System.Drawing.Size(587, 478);
            this.chart2.TabIndex = 4;
            this.chart2.Text = "chart2";
            title10.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            title10.Name = "Detectors\' intensity";
            title10.Text = "Detectors\' intensity";
            title11.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Left;
            title11.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            title11.Name = "Intensity";
            title11.Text = "Intensity [a.u]";
            title12.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            title12.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            title12.Name = "Time";
            title12.Text = "Time [sec]";
            this.chart2.Titles.Add(title10);
            this.chart2.Titles.Add(title11);
            this.chart2.Titles.Add(title12);
            // 
            // Current
            // 
            this.Current.AutoSize = true;
            this.Current.Location = new System.Drawing.Point(75, 110);
            this.Current.Name = "Current";
            this.Current.Size = new System.Drawing.Size(65, 13);
            this.Current.TabIndex = 5;
            this.Current.Text = "Current [mA]";
            this.Current.Click += new System.EventHandler(this.label7_Click);
            // 
            // Recording_time
            // 
            this.Recording_time.AutoSize = true;
            this.Recording_time.Location = new System.Drawing.Point(75, 140);
            this.Recording_time.Name = "Recording_time";
            this.Recording_time.Size = new System.Drawing.Size(104, 13);
            this.Recording_time.TabIndex = 6;
            this.Recording_time.Text = "Recording time [sec]";
            // 
            // Sensor_version
            // 
            this.Sensor_version.AutoSize = true;
            this.Sensor_version.Location = new System.Drawing.Point(75, 170);
            this.Sensor_version.Name = "Sensor_version";
            this.Sensor_version.Size = new System.Drawing.Size(77, 13);
            this.Sensor_version.TabIndex = 7;
            this.Sensor_version.Text = "Sensor version";
            this.Sensor_version.Click += new System.EventHandler(this.label7_Click_1);
            // 
            // Current_box
            // 
            this.Current_box.Location = new System.Drawing.Point(226, 102);
            this.Current_box.Name = "Current_box";
            this.Current_box.Size = new System.Drawing.Size(100, 20);
            this.Current_box.TabIndex = 8;
            this.Current_box.Text = "15";
            // 
            // Recording_time_box
            // 
            this.Recording_time_box.Location = new System.Drawing.Point(226, 137);
            this.Recording_time_box.Name = "Recording_time_box";
            this.Recording_time_box.Size = new System.Drawing.Size(100, 20);
            this.Recording_time_box.TabIndex = 9;
            this.Recording_time_box.Text = "10";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "00",
            "01"});
            this.comboBox1.Location = new System.Drawing.Point(226, 170);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(100, 21);
            this.comboBox1.TabIndex = 10;
            this.comboBox1.Text = "00";
            // 
            // LED_color_box
            // 
            this.LED_color_box.BackColor = System.Drawing.Color.WhiteSmoke;
            this.LED_color_box.Controls.Add(this.LED_Green_select);
            this.LED_color_box.Controls.Add(this.LED_Red_select);
            this.LED_color_box.Location = new System.Drawing.Point(240, 225);
            this.LED_color_box.Name = "LED_color_box";
            this.LED_color_box.Size = new System.Drawing.Size(107, 71);
            this.LED_color_box.TabIndex = 11;
            this.LED_color_box.TabStop = false;
            this.LED_color_box.Text = "LED Color";
            this.LED_color_box.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // LED_Red_select
            // 
            this.LED_Red_select.AutoSize = true;
            this.LED_Red_select.Location = new System.Drawing.Point(7, 20);
            this.LED_Red_select.Name = "LED_Red_select";
            this.LED_Red_select.Size = new System.Drawing.Size(45, 17);
            this.LED_Red_select.TabIndex = 0;
            this.LED_Red_select.TabStop = true;
            this.LED_Red_select.Text = "Red";
            this.LED_Red_select.UseVisualStyleBackColor = true;
            // 
            // LED_Green_select
            // 
            this.LED_Green_select.AutoSize = true;
            this.LED_Green_select.Location = new System.Drawing.Point(7, 41);
            this.LED_Green_select.Name = "LED_Green_select";
            this.LED_Green_select.Size = new System.Drawing.Size(54, 17);
            this.LED_Green_select.TabIndex = 1;
            this.LED_Green_select.TabStop = true;
            this.LED_Green_select.Text = "Green";
            this.LED_Green_select.UseVisualStyleBackColor = true;
            // 
            // RTIA
            // 
            this.RTIA.FormattingEnabled = true;
            this.RTIA.Items.AddRange(new object[] {
            "200",
            "100",
            "50",
            "25",
            "12.5"});
            this.RTIA.Location = new System.Drawing.Point(19, 411);
            this.RTIA.Name = "RTIA";
            this.RTIA.Size = new System.Drawing.Size(121, 21);
            this.RTIA.TabIndex = 12;
            this.RTIA.Text = "200";
            // 
            // Rint
            // 
            this.Rint.FormattingEnabled = true;
            this.Rint.Items.AddRange(new object[] {
            "400",
            "200",
            "100"});
            this.Rint.Location = new System.Drawing.Point(180, 411);
            this.Rint.Name = "Rint";
            this.Rint.Size = new System.Drawing.Size(121, 21);
            this.Rint.TabIndex = 13;
            this.Rint.Text = "400";
            // 
            // RInt_labal
            // 
            this.RInt_labal.Location = new System.Drawing.Point(177, 395);
            this.RInt_labal.Name = "RInt_labal";
            this.RInt_labal.Size = new System.Drawing.Size(124, 13);
            this.RInt_labal.TabIndex = 14;
            this.RInt_labal.Text = "RInt [Kohm]";
            this.RInt_labal.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // RTIA_labal
            // 
            this.RTIA_labal.Location = new System.Drawing.Point(16, 395);
            this.RTIA_labal.Name = "RTIA_labal";
            this.RTIA_labal.Size = new System.Drawing.Size(124, 13);
            this.RTIA_labal.TabIndex = 15;
            this.RTIA_labal.Text = "RTIA [Kohm]";
            this.RTIA_labal.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.RTIA_labal.Click += new System.EventHandler(this.label7_Click_2);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 672);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "OxyD";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.Dashboard.ResumeLayout(false);
            this.Dashboard.PerformLayout();
            this.Developer.ResumeLayout(false);
            this.Developer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart2)).EndInit();
            this.LED_color_box.ResumeLayout(false);
            this.LED_color_box.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Dashboard;
        private System.Windows.Forms.TabPage Developer;
        private System.Windows.Forms.Button Save_Recording;
        private System.Windows.Forms.Button Record;
        private System.Windows.Forms.Button Test;
        private System.Windows.Forms.Button Play;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart2;
        private System.Windows.Forms.Label Recording_time;
        private System.Windows.Forms.Label Current;
        private System.Windows.Forms.Label Sensor_version;
        private System.Windows.Forms.TextBox Recording_time_box;
        private System.Windows.Forms.TextBox Current_box;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox LED_color_box;
        private System.Windows.Forms.RadioButton LED_Green_select;
        private System.Windows.Forms.RadioButton LED_Red_select;
        private System.Windows.Forms.Label RTIA_labal;
        private System.Windows.Forms.Label RInt_labal;
        private System.Windows.Forms.ComboBox Rint;
        private System.Windows.Forms.ComboBox RTIA;
    }
}

