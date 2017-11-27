using Newtonsoft.Json;

namespace ExscudoTestnetGUI
{
    internal class WalletClass
    {
        public WalletClass()
        {
            ConfigJson = new configClass
            {
                Payouts = new configClass.Payouts2()
            };

            NickName = "";
            Seed = "";
            AccountID = "";
            PublicKey = "";
            Balance = "0";
            Deposit = "0";
            ConfigJson.Coin = "eon";
            ConfigJson.Name = "main";
            ConfigJson.Threads = 2;
            ConfigJson.UpstreamCheckInterval = "5s";
            ConfigJson.Payouts.Deadline = 60;
            ConfigJson.Payouts.Enabled = true;
            ConfigJson.Payouts.Fee = 10;
            ConfigJson.Payouts.Raw = true;
            ConfigJson.Payouts.Interval = "120m";
            ConfigJson.Payouts.Peer = "peer.testnet.eontechnology.org:9443";
            ConfigJson.Payouts.Seed = "";
            ConfigJson.Payouts.Threshold = 500000000;
            ConfigJson.Payouts.Timeout = "10s";
        }

        public string NickName { get; set; }
        public string Seed { get; set; }
        public string AccountID { get; set; }
        public string PublicKey { get; set; }
        public configClass ConfigJson { get; set; }
        [JsonIgnore] public string Balance { get; set; }
        [JsonIgnore] public string Deposit { get; set; }

    }
}
