using FastColoredTextBoxNS;
using ImageMagick;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class VideoListForm : Form
    {
        private string videoUrl;
        private string cachePath;
        private string url;
        private HashSet<string> openedLinks = new HashSet<string>();
        public VideoListForm(string url)
        {
            InitializeComponent();
            videoUrl = url;
            cachePath = Path.Combine(Application.StartupPath, GetSafeFilename(videoUrl) + ".json");
            this.url = url;

            // Mantieni sempre il form in primo piano
            this.TopMost = true;

            // Se vuoi puoi anche mostrare l'URL da qualche parte, ad esempio:
            this.Text = "VideoList - " + url;
        }
        private void ApriLink(string url)
        {
            if (string.IsNullOrEmpty(url)) return;

            // Non aprire link già aperti
            if (openedLinks.Contains(url)) return;

            openedLinks.Add(url);

            Task.Run(() =>
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    this.Invoke((Action)(() =>
                    {
                        MessageBox.Show("Errore nell'aprire il link: " + ex.Message);
                    }));
                }
            });
        }
        private string GetSafeFilename(string url)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                url = url.Replace(c, '_');
            return url;
        }

        private async void VideoListForm_Load(object sender, EventArgs e)
        {
            await CaricaVideoAsync(false);
        }

        private async Task CaricaVideoAsync(bool forzaAggiorna)
        {
            flowPanel.Controls.Clear();

            dynamic videoInfo = null;

            if (!forzaAggiorna && File.Exists(cachePath))
            {
                string cachedJson = File.ReadAllText(cachePath);
                videoInfo = JsonConvert.DeserializeObject(cachedJson);
            }
            else
            {
                string ytDlpPath = Path.Combine(Application.StartupPath, "yt-dlp.exe");
                if (!File.Exists(ytDlpPath))
                {
                    MessageBox.Show("Non ho trovato yt-dlp.exe nella cartella dell'app.");
                    return;
                }

                string command = $"\"{ytDlpPath}\" -j {videoUrl}";
                string output = await Task.Run(() => RunYtDlp(command));

                if (string.IsNullOrWhiteSpace(output))
                {
                    MessageBox.Show("yt-dlp non ha restituito dati.");
                    return;
                }

                videoInfo = JsonConvert.DeserializeObject(output);

                // Salva cache JSON
                File.WriteAllText(cachePath, JsonConvert.SerializeObject(videoInfo, Formatting.Indented));
            }

            await MostraVideoAsync(videoInfo);
        }

        private async Task MostraVideoAsync(dynamic videoInfo)
        {
            flowPanel.Controls.Clear();

            // Scarica miniatura
            string thumbUrl = videoInfo.thumbnail_maxres != null ? (string)videoInfo.thumbnail_maxres
                            : videoInfo.thumbnail != null ? (string)videoInfo.thumbnail
                            : null;
            Image thumbnail = await ScaricaMiniaturaAsync(thumbUrl);

            Panel videoPanel = new Panel
            {
                Width = 765,
                Height = 400,
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(2)
            };

            // PictureBox miniatura
            PictureBox pb = new PictureBox
            {
                Image = thumbnail,
                SizeMode = PictureBoxSizeMode.Zoom,
                Width = 350,
                Height = 250,
                Location = new Point(2, 2)
            };
            videoPanel.Controls.Add(pb);

            // ListBox formati sotto la miniatura
            ListBox lstFormati = new ListBox
            {
                Width = 350,
                Height = 120,
                Location = new Point(2, 260),
                Font = new Font("Courier New", 9)
            };

            if (videoInfo.formats != null)
            {
                foreach (var fmt in videoInfo.formats)
                {
                    string formatId = fmt.format_id != null ? (string)fmt.format_id : "";
                    string ext = fmt.ext != null ? (string)fmt.ext : "";
                    string note = fmt.format_note != null ? (string)fmt.format_note : "";
                    long filesizeBytes = fmt.filesize != null ? (long)fmt.filesize : 0;

                    // Converte in MB/GB
                    string sizeText;
                    if (filesizeBytes >= 1073741824) // >= 1 GB
                        sizeText = (filesizeBytes / 1073741824.0).ToString("0.##") + " GB";
                    else if (filesizeBytes >= 1048576) // >= 1 MB
                        sizeText = (filesizeBytes / 1048576.0).ToString("0.##") + " MB";
                    else if (filesizeBytes >= 1024) // >= 1 KB
                        sizeText = (filesizeBytes / 1024.0).ToString("0.##") + " KB";
                    else
                        sizeText = filesizeBytes + " B";

                    string line = $"{formatId,-6} {ext,-5} {note,-15} {sizeText}";
                    lstFormati.Items.Add(line);
                }
            }
            else
            {
                lstFormati.Items.Add("Nessun formato trovato");
            }
            videoPanel.Controls.Add(lstFormati);

            // Info e descrizione a destra
            Panel infoPanel = new Panel
            {
                Location = new Point(360, 2),
                Width = 400,
                Height = 380
            };

            // Info principali
            string uploadDateText = "N/D";
            string uploadDateStr = videoInfo.upload_date != null ? (string)videoInfo.upload_date : null;
            if (!string.IsNullOrEmpty(uploadDateStr) && uploadDateStr.Length == 8)
                uploadDateText = DateTime.ParseExact(uploadDateStr, "yyyyMMdd", null).ToString("dd/MM/yyyy");

            Label lblInfo = new Label
            {
                AutoSize = false,
                Width = 400,
                Height = 160,
                Font = new Font("Segoe UI", 10),
                Text =
                    $"🎬 Titolo: {(videoInfo.title != null ? (string)videoInfo.title : "N/D")}\n" +
                    $"👤 Uploader: {(videoInfo.uploader != null ? (string)videoInfo.uploader : "N/D")}\n" +
                    $"📅 Data upload: {uploadDateText}\n" +
                    $"⏱ Durata: {(videoInfo.duration_string != null ? (string)videoInfo.duration_string : "N/D")}\n" +
                    $"👁 Views: {(videoInfo.view_count != null ? videoInfo.view_count.ToString() : "0")}\n" +
                    $"👍 Like: {(videoInfo.like_count != null ? videoInfo.like_count.ToString() : "0")}\n" +
                    $"💬 Commenti: {(videoInfo.comment_count != null ? videoInfo.comment_count.ToString() : "0")}"
            };

            // Link cliccabile
            string linkUrl = videoInfo.webpage_url != null ? (string)videoInfo.webpage_url : "";
            LinkLabel link = new LinkLabel
            {
                Text = linkUrl,
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Underline),
                LinkColor = Color.Blue,
                Location = new Point(0, 170)
            };
            link.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(linkUrl))
                    ApriLink(linkUrl);
            };

            // FastColoredTextBox descrizione
            var fctbDescription = new FastColoredTextBox
            {
                Multiline = true,
                ReadOnly = true,
                Width = 400,
                Height = 200,
                Location = new Point(0, 200),
                Font = new Font("Consolas", 9),
                Text = videoInfo.description != null ? (string)videoInfo.description : "N/D",
                Language = Language.Custom,
                WordWrap = true,
                BackColor = Color.White
            };

            var linkStyle = new TextStyle(Brushes.Blue, null, FontStyle.Underline);
            var tagStyle = new TextStyle(Brushes.Green, null, FontStyle.Bold);
            fctbDescription.TextChanged += (s, e) =>
            {
                e.ChangedRange.ClearStyle(linkStyle);
                e.ChangedRange.ClearStyle(tagStyle);
                e.ChangedRange.SetStyle(linkStyle, @"https?://\S+");
                e.ChangedRange.SetStyle(tagStyle, @"#\w+");
            };

            infoPanel.Controls.Add(lblInfo);
            infoPanel.Controls.Add(link);
            infoPanel.Controls.Add(fctbDescription);
            videoPanel.Controls.Add(infoPanel);

            flowPanel.Controls.Add(videoPanel);
        }





        private async Task<Image> ScaricaMiniaturaAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
                return SystemIcons.Warning.ToBitmap();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    byte[] imgBytes = await client.GetByteArrayAsync(url);
                    using (var ms = new MemoryStream(imgBytes))
                    using (var img = new MagickImage(ms))
                    {
                        img.Format = MagickFormat.Png;
                        img.Resize(350, 250);
                        img.FilterType = FilterType.Cubic; // alta qualità

                        using (var msOut = new MemoryStream())
                        {
                            img.Write(msOut);
                            msOut.Position = 0;
                            return Image.FromStream(msOut);
                        }
                    }
                }
            }
            catch
            {
                return SystemIcons.Warning.ToBitmap();
            }
        }

        private string RunYtDlp(string command)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c " + command,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output;
        }

        private async void btnAggiorna_Click(object sender, EventArgs e)
        {
            await CaricaVideoAsync(true);
        }

        private void flowPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
