using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wakaran
{
    public partial class MessageForm : Form
    {
        public MessageForm(string inTitle, string inMessage)
        {
            InitializeComponent();

            this.Text = inTitle;
            lblMessage.Text = inMessage;
        }

        private void MessageForm_Load(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
