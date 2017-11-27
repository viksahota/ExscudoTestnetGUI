using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExscudoTestnetGUI
{
    internal class SwapTestDataClass
    {
        public int AccountIndex_A;
        public int AccountIndex_B;
        public double StartBalanceA;
        public double StartBalanceB;
        public double SentA;
        public double SentB;
        public double FeesA;
        public double FeesB;
        public double IncomingA;
        public double IncomingB;
        public double ExpectedBalanceA;
        public double ExpectedBalanceB;
        public double ActualBalanceA;
        public double ActualBalanceB;
        public int TxCountAB;
        public int TxCountBA;
        public int PendingTX_A;
        public int PendingTX_B;
        public int TotalTransactions;
        public int FailedTransactions;
        public TimeSpan TestDuration;
        public decimal TransactionRate;
        public double TransactionSize;
        public bool IsRunning;
        public DateTime StartTimeStamp;

        public SwapTestDataClass()
        {
            AccountIndex_A = -1;
            AccountIndex_B = -1;
            StartBalanceA = 0.0;
            StartBalanceB = 0.0;
            SentA = 0;
            SentB = 0;
            FeesA = 0;
            FeesB = 0;
            IncomingA = 0;
            IncomingB = 0;
            ExpectedBalanceA = 0;
            ExpectedBalanceB = 0;
            ActualBalanceA = 0;
            ActualBalanceB = 0;
            TxCountAB = 0;
            TxCountBA = 0;
            PendingTX_A = 0;
            PendingTX_B = 0;
            TotalTransactions = 0;
            FailedTransactions = 0;
            TransactionRate = 0;
            TestDuration = TimeSpan.FromSeconds(0);
            IsRunning = false;
            TransactionSize = 0.01;
            StartTimeStamp = DateTime.Now;

        }
        
    }
}
