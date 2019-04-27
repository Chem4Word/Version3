// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Model.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Chem4Word.Model.Converters.Json
{
    /// <summary>
    /// Converts a Model from and to JSON
    /// </summary>
    public class JSONConverter : IConverter
    {
        private const string Protruding = "protruding";
        private const string Recessed = "recessed";
        private const string Ambiguous = "ambiguous";

        private class AtomJSON
        {
            public string i;    // Id
            public double x;
            public double y;
            public string l;    // element
            public int? c;      // charge
        }

        private class BondJSON
        {
            public string i;    // Id
            public int? b;      //start atom
            public int? e;      //end atom
            public double? o;   //order
            public string s;    //stereo
        }

        private class MolJSON
        {
            public AtomJSON[] a;
            public BondJSON[] b;
        }

        private class ModelJSON
        {
            public MolJSON[] m;
        }

        public string Description => "JSON Molecular Format";
        public string[] Extensions => new string[] { "*.json" };

        public string Export(Model model)
        {
            ModelJSON mdJson = new ModelJSON();
            model.Relabel();
            if (model.Molecules.Count == 1)
            {
                Molecule m1 = model.Molecules[0];
                var mj = ExportMol(m1);
                return JsonConvert.SerializeObject(mj, Formatting.Indented, new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore });
            }
            else if (model.Molecules.Count > 1)
            {
                mdJson.m = new MolJSON[model.Molecules.Count];
                int i = 0;
                foreach (Molecule mol in model.Molecules)
                {
                    var mj = ExportMol(mol);
                    mdJson.m[i] = mj;
                    i++;
                }
            }
            return JsonConvert.SerializeObject(mdJson, Formatting.Indented, new JsonSerializerSettings() { DefaultValueHandling = DefaultValueHandling.Ignore });
        }

        private static MolJSON ExportMol(Molecule m1)
        {
            MolJSON mj = new MolJSON();

            mj.a = new AtomJSON[m1.Atoms.Count];
            Dictionary<Atom, int> indexLookup = new Dictionary<Atom, int>();

            int iAtom = 0;
            foreach (Atom a in m1.Atoms)
            {
                string elem = null;
                if (a.Element.Symbol != "C")
                {
                    elem = a.Element.Symbol;
                }
                mj.a[iAtom] = new AtomJSON()
                {
                    i = a.Id,
                    x = a.Position.X,
                    y = a.Position.Y,
                    l = elem
                };
                if (a.FormalCharge != null)
                {
                    mj.a[iAtom].c = a.FormalCharge.Value;
                }
                indexLookup[a] = iAtom;
                iAtom++;
            }

            int iBond = 0;
            if (m1.Bonds.Any())
            {
                mj.b = new BondJSON[m1.Bonds.Count];
                foreach (Bond bond in m1.Bonds)
                {
                    mj.b[iBond] = new BondJSON()
                    {
                        i = bond.Id,
                        b = indexLookup[bond.StartAtom],
                        e = indexLookup[bond.EndAtom]
                    };

                    if (bond.Stereo == BondStereo.Wedge)
                    {
                        mj.b[iBond].s = Protruding;
                    }
                    else if (bond.Stereo == BondStereo.Hatch)
                    {
                        mj.b[iBond].s = Recessed;
                    }
                    else if (bond.Stereo == BondStereo.Indeterminate)
                    {
                        mj.b[iBond].s = Ambiguous;
                    }
                    if (bond.Order != Bond.OrderSingle)
                    {
                        mj.b[iBond].o = bond.OrderValue;
                    }
                    iBond++;
                }
            }
            return mj;
        }

        public Chem4Word.Model.Model Import(object data)
        {
            var jsonModel = JsonConvert.DeserializeObject<ModelJSON>(data as string);

            var newModel = new Chem4Word.Model.Model();
            if (jsonModel.m != null)
            {
                foreach (var molJson in jsonModel.m)
                {
                    AddMolecule(molJson, newModel);
                }
            }
            else
            {
                var jsonMol = JsonConvert.DeserializeObject<MolJSON>(data as string);
                AddMolecule(jsonMol, newModel);
            }

            return newModel;
        }

        private static void AddMolecule(dynamic data, Model newModel)
        {
            var newMol = new Molecule();
            Element ce;
            foreach (AtomJSON a in data.a)
            {
                if (!string.IsNullOrEmpty(a.l))
                {
                    bool OK = Globals.PeriodicTable.HasElement(a.l);
                    ce = Globals.PeriodicTable.Elements[a.l];
                }
                else
                {
                    ce = Globals.PeriodicTable.C;
                }

                Atom atom = new Atom()
                {
                    Element = ce,
                    Position = new Point(a.x, a.y)
                };

                if (a.c != null)
                {
                    atom.FormalCharge = a.c.Value;
                }
                newMol.Atoms.Add(atom);
            }

            if (data.b != null)
            {
                foreach (BondJSON b in data.b)
                {
                    string o;
                    if (b.o != null)
                    {
                        o = Bond.OrderValueToOrder(double.Parse(b.o.ToString()));
                    }
                    else
                    {
                        o = Bond.OrderSingle;
                    }

                    BondStereo s;
                    if (!string.IsNullOrEmpty(b.s))
                    {
                        if (o == Bond.OrderDouble)
                        {
                            if (b.s.Equals(Ambiguous))
                            {
                                s = BondStereo.Indeterminate;
                            }
                            else
                            {
                                s = BondStereo.None;
                            }
                        }
                        else
                        {
                            if (b.s.Equals(Recessed))
                            {
                                s = BondStereo.Hatch;
                            }
                            else if (b.s.Equals(Protruding))
                            {
                                s = BondStereo.Wedge;
                            }
                            else if (b.s.Equals(Ambiguous))
                            {
                                s = BondStereo.Indeterminate;
                            }
                            else
                            {
                                s = BondStereo.None;
                            }
                        }
                    }
                    else
                    {
                        s = BondStereo.None;
                    }

                    Bond newBond = new Bond()
                    {
                        StartAtom = newMol.Atoms[b.b.Value],
                        EndAtom = newMol.Atoms[b.e.Value],
                        Stereo = s,
                        Order = o
                    };
                    newMol.Bonds.Add(newBond);
                }
            }
            newModel.Molecules.Add(newMol);
        }

        public bool CanImport
        {
            get { return true; }
        }

        public bool CanExport
        {
            get { return true; }
        }
    }
}