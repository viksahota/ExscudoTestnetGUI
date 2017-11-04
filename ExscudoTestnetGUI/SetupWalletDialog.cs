using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExscudoTestnetGUI
{
    public partial class SetupWalletDialog : Form
    {

        public string result = "";
        public string seedVal = "";

        public SetupWalletDialog()
        {
            InitializeComponent();
        }

        //create new button
        private void Button1_Click(object sender, EventArgs e)
        {
            this.result = "Create";
            this.Dispose();
        }

        // import button
        private void Button2_Click(object sender, EventArgs e)
        {
            this.result = "Import";
            this.Dispose();
        }

        //set SEED button
        private void Button4_Click(object sender, EventArgs e)
        {
            this.result = "Set";
            seedVal = seedTB.Text;
            this.Dispose();
        }


        //cancel button
        private void Button3_Click(object sender, EventArgs e)
        {
            this.result = "Cancel";
            this.Dispose();
        }
    }
}
