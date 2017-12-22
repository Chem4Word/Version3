// ---------------------------------------------------------------------------
//  Copyright (c) 2018, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.Threading;
using System.Windows.Forms;

namespace Chem4WordSetup
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            bool created;
            using (new Mutex(true, "21ab7215-5081-4e52-86ad-be2408418a57", out created))
            {
                if (created)
                {
                    RegistryHelper.WriteAction("Starting Setup");

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Setup());
                }
                else
                {
                    RegistryHelper.WriteAction("Setup is already running");
                }
            }
        }
    }
}