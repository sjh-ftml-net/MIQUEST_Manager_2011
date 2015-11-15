namespace MIQUEST_Updater
{
    partial class Home
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Home));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.sh_tb_infolder = new System.Windows.Forms.TextBox();
            this.sh_tb_outfolder = new System.Windows.Forms.TextBox();
            this.sh_bt_miquest_response = new System.Windows.Forms.Button();
            this.sh_bt_output_folder = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.sh_bt_run = new System.Windows.Forms.Button();
            this.SH_dp_end_date = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.sh_tb_export_period = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.sh_tb_query_split = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.sh_tb_practice_code = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.sh_prog_bar = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.sh_tb_encounter_split = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.sh_tb_ltc_export_period = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.sh_cb_tpp_encounter_split = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.sh_tb_population = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.sh_bt_suggest_splits = new System.Windows.Forms.Button();
            this.sh_cb_initial_load = new System.Windows.Forms.CheckBox();
            this.sh_cb_write_code_list = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "HQL Template Folder";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "HQL Output Folder";
            // 
            // sh_tb_infolder
            // 
            this.sh_tb_infolder.Location = new System.Drawing.Point(154, 73);
            this.sh_tb_infolder.Name = "sh_tb_infolder";
            this.sh_tb_infolder.Size = new System.Drawing.Size(423, 20);
            this.sh_tb_infolder.TabIndex = 2;
            // 
            // sh_tb_outfolder
            // 
            this.sh_tb_outfolder.Location = new System.Drawing.Point(154, 99);
            this.sh_tb_outfolder.Name = "sh_tb_outfolder";
            this.sh_tb_outfolder.Size = new System.Drawing.Size(423, 20);
            this.sh_tb_outfolder.TabIndex = 3;
            // 
            // sh_bt_miquest_response
            // 
            this.sh_bt_miquest_response.Location = new System.Drawing.Point(583, 73);
            this.sh_bt_miquest_response.Name = "sh_bt_miquest_response";
            this.sh_bt_miquest_response.Size = new System.Drawing.Size(33, 20);
            this.sh_bt_miquest_response.TabIndex = 4;
            this.sh_bt_miquest_response.Text = "...";
            this.sh_bt_miquest_response.UseVisualStyleBackColor = true;
            this.sh_bt_miquest_response.Click += new System.EventHandler(this.sh_bt_miquest_response_Click);
            // 
            // sh_bt_output_folder
            // 
            this.sh_bt_output_folder.Location = new System.Drawing.Point(583, 99);
            this.sh_bt_output_folder.Name = "sh_bt_output_folder";
            this.sh_bt_output_folder.Size = new System.Drawing.Size(33, 20);
            this.sh_bt_output_folder.TabIndex = 5;
            this.sh_bt_output_folder.Text = "...";
            this.sh_bt_output_folder.UseVisualStyleBackColor = true;
            this.sh_bt_output_folder.Click += new System.EventHandler(this.sh_bt_output_folder_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(27, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(111, 24);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // sh_bt_run
            // 
            this.sh_bt_run.Location = new System.Drawing.Point(542, 430);
            this.sh_bt_run.Name = "sh_bt_run";
            this.sh_bt_run.Size = new System.Drawing.Size(75, 23);
            this.sh_bt_run.TabIndex = 10;
            this.sh_bt_run.Text = "Update!";
            this.sh_bt_run.UseVisualStyleBackColor = true;
            this.sh_bt_run.Click += new System.EventHandler(this.sh_bt_run_Click);
            // 
            // SH_dp_end_date
            // 
            this.SH_dp_end_date.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.SH_dp_end_date.Location = new System.Drawing.Point(154, 178);
            this.SH_dp_end_date.Name = "SH_dp_end_date";
            this.SH_dp_end_date.Size = new System.Drawing.Size(136, 20);
            this.SH_dp_end_date.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(80, 182);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "End Date";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(62, 208);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Export Period";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(74, 331);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Query Split";
            // 
            // sh_tb_export_period
            // 
            this.sh_tb_export_period.Location = new System.Drawing.Point(154, 204);
            this.sh_tb_export_period.Name = "sh_tb_export_period";
            this.sh_tb_export_period.Size = new System.Drawing.Size(136, 20);
            this.sh_tb_export_period.TabIndex = 17;
            this.sh_tb_export_period.Text = "12";
            this.sh_tb_export_period.TextChanged += new System.EventHandler(this.sh_tb_export_period_TextChanged);
            this.sh_tb_export_period.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.sh_tb_export_period_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(296, 208);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(166, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Months of current codes to export";
            // 
            // sh_tb_query_split
            // 
            this.sh_tb_query_split.Location = new System.Drawing.Point(154, 327);
            this.sh_tb_query_split.Name = "sh_tb_query_split";
            this.sh_tb_query_split.Size = new System.Drawing.Size(136, 20);
            this.sh_tb_query_split.TabIndex = 19;
            this.sh_tb_query_split.Text = "6";
            this.sh_tb_query_split.TextChanged += new System.EventHandler(this.sh_tb_query_split_TextChanged);
            this.sh_tb_query_split.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.sh_tb_query_split_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(296, 331);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(234, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Months to split each query by (based on current)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(296, 182);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(165, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "End date of the query (e.g. today)";
            // 
            // sh_tb_practice_code
            // 
            this.sh_tb_practice_code.Location = new System.Drawing.Point(154, 142);
            this.sh_tb_practice_code.Name = "sh_tb_practice_code";
            this.sh_tb_practice_code.Size = new System.Drawing.Size(136, 20);
            this.sh_tb_practice_code.TabIndex = 22;
            this.sh_tb_practice_code.Text = "XXXX";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(58, 145);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 13);
            this.label10.TabIndex = 23;
            this.label10.Text = "Practice Code";
            // 
            // sh_prog_bar
            // 
            this.sh_prog_bar.Location = new System.Drawing.Point(154, 430);
            this.sh_prog_bar.Name = "sh_prog_bar";
            this.sh_prog_bar.Size = new System.Drawing.Size(381, 23);
            this.sh_prog_bar.TabIndex = 24;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(144, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(234, 23);
            this.label3.TabIndex = 25;
            this.label3.Text = "MIQUEST Manager - Updater";
            // 
            // sh_tb_encounter_split
            // 
            this.sh_tb_encounter_split.Location = new System.Drawing.Point(154, 380);
            this.sh_tb_encounter_split.Name = "sh_tb_encounter_split";
            this.sh_tb_encounter_split.Size = new System.Drawing.Size(136, 20);
            this.sh_tb_encounter_split.TabIndex = 26;
            this.sh_tb_encounter_split.Text = "4";
            this.sh_tb_encounter_split.TextChanged += new System.EventHandler(this.sh_tb_encounter_split_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(59, 383);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(79, 13);
            this.label11.TabIndex = 27;
            this.label11.Text = "Encounter Split";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(296, 383);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(152, 13);
            this.label12.TabIndex = 28;
            this.label12.Text = "Days to split encounters export";
            // 
            // sh_tb_ltc_export_period
            // 
            this.sh_tb_ltc_export_period.Location = new System.Drawing.Point(154, 253);
            this.sh_tb_ltc_export_period.Name = "sh_tb_ltc_export_period";
            this.sh_tb_ltc_export_period.Size = new System.Drawing.Size(136, 20);
            this.sh_tb_ltc_export_period.TabIndex = 29;
            this.sh_tb_ltc_export_period.Text = "60";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(39, 256);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(93, 13);
            this.label13.TabIndex = 31;
            this.label13.Text = "LTC Export Period";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(296, 256);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(180, 13);
            this.label15.TabIndex = 33;
            this.label15.Text = "Months of LTC/QOF codes to export";
            // 
            // sh_cb_tpp_encounter_split
            // 
            this.sh_cb_tpp_encounter_split.AutoSize = true;
            this.sh_cb_tpp_encounter_split.Location = new System.Drawing.Point(154, 357);
            this.sh_cb_tpp_encounter_split.Name = "sh_cb_tpp_encounter_split";
            this.sh_cb_tpp_encounter_split.Size = new System.Drawing.Size(186, 17);
            this.sh_cb_tpp_encounter_split.TabIndex = 34;
            this.sh_cb_tpp_encounter_split.Text = "Encounter Split (For TPP systems)";
            this.sh_cb_tpp_encounter_split.UseVisualStyleBackColor = true;
            this.sh_cb_tpp_encounter_split.CheckedChanged += new System.EventHandler(this.sh_cb_tpp_encounter_split_CheckedChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(566, 9);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(60, 13);
            this.label14.TabIndex = 35;
            this.label14.Text = "Version 1.5";
            this.label14.Click += new System.EventHandler(this.label14_Click);
            // 
            // sh_tb_population
            // 
            this.sh_tb_population.Location = new System.Drawing.Point(154, 298);
            this.sh_tb_population.Name = "sh_tb_population";
            this.sh_tb_population.Size = new System.Drawing.Size(136, 20);
            this.sh_tb_population.TabIndex = 36;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(33, 301);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(99, 13);
            this.label16.TabIndex = 37;
            this.label16.Text = "Practice Population";
            // 
            // sh_bt_suggest_splits
            // 
            this.sh_bt_suggest_splits.Location = new System.Drawing.Point(299, 296);
            this.sh_bt_suggest_splits.Name = "sh_bt_suggest_splits";
            this.sh_bt_suggest_splits.Size = new System.Drawing.Size(102, 23);
            this.sh_bt_suggest_splits.TabIndex = 38;
            this.sh_bt_suggest_splits.Text = "Suggest Splits";
            this.sh_bt_suggest_splits.UseVisualStyleBackColor = true;
            this.sh_bt_suggest_splits.Click += new System.EventHandler(this.sh_bt_suggest_splits_Click);
            // 
            // sh_cb_initial_load
            // 
            this.sh_cb_initial_load.AutoSize = true;
            this.sh_cb_initial_load.Location = new System.Drawing.Point(154, 231);
            this.sh_cb_initial_load.Name = "sh_cb_initial_load";
            this.sh_cb_initial_load.Size = new System.Drawing.Size(111, 17);
            this.sh_cb_initial_load.TabIndex = 39;
            this.sh_cb_initial_load.Text = "Create Initial Load";
            this.sh_cb_initial_load.UseVisualStyleBackColor = true;
            this.sh_cb_initial_load.CheckedChanged += new System.EventHandler(this.sh_cb_initial_load_CheckedChanged);
            // 
            // sh_cb_write_code_list
            // 
            this.sh_cb_write_code_list.AutoSize = true;
            this.sh_cb_write_code_list.Location = new System.Drawing.Point(154, 407);
            this.sh_cb_write_code_list.Name = "sh_cb_write_code_list";
            this.sh_cb_write_code_list.Size = new System.Drawing.Size(133, 17);
            this.sh_cb_write_code_list.TabIndex = 40;
            this.sh_cb_write_code_list.Text = "Write Debug Code List";
            this.sh_cb_write_code_list.UseVisualStyleBackColor = true;
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(638, 544);
            this.Controls.Add(this.sh_cb_write_code_list);
            this.Controls.Add(this.sh_cb_initial_load);
            this.Controls.Add(this.sh_bt_suggest_splits);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.sh_tb_population);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.sh_cb_tpp_encounter_split);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.sh_tb_ltc_export_period);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.sh_tb_encounter_split);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.sh_prog_bar);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.sh_tb_practice_code);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.sh_tb_query_split);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.sh_tb_export_period);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.SH_dp_end_date);
            this.Controls.Add(this.sh_bt_run);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.sh_bt_output_folder);
            this.Controls.Add(this.sh_bt_miquest_response);
            this.Controls.Add(this.sh_tb_outfolder);
            this.Controls.Add(this.sh_tb_infolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Home";
            this.Text = "MIQUEST Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Home_FormClosing);
            this.Load += new System.EventHandler(this.Home_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox sh_tb_infolder;
        private System.Windows.Forms.TextBox sh_tb_outfolder;
        private System.Windows.Forms.Button sh_bt_miquest_response;
        private System.Windows.Forms.Button sh_bt_output_folder;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button sh_bt_run;
        private System.Windows.Forms.DateTimePicker SH_dp_end_date;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox sh_tb_export_period;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox sh_tb_query_split;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox sh_tb_practice_code;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ProgressBar sh_prog_bar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox sh_tb_encounter_split;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox sh_tb_ltc_export_period;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.CheckBox sh_cb_tpp_encounter_split;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox sh_tb_population;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button sh_bt_suggest_splits;
        private System.Windows.Forms.CheckBox sh_cb_initial_load;
        private System.Windows.Forms.CheckBox sh_cb_write_code_list;
    }
}

