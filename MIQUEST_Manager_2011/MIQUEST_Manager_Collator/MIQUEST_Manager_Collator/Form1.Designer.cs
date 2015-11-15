namespace MIQUEST_Manager_Collator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.sh_tb_folder_path_response = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.sh_tb_folder_path_output = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.sh_bt_select_response = new System.Windows.Forms.Button();
            this.sh_bt_select_output = new System.Windows.Forms.Button();
            this.sh_bt_run = new System.Windows.Forms.Button();
            this.sh_progress_bar = new System.Windows.Forms.ProgressBar();
            this.sh_lb_status_message = new System.Windows.Forms.Label();
            this.SH_CB_Save_Logfile = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.sh_tb_practice_code = new System.Windows.Forms.TextBox();
            this.sh_cb_practice_code_override = new System.Windows.Forms.CheckBox();
            this.sh_cb_init_bul_override = new System.Windows.Forms.CheckBox();
            this.sh_rb_initial = new System.Windows.Forms.RadioButton();
            this.sh_rb_bulk = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // sh_tb_folder_path_response
            // 
            this.sh_tb_folder_path_response.Location = new System.Drawing.Point(135, 127);
            this.sh_tb_folder_path_response.Name = "sh_tb_folder_path_response";
            this.sh_tb_folder_path_response.Size = new System.Drawing.Size(521, 20);
            this.sh_tb_folder_path_response.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 131);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Response Folder";
            // 
            // sh_tb_folder_path_output
            // 
            this.sh_tb_folder_path_output.Location = new System.Drawing.Point(135, 160);
            this.sh_tb_folder_path_output.Name = "sh_tb_folder_path_output";
            this.sh_tb_folder_path_output.Size = new System.Drawing.Size(521, 20);
            this.sh_tb_folder_path_output.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 164);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Output Folder";
            // 
            // sh_bt_select_response
            // 
            this.sh_bt_select_response.Location = new System.Drawing.Point(662, 127);
            this.sh_bt_select_response.Name = "sh_bt_select_response";
            this.sh_bt_select_response.Size = new System.Drawing.Size(26, 20);
            this.sh_bt_select_response.TabIndex = 4;
            this.sh_bt_select_response.Text = "...";
            this.sh_bt_select_response.UseVisualStyleBackColor = true;
            this.sh_bt_select_response.Click += new System.EventHandler(this.sh_bt_select_response_Click);
            // 
            // sh_bt_select_output
            // 
            this.sh_bt_select_output.Location = new System.Drawing.Point(662, 159);
            this.sh_bt_select_output.Name = "sh_bt_select_output";
            this.sh_bt_select_output.Size = new System.Drawing.Size(25, 20);
            this.sh_bt_select_output.TabIndex = 5;
            this.sh_bt_select_output.Text = "...";
            this.sh_bt_select_output.UseVisualStyleBackColor = true;
            this.sh_bt_select_output.Click += new System.EventHandler(this.sh_bt_select_output_Click);
            // 
            // sh_bt_run
            // 
            this.sh_bt_run.Location = new System.Drawing.Point(613, 346);
            this.sh_bt_run.Name = "sh_bt_run";
            this.sh_bt_run.Size = new System.Drawing.Size(75, 23);
            this.sh_bt_run.TabIndex = 6;
            this.sh_bt_run.Text = "Collate!";
            this.sh_bt_run.UseVisualStyleBackColor = true;
            this.sh_bt_run.Click += new System.EventHandler(this.sh_bt_run_Click);
            // 
            // sh_progress_bar
            // 
            this.sh_progress_bar.Location = new System.Drawing.Point(47, 317);
            this.sh_progress_bar.Name = "sh_progress_bar";
            this.sh_progress_bar.Size = new System.Drawing.Size(640, 23);
            this.sh_progress_bar.TabIndex = 7;
            // 
            // sh_lb_status_message
            // 
            this.sh_lb_status_message.AutoSize = true;
            this.sh_lb_status_message.Location = new System.Drawing.Point(50, 351);
            this.sh_lb_status_message.Name = "sh_lb_status_message";
            this.sh_lb_status_message.Size = new System.Drawing.Size(0, 13);
            this.sh_lb_status_message.TabIndex = 8;
            // 
            // SH_CB_Save_Logfile
            // 
            this.SH_CB_Save_Logfile.AutoSize = true;
            this.SH_CB_Save_Logfile.Location = new System.Drawing.Point(135, 93);
            this.SH_CB_Save_Logfile.Name = "SH_CB_Save_Logfile";
            this.SH_CB_Save_Logfile.Size = new System.Drawing.Size(91, 17);
            this.SH_CB_Save_Logfile.TabIndex = 10;
            this.SH_CB_Save_Logfile.Text = "Save Log File";
            this.SH_CB_Save_Logfile.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(52, 40);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(104, 24);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(162, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(231, 23);
            this.label3.TabIndex = 12;
            this.label3.Text = "MIQUEST Manager - Collator";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(650, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Version 1.9";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(42, 224);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Practice Code";
            // 
            // sh_tb_practice_code
            // 
            this.sh_tb_practice_code.Location = new System.Drawing.Point(138, 221);
            this.sh_tb_practice_code.Name = "sh_tb_practice_code";
            this.sh_tb_practice_code.Size = new System.Drawing.Size(136, 20);
            this.sh_tb_practice_code.TabIndex = 18;
            // 
            // sh_cb_practice_code_override
            // 
            this.sh_cb_practice_code_override.AutoSize = true;
            this.sh_cb_practice_code_override.Location = new System.Drawing.Point(138, 198);
            this.sh_cb_practice_code_override.Name = "sh_cb_practice_code_override";
            this.sh_cb_practice_code_override.Size = new System.Drawing.Size(136, 17);
            this.sh_cb_practice_code_override.TabIndex = 17;
            this.sh_cb_practice_code_override.Text = "Practice Code Override";
            this.sh_cb_practice_code_override.UseVisualStyleBackColor = true;
            this.sh_cb_practice_code_override.CheckedChanged += new System.EventHandler(this.sh_cb_practice_code_override_CheckedChanged);
            // 
            // sh_cb_init_bul_override
            // 
            this.sh_cb_init_bul_override.AutoSize = true;
            this.sh_cb_init_bul_override.Location = new System.Drawing.Point(138, 257);
            this.sh_cb_init_bul_override.Name = "sh_cb_init_bul_override";
            this.sh_cb_init_bul_override.Size = new System.Drawing.Size(119, 17);
            this.sh_cb_init_bul_override.TabIndex = 20;
            this.sh_cb_init_bul_override.Text = "Initial/Bulk Override";
            this.sh_cb_init_bul_override.UseVisualStyleBackColor = true;
            // 
            // sh_rb_initial
            // 
            this.sh_rb_initial.AutoSize = true;
            this.sh_rb_initial.Location = new System.Drawing.Point(138, 280);
            this.sh_rb_initial.Name = "sh_rb_initial";
            this.sh_rb_initial.Size = new System.Drawing.Size(76, 17);
            this.sh_rb_initial.TabIndex = 21;
            this.sh_rb_initial.TabStop = true;
            this.sh_rb_initial.Text = "Initial Load";
            this.sh_rb_initial.UseVisualStyleBackColor = true;
            // 
            // sh_rb_bulk
            // 
            this.sh_rb_bulk.AutoSize = true;
            this.sh_rb_bulk.Location = new System.Drawing.Point(221, 281);
            this.sh_rb_bulk.Name = "sh_rb_bulk";
            this.sh_rb_bulk.Size = new System.Drawing.Size(73, 17);
            this.sh_rb_bulk.TabIndex = 22;
            this.sh_rb_bulk.TabStop = true;
            this.sh_rb_bulk.Text = "Bulk Load";
            this.sh_rb_bulk.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 396);
            this.Controls.Add(this.sh_rb_bulk);
            this.Controls.Add(this.sh_rb_initial);
            this.Controls.Add(this.sh_cb_init_bul_override);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.sh_tb_practice_code);
            this.Controls.Add(this.sh_cb_practice_code_override);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.SH_CB_Save_Logfile);
            this.Controls.Add(this.sh_lb_status_message);
            this.Controls.Add(this.sh_progress_bar);
            this.Controls.Add(this.sh_bt_run);
            this.Controls.Add(this.sh_bt_select_output);
            this.Controls.Add(this.sh_bt_select_response);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.sh_tb_folder_path_output);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sh_tb_folder_path_response);
            this.Name = "Form1";
            this.Text = "MIQUEST Manager - Collator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox sh_tb_folder_path_response;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox sh_tb_folder_path_output;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button sh_bt_select_response;
        private System.Windows.Forms.Button sh_bt_select_output;
        private System.Windows.Forms.Button sh_bt_run;
        private System.Windows.Forms.ProgressBar sh_progress_bar;
        private System.Windows.Forms.Label sh_lb_status_message;
        private System.Windows.Forms.CheckBox SH_CB_Save_Logfile;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox sh_tb_practice_code;
        private System.Windows.Forms.CheckBox sh_cb_practice_code_override;
        private System.Windows.Forms.CheckBox sh_cb_init_bul_override;
        private System.Windows.Forms.RadioButton sh_rb_initial;
        private System.Windows.Forms.RadioButton sh_rb_bulk;
    }
}

