using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATMF_TranslationsTool
{
    public partial class EditForm : Form
    {
        public string content;

        public EditForm(string content="")
        {
            InitializeComponent();
            this.content = content;
            txtContent.Text = content;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            content = txtContent.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void txtContent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                e.Handled = true;
                SendKeys.Send("    ");
                e.SuppressKeyPress = true;
            }
        }
    }
}
