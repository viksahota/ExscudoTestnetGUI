using System;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.IO.Compression;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace ExscudoTestnetGUI
{

    public partial class Form1 : Form
    {

        private int selectedWallet=0;

        //create wallet list and the wallet object for the main account. Additional accounts can be added
        List<WalletClass> WalletList = new List<WalletClass>();
        WalletClass mainWallet = new WalletClass();

        // This delegates enable asynchronous calls to gui from other threads
        delegate void SetDebugCallback(string text);
        delegate void SetDebugClearCallback();
        delegate void SetLogCallback(string text);
        delegate void SetInfTextCallback(string text);
        delegate void SetConfigUpdateCallback(int index);
        delegate void SetAccountInfoCallback(string infoResponse);
        delegate void SetBalanceUpdateCallback(string text);
        delegate void SetBalanceUpdate2Callback(string text);
        delegate void SetUpdateCommitedCallback(string text);
        delegate void SetBackupRestoreCallback(int index);
        delegate void SetSyncGUIBalancesCallback();
        delegate void SetAccountCheckImportCreateCallback();
        delegate void SetSelectedAccountCallback(int index);

        //define a thread which will process away from form thread.
        Thread eonThread;
        bool eonThreadRun = true;
        bool os64bit = false;
        //so we know when the selected wallet is really changed, can be spurious events
        int lastWalletIndex = -1;

        string appPath = "";
                
        public Form1()
        {
            InitializeComponent();

            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            appPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\ExscudoTestnetGUI";

            os64bit = Environment.Is64BitOperatingSystem;

            WalletList.Add(mainWallet);

            //set the primary wallet as default selected
            selectedWallet = 0;
            

            //start the eon thread
            eonThread = new Thread(new ThreadStart(EonThreadStart));
            eonThread.SetApartmentState(ApartmentState.STA);
            eonThread.Start();

        }


        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            // Log the exception, display it, etc
            MessageBox.Show("ThreadException error.\r\n\r\nPlease submit screenshot for debug : \r\n\r\n" + e.Exception.Message, "Error - Terminating....", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

            // Log the exception, display it, etc
            MessageBox.Show("Unhandled Exception error.\r\n\r\nPlease submit screenshot for debug : \r\n\r\n" + (e.ExceptionObject as Exception).Message, "Error - Terminating....", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

        public void EonThreadStart()
        {
            //allow the form to open
            Thread.Sleep(100);
            DebugLogMsg("~~ Application start ~~\r\n");

            Thread.Sleep(100);
            DebugLogMsg("Working directory : " + appPath + "\r\n");


            //set up the eon folders and wallets
            try
            {

                if (SetupEonFolder(0))
                {
                    //try and load stored walletslist
                    if (File.Exists(appPath + @"\wallets.json"))
                    {
                        WalletList = ReadFromJsonFile<List<WalletClass>>(appPath + @"\wallets.json");
                       
                    }

                    //setup the main wallet
                    SetupWallet(0);

                    //setup folders for each attached account
                    if (WalletList.Count>1)
                    {
                        for (int i=1; i < WalletList.Count ; i++)
                        {
                            SetupEonFolder(i);
                            SetupWallet(i);
                        }
                    }


                }

                
            }
            catch (Exception ex)
            {
                DebugLogMsg("EonThread() exception during setup :" + ex.Message + "\r\n");

            }

            

            Thread.Sleep(100);
            //execute eon and display version information 
            string eontext = EonCMD(0, "eon");
            if (eontext != "-1") InfoUpdate(eontext);

            //update config
            UpdateConfigDisplay(0);

            //get the account info and populate the GUI
            string inftext = EonCMD(0, "eon info");
            if (inftext != "-1") UpdateAccountInfo(inftext);

            //get the account balances and update display
            UpdateBalances();

            //populate the account list & select the primary account
            this.accountLV.SetObjects(WalletList);
            SetSelectedAccount(0);
            SyncGUIBalances();
            


            int counter = 0;
            DateTime lastBalancePollTime = DateTime.Now;

            while (eonThreadRun)
            {
                Thread.Sleep(50);
                //every 5 seconds update the main account balance/deposit
                //query EON
                TimeSpan elapsed = DateTime.Now - lastBalancePollTime;
                if (elapsed.Seconds >= 10)
                {
                    if (eonThreadRun) UpdateBalances();
                    if (eonThreadRun) SyncGUIBalances();
                    lastBalancePollTime = DateTime.Now;

                    counter++;
                    if (counter >= 10)
                    {
                        GC.Collect();
                        counter = 0;
                    }
                }

            }
        }

        private void GetAttachedAccounts()
        {
            AccountSeedDialog asDialog = new AccountSeedDialog();

            //we need to get the commit response and search for "New account" tranactions
            string commitResponse = EonCMD(0, "eon allcommit " + accountTB.Text);

            if (commitResponse != "-1")
            {
                //locate the braces containing json response
                int ind1 = commitResponse.IndexOf('{');
                int ind2 = commitResponse.LastIndexOf('}');

                if ((ind1 != -1) && (ind2 != -1) && (!commitResponse.Contains("null")) && (commitResponse != "-1"))
                {
                    string jsonResponse = commitResponse.Substring(ind1, (ind2 - ind1 + 1));

                    //output may be broken up through pagination of results introduced in eon 0.12. Try and remove <nil> seperators before deserialising..
                    string pagePattern = @"}\s*]\s*}\s*<nil>\s*{\s*""all"":\s\[\s*{";
                    jsonResponse = Regex.Replace(jsonResponse, pagePattern, "},\n{");

                    //deserialise the response
                commitClass oResp = new commitClass();
                    oResp = JsonConvert.DeserializeObject<commitClass>(jsonResponse);

                    //reverse the transactions order so we will locate the attached accounts in the created order
                    Array.Reverse(oResp.All);
                    

                    foreach (commitClass.All2 m in oResp.All)
                    {
                        if (m.Type == 100)
                        {
                            string newAccountAttachment = m.Attachment.ToString();

                            //attachment data needs to be extracted for this entry from the commit list, hunt the signature and get the attachement...
                            string newAccountSig = m.Signature;
                            string pattern = newAccountSig + @""",\s*""attachment"":\s*({\s*[^}]*\s})";
                            Match transactionJsonMatch = Regex.Match(commitResponse, pattern);
                            if (transactionJsonMatch.Groups.Count == 2)
                            {
                                //get the attachment, remove brackets and whitespace
                                string transactionJson = transactionJsonMatch.Groups[1].ToString();
                                transactionJson = transactionJson.Remove(0, 1);
                                transactionJson = transactionJson.Remove(transactionJson.Length - 1, 1);
                                transactionJson.Trim();

                                //get new account info
                                string nPattern = @"""([^\s]*)"":\s""([^\s]*)""";
                                Match newAccountMatch = Regex.Match(transactionJson, nPattern);

                                string newAccountID = newAccountMatch.Groups[1].ToString();
                                string pubKey = newAccountMatch.Groups[2].ToString();


                                //scan if we already have this accountID in the walletList
                                int matchVal = -1;
                                for (int a = 0; a < WalletList.Count; a++)
                                {
                                    if (WalletList[a].AccountID == newAccountID) matchVal = a;
                                }

                                if (matchVal == -1)  //entry does not exist in walletlist, create it and ask for seed.
                                {
                                    //create a new wallet in our list
                                    WalletClass wal = new WalletClass
                                    {
                                        AccountID = newAccountID
                                    };
                                    WalletList.Add(wal);

                                    //request the private-key (SEED) for this account from the user
                                    asDialog = new AccountSeedDialog();
                                    asDialog.setAccountLabel(wal.AccountID);
                                    asDialog.ShowDialog();

                                    //if the seed and nickname were supplied, update the wallet class
                                    if (asDialog.result == true)
                                    {
                                        //store the nickname and seed
                                        WalletList[WalletList.Count - 1].Seed = asDialog.seedVal;
                                        WalletList[WalletList.Count - 1].NickName = asDialog.nickName;
                                        WalletList[WalletList.Count - 1].ConfigJson.Payouts.Seed = asDialog.seedVal;

                                        //get the public key
                                        string infoResponse = EonCMD(0, "eon info " + asDialog.seedVal);
                                        Match infoMatch = Regex.Match(infoResponse, @"Seed:\s*(.*)\s*Account:\s*(.*)\s*Public key:\s*(.*)\s*end...");

                                        if (infoMatch.Groups.Count == 4)
                                        {
                                            string pubkeyString = infoMatch.Groups[3].ToString();
                                            WalletList[WalletList.Count - 1].PublicKey = pubkeyString;
                                        }
                                        else
                                        {
                                            DebugLogMsg("GetAttachedAccount() Error parsing 'eon info' response : " + infoResponse);
                                        }
                                    }
                                    Thread.Sleep(1000);
                                }

                                else //entry already exists in the walletlist, just check in case we dont have the seed yet
                                {
                                    WalletList[matchVal].Seed = WalletList[matchVal].Seed.Trim();

                                    //if the seed is not set, request it from the user
                                    if (WalletList[matchVal].Seed.Length != 64)
                                    {
                                        asDialog = new AccountSeedDialog();
                                        asDialog.setAccountLabel(newAccountID);
                                        asDialog.ShowDialog();

                                        //if the seed and nickname were supplied, update the wallet class
                                        if (asDialog.result == true)
                                        {
                                            //store the nickname and seed
                                            WalletList[WalletList.Count - 1].Seed = asDialog.seedVal;
                                            WalletList[WalletList.Count - 1].NickName = asDialog.nickName;
                                            WalletList[WalletList.Count - 1].ConfigJson.Payouts.Seed = asDialog.seedVal;
                                            Thread.Sleep(1000);

                                        }

                                    }

                                    //if the seed is ok , pass through....

                                }


                                //report the account in the view
                                DebugMsg("Attached account detected: " + newAccountID + "\r\n");
                            }

                        }

                        


                        
                    }

                }


            }


        }

        //tests the identified wallet to see if the account exists and is active. returns:  -1= does not exist or other fail ,   1= active  ,  2= exists but inactive
        private int CheckAccountState(int index)
        {
            int result = 0-1;

            try
            {
                string stateResponse = EonCMD(index, "eon state", true);
                if (stateResponse != "-1")
                {

                    //Match stateMatches = Regex.Match(stateResponse, @"state:\s*(\d*).*\s*Amount:\s*(.*)\s*Deposit:\s*(.*)\s*end...");

                    string pattern = @"""code"": (\d*),\s*""name"": ""([^""]*)""\s*},\s*""amount"": (\d*),\s*""deposit"": (\d*)\s*}";
                    Match stateMatches = Regex.Match(stateResponse,pattern);
                    

                    //if we have a valid match , extract the items into a stateResponse object
                    if (stateMatches.Groups.Count == 5)
                    {
                        //recover the parameters
                        int code = Convert.ToInt16(stateMatches.Groups[1].ToString());

                        //if OK, update the GUI balance
                        if (code == 200)
                        {
                            //account active
                            result = 1;
                        }
                        else if (code == 404)
                        {
                            //account not active
                            //display message for user to register this info with exscudo....
                            DebugMsg("~~~  Your account cannot be found. To register it with Exscudo you will need these details : ~~~\r\nYour email\r\nYour account ID : " + accountTB.Text + "\r\nYour public key : " + pubkeyTB.Text + "\r\n" + @"Register at :  https://testnet.eontechnology.org/" + "\r\n");
                            DebugMsg("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\r\n");

                            //exists, inactive
                            result = 2;

                        }
                    }
                    else
                    {
                        //does not exist, or other error
                        result = 0;
                        DebugLogMsg("CheckAccountState() - unexpected response : " + stateResponse);
                        result = -1;
                    }
                }
                //failed to get state response
                else result = -1; 
            }
            catch (Exception ex)
            {
                DebugLogMsg("Exception in CheckAcountState() - " + ex.Message);
                result = -1;
            }

            return result;
        }

        private void UpdateBalances()
        {
            for(int i=0; i<WalletList.Count; i++)
            {
                string stateResponse = EonCMD(i, "eon state",false);
                if ((stateResponse != "-1")&&(eonThreadRun))
                {
                    try
                    {
                        //Match stateMatches = Regex.Match(stateResponse, @"state:\s*(\d*).*\s*Amount:\s*(.*)\s*Deposit:\s*(.*)\s*end...");
                        string pattern = @"""code"": (\d*),\s*""name"": ""([^""]*)""\s*},\s*""amount"": (\d*),\s*""deposit"": (\d*)\s*}";
                        Match stateMatches = Regex.Match(stateResponse, pattern);

                        //if we have a valid match , extract the items into a stateResponse object
                        if (stateMatches.Groups.Count == 5)
                        {
                            //recover the parameters
                            int code = Convert.ToInt16(stateMatches.Groups[1].ToString());

                            //if OK, update the walletlist balance
                            if (code == 200)
                            {
                                //update wallet object
                                WalletList[i].Balance = (Convert.ToDecimal(stateMatches.Groups[3].ToString()) / 1000000).ToString();
                                WalletList[i].Deposit = (Convert.ToDecimal(stateMatches.Groups[4].ToString()) / 1000000).ToString();
                            }
                            else if (code == 404)
                            {
                                //Account not found
                            }
                        }
                        else
                        {
                            //if we have a seed format error, trim the seed value
                            if (stateResponse.Contains("Seed format error:"))
                            {
                                WalletList[i].Seed = WalletList[i].Seed.Trim();
                                WalletList[i].ConfigJson.Payouts.Seed = WalletList[i].ConfigJson.Payouts.Seed.Trim();
                                WalletList[i].PublicKey = WalletList[i].PublicKey.Trim();
                                WalletList[i].AccountID = WalletList[i].AccountID.Trim();
                                WriteConfig(i);
                                SaveWalletList();
                            }

                            //else report it
                            else DebugLogMsg("UpdateBalances(" + i + ") - unexpected response : " + stateResponse);
                        }

                    }
                    catch (Exception ex)
                    {
                        DebugLogMsg("Exception in UpdateBalances(" + i + ") - " + ex.Message);
                    }



                }

            }
        }

     
        //Updates the onscreen balances using the data in walletlist
        private void SyncGUIBalances()
        {
            if (this.balanceLBL.InvokeRequired)
            {
                SetSyncGUIBalancesCallback d = new SetSyncGUIBalancesCallback(SyncGUIBalances);
                this.Invoke(d, new object[] {  });
            }
            else
            {
                //refresh if needed
                if (balanceLBL.Text != WalletList[selectedWallet].Balance) balanceLBL.Text = WalletList[selectedWallet].Balance;
                if (depositLBL.Text != WalletList[selectedWallet].Deposit) depositLBL.Text = WalletList[selectedWallet].Deposit;
                if (rxAddressLBL.Text != WalletList[selectedWallet].AccountID) rxAddressLBL.Text = WalletList[selectedWallet].AccountID;
                accountLV.BuildList(true);

            }
        }

        //sets the selected account index, needs to be set to primary on startup, as default.
        private void SetSelectedAccount(int index)
        {
            if (this.balanceLBL.InvokeRequired)
            {
                SetSelectedAccountCallback d = new SetSelectedAccountCallback(SetSelectedAccount);
                this.Invoke(d, new object[] { index });
            }
            else
            {
                    selectedWallet = index;
                    accountLV.SelectedIndex = index;
                    accountLV.UnfocusedHighlightBackgroundColor = System.Drawing.Color.BlanchedAlmond;
                    accountLV.Update();
                    UpdateConfigDisplay(selectedWallet);

            }
        }

        //checks if the identified eon instance is already set up, sets up if neccesary.  Allows to prepare multiple directories for multiple wallets.
        //wallet0 is the main wallet.
        private bool SetupEonFolder(int instance)
        {

            int cstate = 0;

            while ((cstate != 3) && (cstate != 99))
            {
                switch (cstate)
                {
                    case (0):
                        try
                        {
                            //check if specicifed eon dir exists , create if necc.
                            if (Directory.Exists(appPath + @"\eon" + instance.ToString()))
                            {
                                if (File.Exists(appPath + @"\eon" + instance.ToString() + @"\eon.exe"))
                                {
                                    cstate = 3;
                                }
                                else
                                {
                                    cstate = 2;
                                }

                            }
                            else
                            {
                                cstate = 1;
                            }
                        }
                        catch(Exception ex)
                        {
                            DebugLogMsg("SetupEonInstance(" + instance.ToString() + ") state0 exception : " + ex.Message);
                            cstate = 99;
                        }
                        break;

                    case (1):
                        try
                        {
                            Directory.CreateDirectory(appPath + @"\eon" + instance.ToString());
                            cstate = 2;
                        }
                        catch(Exception ex)
                        {
                            DebugLogMsg("SetupEonInstance(" + instance.ToString() + ") state1 exception : " + ex.Message);
                            cstate = 99;
                        }
                        break;

                    case (2):
                        try
                        {
                            //deploy eon client
                            if (os64bit)
                            {
                                File.WriteAllBytes(appPath + @"\eon" + instance.ToString() + @"\eon.exe", ExscudoTestnetGUI.Properties.Resources.eon64);
                                if (instance == 0) DebugLogMsg("eon (x64) deployed\r\n");
                            }
                            else
                            {
                                File.WriteAllBytes(appPath + @"\eon" + instance.ToString() + @"\eon.exe", ExscudoTestnetGUI.Properties.Resources.eon32);
                                if (instance == 0) DebugLogMsg("eon (x32) deployed\r\n");
                            }

                            File.WriteAllText(appPath + @"\eon" + instance.ToString() + @"\config.json", ExscudoTestnetGUI.Properties.Resources.config);
                            File.WriteAllText(appPath + @"\eon" + instance.ToString() + @"\Readme.md", ExscudoTestnetGUI.Properties.Resources.ReadMe);

                            DebugMsg("Checking in case antivirus software removes eon.exe ...");
                            int i = 4;
                            while (i > 0)
                            {
                                DebugMsg(i.ToString() + "...");
                                Thread.Sleep(1000);
                                i--;
                            }
                            DebugMsg("\r\n");

                            //test eon.exe
                            if (File.Exists(appPath + @"\eon" + instance.ToString() + @"\eon.exe"))
                            {
                                cstate = 3;
                                DebugMsg("eon OK\r\n");
                            }
                            else
                            {
                                MessageBox.Show(@"\eon" + instance.ToString() + @"\eon.exe" + " is missing, this can occur since some antivirus tools can\r\nidentify eon.exe as a virus and quarantine it\r\nPlease check you antivirus quarantine, restore eon.exe, and restart this application");
                                DebugLogMsg("SetupEonInstance() : eon.exe is missing, removed by antivirus?\r\n");
                                cstate = 99;
                            }

                        }
                        catch (Exception ex)
                        {
                            DebugLogMsg("SetupEonInstance(" + instance.ToString() + ") state2 exception : " + ex.Message);
                            cstate = 99;
                        }


                        break;

                    case (3):
                        //success
                        break;

                    case (99):
                        //fail
                        break;
                }
            }

            if (cstate == 3) return true;
            else if (cstate == 99) return false;
            else return false;
        }

        private bool SaveWalletList()
        {
            bool res = false;
            try
            {
                WriteToJsonFile(appPath + @"\wallets.json", WalletList, false);
                res = true;
            }
            catch (Exception ex)
            {
                DebugLogMsg("SaveWalletList() Exception saving wallets.json : " + ex.Message);
                res = false;
            }
            return (res);
        }


        private bool SetupWallet(int walletIndex)
        {
            int cstate = 0;
            SetupWalletDialog swDialog = new SetupWalletDialog();


            while ((cstate != 9)&&(cstate != 99))
            {
                switch (cstate)
                {
                    case (0):
                        //check if there is an entry in WalletList for the requested instance
                        if ((walletIndex + 1) <= WalletList.Count)
                        {
                            WalletList[walletIndex].Seed = WalletList[walletIndex].Seed.Trim();
                            WalletList[walletIndex].PublicKey = WalletList[walletIndex].PublicKey.Trim();
                            WalletList[walletIndex].AccountID = WalletList[walletIndex].AccountID.Trim();

                            //check if SEED is not yet set for this account
                            if (WalletList[walletIndex].Seed.Length != 64)
                            {
                                cstate = 1;
                            }
                            else
                            {
                                cstate = 6;
                            }

                        }
                        break;

                    case (1):

                        //show dialog to either create a new account, import a config.json, or paste a SEED

                        swDialog.ShowDialog();
                        //swDialog.Show();

                        if (swDialog.result == "Create") cstate = 4;
                        else if (swDialog.result == "Import") cstate = 2;
                        else if (swDialog.result == "Set") cstate = 3;
                        else if (swDialog.result == "Cancel") cstate = 99;

                        break;

                    case (2):
                        if (ImportJsonDLG(walletIndex))
                        {
                           WalletList[walletIndex].Seed = WalletList[walletIndex].ConfigJson.Payouts.Seed;
                            cstate = 5;
                        }
                        //if it fails, return to selection
                        else cstate = 2;

                        break;

                    case (3):

                        //set the pasted SEED value ----------------------------------

                        try
                        {
                            //read the config file into the wallet list
                            ReadConfigObject(walletIndex);

                            //adjust the SEED
                            WalletList[walletIndex].Seed = swDialog.seedVal;
                            WalletList[walletIndex].ConfigJson.Payouts.Seed = swDialog.seedVal;

                            //write the new config back to the config.json file
                            WriteConfig(walletIndex);
                            SaveWalletList();
                            cstate = 5;
                        }
                        catch (Exception ex)
                        {
                            DebugLogMsg("SetupWallet() state3 exception : " + ex.Message);
                            cstate = 99;
                        }

                        break;

                    case (4):

                        //create a new SEED
                        string seedResponse = EonCMD(0, "eon seed");
                        DebugLogMsg(seedResponse);
                        string pattern = @"Seed:\s*(.*)\s*Account:\s*(.*)\s*Public key:\s*(.*)";
                        Match srMatch = Regex.Match(seedResponse, pattern);


                        string seed = srMatch.Groups[1].ToString();
                        string accountID = srMatch.Groups[2].ToString();
                        string publicKey = srMatch.Groups[3].ToString();

                        //register this new account (if its not primary)
                        if (walletIndex != 0)
                        {
                            string createResponse = EonCMD(0, "eon new_account " + publicKey);
                            DebugLogMsg(createResponse);
                            string pattern2 = @"send transaction.*<<\s*""(.*)""";
                            Match crMatch = Regex.Match(createResponse, pattern2);
                            string createResult = crMatch.Groups[1].ToString();


                            if (createResult == "success")
                            {
                                //new account has been created.  update the walletlist item and the config file
                                ReadConfigObject(walletIndex);
                                WalletList[walletIndex].Seed = seed;
                                WalletList[walletIndex].AccountID = accountID;
                                WalletList[walletIndex].PublicKey = publicKey;
                                if (walletIndex == 0) WalletList[walletIndex].NickName = "";
                                WalletList[walletIndex].ConfigJson.Payouts.Seed = seed;
                                WriteConfig(walletIndex);
                                SaveWalletList();

                                cstate = 6;


                            }
                            else cstate = 99;
                        }
                        else
                        {
                            //its the primary wallet -- display a notification
                            //display message for user to register this info with exscudo....


                            ReadConfigObject(walletIndex);
                            WalletList[walletIndex].Seed = seed;
                            WalletList[walletIndex].AccountID = accountID;
                            WalletList[walletIndex].PublicKey = publicKey;
                            if (walletIndex == 0) WalletList[walletIndex].NickName = "Primary";
                            WalletList[walletIndex].ConfigJson.Payouts.Seed = seed;
                            WriteConfig(walletIndex);
                            SaveWalletList();

                            DebugLogMsg("~~~  New account has been created - register it with Exscudo. You will need these details : ~~~\r\nYour email\r\nYour account ID : " + accountID + "\r\nYour public key : " + publicKey + "\r\n" + @"Register at :  https://testnet.eontechnology.org/" + "\r\n");
                            DebugLogMsg("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\r\n");

                            MessageBox.Show("New account has been created but needs registering with Exscudo. Check the debug log for details.\r\nOnce you've registered, restart this application");

                            cstate = 6;
                        }

                        break;


                    case (5):
                        //get wallet info from imported/pasted seed and populate the wallet object / walletlist
                        string infoResponse = EonCMD(walletIndex, "eon info");
                        Match infoMatch = Regex.Match(infoResponse, @"Seed:\s*(.*)\s*Account:\s*(.*)\s*Public key:\s*(.*)\s*end...");

                        if (infoMatch.Groups.Count == 4)
                        {
                            string seedString = infoMatch.Groups[1].ToString();
                            string accountString = infoMatch.Groups[2].ToString();
                            string pubkeyString = infoMatch.Groups[3].ToString();

                            WalletList[walletIndex].AccountID = accountString;
                            WalletList[walletIndex].PublicKey = pubkeyString;
                            if (walletIndex == 0) WalletList[walletIndex].NickName = "Primary";

                            cstate = 0;
                            
                        }
                        else
                        {
                            DebugLogMsg("SetupWallet() state5 - Error parsing info response : " + infoResponse);
                            cstate = 99;
                        }


                        break;

                    case (6):

                        //check if account is active?
                        WriteConfig(walletIndex);
                        int checkResult = CheckAccountState(walletIndex);

                        if (checkResult == 1) cstate = 7;
                        else if (checkResult == -1) cstate = 99;
                        else if (checkResult == 2) cstate = 8;  //store and finish if its unregistered


                        break;

                    case (7):
                        //check if primary account, if so get the attached accounts and update walletList
                        if (walletIndex == 0)
                        {
                            //find the attached accounts and populate walletlist with accountID & public key
                            GetAttachedAccounts();
                            cstate = 8;
                        }
                        else cstate = 8;

                        break;

                    case (8):
                        //write out the config file and store walletList to json file
                        WriteConfig(walletIndex);
                        SaveWalletList();
                        cstate = 9;

                        break;

                    case (9):
                        //success
                        break;

                    case (99):
                        //fail
                        break;
                }
            }

            return false;
        }


        //updates the GUI account information fields. takes the 'eon info' response as its input and invokes itself on form thread
        private void UpdateAccountInfo(string infoResponse)
        {
            if (accountTB.InvokeRequired)
            {
                SetAccountInfoCallback d = new SetAccountInfoCallback(UpdateAccountInfo);
                this.Invoke(d, new object[] {infoResponse});
            }
            else
            {
                try
                {
                    Match infoMatch = Regex.Match(infoResponse, @"Seed:\s*(.*)\s*Account:\s*(.*)\s*Public key:\s*(.*)\s*end...");

                    if(infoMatch.Groups.Count==4)
                    {
                        mainWallet.Seed = infoMatch.Groups[1].ToString();
                        mainWallet.AccountID = infoMatch.Groups[2].ToString();
                        mainWallet.PublicKey = infoMatch.Groups[3].ToString();
                        accountTB.Text = mainWallet.AccountID;
                        rxAddressLBL.Text = mainWallet.AccountID;
                        pubkeyTB.Text = mainWallet.PublicKey;
                    }
                    else
                    {
                        DebugLogMsg("UpdateAccountInfo() - Error parsing info response : " + infoResponse);
                    }
                }
                catch(Exception ex)
                {
                    DebugMsg("Exception in UpdateAccountInfo() - " + ex.Message);
                    LogMsg("Exception in UpdateAccountInfo() - " + ex.Message);
                }
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
                try
                {

                    if (commitResponse != "-1")
                    {
                        //locate the braces containing json response
                        int ind1 = commitResponse.IndexOf('{');
                        int ind2 = commitResponse.LastIndexOf('}');

                        if ((ind1 != -1) && (ind2 != -1) && (!commitResponse.Contains("null")) && (commitResponse != "-1"))
                        {
                            string jsonResponse = commitResponse.Substring(ind1, (ind2 - ind1 + 1));

                            //output may be broken up through pagination of results introduced in eon 0.12. Try and remove <nil> seperators before deserialising..
                            string pagePattern = @"}\s*]\s*}\s*<nil>\s*{\s*""all"":\s\[\s*{";
                            jsonResponse = Regex.Replace(jsonResponse, pagePattern, "},\n{");

                            //deserialise the response
                            commitClass oResp = new commitClass();
                            oResp = JsonConvert.DeserializeObject<commitClass>(jsonResponse);

                            
                            //Alter for better output in listview - convert the UNIX timestamp , convert amounts to EON , generate verbose transaction type 
                            foreach(commitClass.All2 transaction in oResp.All)
                            {
                                var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds((long)transaction.Timestamp);
                                DateTime dateTime = dateTimeOffset.DateTime;
                                transaction.Attachment.Amount = transaction.Attachment.Amount / 1000000;

                                switch(transaction.Type)
                                {
                                    case (100):
                                        transaction.TypeString = "New Account";
                                        break;

                                    case (200):
                                        transaction.TypeString = "Payment";
                                        break;
                                    case (310):
                                        transaction.TypeString = "Refill";
                                        break;

                                    case (320):
                                        transaction.TypeString = "Withdraw";
                                        break;

                                    default:
                                        transaction.TypeString = transaction.Type.ToString();
                                        break;

                                }

                                //TimeSpan ts = TimeSpan.FromSeconds(transaction.Timestamp);
                                //DateTime localDateTime = new DateTime(ts.Ticks).ToLocalTime();
                                transaction.TimestampString = dateTime.ToShortTimeString() + "  " + dateTime.ToShortDateString();
                                
                            }




                            this.transactionLV.SetObjects(oResp.All);
                        }
                        else
                        {
                            DebugMsg("Error checking tranactions, unexpected  commit response : " + commitResponse);
                        }
                    }
                }
                catch (Exception ex)
                {
                    DebugMsg("Exception in UpdateCommited() - " + ex.Message);
                    LogMsg("Exception in UpdateCommited() - " + ex.Message);
                }

                //string n = oResp.ToString();

            }
        }
        
        private void UpdateConfigDisplay(int index)
        {
            if (this.rootThreadsNM.InvokeRequired)
            {
                SetConfigUpdateCallback d = new SetConfigUpdateCallback(UpdateConfigDisplay);
                this.Invoke(d, new object[] { index });
            }
            else
            {
                try
                {
                    rootThreadsNM.Value = WalletList[index].ConfigJson.Threads;
                    rootCoinTB.Text = WalletList[index].ConfigJson.Coin;
                    rootNameTB.Text = WalletList[index].ConfigJson.Name;

                    //remove the s from the interval and convert to int
                    string intString = WalletList[index].ConfigJson.UpstreamCheckInterval;
                    intString = intString.Remove(intString.Length - 1, 1);
                    //int interval = intString;
                    int interval = Convert.ToInt16(intString);
                    rootIntervalNM.Value = interval;
                    //update the payouts enable checkbox
                    if (WalletList[index].ConfigJson.Payouts.Enabled)
                    {
                        payoutEnabledCB.Checked = true;
                    }
                    else
                    {
                        payoutEnabledCB.Checked = false;
                    }

                    //remove the m from the interval and convert to int
                    string intString2 = WalletList[index].ConfigJson.Payouts.Interval;
                    intString2 = intString2.Remove(intString2.Length - 1, 1);
                    int interval2 = Convert.ToInt16(intString2);
                    payoutIntervalNM.Value = interval2;

                    payoutPeerTB.Text = WalletList[index].ConfigJson.Payouts.Peer;

                    payoutSeedTB.Text = WalletList[index].ConfigJson.Payouts.Seed;

                    payoutDeadlineNM.Value = WalletList[index].ConfigJson.Payouts.Deadline;

                    payoutFeeNM.Value = WalletList[index].ConfigJson.Payouts.Fee;

                    //update the payouts Raw checkbox
                    if (WalletList[index].ConfigJson.Payouts.Raw)
                    {
                        payoutRawCB.Checked = true;
                    }
                    else
                    {
                        payoutRawCB.Checked = false;
                    }

                    //remove the s from the timeout and convert to int
                    string intString3 = WalletList[index].ConfigJson.Payouts.Timeout;
                    intString3 = intString3.Remove(intString3.Length - 1, 1);
                    int timeoutVal = Convert.ToInt16(intString3);
                    payoutTimeoutNM.Value = timeoutVal;
                    
                    payoutThresholdNM.Value = WalletList[index].ConfigJson.Payouts.Threshold;
                }
                catch(Exception ex)
                {
                    DebugMsg("Exception in UpdateConfigDisplay() - " + ex.Message);
                    LogMsg("Exception in UpdateConfigDisplay() - " + ex.Message);
                }
            }

        }

        //takes the current settings on screen and stores them in the selectedWallet, writes the updated config.json to the directory to apply the changes.
        private void WriteConfigBTN_Click(object sender, EventArgs e)
        {
            WalletList[selectedWallet].ConfigJson.Threads = (byte)rootThreadsNM.Value;
            WalletList[selectedWallet].ConfigJson.Coin = rootCoinTB.Text;
            WalletList[selectedWallet].ConfigJson.Name = rootNameTB.Text;
            WalletList[selectedWallet].ConfigJson.UpstreamCheckInterval = rootIntervalNM.Value.ToString() + "s";
            WalletList[selectedWallet].ConfigJson.Payouts.Enabled = payoutEnabledCB.Checked;
            WalletList[selectedWallet].ConfigJson.Payouts.Interval = payoutIntervalNM.Value.ToString() + "m";
            WalletList[selectedWallet].ConfigJson.Payouts.Peer = payoutPeerTB.Text;
            WalletList[selectedWallet].ConfigJson.Payouts.Seed = payoutSeedTB.Text;
            WalletList[selectedWallet].ConfigJson.Payouts.Deadline = (uint)payoutDeadlineNM.Value;
            WalletList[selectedWallet].ConfigJson.Payouts.Fee = (uint)payoutFeeNM.Value;
            WalletList[selectedWallet].ConfigJson.Payouts.Raw = payoutRawCB.Checked;
            WalletList[selectedWallet].ConfigJson.Payouts.Timeout = payoutTimeoutNM.Value.ToString() + "s";
            WalletList[selectedWallet].ConfigJson.Payouts.Threshold = (uint)payoutThresholdNM.Value;
            WriteConfig(selectedWallet);

            SaveWalletList();

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

        private void DebugLogMsg(string line)
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

                LogMsg(line);

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

                //logTB.Text += (DateTime.Now.ToLongTimeString() + ": " + line + "\r\n");
                logTB.AppendText(DateTime.Now.ToLongTimeString() + ": " + line + "\r\n");
                //logTB.Text += line;

                if (logScrollCB.Checked)
                {
                    //logTB.Select(logTB.Text.Length, 0);
                    
                    logTB.SelectionStart = logTB.Text.Length;
                    logTB.ScrollToCaret();
                }

                logTB.Update();
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
                if (line != "-1")
                {
                    infoLBL.Text = line;
                    infoLBL.Update();
                }
            }
        }



        //writes config data from walletlist index to corresponding eon folder
        private void WriteConfig(int index)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(WalletList[index].ConfigJson);
                File.WriteAllText(appPath + @"\eon" + index.ToString() + @"\config.json", jsonData);
            }
            catch(Exception ex)
            {
                DebugLogMsg("WriteConfig(" + index.ToString() + ") exception : " + ex.Message);
            }
        }

        // returns the raw cmds response, or -1 for failure
        public string ExecuteCommandSync(object command)
        {
            string result = "-1";

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
            catch (Exception ex)
            {
                // Log the exception
                DebugMsg("ExecuteCommandSync: Exception raised executing eon.exe  - " + ex.Message);
                result = "-1";
            }

            return (result);
        }
 
        //return the raw command response , or -1 for fail. Wraps ExecuteCommandSync to allow exec in eon folder
        private string EonCMD(int index, string cmd, bool enableLog)
        {
            string res = "-1";

            try
            {
                Directory.SetCurrentDirectory(Path.GetDirectoryName(appPath + @"\eon" + index + @"\"));
                res = ExecuteCommandSync(cmd);
                Directory.SetCurrentDirectory(appPath);
                if (enableLog) LogMsg(res + "\r\n");
            }
            catch (Exception ex)
            {
                DebugLogMsg("ERROR during EON command execution (walletIndex" + index.ToString() + ") :\r\nCMD: " + cmd + "\r\n\r\nException: " + ex.Message);
                res = "-1";
            }
                return (res);
        }

        private string EonCMD(int index, string cmd)
        {
            return(EonCMD(index, cmd, true));
        }



        private void CommitBTN_Click(object sender, EventArgs e)
        {
            string commitResponse = EonCMD(selectedWallet, "eon allcommit " + accountTB.Text);
            if (commitResponse!="-1") UpdateCommited(commitResponse);           
        }

        //clear log view
        private void Button1_Click(object sender, EventArgs e)
        {
           logTB.Text = "";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            eonThreadRun = false;
            Thread.Sleep(1);
        }

        private void EonCommandLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //launch eon command line
            try
            {
                Directory.SetCurrentDirectory(appPath + @"\eon" + selectedWallet);
                Process.Start("cmd.exe", "/k");
                Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            }
            catch(Exception ex)

            {
                DebugMsg("Exception trying to open eon command terminal - " + ex.Message + "\r\n");
                LogMsg("Exception trying to open eon command terminal - " + ex.Message + "\r\n");
            }
        }

        private void OpensslCommandLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //launch openssl command line
            try
            { 
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\openssl-0.9.8r-x64_86-win64-rev2");
            Process.Start("cmd.exe", "/k");
            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            }
            catch (Exception ex)

            {
                DebugMsg("Exception laucnhing openssl command terminal - " + ex.Message);
                LogMsg("Exception trying to opening command terminal - " + ex.Message);
            }
        }

        //read a specific config.json file into the corresponding eon wallet 
        private bool ReadConfigObject(int index)
        {
            bool res = false;

            try
            {
                string filename = appPath + @"\eon" + index.ToString() + @"\config.json";
                string newJSON = File.ReadAllText(filename);
                configClass thisConf = new configClass();

                try
                {
                    thisConf = JsonConvert.DeserializeObject<configClass>(newJSON);
                }
                catch (Exception ex)
                {
                    DebugMsg("Error deserializing json file - bad format? : " + ex.Message);
                    return false;
                }
                               
                //save this config object in the walletlist & create the config file
                WalletList[index].ConfigJson = thisConf;

                DebugMsg("config.json read OK\r\n");
                res = true;

            }
            catch (Exception ex)
            {
                DebugMsg("Error: Could not read file. [ " + ex.Message + "]");
                res = false;
            }

            return res;

        }
       

        private bool ImportWalletsJson()
        {
            bool res = false;

            //import a json file, overwrites existing one
            OpenFileDialog importDG = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Filter = "wallets.json files (*.json)|*.json",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (importDG.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filename = importDG.FileName;
                    

                    try
                    {
                        WalletList = ReadFromJsonFile<List<WalletClass>>(filename);
                    }
                    catch (Exception ex)
                    {
                        DebugMsg("Error deserializing json file - bad format? : " + ex.Message);
                        return false;
                    }


                    DebugMsg("wallets.json import OK\r\n");
                    res = true;

                }
                catch (Exception ex)
                {
                    DebugMsg("Error reading file : [ " + ex.Message + "]");
                    res = false;
                }
            }

            return res;

        }

        //use a dialog to select and import a config.json into the selected index in WalletList and the corresponding eon folder
        private bool ImportJsonDLG(int index)
        {
            bool res = false;

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
                    configClass thisConf = new configClass();

                    try
                    {
                        thisConf = JsonConvert.DeserializeObject<configClass>(newJSON);
                    }
                    catch (Exception ex)
                    {
                        DebugMsg("Error deserializing json file - bad format? : " + ex.Message);
                        return false;
                    }

                    //UpdateConfigDisplay();

                    //save this config object in the walletlist & create the config file
                    WalletList[index].ConfigJson = thisConf;
                    WriteConfig(index);

                    //get the account info and populate the GUI
                    //debugTB.Text = "";
                    //string res = EonCMD("eon info");
                    //UpdateAccountInfo(res);
                    //registerWarning = false;
                    //UpdateBalance(EonCMD("eon state"));

                    DebugMsg("config.json import OK\r\n");
                    res = true;

                }
                catch (Exception ex)
                {
                    DebugMsg("Error: Could not read file. [ " + ex.Message + "]");
                    res = false;
                }
            }

            return res;
        }


        private void ImportWalletsjsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //stop the eonthread
                eonThreadRun = false;

                //show file requester for importing wallets.json and rebuild the working folders
                if (ImportWalletsJson())
                {
                    RebuildWorkingFolders(true);
                }



            }
            catch(Exception ex)
            {
                DebugMsg("Error importing json, check it is valid - " + ex.Message);
                LogMsg("Error importing json, check it is valid - " + ex.Message);
            }

        }

        private void ExportWalletsJsonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //export the json file somewhere.
            SaveFileDialog jsonDG = new SaveFileDialog
            {
                Filter = "wallets.json files (*.json)|*.json",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            try
            {
                if (jsonDG.ShowDialog() == DialogResult.OK)
                {

                    string filename = jsonDG.FileName;

                    try
                    {
                        WriteToJsonFile(filename, WalletList, false);
                        DebugLogMsg("Wallets.json export OK\r\n");
                    }
                    catch (Exception ex)
                    {
                        DebugLogMsg("SaveWalletList() Exception saving wallets.json : " + ex.Message);
                    }
                    
                }
            }
            catch(Exception ex)
            {
                DebugMsg("Error exporting config file  : " + ex.Message + "\r\n");
                LogMsg("Error exporting config file  : " + ex.Message);
            }
        


        }

        //open the working folder in explorer
        private void OpenWorkingFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //launch eon command line
                Process.Start("explorer.exe", appPath + @"\eon" + selectedWallet);
            }
            catch (Exception ex)
            {
                DebugMsg("Error opening working folder  : " + ex.Message + "\r\n");
                LogMsg("Error opening working folder  : " + ex.Message + "\r\n");
            }
        }

        private void RefillBTN_Click(object sender, EventArgs e)
        {
            DialogResult res;

            using (DialogCenteringService centeringService = new DialogCenteringService(this)) // center message box
            {
                res = MessageBox.Show("Transfer " + balDepAmountTB.Text + " to deposit?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            }

            if (res == DialogResult.Yes)
            {
                //code for Yes
                string refillResp = EonCMD(selectedWallet, "eon refill " + balDepAmountTB.Text);

                if (refillResp != "-1")
                {
                    try
                    {
                        string pt = @"Amount:\s(\d*.\d*)\s*([^\n]*)";
                        Match rfMatch = Regex.Match(refillResp, pt);

                        if (rfMatch.Groups.Count == 3)
                        {
                            string amount = rfMatch.Groups[1].ToString();
                            string resultS = rfMatch.Groups[2].ToString();


                            using (DialogCenteringService centeringService = new DialogCenteringService(this)) // center message box
                            {
                                MessageBox.Show("Refill " + amount + " : " + resultS);
                                DebugClear();
                                DebugMsg(refillResp + "\r\n");
                            }

                        }
                    }
                    catch(Exception ex)
                    {
                        DebugMsg("Exception parsing refill response" + ex.Message + "\r\n");
                        LogMsg("Exception parsing refill response" + ex.Message + "\r\n");
                    }
                }
                else
                {
                    DebugMsg("Refill failed : " + refillResp + "\r\n");
                    LogMsg("Refill failed  : " + refillResp + "\r\n");
                }

            }
            else if (res == DialogResult.No)
            {
                //code for No
                DebugMsg("Refill cancelled\r\n");
            }
            
        }

        private void WithdrawBTN_Click(object sender, EventArgs e)
        {

            
            DialogResult res;

            using (DialogCenteringService centeringService = new DialogCenteringService(this)) // center message box
            {
                res = MessageBox.Show("Withdraw " + balDepAmountTB.Text + " from deposit?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            }

            if (res == DialogResult.Yes)
            {
                string wdResp = EonCMD(selectedWallet, "eon withdraw " + balDepAmountTB.Text);

                if (wdResp != "-1")
                {
                    try
                    {
                        string pt = @"Amount:\s(\d*.\d*)\s*([^\n]*)";
                        Match wdMatch = Regex.Match(wdResp, pt);

                        if (wdMatch.Groups.Count == 3)
                        {
                            string amount = wdMatch.Groups[1].ToString();
                            string resultS = wdMatch.Groups[2].ToString();

                            //MessageBox.Show();
                            using (DialogCenteringService centeringService = new DialogCenteringService(this)) // center message box
                            {
                                MessageBox.Show("Withdraw " + amount + " : " + resultS);
                                DebugClear();
                                DebugMsg(wdResp + "\r\n");
                            }
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        DebugMsg("Exception parsing withdraw response" + ex.Message + "\r\n");
                        LogMsg("Exception parsing withdraw response" + ex.Message + "\r\n");
                    }
                }
                else
                {
                    DebugMsg("Withdraw failed : " + wdResp + "\r\n");
                    LogMsg("Withdraw failed  : " + wdResp + "\r\n");
                }
            

            }
            else if (res == DialogResult.No)
            {
                //code for No
                DebugMsg("Withdraw cancelled\r\n");
            }

        }

        private void TxSendBTN_Click(object sender, EventArgs e)
        {
            
            string recipientAddress = txRecipientTB.Text;

            DialogResult res;

            using (DialogCenteringService centeringService = new DialogCenteringService(this)) // center message box
            {
                res = MessageBox.Show("Send " + txAmountTB.Text + " from " + WalletList[selectedWallet].AccountID + "\r\n to account : " + recipientAddress + "?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            }

            if (res == DialogResult.Yes)
            {
                //code for Yes
                string txResp = EonCMD(selectedWallet, "eon payment " + recipientAddress + " " + txAmountTB.Text);

                if (txResp != "-1")
                {
                    try
                    {

                        //extract the values
                        string pattern = @"account:([^\s]*)\s*Amount:\s(\d*.\d*)\s*([^\n]*)";
                        Match paymentResultMatch = Regex.Match(txResp, pattern);

                        string rAccount = paymentResultMatch.Groups[1].ToString();
                        string rAmount = paymentResultMatch.Groups[2].ToString();
                        string resultS = paymentResultMatch.Groups[3].ToString();

                        /*
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
                        */

                        //MessageBox.Show();
                        using (DialogCenteringService centeringService = new DialogCenteringService(this)) // center message box
                        {
                            MessageBox.Show("Send " + rAmount + " from " + WalletList[selectedWallet].AccountID + " to " + rAccount + " : " + resultS);

                            DebugClear();
                            DebugMsg(txResp + "\r\n");
                        }
                    }
                    catch(Exception ex)
                    {
                        DebugMsg("Exception parsing send response" + ex.Message + "\r\n");
                        LogMsg("Exception parsing send response" + ex.Message + "\r\n");
                    }
                }
                else
                {
                    DebugMsg("Send failed : " + txResp + "\r\n");
                    LogMsg("Send failed  : " + txResp + "\r\n");
                }

            }
            else if (res == DialogResult.No)
            {
                //code for No
                DebugMsg("Send cancelled\r\n");
            }
        }

        private void OpenExscudoRegistationWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://testnet.eontechnology.org/");
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show();
            using (DialogCenteringService centeringService = new DialogCenteringService(this)) // center message box
            {
                MessageBox.Show("Exscudo Testnet GUI\r\n\r\nNon-official test net tool\r\n\r\nexscudo slack user : gassman\r\nOct 2017","About");
            }
        }

        private void QuitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            eonThreadRun = false;
            Application.Exit();
        }

        private void UncommitBTN_Click(object sender, EventArgs e)
        {
            string commitResponse = EonCMD(selectedWallet, "eon uncommit " + accountTB.Text);
            if (commitResponse != "-1") UpdateCommited(commitResponse);
        }

        /// <summary>
        /// Writes the given object instance to a Json file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
        /// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [JsonIgnore] attribute.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToJsonFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite);
                writer = new StreamWriter(filePath, append);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        /// <summary>
        /// Reads an object instance from an Json file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object to read from the file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the Json file.</returns>
        public static T ReadFromJsonFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                reader = new StreamReader(filePath);
                var fileContents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(fileContents);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        private void AccountLV_SelectionChanged(object sender, EventArgs e)
        {
            if ((0 <= accountLV.SelectedIndex) && (accountLV.SelectedIndex <= (WalletList.Count - 1)))
            {
                if (accountLV.SelectedIndex != lastWalletIndex)
                {
                    selectedWallet = accountLV.SelectedIndex;
                    accountTB.Text = WalletList[selectedWallet].AccountID;
                    pubkeyTB.Text = WalletList[selectedWallet].PublicKey;
                    UpdateConfigDisplay(selectedWallet);
                    SyncGUIBalances();
                    lastWalletIndex = accountLV.SelectedIndex;
                }
            }
        }
        

        private void RebuildWorkingFoldersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //allow eon thread to shut down
            eonThreadRun = false;
            Thread.Sleep(2);

            RebuildWorkingFolders(true);
        }

        private bool RebuildWorkingFolders(bool restoreWallets)
        {
            bool res = false;

            //clear the gui elements
            accountTB.Text = "";
            pubkeyTB.Text = "";
            rxAddressLBL.Text = "";
            balanceLBL.Text = "";
            depositLBL.Text = "";
            accountLV.ClearObjects();
            transactionLV.ClearObjects();


            
            try
            {
                //delete workingfolders
                DirectoryInfo di = new System.IO.DirectoryInfo(appPath + @"\");
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }

                Thread.Sleep(500);

                //re-save wallets.json if we are restoring it
                if (restoreWallets)
                {
                    SaveWalletList();
                }
                else //otherwise restore the default conditions of fresh start
                {
                    WalletList.Clear();
                    mainWallet = new WalletClass();
                    WalletList.Add(mainWallet);
                    
                }
                
                //set the primary wallet as default selected
                selectedWallet = 0;
                
                //rebuild working folders (restart the eon thread now)
                eonThreadRun = true;

                //start the eon thread
                eonThread = new Thread(new ThreadStart(EonThreadStart));
                eonThread.SetApartmentState(ApartmentState.STA);
                eonThread.Start();

                res = true;

            }
            catch (Exception ex)
            {
                DebugMsg("ImportWallets() failed to delete working folders : [ " + ex.Message + "]");
            }

            return res;
        }


        private void ResetAllConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //allow eon thread to shut down
            eonThreadRun = false;
            Thread.Sleep(2);

            //rebuild and restart, without restoring wallets.json
            RebuildWorkingFolders(false);
        }

        //create new account button
        private void CreateAccountBTN_Click(object sender, EventArgs e)
        {
            //allow eon thread to shut down
            //eonThreadRun = false;

            //show a dialog and get a nickname for the account
            CreateNewAccountDialog naDialog = new CreateNewAccountDialog();

            naDialog.ShowDialog();

            if (naDialog.result == "Create")
            {
                string nick = naDialog.nickname;

                int walletIndex = WalletList.Count;

                //create a new SEED
                string seedResponse = EonCMD(0, "eon seed");
                DebugLogMsg(seedResponse);
                string pattern = @"Seed:\s*(.*)\s*Account:\s*(.*)\s*Public key:\s*(.*)";
                Match srMatch = Regex.Match(seedResponse, pattern);


                string seed = srMatch.Groups[1].ToString();
                string accountID = srMatch.Groups[2].ToString();
                string publicKey = srMatch.Groups[3].ToString();

                //register this new account (its not primary)
                string createResponse = EonCMD(0, "eon new_account " + publicKey);
                DebugLogMsg(createResponse);
                string pattern2 = @"send transaction.*<<\s*""(.*)""";
                Match crMatch = Regex.Match(createResponse, pattern2);
                string createResult = crMatch.Groups[1].ToString();
                
                if (createResult == "success")
                {
                    //create a new wallet in our list
                    WalletClass wal = new WalletClass
                    {
                        AccountID = accountID,
                        PublicKey = publicKey,
                        Seed = seed,
                        NickName = nick
                    };
                    WalletList.Add(wal);

                    SetupEonFolder(walletIndex);

                    //new account has been created.  update the walletlist item and the config file
                    ReadConfigObject(walletIndex);
                    WalletList[walletIndex].Seed = seed.Trim();
                    WalletList[walletIndex].AccountID = accountID.Trim();
                    WalletList[walletIndex].PublicKey = publicKey.Trim();
                    if (walletIndex == 0) WalletList[walletIndex].NickName = nick.Trim();
                    WalletList[walletIndex].ConfigJson.Payouts.Seed = seed.Trim();
                    WriteConfig(walletIndex);
                    SaveWalletList();

                }

            }


        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex==3)
            {
                logTB.Focus();
                logTB.SelectionStart = logTB.Text.Length;
                logTB.ScrollToCaret();
                logTB.Update();
            }
        }
    }


}
