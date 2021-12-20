using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATMF_TranslationsTool
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();

            var thisApp = Assembly.GetExecutingAssembly();
            AssemblyName name = new AssemblyName(thisApp.FullName);
            lblVersion.Text += name.Version.Major + "." + name.Version.Minor + "." + name.Version.Build;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://github.com/skito/ATMF-TranslationsTool-Windows",
                UseShellExecute = true
            });
        }
    }
}
