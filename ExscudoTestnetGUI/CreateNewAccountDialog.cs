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
            this.Dispose();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.result = "Cancel";
            this.Dispose();
        }
    }
}
