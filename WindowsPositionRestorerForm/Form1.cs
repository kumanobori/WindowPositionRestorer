using System;
using System.Windows.Forms;
using NLog;

namespace WindowsPositionRestorerForm
{
    public partial class Form1 : Form
    {
        WindowPositionManager manager = new WindowPositionManager();

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
        private void buttonSave_Click(object sender, EventArgs e)
        {
            manager.save();
            messageArea.Text = "以下のウィンドウ情報を保存しました\r\n" + manager.fetchResults();
        }

        /// <summary>
        /// restoreボタン押下時処理
        /// ウィンドウの位置とサイズを復元する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRestore_Click(object sender, EventArgs e)
        {
            manager.restore();
            messageArea.Text = "復元結果は以下の通りです\r\n" + manager.fetchResults();
        }
    }
}
