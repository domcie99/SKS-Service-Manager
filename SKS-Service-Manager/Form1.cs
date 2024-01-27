
using System.Diagnostics;
using System.Net;
using Ionic.Zip;

namespace SKS_Service_Manager
{
    public partial class Form1 : Form
    {
        private Settings settingsForm;
        private IssueUKS issueUksForm;
        private UserList userlistForm;
        private UksList uksListForm;
        private DataBase database;

        string remoteUrl = "https://github.com/domcie99/SKS-Service-Manager/releases/download/v1.0.1/PDFConvert.zip";

        string localPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string localPathZip = AppDomain.CurrentDomain.BaseDirectory + "/PDFConvert.zip";
        string libreOfficeInst = "C:\\Program Files\\LibreOffice\\program\\soffice.exe";
        string libreOfficePort = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LibreOfficePortable\\App\\libreoffice\\program\\soffice.exe";

        private string versionUrl = "https://raw.githubusercontent.com/domcie99/SKS-Service-Manager/master/SKS-Service-Manager/version.txt";
        private string updateUrl = "https://github.com/domcie99/SKS-Service-Manager/raw/master/SKS-Service-Manager-Installer/SKS-Service-Manager.msi";
        private string localVersion = "1.0.4.0"; // Wersja Twojej aplikacji



        public Form1()
        {
            InitializeComponent();
            this.Text = "SKS-Service Manager v" + localVersion;

            settingsForm = new Settings(this); // Inicjalizacja formularza ustawieñ
            database = new DataBase(this);

            CheckMySQLConnection();
        }

        private async void CheckForUpdates()
        {
            try
            {
                WebClient webClient = new WebClient();
                string latestVersion = webClient.DownloadString(versionUrl).Trim();

                if (latestVersion != localVersion)
                {
                    DialogResult result = MessageBox.Show($"Nowa aktualizacja ({latestVersion}) jest dostêpna, czy chcesz j¹ pobraæ teraz?", "Aktualizacja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        DownloadAndInstallUpdate(latestVersion);
                    }
                }
            }
            catch (Exception ex)
            {
                // Obs³uga b³êdów
                Console.WriteLine("B³¹d podczas sprawdzania aktualizacji: " + ex.Message);
            }
        }


        private async void DownloadAndInstallUpdate(string latestVersion)
        {
            label2.Invoke(new Action(() => label2.Visible = true));
            progressBar1.Invoke(new Action(() => progressBar1.Visible = true));
            try
            {
                WebClient client = new WebClient();
                string updateUrlFormatted = string.Format(updateUrl, latestVersion);
                string msiPath = Path.Combine(Path.GetTempPath(), "SKS-Service-Manager.msi");

                client.DownloadProgressChanged += Client_DownloadProgressChanged; // Dodajemy obs³ugê zdarzenia postêpu pobierania

                await client.DownloadFileTaskAsync(new Uri(updateUrlFormatted), msiPath);

                try
                {
                    Process.Start("cmd", $"/c start {msiPath}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("B³¹d podczas otwierania pliku: " + ex.Message, "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                Application.Exit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("B³¹d podczas pobierania i instalowania aktualizacji: " + ex.Message);
            }
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Aktualizujemy progressBar2 na podstawie postêpu pobierania
            progressBar1.Invoke(new Action(() => progressBar1.Value = e.ProgressPercentage));
            // Aktualizujemy label2
            label2.Invoke(new Action(() => label2.Text = $"Pobieranie {e.ProgressPercentage}%"));
        }


        public async void CheckAndShowOfficeMessage()
        {
            if (!File.Exists(libreOfficeInst) && !File.Exists(libreOfficePort))
            {
                DialogResult result = MessageBox.Show("Aby korzystaæ z tej aplikacji, potrzebujesz zainstalowanego LibreOffice. Kliknij 'OK', aby automatycznie pobrac i zainstalowaæ pakiet.", "Brak zainstalowanego LibreOffice", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.OK)
                {
                    try
                    {
                        // Spróbuj otworzyæ stronê do pobrania Microsoft Office
                        await DownloadFileAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("B³¹d podczas otwierania strony do pobrania Office: " + ex.Message, "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void buttonDownload_Click(object sender, EventArgs e)
        {
            // Wywo³aj metodê DownloadFileAsync
            await DownloadFileAsync();
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

        // Metoda do sprawdzania po³¹czenia z baz¹ danych MySQL
        public void CheckMySQLConnection()
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

            database = new DataBase(this);
            issueUksForm = new IssueUKS(-1, this);
            uksListForm = new UksList(this);
            userlistForm = new UserList(this);
        }

        private void OpenSettingsForm()
        {
            if (settingsForm == null || settingsForm.IsDisposed)
            {
                settingsForm = new Settings(this); // Tworzenie nowego formularza ustawieñ, jeœli nie istnieje lub zosta³ zamkniêty
            }
            settingsForm.ShowDialog(); // Wyœwietlanie formularza ustawieñ
        }

        private void OpenUksForm()
        {
            if (uksListForm == null || uksListForm.IsDisposed)
            {
                uksListForm = new UksList(this);
            }
            uksListForm.LoadData();
            uksListForm.ShowDialog(); // Wyœwietlanie formularza ustawieñ
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Tworzenie nowego formularza listy u¿ytkowników i przekazanie do niego referencji do formy Settings
            if (userlistForm == null || userlistForm.IsDisposed)
            {
                userlistForm = new UserList(this); // Tworzenie nowego formularza ustawieñ, jeœli nie istnieje lub zosta³ zamkniêty
            }
            userlistForm.Show(); // Wyœwietlenie formularza listy u¿ytkowników
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

        private async Task DownloadFileAsync()
        {
            label2.Invoke(new Action(() => label2.Visible = true));
            progressBar1.Invoke(new Action(() => progressBar1.Visible = true));


            label2.Invoke(new Action(() => label2.Text = "Pobieranie 0%"));
            progressBar1.Invoke(new Action(() => progressBar1.Value = 0));

            using (var httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(remoteUrl, HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();

                    long? totalBytes = response.Content.Headers.ContentLength;
                    long bytesRead = 0;

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        using (var fileStream = new FileStream(localPathZip, FileMode.Create, FileAccess.Write, FileShare.None))
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

                    label2.Invoke(new Action(() => label2.Text = "Wypakowywanie... To mo¿e chwilê zaj¹æ"));
                    // Plik zosta³ pomyœlnie pobrany, teraz mo¿esz go rozpakowaæ lub podj¹æ inne dzia³ania
                    ExtractZipFile(localPathZip, localPath);
                }
                catch (HttpRequestException ex)
                {
                    // Obs³u¿ b³¹d ¿¹dania HTTP
                    MessageBox.Show("B³¹d podczas pobierania pliku: " + ex.Message, "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    // Obs³u¿ inne b³êdy
                    MessageBox.Show("B³¹d: " + ex.Message, "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    // Po zakoñczeniu wypakowywania pliku, zaktualizuj pasek postêpu na 100% (pe³ny postêp)
                    UpdateProgressBar(100);

                    progressBar1.Invoke(new Action(() => progressBar1.Visible = false));
                    label2.Invoke(new Action(() => label2.Visible = false));

                    MessageBox.Show("Zainstalowano pakiet LibreOffice", "MSG", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("B³¹d podczas wypakowywania pliku: " + ex.Message, "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }






        private async void Form1_Shown(object sender, EventArgs e)
        {
            await Task.Run(() => CheckForUpdates());
            await Task.Run(() => CheckAndShowOfficeMessage());
        }
    }
}
