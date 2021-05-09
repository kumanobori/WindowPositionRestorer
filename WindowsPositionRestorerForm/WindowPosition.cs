using System;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
using NLog;

namespace WindowsPositionRestorerForm
{
    /// <summary>
    /// ウィンドウハンドルとウィンドウ位置情報を記憶し、復元するクラス。
    /// </summary>
    class WindowPosition
    {
        [DllImport("user32", SetLastError = true)]
        private static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32", SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32", SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        /// <summary>
        /// ウィンドウハンドル
        /// </summary>
        public IntPtr hWnd;
        /// <summary>
        /// プロセスID
        /// </summary>
        public int processId;
        /// <summary>
        /// プロセス名
        /// </summary>
        public string processName;
        /// <summary>
        /// クラス名
        /// </summary>
        public string className;
        /// <summary>
        /// ウィンドウタイトルバー文字列
        /// </summary>
        public string windowText;
        /// <summary>
        /// 可視かどうか
        /// </summary>
        public Boolean isVisible;
        /// <summary>
        /// ウィンドウ開始位置x座標
        /// </summary>
        public int left;
        /// <summary>
        /// ウィンドウ開始位置y座標
        /// </summary>
        public int top;
        /// <summary>
        /// ウィンドウ終了位置x座標
        /// </summary>
        public int right;
        /// <summary>
        /// ウィンドウ終了位置y座標
        /// </summary>
        public int bottom;
        /// <summary>
        /// 復元結果文字列を格納する変数
        /// </summary>
        public StringBuilder result = new StringBuilder();

        /// <summary>
        /// ロガー
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// コンストラクタ
        /// ウィンドウハンドルをもとに情報を取得する
        /// </summary>
        /// <param name="hWnd"></param>
        public WindowPosition(IntPtr hWnd)
        {
            // ウィンドウハンドルポインタ
            this.hWnd = hWnd;

            // 可視・不可視
            isVisible = IsWindowVisible(hWnd);

            // タイトルバー文字列とクラス名の最大文字数
            int maxTextLength = 100;

            // タイトルバー文字列
            StringBuilder windowText = new StringBuilder(maxTextLength);
            GetWindowText(hWnd, windowText, windowText.Capacity);
            this.windowText = windowText.ToString();

            // クラス名
            StringBuilder className = new StringBuilder(maxTextLength);
            GetClassName(hWnd, className, className.Capacity);
            this.className = className.ToString();

            // プロセスID
            int processId;
            GetWindowThreadProcessId(hWnd, out processId);
            this.processId = processId;

            // プロセス名
            Process p = Process.GetProcessById(processId);
            this.processName = p.ProcessName;

            // 位置情報
            RECT rect;
            bool flag = GetWindowRect(hWnd, out rect);
            this.left = rect.left;
            this.top = rect.top;
            this.right = rect.right;
            this.bottom = rect.bottom;

        }

