using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMF_TranslationsTool.Components
{
    class TranslationsRepository
    {
        public DataTable data = new DataTable();
        private BackgroundWorker wsSave = new BackgroundWorker();
        public event EventHandler WorkspaceSaved;

        public TranslationsRepository(DataTable data = null)
        {
            if (data != null)
                this.data = data;

            wsSave.DoWork += WsSave_DoWork;
            wsSave.RunWorkerCompleted += WsSave_RunWorkerCompleted;
        }

        private void WsSave_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (WorkspaceSaved != null) WorkspaceSaved(this, EventArgs.Empty);
        }

        private void WsSave_DoWork(object sender, DoWorkEventArgs e)
        {
            var path = e.Argument.ToString();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var languages = new List<string>();
            foreach (var column in data.Columns)
            {
                var columnName = column.ToString();
                if (columnName != "_Key" && columnName != "Key")
                {
                    languages.Add(columnName);
                }
            }

            var structure = new Dictionary<string, Dictionary<string, object>>();
            for (var i = 0; i < data.Rows.Count; i++)
            {
                //if (i < 1) continue;

                var row = data.Rows[i];

                if (row["_Key"] == null || row["_Key"].ToString() == "")
                    continue;

                var rowKey = row["_Key"].ToString().Split('.');
                if (rowKey.Length < 2) continue;

                var key = rowKey[1];
                if (key == "") continue;

                foreach (var lang in languages)
                {
                    var keyPath = lang + "/" + rowKey[0];
                    if (!structure.ContainsKey(keyPath))
                        structure[keyPath] = new Dictionary<string, object>();

                    if (key.IndexOf(" <plural>") > 0 || key.IndexOf(" <single>") > 0)
                    {
                        var realKey = key.Replace(" <plural>", "").Replace(" <single>", "");
                        if (!structure[keyPath].ContainsKey(realKey))
                            structure[keyPath][realKey] = new string[2];

                        var index = key.IndexOf(" <single>") > 0 ? 1 : 0;
                        ((string[])structure[keyPath][realKey])[index] = row[lang].ToString();
                    }
                    else structure[keyPath][key] = row[lang];

                }
            }

            var baseFolder = new DirectoryInfo(path);
            foreach (var ns in structure)
            {
                var fileInfo = new FileInfo(baseFolder.FullName + "/" + ns.Key + ".json");
                if (!fileInfo.Directory.Exists)
                    fileInfo.Directory.Create();

                string json = JsonConvert.SerializeObject(ns.Value, Formatting.Indented);
                File.WriteAllText(fileInfo.FullName, json);
            }
        }

        public List<string> GetNamespaces()
        {
            var items = new List<string>();
            foreach (DataRow row in data.Rows)
            {
                var item = row["_Key"].ToString().Split(".")[0];
                items.Add(item);
            }

            return items.Distinct().ToList();
        }

        public List<string> GetKeysByNamespace(string ns)
        {
            var items = new List<string>();
            foreach (DataRow row in data.Rows)
            {
                var keyParts = row["_Key"].ToString().Split(".");
                if (keyParts[0] == ns)
                    items.Add(keyParts[1]);
            }

            return items;
        }

        public void SaveWorkspace(string path)
        {
            wsSave.RunWorkerAsync(path);
        }

        public bool LoadWorkspace(string path)
        {
            if (!Directory.Exists(path))
            {
                // Message box here
                return false;
            }

            data = new DataTable();
            data.Columns.Add("_Key");
            data.Columns.Add("Key");
            string[] folders = Directory.GetDirectories(path);
            foreach(var folder in folders)
            {
                var folderInfo = new DirectoryInfo(folder);
                var lang = folderInfo.Name;

                data.Columns.Add(lang);
            }

            var dictTable = new Dictionary<string, Dictionary<string, string>> ();
            foreach (var folder in folders)
            {
                var folderInfo = new DirectoryInfo(folder);
                var lang = folderInfo.Name;

                var ns = LoadNamespaces(folder, "");
                foreach(var record in ns)
                {
                    if (!dictTable.ContainsKey(record.Key))
                        dictTable[record.Key] = new Dictionary<string, string>();

                    dictTable[record.Key][lang] = record.Value;
                }
            }

            foreach(var dictRow in dictTable)
            {
                var row = data.NewRow();
                row["_Key"] = dictRow.Key;
                row["Key"] = dictRow.Key.Split(".")[1];
                foreach (var langVal in dictRow.Value)
                {
                    row[langVal.Key] = langVal.Value;
                }
                data.Rows.Add(row);
            }

            return true;
        }

        private IDictionary<string, string> LoadNamespaces(string fullPath, string baseNS)
        {
            var ns = new Dictionary<string, string>();
            string[] files = Directory.GetFiles(fullPath, "*.json");
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                var nspath = baseNS + fileInfo.Name.Split(".json")[0];
                var trans = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(File.ReadAllText(file));
                foreach(var record in trans)
                {
                    var key = nspath + "." + record.Key;
                    if (record.Value is JArray)
                    {
                        var jarr = (JArray)record.Value;
                        ns[key + " <plural>"] = jarr.Count > 0 ? (string)jarr[0] : "";
                        ns[key + " <single>"] = jarr.Count > 0 ? (string)jarr[1] : "";
                    }
                    else if (record.Value is string)
                    {
                        ns[key] = (string)record.Value;
                    }
                }
            }

            string[] folders = Directory.GetDirectories(fullPath);
            foreach (var folder in folders)
            {
                var folderInfo = new DirectoryInfo(folder);
                var addonNS = LoadNamespaces(folder, baseNS + folderInfo.Name + "/");
                foreach (var record in addonNS) ns.Add(record.Key, record.Value);
            }

            return ns;
        }
    }

    class LanguageRepository
    {
        public string language = "";
        public IDictionary<string, LanguageResource> dict = new Dictionary<string, LanguageResource>();

        public LanguageRepository(string language="", IDictionary<string, LanguageResource> dict = null)
        {
            this.language = language;
            if (dict != null) this.dict = dict;
        }
    }

    class LanguageResource
    {
        public string plural = "";
        public string single = null;

        public LanguageResource(string plural = "", string single = "")
        {
            this.plural = plural;
            this.single = single;
        }
    }
}
