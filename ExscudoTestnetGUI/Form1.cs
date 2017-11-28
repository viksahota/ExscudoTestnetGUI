using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using ExscudoTestnetGUI.Properties;
using Newtonsoft.Json;

namespace ExscudoTestnetGUI
{

    public partial class Form1 : Form
    {

        private int _selectedWallet;

        //create wallet list and the wallet object for the main account. Additional accounts can be added
        private List<WalletClass> _walletList = new List<WalletClass>();
        private WalletClass _mainWallet = new WalletClass();

        //create an object for swapTest data
        private SwapTestDataClass _swapData = new SwapTestDataClass();

        // These delegates enable asynchronous calls to gui from other threads
        private delegate void SetDebugCallback(string text);
        private delegate void SetDebugClearCallback();
        private delegate void SetLogCallback(string text);
        private delegate void SetInfTextCallback(string text);
        private delegate void SetConfigUpdateCallback(int index);
        private delegate void SetAccountInfoCallback(string infoResponse);
        private delegate void SetUpdateCommitedCallback(string text);
        private delegate void SetSyncGuiBalancesCallback();
        private delegate void SetSelectedAccountCallback(int index);
        private delegate void SetUpdateSwapTestAccountLisCallback();
        private delegate void SetUpdateSwapTestGUICallback();

        //define a thread which will process away from form thread.
        private Thread _eonThread;
        private bool _eonThreadRun = true;
        private readonly bool _os64Bit;
        //so we know when the selected wallet is really changed, can be spurious events
        private int _lastWalletIndex = -1;

        //define a thread dedicated to swapTest
        private Thread _swapTestThread;
        private bool _swapTestThreadRun = true;

        private readonly string _appPath;
                
