using System;
using System.Threading;
using System.Windows.Forms;
using Chem4Word.Shared;

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
                    string dl = FolderHelper.GetPath(KnownFolder.Downloads);
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