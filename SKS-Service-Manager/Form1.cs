using System.Diagnostics;
using System.Net;
using Ionic.Zip;

#pragma warning disable
namespace SKS_Service_Manager
{
    public partial class Form1 : Form
    {
        private Settings settingsForm;
        private IssueUKS issueUksForm;
        private UserList userlistForm;
        private UksList uksListForm;
        private DataBase database;
        private System.Timers.Timer syncTimer;

        private string appdataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        private string word2Pdf = "C:\\Program Files (x86)\\Weeny Free Word to PDF Converter\\word2pdf.exe";
        private string word2PdfInstaller = AppDomain.CurrentDomain.BaseDirectory + "word2pdf.msi";

        private string remoteUrl = "https://github.com/domcie99/SKS-Service-Manager/releases/download/v1.0.1/PDFConvert.zip";
        private string versionUrl = "https://raw.githubusercontent.com/domcie99/SKS-Service-Manager/master/SKS-Service-Manager/version.txt";
        private string updateUrl = "https://github.com/domcie99/SKS-Service-Manager/raw/master/SKS-Service-Manager-Installer/SKS-Service-Manager.msi";

        private string localVersion = "1.4.1.0";
        private string latestVersion;

        private bool isUpdating = false;

        public Form1()
        {
            InitializeComponent();
            CenterToScreen();
            this.Text = "SKS-Service Manager v" + localVersion;

            syncTimer = new System.Timers.Timer();
            syncTimer.Interval = 5 * 60 * 1000;
            syncTimer.AutoReset = true;
            syncTimer.Elapsed += async (sender, e) => await CompareAndSyncDataAsync();
            syncTimer.Start();

            button1.Enabled = false;
            button2.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;
        }

