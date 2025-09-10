namespace Replication
{
    public partial class MainForm : Form
    {
        private string rootSourcePath = string.Empty;
        private string rootDestinationPath = string.Empty;

        public MainForm()
        {
            InitializeComponent();
            var location = Properties.Settings.Default.MainFormLocation;
            var size = Properties.Settings.Default.MainFormSize;
            if (location.IsEmpty || size.IsEmpty)
                StartPosition = FormStartPosition.CenterScreen;
            else
            {
                StartPosition = FormStartPosition.Manual;
                Location = location;
                Size = size;
            }
            rootSourcePath = Properties.Settings.Default.RootSourcePath;
            if (string.IsNullOrEmpty(rootSourcePath) )
                rootSourcePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            tslRootSourcePath.Text = rootSourcePath;
            rootDestinationPath = Properties.Settings.Default.RootDestinationPath;
            tslRootDestinationPath.Text = rootDestinationPath;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadTree(tvSource, rootSourcePath);
            LoadTree(tvDestination, rootDestinationPath);
        }

        private void LoadTree(TreeView tv, string rootPath)
        {
            tv.Nodes.Clear();
            if (string.IsNullOrEmpty(rootPath)) return;
            var nRoot = new TreeNode(rootPath) { Tag = rootPath };
            tv.Nodes.Add(nRoot);
            var folders = Directory.GetDirectories(rootPath);
            foreach (var folder in folders) 
            {
                var nfolder = new TreeNode(Path.GetRelativePath(rootPath, folder))
                {
                    Tag = folder,
                    ImageIndex = 0,
                    SelectedImageIndex = 0
                };
                nfolder.Nodes.Add("stub");
                nRoot.Nodes.Add(nfolder);
            }
            var files = Directory.GetFiles(rootPath);
            foreach (var file in files)
            {
                var nfile = new TreeNode(Path.GetRelativePath(rootPath, file))
                {
                    Tag = file,
                    ImageIndex = 1,
                    SelectedImageIndex = 1
                };
                nRoot.Nodes.Add(nfile);
            }
            nRoot.Expand();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.MainFormLocation = Location;
            Properties.Settings.Default.MainFormSize = Size;
            Properties.Settings.Default.Save();
        }
    }
}
