// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Chem4Word.Model.Converters.MDL
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

        public static int LineNumber;

        public static string GetNextLine(StreamReader sr)
        {
            LineNumber++;
            return sr.ReadLine();
        }

        public Model Import(object data)
        {
            Model model = null;

            if (data != null)
            {
                string dataAsString = (string)data;
                if (!dataAsString.Contains("v3000") && !dataAsString.Contains("V3000"))
                {
                    model = new Model();
                    LineNumber = 0;
                    // Convert incoming string to a stream
                    MemoryStream stream = new MemoryStream();
                    StreamWriter writer = new StreamWriter(stream);
                    writer.Write(dataAsString);
                    writer.Flush();
                    stream.Position = 0;

                    StreamReader sr = new StreamReader(stream);

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
                                CtabProcessor pct = new CtabProcessor();
                                state = pct.ImportFromStream(sr, molecule, out message);
                                if (state == SdfState.Error)
                                {
                                    model.GeneralErrors.Add(message);
                                }
                                //Ensure we add the molecule after it's populated
                                model.Molecules.Add(molecule);
                                if (model.Molecules.Count >= 16)
                                {
                                    model.GeneralErrors.Add("This file has greater than 16 structures!");
                                    sr.ReadToEnd();
                                }
                                break;

                            case SdfState.EndOfCtab:
                                DataProcessor dp = new DataProcessor(_propertyTypes);
                                state = dp.ImportFromStream(sr, molecule, out message);
                                break;

                            case SdfState.Error:
                                // Swallow rest of stream
                                sr.ReadToEnd();
                                break;

                            case SdfState.Unsupported:
                                // Swallow rest of stream
                                sr.ReadToEnd();
                                break;
                        }
                    }

                    // Can't use RebuildMolecules() as it trashes the formulae and labels
                    //model.RebuildMolecules();
                    model.RefreshMolecules();
                }
            }

            return model;
        }

        public string Export(Model model)
        {
            string result;

            // MDL Standard bond length is 1.54 Angstoms (Å)
            // Already done in Ribbon Export Button code
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