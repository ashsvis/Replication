using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

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
            if (string.IsNullOrEmpty(rootSourcePath))
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
            Cursor = Cursors.WaitCursor;
            try
            {
                try
                {
                    var folders = Directory.GetDirectories(rootPath);
                    var nRoot = new TreeNode(rootPath) { Tag = rootPath };
                    tv.Nodes.Add(nRoot);
                    FillNodes(nRoot, rootPath);
                    nRoot.Expand();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка загрузки файлового пути", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            @break = true;
            Properties.Settings.Default.MainFormLocation = Location;
            Properties.Settings.Default.MainFormSize = Size;
            Properties.Settings.Default.Save();
        }

        private void tsbDefineRootSourcePath_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog
            {
                RootFolder = Environment.SpecialFolder.MyDocuments,
                SelectedPath = rootSourcePath
            };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (rootSourcePath != dlg.SelectedPath)
                {
                    rootSourcePath = dlg.SelectedPath;
                    tslRootSourcePath.Text = rootSourcePath;
                    LoadTree(tvSource, rootSourcePath);
                    Properties.Settings.Default.RootSourcePath = rootSourcePath;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void tsbDefineRootDestinationPath_Click(object sender, EventArgs e)
        {
            var dlg = new RootFolderSelectDialog(rootDestinationPath);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (rootDestinationPath != dlg.SelectedPath)
                {
                    rootDestinationPath = dlg.SelectedPath;
                    tslRootDestinationPath.Text = rootDestinationPath;
                    LoadTree(tvDestination, rootDestinationPath);
                    Properties.Settings.Default.RootDestinationPath = rootDestinationPath;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void tvTree_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node == null) return;
            e.Node.Nodes.Clear();
            if (e.Node.Tag is string path)
            {
                Cursor = Cursors.WaitCursor;
                try
                {
                    try
                    {
                        FillNodes(e.Node, path);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка загрузки файлового пути", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private static void FillNodes(TreeNode node, string path)
        {
            var folders = Directory.GetDirectories(path);
            foreach (var folder in folders)
            {
                var nfolder = new TreeNode(Path.GetRelativePath(path, folder))
                {
                    Tag = folder,
                    ImageIndex = 0,
                    SelectedImageIndex = 0
                };
                nfolder.Nodes.Add("stub");
                node.Nodes.Add(nfolder);
            }
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                var nfile = new TreeNode(Path.GetRelativePath(path, file))
                {
                    Tag = file,
                    ImageIndex = 1,
                    SelectedImageIndex = 1
                };
                node.Nodes.Add(nfile);
            }
        }

        private void tvTree_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null) return;
            e.Node.Nodes.Clear();
            e.Node.Nodes.Add("stub");
        }

        private void tsbDoReplicate_Click(object sender, EventArgs e)
        {
            try
            {
                @break = false;
                tsbBreak.Visible = true;
                tsbDoReplicate.Enabled = false;
                tsbDefineRootSourcePath.Enabled = false;
                tsbDefineRootDestinationPath.Enabled = false;
                toolStrip1.Refresh();
                DoReplicate(rootSourcePath, rootDestinationPath, UpdateStatus);
                LoadTree(tvSource, rootSourcePath);
                LoadTree(tvDestination, rootDestinationPath);
            }
            finally
            {
                tsbBreak.Visible = false;
                tsbDoReplicate.Enabled = true;
                tsbDefineRootSourcePath.Enabled = true;
                tsbDefineRootDestinationPath.Enabled = true;
            }
        }

        private void UpdateStatus(string action, string source, string destination)
        {
            toolStripStatusLabel1.Text = action + source;
            statusStrip1.Refresh();
            toolStripStatusLabel2.Text = destination;
            statusStrip2.Refresh();
        }

        private static bool @break = false;

        public static void DoReplicate(string source, string destination, UpdateStatus? method = null)
        {
            try
            {
                var sourceFolders = Directory.GetDirectories(source);
                var files = Directory.GetFiles(source);
                foreach (var sourcefile in files.Select(x => new FileInfo(x)))
                {
                    if (@break) break;
                    var destfile = new FileInfo(Path.Combine(destination, Path.GetRelativePath(source, sourcefile.FullName)));
                    if (!destfile.Exists)
                    {
                        // копирование файла, которого не было в назначении
                        method?.Invoke("Копирую новый: ",
                            Path.GetRelativePath(Path.GetDirectoryName(source) ?? "", sourcefile.FullName),
                            Path.GetRelativePath(Path.GetDirectoryName(destination) ?? "", destfile.FullName));
                        if (method != null) Application.DoEvents();
                        sourcefile.CopyTo(destfile.FullName);
                    }
                    else if (sourcefile.LastWriteTime > destfile.LastWriteTime)
                    {
                        // перезапись файла, который устарел в назначении
                        method?.Invoke("Обновляю: ",
                            Path.GetRelativePath(Path.GetDirectoryName(source) ?? "", sourcefile.FullName),
                            Path.GetRelativePath(Path.GetDirectoryName(destination) ?? "", destfile.FullName));
                        if (method != null) Application.DoEvents();
                        sourcefile.CopyTo(destfile.FullName, true);
                    }
                }
                foreach (var folder in sourceFolders.Select(x => new DirectoryInfo(x)))
                {
                    if (@break) break;
                    var dirpath = Path.Combine(destination, Path.GetRelativePath(source, folder.FullName));
                    if (!Directory.Exists(dirpath))
                    {
                        // создание папки, которой не было в назначении
                        method?.Invoke("Создаю папку: ",
                            Path.GetRelativePath(Path.GetDirectoryName(source) ?? "", folder.FullName),
                            Path.GetRelativePath(Path.GetDirectoryName(destination) ?? "", dirpath));
                        if (method != null) Application.DoEvents();
                        Directory.CreateDirectory(dirpath);
                    }
                    DoReplicate(folder.FullName, dirpath, method);
                }
                method?.Invoke("Готово.", "", "");
                /*
                var dectinationFolders = Directory.GetDirectories(destination);
                foreach (var folder in dectinationFolders.Select(x => new DirectoryInfo(x)))
                {
                    var dirpath = Path.Combine(source, Path.GetRelativePath(destination, folder.FullName));
                    if (!Directory.Exists(dirpath))
                    {
                        // удаление папки, которой не было в источнике
                        folder.Delete();
                    }
                    var files = Directory.GetFiles(destination);
                    foreach (var file in files.Select(x => new FileInfo(x))) 
                    {
                        var filepath = Path.Combine(source, Path.GetRelativePath(destination, file.FullName));
                        if (!File.Exists(filepath))
                        {
                            // удаление файла, которого не было в источнике
                            file.Delete();
                        }
                    }
                }
                */
            }
            catch { }
        }

        private void tsbBreak_Click(object sender, EventArgs e)
        {
            @break = true;
            tsbBreak.Visible = false;
        }
    }

    public delegate void UpdateStatus(string action, string source, string destination);
}
