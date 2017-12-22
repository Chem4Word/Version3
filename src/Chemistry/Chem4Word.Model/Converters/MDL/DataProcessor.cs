// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Model.Converters.MDL;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Chem4Word.Model.Converters
{
    internal class DataProcessor : SdFileBase
    {
        private List<PropertyType> _propertyTypes;
        private Molecule _molecule;

        public DataProcessor(List<PropertyType> propertyTypes)
        {
            _propertyTypes = propertyTypes;
        }

        public override SdfState ImportFromStream(StreamReader reader, Molecule molecule, out string message)
        {
            message = null;
            _molecule = molecule;

            SdfState result = SdfState.Null;

            try
            {
                bool isFormula = false;
                string internalName = "";

                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();

                    if (!string.IsNullOrEmpty(line))
                    {
                        if (line.Equals(MDLConstants.SDF_END))
                        {
                            // End of SDF Section
                            result = SdfState.EndOfData;
                            break;
                        }

                        if (line.StartsWith(">"))
                        {
                            // Clear existing Property Name
                            internalName = string.Empty;

                            // See if we can find the property in our translation table
                            foreach (var property in _propertyTypes)
                            {
                                if (line.Equals(property.ExternalName))
                                {
                                    isFormula = property.IsFormula;
                                    internalName = property.InternalName;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            // Property Data
                            if (!string.IsNullOrEmpty(internalName))
                            {
                                if (isFormula)
                                {
                                    var formula = new Formula();
                                    formula.Convention = internalName;
                                    formula.Inline = line;
                                    _molecule.Formulas.Add(formula);
                                }
                                else
                                {
                                    var name = new ChemicalName();
                                    name.DictRef = internalName;
                                    name.Name = line;
                                    _molecule.ChemicalNames.Add(name);
                                }
                            }
                        }
                    }
                    else
                    {
                        internalName = string.Empty;
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
                result = SdfState.Error;
            }

            return result;
        }

        public override void ExportToStream(Molecule molecule, StreamWriter writer, out string message)
        {
            message = null;
            _molecule = molecule;

            Dictionary<string, List<string>> properties = new Dictionary<string, List<string>>();

            foreach (var name in _molecule.ChemicalNames)
            {
                if (properties.ContainsKey(name.DictRef))
                {
                    properties[name.DictRef].Add(name.Name);
                }
                else
                {
                    List<string> names = new List<string>();
                    names.Add(name.Name);
                    properties.Add(name.DictRef, names);
                }
            }

            foreach (var formula in _molecule.Formulas)
            {
                if (properties.ContainsKey(formula.Convention))
                {
                    properties[formula.Convention].Add(formula.Inline);
                }
                else
                {
                    List<string> names = new List<string>();
                    names.Add(formula.Inline);
                    properties.Add(formula.Convention, names);
                }
            }

            foreach (var property in properties)
            {
                string externalName = null;
                foreach (var propertyType in _propertyTypes)
                {
                    if (propertyType.InternalName.Equals(property.Key))
                    {
                        externalName = propertyType.ExternalName;
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(externalName))
                {
                    writer.WriteLine(externalName);
                    foreach (var line in property.Value)
                    {
                        writer.WriteLine(line);
                    }
                    writer.WriteLine("");
                }
            }

            writer.WriteLine(MDLConstants.SDF_END);
        }
    }
}