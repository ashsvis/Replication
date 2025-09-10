namespace Replication
{
    partial class RootFolderSelectDialog
    {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tbFolderPath = new TextBox();
            btnSelectLocalFolder = new Button();
            btnOk = new Button();
            SuspendLayout();
            // 
            // tbFolderPath
            // 
            tbFolderPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbFolderPath.Location = new Point(12, 12);
            tbFolderPath.Name = "tbFolderPath";
            tbFolderPath.Size = new Size(398, 23);
            tbFolderPath.TabIndex = 0;
            // 
            // btnSelectLocalFolder
            // 
            btnSelectLocalFolder.Location = new Point(12, 41);
            btnSelectLocalFolder.Name = "btnSelectLocalFolder";
            btnSelectLocalFolder.Size = new Size(179, 25);
            btnSelectLocalFolder.TabIndex = 1;
            btnSelectLocalFolder.Text = "Выбор локальной папки...";
            btnSelectLocalFolder.UseVisualStyleBackColor = true;
            btnSelectLocalFolder.Click += btnSelectLocalFolder_Click;
            // 
            // btnOk
            // 
            btnOk.DialogResult = DialogResult.OK;
            btnOk.Location = new Point(335, 41);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(75, 25);
            btnOk.TabIndex = 2;
            btnOk.Text = "Ввод";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // RootFolderSelectDialog
            // 
            AcceptButton = btnOk;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(422, 76);
            Controls.Add(btnOk);
            Controls.Add(btnSelectLocalFolder);
            Controls.Add(tbFolderPath);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "RootFolderSelectDialog";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Выбор корневой папки";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbFolderPath;
        private Button btnSelectLocalFolder;
        private Button btnOk;
    }
}