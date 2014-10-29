using System;
using System.Windows.Forms;
using SelfDC.Utils;
using System.Reflection;
using System.IO;

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
            String appName = Assembly.GetExecutingAssembly().GetName().Name;
            String appVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            /* Load app settings */
            ScsUtils.WriteLog(string.Format("=== {0} ver. {1} ===", appName, appVersion));
            ScsUtils.WriteLog("Caricamento impostazioni...");
            Settings.AppCfgFileName = string.Format("{0}\\conf.txt", ScsUtils.GetAppPath());
            Settings.LoadFromFile(Settings.AppCfgFileName);

            /* show main form */
            Application.Run(new MainMenu());

            /* Save Config File */
            if (!File.Exists(Settings.AppCfgFileName))
            {
                Settings.SaveToFile(Settings.AppCfgFileName);
            }
            ScsUtils.WriteLog("Applicazione terminata correttamente");
        }
    }
}