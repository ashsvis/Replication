namespace Replication
{
    partial class MainForm
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            splitContainer1 = new SplitContainer();
            tvSource = new TreeView();
            imageList1 = new ImageList(components);
            statusStrip1 = new StatusStrip();
            toolStrip1 = new ToolStrip();
            toolStripLabel1 = new ToolStripLabel();
            tsbDefineRootSourcePath = new ToolStripButton();
            tslRootSourcePath = new ToolStripLabel();
            tvDestination = new TreeView();
            statusStrip2 = new StatusStrip();
            toolStrip2 = new ToolStrip();
            toolStripLabel2 = new ToolStripLabel();
            tsbDefineRootDestinationPath = new ToolStripButton();
            tslRootDestinationPath = new ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            toolStrip1.SuspendLayout();
            toolStrip2.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(tvSource);
            splitContainer1.Panel1.Controls.Add(statusStrip1);
            splitContainer1.Panel1.Controls.Add(toolStrip1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(tvDestination);
            splitContainer1.Panel2.Controls.Add(statusStrip2);
            splitContainer1.Panel2.Controls.Add(toolStrip2);
            splitContainer1.Size = new Size(994, 547);
            splitContainer1.SplitterDistance = 497;
            splitContainer1.TabIndex = 0;
            // 
            // tvSource
            // 
            tvSource.BorderStyle = BorderStyle.None;
            tvSource.Dock = DockStyle.Fill;
            tvSource.ImageIndex = 0;
            tvSource.ImageList = imageList1;
            tvSource.Location = new Point(0, 25);
            tvSource.Name = "tvSource";
            tvSource.SelectedImageIndex = 0;
            tvSource.Size = new Size(497, 500);
            tvSource.TabIndex = 2;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageStream = (ImageListStreamer)resources.GetObject("imageList1.ImageStream");
            imageList1.TransparentColor = Color.Transparent;
            imageList1.Images.SetKeyName(0, "open.png");
            imageList1.Images.SetKeyName(1, "newpage.png");
            // 
            // statusStrip1
            // 
            statusStrip1.Location = new Point(0, 525);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(497, 22);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripLabel1, tsbDefineRootSourcePath, tslRootSourcePath });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(497, 25);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            toolStripLabel1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            toolStripLabel1.Name = "toolStripLabel1";
            toolStripLabel1.Size = new Size(66, 22);
            toolStripLabel1.Text = "Источник:";
            // 
            // tsbDefineRootSourcePath
            // 
            tsbDefineRootSourcePath.Alignment = ToolStripItemAlignment.Right;
            tsbDefineRootSourcePath.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbDefineRootSourcePath.Image = (Image)resources.GetObject("tsbDefineRootSourcePath.Image");
            tsbDefineRootSourcePath.ImageTransparentColor = Color.Magenta;
            tsbDefineRootSourcePath.Name = "tsbDefineRootSourcePath";
            tsbDefineRootSourcePath.Size = new Size(23, 22);
            tsbDefineRootSourcePath.Text = "Определить корневую папку источника";
            tsbDefineRootSourcePath.Click += tsbDefineRootSourcePath_Click;
            // 
            // tslRootSourcePath
            // 
            tslRootSourcePath.Name = "tslRootSourcePath";
            tslRootSourcePath.Size = new Size(17, 22);
            tslRootSourcePath.Text = "\\\\";
            // 
            // tvDestination
            // 
            tvDestination.BorderStyle = BorderStyle.None;
            tvDestination.Dock = DockStyle.Fill;
            tvDestination.ImageIndex = 0;
            tvDestination.ImageList = imageList1;
            tvDestination.Location = new Point(0, 25);
            tvDestination.Name = "tvDestination";
            tvDestination.SelectedImageIndex = 0;
            tvDestination.Size = new Size(493, 500);
            tvDestination.TabIndex = 2;
            // 
            // statusStrip2
            // 
            statusStrip2.Location = new Point(0, 525);
            statusStrip2.Name = "statusStrip2";
            statusStrip2.Size = new Size(493, 22);
            statusStrip2.TabIndex = 1;
            statusStrip2.Text = "statusStrip2";
            // 
            // toolStrip2
            // 
            toolStrip2.Items.AddRange(new ToolStripItem[] { toolStripLabel2, tsbDefineRootDestinationPath, tslRootDestinationPath });
            toolStrip2.Location = new Point(0, 0);
            toolStrip2.Name = "toolStrip2";
            toolStrip2.Size = new Size(493, 25);
            toolStrip2.TabIndex = 0;
            toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel2
            // 
            toolStripLabel2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            toolStripLabel2.Name = "toolStripLabel2";
            toolStripLabel2.Size = new Size(80, 22);
            toolStripLabel2.Text = "Назначение:";
            // 
            // tsbDefineRootDestinationPath
            // 
            tsbDefineRootDestinationPath.Alignment = ToolStripItemAlignment.Right;
            tsbDefineRootDestinationPath.DisplayStyle = ToolStripItemDisplayStyle.Image;
            tsbDefineRootDestinationPath.Image = (Image)resources.GetObject("tsbDefineRootDestinationPath.Image");
            tsbDefineRootDestinationPath.ImageTransparentColor = Color.Magenta;
            tsbDefineRootDestinationPath.Name = "tsbDefineRootDestinationPath";
            tsbDefineRootDestinationPath.Size = new Size(23, 22);
            tsbDefineRootDestinationPath.Text = "Определить корневую папку назначения";
            tsbDefineRootDestinationPath.Click += tsbDefineRootDestinationPath_Click;
            // 
            // tslRootDestinationPath
            // 
            tslRootDestinationPath.Name = "tslRootDestinationPath";
            tslRootDestinationPath.Size = new Size(17, 22);
            tslRootDestinationPath.Text = "\\\\";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(994, 547);
            Controls.Add(splitContainer1);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Репликация файлов и папок";
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            toolStrip2.ResumeLayout(false);
            toolStrip2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private TreeView tvSource;
        private StatusStrip statusStrip1;
        private ToolStrip toolStrip1;
        private TreeView tvDestination;
        private StatusStrip statusStrip2;
        private ToolStrip toolStrip2;
        private ToolStripButton tsbDefineRootSourcePath;
        private ToolStripButton tsbDefineRootDestinationPath;
        private ToolStripLabel tslRootSourcePath;
        private ToolStripLabel tslRootDestinationPath;
        private ImageList imageList1;
        private ToolStripLabel toolStripLabel1;
        private ToolStripLabel toolStripLabel2;
    }
}
