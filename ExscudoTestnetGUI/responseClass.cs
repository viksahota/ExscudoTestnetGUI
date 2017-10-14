using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExscudoTestnetGUI
{

    internal class ResponseClass
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public ulong Amount { get; set; }
        public ulong Deposit { get; set; }
    }

}
