using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace ChemDoodlePoc
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            frmMain f = new frmMain();
            Application.Run(f);
            Debug.WriteLine("frmMain Closed");
            Debug.WriteLine(f.MolStructure);
        }
    }
}