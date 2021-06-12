
namespace WindowPositionRestorerTaskIcon
{
    partial class DummyForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DummyForm));
            this.taskTrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // taskTrayIcon
            // 
            this.taskTrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("taskTrayIcon.Icon")));
            this.taskTrayIcon.Visible = true;
            this.taskTrayIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TaskTrayIcon_MouseClick);
            this.taskTrayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TaskTrayIcon_MouseDoubleClick);
            // 
            // DummyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DummyForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon taskTrayIcon;
    }
}

