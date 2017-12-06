// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System.Configuration;

namespace Chem4Word.Model.Converters
{
    public class ConverterSection : ConfigurationSection
    {
        [ConfigurationProperty("converters", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(ConverterCollection),
        AddItemName = "add",
        ClearItemsName = "clear",
        RemoveItemName = "remove")]
        public ConverterCollection Converters
        {
            get
            {
                return ((ConverterCollection)base["converters"]);
            }
            set
            {
                base["converters"] = value;
            }
        }
    }

    public class ConverterCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ConverterElement();
        }

        public override ConfigurationElementCollectionType CollectionType
            => ConfigurationElementCollectionType.AddRemoveClearMap;

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ConverterElement)element).Name;
        }

        public override bool IsReadOnly()
        {
            return false;
        }

        public ConverterElement this[int idx]
        {
            get
            {
                return (ConverterElement)BaseGet(idx);
            }
        }

        public void Add(ConverterElement ce)
        {
            BaseAdd(ce);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);

            // Your custom code goes here.
        }
    }

    public class ConverterElement : ConfigurationElement
    {
        public ConverterElement() : base()
        { }

        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("path", DefaultValue = "", IsRequired = true)]
        public string Path
        {
            get { return (string)this["path"]; }
            set { this["path"] = value; }
        }
    }
}
