using System;
using System.Windows.Forms;

namespace ExscudoTestnetGUI
{
    public partial class AccountSeedDialog : Form
    {

        public bool result;
        public string seedVal = "";
        public string nickName = "";

        public AccountSeedDialog()
        {
            InitializeComponent();
        }

        //ok button
        private void button1_Click(object sender, EventArgs e)
        {
            seedVal = seedTB.Text;
            nickName = nameTB.Text;
            result = true;
            Dispose();
        }

        public void setAccountLabel(string text)
        {
            accountLabel.Text = text;
        }

        //cancel button
        private void button2_Click(object sender, EventArgs e)
        {
            result = false;
            Dispose();
        }
    }
}
