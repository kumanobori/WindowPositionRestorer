using System;
using System.Windows.Forms;
using NLog;

namespace WindowPositionRestorerForm
{
    public partial class MainForm : Form
    {
        private readonly WindowPositionManagerProgressBar manager;

        public MainForm()
        {
            InitializeComponent();
            manager = new WindowPositionManagerProgressBar(progressBar1);
        }

        /// <summary>
        /// saveボタン押下時処理
        /// ウィンドウ情報を保存する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSave_Click(object sender, EventArgs e)
        {
            // 結果表示エリアをクリア
            messageArea.Text = "";
            this.Update();

            manager.SaveWindowCount();
            manager.Save();

            messageArea.Text = $"[{manager.saved.ToString("MM/dd HH:mm")}] {manager.savedWindowCount} 件を復元候補として保存しました\r\n" + manager.FetchResults();
        }

        /// <summary>
        /// restoreボタン押下時処理
        /// ウィンドウの位置とサイズを復元する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonRestore_Click(object sender, EventArgs e)
        {
            manager.Restore();
            messageArea.Text = $"[{manager.saved.ToString("MM/dd HH:mm")}] に保存した{manager.savedWindowCount}件を候補として、復元を試みました。プロセス情報の末尾に結果を追記します。ただし変化のなかったウィンドウについては何も追記しません。\r\n" + manager.FetchResults();
        }
    }
}
