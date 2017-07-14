using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace Chem4Word.Model.Converters
{
    public class ConverterSectionHandler
    {
        private Dictionary<string, IConverter> _converterCollection;

        public ConverterSectionHandler()
        {
            _converterCollection = new Dictionary<string, IConverter>();
        }

        public ConverterCollection Load()
        {
            var cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var sg = cfg.GetSectionGroup("Chem4Word");

            var section = (ConverterSection)sg.Sections["plugins"];
            return section.Converters;
        }

        public object Create(object parent, object configContext, XmlNode section)
        {
            foreach (XmlNode node in section.ChildNodes)
            {
                try
                {
                    object convObject = Activator.CreateInstance(Type.GetType(node.Attributes["type"].Value));
                    IConverter conv = (IConverter)convObject;
                    _converterCollection[conv.Description] = conv;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return _converterCollection;
        }
    }
}