namespace Replication
{
    public partial class RootFolderSelectDialog : Form
    {
        private string path;

        public RootFolderSelectDialog(string path)
        {
            InitializeComponent();
            this.path = path;
            tbFolderPath.Text = path;
        }

        public string SelectedPath { get => path; private set => path = value; }

        private void btnSelectLocalFolder_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog
            {
                RootFolder = Environment.SpecialFolder.MyDocuments,
                SelectedPath = path,
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                tbFolderPath.Text = dlg.SelectedPath;
                path = dlg.SelectedPath;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            path = tbFolderPath.Text;
        }
    }
}
