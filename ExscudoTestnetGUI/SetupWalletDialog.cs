using System;
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
            result = "Create";
            Dispose();
        }

        // import button
        private void Button2_Click(object sender, EventArgs e)
        {
            result = "Import";
            Dispose();
        }

        //set SEED button
        private void Button4_Click(object sender, EventArgs e)
        {
            result = "Set";
            seedVal = seedTB.Text;
            Dispose();
        }


        //cancel button
        private void Button3_Click(object sender, EventArgs e)
        {
            result = "Cancel";
            Dispose();
        }
    }
}
