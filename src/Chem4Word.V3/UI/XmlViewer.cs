// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core.UI.Forms;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace Chem4Word.UI
{
    public partial class XmlViewer : Form
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType?.Name;

        private readonly Color BracketsColor = Color.Blue; // "<" "/" ">"
        private readonly Color EqualsColor = Color.Blue;
        private readonly Color QuoteColor = Color.Black;

        private readonly Color ElementNameColor = Color.DarkRed;
        private readonly Color ElementValueColor = Color.Black;

        private readonly Color AttributeNameColor = Color.Red;
        private readonly Color AttributeValueColor = Color.Blue;

        public System.Windows.Point TopLeft { get; set; }

        public string XmlString { get; set; }

        public XmlViewer()
        {
            InitializeComponent();
        }

        private void FormXmlViewer_Load(object sender, EventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                if (TopLeft.X != 0 && TopLeft.Y != 0)
                {
                    Left = (int)TopLeft.X;
                    Top = (int)TopLeft.Y;
                }

                XmlTextReader reader = new XmlTextReader(new StringReader(XmlString));
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            richTextBox1.SelectionColor = BracketsColor;
                            richTextBox1.AppendText("<");
                            richTextBox1.SelectionColor = ElementNameColor;
                            richTextBox1.AppendText(reader.Name);
                            if (reader.HasAttributes)
                            {
                                for (int i = 0; i < reader.AttributeCount; i++)
                                {
                                    reader.MoveToAttribute(i);
                                    richTextBox1.AppendText(" ");
                                    richTextBox1.SelectionColor = AttributeNameColor;
                                    richTextBox1.AppendText(reader.Name);
                                    richTextBox1.SelectionColor = EqualsColor;
                                    richTextBox1.AppendText("=");
                                    richTextBox1.SelectionColor = QuoteColor;
                                    richTextBox1.AppendText("\"");
                                    richTextBox1.SelectionColor = AttributeValueColor;
                                    richTextBox1.AppendText(reader.Value);
                                    richTextBox1.SelectionColor = QuoteColor;
                                    richTextBox1.AppendText("\"");
                                }
                                reader.MoveToElement();
                            }

                            richTextBox1.SelectionColor = BracketsColor;
                            if (reader.IsEmptyElement)
                            {
                                richTextBox1.AppendText(" />");
                            }
                            else
                            {
                                richTextBox1.AppendText(">");
                            }
                            break;

                        case XmlNodeType.Text:
                            richTextBox1.SelectionColor = ElementValueColor;
                            richTextBox1.AppendText(reader.Value);
                            break;

                        case XmlNodeType.EndElement:
                            richTextBox1.SelectionColor = BracketsColor;
                            richTextBox1.AppendText("</");
                            richTextBox1.SelectionColor = ElementNameColor;
                            richTextBox1.AppendText(reader.Name);
                            richTextBox1.SelectionColor = BracketsColor;
                            richTextBox1.AppendText(">");
                            break;

                        case XmlNodeType.Whitespace:
                            richTextBox1.AppendText(reader.Value);
                            break;

                        case XmlNodeType.XmlDeclaration:
                            // Ignore this
                            break;

                        default:
                            Debug.WriteLine($"Unhandled Node is {reader.NodeType}");
                            break;
                    }
                }

                richTextBox1.SelectionStart = 0;
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }
    }
}