using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATMF_TranslationsTool.Components;

namespace ATMF_TranslationsTool
{
    public partial class NamespaceManager : Form
    {
        private TranslationsRepository repository;

        public NamespaceManager(TranslationsRepository repository=null)
        {
            InitializeComponent();
            this.repository = repository;
            ReloadNamespaces();
        }

        private void ReloadNamespaces()
        {
            ddlNamespaces.Items.Clear();
            if (repository != null)
            {
                var namespaces = repository.GetNamespaces();
                foreach (var item in namespaces)
                {
                    ddlNamespaces.Items.Add(item);
                }
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (repository != null)
            {
                repository.AddNamespace(txtName.Text);
                ReloadNamespaces();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (repository != null)
            {
                repository.RemoveNamespace(ddlNamespaces.SelectedItem.ToString());
                ReloadNamespaces();
            }
        }
    }
}