        public Form1()
        {
            InitializeComponent();

            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            _appPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\ExscudoTestnetGUI";

            _os64Bit = Environment.Is64BitOperatingSystem;

            _walletList.Add(_mainWallet);

            //set the primary wallet as default selected
            _selectedWallet = 0;
            

            //start the eon thread
            _eonThread = new Thread(EonThreadStart);
            _eonThread.SetApartmentState(ApartmentState.STA);
            _eonThread.Start();

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
            DebugLogMsg("Working directory : " + _appPath + "\r\n");


            //set up the eon folders and wallets
            try
            {

                if (SetupEonFolder(0))
                {
                    //try and load stored walletslist
                    if (File.Exists(_appPath + @"\wallets.json"))
                    {
                        _walletList = ReadFromJsonFile<List<WalletClass>>(_appPath + @"\wallets.json");
                       
                    }

                    //setup the main wallet
                    SetupWallet(0);

                    //setup folders for each attached account
                    if (_walletList.Count>1)
                    {
                        for (int i=1; i < _walletList.Count ; i++)
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
            accountLV.SetObjects(_walletList);

            //update the account lists on the transaction-swap test tab
            UpdateSwapTestAccountList();

            SetSelectedAccount(0);
            SyncGuiBalances();
            
            int counter = 0;
            DateTime lastBalancePollTime = DateTime.Now;

            while (_eonThreadRun)
            {
                Thread.Sleep(50);
                //every 5 seconds update the main account balance/deposit
                //query EON
                TimeSpan elapsed = DateTime.Now - lastBalancePollTime;
                if (elapsed.Seconds >= 10)
                {
                    if (_eonThreadRun) UpdateBalances();
                    if (_eonThreadRun) SyncGuiBalances();
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

        private void UpdateSwapTestAccountList()
        {
            if (swapAccountA_CB.InvokeRequired)
            {
                SetUpdateSwapTestAccountLisCallback d = UpdateSwapTestAccountList;
                Invoke(d, new object[] { });
            }
            else
            {
                foreach (WalletClass wal in _walletList)
                {
                    swapAccountA_CB.Items.Add(wal.NickName + " : " + wal.AccountID);
                    swapAccountB_CB.Items.Add(wal.NickName + " : " + wal.AccountID);
                }
            }

        }

        private void GetAttachedAccounts()
        {
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
                    var oResp = JsonConvert.DeserializeObject<commitClass>(jsonResponse);

                    //reverse the transactions order so we will locate the attached accounts in the created order
                    Array.Reverse(oResp.All);
                    

                    foreach (commitClass.All2 m in oResp.All)
                    {
                        if (m.Type == 100)
                        {
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

                                //get new account info
                                string nPattern = @"""([^\s]*)"":\s""([^\s]*)""";
                                Match newAccountMatch = Regex.Match(transactionJson, nPattern);

                                string newAccountId = newAccountMatch.Groups[1].ToString();

                                //scan if we already have this accountID in the walletList
                                int matchVal = -1;
                                for (int a = 0; a < _walletList.Count; a++)
                                {
                                    if (_walletList[a].AccountID == newAccountId) matchVal = a;
                                }

                                AccountSeedDialog asDialog;
                                if (matchVal == -1)  //entry does not exist in walletlist, create it and ask for seed.
                                {
                                    //create a new wallet in our list
                                    WalletClass wal = new WalletClass
                                    {
                                        AccountID = newAccountId
                                    };
                                    _walletList.Add(wal);

                                    //request the private-key (SEED) for this account from the user
                                    asDialog = new AccountSeedDialog();
                                    asDialog.setAccountLabel(wal.AccountID);
                                    asDialog.ShowDialog();

                                    //if the seed and nickname were supplied, update the wallet class
                                    if (asDialog.result)
                                    {
                                        //store the nickname and seed
                                        _walletList[_walletList.Count - 1].Seed = asDialog.seedVal;
                                        _walletList[_walletList.Count - 1].NickName = asDialog.nickName;
                                        _walletList[_walletList.Count - 1].ConfigJson.Payouts.Seed = asDialog.seedVal;

                                        //get the public key
                                        string infoResponse = EonCMD(0, "eon info " + asDialog.seedVal);
                                        Match infoMatch = Regex.Match(infoResponse, @"Seed:\s*(.*)\s*Account:\s*(.*)\s*Public key:\s*(.*)\s*end...");

                                        if (infoMatch.Groups.Count == 4)
                                        {
                                            string pubkeyString = infoMatch.Groups[3].ToString();
                                            _walletList[_walletList.Count - 1].PublicKey = pubkeyString;
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
                                    _walletList[matchVal].Seed = _walletList[matchVal].Seed.Trim();

                                    //if the seed is not set, request it from the user
                                    if (_walletList[matchVal].Seed.Length != 64)
                                    {
                                        asDialog = new AccountSeedDialog();
                                        asDialog.setAccountLabel(newAccountId);
                                        asDialog.ShowDialog();

                                        //if the seed and nickname were supplied, update the wallet class
                                        if (asDialog.result)
                                        {
                                            //store the nickname and seed
                                            _walletList[_walletList.Count - 1].Seed = asDialog.seedVal;
                                            _walletList[_walletList.Count - 1].NickName = asDialog.nickName;
                                            _walletList[_walletList.Count - 1].ConfigJson.Payouts.Seed = asDialog.seedVal;
                                            Thread.Sleep(1000);

                                        }

                                    }

                                    //if the seed is ok , pass through....

                                }


                                //report the account in the view
                                DebugMsg("Attached account detected: " + newAccountId + "\r\n");
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
            for(int i=0; i<_walletList.Count; i++)
            {
                string stateResponse = EonCMD(i, "eon state",false);
                if ((stateResponse != "-1")&&(_eonThreadRun))
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
                                _walletList[i].Balance = (Convert.ToDecimal(stateMatches.Groups[3].ToString()) / 1000000).ToString(CultureInfo.InvariantCulture);
                                _walletList[i].Deposit = (Convert.ToDecimal(stateMatches.Groups[4].ToString()) / 1000000).ToString(CultureInfo.InvariantCulture);
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
                                _walletList[i].Seed = _walletList[i].Seed.Trim();
                                _walletList[i].ConfigJson.Payouts.Seed = _walletList[i].ConfigJson.Payouts.Seed.Trim();
                                _walletList[i].PublicKey = _walletList[i].PublicKey.Trim();
                                _walletList[i].AccountID = _walletList[i].AccountID.Trim();
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
        private void SyncGuiBalances()
        {
            if (balanceLBL.InvokeRequired)
            {
                SetSyncGuiBalancesCallback d = SyncGuiBalances;
                Invoke(d, new object[] {  });
            }
            else
            {
                //refresh if needed
                if (balanceLBL.Text != _walletList[_selectedWallet].Balance) balanceLBL.Text = _walletList[_selectedWallet].Balance;
                if (depositLBL.Text != _walletList[_selectedWallet].Deposit) depositLBL.Text = _walletList[_selectedWallet].Deposit;
                if (rxAddressLBL.Text != _walletList[_selectedWallet].AccountID) rxAddressLBL.Text = _walletList[_selectedWallet].AccountID;
                accountLV.BuildList(true);

                //update the swapTest account balances
                if (swapAccountA_CB.SelectedIndex >-1) swaptestActBalA_lbl.Text = _walletList[swapAccountA_CB.SelectedIndex].Balance;
                if (swapAccountB_CB.SelectedIndex >-1) swaptestActBalB_lbl.Text = _walletList[swapAccountB_CB.SelectedIndex].Balance;

            }
        }

        //sets the selected account index, needs to be set to primary on startup, as default.
        private void SetSelectedAccount(int index)
        {
            if (balanceLBL.InvokeRequired)
            {
                SetSelectedAccountCallback d = SetSelectedAccount;
                Invoke(d, index);
            }
            else
            {
                    _selectedWallet = index;
                    accountLV.SelectedIndex = index;
                    accountLV.UnfocusedSelectedBackColor = Color.BlanchedAlmond;
                    accountLV.Update();
                    UpdateConfigDisplay(_selectedWallet);

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
                            if (Directory.Exists(_appPath + @"\eon" + instance))
                            {
                                if (File.Exists(_appPath + @"\eon" + instance + @"\eon.exe"))
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
                            DebugLogMsg("SetupEonInstance(" + instance + ") state0 exception : " + ex.Message);
                            cstate = 99;
                        }
                        break;

                    case (1):
                        try
                        {
                            Directory.CreateDirectory(_appPath + @"\eon" + instance);
                            cstate = 2;
                        }
                        catch(Exception ex)
                        {
                            DebugLogMsg("SetupEonInstance(" + instance + ") state1 exception : " + ex.Message);
                            cstate = 99;
                        }
                        break;

                    case (2):
                        try
                        {
                            //deploy eon client
                            if (_os64Bit)
                            {
                                File.WriteAllBytes(_appPath + @"\eon" + instance + @"\eon.exe", Resources.eon64);
                                if (instance == 0) DebugLogMsg("eon (x64) deployed\r\n");
                            }
                            else
                            {
                                File.WriteAllBytes(_appPath + @"\eon" + instance + @"\eon.exe", Resources.eon32);
                                if (instance == 0) DebugLogMsg("eon (x32) deployed\r\n");
                            }

                            File.WriteAllText(_appPath + @"\eon" + instance + @"\config.json", Resources.config);
                            File.WriteAllText(_appPath + @"\eon" + instance + @"\Readme.md", Resources.ReadMe);

                            DebugMsg("Checking in case antivirus software removes eon.exe ...");
                            int i = 4;
                            while (i > 0)
                            {
                                DebugMsg(i + "...");
                                Thread.Sleep(1000);
                                i--;
                            }
                            DebugMsg("\r\n");

                            //test eon.exe
                            if (File.Exists(_appPath + @"\eon" + instance + @"\eon.exe"))
                            {
                                cstate = 3;
                                DebugMsg("eon OK\r\n");
                            }
                            else
                            {
                                MessageBox.Show(@"\eon" + instance + @"\eon.exe" + " is missing, this can occur since some antivirus tools can\r\nidentify eon.exe as a virus and quarantine it\r\nPlease check you antivirus quarantine, restore eon.exe, and restart this application");
                                DebugLogMsg("SetupEonInstance() : eon.exe is missing, removed by antivirus?\r\n");
                                cstate = 99;
                            }

                        }
                        catch (Exception ex)
                        {
                            DebugLogMsg("SetupEonInstance(" + instance + ") state2 exception : " + ex.Message);
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
            if (cstate == 99) return false;
            return false;
        }

        private bool SaveWalletList()
        {
            bool res = false;
            try
            {
                WriteToJsonFile(_appPath + @"\wallets.json", _walletList, false);
                res = true;
            }
            catch (Exception ex)
            {
                DebugLogMsg("SaveWalletList() Exception saving wallets.json : " + ex.Message);
                res = false;
            }
            return (res);
        }


        private void SetupWallet(int walletIndex)
        {
            int cstate = 0;
            SetupWalletDialog swDialog = new SetupWalletDialog();


            while ((cstate != 9)&&(cstate != 99))
            {
                switch (cstate)
                {
                    case (0):
                        //check if there is an entry in WalletList for the requested instance
                        if ((walletIndex + 1) <= _walletList.Count)
                        {
                            _walletList[walletIndex].Seed = _walletList[walletIndex].Seed.Trim();
                            _walletList[walletIndex].PublicKey = _walletList[walletIndex].PublicKey.Trim();
                            _walletList[walletIndex].AccountID = _walletList[walletIndex].AccountID.Trim();

                            //check if SEED is not yet set for this account
                            cstate = _walletList[walletIndex].Seed.Length != 64 ? 1 : 6;

                        }
                        break;

                    case 1:

                        //show dialog to either create a new account, import a config.json, or paste a SEED

                        swDialog.ShowDialog();
                        //swDialog.Show();

                        if (swDialog.result == "Create") cstate = 4;
                        else if (swDialog.result == "Import") cstate = 2;
                        else if (swDialog.result == "Set") cstate = 3;
                        else if (swDialog.result == "Cancel") cstate = 99;

                        break;

                    case 2:
                        if (ImportJsonDlg(walletIndex))
                        {
                           _walletList[walletIndex].Seed = _walletList[walletIndex].ConfigJson.Payouts.Seed;
                            cstate = 5;
                        }
                        //if it fails, return to selection
                        else cstate = 2;

                        break;

                    case 3:

                        //set the pasted SEED value ----------------------------------

                        try
                        {
                            //read the config file into the wallet list
                            ReadConfigObject(walletIndex);

                            //adjust the SEED
                            _walletList[walletIndex].Seed = swDialog.seedVal;
                            _walletList[walletIndex].ConfigJson.Payouts.Seed = swDialog.seedVal;

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

                    case 4:

                        //create a new SEED
                        string seedResponse = EonCMD(0, "eon seed");
                        DebugLogMsg(seedResponse);
                        string pattern = @"Seed:\s*(.*)\s*Account:\s*(.*)\s*Public key:\s*(.*)";
                        Match srMatch = Regex.Match(seedResponse, pattern);


                        string seed = srMatch.Groups[1].ToString();
                        string accountId = srMatch.Groups[2].ToString();
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
                                _walletList[walletIndex].Seed = seed;
                                _walletList[walletIndex].AccountID = accountId;
                                _walletList[walletIndex].PublicKey = publicKey;
                                if (walletIndex == 0) _walletList[walletIndex].NickName = "";
                                _walletList[walletIndex].ConfigJson.Payouts.Seed = seed;
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
                            _walletList[walletIndex].Seed = seed;
                            _walletList[walletIndex].AccountID = accountId;
                            _walletList[walletIndex].PublicKey = publicKey;
                            if (walletIndex == 0) _walletList[walletIndex].NickName = "Primary";
                            _walletList[walletIndex].ConfigJson.Payouts.Seed = seed;
                            WriteConfig(walletIndex);
                            SaveWalletList();

                            DebugLogMsg("~~~  New account has been created - register it with Exscudo. You will need these details : ~~~\r\nYour email\r\nYour account ID : " + accountId + "\r\nYour public key : " + publicKey + "\r\n" + @"Register at :  https://testnet.eontechnology.org/" + "\r\n");
                            DebugLogMsg("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\r\n");

                            MessageBox.Show("New account has been created but needs registering with Exscudo. Check the debug log for details.\r\nOnce you've registered, restart this application");

                            cstate = 6;
                        }

                        break;


                    case 5:
                        //get wallet info from imported/pasted seed and populate the wallet object / walletlist
                        string infoResponse = EonCMD(walletIndex, "eon info");
                        Match infoMatch = Regex.Match(infoResponse, @"Seed:\s*(.*)\s*Account:\s*(.*)\s*Public key:\s*(.*)\s*end...");

                        if (infoMatch.Groups.Count == 4)
                        {
                            string accountString = infoMatch.Groups[2].ToString();
                            string pubkeyString = infoMatch.Groups[3].ToString();

                            _walletList[walletIndex].AccountID = accountString;
                            _walletList[walletIndex].PublicKey = pubkeyString;
                            if (walletIndex == 0) _walletList[walletIndex].NickName = "Primary";

                            cstate = 0;
                            
                        }
                        else
                        {
                            DebugLogMsg("SetupWallet() state5 - Error parsing info response : " + infoResponse);
                            cstate = 99;
                        }


                        break;

                    case 6:

                        //check if account is active?
                        WriteConfig(walletIndex);
                        int checkResult = CheckAccountState(walletIndex);

                        if (checkResult == 1) cstate = 7;
                        else if (checkResult == -1) cstate = 99;
                        else if (checkResult == 2) cstate = 8;  //store and finish if its unregistered


                        break;

                    case 7:
                        //check if primary account, if so get the attached accounts and update walletList
                        if (walletIndex == 0)
                        {
                            //find the attached accounts and populate walletlist with accountID & public key
                            GetAttachedAccounts();
                            cstate = 8;
                        }
                        else cstate = 8;

                        break;

                    case 8:
                        //write out the config file and store walletList to json file
                        WriteConfig(walletIndex);
                        SaveWalletList();
                        cstate = 9;

                        break;
                }
            }
        }


        //updates the GUI account information fields. takes the 'eon info' response as its input and invokes itself on form thread
        private void UpdateAccountInfo(string infoResponse)
        {
            if (accountTB.InvokeRequired)
            {
                SetAccountInfoCallback d = UpdateAccountInfo;
                Invoke(d, infoResponse);
            }
            else
            {
                try
                {
                    Match infoMatch = Regex.Match(infoResponse, @"Seed:\s*(.*)\s*Account:\s*(.*)\s*Public key:\s*(.*)\s*end...");

                    if(infoMatch.Groups.Count==4)
                    {
                        _mainWallet.Seed = infoMatch.Groups[1].ToString();
                        _mainWallet.AccountID = infoMatch.Groups[2].ToString();
                        _mainWallet.PublicKey = infoMatch.Groups[3].ToString();
                        accountTB.Text = _mainWallet.AccountID;
                        rxAddressLBL.Text = _mainWallet.AccountID;
                        pubkeyTB.Text = _mainWallet.PublicKey;
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
                SetUpdateCommitedCallback d = UpdateCommited;
                Invoke(d, commitResponse);
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
                            var oResp = JsonConvert.DeserializeObject<commitClass>(jsonResponse);

                            
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
                            transactionLV.SetObjects(oResp.All);
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
            }
        }
        
        private void UpdateConfigDisplay(int index)
        {
            if (rootThreadsNM.InvokeRequired)
            {
                SetConfigUpdateCallback d = UpdateConfigDisplay;
                Invoke(d, index);
            }
            else
            {
                try
                {
                    rootThreadsNM.Value = _walletList[index].ConfigJson.Threads;
                    rootCoinTB.Text = _walletList[index].ConfigJson.Coin;
                    rootNameTB.Text = _walletList[index].ConfigJson.Name;

                    //remove the s from the interval and convert to int
                    string intString = _walletList[index].ConfigJson.UpstreamCheckInterval;
                    intString = intString.Remove(intString.Length - 1, 1);
                    //int interval = intString;
                    int interval = Convert.ToInt16(intString);
                    rootIntervalNM.Value = interval;
                    //update the payouts enable checkbox
                    payoutEnabledCB.Checked = _walletList[index].ConfigJson.Payouts.Enabled;

                    //remove the m from the interval and convert to int
                    string intString2 = _walletList[index].ConfigJson.Payouts.Interval;
                    intString2 = intString2.Remove(intString2.Length - 1, 1);
                    int interval2 = Convert.ToInt16(intString2);
                    payoutIntervalNM.Value = interval2;

                    payoutPeerTB.Text = _walletList[index].ConfigJson.Payouts.Peer;

                    payoutSeedTB.Text = _walletList[index].ConfigJson.Payouts.Seed;

                    payoutDeadlineNM.Value = _walletList[index].ConfigJson.Payouts.Deadline;

                    payoutFeeNM.Value = _walletList[index].ConfigJson.Payouts.Fee;

                    //update the payouts Raw checkbox
                    payoutRawCB.Checked = _walletList[index].ConfigJson.Payouts.Raw;

                    //remove the s from the timeout and convert to int
                    string intString3 = _walletList[index].ConfigJson.Payouts.Timeout;
                    intString3 = intString3.Remove(intString3.Length - 1, 1);
                    int timeoutVal = Convert.ToInt16(intString3);
                    payoutTimeoutNM.Value = timeoutVal;
                    
                    payoutThresholdNM.Value = _walletList[index].ConfigJson.Payouts.Threshold;
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
            _walletList[_selectedWallet].ConfigJson.Threads = (byte)rootThreadsNM.Value;
            _walletList[_selectedWallet].ConfigJson.Coin = rootCoinTB.Text;
            _walletList[_selectedWallet].ConfigJson.Name = rootNameTB.Text;
            _walletList[_selectedWallet].ConfigJson.UpstreamCheckInterval = rootIntervalNM.Value.ToString(CultureInfo.InvariantCulture) + "s";
            _walletList[_selectedWallet].ConfigJson.Payouts.Enabled = payoutEnabledCB.Checked;
            _walletList[_selectedWallet].ConfigJson.Payouts.Interval = payoutIntervalNM.Value.ToString(CultureInfo.InvariantCulture) + "m";
            _walletList[_selectedWallet].ConfigJson.Payouts.Peer = payoutPeerTB.Text;
            _walletList[_selectedWallet].ConfigJson.Payouts.Seed = payoutSeedTB.Text;
            _walletList[_selectedWallet].ConfigJson.Payouts.Deadline = (uint)payoutDeadlineNM.Value;
            _walletList[_selectedWallet].ConfigJson.Payouts.Fee = (uint)payoutFeeNM.Value;
            _walletList[_selectedWallet].ConfigJson.Payouts.Raw = payoutRawCB.Checked;
            _walletList[_selectedWallet].ConfigJson.Payouts.Timeout = payoutTimeoutNM.Value.ToString(CultureInfo.InvariantCulture) + "s";
            _walletList[_selectedWallet].ConfigJson.Payouts.Threshold = (uint)payoutThresholdNM.Value;
            WriteConfig(_selectedWallet);

            SaveWalletList();

            DebugMsg("config.json updated\r\n");
        }

        private void DebugMsg(string line)
        {
            if (debugTB.InvokeRequired)
            {
                SetDebugCallback d = DebugMsg;
                Invoke(d, line);
            }
            else
            {
                if (debugTB.Text.Length > 10000)
                {
                    debugTB.Text = debugTB.Text.Remove(0, 500);
                }

                debugTB.Text += (DateTime.Now.ToLongTimeString() + ": " + line + "\r\n");
                //debugTB.Text += line;
                debugTB.SelectionStart = debugTB.Text.Length;
                debugTB.ScrollToCaret();
                debugTB.Update();
            }
        }

        private void DebugLogMsg(string line)
        {
            if (debugTB.InvokeRequired)
            {
                SetDebugCallback d = DebugMsg;
                Invoke(d, line);
            }
            else
            {
                if (debugTB.Text.Length > 10000)
                {
                    debugTB.Text = debugTB.Text.Remove(0, 500);
                }

                debugTB.Text += (DateTime.Now.ToLongTimeString() + ": " + line + "\r\n");
                //debugTB.Text += line;
                debugTB.SelectionStart = debugTB.Text.Length;
                debugTB.ScrollToCaret();
                debugTB.Update();

                LogMsg(line);

            }
        }

        private void DebugClear()
        {
            if (debugTB.InvokeRequired)
            {
                SetDebugClearCallback d = DebugClear;
                Invoke(d, new object[] {});
            }
            else
            {
                debugTB.Clear();
            }
        }

        private void LogMsg(string line)
        {
            if (logTB.InvokeRequired)
            {
                SetLogCallback d = LogMsg;
                Invoke(d, line);
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
            if (infoLBL.InvokeRequired)
            {
                SetInfTextCallback d = InfoUpdate;
                Invoke(d, line);
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
                string jsonData = JsonConvert.SerializeObject(_walletList[index].ConfigJson);
                File.WriteAllText(_appPath + @"\eon" + index + @"\config.json", jsonData);
            }
            catch(Exception ex)
            {
                DebugLogMsg("WriteConfig(" + index + ") exception : " + ex.Message);
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
                ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + command)
                {

                    // The following commands are needed to redirect the standard output.
                    // This means that it will be redirected to the Process.StandardOutput StreamReader.
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    // Do not create the black window.
                    CreateNoWindow = true
                };
                // Now we create a process, assign its ProcessStartInfo and start it
                Process proc = new Process
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
                Directory.SetCurrentDirectory(Path.GetDirectoryName(_appPath + @"\eon" + index + @"\"));
                res = ExecuteCommandSync(cmd);
                Directory.SetCurrentDirectory(_appPath);
                if (enableLog) LogMsg(res + "\r\n");
            }
            catch (Exception ex)
            {
                DebugLogMsg("ERROR during EON command execution (walletIndex" + index + ") :\r\nCMD: " + cmd + "\r\n\r\nException: " + ex.Message);
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
            string commitResponse = EonCMD(_selectedWallet, "eon allcommit " + accountTB.Text);
            if (commitResponse!="-1") UpdateCommited(commitResponse);           
        }

        //clear log view
        private void Button1_Click(object sender, EventArgs e)
        {
           logTB.Text = "";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _eonThreadRun = false;
            Thread.Sleep(1);
        }

        private void EonCommandLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //launch eon command line
            try
            {
                Directory.SetCurrentDirectory(_appPath + @"\eon" + _selectedWallet);
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
        private void ReadConfigObject(int index)
        {
            try
            {
                string filename = _appPath + @"\eon" + index + @"\config.json";
                string newJSON = File.ReadAllText(filename);
                configClass thisConf = new configClass();

                try
                {
                    thisConf = JsonConvert.DeserializeObject<configClass>(newJSON);
                }
                catch (Exception ex)
                {
                    DebugMsg("Error deserializing json file - bad format? : " + ex.Message);
                    return;
                }
                               
                //save this config object in the walletlist & create the config file
                _walletList[index].ConfigJson = thisConf;

                DebugMsg("config.json read OK\r\n");
            }
            catch (Exception ex)
            {
                DebugMsg("Error: Could not read file. [ " + ex.Message + "]");
            }
        }
       

        private bool ImportWalletsJson()
        {
            bool res = false;

            //import a json file, overwrites existing one
            OpenFileDialog importDg = new OpenFileDialog
            {
                InitialDirectory = "c:\\",
                Filter = "wallets.json files (*.json)|*.json",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (importDg.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filename = importDg.FileName;
                    

                    try
                    {
                        _walletList = ReadFromJsonFile<List<WalletClass>>(filename);
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
        private bool ImportJsonDlg(int index)
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
                    _walletList[index].ConfigJson = thisConf;
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
                _eonThreadRun = false;

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
            SaveFileDialog jsonDg = new SaveFileDialog
            {
                Filter = "wallets.json files (*.json)|*.json",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            try
            {
                if (jsonDg.ShowDialog() == DialogResult.OK)
                {

                    string filename = jsonDg.FileName;

                    try
                    {
                        WriteToJsonFile(filename, _walletList, false);
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
                Process.Start("explorer.exe", _appPath + @"\eon" + _selectedWallet);
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

            using (new DialogCenteringService(this))
            {
                res = MessageBox.Show("Transfer " + balDepAmountTB.Text + " to deposit?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            }

            if (res == DialogResult.Yes)
            {
                //code for Yes
                string refillResp = EonCMD(_selectedWallet, "eon refill " + balDepAmountTB.Text);

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


                            using (new DialogCenteringService(this))
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
                string wdResp = EonCMD(_selectedWallet, "eon withdraw " + balDepAmountTB.Text);

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

            using (new DialogCenteringService(this))
            {
                res = MessageBox.Show("Send " + txAmountTB.Text + " from " + _walletList[_selectedWallet].AccountID + "\r\n to account : " + recipientAddress + "?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            }

            if (res == DialogResult.Yes)
            {
                //code for Yes
                string txResp = EonCMD(_selectedWallet, "eon payment " + recipientAddress + " " + txAmountTB.Text);

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

                        //MessageBox.Show();
                        using (new DialogCenteringService(this))
                        {
                            MessageBox.Show("Send " + rAmount + " from " + _walletList[_selectedWallet].AccountID + " to " + rAccount + " : " + resultS);

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
            Process.Start("https://testnet.eontechnology.org/");
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutDialog aDialog = new AboutDialog();
            aDialog.ShowDialog();
           
        }

        private void QuitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _eonThreadRun = false;
            Application.Exit();
        }

        private void UncommitBTN_Click(object sender, EventArgs e)
        {
            string commitResponse = EonCMD(_selectedWallet, "eon uncommit " + accountTB.Text);
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
                writer?.Close();
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
                reader?.Close();
            }
        }

        private void AccountLV_SelectionChanged(object sender, EventArgs e)
        {
            if ((0 <= accountLV.SelectedIndex) && (accountLV.SelectedIndex <= (_walletList.Count - 1)))
            {
                if (accountLV.SelectedIndex != _lastWalletIndex)
                {
                    _selectedWallet = accountLV.SelectedIndex;
                    accountTB.Text = _walletList[_selectedWallet].AccountID;
                    pubkeyTB.Text = _walletList[_selectedWallet].PublicKey;
                    UpdateConfigDisplay(_selectedWallet);
                    SyncGuiBalances();
                    _lastWalletIndex = accountLV.SelectedIndex;
                }
            }
        }
        

        private void RebuildWorkingFoldersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //allow eon thread to shut down
            _eonThreadRun = false;
            Thread.Sleep(2);

            RebuildWorkingFolders(true);
        }

        private void RebuildWorkingFolders(bool restoreWallets)
        {
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
                DirectoryInfo di = new DirectoryInfo(_appPath + @"\");
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
                    _walletList.Clear();
                    _mainWallet = new WalletClass();
                    _walletList.Add(_mainWallet);
                    
                }
                
                //set the primary wallet as default selected
                _selectedWallet = 0;
                
                //rebuild working folders (restart the eon thread now)
                _eonThreadRun = true;

                //start the eon thread
                _eonThread = new Thread(EonThreadStart);
                _eonThread.SetApartmentState(ApartmentState.STA);
                _eonThread.Start();
            }
            catch (Exception ex)
            {
                DebugMsg("ImportWallets() failed to delete working folders : [ " + ex.Message + "]");
            }
        }


        private void ResetAllConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //allow eon thread to shut down
            _eonThreadRun = false;
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

                int walletIndex = _walletList.Count;

                //create a new SEED
                string seedResponse = EonCMD(0, "eon seed");
                DebugLogMsg(seedResponse);
                string pattern = @"Seed:\s*(.*)\s*Account:\s*(.*)\s*Public key:\s*(.*)";
                Match srMatch = Regex.Match(seedResponse, pattern);


                string seed = srMatch.Groups[1].ToString();
                string accountId = srMatch.Groups[2].ToString();
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
                        AccountID = accountId,
                        PublicKey = publicKey,
                        Seed = seed,
                        NickName = nick
                    };
                    _walletList.Add(wal);

                    SetupEonFolder(walletIndex);

                    //new account has been created.  update the walletlist item and the config file
                    ReadConfigObject(walletIndex);
                    _walletList[walletIndex].Seed = seed.Trim();
                    _walletList[walletIndex].AccountID = accountId.Trim();
                    _walletList[walletIndex].PublicKey = publicKey.Trim();
                    if (walletIndex == 0) _walletList[walletIndex].NickName = nick.Trim();
                    _walletList[walletIndex].ConfigJson.Payouts.Seed = seed.Trim();
                    WriteConfig(walletIndex);
                    SaveWalletList();

                }

            }


        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex==3)
            {
                logTB.Focus();
                logTB.SelectionStart = logTB.Text.Length;
                logTB.ScrollToCaret();
                logTB.Update();
            }

            //test tab selected
            if (tabControl1.SelectedIndex == 4)
            {

            }
        }

        private void SwapAccountA_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            //update the balance display according to the selected items
            swaptestActBalA_lbl.Text = _walletList[swapAccountA_CB.SelectedIndex].Balance;
        }

        private void SwapAccountB_CB_SelectedIndexChanged(object sender, EventArgs e)
        {
            //update the balance display according to the selected items
            swaptestActBalB_lbl.Text = _walletList[swapAccountB_CB.SelectedIndex].Balance;
        }

        //update the GUI fields on the swap-test tab
        private void UpdateSwapTestGUI()
        {
            if (swapAccountA_CB.InvokeRequired)
            {
                SetUpdateSwapTestGUICallback d = UpdateSwapTestGUI;
                Invoke(d, new object[] { });
            }
            else
            {
                swaptestStartBalA_lbl.Text = _swapData.StartBalanceA.ToString();
                swaptestStartBalB_lbl.Text = _swapData.StartBalanceB.ToString();
                swaptestSentA_lbl.Text = _swapData.SentA.ToString();
                swaptestSentB_lbl.Text = _swapData.SentB.ToString();
                swaptestFeesA_lbl.Text = _swapData.FeesA.ToString();
                swaptestFeesB_lbl.Text = _swapData.FeesB.ToString();
                swaptestIncomingA_lbl.Text = _swapData.IncomingA.ToString();
                swaptestIncomingB_lbl.Text = _swapData.IncomingB.ToString();
                swaptestExpBalA_lbl.Text = _swapData.ExpectedBalanceA.ToString();
                swaptestExpBalB_lbl.Text = _swapData.ExpectedBalanceB.ToString();
                swaptestActBalA_lbl.Text = _swapData.ActualBalanceA.ToString();
                swaptestActBalB_lbl.Text = _swapData.ActualBalanceB.ToString();
                swapStatsTxCountAB_lbl.Text = _swapData.TxCountAB.ToString();
                swapStatsTxCountBA_lbl.Text = _swapData.TxCountBA.ToString();
                swapStatsPendingA_lbl.Text = _swapData.PendingTX_A.ToString();
                swapStatsPendingB_lbl.Text = _swapData.PendingTX_B.ToString();
                swapStatsTotalTX_lbl.Text = _swapData.TotalTransactions.ToString();
                swapStatsTXFails_lbl.Text = _swapData.FailedTransactions.ToString();
                swapStatsTestDuration_lbl.Text = _swapData.TestDuration.Days.ToString() + " Days , " + _swapData.TestDuration.Hours.ToString() + ":" + _swapData.TestDuration.Minutes.ToString() + ":" + _swapData.TestDuration.Seconds.ToString();
                swapStatsTXRate_lbl.Text = _swapData.TransactionRate.ToString();
                swapTestTxSize.Text = _swapData.TransactionSize.ToString();



            }
        }

        //start the swapTest
        private void SwapTestStart_BTN_Click(object sender, EventArgs e)
        {
            //record the selected account ID's
            _swapData.AccountIndex_A = swapAccountA_CB.SelectedIndex;
            _swapData.AccountIndex_B = swapAccountB_CB.SelectedIndex;


            //start a new thread to run the swapTestprocess with
            //start the eon thread

            _swapTestThreadRun = true;

            _swapTestThread = new Thread(SwapTestThreadStart);
            _swapTestThread.SetApartmentState(ApartmentState.STA);
            _swapTestThread.Start();

            swapTestStart_BTN.Enabled = false;
            swapTestStop_BTN.Enabled = true;
            swapTestReset_BTN.Enabled = false;


        }

        public void SwapTestThreadStart()
        {
            DebugLogMsg("SwapTestThread started...\r\n");
            _swapData.IsRunning = true;

            //record the starting balances
            _swapData.StartBalanceA = Convert.ToDouble(_walletList[_swapData.AccountIndex_A].Balance);
            _swapData.StartBalanceB = Convert.ToDouble(_walletList[_swapData.AccountIndex_B].Balance);

            //setup the expected & actual balances
            _swapData.ExpectedBalanceA = _swapData.ActualBalanceA = _swapData.StartBalanceA;
            _swapData.ExpectedBalanceB = _swapData.ActualBalanceB = _swapData.StartBalanceB;

            //set the test start time
            _swapData.StartTimeStamp = DateTime.Now;

            bool failureDetect = false;

            DateTime lastGUIUpdateDT = DateTime.Now;

            while ((!failureDetect)&&(_swapTestThreadRun))
            {

                //send swap transactions
                bool sendResA = SendPayment(_swapData.AccountIndex_A, _swapData.AccountIndex_B);

                if (sendResA)
                {
                    //update stats
                    _swapData.SentA += _swapData.TransactionSize;
                    _swapData.FeesA += 0.000010;
                    _swapData.IncomingB += _swapData.TransactionSize;
                    _swapData.ExpectedBalanceA = _swapData.StartBalanceA - _swapData.SentA - _swapData.FeesA;
                    _swapData.TxCountAB += 1;
                    _swapData.TotalTransactions += 1;

                    bool sendRes2 = SendPayment(_swapData.AccountIndex_B, _swapData.AccountIndex_A);

                    if (sendRes2)
                    {
                        _swapData.SentB += _swapData.TransactionSize;
                        _swapData.FeesB += 0.000010;
                        _swapData.IncomingA += _swapData.TransactionSize;
                        _swapData.ExpectedBalanceB = _swapData.StartBalanceB - _swapData.SentB - _swapData.FeesB + _swapData.IncomingB;
                        _swapData.TxCountBA += 1;
                        _swapData.TotalTransactions += 1;

                        //update transaction rate
                        TimeSpan elapsed = DateTime.Now - _swapData.StartTimeStamp;
                        _swapData.TransactionRate = (decimal)(_swapData.TotalTransactions / elapsed.TotalSeconds);

                        //update accountA now that we have incomingA
                        _swapData.ExpectedBalanceA = _swapData.StartBalanceA - _swapData.SentA - _swapData.FeesA + _swapData.IncomingA;

                    }
                    else
                    {
                        //update accountA now that we have incomingA
                        _swapData.ExpectedBalanceA = _swapData.StartBalanceA - _swapData.SentA - _swapData.FeesA;
                        _swapData.FailedTransactions++;
                        

                    }

                }
                else
                {
                    _swapData.FailedTransactions++;
                    
                }


                //update the test duration
                _swapData.TestDuration = DateTime.Now - _swapData.StartTimeStamp;


                //update the GUI periodically
                if (DateTime.Now - lastGUIUpdateDT > TimeSpan.FromSeconds(1))
                {
                    UpdateSwapTestGUI();
                    lastGUIUpdateDT = DateTime.Now;
                }

                Thread.Sleep(100);
            }

            UpdateSwapTestGUI();
            _swapData.IsRunning = false;
            DebugLogMsg("SwapTestThread stopped\r\n");


        }


        private bool SendPayment(int senderIndex, int receiverIndex)
        {
            bool result = false;

            string txResp = EonCMD(senderIndex, "eon payment " + _walletList[receiverIndex].AccountID + " " + _swapData.TransactionSize.ToString());

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
                    resultS.Trim();

                    if (resultS.Contains("success")) result = true;

                }
                catch (Exception ex)
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

            return result;

        }


        private void SwapTestStop_BTN_Click(object sender, EventArgs e)
        {
            _swapTestThreadRun = false;
            swapTestStart_BTN.Enabled = false;
            swapTestStop_BTN.Enabled = false;
            swapTestReset_BTN.Enabled = true;
        }
        
        //update the swapData if the user changes the transaction size
        private void SwapTestTxSize_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!_swapData.IsRunning) _swapData.TransactionSize = Convert.ToDouble(swapTestTxSize.Text);
            }
           catch(Exception ex)
            {                
            }
        }


        private void SwapTestReset_BTN_Click(object sender, EventArgs e)
        {
            if (!_swapData.IsRunning) _swapData = new SwapTestDataClass();
            UpdateSwapTestGUI();
            swapTestStart_BTN.Enabled = true;
            swapTestStop_BTN.Enabled = false;
            swapTestReset_BTN.Enabled = false;
        }


    }


}
