using DVLD.Presentation.Logger;
using DVLD.Presentation.Logger.StoringStrategies;
using System;
using System.Windows.Forms;

namespace DVLD.Presentation
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppLogger.Provider = new EventViewStoringStrategy("DVLD");

            Application.Run(new FrmLogin());

        }
    }
}
