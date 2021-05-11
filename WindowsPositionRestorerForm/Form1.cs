using System;
using System.Windows.Forms;
using NLog;

namespace WindowsPositionRestorerForm
{
    public partial class Form1 : Form
    {
        private readonly WindowPositionManager manager = new();

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// saveボタン押下時処理
        /// ウィンドウ情報を保存する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSave_Click(object sender, EventArgs e)
        {
            int windowCount = manager.FetchWindowCount();
            progressBar1.Maximum = windowCount;
            progressBar1.Minimum = 0;
            progressBar1.Value = 0;
            messageArea.Text = "";
            this.Update();

            manager.Save();
            messageArea.Text = "以下のウィンドウ情報を保存しました\r\n" + manager.FetchResults();
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
            messageArea.Text = "復元結果は以下の通りです\r\n" + manager.FetchResults();
        }
    }
}
