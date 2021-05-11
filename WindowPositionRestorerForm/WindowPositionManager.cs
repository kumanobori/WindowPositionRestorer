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
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// EnumWindowsで列挙されるウィンドウの総数を格納する。進捗状況管理に用いる。
        /// </summary>
        private int enumWindowCount;

        /// <summary>
        /// 復元候補として保存されたウィンドウの総数を格納する。進捗状況管理に用いる。
        /// </summary>
        private int savedWindowCount;

        /// <summary>
        /// WindowPositionのコレクション
        /// </summary>
        List<WindowPosition> listPosition = new();

        [DllImport("user32")]
        private static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, IntPtr lParam);
        private delegate bool WNDENUMPROC(IntPtr hWnd, IntPtr lParam);

        /// <summary>
        /// EnumWindowsで列挙されるウィンドウの数を取得する。
        /// </summary>
        /// <returns>EnumWindowsで列挙されるウィンドウの数</returns>
        public int FetchWindowCount()
        {
            logger.Info("fetchWindowCount start.");
            
            enumWindowCount = 0;
            // ウィンドウごとにEnumerateWindowを実行する
            EnumWindows(EnumerateWindowForCount, IntPtr.Zero);
            
            logger.Info("fetchWindowCount term.");
            return enumWindowCount;
        }
        /// <summary>
        /// fetchWindowCount()で呼ぶEnumWindowsからのコールバック関数
        /// ウィンドウの数を増分する。        
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private bool EnumerateWindowForCount(IntPtr hWnd, IntPtr lParam)
        {
            enumWindowCount++;
            return true;
        }

        /// <summary>
        /// ウィンドウ一覧を取得、保存する
        /// </summary>
        public int Save()
        {
            logger.Info("save start.");
            listPosition = new List<WindowPosition>();
            savedWindowCount = 0;

            // ウィンドウごとにEnumerateWindowを実行する
            EnumWindows(EnumerateWindow, IntPtr.Zero);
            logger.Info("save term.");
            return savedWindowCount;
        }

        /// <summary>
        /// save()で呼ぶEnumWindowsからのコールバック関数
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private bool EnumerateWindow(IntPtr hWnd, IntPtr lParam)
        {
            SaveWindowInfoIfRequired(hWnd);
            savedWindowCount++;
            return true;
        }

        /// <summary>
        /// ウィンドウ情報を取得し、必要なら保存する
        /// </summary>
        /// <param name="hWnd"></param>
        private void SaveWindowInfoIfRequired(IntPtr hWnd)
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
        public void Restore()
        {
            logger.Info("restore start.");
            // 実行結果のクリア
            listPosition.ForEach(x => x.result.Clear());

            // 保存しているウィンドウ位置情報ごとに、復元要否を判定して必要なら復元する
            foreach (WindowPosition saved in listPosition)
            {
                WindowPosition current = new(saved.hWnd);
                if (saved.isToRestore(current))
                {
                    saved.restore();
                }
            }
            logger.Info("restore term.");
        }

        /// <summary>
        /// 保存しているウィンドウ情報を文字列で返す。
        /// </summary>
        /// <returns>保存しているウィンドウ情報の文字列</returns>
        public string FetchResults()
        {
            logger.Info("fetchResults start.");
            List<string> listResult = new();
            listPosition.ForEach(x => listResult.Add(x.toStringForDisp()));
            logger.Info("fetchResults term.");
            return String.Join("\r\n", listResult);
        }
    }
}
