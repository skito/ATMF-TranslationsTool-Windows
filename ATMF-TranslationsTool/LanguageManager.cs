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
    public partial class LanguageManager : Form
    {
        private TranslationsRepository repository;

        public LanguageManager(TranslationsRepository repository = null)
        {
            InitializeComponent();
            this.repository = repository;
            ReloadLanguages();
        }

        private void ReloadLanguages()
        {
            ddlLanguages.Items.Clear();
            if (repository != null)
            {
                var languages = repository.GetLanguages();
                foreach (var item in languages)
                {
                    ddlLanguages.Items.Add(item);
                }
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (repository != null)
            {
                repository.AddLanguage(txtName.Text);
                ReloadLanguages();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (repository != null)
            {
                repository.RemoveLanguage(ddlLanguages.SelectedItem.ToString());
                ReloadLanguages();
            }
        }
    }
}
