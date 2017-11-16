using System;
using System.Windows.Forms;

namespace ExscudoTestnetGUI
{
    public partial class CreateNewAccountDialog : Form
    {

        public string nickname = "";
        public string result = "";

        public CreateNewAccountDialog()
        {
            InitializeComponent();
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            nickname = nicknameTB.Text;
            result = "Create";
            Dispose();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            result = "Cancel";
            Dispose();
        }
    }
}