        /// <summary>
        /// クラスの文字列情報出力
        /// </summary>
        /// <returns>クラスの文字列情報</returns>
        public string toString()
        {
            return $"◆{hWnd}◆{processId}◆{processName}◆{className}◆{windowText}◆{isVisible}◆{left}◆{top}◆{right}◆{bottom}◆{result.ToString()}◆";
        }
        /// <summary>
        /// 画面表示用の文字列情報出力
        /// </summary>
        /// <returns>画面表示用の文字列情報出力</returns>
        public string toStringForDisp()
        {
            return $"{processName}:{windowText}:{result.ToString()}";
        }
        /// <summary>
        /// 復元対象として保存しておくべきかどうかを判定する
        /// </summary>
        /// <returns>復元対象として保存しておくべきであればtrue</returns>
        public Boolean isToSave()
        {
            // 自分自身のウィンドウは復元対象外とする。理由は、復元ボタンを押した瞬間に移動されると戸惑うから。
            if (hWnd == Process.GetCurrentProcess().MainWindowHandle)
            {
                logger.Debug("restorer itself is not target.");
                return false;
            }

            // 不可視ウィンドウは保存対象外とする
            if (!isVisible)
            {
                logger.Debug("not to save because visible = false. " + toString());
                return false;
            }
            // 位置情報がないウィンドウは保存対象外とする
            if (left == 0 && top == 0 && right == 0 && bottom == 0)
            {
                logger.Debug("not to save because RECT is all zero. " + toString());
                return false;
            }
            // ウィンドウタイトルバー文字列がないウィンドウは保存対象外とする
            if (windowText.Equals(""))
            {
                logger.Debug("not to save because windowText is blank.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 2つのインスタンスの内容が一致するかを判定する
        /// </summary>
        /// <param name="x">比較対象のインスタンス</param>
        /// <returns>2つのインスタンスの内容が一致すればtrue</returns>
        public Boolean equals(WindowPosition x)
        {
            return (hWnd == x.hWnd
                && processId == x.processId
                && processName == x.processName
                && className == x.className
                && windowText == x.windowText
                && isVisible == x.isVisible
                && left == x.left
                && top == x.top
                && right == x.right
                && bottom == x.bottom
                );
        }
        /// <summary>
        /// 現在のインスタンスが、復元されるべきであるかを判定する。
        /// 同一インスタンスが存在し、かつ位置情報が変更されていたら、復元対象と判定する。
        /// </summary>
        /// <param name="x">現在のインスタンス</param>
        /// <returns>復元されるべきであればtrue</returns>
        public Boolean isToRestore(WindowPosition x)
        {
            // プロセスIDが異なっている場合、そのウィンドウハンドルは別物になったとみなし、復元対象外とする
            if (processId != x.processId)
            {
                logger.Debug($"not to restore: processId unmatch: {x.processId}");
                result.Append("対象が見つかりませんでした。");
                return false;
            }
            // プロセス名が異なっている場合、そのウィンドウハンドルは別物になったとみなし、復元対象外とする
            if (processName != x.processName)
            {
                logger.Debug($"not to restore: processName unmatch: {x.processName}");
                result.Append("対象が見つかりませんでした。");
                return false;
            }
            // クラス名が異なっている場合、そのウィンドウハンドルは別物になったとみなし、復元対象外とする
            if (className != x.className)
            {
                logger.Debug($"not to restore: className unmatch: {x.className}");
                result.Append("対象が見つかりませんでした。");
                return false;
            }
            // 位置情報に変動がなければ、復元不要
            if (left == x.left && top == x.top && right == x.right && bottom == x.bottom)
            {
                logger.Debug($"not to restore: window not moved");
                result.Append("");
                return false;
            }
            return true;
        }
        /// <summary>
        /// ウィンドウ位置を復元する
        /// </summary>
        public void restore()
        {
            //const int HWND_TOP = 0;
            //const int HWND_BOTTOM = 1;
            //const int HWND_TOPMOST = -1;
            const int HWND_NOTOPMOST = -2;

            //const int SWP_ASYNCWINDOWPOS = 0x4000;
            //const int SWP_DEFERERASE = 0x2000;
            //const int SWP_DRAWFRAME = 0x0020;
            //const int SWP_FRAMECHANGED = 0x0020;
            //const int SWP_HIDEWINDOW = 0x0080;
            const int SWP_NOACTIVATE = 0x0010;
            //const int SWP_NOCOPYBITS = 0x0100;
            //const int SWP_NOMOVE = 0x0002;
            //const int SWP_NOOWNERZORDER = 0x0200;
            //const int SWP_NOREDRAW = 0x0008;
            //const int SWP_NOREPOSITION = 0x0200;
            //const int SWP_NOSENDCHANGING = 0x0400;
            //const int SWP_NOSIZE = 0x0001;
            const int SWP_NOZORDER = 0x0004;
            //const int SWP_SHOWWINDOW = 0x0040;

            bool ret = SetWindowPos(hWnd, HWND_NOTOPMOST, left, top, right - left, bottom - top, SWP_NOZORDER | SWP_NOACTIVATE);
            if (ret)
            {
                var msg = "復元に成功しました。";
                result.Append(msg);
                logger.Info(msg);
            }
            else
            {
                var win32error = Marshal.GetLastWin32Error();
                var msg = "復元に失敗しました。" + win32error;
                result.Append(msg);
                logger.Error(msg);
            }
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }
}
