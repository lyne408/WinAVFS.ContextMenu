using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace WinAVFS.ContextMenu
{
    public class RegisteryDialog : Form
    {
        public RegisteryDialog()
        {
            InitializeComponent();
        }

        private System.ComponentModel.IContainer components = null;


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
            this.registerButton = new System.Windows.Forms.Button();
            this.unregisterButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // registerButton
            // 
            this.registerButton.Location = new System.Drawing.Point(38, 15);
            this.registerButton.Name = "registerButton";
            this.registerButton.Size = new System.Drawing.Size(75, 23);
            this.registerButton.TabIndex = 0;
            this.registerButton.Text = "Register";
            this.registerButton.UseVisualStyleBackColor = true;
            this.registerButton.Click += new System.EventHandler(this.registerButton_Click);
            // 
            // unregisterButton
            // 
            this.unregisterButton.Location = new System.Drawing.Point(139, 15);
            this.unregisterButton.Name = "unregisterButton";
            this.unregisterButton.Size = new System.Drawing.Size(75, 23);
            this.unregisterButton.TabIndex = 1;
            this.unregisterButton.Text = "Unregister";
            this.unregisterButton.UseVisualStyleBackColor = true;
            this.unregisterButton.Click += new System.EventHandler(this.unregisterButton_Click);
            // 
            // RegisteryDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(260, 50);
            this.Controls.Add(this.unregisterButton);
            this.Controls.Add(this.registerButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = WinAVFS.ContextMenu.Properties.Resources.WinAVFSIcon;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RegisteryDialog";
            this.Text = "Register Context Menu - WinAVFS";
            this.ResumeLayout(false);

        }


        private System.Windows.Forms.Button registerButton;
        private System.Windows.Forms.Button unregisterButton;

        private void registerButton_Click(object sender, EventArgs e)
        {

            WinAVFS.Utils.ShellItemRegistry shellItem = new WinAVFS.Utils.ShellItemRegistry();
            shellItem.ItemName = "Mount_by_WinAVFS";
            shellItem.MenuText = "Mount by WinAVFS";
            string currentProgramPath = Process.GetCurrentProcess().MainModule.FileName;
            shellItem.IconPath = currentProgramPath;
            shellItem.ExecutableFilePath = currentProgramPath;
            shellItem.RegisterAllFiles();
            MessageBox.Show("Registered.");
            this.Close();
            

        }

        private void unregisterButton_Click(object sender, EventArgs e)
        {
            WinAVFS.Utils.ShellItemRegistry shellItem = new WinAVFS.Utils.ShellItemRegistry();
            shellItem.ItemName = "Mount_by_WinAVFS";
            shellItem.UnregisterAllFiles();
            MessageBox.Show("Unregistered.");
            this.Close();
            
        }
    }
}
