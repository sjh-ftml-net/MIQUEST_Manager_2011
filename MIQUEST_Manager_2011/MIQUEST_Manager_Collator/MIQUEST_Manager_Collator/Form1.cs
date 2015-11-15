/*
 * Collate a set of MIQUEST response files into Capita ACG formats
 * 
 * History
 * 01/08/2011: V1.0 First test release
 * 08/08/2011: V1.1 Corrected error, missing 'GP' column added to standard output
 * 31/08/2011: V1.2 Corrected Encounters processing (TPP specific)
 * 05/09/2011: V1.3 Creates A2 folder structure
 * 05/09/2011: V1.4 Updated for Sollis header rules
 * 19/09/2011: V1.5 Updated again for Sollis header rules
 * 29/09/2011: V1.6 Added more error checking (after Vision Tests)
 * 30/09/2011: V1.7 Fixed PRIMIS 5Byte had different fields in LTC
 * 30/09/2011: V1.8 Allowed practice code override for system sharing practices
 * 11/09/2012: V1.9 Added INITIAL/BULK override
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
//using System.Security.Cryptography;

namespace MIQUEST_Manager_Collator
{
    public partial class Form1 : Form
    {

        #region VARS

        private enum cols_patients : int
        {
            C1, NHS_NUMBER, REFERENCE, SEX, DATE_OF_BIRTH, POSTCODE, GP_USUAL, REGISTERED_DATE, REMOVED_DATE, DATE_OF_DEATH, ACTIVE, PCODE, _END
        }
        int cnt_nullnhs = 0;
        int cnt_pat_death = 0;
        int cnt_pat_deducted = 0;
        int cnt_pat_private = 0;
        int cnt_pat_reg_non_gms = 0;
        int cnt_pat_temp = 0;
        int cnt_pat_reg = 0;

        int malformed_events = 0;
        int malformed_encounters = 0;
        int malformed_patients = 0;

        bool initial_load = false;

        
        private enum cols_encounters : int
        {
            C1, NHS_NUMBER, REFERENCE, DATE, HCP, HCP_TYPE, SESSION, LOCATION, PCODE, _END
        }
        long enc_count = 0;
        //DateTime first_enc = DateTime.Now;
        //DateTime last_enc = Convert.ToDateTime("1800-01-01");
        const string delimiter = ",";

        string practice_code;
        string query_date;
        int prog_inc = 100 / 4; // Progress bar increment
        string warning = "";

        // Events
        long cnt_diagnoses = 0;
        long cnt_drugs = 0;
        long cnt_ltc = 0;
        long cnt_qof = 0;
        int cnt_event = 0;
        //DateTime first_event = DateTime.Now;
        //DateTime last_event = Convert.ToDateTime("1800-01-01");

        // Stores for standard output
        static Hashtable std_patient_list = new Hashtable();
        static Hashtable std_encounter_list = new Hashtable();
        static Hashtable std_event_list = new Hashtable();

        // Stores for ctron output
        static Hashtable ctron_patient_list = new Hashtable();
        static Hashtable ctron_encounter_list = new Hashtable();

        // Pseudo store
        //static Hashtable pseuds = new Hashtable();
        // Maybe check the item is here, if it is use the index (or something), if not add it and use.

        #endregion


        public Form1()
        {
            InitializeComponent();

            if (sh_cb_practice_code_override.Checked)
                sh_tb_practice_code.Enabled = true;
            else
                sh_tb_practice_code.Enabled = false;
        }

        // ---- Main Operation ----
        private void sh_bt_run_Click(object sender, EventArgs e)
        {
            string inpath = sh_tb_folder_path_response.Text;
            string outpath = sh_tb_folder_path_output.Text;
            bool fatal = false;
            string logfile_path;

            // Initialise lists etc
            std_patient_list.Clear();
            std_encounter_list.Clear();
            std_event_list.Clear();

            ctron_patient_list.Clear();
            ctron_encounter_list.Clear();

            sh_lb_status_message.Text = "Started";
            sh_progress_bar.Value = 0;
            warning = "";
            initial_load = false;
            malformed_events = 0;
            malformed_encounters = 0;
            malformed_patients = 0;

            // Check for '\' on paths
            if (!inpath.EndsWith("\\")) inpath = inpath + "\\";
            if (!outpath.EndsWith("\\")) outpath = outpath + "\\";

            // Check we have the folders
            if (!Directory.Exists(outpath))
            {
                switch (MessageBox.Show("The output folder was not found (" + outpath + "), create the folder?",
                                            "Output folder not found",
                                            MessageBoxButtons.YesNoCancel,
                                            MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        Directory.CreateDirectory(outpath);
                        break;
                    case DialogResult.No:
                        fatal = true;
                        break;
                    case DialogResult.Cancel:
                        fatal = true;
                        break;
                }
            }
            //else
            //{
            //    // Clean out the target folder
            //    DirectoryInfo target = new DirectoryInfo(outpath);

            //    foreach (FileInfo file in target.GetFiles())
            //    {
            //        file.Delete();
            //    }
            //    foreach (DirectoryInfo dir in target.GetDirectories())
            //    {
            //        dir.Delete(true);
            //    }
            //}

            // ---- MAIN LOOP ----
            if (!fatal)
            {
                Process_Header(inpath);

                // Create the Sollis A2 folder structure (use todays date)
                string a2path = DateTime.Today.Year + "\\" + DateTime.Today.Month + "\\" + DateTime.Today.Day;
                outpath = outpath + a2path;
                if (!Directory.Exists(outpath))
                {
                    Directory.CreateDirectory(outpath);
                }
                if (!outpath.EndsWith("\\")) outpath = outpath + "\\";

                // Create logfile
                logfile_path = outpath + practice_code + "_" + query_date + "_" + String.Format("{0:yyyyMMdd}", DateTime.Today) + "_log.txt";
                StreamWriter log_file = new StreamWriter(logfile_path);

                if (sh_cb_practice_code_override.Checked)
                {
                    practice_code = sh_tb_practice_code.Text.Trim();
                    log_file.WriteLine("Overriding the practice code, new code is " + practice_code);
                }

                log_file.WriteLine("Starting MIQUEST result collation at " + DateTime.Now);
                log_file.WriteLine("From " + inpath);
                log_file.WriteLine("To " + outpath);
                log_file.WriteLine("Practice Code " + practice_code);
                log_file.WriteLine("Query Date " + query_date);

                // Process the patients ------------------------------------------------------------------*
                // Read all folders under the inpath
                log_file.WriteLine("---- Processing Patients ---- ");
                string pat_file_name = outpath + practice_code + "_" + query_date + "_patients.txt";
                Process_Patient_Files(inpath, log_file);

                sh_progress_bar.Value = sh_progress_bar.Value + prog_inc;

                // Process the encounters ----------------------------------------------------------------*
                // Need to check if this has split the encounters into a seperate folder
                string enc_file_name;
                if (Directory.Exists(inpath + "ENCOUNTERS\\"))
                {
                    // This just reads the ENCOUNTERS folder
                    enc_count = 0;
                    enc_file_name = outpath + practice_code + "_" + query_date + "_encounters.txt";
                    log_file.WriteLine("---- Processing Encounters FOLDER ---- ");
                    Process_Encounter_Files(inpath + "ENCOUNTERS\\", log_file);
                }
                else
                {
                    // This reads the ENCOUNTERS files from all subfolders
                    enc_count = 0;
                    enc_file_name = outpath + practice_code + "_" + query_date + "_encounters.txt";
                    log_file.WriteLine("---- Processing Encounters INLINE ---- ");
                    Process_Encounter_Files_Inline(inpath, log_file);
                }
                sh_progress_bar.Value = sh_progress_bar.Value + prog_inc;

                // Process Events (Diagnoses, Drugs, LTCS and QOF) --------------------------------------*
                // Read all folders under the inpath
                cnt_event = 0;
                string evt_file_name = outpath + practice_code + "_" + query_date + "_events.txt";
                StreamWriter event_file = new StreamWriter(evt_file_name);
                log_file.WriteLine("---- Start Processing Events ");
                event_file.WriteLine("&1,NHS_NUMBER,REFERENCE,HCP,HCP_TYPE,CODE,RUBRIC,DATE,PCODE,TYPE");
                Process_Event_Files(inpath, event_file, log_file);
                event_file.Close();
                sh_progress_bar.Value = sh_progress_bar.Value + prog_inc;

                // Not necessary for current implementation
                // Process Encounters from Journal File -------------------------------------------------
                // Read all folders under the inpath
                //string jnenc_file_name = outpath + practice_code + "_" + query_date + "_jnenc.txt";
                //StreamWriter jn_enc_file = new StreamWriter(jnenc_file_name);
                //log_file.WriteLine("---- Processing Encounters from Journals ---- ");
                //jn_enc_file.WriteLine("&1,NHS_NUMBER,REFERENCE,CODE,DATE,HCP,HCP_TYPE,PCODE");
                //Process_Jn_Event_Files(inpath, jn_enc_file, log_file);
                //jn_enc_file.Close();
                //sh_progress_bar.Value = sh_progress_bar.Value + prog_inc;

                string exp_type;
                if (initial_load)
                {
                    exp_type = "Initial";
                }
                else
                {
                    exp_type = "Bulk";
                }

                // Write the output files (do this last because we need the first/last dates)
                // Patient
                //write_ctron_patient_file(pat_file_name, log_file);
                write_standard_patient_file(outpath + practice_code + "_Tribal_-_" + exp_type + "_Patients_1.csv", log_file);
                // Log a few counts
                log_file.WriteLine("Patient Count: " + std_patient_list.Count);
                log_file.WriteLine("NULL NHS Number: " + cnt_nullnhs);
                log_file.WriteLine("Death: " + cnt_pat_death);
                log_file.WriteLine("Deducted: " + cnt_pat_deducted);
                log_file.WriteLine("Private: " + cnt_pat_private);
                log_file.WriteLine("Non GMS: " + cnt_pat_reg_non_gms);
                log_file.WriteLine("Temp: " + cnt_pat_temp);
                log_file.WriteLine("GMS: " + cnt_pat_reg);
                std_patient_list.Clear();
                ctron_patient_list.Clear();

                // Encounters
                write_standard_encounter_file(outpath + practice_code + "_Tribal_-_" + exp_type + "_Encounters_1.csv", log_file);
                //write_ctron_encounter_file(enc_file_name, log_file);
                std_encounter_list.Clear();
                ctron_encounter_list.Clear();

                // Events
                write_standard_events_file(outpath + practice_code + "_Tribal_-_" + exp_type + "_Events_1.csv", log_file);
                std_event_list.Clear();


                // Log some details of encounters
                log_file.WriteLine("Total encounters: " + enc_count);
                //log_file.WriteLine("First encounter: " + first_enc);
                //log_file.WriteLine("Last encounter: " + last_enc);

                log_file.WriteLine("Total events: " + cnt_event);

                log_file.WriteLine("Malformed Patients: " + malformed_patients);
                log_file.WriteLine("Malformed Events: " + malformed_events);
                log_file.WriteLine("Malformed Encounters: " + malformed_encounters);

                // Processing Complete --------------------------------------------------------------------
                log_file.WriteLine("Completed MIQUEST result collation at " + DateTime.Now);          
                log_file.Close();
                             
                //if (SH_CB_Save_MIQUEST_CSV.Checked == false)
                //{
                    File.Delete(pat_file_name);
                    File.Delete(enc_file_name);
                    File.Delete(evt_file_name);
                    //File.Delete(jnenc_file_name);
                //}
                if (SH_CB_Save_Logfile.Checked == false)
                {
                    File.Delete(logfile_path);
                }
            }
            // ---- MAIN LOOP ----

            // Show we've finished
            sh_progress_bar.Value = 100;
            sh_lb_status_message.Text = "Completed Processing";
            if (warning != "")
            {
                MessageBox.Show("Completed with warnings: " + warning);
            }
        }

        // ------------------------------------------------------------------------------------------------------------
        // ---- JOURNAL EVENTS ----------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------
        #region Journal Events
        void Process_Jn_Event_Files(string sDir, StreamWriter event_file, StreamWriter log_file)
        {
            FileInfo[] rgFiles;
            // Process all folders
            DirectoryInfo di = new DirectoryInfo(sDir);

            // Process Diagnoses files
            log_file.WriteLine("---- Processing Journal Encounters ---- ");
            rgFiles = di.GetFiles("ENC002R.CSV", SearchOption.AllDirectories);
            foreach (FileInfo fi in rgFiles)
            {
                if(!fi.FullName.Contains("ENCOUNTERS")) // Don't process the encounters folder
                {
                    log_file.WriteLine("Processing: " + fi.FullName);
                    sh_lb_status_message.Text = fi.Name;
                    if (!sDir.EndsWith("\\")) sDir = sDir + "\\";
                    Process_Jn_Event_File(fi.FullName, event_file, log_file);
                }
            }

        }

        void Process_Jn_Event_File(string fname, StreamWriter event_file, StreamWriter log_file)
        {

            StreamReader infile = new StreamReader(fname);

            string line = infile.ReadLine();
            while (line != null)
            {


                if (line.StartsWith("$1"))
                {
                    if (line.IndexOf("There are too many rows in the response") != -1)
                    {
                        log_file.WriteLine("WARNING: " + fname + ", " + line);
                        warning = "too many rows warning encountered, please look at the log file";
                    }
                    else
                    {
                        // Write to a simple output file
                        event_file.WriteLine(line + "," + quote_field(practice_code));
                    }
                }

                line = infile.ReadLine();
            }
            infile.Close();
        }
        #endregion

        // ------------------------------------------------------------------------------------------------------------
        // ---- EVENTS ------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------
        #region Events
        void Process_Event_Files(string sDir, StreamWriter event_file, StreamWriter log_file)
        {
            FileInfo[] rgFiles;
            // Process all folders
            DirectoryInfo di = new DirectoryInfo(sDir);

            // Process Diagnoses files
            log_file.WriteLine("---- Processing Diagnoses ---- ");
            rgFiles = di.GetFiles("DIA*R.CSV", SearchOption.AllDirectories);
            foreach (FileInfo fi in rgFiles)
            {
                log_file.WriteLine("Processing: " + fi.FullName);
                sh_lb_status_message.Text = fi.Name;
                if (!sDir.EndsWith("\\")) sDir = sDir + "\\";
                Process_Event_File(fi.FullName, event_file, log_file);
            }

            // Process Drugs
            log_file.WriteLine("---- Processing Drugs ---- ");
            rgFiles = di.GetFiles("DRU*R.CSV", SearchOption.AllDirectories);
            foreach (FileInfo fi in rgFiles)
            {
                log_file.WriteLine("Processing: " + fi.FullName);
                sh_lb_status_message.Text = fi.Name;
                if (!sDir.EndsWith("\\")) sDir = sDir + "\\";
                Process_Event_File(fi.FullName, event_file, log_file);
            }

            // Process LTC
            log_file.WriteLine("---- Processing LTC ---- ");
            rgFiles = di.GetFiles("LTC*R.CSV", SearchOption.AllDirectories);
            if (rgFiles.Length > 0)
            {
                // This must be an Initial Load as there are LTC files
                initial_load = true;
            }
            foreach (FileInfo fi in rgFiles)
            {
                log_file.WriteLine("Processing: " + fi.FullName);
                sh_lb_status_message.Text = fi.Name;
                if (!sDir.EndsWith("\\")) sDir = sDir + "\\";
                Process_Event_File(fi.FullName, event_file, log_file);
            }

            // Process QOF
            log_file.WriteLine("---- Processing QOF ---- ");
            rgFiles = di.GetFiles("QOF*R.CSV", SearchOption.AllDirectories);
            foreach (FileInfo fi in rgFiles)
            {
                log_file.WriteLine("Processing: " + fi.FullName);
                sh_lb_status_message.Text = fi.Name;
                if (!sDir.EndsWith("\\")) sDir = sDir + "\\";
                Process_Event_File(fi.FullName, event_file, log_file);
            }

        }

        void Process_Event_File(string fname, StreamWriter event_file, StreamWriter log_file)
        {
            string standard_line = "";
            //DateTime dt;

            StreamReader infile = new StreamReader(fname);

            //string tmp;
            //string edate;
            string key;
            string file_type = "";
            bool fatal = false;

            if (fname.Contains("DIA"))
                file_type = "DIA";
            else if (fname.Contains("DRU"))
                file_type = "DRU";
            else if (fname.Contains("LTC"))
                file_type = "LTC";
            else if (fname.Contains("QOF"))
                file_type = "QOF";
            
            string line = infile.ReadLine();
            int line_no = 1;
            while (line != null && !fatal)
            {
                if (line.StartsWith("&0"))
                {
                    if (line.IndexOf("ERROR") != -1 || line.IndexOf("REJECTED") != -1)
                    {
                        log_file.WriteLine("ERROR: " + fname + ", " + line);
                        warning = "Error in response, please look at the log file";
                        fatal = true;
                    }
                }
                else if (line.StartsWith("$1"))
                {
                    if (line.IndexOf("There are too many rows in the response") != -1)
                    {
                        log_file.WriteLine("ERROR: " + fname + ", " + line);
                        warning = "too many rows warning encountered, please look at the log file";
                    }
                    if (line.IndexOf("Syntax error") != -1)
                    {
                        log_file.WriteLine("ERROR: " + fname + ", " + line);
                        warning = "HQL syntax error encountered, please look at the log file";
                    }
                    else
                    {
                        //&1,NHS_NUMBER,REFERENCE,HCP,HCP_TYPE,CODE,RUBRIC,DATE
                        //0 ,1         ,2        ,3  ,4       ,5   ,6     ,7

                        // In 5Btye LTC !!
                        // &1,NHS_NUMBER,PRACT_NUMBER,CODE,DATE
                        // 0 ,1         ,2           ,3   ,4
                        string[] fields = line.Split(',');

                        if (fields.Length != 8 && fields.Length != 5)
                        {
                            warning = "MALFORMED data encountered, please look at the logfile";
                            log_file.WriteLine("WARNING: MALFORMED data at line " + line_no + " in " + fname);
                            malformed_events++;
                        }
                        else
                        {
                            string nhs_num = clean_field(fields[1], null);

                            // Write to a simple output file
                            event_file.WriteLine(line + "," + quote_field(practice_code) + "," + quote_field(file_type));


                            // Process to the 'standard' format
                            if (nhs_num != "")
                            {

                                if (fields.Length == 8)
                                {
                                    standard_line = quote_field(clean_field(fields[1], null));                          // NHS Number
                                    standard_line = standard_line + "," + quote_field(clean_field(fields[7], null));    // Event Date
                                    standard_line = standard_line + "," + quote_field(clean_field(fields[3], null));    // HCP
                                    standard_line = standard_line + "," + quote_field(clean_field(fields[4], null));    // HCP_Type
                                    standard_line = standard_line + "," + quote_field(clean_field(fields[7], null));    // Recorded Date
                                    standard_line = standard_line + "," + quote_field(clean_field(fields[5], null));    // Code
                                    standard_line = standard_line + "," + quote_field("");                              // Value 1
                                    standard_line = standard_line + "," + quote_field("");                              // Value 2
                                    standard_line = standard_line + "," + quote_field(clean_field(fields[7], null));    // End Date
                                    standard_line = standard_line + "," + quote_field("");                              // Prescription Type
                                }
                                else if (fields.Length == 5)
                                {
                                    // &1,NHS_NUMBER,PRACT_NUMBER,CODE,DATE
                                    // 0 ,1         ,2           ,3   ,4
                                    standard_line = quote_field(clean_field(fields[1], null));                          // NHS Number
                                    standard_line = standard_line + "," + quote_field(clean_field(fields[4], null));    // Event Date
                                    standard_line = standard_line + "," + quote_field("");                              // HCP
                                    standard_line = standard_line + "," + quote_field("");                              // HCP_Type
                                    standard_line = standard_line + "," + quote_field(clean_field(fields[4], null));    // Recorded Date
                                    standard_line = standard_line + "," + quote_field(clean_field(fields[3], null));    // Code
                                    standard_line = standard_line + "," + quote_field("");                              // Value 1
                                    standard_line = standard_line + "," + quote_field("");                              // Value 2
                                    standard_line = standard_line + "," + quote_field(clean_field(fields[4], null));    // End Date
                                    standard_line = standard_line + "," + quote_field("");                              // Prescription Type
                                }

                                if (file_type == "DIA")
                                {
                                    standard_line = standard_line + "," + quote_field("C");    // Event Type
                                    cnt_diagnoses++;
                                }
                                else if (file_type == "DRU")
                                {
                                    standard_line = standard_line + "," + quote_field("P");    // Event Type
                                    cnt_drugs++;
                                }
                                else if (file_type == "LTC")
                                {
                                    standard_line = standard_line + "," + quote_field("C");    // Event Type
                                    cnt_ltc++;
                                }
                                else if (file_type == "QOF")
                                {
                                    standard_line = standard_line + "," + quote_field("C");    // Event Type
                                    cnt_qof++;
                                }

                                // Hold dates
                                //tmp = clean_field(fields[7], null);
                                //edate = tmp.Substring(0, 4) + "-" + tmp.Substring(4, 2) + "-" + tmp.Substring(6, 2);
                                //dt = Convert.ToDateTime(edate);
                                //if (first_event > dt) first_event = dt;
                                //if (last_event < dt) last_event = dt;

                                // Save diagnoses
                                key = Convert.ToString(cnt_event++);
                                std_event_list.Add(key, standard_line);
                            }
                        }
                    }
                }

                line = infile.ReadLine();
                line_no++;
            }
            infile.Close();

        }

        void write_standard_events_file(string filename, StreamWriter log_file)
        {
            log_file.WriteLine("---- Writing Standard Event Data");
            // Log some details
            log_file.WriteLine("Total events: " + cnt_event);
            //log_file.WriteLine("First event: " + first_event);
            //log_file.WriteLine("Last event: " + last_event);
            log_file.WriteLine("Total diagnoses: " + cnt_diagnoses);
            log_file.WriteLine("Total drugs: " + cnt_drugs);
            log_file.WriteLine("Total LTC: " + cnt_ltc);
            log_file.WriteLine("Total QOF: " + cnt_qof);

            // Write standard File
            StreamWriter enc_file = new StreamWriter(filename);

            // Write header
            //write_standard_header(enc_file, query_date, query_date, String.Format("{0:yyyyMMdd}", first_event), practice_code);
            write_standard_header(enc_file);

            foreach (DictionaryEntry p in std_event_list)
            {
                enc_file.WriteLine(p.Value);
            }
            enc_file.Close();
            log_file.WriteLine("---- Completed Standard Event Data ----");
        }
#endregion

        // ------------------------------------------------------------------------------------------------------------
        // ---- ENCOUNTERS --------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------
        #region Encounters

        string fmt_ctron_encounter_file(string line)
        // ------------------------------------------------------------------------------------------------------------
        // Format the raw MIQUEST Line to the standard form
        // ------------------------------------------------------------------------------------------------------------
        {
            // &1,NHS_NUMBER,REFERENCE,DATE,HCP,HCP_TYPE,SESSION,LOCATION,PCODE                           

            string[] fields = line.Split(',');

            string standard_line = fmt_field(query_date, "DATE"); // QUERY DATE
            standard_line = standard_line + "," + fmt_field(fields[Convert.ToInt16(cols_encounters.NHS_NUMBER)], null);
            standard_line = standard_line + "," + fmt_field(fields[Convert.ToInt16(cols_encounters.REFERENCE)], null);
            standard_line = standard_line + "," + fmt_field(fields[Convert.ToInt16(cols_encounters.DATE)], "DATE");
            standard_line = standard_line + "," + fmt_field(fields[Convert.ToInt16(cols_encounters.HCP)], null);
            standard_line = standard_line + "," + fmt_field(fields[Convert.ToInt16(cols_encounters.HCP_TYPE)], null);
            standard_line = standard_line + "," + fmt_field(fields[Convert.ToInt16(cols_encounters.SESSION)], null);
            standard_line = standard_line + "," + fmt_field(fields[Convert.ToInt16(cols_encounters.LOCATION)], null);
            standard_line = standard_line + "," + fmt_field(fields[Convert.ToInt16(cols_encounters.PCODE)], null);

            // Hold dates
            //string tmp = clean_field(fields[3], null);
            //string edate = tmp.Substring(0, 4) + "-" + tmp.Substring(4, 2) + "-" + tmp.Substring(6, 2);
            //DateTime dt = Convert.ToDateTime(edate);
            //if (first_enc > dt) first_enc = dt;
            //if (last_enc < dt) last_enc = dt;

            return standard_line;
        }

        string fmt_std_encounter_file(string line)
        // ------------------------------------------------------------------------------------------------------------
        // Format the raw MIQUEST Line to the standard form
        // ------------------------------------------------------------------------------------------------------------
        {
            // &1,NHS_NUMBER,REFERENCE,DATE,HCP,HCP_TYPE,SESSION,LOCATION                           

            string[] fields = line.Split(',');

            string standard_line = quote_field(clean_field(fields[Convert.ToInt16(cols_encounters.NHS_NUMBER)], null));
            standard_line = standard_line + "," + quote_field(clean_field(fields[Convert.ToInt16(cols_encounters.DATE)], null)); 
            standard_line = standard_line + "," + quote_field(clean_field(fields[Convert.ToInt16(cols_encounters.HCP)], null)); 
            standard_line = standard_line + "," + quote_field(clean_field(fields[Convert.ToInt16(cols_encounters.HCP_TYPE)], null)); 
            standard_line = standard_line + "," + quote_field(clean_field(fields[Convert.ToInt16(cols_encounters.SESSION)], null)); 
            standard_line = standard_line + "," + quote_field(clean_field(fields[Convert.ToInt16(cols_encounters.LOCATION)], null));

            return standard_line;
        }

        void Process_Encounter_Files_Inline(string sDir, StreamWriter log_file)
        {
            // Process all subdiretcories
            DirectoryInfo di = new DirectoryInfo(sDir);
            FileInfo[] rgFiles = di.GetFiles("ENC001R.CSV", SearchOption.AllDirectories);
            foreach (FileInfo fi in rgFiles)
            {
                log_file.WriteLine("Processing: " + fi.FullName);
                sh_lb_status_message.Text = fi.Name;
                if (!sDir.EndsWith("\\")) sDir = sDir + "\\";
                Process_Encounter_File(fi.FullName, log_file);
            }
        }

        void Process_Encounter_Files(string sDir, StreamWriter log_file)
        {
            // Process the single encounters folder only
            DirectoryInfo di = new DirectoryInfo(sDir);
            FileInfo[] rgFiles = di.GetFiles("ENC*R.CSV");
            foreach (FileInfo fi in rgFiles)
            {
                log_file.WriteLine("Processing: " + fi.FullName);
                sh_lb_status_message.Text = fi.Name;
                if (!sDir.EndsWith("\\")) sDir = sDir + "\\";
                Process_Encounter_File(sDir + fi.Name, log_file);
            }
        }

        void Process_Encounter_File(string fname, StreamWriter log_file)
        {
            string standard_line = "";
            StreamReader infile = new StreamReader(fname);
            string key;

            int line_no = 1;
            string line = infile.ReadLine();
            while (line != null)
            {
                if (line.StartsWith("$1"))
                {
                    if (line.IndexOf("There are too many rows in the response") != -1)
                    {
                        log_file.WriteLine("ERROR: " + fname + ", " + line);
                        warning = "too many rows warning encountered, please look at the log file";
                    }
                    if (line.IndexOf("Syntax error") != -1)
                    {
                        log_file.WriteLine("ERROR: " + fname + ", " + line);
                        warning = "HQL syntax error encountered, please look at the log file";
                    }
                    else
                    {
                        string[] fields;

                        fields = line.Split(',');

                        if (fields.Length != Convert.ToInt16(cols_encounters.PCODE))
                        {
                            warning = "MALFORMED data at line " + line_no + " in " + fname;
                            log_file.WriteLine("WARNING: " + warning);
                            malformed_encounters++;
                        }
                        else
                        {
                            key = Convert.ToString(enc_count++);
                            // Process to the 'standard' format
                            standard_line = fmt_ctron_encounter_file(line + "," + quote_field(practice_code));
                            ctron_encounter_list.Add(key, standard_line);
                            standard_line = fmt_std_encounter_file(line);
                            std_encounter_list.Add(key, standard_line);
                        }
                    }
                }
                line = infile.ReadLine();
                line_no++;
            }
            infile.Close();
        }

        void write_standard_encounter_file(string filename, StreamWriter log_file)
        {
            log_file.WriteLine("---- Write Standard Encounter Data ----");

            // Write standard File
            StreamWriter enc_file = new StreamWriter(filename);

            // Write header
            //write_standard_header(enc_file, query_date, query_date, String.Format("{0:yyyyMMdd}", first_enc), practice_code);
            write_standard_header(enc_file);

            foreach (DictionaryEntry p in std_encounter_list)
            {
                enc_file.WriteLine(p.Value);
            }
            enc_file.Close();
        }

        void write_ctron_encounter_file(string filename, StreamWriter log_file)
        {
            log_file.WriteLine("---- Write CTRON Encounter Data ----");

            // Write standard File
            StreamWriter enc_file = new StreamWriter(filename);

            // Write header
            enc_file.WriteLine("EXPORT_DATE, NHS_NUMBER, REFERENCE, DATE, HCP, HCP_TYPE, SESSION, LOCATION, PCODE");

            foreach (DictionaryEntry p in ctron_encounter_list)
            {
                enc_file.WriteLine(p.Value);
            }
            enc_file.Close();
        }

#endregion

        // ------------------------------------------------------------------------------------------------------------
        // ---- PATIENTS ----------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------
        #region Patients 

        string fmt_ctron_patient_file(string line)
        // ------------------------------------------------------------------------------------------------------------
        // Format the raw MIQUEST Line to the ctron form
        // ------------------------------------------------------------------------------------------------------------
        {

            bool pseudo = true;
            string standard_line;

            // $1, NHS_NUMBER, REFERENCE, SEX, DATE_OF_BIRTH, POSTCODE, GP_USUAL,
            // REGISTERED_DATE, REMOVED_DATE, DATE_OF_DEATH, ACTIVE, PCODE
            string[] fields = line.Split(',');

            standard_line = fmt_field(query_date, "DATE"); // QUERY DATE
            standard_line = standard_line + delimiter + fmt_field(fields[Convert.ToInt16(cols_patients.NHS_NUMBER)], null, pseudo);
            standard_line = standard_line + delimiter + fmt_field(fields[Convert.ToInt16(cols_patients.REFERENCE)], null, pseudo);
            standard_line = standard_line + delimiter + fmt_field(fields[Convert.ToInt16(cols_patients.SEX)], "STRING");
            standard_line = standard_line + delimiter + fmt_field(fields[Convert.ToInt16(cols_patients.DATE_OF_BIRTH)], "DATE", pseudo);
            standard_line = standard_line + delimiter + fmt_field(fields[Convert.ToInt16(cols_patients.POSTCODE)], "STRING", pseudo);
            standard_line = standard_line + delimiter + fmt_field(fields[Convert.ToInt16(cols_patients.GP_USUAL)], "STRING", pseudo);
            standard_line = standard_line + delimiter + fmt_field(fields[Convert.ToInt16(cols_patients.REGISTERED_DATE)], "DATE");
            standard_line = standard_line + delimiter + fmt_field(fields[Convert.ToInt16(cols_patients.REMOVED_DATE)], "DATE");
            standard_line = standard_line + delimiter + fmt_field(fields[Convert.ToInt16(cols_patients.DATE_OF_DEATH)], "DATE", pseudo);
            standard_line = standard_line + delimiter + fmt_field(fields[Convert.ToInt16(cols_patients.ACTIVE)], "STRING");
            standard_line = standard_line + delimiter + fmt_field(fields[Convert.ToInt16(cols_patients.PCODE)], "STRING");

            return standard_line;

        }

        string fmt_std_patient_file(string line)
        // ------------------------------------------------------------------------------------------------------------
        // Format the raw MIQUEST Line to the standard form
        // ------------------------------------------------------------------------------------------------------------
        {
            //&1,NHS_NUMBER,REFERENCE,SEX,DATE_OF_BIRTH,POSTCODE,GP_USUAL,REGISTERED_DATE,REMOVED_DATE,DATE_OF_DEATH,ACTIVE                           

            string[] fields = line.Split(',');
            string standard_line;

            standard_line = quote_field(clean_field(fields[Convert.ToInt16(cols_patients.DATE_OF_BIRTH)], "DATE"));                         // Date of Birth
            standard_line = standard_line + "," + quote_field(clean_field(fields[Convert.ToInt16(cols_patients.SEX)], null));               // Sex
            standard_line = standard_line + "," + quote_field(clean_field(fields[Convert.ToInt16(cols_patients.GP_USUAL)], null));          // GP
            standard_line = standard_line + "," + quote_field(clean_field(fields[Convert.ToInt16(cols_patients.GP_USUAL)], null));          // GP
            standard_line = standard_line + "," + quote_field(clean_field(fields[Convert.ToInt16(cols_patients.ACTIVE)], null));            // Active (REG STATUS)
            standard_line = standard_line + "," + quote_field(clean_field(fields[Convert.ToInt16(cols_patients.REGISTERED_DATE)], "DATE")); // Reg Date
            standard_line = standard_line + "," + quote_field(clean_field(fields[Convert.ToInt16(cols_patients.REMOVED_DATE)], "DATE"));    // Rem Date
            standard_line = standard_line + "," + quote_field(clean_field(fields[Convert.ToInt16(cols_patients.POSTCODE)], null));          // Postcode
            standard_line = standard_line + "," + quote_field(clean_field(fields[Convert.ToInt16(cols_patients.NHS_NUMBER)], null));        // NHS Number
            standard_line = standard_line + "," + quote_field(clean_field(fields[Convert.ToInt16(cols_patients.NHS_NUMBER)], null));        // Patient ID
            standard_line = standard_line + "," + quote_field(clean_field(fields[Convert.ToInt16(cols_patients.DATE_OF_DEATH)], "DATE"));   // Date of Death

            switch (clean_field(fields[10], null))
            {
                case "D":
                    cnt_pat_death++;
                    break;
                case "L":
                    cnt_pat_deducted++;
                    break;
                case "P":
                    cnt_pat_private++;
                    break;
                case "S":
                    cnt_pat_reg_non_gms++;
                    break;
                case "T":
                    cnt_pat_temp++;
                    break;
                case "R":
                    cnt_pat_reg++;
                    break;             
            }

            return standard_line;
        }

        void Process_Patient_Files(string sDir, StreamWriter log_file)
        // ------------------------------------------------------------------------------------------------------------
        // Process the patient files (step through each)
        // ------------------------------------------------------------------------------------------------------------
        {
            // Process This folder
            //log_file.WriteLine("---- Processing: " + sDir);
            DirectoryInfo di = new DirectoryInfo(sDir);
            FileInfo[] rgFiles = di.GetFiles("PAT*R.CSV", SearchOption.AllDirectories);
            foreach (FileInfo fi in rgFiles)
            {
                log_file.WriteLine("Processing: " + fi.FullName);
                sh_lb_status_message.Text = fi.Name;
                if (!sDir.EndsWith("\\")) sDir = sDir + "\\";
                Process_Patient_File(fi.FullName, log_file);
            }
        }
        
        void Process_Patient_File(string fname, StreamWriter log_file)
        // ------------------------------------------------------------------------------------------------------------
        // Process a single patient file (load the hashtables)
        // ------------------------------------------------------------------------------------------------------------
        {
            StreamReader infile = new StreamReader(fname);
            string line = infile.ReadLine();
            int line_no = 1;

            while (line != null)
            {
                if (line.StartsWith("$1"))
                {
                    if (line.IndexOf("There are too many rows in the response") != -1)
                    {
                        log_file.WriteLine("ERROR: " + fname + ", " + line);
                        warning = "Too many rows warning encountered, please look at the log file";
                    }
                    if (line.IndexOf("Syntax error") != -1)
                    {
                        log_file.WriteLine("ERROR: " + fname + ", " + line);
                        warning = "HQL syntax error encountered, please look at the log file";
                    }
                    else
                    {
                        string[] fields = line.Split(',');
                        string nhs_num = clean_field(fields[Convert.ToInt16(cols_patients.NHS_NUMBER)], null);

                        if (fields.Length != Convert.ToInt16(cols_patients.PCODE))
                        {
                            warning = "MALFORMED data at line " + line_no + " in " + fname;
                            log_file.WriteLine("WARNING: " + warning);
                            malformed_patients++;
                        }
                        else
                        {

                            if (nhs_num == "")
                            {
                                cnt_nullnhs++;
                            }
                            else if (!std_patient_list.ContainsKey(nhs_num))
                            {

                                // Write to a ctron form
                                string ctron_line = fmt_ctron_patient_file(line + "," + practice_code);
                                ctron_patient_list.Add(nhs_num, ctron_line);

                                // Write to a standard form
                                string std_line = fmt_std_patient_file(line);
                                std_patient_list.Add(nhs_num, std_line);
                            }
                        }
                    }
                }             
                line = infile.ReadLine();
                line_no++;
           }
            infile.Close();
        }

        void write_ctron_patient_file(string filename, StreamWriter log_file)
        // ------------------------------------------------------------------------------------------------------------
        // Save the ctron hashtable to the output file
        // ------------------------------------------------------------------------------------------------------------
        {
            log_file.WriteLine("---- Write CTRON Patient Data ----");

            // Write standard File
            StreamWriter patient_file = new StreamWriter(filename);

            // Write header line
            patient_file.WriteLine("EXPORT_DATE, NHS_NUMBER, REFERENCE, SEX, DATE_OF_BIRTH, POSTCODE, GP_USUAL, REGISTERED_DATE, REMOVED_DATE, DATE_OF_DEATH, ACTIVE, PCODE");

            foreach (DictionaryEntry p in ctron_patient_list)
            {
                patient_file.WriteLine(p.Value);
            }
            patient_file.Close();
        }

        void write_standard_patient_file(string filename, StreamWriter log_file)
        {
            log_file.WriteLine("---- Writing Standard Patient Data");

            // Write standard File
            StreamWriter patient_file = new StreamWriter(filename);

            // Write header
            //write_standard_header(patient_file, query_date, query_date, query_date, practice_code);
            write_standard_header(patient_file);

            foreach (DictionaryEntry p in std_patient_list)
            {
                patient_file.WriteLine(p.Value);
            }
            patient_file.Close();
        }

        #endregion patients

        // ------------------------------------------------------------------------------------------------------------
        // ---- HEADER DATA -------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------
        #region Header

        // Read the header
        void Process_Header(string sDir)
        {
            // Process all folders, find the patient files
            DirectoryInfo di = new DirectoryInfo(sDir);
            FileInfo[] rgFiles = di.GetFiles("PAT*R.CSV", SearchOption.AllDirectories);

            // Just look at the first file
            ReadHeader(rgFiles.First().FullName);
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

                if (line.StartsWith("*ENQ_RSPID"))
                {
                    pc = line.Split(',');
                    if (pc.Length < 2)
                        MessageBox.Show("ENQ_RSPID line malformed in " + filepath);
                    practice_code = clean_field(pc[1], null);
                }
                else if (line.StartsWith("*RSP_RDATE"))
                {
                    pc = line.Split(',');
                    if (pc.Length < 2)
                        MessageBox.Show("RSP_RDATE line malformed in " + filepath);
                    query_date = clean_field(pc[1], null);
                }

                line = hfile.ReadLine();
            }
            hfile.Close();
        }

        #endregion

        // ------------------------------------------------------------------------------------------------------------
        // ---- UTILS -------------------------------------------------------------------------------------------------
        // ------------------------------------------------------------------------------------------------------------
        #region Utils

        string clean_field(string field, string type)
        // ---- Clean a field from a CSV, remove " and trim space
        {
            field = field.Replace("\"", "");
            field = field.Trim();
            if (type == "DATE")
            {
                if (field == "99999999") field = "";
            }
            return field;
        }

        string quote_field(string field)
        // Quote a field
        {
            field = "\"" + field + "\"";
            return field;
        }

        string fmt_field(string field, string type, bool pseudo = false)
        // --------------------------------
        // Format a field for CTRON output
        // --------------------------------
        {
            string result = "";
            DateTime tmpDate = Convert.ToDateTime("1800-01-01");

            // get rid of any " and spaces
            field = field.Replace("\"", "");
            field = field.Trim();

            if (type == "DATE")
            {
                if (field.Length == 8)
                {
                    string sdate = field.Substring(0, 4) + "-" + field.Substring(4, 2) + "-" + field.Substring(6, 2);

                    if (DateTime.TryParse(sdate, out tmpDate))
                        result = sdate;
                }
            }
            else if (type == "STRING")
            {
                result = field;
            }
            else
            {
                result = field;
            }

            //if (pseudo)
            //{
            //    if (result.Length > 0)
            //    {
            //        if (type == "DATE")
            //        {
            //            tmpDate = tmpDate.AddDays(5);
            //            tmpDate = tmpDate.AddMonths(-4);
            //            tmpDate = tmpDate.AddYears(2);
            //            result = String.Format("{0:yyyy-MM-dd}", tmpDate);
            //        }
            //        else
            //        {
            //            byte[] tmpSource;
            //            byte[] tmpHash;

            //            tmpSource = ASCIIEncoding.ASCII.GetBytes(result);
            //            tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
            //            result = ByteArrayToString(tmpHash);
            //        }
            //    }
            //}

            return quote_field(result);

        }

        static string ByteArrayToString(byte[] arrInput)
        // Convert a byte array to a hex string
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }

        void write_standard_header(StreamWriter file)
        {
            // The standard is weird so best to artificially generate from query_date
            // Format = BULK, Query Date, Start Date, End Date, Practice Code

            if (query_date.Length != 8)
            {
                MessageBox.Show("The query date is not correctly formatted (" + query_date + ") cannot create header");
            }
            else
            {
                string qdate = query_date.Substring(0, 4) + "-" + query_date.Substring(4, 2) + "-" + query_date.Substring(6, 2);
                DateTime qd;
                if (!DateTime.TryParse(qdate, out qd))
                {
                    MessageBox.Show("The query date is not correctly formatted (" + query_date + ") cannot create header");
                }
                else
                {
                    if (sh_cb_init_bul_override.Checked)
                    {
                        if (sh_rb_initial.Checked)
                            file.Write(quote_field("INITIAL") + ",");
                        else
                            file.Write(quote_field("BULK") + ",");
                    }
                    else
                    {
                        if (initial_load)
                            file.Write(quote_field("INITIAL") + ",");
                        else
                            file.Write(quote_field("BULK") + ",");
                    }

                    file.Write(quote_field(query_date) + ","); // Query Date
                    file.Write(quote_field(String.Format("{0:yyyyMMdd}", qd.AddMonths(-12))) + ","); // Start Date
                    file.Write(quote_field(query_date) + ","); // End Date
                    file.WriteLine(quote_field(practice_code));
                }
            }
        }

        //void write_standard_header(StreamWriter file, string qdate, string sdate, string edate, string pcode)
        //{
        //    // Always use event dates for header in ALL files
        //    // Format BULK, Query Date, Start Date, End Date, Practice Code
        //    //BULK, Query Date, Start Date, End Date, Practice Code
        //    // Ignore the passed edate/sdate

        //    sdate = String.Format("{0:yyyyMMdd}", first_event);
        //    edate = String.Format("{0:yyyyMMdd}", last_event);

        //    file.Write(quote_field("BULK") + ",");
        //    file.Write(quote_field(qdate) + ",");
        //    file.Write(quote_field(sdate) + ",");
        //    file.Write(quote_field(edate) + ",");
        //    file.WriteLine(quote_field(pcode));
        //}

        // ---- Folder Selection ----
        private void sh_bt_select_response_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = false;
            folderDlg.SelectedPath = sh_tb_folder_path_response.Text;
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                sh_tb_folder_path_response.Text = folderDlg.SelectedPath;
            }
        }

        private void sh_bt_select_output_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = false;
            folderDlg.SelectedPath = sh_tb_folder_path_output.Text;
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                sh_tb_folder_path_output.Text = folderDlg.SelectedPath;
            }
        }

        // ---- Save/Load Settings ----
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string app_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            if (!Directory.Exists(app_path))
            {
                Directory.CreateDirectory(app_path);
            }

            if (!app_path.EndsWith("\\")) app_path = app_path + "\\";
            app_path = app_path + "MIQUEST_Manager_Collator.ini";

            StreamWriter ini_file = new StreamWriter(app_path);

            ini_file.WriteLine("RESPONSE_PATH = " + sh_tb_folder_path_response.Text);
            ini_file.WriteLine("OUTPUT_PATH = " + sh_tb_folder_path_output.Text);
            //ini_file.WriteLine("SAVE_MIQUEST = " + Convert.ToString(SH_CB_Save_MIQUEST_CSV.Checked));
            ini_file.WriteLine("SAVE_LOG = " + Convert.ToString(SH_CB_Save_Logfile.Checked));
            ini_file.WriteLine("PCODE_OVERRIDE = " + Convert.ToString(sh_cb_practice_code_override.Checked));
            ini_file.WriteLine("PCODE = " + sh_tb_practice_code.Text);
            ini_file.WriteLine("INIT_BULK_OR = " + Convert.ToString(sh_cb_init_bul_override.Checked));
            ini_file.WriteLine("INIT_OR = " + Convert.ToString(sh_rb_initial.Checked));
            ini_file.WriteLine("BULK_OR = " + Convert.ToString(sh_rb_bulk.Checked));
            

            ini_file.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string app_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (!app_path.EndsWith("\\")) app_path = app_path + "\\";
            app_path = app_path + "MIQUEST_Manager_Collator.ini";
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
                        if (bits[0].Trim() == "RESPONSE_PATH")
                        {
                            sh_tb_folder_path_response.Text = bits[1].Trim();
                        }
                        else if (bits[0].Trim() == "OUTPUT_PATH")
                        {
                            sh_tb_folder_path_output.Text = bits[1].Trim();
                        }
                        //else if (bits[0].Trim() == "SAVE_MIQUEST")
                        //{
                        //    SH_CB_Save_MIQUEST_CSV.Checked = Convert.ToBoolean(bits[1].Trim());
                        //}
                        else if (bits[0].Trim() == "PCODE_OVERRIDE")
                        {
                            sh_cb_practice_code_override.Checked = Convert.ToBoolean(bits[1].Trim());
                        }
                        else if (bits[0].Trim() == "PCODE")
                        {
                            sh_tb_practice_code.Text = bits[1].Trim();
                        }
                        else if (bits[0].Trim() == "SAVE_LOG")
                        {
                           SH_CB_Save_Logfile.Checked = Convert.ToBoolean(bits[1].Trim());
                        }
                        else if (bits[0].Trim() == "INIT_BULK_OR")
                        {
                            sh_cb_init_bul_override.Checked = Convert.ToBoolean(bits[1].Trim());
                        }
                        else if (bits[0].Trim() == "INIT_OR")
                        {
                            sh_rb_initial.Checked = Convert.ToBoolean(bits[1].Trim());
                        }
                        else if (bits[0].Trim() == "BULK_OR")
                        {
                            sh_rb_bulk.Checked = Convert.ToBoolean(bits[1].Trim());
                        }
                    }
                    line = ini_file.ReadLine();
                }
                ini_file.Close();
            }
        }

        private void sh_cb_practice_code_override_CheckedChanged(object sender, EventArgs e)
        {
            if (sh_cb_practice_code_override.Checked)
                sh_tb_practice_code.Enabled = true;
            else
                sh_tb_practice_code.Enabled = false;
        }

        #endregion

    }
}


#region archive

#endregion