using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        private readonly string fileLink = "links.txt";
        // 👉 qui dichiariamo la cartella di download
        // Variabile globale con cartella predefinita
       // private string downloadFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
        private string downloadFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

        private readonly List<Process> downloadProcesses = new List<Process>();
        public Form1()
        {
            InitializeComponent();
            txtArgs.Text = "-f bestvideo[ext=mp4][vcodec^=avc]+bestaudio[ext=m4a]/bestvideo[ext=mp4]+bestaudio[ext=m4a]/bestvideo+bestaudio/best[ext=mp4]/best";
            // Eventi
            txtUrl.KeyDown += DMTextBox_KeyDown;
            // DMListBox.SelectedIndexChanged += DMListBox_SelectedIndexChanged;
            // DMCopyButton.Click += DMCopyButton_Click;
            DMListBox.SelectedIndexChanged += DMListBox_SelectedIndexChanged;
            //menu
            // Nel costruttore del form, dopo InitializeComponent()
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem eliminaItem = new ToolStripMenuItem("Elimina link selezionati");
            eliminaItem.Click += EliminaLinkSelezionati_Click;
            menu.Items.Add(eliminaItem);
            // Voce copia link
            ToolStripMenuItem copiaItem = new ToolStripMenuItem("Copia link selezionati");
            copiaItem.Click += CopiaLinkSelezionati_Click;
            menu.Items.Add(copiaItem);
            // Associa il menu alla ListBox
            DMListBox.ContextMenuStrip = menu;
            DMListBox.SelectionMode = SelectionMode.MultiExtended;
            this.FormClosing += Form1_FormClosing; // <- associa l'evento
            // 🔹 ComboBox Arguments
            cmbFormato.Items.Add("Video MP4");
            cmbFormato.Items.Add("Solo Video");
            cmbFormato.Items.Add("Audio MP3");
            cmbFormato.Items.Add("Audio M4A");
            cmbFormato.SelectedIndex = 0; // default Video MP4
            // 🔹 ComboBox Arguments fine
        }
        private void SalvaLink()
        {
            var links = DMListBox.Items.Cast<string>().ToList();
            File.WriteAllLines(fileLink, links);
        }
        private void CopiaLinkSelezionati_Click(object sender, EventArgs e)
        {
            if (DMListBox.SelectedItems.Count > 0)
            {
                // Unisci tutti i link selezionati separati da a capo
                string testo = string.Join(Environment.NewLine, DMListBox.SelectedItems.Cast<object>());
                Clipboard.SetText(testo);
                MessageBox.Show("Link copiati negli appunti!");
            }
            else
            {
                MessageBox.Show("Seleziona almeno un link da copiare.");
            }
        }
        private void EliminaLinkSelezionati_Click(object sender, EventArgs e)
        {
            var itemsToRemove = DMListBox.SelectedItems.Cast<object>().ToList();
            foreach (var item in itemsToRemove)
            {
                DMListBox.Items.Remove(item);
            }
            SalvaLink(); // salva su file
        }
        private void DMTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                AddValidLinksFromTextBox();
            }
        }


        private void AddValidLinksFromTextBox()
        {
            var lines = txtUrl.Lines
                        .Where(l => !string.IsNullOrWhiteSpace(l))
                        .Select(l => l.Trim());

            foreach (var line in lines)
            {
                if (IsValidLink(line) && !DMListBox.Items.Contains(line))
                {
                    DMListBox.Items.Add(line);
                }
            }

            txtUrl.Clear();
            SalvaLink(); // salva su file
        }

        private bool IsValidLink(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) &&
                   (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
        private void DMListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            BtnApriLink.Enabled = DMListBox.SelectedItem != null;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DMManagerGUI.Properties.Settings.Default.UserArgs = txtArgs.Text;
            DMManagerGUI.Properties.Settings.Default.Save();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Ripristina il testo che l’utente aveva scritto
            
            // Recupera la cartella salvata o imposta quella di default
            downloadFolder = string.IsNullOrEmpty(DMManagerGUI.Properties.Settings.Default.DownloadFolder)
                ? Environment.GetFolderPath(Environment.SpecialFolder.MyVideos)
                : DMManagerGUI.Properties.Settings.Default.DownloadFolder;
            txtArgs.Text = DMManagerGUI.Properties.Settings.Default.UserArgs ?? "";
            // Aggiorna label
            lblDownloadPath.Text = "Cartella attuale: " + downloadFolder;
            if (File.Exists(fileLink))
            {
                var lines = File.ReadAllLines(fileLink);
                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                        DMListBox.Items.Add(line.Trim());
                }
            }
        }

        private void BtnApriLink_Click(object sender, EventArgs e)
        {
            // Se non è selezionato niente, seleziona il primo elemento
            if (DMListBox.SelectedIndex == -1 && DMListBox.Items.Count > 0)
                DMListBox.SelectedIndex = 0;

            if (DMListBox.SelectedItem != null)
            {
                string url = DMListBox.SelectedItem.ToString();

                // Apro il form secondario in modalità modale
                var lista = new VideoListForm(url)
                {
                    TopMost = true // opzionale: sempre in primo piano
                };
                lista.ShowDialog();
            }
            else
            {
                MessageBox.Show("Seleziona un link dalla lista prima di aprirlo.");
            }
        }

        private void DMCopyButton_Click(object sender, EventArgs e)
        {
            AddValidLinksFromTextBox();
        }
        // ------------------- Download Async -------------------
        private async Task ScaricaLinkAsync(string url, Label lblProgress, bool isPlaylist, string additionalArgs = "")
        {
            // Mostra subito la scritta iniziale
            lblProgress.Invoke((Action)(() => lblProgress.Text = "In preparazione..."));

            string fileOutput = Path.Combine(downloadFolder, "%(title)s.%(ext)s");
            // Cartella temporanea di Windows per i .part
            //string tempFolder = Path.GetTempPath();

            // Base args
            //string args = $"\"{url}\" -o \"{fileOutput}\" -P \"temp:{tempFolder}\"";
            string args = $"\"{url}\" -o \"{fileOutput}\" --no-part --no-overwrites --continue";

            if (isPlaylist)
                args += " --yes-playlist";
            else
                args += " --no-playlist";

            if (!string.IsNullOrWhiteSpace(additionalArgs))
                args += " " + additionalArgs;

            string ytDlpPath = Path.Combine(Application.StartupPath, "yt-dlp.exe");

            await Task.Run(() =>
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = ytDlpPath,
                        Arguments = args,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    }
                };

                downloadProcesses.Add(process);

                Regex regexProgress = new Regex(@"\[download\]\s+\d+(\.\d+)?% of\s+.* at\s+.* ETA \d{2}:\d{2}");

                process.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        var match = regexProgress.Match(e.Data);
                        if (match.Success)
                        {
                            lblProgress.Invoke((Action)(() =>
                            {
                                lblProgress.Text = match.Value; // mostra solo il progresso filtrato
                            }));
                        }
                    }
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        var match = regexProgress.Match(e.Data);
                        if (match.Success)
                        {
                            lblProgress.Invoke((Action)(() =>
                            {
                                lblProgress.Text = match.Value;
                            }));
                        }
                    }
                };

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();

                downloadProcesses.Remove(process);

                lblProgress.Invoke((Action)(() =>
                {
                    lblProgress.Text = "Download completato";
                }));
            });
        }

        // ------------------- Bottoni -------------------
        private async void BtnDownloadSelezionati_Click(object sender, EventArgs e)
        {
            var links = DMListBox.SelectedItems.Cast<string>().ToList();
            if (links.Count == 0)
            {
                MessageBox.Show("Seleziona almeno un link da scaricare.");
                return;
            }

            bool isPlaylist = chkPlaylist.Checked;
            string additionalArgs = txtArgs.Text;

            foreach (var link in links)
            {
                await ScaricaLinkAsync(link, lblProgress, isPlaylist, additionalArgs);
            }
        }



        private async void BtnDownloadTutti_Click(object sender, EventArgs e)
        {
            var links = DMListBox.Items.Cast<string>().Take(10).ToList();
            if (links.Count == 0)
            {
                MessageBox.Show("Non ci sono link da scaricare.");
                return;
            }

            bool isPlaylist = chkPlaylist.Checked;
            string additionalArgs = txtArgs.Text;

            foreach (var link in links)
            {
                await ScaricaLinkAsync(link, lblProgress, isPlaylist, additionalArgs);
            }
        }

        private void KillProcessAndChildren(int pid)
        {
            using (var searcher = new ManagementObjectSearcher(
                $"Select * From Win32_Process Where ParentProcessID={pid}"))
            {
                foreach (var mo in searcher.Get())
                {
                    int childPid = Convert.ToInt32(mo["ProcessID"]);
                    KillProcessAndChildren(childPid);
                }
            }

            try
            {
                var proc = Process.GetProcessById(pid);
                proc.Kill();
            }
            catch { }
        }
        private void BtnKill_Click(object sender, EventArgs e)
        {
            foreach (var p in downloadProcesses.ToList())
            {
                try
                {
                    if (!p.HasExited)
                    {
                        // Termina processo principale e figli usando taskkill
                        Process.Start("taskkill", $"/PID {p.Id} /T /F");
                    }
                }
                catch { }
            }

            downloadProcesses.Clear();
            lblProgress.Text = "Download interrotto";
        }

