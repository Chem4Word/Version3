// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using Chem4Word.Controls;
using Chem4Word.Core;
using Chem4Word.Helpers;
using Chem4Word.Model.Converters;
using IChem4Word.Contracts;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Chem4Word.Core.UI.Forms;
using Microsoft.Office.Core;
using Shape = Microsoft.Office.Interop.Word.Shape;

namespace Chem4Word.Navigator
{
    internal class NavigatorSupport
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType.Name;

        public static void InsertChemistry(bool isCopy, Application app, FlexDisplay flexDisplay)
        {
            string module = $"{_product}.{_class}.{MethodBase.GetCurrentMethod().Name}()";

            Document doc = app.ActiveDocument;
            Selection sel = app.Selection;
            ContentControl cc = null;

            WdContentControlType? contentControlType = null;
            foreach (ContentControl ccd in doc.ContentControls)
            {
                if (ccd.Range.Start <= sel.Range.Start && ccd.Range.End >= sel.Range.End)
                {
                    contentControlType = ccd.Type;
                }
            }

            string message = "";
            bool allowedToInsert = (sel.StoryType == WdStoryType.wdMainTextStory);
            if (!allowedToInsert)
            {
                message = $"You can't insert a chemistry object inside a {sel.StoryType}";
            }

            if (allowedToInsert && contentControlType != null && contentControlType != WdContentControlType.wdContentControlRichText)
            {
                allowedToInsert = false;
                message = $"You can't insert a chemistry object inside a {DecodeContentControlType(contentControlType)} control";
            }

            if (allowedToInsert)
            {
                if (sel.ContentControls.Count > 0)
                {
                    cc = sel.ContentControls[1];
                    if (cc.Title != null && cc.Title.Equals(Constants.ContentControlTitle))
                    {
                        UserInteractions.WarnUser("You can't insert a chemistry object inside a chemistry object");
                    }
                }
                else
                {
                    app.ScreenUpdating = false;
                    Globals.Chem4WordV3.DisableDocumentEvents(doc);

                    try
                    {
                        CMLConverter cmlConverter = new CMLConverter();
                        Model.Model chem = cmlConverter.Import(flexDisplay.Chemistry);
                        if (chem.MeanBondLength < 5 || chem.MeanBondLength > 100)
                        {
                            chem.Rescale(20);
                        }

                        if (isCopy)
                        {
                            // Always generate new Guid on Import
                            chem.CustomXmlPartGuid = Guid.NewGuid().ToString("N");
                        }

                        string guidString = chem.CustomXmlPartGuid;
                        string bookmarkName = "C4W_" + guidString;

                        Globals.Chem4WordV3.SystemOptions.WordTopLeft = Globals.Chem4WordV3.WordTopLeft;

                        IChem4WordRenderer renderer =
                            Globals.Chem4WordV3.GetRendererPlugIn(
                                Globals.Chem4WordV3.SystemOptions.SelectedRendererPlugIn);

                        if (renderer == null)
                        {
                            UserInteractions.WarnUser("Unable to find a Renderer Plug-In");
                        }
                        else
                        {
                            // Export just incase the CustomXmlPartGuid has been changed
                            string cml = cmlConverter.Export(chem);
                            renderer.Properties = new Dictionary<string, string>();
                            renderer.Properties.Add("Guid", guidString);
                            renderer.Cml = cml;

                            string tempfileName = renderer.Render();

                            cc = CustomRibbon.Insert2D(doc, tempfileName, bookmarkName, guidString);

                            if (isCopy)
                            {
                                doc.CustomXMLParts.Add(cml);
                            }

                            try
                            {
                                // Delete the temporary file now we are finished with it
                                File.Delete(tempfileName);
                            }
                            catch
                            {
                                // Not much we can do here
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        new ReportError(Globals.Chem4WordV3.Telemetry, Globals.Chem4WordV3.WordTopLeft, module, ex).ShowDialog();
                    }
                    finally
                    {
                        // Tidy Up - Resume Screen Updating and Enable Document Event Handlers
                        app.ScreenUpdating = true;
                        Globals.Chem4WordV3.EnableDocumentEvents(doc);

                        if (cc != null)
                        {
                            // Move selection point into the Content Control which was just edited or added
                            app.Selection.SetRange(cc.Range.Start, cc.Range.End);
                        }
                    }
                }
            }
            else
            {
                UserInteractions.WarnUser(message);
            }
        }

        private static string DecodeContentControlType(WdContentControlType? contentControlType)
        {
            string result = "";

            switch (contentControlType)
            {
                case WdContentControlType.wdContentControlRichText:
                    result = "Rich Text";
                    break;
                case WdContentControlType.wdContentControlText:
                    result = "Text";
                    break;
                case WdContentControlType.wdContentControlBuildingBlockGallery:
                    result = "Picture";
                    break;
                case WdContentControlType.wdContentControlComboBox:
                    result = "Combo Box";
                    break;
                case WdContentControlType.wdContentControlDropdownList:
                    result = "Drop Down List";
                    break;
                case WdContentControlType.wdContentControlPicture:
                    result = "Building Block Gallery";
                    break;
                case WdContentControlType.wdContentControlDate:
                    result = "Date Picker";
                    break;
                case WdContentControlType.wdContentControlGroup:
                    result = "Group";
                    break;
                case WdContentControlType.wdContentControlCheckBox:
                    result = "Check Box";
                    break;
                case WdContentControlType.wdContentControlRepeatingSection:
                    result = "Repeating Section";
                    break;

                default:
                    result = contentControlType.ToString();
                    break;
            }

            return result;
        }
    }
}
