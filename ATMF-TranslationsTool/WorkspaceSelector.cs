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

        private void btnCreateWorkspace_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    var folder = new DirectoryInfo(fbd.SelectedPath);
                    if (!folder.Exists)
                    {
                        MessageBox.Show("Specified folder doesn't exists.");
                        return;
                    }

                    if (folder.GetDirectories().Length > 0 || folder.GetFiles().Length > 0)
                    {
                        MessageBox.Show("Specified folder must be an empty one.");
                        return;
                    }
                    
                    var langFolder = folder.FullName + "/en";
                    Directory.CreateDirectory(langFolder);
                    using (var writer = File.CreateText(langFolder + "/home.json"))
                    {
                        writer.Write("{\r\n\t\"title\": \"My new ATMF translations workspace.\"\r\n}");
                    }

                    Program.mainForm = new MainForm(fbd.SelectedPath);
                    Program.mainForm.Show();
                    this.Hide();
                }
            }
        }
    }
}
