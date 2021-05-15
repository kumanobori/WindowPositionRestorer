using System;
using System.Windows.Forms;
using NLog;

namespace WindowPositionRestorerTaskIcon
{
    static class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                logger.Info("start.");
                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //Application.Run(new DummyForm());
                Form f = new DummyForm();
                Application.Run();
            }
            catch (Exception e)
            {
                logger.Error(e.Message);
                logger.Error(e.StackTrace);
            }
            finally
            {
                logger.Info("terminated");
            }
        }
    }
}
