using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ATMF_TranslationsTool.Components;

namespace ATMF_TranslationsTool
{
    public partial class MainForm : Form
    {
        string workspace;
        BindingSource gridSource;
        TranslationsRepository repository;

        public MainForm()
        {

            //SetTranslationsData();
            InitializeComponent();

            workspace = @"D:\atmf-test\language";
            gridSource = new BindingSource();
            repository = new TranslationsRepository();
            repository.LoadWorkspace(workspace);
            gridSource.DataSource = repository.data;
            repository.data.ColumnChanged += Data_ColumnChanged;
            repository.WorkspaceSaved += Repository_WorkspaceSaved;

            RebuildGrid();
        }

        

        public void RebuildGrid()
        {
            // Namespaces
            ddlSections.Items.Clear();
            foreach (var item in repository.GetNamespaces())
            {
                ddlSections.Items.Add(item);
            }

            if (ddlSections.Items.Count > 0)
                ddlSections.SelectedIndex = 0;

            translationsGrid.AutoGenerateColumns = true;
            translationsGrid.AutoSize = true;
            translationsGrid.DataSource = gridSource;
            translationsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (translationsGrid.Columns.Count > 0)
                translationsGrid.Columns[0].Visible = false;
            // translations.SelectMany(x => x);// gridSource;
            

            /*DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "resourceKey";
            column.Name = "Resource Key";
            translationsGrid.Columns.Add(column);

            foreach (var language in languages)
            {
                DataGridViewColumn langColumn = new DataGridViewTextBoxColumn();
                langColumn.DataPropertyName = "lang_" + language;
                langColumn.Name = language;
                translationsGrid.Columns.Add(langColumn);
            }*/

            /*ddlSections.Size = new Size(translationsGrid.Columns[0].Width, 100);
            ddlSections.Location = new Point(80, 0);*/

            translationsGrid.RowTemplate.Height = 100;
            translationsGrid.RowTemplate.DefaultCellStyle.Padding = new Padding(5);
            
        }

        private void translationsGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0 && e.ColumnIndex > 0)
            {
                string content = translationsGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                var editForm = new EditForm(content);
                editForm.ShowDialog();
                translationsGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = editForm.content;
                translationsGrid_CellEndEdit(sender, e);
            }
        }

        private void ddlSections_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterRowsByNamespace();
        }

        private void FilterRowsByNamespace()
        {
            if (translationsGrid.DataSource == null) return;

            CurrencyManager cm = (CurrencyManager)BindingContext[translationsGrid.DataSource];
            cm.SuspendBinding();

            for (var i = 0; i < translationsGrid.Rows.Count; i++)
            {
                var row = translationsGrid.Rows[i];
                if (row.Cells[0].Value != null && row.Cells[0].Value.ToString().IndexOf('.') > 0)
                {
                    var keyParts = row.Cells[0].Value.ToString().Split('.');
                    row.Visible = (keyParts[0] == ddlSections.SelectedItem.ToString());
                }

            }
            cm.ResumeBinding();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            translationsGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            translationsGrid.EndEdit();
            translationsGrid.BindingContext[translationsGrid.DataSource].EndCurrentEdit();

            lblStatus.Text = "Saving...";
            repository.SaveWorkspace(workspace);
        }

        private void translationsGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var row = translationsGrid.Rows[e.RowIndex];

            var key = "";
            if (row.Cells[1].Value != null && row.Cells[1].Value.ToString() != "")
            {
                key = FormatJSONKey(row.Cells[1].Value.ToString());
            }

            var keysInNS = repository.GetKeysByNamespace(ddlSections.SelectedItem.ToString());
            if (key == "")
            {
                var index = 0;
                do
                {
                    key = index > 0 ? "newKey" + index : "newKey";
                    index++;

                } while (keysInNS.IndexOf(key) >= 0);
            }

            row.Cells[1].Value = key;
            translationsGrid.Rows[e.RowIndex].Cells[0].Value = ddlSections.SelectedItem.ToString() + "." + key;
        }

        private string FormatJSONKey(string key)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9_]");
            return rgx.Replace(key, "");
        }

        private void translationsGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (ddlSections.Items.Count > 0)
            {
                FilterRowsByNamespace();
            }
        }

        private void Repository_WorkspaceSaved(object sender, EventArgs e)
        {
            lblStatus.Text = "Ready";
        }

        private void Data_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            if (e.Column.ColumnName == "_Key")
            {
                var keysInNS = repository.GetKeysByNamespace(ddlSections.SelectedItem.ToString());
                var distinct = keysInNS.Distinct().ToList();
                if (keysInNS.Count != distinct.Count)
                    MessageBox.Show("Typed key already exists! You will loose data on save!");
            }
        }
    }
}
