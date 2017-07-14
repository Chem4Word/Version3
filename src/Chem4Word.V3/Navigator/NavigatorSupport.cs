using Chem4Word.Controls;
using Chem4Word.Core;
using Chem4Word.Helpers;
using Chem4Word.Model.Converters;
using IChem4Word.Contracts;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;

namespace Chem4Word.Navigator
{
    internal class NavigatorSupport
    {
        public static void InsertChemistry(bool isCopy, Microsoft.Office.Interop.Word.Application app, FlexDisplay flexDisplay)
        {
            Microsoft.Office.Interop.Word.Document doc = app.ActiveDocument;
            Selection sel = app.Selection;
            Microsoft.Office.Interop.Word.ContentControl cc = null;

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
    }
}