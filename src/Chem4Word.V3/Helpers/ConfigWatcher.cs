using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chem4Word.View;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Chem4Word.Helpers
{
    public class ConfigWatcher
    {
        private const string _filter = "*.json";
        private const string _attributeShowHydrogens = "ShowHydrogens";
        private const string _attributeColouredAtoms = "ColouredAtoms";

        private FileSystemWatcher _watcher;
        private string _watchedPath;
        private bool _handleEvents = true;

        public ConfigWatcher(string watchedPath)
        {
            _watchedPath = watchedPath;

            _watcher = new FileSystemWatcher();
            _watcher.Path = _watchedPath;
            _watcher.NotifyFilter = NotifyFilters.LastWrite;
            _watcher.Filter = _filter;
            _watcher.Changed += OnChanged;
            _watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            if (_handleEvents)
            {
                _handleEvents = false;
                JToken tokenShowHydrogens = null;
                JToken tokenColouredAtoms = null;

                string thisFile = e.FullPath;
                Thread.Sleep(250);
                using (StreamReader sr = File.OpenText(e.FullPath))
                {
                    using (JsonTextReader reader = new JsonTextReader(sr))
                    {
                        JObject jObject = (JObject)JToken.ReadFrom(reader);
                        tokenShowHydrogens = jObject[_attributeShowHydrogens];
                        tokenColouredAtoms = jObject[_attributeColouredAtoms];
                    }
                }

                if (tokenShowHydrogens != null && tokenColouredAtoms != null)
                {
                    bool showHydrogens = tokenShowHydrogens.Value<bool>();
                    bool colouredAtoms = tokenColouredAtoms.Value<bool>();

                    string[] files = Directory.GetFiles(_watchedPath, _filter);
                    foreach (var file in files)
                    {
                        if (!file.Equals(thisFile))
                        {
                            JToken tokenShowHydrogensOther = null;
                            JToken tokenColouredAtomsOther = null;
                            JObject jObject = null;

                            using (StreamReader sr = File.OpenText(file))
                            {
                                using (JsonTextReader reader = new JsonTextReader(sr))
                                {
                                    jObject = (JObject)JToken.ReadFrom(reader);
                                    tokenShowHydrogensOther = jObject[_attributeShowHydrogens];
                                    tokenColouredAtomsOther = jObject[_attributeColouredAtoms];
                                }
                            }

                            if (tokenShowHydrogensOther != null && tokenColouredAtomsOther != null)
                            {
                                bool showHydrogensOther = tokenShowHydrogensOther.Value<bool>();
                                bool colouredAtomsOther = tokenColouredAtomsOther.Value<bool>();

                                bool write = false;
                                if (colouredAtomsOther != colouredAtoms)
                                {
                                    jObject[_attributeColouredAtoms] = colouredAtoms;
                                    write = true;
                                }
                                if (showHydrogensOther != showHydrogens)
                                {
                                    jObject[_attributeShowHydrogens] = showHydrogens;
                                    write = true;
                                }
                                if (write)
                                {
                                    string json = JsonConvert.SerializeObject(jObject, Formatting.Indented);
                                    File.WriteAllText(file, json);
                                }
                            }
                        }
                    }
                }

                _handleEvents = true;
            }
        }
    }
}
