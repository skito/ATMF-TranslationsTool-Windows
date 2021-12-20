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
        bool hasChanges;
        bool quitOnSave;

        public MainForm(string workspace)
        {

            //SetTranslationsData();
            InitializeComponent();

            this.workspace = workspace;
            hasChanges = false;
            quitOnSave = false;

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
            if (translationsGrid.Columns.Count > 1)
                translationsGrid.Columns[1].Visible = false;

            translationsGrid.RowTemplate.Height = 100;
            translationsGrid.RowTemplate.DefaultCellStyle.Padding = new Padding(5);
            RebuildColumnsMenuItems();
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
                if (row.Cells[1].Value != null && row.Cells[1].Value.ToString().IndexOf('.') > 0)
                {
                    var keyParts = row.Cells[1].Value.ToString().Split('.');
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
            if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() != "")
            {
                key = FormatJSONKey(row.Cells[0].Value.ToString());
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

            row.Cells[0].Value = key;
            translationsGrid.Rows[e.RowIndex].Cells[1].Value = ddlSections.SelectedItem.ToString() + "." + key;
        }

        private string FormatJSONKey(string key)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9_<> ]");
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
            hasChanges = false;
            if (quitOnSave) Close();
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

            hasChanges = true;
        }

        private void getHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utilities.OpenWebURL("https://github.com/skito/ATMF-TranslationsTool-Windows/issues");
        }

        private void manageNamespacesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var nsCount = repository.GetNamespaces().Count;
            var nsManager = new NamespaceManager(repository);
            nsManager.ShowDialog();
            RebuildGrid();

            if (repository.GetNamespaces().Count() != nsCount)
                hasChanges = true;
        }

        private void manageTranslationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lngCount = repository.GetLanguages().Count();
            var lngManager = new LanguageManager(repository);
            lngManager.ShowDialog();
            RebuildGrid();

            if (repository.GetLanguages().Count() != lngCount)
                hasChanges = true;
        }

        private void RebuildColumnsMenuItems()
        {
            var hiddenColumns = new List<string>();
            foreach(ToolStripMenuItem item in columnsToolStripMenuItem.DropDownItems)
            {
                if (!item.Checked)
                    hiddenColumns.Add(item.Text);
            }

            columnsToolStripMenuItem.DropDownItems.Clear();
            foreach (var lang in repository.GetLanguages())
            {
                var menuItem = new ToolStripMenuItem();
                //menuItem.Checked = true;
                menuItem.CheckOnClick = true;
                menuItem.CheckState = CheckState.Checked;
                menuItem.ImageScaling = ToolStripItemImageScaling.None;
                menuItem.Name = "languageMenuItem_" + lang;
                //menuItem.Size = new System.Drawing.Size(180, 22);
                menuItem.Text = lang;
                menuItem.CheckedChanged += LanguageMenuItem_CheckedChanged;

                var isVisible = true;
                foreach(var column in hiddenColumns)
                {
                    if (column == lang)
                    {
                        isVisible = false;
                        break;
                    }
                }
                menuItem.Checked = isVisible;
                columnsToolStripMenuItem.DropDownItems.Add(menuItem);
            }
            
        }

        private void LanguageMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            var menuItem = (ToolStripMenuItem)sender;
            var lang = menuItem.Text;
            var isVisible = menuItem.Checked;

            foreach(DataGridViewColumn column in translationsGrid.Columns)
            {
                if (column.HeaderText == lang)
                {
                    column.Visible = isVisible;
                    break;
                }
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendKeys.Send("^X");
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendKeys.Send("^C");
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendKeys.Send("^V");
        }

        private void closeWorkspaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (hasChanges)
            {
                var result = MessageBox.Show("Do you want to save your changes before closing?", "You have unsaved changes", MessageBoxButtons.YesNoCancel);
                if (result != DialogResult.No)
                {
                    e.Cancel = true;
                    if (result == DialogResult.Yes)
                    {
                        quitOnSave = true;
                        saveToolStripMenuItem_Click(null, null);
                    }
                }
            }
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var about = new About();
            about.ShowDialog();
        }
    }
}
