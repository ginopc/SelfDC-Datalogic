using System;
using System.Windows.Forms;
using SelfDC.Utils;

namespace SelfDC
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            /* Load app settings */
            ScsUtils.WriteLog("===========================");
            ScsUtils.WriteLog("Caricamento impostazioni...");
            Settings.AppCfgFileName = string.Format("{0}\\conf.txt", ScsUtils.GetAppPath());
            Settings.LoadFromFile(Settings.AppCfgFileName);

            /* show main form */
            Application.Run(new MainMenu());

            ScsUtils.WriteLog("Applicazione terminata correttamente");
        }
    }
}