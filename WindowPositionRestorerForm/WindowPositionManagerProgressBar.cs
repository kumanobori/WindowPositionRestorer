using System;
using System.Windows.Forms;
using NLog;

namespace WindowPositionRestorerForm
{
    /// <summary>
    /// WindowsPositionManagerを、プログレスバーで進捗状況を把握できるように拡張したクラス。
    /// </summary>
    public class WindowPositionManagerProgressBar : WindowPositionRestorerCommon.WindowPositionManager
    {
        /// <summary>
        /// ロガー
        /// </summary>
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// プログレスバー
        /// </summary>
        public ProgressBar ProgressBar;

        /// <summary>
        /// コンストラクタ
        /// プログレスバーを引数として取得する
        /// </summary>
        /// <param name="progressBar"></param>
        public WindowPositionManagerProgressBar (ProgressBar progressBar)
        {
            this.ProgressBar = progressBar;
        }

        /// <summary>
        /// Saveメソッド
        /// プログレスバー情報をセットしてから、親クラスのSave()を呼び出す
        /// </summary>
        public override void Save()
        {
            base.SaveWindowCount();
            ProgressBar.Minimum = 0;
            ProgressBar.Maximum = enumWindowCount;
            ProgressBar.Value = 0;
            base.Save();
        }

        /// <summary>
        /// Save()の進捗状況を更新する
        /// プログレスバーの値をプラス1する。
        /// </summary>
        protected override void ProgressSave()
        {
            ProgressBar.Value++;
        }


        /// <summary>
        /// Restoreメソッド
        /// プログレスバー情報をセットしてから、親クラスのRestore()を呼び出す
        /// </summary>
        public override void Restore()
        {
            ProgressBar.Minimum = 0;
            ProgressBar.Maximum = savedWindowCount;
            ProgressBar.Value = 0;
            base.Restore();
        }

        /// <summary>
        /// Restore()の進捗状況を更新する
        /// プログレスバーの値をプラス1する。
        /// </summary>
        protected override void ProgressRestore()
        {
            ProgressBar.Value++;
        }

    }
}
