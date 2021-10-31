using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATMF_TranslationsTool
{
    public partial class MainForm : Form
    {
        DataTable translations;
        List<string> languages;
        BindingSource gridSource;

        public MainForm()
        {
            translations = new DataTable();
            gridSource = new BindingSource();
            languages = new List<string>();
            languages.Add("en");
            languages.Add("bg");

            SetTranslationsData();
            InitializeComponent();
            RebuildGrid();
        }


        public void RebuildGrid()
        {

            gridSource.DataSource = translations; 
            
            translationsGrid.AutoGenerateColumns = true;
            translationsGrid.AutoSize = true;
            translationsGrid.CellEndEdit += TranslationsGrid_CellEndEdit;
            translationsGrid.DataSource = gridSource;
            translationsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
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

        private void TranslationsGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            return;
        }

        public void SetTranslationsData()
        {

            translations.Columns.Add("Key");
            foreach(var lang in languages)
            {
                translations.Columns.Add(lang);
            }

            for (var i = 0; i < 100; i++)
            {
                var row = translations.NewRow();
                row["Key"] = "key" + i;
                row["bg"] = "bulgarian value";
                row["en"] = "english value";
                translations.Rows.Add(row);
            }

            
            
        }


    }
}
