/*
 * Create a runable set of MIQUEST queries from templates.
 * 
 * History
 * 01/08/2011: V1.0 First test release
 * 31/08/2011: V1.1 Corrected Encounters processing (TPP specific)
 * 15/09/2011: V1.2 Added population suggestions and IL/Bulk process
 * 01/02/2012: V1.3 Fixed encounter subset name clash
 * 16/06/2012: V1.4 Added debug code listing and query order
 * 15/08/2012  V1.5 Allowed 1 day encounter split (and removed date overlaps in encounters)
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace MIQUEST_Updater
{
    public partial class Home : Form
    {

        public Home()
        {
            InitializeComponent();
            if (sh_cb_tpp_encounter_split.Checked)
            {
                sh_tb_encounter_split.Enabled = true;
            }
            else
            {
                sh_tb_encounter_split.Enabled = false;
            }
            if (sh_cb_initial_load.Checked)
            {
                sh_tb_ltc_export_period.Enabled = true;
            }
            else
            {
                sh_tb_ltc_export_period.Enabled = false;
            }
        }

        static string today = String.Format("{0:yyyyMMdd}", DateTime.Today);
        string logfile_path;
        string code_type;
        int max_order;

        // -------------------------------------------------------------------------------------------------------------------
        // Main Loop
        // -------------------------------------------------------------------------------------------------------------------
        private void sh_bt_run_Click(object sender, EventArgs e)
        {
            string inpath = sh_tb_infolder.Text;
            string outpath = sh_tb_outfolder.Text;
            string p_outpath = "";
            bool fatal = false;
            StreamWriter code_include_list = null;
            StreamWriter code_exclude_list = null;
            //StreamWriter qry_order = null;

            // Check for '\' on paths
            if (!inpath.EndsWith("\\")) inpath = inpath + "\\";
            if (!outpath.EndsWith("\\")) outpath = outpath + "\\";

            // Check the folders exist
            if (!Directory.Exists(inpath))
            {
                MessageBox.Show("The template folder was not found (" + inpath + ")");
                fatal = true;
            }
            if (!Directory.Exists(outpath))
            {
                switch (MessageBox.Show("The output folder was not found (" + outpath + "), create the folder?",
                                            "Output folder not found",
                                            MessageBoxButtons.YesNoCancel,
                                            MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        // "Yes" processing
                        Directory.CreateDirectory(outpath);
                        break;

                    case DialogResult.No:
                        // "No" processing
                        fatal = true;
                        break;

                    case DialogResult.Cancel:
                        // "Cancel" processing
                        fatal = true;
                        break;
                }
            }
            else
            {
                // Clean out the target folder
                DirectoryInfo target = new DirectoryInfo(outpath);

                foreach (FileInfo file in target.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in target.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
			
			if (!File.Exists(inpath + "DIA001R.TPL"))
			{
				MessageBox.Show("Cannot find the template files (" + inpath + ")");
				fatal = true;
			}

            sh_prog_bar.Value = 0;

            ReadHeader(inpath + "DIA001R.TPL");

            if (!fatal)
            {

                // Calculate the splits for current diag/drug
                int export_period = Convert.ToInt32(sh_tb_export_period.Text);
                int query_split = Convert.ToInt32(sh_tb_query_split.Text);
                DateTime end_date = Convert.ToDateTime(SH_dp_end_date.Text);
                DateTime q_end_date;
                DateTime q_ltc_end_date;
                DateTime start_date = end_date.AddMonths(0 - export_period);
                int part_no = 1;
                bool tail_ender = false;
                bool multi_set = false;
                string practice_code = sh_tb_practice_code.Text.Trim();

                // LTCs
                int ltc_period = Convert.ToInt32(sh_tb_ltc_export_period.Text);
                DateTime ltc_start_date = end_date.AddMonths(0 - ltc_period);
                int ltc_months = ltc_period / (export_period / query_split);

                sh_prog_bar.Value = 0;

                logfile_path = outpath + today + "_log.txt";

                if (sh_cb_write_code_list.Checked)
                {
                    // ** Read Code Use Report
                    code_include_list = new StreamWriter(outpath + "code_include_list.txt");
                    code_exclude_list = new StreamWriter(outpath + "code_exclude_list.txt");
                    //qry_order = new StreamWriter(outpath + "qry_order.txt");
                }

                // Open the logfile and save the plan
                logfile_path = outpath + today + "_log.txt";
                StreamWriter log_file = new StreamWriter(logfile_path);
                log_file.WriteLine("Starting MIQUEST result collation at " + DateTime.Now);
                log_file.WriteLine("Code Type = " + code_type);
                log_file.WriteLine("From " + inpath);
                log_file.WriteLine("To " + outpath);
                log_file.WriteLine("End Date   " + Convert.ToString(end_date));

                log_file.WriteLine("Start Date " + Convert.ToString(start_date));
                log_file.WriteLine(Convert.ToString(export_period) + " months current data in " + Convert.ToString(query_split) + " month periods");

                if (sh_cb_initial_load.Checked)
                {
                    log_file.WriteLine("Initial Load Extract");
                    log_file.WriteLine("LTC Start Date " + Convert.ToString(ltc_start_date));
                    log_file.WriteLine(Convert.ToString(ltc_period) + " months LTC data in " + Convert.ToString(ltc_months) + " month periods");
                }
                else
                {
                    log_file.WriteLine("BULK Extract (no LTC or QOF)");
                }

                log_file.WriteLine("Export in " + export_period / query_split + " parts");
                log_file.WriteLine("Encounter split: " + Convert.ToString(sh_cb_tpp_encounter_split.Checked));
                if (sh_cb_tpp_encounter_split.Checked)
                {
                    log_file.WriteLine("Encounter split in " + Convert.ToString(sh_tb_encounter_split.Text) + " week parts");
                }

                if (query_split > export_period)
                {
                    query_split = export_period;
                }

                // Test if we have more than one set
                if (export_period > query_split)
                {
                    multi_set = true;
                }

                log_file.WriteLine("Multi Set Output: " + Convert.ToString(multi_set));

                // set up first set
                DateTime p_start = start_date;
                DateTime p_end = start_date.AddMonths(query_split);

                DateTime ltc_p_start = ltc_start_date;
                DateTime ltc_p_end = ltc_start_date.AddMonths(ltc_months);


                while (p_end <= end_date && p_end > p_start)
                {
                    // Don't overlap dates
                    if (p_end != end_date)
                    {
                        q_end_date = p_end.AddDays(-1);
                        q_ltc_end_date = ltc_p_end.AddDays(-1);
                    }
                    else
                    {
                        q_end_date = p_end;
                        q_ltc_end_date = ltc_p_end;
                    }

                    // Log activity
                    log_file.WriteLine("Part " + Convert.ToString(part_no) + " -------------------------------------");
                    log_file.WriteLine("Start Date " + Convert.ToString(p_start));
                    log_file.WriteLine("End Date   " + Convert.ToString(q_end_date));
                    if (sh_cb_initial_load.Checked)
                    {
                        log_file.WriteLine("LTC Start Date   " + Convert.ToString(ltc_p_start));
                        log_file.WriteLine("LTC End Date   " + Convert.ToString(q_ltc_end_date));
                    }

                    if (multi_set)
                    {
                        // Create folder if necessary
                        p_outpath = outpath + "PART" + Convert.ToString(part_no);
                        if (!Directory.Exists(p_outpath))
                        {
                            Directory.CreateDirectory(p_outpath);
                        }
                    }
                    else
                    {
                        p_outpath = outpath + "PART1";
                        if (!Directory.Exists(p_outpath))
                        {
                            Directory.CreateDirectory(p_outpath);
                        }
                    }

                    // Write the files
                    int file_cnt = 0;
                    string line = "";
                    StreamWriter outfile;
                    string outfilepath;
                    string tmp;

                    DirectoryInfo di = new DirectoryInfo(inpath);
                    FileInfo[] rgFiles = di.GetFiles("*.TPL");
                    foreach (FileInfo fi in rgFiles)
                    {
                        // Check we need this file according to configuration
                        bool process_file = true;
                        if (sh_cb_tpp_encounter_split.Checked)
                        {
                            if (fi.Name == "ENC001G.TPL" || fi.Name == "ENC001R.TPL")
                            {
                                process_file = false;
                                log_file.WriteLine("Skipping   " + fi.Name);
                            }
                        }
                        if (!sh_cb_initial_load.Checked)
                        {
                            if (fi.Name.StartsWith("LTC") || fi.Name.StartsWith("QOF"))
                            {
                                process_file = false;
                                log_file.WriteLine("Skipping   " + fi.Name);
                            }
                        }

                        if (process_file)
                        {
                            file_cnt++;
                            StreamReader infile = fi.OpenText();

                            // Open the target file
                            outfilepath = p_outpath;
                            if (!outfilepath.EndsWith("\\")) outfilepath = outfilepath + "\\";
                            outfilepath = outfilepath + fi.Name;

                            if (multi_set)
                            {
                                tmp = "_P" + Convert.ToString(part_no) + ".HQL";
                            }
                            else
                            {
                                tmp = ".HQL";
                            }

                            outfilepath = outfilepath.Replace(".TPL", tmp);

                            outfile = new StreamWriter(outfilepath);

                            line = infile.ReadLine();
                            while (line != null)
                            {
                                string stmp = Path.GetFileNameWithoutExtension(fi.Name);

                                if (sh_cb_write_code_list.Checked)
                                {
                                    // ** Counts
                                    if (line.IndexOf("QRY_ORDER") != -1)
                                    {
                                        string[] temp = line.Split(',');
                                        if (temp.Length >= 2)
                                        {
                                            string t = temp[1].Replace("\"", "");
                                            int order = Convert.ToInt32(t);
                                            if (max_order < order)
                                                max_order = order;
                                        }
                                        //qry_order.WriteLine(tst[1]);
                                    }

                                    // ** Read Code Use Report
                                    if (stmp.EndsWith("R"))
                                    {
                                        string code_test = "CODE IN";
                                        if (line.IndexOf(code_test) != -1 && line.IndexOf("CHOSEN") == -1)
                                        {

                                            string nline = line;
                                            nline = nline.Replace("AND", "");
                                            nline = nline.Replace("\"", "");
                                            nline = nline.Replace("(", "");
                                            nline = nline.Replace(")", "");
                                            nline = nline.Replace("WHERE", "");
                                            string[] codes = nline.Split(',');
                                            foreach (string code in codes)
                                            {
                                                string ncode = code.Trim();
                                                if (ncode.IndexOf(code_test) != -1)
                                                {
                                                    ncode = ncode.Replace(code_test, "").Trim();
                                                }
                                                ncode = ncode.TrimEnd();
                                                if (ncode != "")
                                                    code_include_list.WriteLine(stmp + "," + ncode);
                                            }
                                        }

                                        code_test = "CODE NOT_IN";
                                        if (line.IndexOf(code_test) != -1 && line.IndexOf("CHOSEN") == -1)
                                        {

                                            string nline = line;
                                            nline = nline.Replace("AND", "");
                                            nline = nline.Replace("\"", "");
                                            nline = nline.Replace("(", "");
                                            nline = nline.Replace(")", "");
                                            string[] codes = nline.Split(',');
                                            foreach (string code in codes)
                                            {
                                                string ncode = code.Trim();
                                                if (ncode.IndexOf(code_test) != -1)
                                                {
                                                    ncode = ncode.Replace(code_test, "").Trim();
                                                }
                                                ncode = ncode.TrimEnd();
                                                if (ncode != "")
                                                    code_exclude_list.WriteLine(stmp + "," + ncode);
                                            }
                                        }
                                    }
                                }

                                // Replace the text
                                line = line.Replace("<practice_code>", practice_code);
                                line = line.Replace("<start_date>", String.Format("{0:dd/MM/yyyy}", p_start));
                                line = line.Replace("<end_date>", String.Format("{0:dd/MM/yyyy}", q_end_date));
                                line = line.Replace("<ltc_start_date>", String.Format("{0:dd/MM/yyyy}", ltc_p_start));
                                line = line.Replace("<ltc_end_date>", String.Format("{0:dd/MM/yyyy}", q_ltc_end_date));

                                // Write the line to the target file
                                outfile.WriteLine(line);

                                line = infile.ReadLine();
                            }
                            infile.Close();
                            outfile.Close();
                        }
                    }

                    if (sh_prog_bar.Value < 100)
                        sh_prog_bar.Value = sh_prog_bar.Value + (100 / (export_period / query_split));

                    // Calculate new dates
                    p_start = p_end;
                    p_end = p_start.AddMonths(query_split);

                    ltc_p_start = ltc_p_end;
                    ltc_p_end = ltc_p_start.AddMonths(ltc_months);

                    // Check for the tail ender
                    if (!tail_ender && p_end >= end_date)
                    {
                        p_end = end_date;
                        tail_ender = true;
                        if (ltc_p_end != end_date)
                        {
                            ltc_p_end = end_date;
                        }
                    }
                    part_no++;
                    log_file.WriteLine("Files Converted: " + Convert.ToString(file_cnt));
                    file_cnt = 0;
                }

                // If checked, produce the TPP encounter file set
                if (sh_cb_tpp_encounter_split.Checked)
                {
                    if (code_type != "CTV3")
                    {
                        log_file.WriteLine("!!! WARNING: Creating ENCOUNTERS for non CTV3 system");
                        MessageBox.Show("!!! WARNING: Creating ENCOUNTERS for non CTV3 system");
                    }
                    sh_create_encounter_files();
                }

                if (sh_cb_write_code_list.Checked)
                {
                    log_file.WriteLine("Maximum query order number: " + max_order);
                }

                log_file.Close();
                if (sh_cb_write_code_list.Checked)
                {
                    // ** Read Code Use Report
                    code_include_list.Close();
                    code_exclude_list.Close();
                    //qry_order.Close();
                }
                // Make sure the progress bar is full
                sh_prog_bar.Value = 100;

                //MessageBox.Show("HQL Files Complete");
            }
        }

        // -------------------------------------------------------------------------------------------------------------------
        // Create Encounter files (for CTV3 TPP systems) NOTE- WORKS FOR CTV3 ONLY AT PRESENT
        // -------------------------------------------------------------------------------------------------------------------

        private void sh_create_encounter_files()
        {
            string inpath = sh_tb_infolder.Text;
            string outpath = sh_tb_outfolder.Text;

            if (!outpath.EndsWith("\\")) outpath = outpath + "\\";
            if (!inpath.EndsWith("\\")) inpath = inpath + "\\";

            outpath = outpath + "ENCOUNTERS\\";
            Directory.CreateDirectory(outpath);

            int export_period = Convert.ToInt32(sh_tb_export_period.Text);
            DateTime end_date = Convert.ToDateTime(SH_dp_end_date.Text);
            DateTime start_date = end_date.AddMonths(0 - export_period);

            // Copy the population files
            DirectoryInfo di = new DirectoryInfo(inpath);
            FileInfo[] rgFiles = di.GetFiles("CON*.TPL");
            foreach (FileInfo fi in rgFiles)
            {
                string dest = outpath + fi.Name;
                string source = fi.FullName;
                dest = dest.Replace(".TPL", ".HQL");
                StreamWriter outfile = new StreamWriter(dest);
                StreamReader infile = new StreamReader(source);

                string line = infile.ReadLine();
                while (line != null)
                {
                    // Replace the text
                    line = line.Replace("<practice_code>", sh_tb_practice_code.Text.Trim());
                    outfile.WriteLine(line);
                    line = infile.ReadLine();
                }
                infile.Close();
                outfile.Close();
            }

            int cnt = 1;
            int order = 10;
            string title;
            while (start_date < end_date)
            {
                string sdate = String.Format("{0:dd/MM/yyyy}", start_date);
                // WEEK string edate = String.Format("{0:dd/MM/yyyy}", start_date.AddDays(Convert.ToInt32(sh_tb_encounter_split.Text) * 7));
                string edate = String.Format("{0:dd/MM/yyyy}", start_date.AddDays(Convert.ToInt32(sh_tb_encounter_split.Text)));
                // G file
                title = "ENC0" + cnt.ToString("d2");
                StreamWriter outfile = new StreamWriter(outpath + title + "G.HQL");
                outfile.WriteLine("*QRY_WDATE,20110523,23/05/2011");
                outfile.WriteLine("*QRY_SDATE,20110201,01/02/2011");
                outfile.WriteLine("*QRY_TITLE," + title + "G,Subset");
                outfile.WriteLine("*QRY_ORDER," + order + ",");
                outfile.WriteLine("*QRY_MEDIA,D,DISK");
                outfile.WriteLine("*QRY_AGREE,LOCAL,");
                outfile.WriteLine("*ENQ_IDENT,LOCAL,");
                outfile.WriteLine("*QRY_SETID,CTV3, CTV3 set");
                outfile.WriteLine("*ENQ_RSPID," + sh_tb_practice_code.Text.Trim() + ",");
                outfile.WriteLine("*QRY_CODES,0,9999R3,Read Version 3");
                outfile.WriteLine("FOR CON003G");
                outfile.WriteLine("SUBSET " + title + " TEMP");
                outfile.WriteLine("FROM ENCOUNTERS (ONE FOR PATIENT)");
                outfile.WriteLine("WHERE ACTIVE IN (\"R\")");
                outfile.WriteLine("AND DATE IN (\"" + sdate + "\"-\"" + edate + "\")");
                outfile.Close();
                order++;

                // R file
                StreamWriter rfile = new StreamWriter(outpath + "ENC0" + cnt.ToString("d2") + "R.HQL");
                rfile.WriteLine("*QRY_WDATE,20110523,23/05/2011");
                rfile.WriteLine("*QRY_SDATE,20110201,01/02/2011");
                rfile.WriteLine("*QRY_TITLE," + title + "R,Subset");
                rfile.WriteLine("*QRY_ORDER," + order + ",");
                rfile.WriteLine("*QRY_MEDIA,D,DISK");
                rfile.WriteLine("*QRY_AGREE,LOCAL,");
                rfile.WriteLine("*ENQ_IDENT,LOCAL,");
                rfile.WriteLine("*QRY_SETID,CTV3, CTV3 set");
                rfile.WriteLine("*ENQ_RSPID," + sh_tb_practice_code.Text.Trim() + ",");
                rfile.WriteLine("*QRY_CODES,0,9999R3,Read Version 3");
                rfile.WriteLine("FOR " + title);
                rfile.WriteLine("REPORT");
                rfile.WriteLine("PRINT NHS_NUMBER,REFERENCE,DATE,HCP,HCP_TYPE,SESSION,LOCATION");
                rfile.WriteLine("FROM ENCOUNTERS (ALL FOR PATIENT)");
                rfile.WriteLine("WHERE ACTIVE IN (\"R\")");
                rfile.WriteLine("AND DATE IN (\"" + sdate + "\"-\"" + edate + "\")");
                rfile.Close();
                order++;

                // WEEK start_date = start_date.AddDays(Convert.ToInt32(sh_tb_encounter_split.Text) * 7);
                start_date = start_date.AddDays(Convert.ToInt32(sh_tb_encounter_split.Text));
                start_date = start_date.AddDays(1);
                cnt++;
            }
        }

        void ReadHeader(string filepath)
        // Read a header to get the practice code and check format
        {
            string line;
            string[] pc;

            StreamReader hfile = new StreamReader(filepath);
            line = hfile.ReadLine();
            while (line != null)
            {
                line = line.Trim();

                if (line.StartsWith("*QRY_SETID"))
                {
                    pc = line.Split(',');
                    code_type = pc[1].Trim();
                }

                line = hfile.ReadLine();
            }
            hfile.Close();
        }

        private void sh_bt_miquest_response_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = false;
            folderDlg.SelectedPath = sh_tb_infolder.Text;
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                sh_tb_infolder.Text = folderDlg.SelectedPath;
            }
        }

        private void sh_bt_output_folder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            folderDlg.SelectedPath = sh_tb_outfolder.Text;
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                sh_tb_outfolder.Text = folderDlg.SelectedPath;
            }
        }

        private void Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            string app_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            if (!Directory.Exists(app_path))
            {
                Directory.CreateDirectory(app_path);
            }

            if (!app_path.EndsWith("\\")) app_path = app_path + "\\";
            app_path = app_path + "MIQUESTUpdater.ini";

            StreamWriter ini_file = new StreamWriter(app_path);

            ini_file.WriteLine("REPORT_PATH = " + sh_tb_infolder.Text);
            ini_file.WriteLine("OUTPUT_PATH = " + sh_tb_outfolder.Text);
            ini_file.WriteLine("EXPORT_PERIOD = " + sh_tb_export_period.Text);
            ini_file.WriteLine("QUERY_SPLIT = " + sh_tb_query_split.Text);
            ini_file.WriteLine("PRACTICE_CODE = " + sh_tb_practice_code.Text);
            ini_file.WriteLine("LTC_EXPORT_PERIOD = " + sh_tb_ltc_export_period.Text);
            ini_file.WriteLine("POPULATION = " + sh_tb_population.Text);
            ini_file.WriteLine("TPP_ENCOUNTER_SPLIT = " + Convert.ToString(sh_cb_tpp_encounter_split.Checked));
            ini_file.WriteLine("INITIAL_LOAD = " + Convert.ToString(sh_cb_initial_load.Checked));
            ini_file.WriteLine("ENCOUNTER_SPLIT = " + sh_tb_encounter_split.Text);
            ini_file.WriteLine("CODE_LIST = " + Convert.ToString(sh_cb_write_code_list.Checked));
           
            ini_file.Close();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            string app_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (!app_path.EndsWith("\\")) app_path = app_path + "\\";
            app_path = app_path + "MIQUESTUpdater.ini";
            string line;
            string[] bits;

            if (File.Exists(app_path))
            {
                StreamReader ini_file = new StreamReader(app_path);

                line = ini_file.ReadLine();
                while (line != null)
                {
                    line = line.Trim();
                    if (!line.StartsWith("#"))
                    {
                        bits = line.Split('=');
                        if (bits[0].Trim() == "REPORT_PATH")
                        {
                            sh_tb_infolder.Text = bits[1].Trim();
                        }
                        else if (bits[0].Trim() == "OUTPUT_PATH")
                        {
                            sh_tb_outfolder.Text = bits[1].Trim();
                        }
                        else if (bits[0].Trim() == "EXPORT_PERIOD")
                        {
                            sh_tb_export_period.Text = bits[1].Trim();
                        }
                        else if (bits[0].Trim() == "QUERY_SPLIT")
                        {
                            sh_tb_query_split.Text = bits[1].Trim();
                        }
                        else if (bits[0].Trim() == "PRACTICE_CODE")
                        {
                            sh_tb_practice_code.Text = bits[1].Trim();
                        }
                        else if (bits[0].Trim() == "ENCOUNTER_SPLIT")
                        {
                            sh_tb_encounter_split.Text = bits[1].Trim();
                        }
                        else if (bits[0].Trim() == "LTC_EXPORT_PERIOD")
                        {
                            sh_tb_ltc_export_period.Text = bits[1].Trim();
                        }
                        else if (bits[0].Trim() == "POPULATION")
                        {
                            sh_tb_population.Text = bits[1].Trim();
                        }
                        else if (bits[0].Trim() == "TPP_ENCOUNTER_SPLIT")
                        {
                            sh_cb_tpp_encounter_split.Checked = Convert.ToBoolean(bits[1].Trim());
                        }
                        else if (bits[0].Trim() == "INITIAL_LOAD")
                        {
                            sh_cb_initial_load.Checked = Convert.ToBoolean(bits[1].Trim());
                        }
                        else if (bits[0].Trim() == "CODE_LIST")
                        {
                            sh_cb_write_code_list.Checked = Convert.ToBoolean(bits[1].Trim());
                        }
                        
                        line = ini_file.ReadLine();
                    }
                }
                ini_file.Close();
            }
        }

        private void sh_tb_query_split_TextChanged(object sender, EventArgs e)
        {
            sh_tb_export_period.KeyPress += new KeyPressEventHandler(sh_tb_query_split_KeyPress);
        }

        private void sh_tb_query_split_KeyPress(object sender, KeyPressEventArgs e)
        {
            //accept only numbers and back space.
            if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b'))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void sh_tb_export_period_TextChanged(object sender, EventArgs e)
        {
            sh_tb_export_period.KeyPress += new KeyPressEventHandler(sh_tb_export_period_KeyPress);
        }

        private void sh_tb_export_period_KeyPress(object sender, KeyPressEventArgs e)
        {
            //accept only numbers and back space.
            if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar != '\b'))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void sh_tb_encounter_split_TextChanged(object sender, EventArgs e)
        {
            sh_tb_export_period.KeyPress += new KeyPressEventHandler(sh_tb_export_period_KeyPress);
        }

        private void sh_cb_tpp_encounter_split_CheckedChanged(object sender, EventArgs e)
        {
            if (sh_cb_tpp_encounter_split.Checked)
            {
                sh_tb_encounter_split.Enabled = true;
            }
            else
            {
                sh_tb_encounter_split.Enabled = false;
            }
        }

        private void sh_cb_initial_load_CheckedChanged(object sender, EventArgs e)
        {
            if (sh_cb_initial_load.Checked)
            {
                sh_tb_ltc_export_period.Enabled = true;
            }
            else
            {
                sh_tb_ltc_export_period.Enabled = false;
            }
        }

        private void sh_bt_suggest_splits_Click(object sender, EventArgs e)
        {
            if (sh_tb_population.Text.Trim() != "")
            {
                long pop = Convert.ToInt32(sh_tb_population.Text);

                if (pop > 20000)
                {
                    sh_tb_query_split.Text = "1"; // months
                    sh_tb_encounter_split.Text = "1"; // weeks
                }
                else if (pop > 15000)
                {
                    sh_tb_query_split.Text = "2"; // months
                    sh_tb_encounter_split.Text = "2"; // weeks
                }
                else if (pop > 10000)
                {
                    sh_tb_query_split.Text = "3"; // months
                    sh_tb_encounter_split.Text = "2"; // weeks
                }
                else if (pop > 8000)
                {
                    sh_tb_query_split.Text = "3"; // months
                    sh_tb_encounter_split.Text = "2"; // weeks
                }
                else
                {
                    sh_tb_query_split.Text = "6"; // months
                    sh_tb_encounter_split.Text = "4"; // weeks
                }
            }

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

    }
}