        private async void CheckForUpdates()
        {
            try
            {
                WebClient webClient = new WebClient();
                latestVersion = webClient.DownloadString(versionUrl).Trim();

                if (latestVersion != localVersion)
                {
                    DialogResult result = MessageBox.Show($"Nowa aktualizacja ({latestVersion}) jest dost�pna, czy chcesz j� pobra� teraz?", "Aktualizacja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        DownloadAndInstallUpdate(latestVersion);
                    }
                }
            }
            catch (Exception ex)
            {
                // Obs�uga b��d�w
                MessageBox.Show($"B��d sprawdzania aktualizacji:" + ex, "Aktualizacja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
        }

        private async void DownloadAndInstallUpdate(string latestVersion)
        {
            isUpdating = true;

            label2.Invoke(new Action(() => label2.Visible = true));
            progressBar1.Invoke(new Action(() => progressBar1.Visible = true));
            try
            {
                WebClient client = new WebClient();
                string updateUrlFormatted = string.Format(updateUrl, latestVersion);
                string msiPath = Path.Combine(Path.GetTempPath(), "SKS-Service-Manager.msi");

                client.DownloadProgressChanged += Client_DownloadProgressChanged; // Dodajemy obs�ug� zdarzenia post�pu pobierania

                await client.DownloadFileTaskAsync(new Uri(updateUrlFormatted), msiPath);


                Task.Run(() =>
                {
                    MessageBox.Show("Aplikacja zostanie teraz zaaktualizowana", "Instalator", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }).ContinueWith((t) =>
                {
                    try
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo("cmd", $"/c start /wait {msiPath}")
                        {
                            WindowStyle = ProcessWindowStyle.Hidden,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };
                        Process process = Process.Start(startInfo);
                        Application.Exit(); // U�yj ExitThread zamiast Exit
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("B��d podczas otwierania pliku: " + ex.Message, "B��d", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("B��d podczas pobierania i instalowania aktualizacji: " + ex.Message);
            }
            finally
            {
                isUpdating = false;
            }
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Aktualizujemy progressBar2 na podstawie post�pu pobierania
            progressBar1.Invoke(new Action(() => progressBar1.Value = e.ProgressPercentage));
            // Aktualizujemy label2
            label2.Invoke(new Action(() => label2.Text = $"Pobieranie {e.ProgressPercentage}%"));
        }

        public async void CheckDependanceInstalled()
        {
            if (isUpdating) return;

            if (!File.Exists(word2Pdf))
            {
                // Wy�wietlanie okna dialogowego informuj�cego o instalacji pakietu
                MessageBox.Show("Aby korzysta� z tej aplikacji, potrzebujesz zainstalowanego dodatkowego pakietu. Automatyczne pobieranie i instalacja pakietu rozpoczn� si� teraz.", "Brak zainstalowanego word2pdf", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo("cmd", $"/c start /wait {word2PdfInstaller}")
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    Process process = Process.Start(startInfo);
                    process.WaitForExit();

                    MessageBox.Show("Pakiet zosta� pomy�lnie zainstalowany.", "Instalacja zako�czona", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("B��d podczas otwierania pliku: " + ex.Message, "B��d", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }





        public void setDataBase()
        {
            database = new DataBase(this);
        }
        public DataBase getDataBase()
        {
            return database;
        }

        private void button1_Click(object sender, EventArgs e) //Issue 1
        {
            OpenIssueUKSForm(-1);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenSettingsForm();
        }

        // Metoda do sprawdzania po��czenia z baz� danych MySQL
        public async void CheckMySQLConnection()
        {
            if (database.useMySQL)
            {
                // Ustawienie obrazu na good-connection.png
                pictureBox1.Image = Properties.Resources.good_connection;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                // Ustawienie obrazu na no-connection.png
                pictureBox1.Image = Properties.Resources.no_connection;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }

            CreateIssueUKSForm();
            CreateUksListForm();
            CreateUserListForm();

            button1.Invoke(new Action(() => button1.Enabled = true));
            button2.Invoke(new Action(() => button2.Enabled = true));
            button7.Invoke(new Action(() => button7.Enabled = true));
            button8.Invoke(new Action(() => button8.Enabled = true));
        }

        private void CreateIssueUKSForm()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    issueUksForm = new IssueUKS(-1, this);
                });
            }
            else
            {
                issueUksForm = new IssueUKS(-1, this);
            }
        }

        private void CreateUksListForm()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    uksListForm = new UksList(this);
                });
            }
            else
            {
                uksListForm = new UksList(this);
            }
        }

        private void CreateUserListForm()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    userlistForm = new UserList(this);
                });
            }
            else
            {
                userlistForm = new UserList(this);
            }
        }

        private void OpenSettingsForm()
        {
            if (settingsForm == null || settingsForm.IsDisposed)
            {
                settingsForm = new Settings(this); // Tworzenie nowego formularza ustawie�, je�li nie istnieje lub zosta� zamkni�ty
            }
            settingsForm.ShowDialog(); // Wy�wietlanie formularza ustawie�
        }

        private void OpenUksForm()
        {
            if (uksListForm == null || uksListForm.IsDisposed)
            {
                uksListForm = new UksList(this);
            }
            uksListForm.LoadData();
            uksListForm.ShowDialog(); // Wy�wietlanie formularza ustawie�
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (userlistForm == null || userlistForm.IsDisposed)
            {
                userlistForm = new UserList(this);
            }

            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate
                {
                    userlistForm.Show();
                });
            }
            else
            {
                userlistForm.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenUksForm();
        }
        private void OpenIssueUKSForm(int Id)
        {
            if (issueUksForm == null || issueUksForm.IsDisposed)
            {
                issueUksForm = new IssueUKS(Id, this);
            }
            issueUksForm.ShowDialog();
        }

        private async Task DownloadFileAsync(string fileUrl, string zipPath, string unzipPath)
        {
            label2.Invoke(new Action(() => label2.Visible = true));
            progressBar1.Invoke(new Action(() => progressBar1.Visible = true));


            label2.Invoke(new Action(() => label2.Text = "Pobieranie 0%"));
            progressBar1.Invoke(new Action(() => progressBar1.Value = 0));

            using (var httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();

                    long? totalBytes = response.Content.Headers.ContentLength;
                    long bytesRead = 0;

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        using (var fileStream = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            var buffer = new byte[4096];
                            int bytesReadThisChunk;
                            double progressPercentage;

                            while ((bytesReadThisChunk = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                            {
                                await fileStream.WriteAsync(buffer, 0, bytesReadThisChunk);
                                bytesRead += bytesReadThisChunk;

                                if (totalBytes.HasValue)
                                {
                                    progressPercentage = (double)bytesRead / totalBytes.Value * 100;
                                    UpdateProgressBar((int)progressPercentage);
                                    label2.Invoke(new Action(() => label2.Text = $"Pobieranie {progressPercentage:F1}%"));
                                }
                            }
                        }
                    }
                    UpdateProgressBar(100);
                    label2.Invoke(new Action(() => label2.Text = "Wypakowywanie... To mo�e chwil� zaj��"));
                    ExtractZipFile(zipPath, unzipPath);
                }
                catch (HttpRequestException ex)
                {
                    // Obs�u� b��d ��dania HTTP
                    MessageBox.Show("B��d podczas pobierania pliku: " + ex.Message, "B��d", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    // Obs�u� inne b��dy
                    MessageBox.Show("B��d: " + ex.Message, "B��d", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateProgressBar(int value)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<int>(UpdateProgressBar), value);
            }
            else
            {
                progressBar1.Value = value;
            }
        }

        private async void ExtractZipFile(string zipFilePath, string extractPath)
        {
            try
            {
                using (Ionic.Zip.ZipFile zip = Ionic.Zip.ZipFile.Read(zipFilePath))
                {
                    int totalEntries = zip.Entries.Count;
                    int entriesExtracted = 0;

                    foreach (var entry in zip)
                    {
                        entry.Extract(extractPath, ExtractExistingFileAction.OverwriteSilently);

                        entriesExtracted++;
                        double progressPercentage = (double)entriesExtracted / totalEntries * 100;
                        UpdateProgressBar((int)progressPercentage);
                        label2.Invoke(new Action(() => label2.Text = $"Wypakowywanie {progressPercentage:F1}%"));
                    }

                    // Po zako�czeniu wypakowywania pliku, zaktualizuj pasek post�pu na 100% (pe�ny post�p)
                    UpdateProgressBar(100);

                    progressBar1.Invoke(new Action(() => progressBar1.Visible = false));
                    label2.Invoke(new Action(() => label2.Visible = false));

                    MessageBox.Show("Zainstalowano pakiet LibreOffice", "MSG", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("B��d podczas wypakowywania pliku: " + ex.Message, "B��d", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void Form1_Shown(object sender, EventArgs e)
        {
            settingsForm = new Settings(this);
            database = new DataBase(this);
            await Task.Run(() => CheckMySQLConnection());
            await Task.Run(() => CheckForUpdates());
            await Task.Run(() => CheckDependanceInstalled());
        }

        private async Task CompareAndSyncDataAsync()
        {
            await Task.Run(() => CheckMySQLConnection());
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
