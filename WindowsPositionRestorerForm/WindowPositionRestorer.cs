using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NLog;
namespace WindowsPositionRestorerForm
{
    /// <summary>
    /// WindowPosisionの操作用クラス。
    /// </summary>
    class WindowPositionManager
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();
        
        /// <summary>
        /// WindowPositionのコレクション
        /// </summary>
        List<WindowPosition> listPosition = new List<WindowPosition>();

        [DllImport("user32")]
        private static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, IntPtr lParam);
        private delegate bool WNDENUMPROC(IntPtr hWnd, IntPtr lParam);

        /// <summary>
        /// ウィンドウ一覧を取得、保存する
        /// </summary>
        public void save()
        {
            listPosition = new List<WindowPosition>();

            // ウィンドウごとにEnumerateWindowを実行する
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
            saveWindowInfoIfRequired(hWnd);
            return true;
        }

        /// <summary>
        /// ウィンドウ情報を取得し、必要なら保存する
        /// </summary>
        /// <param name="hWnd"></param>
        private void saveWindowInfoIfRequired(IntPtr hWnd)
        {
            // ウィンドウ情報取得・生成
            var windowPosition = new WindowPosition(hWnd);

            // 保存対象と判定したら保存
            if (windowPosition.isToSave())
            {
                listPosition.Add(windowPosition);
            }

        }

        /// <summary>
        /// 保存したウィンドウ情報と現在のウィンドウ情報を比較し、必要なら復元する
        /// </summary>
        public void restore()
        {
            // 実行結果のクリア
            listPosition.ForEach(x => x.result.Clear());

            // 保存しているウィンドウ位置情報ごとに、復元要否を判定して必要なら復元する
            foreach (WindowPosition x in listPosition)
            {
                WindowPosition current = new WindowPosition(x.hWnd);
                if (x.isToRestore(current))
                {
                    x.restore();
                }
            }
        }

        /// <summary>
        /// 保存しているウィンドウ情報を文字列で返す。
        /// </summary>
        /// <returns>保存しているウィンドウ情報の文字列</returns>
        public string fetchResults()
        {
            List<string> listResult = new List<string>();
            listPosition.ForEach(x => listResult.Add(x.toStringForDisp()));
            return String.Join("\r\n", listResult);
        }
    }
}
