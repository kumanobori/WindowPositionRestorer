using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using NLog;
namespace WindowsPositionRestorerForm
{
    class WindowPositionManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        List<WindowPosition> listPosition = new List<WindowPosition>();

        [DllImport("user32")]
        private static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, IntPtr lParam);

        // EnumWindowsから呼び出されるコールバック関数WNDENUMPROCのデリゲート
        private delegate bool WNDENUMPROC(IntPtr hWnd, IntPtr lParam);

        public void save()
        {
            listPosition = new List<WindowPosition>();

            EnumWindows(EnumerateWindow, IntPtr.Zero);

        }

        /// <summary>
        /// EnumWindowsからのコールバック関数
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private bool EnumerateWindow(IntPtr hWnd, IntPtr lParam)
        {
            saveWindowInfo(hWnd);
            return true;
        }
        private void saveWindowInfo(IntPtr hWnd)
        {

            var manager = new WindowPosition(hWnd);

            if (manager.isToSave())
            {
                listPosition.Add(manager);
            }

        }

        public void restore()
        {
            listPosition.ForEach(x => x.result.Clear());

            foreach (WindowPosition x in listPosition)
            {
                WindowPosition current = new WindowPosition(x.hWnd);
                if (x.isToRestore(current))
                {
                    x.restore();
                }
            }
        }



        public string fetchResults()
        {
            List<string> listResult = new List<string>();
            listPosition.ForEach(x => listResult.Add(x.toStringForDisp()));
            return String.Join("\r\n", listResult);
        }
    }
}
