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
    public partial class WorkspaceSelector : Form
    {
        public WorkspaceSelector()
        {
            InitializeComponent();
            
        }

        private void btnOpenWorkspace_Click(object sender, EventArgs e)
        {

            Program.mainForm = new MainForm();
            Program.mainForm.ShowDialog();
            
            this.Close();
        }

        private void WorkspaceSelector_Load(object sender, EventArgs e)
        {
            btnOpenWorkspace_Click(null, null);
        }
    }
}
