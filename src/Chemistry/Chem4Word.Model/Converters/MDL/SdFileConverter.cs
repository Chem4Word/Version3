﻿using Chem4Word.Core.Helpers;
using Chem4Word.Model.Converters.MDL;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Chem4Word.Model.Converters
{
    public class SdFileConverter : IConverter
    {
        private List<PropertyType> _propertyTypes = null;

        public bool CanExport => true;

        public bool CanImport => true;

        public string Description => "Import and Export of MDL MolFile and SdFile formats";

        public string[] Extensions => new string[]
        {
            "*.sdf",
            "*.mol"
        };

        public SdFileConverter()
        {
            var resource = ResourceHelper.GetStringResource(Assembly.GetExecutingAssembly(), "PropertyTypes.json");
            _propertyTypes = JsonConvert.DeserializeObject<List<PropertyType>>(resource);
        }

        public Model Import(object data)
        {
            // Convert incoming string to a stream
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write((string)data);
            writer.Flush();
            stream.Position = 0;

            StreamReader sr = new StreamReader(stream);

            Model model = new Model();
            Molecule molecule = null;

            SdfState state = SdfState.Null;

            string message = null;

            while (!sr.EndOfStream)
            {
                switch (state)
                {
                    case SdfState.Null:
                    case SdfState.EndOfData:
                        molecule = new Molecule();
                        //NOTE:  do NOT add an empty molecule to the model.  Add it AFTER it's been populated
                        //model.Molecules.Add(molecule);
                        CtabProcessor pct = new CtabProcessor();
                        state = pct.ImportFromStream(sr, molecule, out message);
                        //THIS is where you should add the molecule!
                        model.Molecules.Add(molecule);
                        break;

                    case SdfState.EndOfCtab:
                        DataProcessor dp = new DataProcessor(_propertyTypes);
                        state = dp.ImportFromStream(sr, molecule, out message);
                        break;

                    case SdfState.Error:
                        // Swallow rest of stream
                        string dumpOnError = sr.ReadToEnd();
                        break;

                    case SdfState.Unsupported:
                        // Swallow rest of stream
                        string dumponUnsupported = sr.ReadToEnd();
                        break;
                }
            }

            //if (model.MeanBondLength < 5 || model.MeanBondLength > 100)
            //{
            //    model.ScaleToAverageBondLength(20);
            //}

            // Can't use RebuildMolecules() as it trashes the formulae and labels
            //model.RebuildMolecules();
            model.RefreshMolecules();
            return model;
        }

        public string Export(Model model)
        {
            string result;

            // MDL Standard bond length is 1.54 Angstoms (Å)
            //model.ScaleToAverageBondLength(1.54);

            MemoryStream stream = new MemoryStream();
            using (StreamWriter writer = new StreamWriter(stream))
            {
                foreach (var mol in model.Molecules)
                {
                    string message;
                    CtabProcessor pct = new CtabProcessor();
                    pct.ExportToStream(mol, writer, out message);

                    DataProcessor dp = new DataProcessor(_propertyTypes);
                    dp.ExportToStream(mol, writer, out message);
                }
                writer.Flush();
            }

            result = Encoding.ASCII.GetString(stream.ToArray());
            return result;
        }
    }
}