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
            backgroundWorker1.CancelAsync();
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

        private static long countTotal = 0;
        private static long foldersTotal = 0;
        private static long filesTotal = 0;
        private static long countToCopy = 0;
        private static long countProgress = 0;
        private static long foldersToCopy = 0;
        private static long filesToCopy = 0;

        private void tsbDoReplicate_Click(object sender, EventArgs e)
        {
            tsbBreak.Enabled = true;
            tsbDoReplicate.Enabled = false;
            tsbDefineRootSourcePath.Enabled = false;
            tsbDefineRootDestinationPath.Enabled = false;
            toolStripProgressBar2.Value = 0;
            toolStripProgressBar2.Visible = true;
            toolStrip1.Refresh();
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            countTotal = 0;
            foldersTotal = 0;
            filesTotal = 0;
            countToCopy = 0;
            countProgress = 0;
            foldersToCopy = 0;
            filesToCopy = 0;
            DoCalculate(rootSourcePath, rootDestinationPath, (BackgroundWorker)sender);
            var worker = (BackgroundWorker)sender;
            worker?.ReportProgress(0, ("Готово. ", 
                $"Всего {countTotal / 1000000} МБайт (папок: {foldersTotal}, файлов: {filesTotal}) в источнике", 
                $"Будет добавлено (обновлено) {countToCopy / 1000000} МБайт (папок: {foldersToCopy}, файлов: {filesToCopy}) в назначении"));
            DoReplicate(rootSourcePath, rootDestinationPath, (BackgroundWorker)sender);
            worker?.ReportProgress((int)(countProgress * 100.0 / countToCopy), ("Готово. ",
                $"Всего {countTotal / 1000000} МБайт (папок: {foldersTotal}, файлов: {filesTotal}) в источнике",
                $"Добавлено (обновлено) {countToCopy / 1000000} МБайт (папок: {foldersToCopy}, файлов: {filesToCopy}) в назначении"));
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
            {
                (string action, string source, string destination) = ((string, string, string))e.UserState;
                UpdateStatus(e.ProgressPercentage, action, source, destination);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            LoadTree(tvSource, rootSourcePath);
            LoadTree(tvDestination, rootDestinationPath);
            toolStripProgressBar2.Visible = false;
            tsbBreak.Enabled = false;
            tsbDoReplicate.Enabled = true;
            tsbDefineRootSourcePath.Enabled = true;
            tsbDefineRootDestinationPath.Enabled = true;
        }

        private void UpdateStatus(int progressPercentage, string action, string source, string destination)
        {
            toolStripStatusLabel1.Text = action + source;
            statusStrip1.Refresh();
            toolStripStatusLabel2.Text = destination;
            if (progressPercentage >= 0)
                toolStripProgressBar2.Value = progressPercentage;
            statusStrip2.Refresh();
        }

        public static void DoCalculate(string source, string destination, BackgroundWorker? worker = null)
        {
            try
            {
                var sourceFolders = Directory.GetDirectories(source);
                var files = Directory.GetFiles(source);
                foreach (var sourcefile in files.Select(x => new FileInfo(x)))
                {
                    if (worker != null && worker.CancellationPending) break;
                    filesTotal++;
                    countTotal += sourcefile.Length;
                    var destfile = new FileInfo(Path.Combine(destination, Path.GetRelativePath(source, sourcefile.FullName)));
                    if (!destfile.Exists)
                    {
                        // копирование файла, которого не было в назначении
                        filesToCopy++;
                        countToCopy += sourcefile.Length;
                        worker?.ReportProgress(0, ("Будет добавлен: ",
                            Path.GetRelativePath(Path.GetDirectoryName(destination) ?? "", destfile.FullName), ""));
                    }
                    else if (sourcefile.LastWriteTime > destfile.LastWriteTime)
                    {
                        // перезапись файла, который устарел в назначении
                        filesToCopy++;
                        countToCopy += sourcefile.Length;
                        worker?.ReportProgress(0, ("Будет обновлён: ",
                            Path.GetRelativePath(Path.GetDirectoryName(destination) ?? "", destfile.FullName), ""));
                    }
                }
                foreach (var folder in sourceFolders.Select(x => new DirectoryInfo(x)))
                {
                    if (worker != null && worker.CancellationPending) break;
                    foldersTotal++;
                    var dirpath = Path.Combine(destination, Path.GetRelativePath(source, folder.FullName));
                    if (!Directory.Exists(dirpath))
                    {
                        foldersToCopy++;
                        // создание папки, которой не было в назначении
                        worker?.ReportProgress(0, ("Будет создана папка: ",
                            Path.GetRelativePath(Path.GetDirectoryName(destination) ?? "", dirpath), ""));
                    }
                    DoCalculate(folder.FullName, dirpath, worker);
                }
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

        public static void DoReplicate(string source, string destination, BackgroundWorker? worker = null)
        {
            try
            {
                var sourceFolders = Directory.GetDirectories(source);
                var files = Directory.GetFiles(source);
                foreach (var sourcefile in files.Select(x => new FileInfo(x)))
                {
                    if (worker != null && worker.CancellationPending) break;
                    var destfile = new FileInfo(Path.Combine(destination, Path.GetRelativePath(source, sourcefile.FullName)));
                    if (!destfile.Exists)
                    {
                        countProgress += sourcefile.Length;
                        // копирование файла, которого не было в назначении
                        worker?.ReportProgress((int)(countProgress * 100.0 / countToCopy), ("Копирую новый: ",
                            Path.GetRelativePath(Path.GetDirectoryName(source) ?? "", sourcefile.FullName),
                            Path.GetRelativePath(Path.GetDirectoryName(destination) ?? "", destfile.FullName)));
                        sourcefile.CopyTo(destfile.FullName);
                    }
                    else if (sourcefile.LastWriteTime > destfile.LastWriteTime)
                    {
                        countProgress += sourcefile.Length;
                        // перезапись файла, который устарел в назначении
                        worker?.ReportProgress((int)(countProgress * 100.0 / countToCopy), ("Обновляю: ",
                            Path.GetRelativePath(Path.GetDirectoryName(source) ?? "", sourcefile.FullName),
                            Path.GetRelativePath(Path.GetDirectoryName(destination) ?? "", destfile.FullName)));
                        sourcefile.CopyTo(destfile.FullName, true);
                    }
                }
                foreach (var folder in sourceFolders.Select(x => new DirectoryInfo(x)))
                {
                    if (worker != null && worker.CancellationPending) break;
                    var dirpath = Path.Combine(destination, Path.GetRelativePath(source, folder.FullName));
                    if (!Directory.Exists(dirpath))
                    {
                        // создание папки, которой не было в назначении
                        worker?.ReportProgress((int)(countProgress * 100.0 / countToCopy), ("Создаю папку: ",
                            Path.GetRelativePath(Path.GetDirectoryName(source) ?? "", folder.FullName),
                            Path.GetRelativePath(Path.GetDirectoryName(destination) ?? "", dirpath)));
                        Directory.CreateDirectory(dirpath);
                    }
                    DoReplicate(folder.FullName, dirpath, worker);
                }
                worker?.ReportProgress((int)(countProgress * 100.0 / countToCopy), ("Готово.", "", ""));
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
            backgroundWorker1.CancelAsync();
            tsbBreak.Enabled = false;
        }
    }

    public delegate void UpdateStatus(string action, string source, string destination);
}
