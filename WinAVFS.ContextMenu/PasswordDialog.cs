using System.Diagnostics;
using System.Windows.Forms;
using WinAVFS.ContextMenu;

namespace WWinAVFS.ContextMenu
{
    class PasswordDialog : Form
    {

        public PasswordDialog(string archivePath)
        {
            this.archivePath = archivePath;
            InitializeComponent();
        }

        private string archivePath;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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


        private void InitializeComponent()
        {
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(15, 15);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(230, 21);
            this.passwordTextBox.TabIndex = 0;
            this.passwordTextBox.WordWrap = false;
            this.passwordTextBox.KeyPress += new KeyPressEventHandler(this.passwordTextBox_KeyPress);
            // 
            // PasswordDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 50);
            this.Controls.Add(this.passwordTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = WinAVFS.ContextMenu.Properties.Resources.WinAVFSIcon;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasswordDialog";
            this.Text = "Password Required - WinAVFS";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private TextBox passwordTextBox;

        private void passwordTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                string password = passwordTextBox.Text;
                string argString = $"\"path={this.archivePath}\" \"password={password}\"";
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    FileName = Program.WinAVFSCLIPath,
                    Arguments = argString,
                };
                Process.Start(startInfo);
                this.Close();
            }

        }
    }
}