private void btnSelezionaCartella_Click(object sender, EventArgs e)
        {
            using (var dlg = new CommonOpenFileDialog())
            {
                dlg.IsFolderPicker = true;
                dlg.Title = "Seleziona la cartella di destinazione";
                dlg.InitialDirectory = string.IsNullOrEmpty(downloadFolder)
                    ? Environment.GetFolderPath(Environment.SpecialFolder.MyVideos)
                    : downloadFolder;

                if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    downloadFolder = dlg.FileName;

                    // Aggiorna label
                    lblDownloadPath.Text = "Folder download: " + downloadFolder;

                    // Salva nelle impostazioni
                    DMManagerGUI.Properties.Settings.Default.DownloadFolder = downloadFolder;
                    DMManagerGUI.Properties.Settings.Default.Save();
                }
            }
        }

        private void btnApriCartella_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(downloadFolder) && Directory.Exists(downloadFolder))
            {
                Process.Start("explorer.exe", downloadFolder);
            }
            else
            {
                MessageBox.Show("⚠️ Nessuna cartella valida selezionata.",
                                "Errore", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            txtArgs.Text = "-f bestvideo[ext=mp4][vcodec^=avc]+bestaudio[ext=m4a]/bestvideo[ext=mp4]+bestaudio[ext=m4a]/bestvideo+bestaudio/best[ext=mp4]/best";
        }

        private void btnListClear_Click(object sender, EventArgs e)
        {
            if (DMListBox.Items.Count == 0)
                return;

            var result = MessageBox.Show("Sei sicuro di voler svuotare tutta la lista?", "Conferma", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                DMListBox.Items.Clear();
                lblProgress.Text = "Lista link svuotata";
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("https://github.com/yt-dlp/yt-dlp");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore: " + ex.Message);
            }
        }

            private void cmbFormato_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbFormato.SelectedItem.ToString())
            {
                case "Video MP4":
                    txtArgs.Text = "-f bestvideo[ext=mp4][vcodec^=avc]+bestaudio[ext=m4a]/bestvideo[ext=mp4]+bestaudio[ext=m4a]/bestvideo+bestaudio/best[ext=mp4]/best";
                    break;

                case "Solo Video":
                    txtArgs.Text = "-f bestvideo[ext=mp4][vcodec^=avc]/bestvideo[ext=mp4]/bestvideo";
                    break;

                case "Audio MP3":
                    txtArgs.Text = "-x --audio-format mp3 --audio-quality 192 --embed-thumbnail";
                    break;

                case "Audio M4A":
                    txtArgs.Text = "-f bestaudio[ext=m4a] --embed-thumbnail";
                    break;
            }
        }

        private void aggiornaYtdlpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                string command = "yt-dlp.exe -U";
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c " + command,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore durante l'aggiornamento: " + ex.Message);
            }
        }

        private void MSInfoYtdlp_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void txtArgs_TextChanged(object sender, EventArgs e)
        {

        }

        private void chkPlaylist_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void lblDownloadPath_Click(object sender, EventArgs e)
        {

        }

        private void txtUrl_TextChanged(object sender, EventArgs e)
        {

        }

        private void infoYtdlp_Click(object sender, EventArgs e)
        {
            try
            {
                string command = "yt-dlp.exe --help & pause";
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c " + command,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore nell'aprire l'help: " + ex.Message);
            }
        }

        private void mostraFormati_Click(object sender, EventArgs e)
        {
            try
            {
                string url = txtUrl.Text.Trim(); // prendi l'URL dalla TextBox
                if (string.IsNullOrEmpty(url))
                {
                    MessageBox.Show("Inserisci un URL valido nella casella URL.");
                    return;
                }

                string ytDlpPath = Path.Combine(Application.StartupPath, "yt-dlp.exe");
                if (!File.Exists(ytDlpPath))
                {
                    MessageBox.Show("Non ho trovato yt-dlp.exe nella cartella dell'app.");
                    return;
                }

                // Comando per mostrare i formati e aggiunge pausa
                string command = $"\"{ytDlpPath}\" -F {url} & pause";
                Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c " + command,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore nel mostrare i formati: " + ex.Message);
            }
        }
        private async Task DownloadFileAsync(string url, string outputPath, Label lbl)
        {
            using (HttpClient client = new HttpClient())
            using (var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var totalBytes = response.Content.Headers.ContentLength ?? -1L;
                bool canReportProgress = totalBytes != -1 && lbl != null;

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    byte[] buffer = new byte[8192];
                    long totalRead = 0;
                    int read;
                    int barWidth = 20; // lunghezza barra

                    while ((read = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await fs.WriteAsync(buffer, 0, read);
                        totalRead += read;

                        if (canReportProgress)
                        {
                            int percent = (int)((totalRead * 100) / totalBytes);
                            int progressBars = (percent * barWidth) / 100;
                            string bar = "[" + new string('#', progressBars) + new string('-', barWidth - progressBars) + $"] {percent}%";
                            lbl.Text = $"Scaricando {Path.GetFileName(outputPath)} {bar}";
                            lbl.Refresh();
                        }
                    }
                }
            }
        }



        private async void BtnScaricaTool_Click(object sender, EventArgs e)
        {
            string appPath = Application.StartupPath;
            string ytDlpUrl = "https://github.com/yt-dlp/yt-dlp/releases/latest/download/yt-dlp.exe";
            string ffmpegZipUrl = "https://www.gyan.dev/ffmpeg/builds/ffmpeg-release-essentials.zip";

            string ytPath = Path.Combine(appPath, "yt-dlp.exe");
            string ffZipPath = Path.Combine(appPath, "ffmpeg.zip");
            string ffmpegExePath = Path.Combine(appPath, "ffmpeg.exe");
            string ffprobeExePath = Path.Combine(appPath, "ffprobe.exe");

            try
            {
                // Cancella file esistenti
                if (File.Exists(ytPath)) File.Delete(ytPath);
                if (File.Exists(ffmpegExePath)) File.Delete(ffmpegExePath);
                if (File.Exists(ffprobeExePath)) File.Delete(ffprobeExePath);
                if (File.Exists(ffZipPath)) File.Delete(ffZipPath);

                // Lista dei file da scaricare
                var filesToDownload = new List<(string url, string path)>
        {
            (ytDlpUrl, ytPath),
            (ffmpegZipUrl, ffZipPath)
        };

                int currentFile = 1;
                int totalFiles = filesToDownload.Count;

                foreach (var file in filesToDownload)
                {
                    lblProgress.Text = $"Scaricando file {currentFile}/{totalFiles}: {Path.GetFileName(file.path)}";
                    await DownloadFileAsync(file.url, file.path, lblProgress);
                    currentFile++;
                }

                // Estrai solo ffmpeg.exe e ffprobe.exe
                lblProgress.Text = "Estrazione ffmpeg.exe e ffprobe.exe...";
                using (ZipArchive archive = ZipFile.OpenRead(ffZipPath))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (entry.Name.Equals("ffmpeg.exe", StringComparison.OrdinalIgnoreCase) ||
                            entry.Name.Equals("ffprobe.exe", StringComparison.OrdinalIgnoreCase))
                        {
                            string destinationPath = Path.Combine(appPath, entry.Name);
                            entry.ExtractToFile(destinationPath, true);
                        }
                    }
                }

                // Cancella il file ZIP
                if (File.Exists(ffZipPath)) File.Delete(ffZipPath);

                lblProgress.Text = "Download completato!";
                //MessageBox.Show(
                //    $"Download completato!\n\n" +
                //    $"yt-dlp: {ytPath}\n" +
                //    $"ffmpeg: {ffmpegExePath}\n" +
                //    $"ffprobe: {ffprobeExePath}",
                //    "Download completato",
                //    MessageBoxButtons.OK,
                //    MessageBoxIcon.Information
                //);
            }
            catch (Exception ex)
            {
                lblProgress.Text = "Errore durante il download.";
                MessageBox.Show("Errore: " + ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }

}



        
        
    
    
