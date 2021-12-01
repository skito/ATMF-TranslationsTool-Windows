using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATMF_TranslationsTool
{
    public partial class WorkspaceSelector : Form
    {
        public WorkspaceSelector()
        {
            InitializeComponent();
            
        }

        private void btnOpenWorkspace_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    Program.mainForm = new MainForm(fbd.SelectedPath);
                    Program.mainForm.Show();
                    this.Hide();
                }
            }

            
        }

        private void WorkspaceSelector_Load(object sender, EventArgs e)
        {
            return;
            btnOpenWorkspace_Click(null, null);
        }
    }
}
