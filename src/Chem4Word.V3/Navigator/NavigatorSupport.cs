// ---------------------------------------------------------------------------
//  Copyright (c) 2023, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System.Reflection;
using Chem4Word.Core.Helpers;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Tools;

namespace Chem4Word.Navigator
{
    internal class NavigatorSupport
    {
        private static string _product = Assembly.GetExecutingAssembly().FullName.Split(',')[0];
        private static string _class = MethodBase.GetCurrentMethod().DeclaringType?.Name;

        public static void SelectNavigatorItem(string guid)
        {
            CustomTaskPane custTaskPane = null;
            foreach (CustomTaskPane taskPane in Globals.Chem4WordV3.CustomTaskPanes)
            {
                Application app = Globals.Chem4WordV3.Application;
                if (app.ActiveWindow == taskPane.Window && taskPane.Title == Constants.NavigatorTaskPaneTitle)
                {
                    custTaskPane = taskPane;
                }

                if (custTaskPane != null)
                {
                    var navHost = custTaskPane.Control as NavigatorHost;
                    if (navHost != null)
                    {
                        var navigatorList = navHost.navigatorView1.NavigatorList;
                        int idx = 0;
                        foreach (var item in navigatorList.Items)
                        {
                            var navigatorItem = item as NavigatorItem;
                            if (navigatorItem.CMLId.Equals(guid))
                            {
                                navigatorList.SelectedIndex = idx;
                                navigatorList.ScrollIntoView(navigatorList.SelectedItem);
                                break;
                            }
                            idx++;
                        }
                    }
                }
            }
        }
    }
}