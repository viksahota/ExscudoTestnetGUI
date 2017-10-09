using System;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.IO.Compression;
using System.Reflection;
using System.Threading;
using System.Diagnostics;


namespace ExscudoTestnetGUI
{

    public partial class Form1 : Form
    {

        //insantiate the config object
        private configClass conf;

        // This delegates enable asynchronous calls to gui from other threads
        delegate void SetDebugCallback(string text);
        delegate void SetDebugClearCallback();
        delegate void SetLogCallback(string text);
        delegate void SetInfTextCallback(string text);
        delegate void SetConfigUpdateCallback();
        delegate void SetAccountInfoCallback(string text);
        delegate void SetBalanceUpdateCallback(string text);
        delegate void SetUpdateCommitedCallback(string text);
        delegate void SetBackupRestoreCallback();

        //trips once per start to dispaly a message in debugTB if the account looks like its not registered with exscudo
        bool registerWarning = false;

        //define a thread which will process away from form thread.
        Thread eonThread;
        bool eonThreadRun = true;

        public Form1()
        {
            InitializeComponent();

            //start the eon thread
            eonThread = new Thread(new ThreadStart(EonThreadStart));
            eonThread.Start();
            
        }

        public void EonThreadStart()
        {
            //allow the form to open
            Thread.Sleep(100);
            DebugMsg("~~ Application start ~~\r\n");

            Thread.Sleep(100);
            DebugMsg("Working directory : " + Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\r\n");

            //init sequence
            CheckInstall();

            Thread.Sleep(100);
            //execute eon and display the info 
            string inftext = EonCMD("eon");
            InfoUpdate(inftext);

            //update config
            UpdateConfigDisplay();

            //get the account info and populate the GUI
            string res = EonCMD("eon info");
            UpdateAccountInfo(res);

            //get the account balance and update display
            UpdateBalance(EonCMD("eon state"));


            while (eonThreadRun)
            {
                Thread.Sleep(5000);
                //every 5 seconds update the main account balance/deposit
                //query EON
                UpdateBalance(EonCMD("eon state",false));
            }
        }

        //updates the gui balance and deposit. provide the response of 'eon state' (allows eon cmd to be run in another thread this way, callback to forms to update)
        private void UpdateBalance(string stateResponse)
        {
            if (this.balanceLBL.InvokeRequired)
            {
                SetBalanceUpdateCallback d = new SetBalanceUpdateCallback(UpdateBalance);
                this.Invoke(d, new object[] { stateResponse });
            }
            else
            {
                //locate the braces containing json response
                string jsonResponse = stateResponse.Substring(stateResponse.IndexOf('{'), (stateResponse.LastIndexOf('}') - stateResponse.IndexOf('{') + 1));

                //deserialise the response
                responseClass oResp = new responseClass();
                oResp = JsonConvert.DeserializeObject<responseClass>(jsonResponse);

                //report values if OK
                if (oResp.State.Code == 200)
                {
                    balanceLBL.Text = oResp.Amount.ToString();
                    depositLBL.Text = oResp.Deposit.ToString();

                }
                else if (oResp.State.Code == 404)
                {
                    balanceLBL.Text = "404: Account not found";
                    depositLBL.Text = "404: Account not found";

                    //display message for user to register this info with exscudo....
                    if (!registerWarning)
                    {
                        DebugMsg("~~~  Your account cannot be found. To register it with Exscudo you will need these details : ~~~\r\nYour email\r\nYour account ID : " + accountTB.Text + "\r\nYour public key : " + pubkeyTB.Text + "\r\n" + @"Register at :  https://testnet.eontechnology.org/" + "\r\n");
                        DebugMsg("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\r\n");
                        registerWarning = true;
                    }

                 }
            }

            return;
        }

        private void CheckInstall()
        {
            //test if eon.exe exists in the local folder
            if (File.Exists(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\eon.cli.windows\eon.exe"))
            {
                //debugMsg("eon already present");

            }
            else
            {
                Thread.Sleep(100);
                DebugMsg(">eon install not found - downloading....");
                
                //try to delete the eon.cli.windows.zip file if it exists
                try
                {
                    File.Delete(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\eon.cli.windows.zip");
                }
                catch (Exception ex)
                {
                }

                //remove the eon.cli.windows directory if its present
                try
                {
                    Directory.Delete(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\eon.cli.windows");
                }
                catch (Exception ex)
                {
                }

                //download and unpack eon
                try
                {
                    using (var client = new WebClient())
                    {

                        client.DownloadFile("https://testnet.eontechnology.org/client/eon.cli.windows.zip", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\eon.cli.windows.zip");
                        DebugMsg("OK\r\nUnpacking eon.cli.windows ....");
                        Thread.Sleep(100);
                    }

                    try
                    {
                        //unpack the file
                        ZipFile.ExtractToDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\eon.cli.windows.zip", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\eon.cli.windows");
                        DebugMsg("OK\r\n");
                        Thread.Sleep(100);
                    }
                    catch (Exception ex)
                    {
                        DebugMsg("failed unpacking file\r\n");
                        Thread.Sleep(100);
                    }

                    //remove the zip
                    //try to delete the eon.cli.windows.zip file if it exists
                    try
                    {
                        File.Delete(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\eon.cli.windows.zip");
                    }
                    catch (Exception ex)
                    {
                    }
                }
                catch (Exception ex)
                {
                    DebugMsg("failed to download eon.cli.windows.zip\r\n");
                    Thread.Sleep(100);
                }

                DebugMsg(">OpenSSL - downloading....");
                Thread.Sleep(100);

                //try to delete the openssl zip file if it exists
                try
                {
                    File.Delete(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\openssl-0.9.8r-x64_86-win64-rev2.zip");
                }
                catch (Exception ex)
                {
                }

                //remove the openssl directory if its present
                try
                {
                    Directory.Delete(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\openssl-0.9.8r-x64_86-win64-rev2");
                }
                catch (Exception ex)
                {
                }

                //download and unpack openssl
                try
                {
                    using (var client = new WebClient())
                    {
                        Thread.Sleep(100);
                        client.DownloadFile("https://indy.fulgan.com/SSL/openssl-0.9.8r-x64_86-win64-rev2.zip", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\openssl-0.9.8r-x64_86-win64-rev2.zip");
                        DebugMsg("OK\r\nUnpacking openssl ....");
                    }

                    try
                    {
                        //unpack the file
                        Thread.Sleep(100);
                        ZipFile.ExtractToDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +  @"\openssl-0.9.8r-x64_86-win64-rev2.zip", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                        DebugMsg("OK\r\n");
                    }
                    catch (Exception ex)
                    {
                        DebugMsg("failed unpacking openssl\r\n");
                        Thread.Sleep(100);
                    }

                    //try to delete the openssl zip file if it exists to tidy up
                    try
                    {
                        File.Delete(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\openssl-0.9.8r-x64_86-win64-rev2.zip");
                    }
                    catch (Exception ex)
                    {
                    }



                }
                catch (Exception ex)
                {
                    DebugMsg("failed to download openssl\r\n");
                    Thread.Sleep(330);
                }


            }

            //read the config.json file into the conf object
            if (ReadConfig())
            {
                DebugMsg("config.json load OK\r\n\r\n");
            }

            //if the seed value is empty , generate one
            if (conf.Payouts.Seed == "<SEED>")
            {
                //create the seed hex using openssl
                DebugMsg("Generating new seed key and upating config...");
                Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\openssl-0.9.8r-x64_86-win64-rev2");
                string hexString = ExecuteCommandSync(@"openssl rand -hex 32");
                Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                hexString = hexString.Trim();
                conf.Payouts.Seed = hexString;
                WriteConfig();
                DebugMsg("OK\r\n(Seed Hex: " + hexString + ")\r\n\r\n");

                string res = EonCMD("eon info");
                int accInd1 = res.LastIndexOf("Account:") + 10;
                int accInd2 = res.IndexOf(" ", accInd1);
                string accountString = res.Substring(accInd1, accInd2 - accInd1);
                accountString = accountString.Trim();
                //debugMsg("Account ID : " + accountString + "\r\n");

                int pubkeyInd1 = res.LastIndexOf("key:") + 6;
                int pubkeyInd2 = res.IndexOf("\r\n", pubkeyInd1);
                string pubkeyString = res.Substring(pubkeyInd1, pubkeyInd2 - pubkeyInd1);
                //debugMsg("Public key : " + pubkeyString + "\r\n");

                //display message for user to register this info with exscudo....
                DebugMsg("~~~  New account has been created - register it with Exscudo. You will need these details : ~~~\r\nYour email\r\nYour account ID : " + accountString + "\r\nYour public key : " + pubkeyString + "\r\n" + @"Register at :  https://testnet.eontechnology.org/" + "\r\n");
                DebugMsg("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\r\n");

                BackupRestore();
                
                    //we've shown the message once, its enough.  supress further.
                    registerWarning = true;

            }

        }

        private void BackupRestore()
        {
            if (accountTB.InvokeRequired)
            {
                SetBackupRestoreCallback d = new SetBackupRestoreCallback(BackupRestore);
                this.Invoke(d, new object[] {});
            }
            else
            {
                //present a dialog offering to import an existing json.conf , or export the new one for safekeeping (it will be deleted each time application updates)
                //MessageBox.Show();
                DialogResult queryRes1;
                using (DialogCenteringService centeringService = new DialogCenteringService(this)) // center message box
                {
                    queryRes1 = MessageBox.Show("New account created - it will need registering with Exscudo\r\nCheck the debug view for details then use the menu shortcut to reach the registration page\r\n\r\nIts recommended to save your new config.json somewhere safe since it will be deleted each time the application updates. (you can restore it by importing your backup).\r\n\r\nBackup your new config.json now ?", "New account", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }

                if (queryRes1 == DialogResult.Yes)
                {
                    //backup the new config.json
                    //export the json file somewhere.
                    SaveFileDialog jsonDG = new SaveFileDialog
                    {
                        Filter = "json files (*.json)|*.json",
                        FilterIndex = 1,
                        RestoreDirectory = true
                    };

                    try
                    {
                        if (jsonDG.ShowDialog() == DialogResult.OK)
                        {

                            string filename = jsonDG.FileName;

                            string jsonData = JsonConvert.SerializeObject(conf);
                            File.WriteAllText(filename, jsonData);
                            DebugMsg("File export OK\r\n");
                        }
                    }
                    catch (Exception ex)
                    {
                        DebugMsg("Error exporting config file  : " + ex.Message + "\r\n");
                    }

                }
                else
                {
                    //offer to import an existing .json
                    DialogResult queryRes2;
                    using (DialogCenteringService centeringService = new DialogCenteringService(this)) // center message box
                    {
                        queryRes2 = MessageBox.Show("Import an existing account (config.json) ?", "Account import", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    }

                    if (queryRes2 == DialogResult.Yes)
                    {
                        ImportJson();
                    }
                    else
                    {

                    }
                }


            }
        }

                private void UpdateAccountInfo(string res)
        {
            if (accountTB.InvokeRequired)
            {
                SetAccountInfoCallback d = new SetAccountInfoCallback(UpdateAccountInfo);
                this.Invoke(d, new object[] {res});
            }
            else
            {               
                int accInd1 = res.LastIndexOf("Account:") + 10;
                int accInd2 = res.IndexOf(" ", accInd1);
                string accountString = res.Substring(accInd1, accInd2 - accInd1);
                accountString = accountString.Trim();
                //debugMsg("Account ID : " + accountString + "\r\n");

                int pubkeyInd1 = res.LastIndexOf("key:") + 6;
                int pubkeyInd2 = res.IndexOf("\r\n", pubkeyInd1);
                string pubkeyString = res.Substring(pubkeyInd1, pubkeyInd2 - pubkeyInd1);

                accountTB.Text = accountString;
                rxAddressLBL.Text = accountString;
                pubkeyTB.Text = pubkeyString;
            }
        }

        private void UpdateCommited(string commitResponse)
        {
            if (accountTB.InvokeRequired)
            {
                SetUpdateCommitedCallback d = new SetUpdateCommitedCallback(UpdateCommited);
                this.Invoke(d, new object[] { commitResponse });
            }
            else
            {                
                //locate the braces containing json response
                string jsonResponse = commitResponse.Substring(commitResponse.IndexOf('{'), (commitResponse.LastIndexOf('}') - commitResponse.IndexOf('{') + 1));

                //deserialise the response
                commitClass oResp = new commitClass();
                oResp = JsonConvert.DeserializeObject<commitClass>(jsonResponse);

                this.transactionLV.SetObjects(oResp.All);

                //string n = oResp.ToString();

            }
        }
        
        private void UpdateConfigDisplay()
        {
            if (this.rootThreadsNM.InvokeRequired)
            {
                SetConfigUpdateCallback d = new SetConfigUpdateCallback(UpdateConfigDisplay);
                this.Invoke(d, new object[] { });
            }
            else
            {
                rootThreadsNM.Value = conf.Threads;
                rootCoinTB.Text = conf.Coin;
                rootNameTB.Text = conf.Name;

                //remove the s from the interval and convert to int
                string intString = conf.UpstreamCheckInterval;
                intString = intString.Remove(intString.Length - 1, 1);
                //int interval = intString;
                int interval = Convert.ToInt16(intString);
                rootIntervalNM.Value = interval;
                //update the payouts enable checkbox
                if (conf.Payouts.Enabled)
                {
                    payoutEnabledCB.Checked = true;
                }
                else
                {
                    payoutEnabledCB.Checked = false;
                }

                //remove the m from the interval and convert to int
                string intString2 = conf.Payouts.Interval;
                intString2 = intString2.Remove(intString2.Length - 1, 1);
                int interval2 = Convert.ToInt16(intString2);
                payoutIntervalNM.Value = interval2;

                payoutPeerTB.Text = conf.Payouts.Peer;

                payoutSeedTB.Text = conf.Payouts.Seed;

                payoutDeadlineNM.Value = conf.Payouts.Deadline;

                payoutFeeNM.Value = conf.Payouts.Fee;

                //remove the s from the timeout and convert to int
                string intString3 = conf.Payouts.Timeout;
                intString3 = intString3.Remove(intString3.Length - 1, 1);
                int timeoutVal = Convert.ToInt16(intString3);
                payoutTimeoutNM.Value = timeoutVal;

                payoutThresholdNM.Value = conf.Payouts.Threshold;
            }

        }

        private void WriteConfigBTN_Click(object sender, EventArgs e)
        {
            conf.Threads = (byte)rootThreadsNM.Value;
            conf.Coin = rootCoinTB.Text;
            conf.Name = rootNameTB.Text;
            conf.UpstreamCheckInterval = rootIntervalNM.Value.ToString() + "s";
            conf.Payouts.Enabled = payoutEnabledCB.Checked;
            conf.Payouts.Interval = payoutIntervalNM.Value.ToString() + "m";
            conf.Payouts.Peer = payoutPeerTB.Text;
            conf.Payouts.Seed = payoutSeedTB.Text;
            conf.Payouts.Deadline = (uint)payoutDeadlineNM.Value;
            conf.Payouts.Fee = (uint)payoutFeeNM.Value;
            conf.Payouts.Timeout = payoutTimeoutNM.Value.ToString() + "s";
            conf.Payouts.Threshold = (uint)payoutThresholdNM.Value;

            WriteConfig();

            DebugMsg("config.json updated\r\n");
        }

        private void DebugMsg(string line)
        {
            if (this.debugTB.InvokeRequired)
            {
                SetDebugCallback d = new SetDebugCallback(DebugMsg);
                this.Invoke(d, new object[] { line });
            }
            else
            {
                if (debugTB.Text.Length > 10000)
                {
                    debugTB.Text = debugTB.Text.Remove(0, 500);
                }

                debugTB.Text += (DateTime.Now.ToLongTimeString() + ": " + line + "\r\n");
                //debugTB.Text += line;
                this.debugTB.SelectionStart = debugTB.Text.Length;
                this.debugTB.ScrollToCaret();
                this.debugTB.Update();
            }
        }

        private void DebugClear()
        {
            if (this.debugTB.InvokeRequired)
            {
                SetDebugClearCallback d = new SetDebugClearCallback(DebugClear);
                this.Invoke(d, new object[] {});
            }
            else
            {
                debugTB.Clear();
            }
        }

        private void LogMsg(string line)
        {
            if (this.logTB.InvokeRequired)
            {
                SetLogCallback d = new SetLogCallback(LogMsg);
                this.Invoke(d, new object[] { line });
            }
            else
            {
                if (logTB.Text.Length > 10000)
                {
                    logTB.Text = logTB.Text.Remove(0, 500);
                }

                logTB.Text += (DateTime.Now.ToLongTimeString() + ": " + line + "\r\n");
                //logTB.Text += line;

                if (logScrollCB.Checked)
                {
                    this.logTB.SelectionStart = logTB.Text.Length;
                    this.logTB.ScrollToCaret();
                }
            
                this.logTB.Update();
            }
        }

        private void InfoUpdate(string line)
        {
            if (this.infoLBL.InvokeRequired)
            {
                SetInfTextCallback d = new SetInfTextCallback(InfoUpdate);
                this.Invoke(d, new object[] { line });
            }
            else
            {
                infoLBL.Text = line;
                infoLBL.Update();
            }
        }

        private bool ReadConfig()
        {
            bool res = false;
            conf = new configClass();

            try
            {
                string configString = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\eon.cli.windows\config.json");

                conf = JsonConvert.DeserializeObject<configClass>(configString);

                res = true;
            }
            catch(Exception ex)
            {
                res = false;
            }
            return (res);
        }

        private void WriteConfig()
        {
            string jsonData = JsonConvert.SerializeObject(conf);
            File.WriteAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\eon.cli.windows\config.json", jsonData);
        }

        // ---------------------------------------------------------------
        public string ExecuteCommandSync(object command)
        {
            string result = "";

            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command)
                {

                    // The following commands are needed to redirect the standard output.
                    // This means that it will be redirected to the Process.StandardOutput StreamReader.
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    // Do not create the black window.
                    CreateNoWindow = true
                };
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process
                {
                    StartInfo = procStartInfo
                };
                proc.Start();
                // Get the output into a string
                result = proc.StandardOutput.ReadToEnd();
                // Display the command output.
                result = result.Replace("\n", "\r\n");
                //LogMsg(result);
            }
            catch (Exception objException)
            {
                // Log the exception
            }

            return (result);
        }
        //-------------------------------------------------------------

        private string EonCMD(string cmd, bool enableLog)
        {
            string res = "";

            try
            {
                Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\eon.cli.windows");
                res = ExecuteCommandSync(cmd);
                Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                if (enableLog) LogMsg(res + "\r\n");
            }
            catch (Exception ex)
            {
                DebugMsg("ERROR during EON command execution!");
            }
                return (res);
        }

        private string EonCMD(string cmd)
        {
            return(EonCMD(cmd, true));
        }

        private void InfoBTN_Click(object sender, EventArgs e)
        {
            DebugMsg(EonCMD("eon info"));         
        }

        private void EonBTN_Click(object sender, EventArgs e)
        {
            DebugMsg(EonCMD("eon"));
        }

        private void StateBTN_Click(object sender, EventArgs e)
        {
            DebugMsg(EonCMD("eon state"));
        }

        private void State2BTN_Click(object sender, EventArgs e)
        {
            DebugMsg(EonCMD("eon state EON-3A43B-58N25-RFBBS"));
        }

        private void CommitBTN_Click(object sender, EventArgs e)
        {
            string commitResponse = EonCMD("eon commit " + accountTB.Text);
            File.WriteAllText("commitResponse.json", commitResponse);
            UpdateCommited(commitResponse);

           
        }

        private void Commit2BTN_Click(object sender, EventArgs e)
        {
            DebugMsg(EonCMD("eon commit EON-3A43B-58N25-RFBBS"));
        }

        private void Button1_Click(object sender, EventArgs e)
        {
           logTB.Text = "";
        }

        private void ReadConfigBTN_Click(object sender, EventArgs e)
        {
            ReadConfig();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            eonThreadRun = false;
        }

        private void EonCommandLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //launch eon command line
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\eon.cli.windows");
            Process.Start("cmd.exe", "/k");
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

        }

        private void OpensslCommandLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //launch openssl command line
            //launch eon command line
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\openssl-0.9.8r-x64_86-win64-rev2");
            Process.Start("cmd.exe", "/k");
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        }

        private void ImportJson()
        {
            //import a json file, overwrites existing one
            OpenFileDialog importDG = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Filter = "json files (*.json)|*.json",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (importDG.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filename = importDG.FileName;
                    string newJSON = File.ReadAllText(filename);

                    try
                    {
                        conf = JsonConvert.DeserializeObject<configClass>(newJSON);
                    }
                    catch (Exception ex)
                    {
                        DebugMsg("Error deserializing json file - bad format?\r\n");
                    }

                    UpdateConfigDisplay();

                    //save this conf locally.
                    WriteConfig();

                    //get the account info and populate the GUI
                    debugTB.Text = "";
                    string res = EonCMD("eon info");
                    UpdateAccountInfo(res);
                    registerWarning = false;
                    UpdateBalance(EonCMD("eon state"));

                    DebugMsg("config.json updated by file import\r\n");

                }
                catch (Exception ex)
                {
                    DebugMsg("Error: Could not read file. [ " + ex.Message + "]");
                }
            }
        }

        private void ImportConfigjsonToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ImportJson();

        }

        private void ExportConfigjsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //export the json file somewhere.
            SaveFileDialog jsonDG = new SaveFileDialog
            {
                Filter = "json files (*.json)|*.json",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            try
            {
                if (jsonDG.ShowDialog() == DialogResult.OK)
                {

                    string filename = jsonDG.FileName;

                    string jsonData = JsonConvert.SerializeObject(conf);
                    File.WriteAllText(filename, jsonData);
                    DebugMsg("File export OK\r\n");
                }
            }
            catch(Exception ex)
            {
                DebugMsg("Error exporting config file  : " + ex.Message + "\r\n");
            }
        


        }

        //open the working folder in explorer
        private void OpenWorkingFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //launch eon command line
            Process.Start("explorer.exe", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));            
        }

        private void Label16_Click(object sender, EventArgs e)
        {

        }

        private void refillBTN_Click(object sender, EventArgs e)
        {
            //int amount = Convert.ToInt16(balDepAmountTB.Text);
            DialogResult res;

            //MessageBox.Show();
            using (DialogCenteringService centeringService = new DialogCenteringService(this)) // center message box
            {
                res = MessageBox.Show("Transfer " + balDepAmountTB.Text + " to deposit?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            }

            if (res == DialogResult.Yes)
            {
                //code for Yes
                string refillResp = EonCMD("eon refill " + balDepAmountTB.Text);

                //strip the generic stuff
                refillResp = refillResp.Substring(refillResp.IndexOf("Deposit refill,"), refillResp.IndexOf("end...") - refillResp.IndexOf("Deposit refill,"));

                //get the reported amount
                int ind1 = refillResp.IndexOf("Amount:") + 7;
                int ind2 = refillResp.IndexOf("\r\n", ind1);

                int rAmount = Convert.ToInt16(refillResp.Substring(ind1,ind2-ind1));

                //get the result
                int ind3 = refillResp.IndexOf("\"")+1;
                int ind4 = refillResp.LastIndexOf("\"");
                string resultS = refillResp.Substring(ind3, ind4 - ind3);
                //MessageBox.Show();
                using (DialogCenteringService centeringService = new DialogCenteringService(this)) // center message box
                {
                    MessageBox.Show("Refill " + rAmount + " : " + resultS);
                    DebugClear();
                    DebugMsg(refillResp + "\r\n");
                }

            }
            else if (res == DialogResult.No)
            {
                //code for No
                DebugMsg("Refill cancelled\r\n");
            }
            
        }

        private void withdrawBTN_Click(object sender, EventArgs e)
        {

            
            DialogResult res;

            //MessageBox.Show();
            using (DialogCenteringService centeringService = new DialogCenteringService(this)) // center message box
            {
                res = MessageBox.Show("Withdraw " + balDepAmountTB.Text + " from deposit?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            }

            if (res == DialogResult.Yes)
            {
                //code for Yes
                string wdResp = EonCMD("eon withdraw " + balDepAmountTB.Text);

                //strip the generic stuff
                wdResp = wdResp.Substring(wdResp.IndexOf("Deposit"), wdResp.IndexOf("end...") - wdResp.IndexOf("Deposit"));

                //get the reported amount
                int ind1 = wdResp.IndexOf("Amount:") + 7;
                int ind2 = wdResp.IndexOf("\r\n", ind1);
                
                //get the result
                int ind3 = wdResp.IndexOf("\"") + 1;
                int ind4 = wdResp.LastIndexOf("\"");
                string resultS = wdResp.Substring(ind3, ind4 - ind3);
                //MessageBox.Show();
                using (DialogCenteringService centeringService = new DialogCenteringService(this)) // center message box
                {
                    MessageBox.Show("Withdraw " + balDepAmountTB.Text + " : " + resultS);

                    DebugClear();
                    DebugMsg(wdResp + "\r\n");
                }

            }
            else if (res == DialogResult.No)
            {
                //code for No
                DebugMsg("Withdraw cancelled\r\n");
            }

        }

        private void txSendBTN_Click(object sender, EventArgs e)
        {
            
            string recipientAddress = txRecipientTB.Text;

            DialogResult res;

            using (DialogCenteringService centeringService = new DialogCenteringService(this)) // center message box
            {
                res = MessageBox.Show("Send " + txAmountTB.Text + " to account : " + recipientAddress, "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            }

            if (res == DialogResult.Yes)
            {
                //code for Yes
                string txResp = EonCMD("eon payment " + recipientAddress + " " + balDepAmountTB.Text);

                //strip the generic stuff
                txResp = txResp.Substring(txResp.IndexOf("Ordinary"), txResp.IndexOf("end...") - txResp.IndexOf("Ordinary"));

                //get the reported amount
                int ind1 = txResp.IndexOf("Amount:") + 7;
                int ind2 = txResp.IndexOf("\r\n", ind1);

                string rAmount = txResp.Substring(ind1, ind2 - ind1);

                //get the recipient account
                int ind5 = txResp.IndexOf("account:") + 8;
                int ind6 = txResp.IndexOf("\r\n", ind5);

                string rAccount = txResp.Substring(ind5, ind6 - ind5);


                //get the result
                int ind3 = txResp.IndexOf("\"") + 1;
                int ind4 = txResp.LastIndexOf("\"");
                string resultS = txResp.Substring(ind3, ind4 - ind3);
                //MessageBox.Show();
                using (DialogCenteringService centeringService = new DialogCenteringService(this)) // center message box
                {
                    MessageBox.Show("Sent " + rAmount + " to " + rAccount + " : " + resultS);

                    DebugClear();
                    DebugMsg(txResp + "\r\n");
                }

            }
            else if (res == DialogResult.No)
            {
                //code for No
                DebugMsg("Send cancelled\r\n");
            }
        }

        private void openExscudoRegistationWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://testnet.eontechnology.org/");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show();
            using (DialogCenteringService centeringService = new DialogCenteringService(this)) // center message box
            {
                MessageBox.Show("Exscudo Testnet GUI\r\n\r\nNon-official test net tool\r\n\r\nexscudo slack user : gassman\r\nOct 2017","About");
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eonThreadRun = false;
            Application.Exit();
        }
    }

    public class Payouts
    {
        public bool Enabled { get; set; }
        public string Interval { get; set; }
        public string Peer { get; set; }
        public string Seed { get; set; }
        public uint Deadline { get; set; }
        public uint Fee { get; set; }
        public string Timeout { get; set; }
        public int Threshold { get; set; }
    }

    public class RootObject
    {
        public byte Threads { get; set; }
        public string Coin { get; set; }
        public string Name { get; set; }
        public string UpstreamCheckInterval { get; set; }
        public Payouts Payouts { get; set; }
    }
}
