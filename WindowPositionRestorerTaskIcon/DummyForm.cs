using System;
using System.Windows.Forms;
using WindowPositionRestorerForm;

namespace WindowPositionRestorerTaskIcon
{
    /// <summary>
    /// タスクトレイアイコンアプリとして動作させるための、表示させないフォーム。
    /// </summary>
    public partial class DummyForm : Form
    {
        /// <summary>
        /// ウィンドウ位置復元管理クラス
        /// </summary>
        WindowPositionManagerProgressBar manager;

        /// <summary>
        /// ツール名
        /// </summary>
        private const string TOOL_NAME = "ウィンドウ位置復元ツール";
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DummyForm()
        {
            InitializeComponent();

            // タスクトレイアイコンの設定
            this.taskTrayIcon.Text = TOOL_NAME;
            ToolStripMenuItem menu1 = new();
            taskTrayIcon.ContextMenuStrip = new ContextMenuStrip();
            taskTrayIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("save", null, this.Save));
            taskTrayIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("restore", null, this.Restore));
            taskTrayIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("exit", null, this.Exit));
        }

        /// <summary>
        /// メニューでsaveを押したときの動作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save(object sender, EventArgs e)
        {
            // 進捗表示用フォームを表示
            ProgressForm form = new();
            form.Show();

            // 進捗表示用フォームを復元管理クラスにセット
            manager = new WindowPositionManagerProgressBar(form.progressBar);

            // 保存実行
            manager.Save();
            MessageBox.Show($"{manager.savedWindowCount}件を復元候補として保存しました。");
            
            // フォームを閉じ、タスクトレイアイコンの表示を更新
            form.Close();
            this.taskTrayIcon.Text = $"{TOOL_NAME}：{DateTime.Now.ToString("MM/dd HH:mm")} に {manager.savedWindowCount}件の候補を保存しました。";
        }

        /// <summary>
        /// メニューでrestoreを押したときの動作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Restore(object sender, EventArgs e)
        {
            // 進捗表示用フォームを表示
            ProgressForm form = new();
            form.Show();

            // 進捗表示用フォームを復元管理クラスにセット
            manager.ProgressBar = form.progressBar;
            
            // 復元実行
            manager.Restore();
            MessageBox.Show("復元しました。");
            
            // フォームを閉じる
            form.Close();
        }

        /// <summary>
        /// メニューでexitを押したときの動作。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("終了しますか？", "", MessageBoxButtons.OKCancel);
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                this.taskTrayIcon.Dispose();
                Application.Exit();
            }
        }

        /// <summary>
        /// アイコンクリック時の動作。
        /// メニューを表示する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskTrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            taskTrayIcon.ContextMenuStrip.Show();
        }

        /// <summary>
        /// アイコンダブルクリック時の動作。
        /// メニューを表示する。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskTrayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            taskTrayIcon.ContextMenuStrip.Show();
        }
    }
}
