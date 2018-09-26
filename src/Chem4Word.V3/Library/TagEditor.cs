// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Core.UI.Forms;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Chem4Word.Library
{
    public class TagEditor : RichTextBox
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType?.Name;

        public static readonly DependencyProperty TagTemplateProperty =
                DependencyProperty.Register("TagTemplate", typeof(DataTemplate), typeof(TagEditor));

        public DataTemplate TagTemplate
        {
            get { return (DataTemplate)GetValue(TagTemplateProperty); }
            set { SetValue(TagTemplateProperty, value); }
        }

        public Func<string, object> TagMatcher { get; set; }

        public string TagString
        {
            get { return (string)GetValue(TagStringProperty); }
            set { SetValue(TagStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TagString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TagStringProperty =
            DependencyProperty.Register("TagString", typeof(string), typeof(TagEditor), new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure, TagStringChanged));

        private static void TagStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                TagEditor rtb = (TagEditor)d;
                string newTags = (string)args.NewValue;

                rtb.TextChanged -= rtb.OnTagTextChanged;

                try
                {
                    rtb.Document.Blocks.Clear();
                    var para = new Paragraph(new Run());
                    rtb.Document.Blocks.Add(para);
                    string[] allTags = newTags.Split(new char[] { ';', ',' });
                    foreach (string tag in allTags)
                    {
                        var tagString = tag.Trim();
                        if (tagString != "")
                        {
                            var presenter = new ContentPresenter()
                            {
                                Content = tagString,
                                ContentTemplate = rtb.TagTemplate,
                            };
                            para.Inlines.Add(presenter);
                        }
                    }
                }
                catch (Exception e)
                {
                    new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, e).ShowDialog();
                }
                finally
                {
                    rtb.TextChanged += rtb.OnTagTextChanged;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        public TagEditor()
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                TextChanged += OnTagTextChanged;
                PreviewKeyDown += TagEditor_PreviewKeyDown;

                TagMatcher = text =>
                {
                    if (text.EndsWith(";") | text.EndsWith(" "))
                    {
                        // Remove the ';'
                        return text.Substring(0, text.Length - 1).Trim().ToUpper();
                    }

                    return null;
                };
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void TagEditor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                Debug.WriteLine($"Keydown code = {e.Key} ");
                if (e.Key == Key.Return)
                {
                    //swallow the keystroke
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        public void OnTagTextChanged(object sender, TextChangedEventArgs e)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";
            try
            {
                var text = CaretPosition.GetTextInRun(LogicalDirection.Backward);
                if (TagMatcher != null)
                {
                    var token = TagMatcher(text);
                    if (token != null)
                    {
                        ReplaceTextWithTag(text, token);
                    }
                }
            }
            catch (Exception ex)
            {
                new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
            }
        }

        private void ReplaceTextWithTag(string inputText, object token)
        {
            // Remove the handler temporarily as we will be modifying tokens below, causing more TextChanged events
            TextChanged -= OnTagTextChanged;

            var para = CaretPosition.Paragraph;

            var matchedRun = para.Inlines.FirstOrDefault(inline =>
            {
                var run = inline as Run;
                return (run != null && run.Text.EndsWith(inputText));
            }) as Run;
            if (matchedRun != null) // Found a Run that matched the inputText
            {
                var tokenContainer = CreateTagContainer(inputText, token);
                para.Inlines.InsertBefore(matchedRun, tokenContainer);

                // Remove only if the Text in the Run is the same as inputText, else split up
                if (matchedRun.Text == inputText)
                {
                    para.Inlines.Remove(matchedRun);
                }
                else // Split up
                {
                    var index = matchedRun.Text.IndexOf(inputText) + inputText.Length;
                    var tailEnd = new Run(matchedRun.Text.Substring(index));
                    para.Inlines.InsertAfter(matchedRun, tailEnd);
                    para.Inlines.Remove(matchedRun);
                }
            }

            TextChanged += OnTagTextChanged;
        }

        private InlineUIContainer CreateTagContainer(string inputText, object token)
        {
            // Note: we are not using the inputText here, but could be used in future

            var presenter = new ContentPresenter()
            {
                Content = token,
                ContentTemplate = TagTemplate,
            };

            // BaselineAlignment is needed to align with Run
            return new InlineUIContainer(presenter)
            {
                BaselineAlignment = BaselineAlignment.TextBottom
            };
        }
    }
}