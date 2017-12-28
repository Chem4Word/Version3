// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Chem4Word.Model
{
    /// <summary>
    /// Abstract class from which Model and Molecule both derive.
    /// This allows changes in the atoms and bonds membership to bubble up
    /// the molecule hierarchy
    /// </summary>
    public abstract class ChemistryContainer
    {
        public ObservableCollection<Bond> AllBonds { get; }
        public ObservableCollection<Atom> AllAtoms { get; }

        public ObservableCollection<Molecule> Molecules { get; }

        public ChemistryContainer Parent { get; set; }

        protected ChemistryContainer()
        {
            AllAtoms = new ObservableCollection<Atom>();

            AllAtoms.CollectionChanged += AllAtoms_CollectionChanged;
            AllBonds = new ObservableCollection<Bond>();
            AllBonds.CollectionChanged += AllBonds_CollectionChanged;
            Molecules = new ObservableCollection<Molecule>();
            Molecules.CollectionChanged += Molecules_CollectionChanged;
        }

        private void Molecules_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddNewAtomsAndBonds(e);
                    break;

                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Remove:
                    RemoveOldAtomsAndBond(e);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    AddNewAtomsAndBonds(e);
                    RemoveOldAtomsAndBond(e);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    break;
            }
        }

        private void RemoveOldAtomsAndBond(NotifyCollectionChangedEventArgs e)
        {
            foreach (Molecule child in e.OldItems)
            {
                child.Parent = null;

                foreach (Atom atom in child.Atoms.ToList())
                {
                    AllAtoms.Remove(atom);
                }

                foreach (Bond bond in child.Bonds.ToList())
                {
                    AllBonds.Remove(bond);
                }
            }
        }

        private void AddNewAtomsAndBonds(NotifyCollectionChangedEventArgs e)
        {
            foreach (Molecule child in e.NewItems)
            {
                child.Parent = this;

                foreach (Atom atom in child.Atoms)
                {
                    if (!AllAtoms.Contains(atom))
                    {
                        AllAtoms.Add(atom);
                    }
                }

                foreach (Bond bond in child.Bonds)
                {
                    if (!AllBonds.Contains(bond))
                    {
                        AllBonds.Add(bond);
                    }
                }
            }
        }

        private void AllBonds_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (Parent != null)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (Bond bond in e.NewItems)
                        {
                            this.Parent.AllBonds.Add(bond);
                        }
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        foreach (Bond bond in e.OldItems)
                        {
                            this.Parent.AllBonds.Remove(bond);
                        }
                        break;

                    case NotifyCollectionChangedAction.Replace:
                        foreach (Bond bond in e.NewItems)
                        {
                            this.Parent.AllBonds.Add(bond);
                        }
                        foreach (Bond bond in e.OldItems)
                        {
                            this.Parent.AllBonds.Remove(bond);
                        }
                        break;

                    case NotifyCollectionChangedAction.Reset:
                        break;
                }
            }
        }

        private void AllAtoms_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (Parent != null)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (Atom atom in e.NewItems)
                        {
                            this.Parent.AllAtoms.Add(atom);
                        }
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        foreach (Atom atom in e.OldItems)
                        {
                            this.Parent.AllAtoms.Remove(atom);
                        }
                        break;

                    case NotifyCollectionChangedAction.Replace:
                        foreach (Atom atom in e.NewItems)
                        {
                            this.Parent.AllAtoms.Add(atom);
                        }
                        foreach (Atom atom in e.OldItems)
                        {
                            this.Parent.AllAtoms.Remove(atom);
                        }
                        break;

                    case NotifyCollectionChangedAction.Reset:
                        break;
                }
            }
        }
    }
}