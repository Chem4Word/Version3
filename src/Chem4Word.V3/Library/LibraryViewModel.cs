// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Controls.Annotations;
using Chem4Word.Core.UI.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Chem4Word.Library
{
    public class LibraryViewModel : DependencyObject
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType.Name;

        private bool _initializing = false;

        public class Chemistry : INotifyPropertyChanged
        {
            public ObservableCollection<UserTag> Tags { get; }

            internal XDocument cmlDoc;

            public string XML
            {
                get { return cmlDoc.ToString(); }
                set
                {
                    cmlDoc = XDocument.Parse(value);
                    if (!Initializing)
                    {
                        Save();
                    }
                    OnPropertyChanged();
                }
            }

            private string _name;

            public string Name
            {
                get { return _name; }
                set
                {
                    _name = value;
                    if (!Initializing)
                    {
                        Save();
                    }
                    OnPropertyChanged();
                }
            }

            public List<String> OtherNames { get; }
            public bool HasOtherNames { get; internal set; }

            private long _id;

            public long ID
            {
                get { return _id; }
                set
                {
                    _id = value;
                    OnPropertyChanged();
                }
            }

            private string _formula;

            public string Formula
            {
                get { return _formula; }
                set
                {
                    _formula = value;
                    if (!Initializing)
                    {
                        Save();
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            [NotifyPropertyChangedInvocator]
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
                try
                {
                    Dirty = true;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
                catch (Exception ex)
                {
                    new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                }
            }

            public void Save()
            {
                string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
                try
                {
                    LibraryModel.UpdateChemistry(ID, Name, XML, Formula);
                    Dirty = false;
                }
                catch (Exception ex)
                {
                    new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                }
            }

            public bool Dirty { get; set; }
            public bool Initializing { get; set; }

            public Chemistry()
            {
                string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
                try
                {
                    OtherNames = new List<string>();
                    Dirty = false;
                    Tags = new ObservableCollection<UserTag>();
                    Tags.CollectionChanged += Tags_CollectionChanged;
                    HasOtherNames = false;
                }
                catch (Exception ex)
                {
                    new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                }
            }

            private void Tags_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
                try
                {
                    if (Initializing)
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                }
            }
        }

        public class UserTag : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            private long _id;

            public long ID
            {
                get { return _id; }
                set { _id = value; }
            }

            private string _text;

            public string Text
            {
                get { return _text; }
                set { _text = value; }
            }

            [NotifyPropertyChangedInvocator]
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
                try
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
                catch (Exception ex)
                {
                    new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                }
            }
        }

        public class ChemistryByTag : INotifyPropertyChanged
        {
            private long _galleryID;

            public long GalleryID
            {
                get { return _galleryID; }
                set { _galleryID = value; }
            }

            private long _tagID;

            public long tagID
            {
                get { return _tagID; }
                set { _tagID = value; }
            }

            private long _id;

            public long ID
            {
                get { return _id; }
                set { _id = value; }
            }

            [NotifyPropertyChangedInvocator]
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
                try
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                }
                catch (Exception ex)
                {
                    new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        //used for XAML data binding
        public ObservableCollection<LibraryItem> GalleryItems { get; }

        public ObservableCollection<Chemistry> ChemistryItems { get; }

        public ObservableCollection<ChemistryByTag> ChemistryByTagItems { get; }

        public ObservableCollection<UserTag> UserTagItems { get; }

        // NOT SURE WE NEED THIS!!
        //references the custom XML parts in the document

        public LibraryViewModel(string filter = "")
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                ChemistryItems = new ObservableCollection<Chemistry>();
                ChemistryItems.CollectionChanged += ChemistryItems_CollectionChanged;

                LoadChemistryItems(filter);

                ChemistryByTagItems = new ObservableCollection<ChemistryByTag>();
                LoadChemistryByTagItems();

                UserTagItems = new ObservableCollection<UserTag>();
                LoadUserTagItems();
                AssignUserTags();

                GalleryItems = new ObservableCollection<LibraryItem>();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void AssignUserTags()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                foreach (Chemistry chemistryItem in ChemistryItems)
                {
                    chemistryItem.Initializing = true;
                    var specificTags = from UserTag ut in UserTagItems
                                       join ChemistryByTag cbt in ChemistryByTagItems
                                       on ut.ID equals cbt.tagID
                                       where cbt.GalleryID == chemistryItem.ID
                                       select ut;
                    foreach (UserTag ut2 in specificTags)
                    {
                        chemistryItem.Tags.Add(ut2);
                    }
                    chemistryItem.Initializing = false;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void ChemistryItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        if (!_initializing)
                        {
                            AddNewChemistry(e.NewItems);
                        }
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        if (!_initializing)
                        {
                            DeleteChemistry(e.OldItems);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void DeleteChemistry(IList eOldItems)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (!_initializing)
                {
                    foreach (Chemistry chemistry in eOldItems)
                    {
                        LibraryModel.DeleteChemistry(chemistry.ID);
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void AddNewChemistry(IList eNewItems)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (!_initializing)
                {
                    foreach (Chemistry chemistry in eNewItems)
                    {
                        chemistry.ID = LibraryModel.AddChemistry(chemistry.XML, chemistry.Name, chemistry.Formula);
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        public void LoadUserTagItems()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                UserTagItems.Clear();
                SQLiteDataReader allTags = LibraryModel.GetAllUserTags();

                while (allTags.Read())
                {
                    var tag = new UserTag();
                    tag.ID = (long)allTags["ID"];
                    tag.Text = (string)allTags["UserTag"];
                    UserTagItems.Add(tag);
                }

                allTags.Close();
                allTags.Dispose();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        public ObservableCollection<UserTag> LoadUserTagItems(int ChemistryID)
        {
            var results = new ObservableCollection<UserTag>();
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                SQLiteDataReader allTags = LibraryModel.GetAllUserTags(ChemistryID);

                while (allTags.Read())
                {
                    var tag = new UserTag();
                    tag.ID = (long)allTags["ID"];
                    tag.Text = (string)allTags["UserTag"];
                    results.Add(tag);
                }

                allTags.Close();
                allTags.Dispose();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
            return results;
        }

        public void LoadChemistryByTagItems()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                ChemistryByTagItems.Clear();
                SQLiteDataReader allTags = LibraryModel.GetChemistryByTags();

                while (allTags.Read())
                {
                    var tag = new ChemistryByTag();
                    tag.ID = (long)allTags["ID"];
                    tag.GalleryID = (long)allTags["GalleryID"];
                    tag.tagID = (long)allTags["TagID"];
                    ChemistryByTagItems.Add(tag);
                }

                allTags.Close();
                allTags.Dispose();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        public void LoadChemistryItems(string filter)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                _initializing = true;
                ChemistryItems.Clear();
                SQLiteDataReader chemistry = LibraryModel.GetAllChemistry(filter);

                while (chemistry.Read())
                {
                    var mol = new Chemistry();

                    mol.Initializing = true;

                    mol.ID = (long)chemistry["ID"];
                    var byteArray = (Byte[])chemistry["Chemistry"];
                    mol.XML = Encoding.UTF8.GetString(byteArray);
                    mol.Name = chemistry["name"] as string;
                    mol.Formula = chemistry["formula"] as string;
                    ChemistryItems.Add(mol);
                    LoadOtherNames(mol);
                    mol.Initializing = false;
                }

                chemistry.Close();
                chemistry.Dispose();

                _initializing = false;
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void LoadOtherNames(Chemistry mol)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                XName nameNodeName = Chem4Word.Model.Converters.CML.cml + "name";

                var names = (from namenode in mol.cmlDoc.Descendants(nameNodeName)
                             where namenode.Name == nameNodeName
                             select namenode.Value).Distinct();

                foreach (string name in names)

                {
                    mol.HasOtherNames = true;
                    mol.OtherNames.Add(name);
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        public ListBoxItem SelectedItem
        {
            get { return (ListBoxItem)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(ListBoxItem), typeof(LibraryViewModel), new PropertyMetadata(null));

        public void SaveChanges()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                SaveChemistryChanges();
                //SaveTagChanges();
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void SaveTagChanges()
        {
            throw new NotImplementedException();
        }

        private void SaveChemistryChanges()
        {
            foreach (Chemistry chemistryItem in ChemistryItems)
            {
                if (chemistryItem.Dirty)
                {
                    chemistryItem.Save();
                }
            }
        }
    }
}