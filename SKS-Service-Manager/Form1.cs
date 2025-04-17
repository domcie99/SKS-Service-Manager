using System.Diagnostics;
using System.Net;
using System.Reflection;
using MethodInvoker = System.Windows.Forms.MethodInvoker;

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

        private string versionUrl = "http://83.15.62.46:8081/version.txt";
        private string updateUrl = "http://83.15.62.46:8081/SKS-Service-Manager.msi";

        private string localVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
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
                using (WebClient webClient = new WebClient())
                {
                    // Dodaj losowy parametr do URL, aby unikn¹æ cache'owania
                    string versionUrlWithNoCache = $"{versionUrl}?nocache={Guid.NewGuid()}";

                    // Pobierz najnowsz¹ wersjê z serwera
                    latestVersion = await webClient.DownloadStringTaskAsync(versionUrlWithNoCache).ConfigureAwait(false);
                    latestVersion = latestVersion.Trim();

                    if (latestVersion != localVersion)
                    {
                        DialogResult result = MessageBox.Show($"Nowa aktualizacja ({latestVersion}) jest dostêpna, czy chcesz j¹ pobraæ teraz?",
                                                              "Aktualizacja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            DownloadAndInstallUpdate(latestVersion);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"B³¹d sprawdzania aktualizacji: {ex.Message}", "Aktualizacja", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void DownloadAndInstallUpdate(string latestVersion)
        {
            isUpdating = true;
            SetButtonsEnabled(false);
            Cursor = Cursors.WaitCursor;

            label2.Invoke(new Action(() => label2.Visible = true));
            progressBar1.Invoke(new Action(() => progressBar1.Visible = true));
            try
            {
                WebClient client = new WebClient();
                string updateUrlFormatted = string.Format(updateUrl, latestVersion);
                string msiPath = Path.Combine(Path.GetTempPath(), "SKS-Service-Manager.msi");

                if (File.Exists(msiPath)) { File.Delete(msiPath); }

                client.DownloadProgressChanged += Client_DownloadProgressChanged; // Dodajemy obs³ugê zdarzenia postêpu pobierania

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
                        Application.Exit(); // U¿yj ExitThread zamiast Exit
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("B³¹d podczas otwierania pliku: " + ex.Message, "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("B³¹d podczas pobierania i instalowania aktualizacji: " + ex.Message);
            }
            finally
            {
                isUpdating = false;
                SetButtonsEnabled(true);
                Cursor = Cursors.Default;
            }
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Aktualizujemy progressBar2 na podstawie postêpu pobierania
            progressBar1.Invoke(new Action(() => progressBar1.Value = e.ProgressPercentage));
            // Aktualizujemy label2
            label2.Invoke(new Action(() => label2.Text = $"Pobieranie {e.ProgressPercentage}%"));
        }

        public async void CheckDependanceInstalled()
        {
            if (isUpdating) return;

            if (!File.Exists(word2Pdf))
            {
                // Wyœwietlanie okna dialogowego informuj¹cego o instalacji pakietu
                MessageBox.Show("Aby korzystaæ z tej aplikacji, potrzebujesz zainstalowanego dodatkowego pakietu. Automatyczne pobieranie i instalacja pakietu rozpoczn¹ siê teraz.", "Brak zainstalowanego word2pdf", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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

                    MessageBox.Show("Pakiet zosta³ pomyœlnie zainstalowany.", "Instalacja zakoñczona", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("B³¹d podczas otwierania pliku: " + ex.Message, "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // Metoda do sprawdzania po³¹czenia z baz¹ danych MySQL
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

        private async void Form1_Shown(object sender, EventArgs e)
        {
            settingsForm = new Settings(this);
            database = new DataBase(this);
            await Task.Run(() => CheckForUpdates());
            await Task.Run(() => CheckDependanceInstalled());
            await Task.Run(() => CheckMySQLConnection());
        }

        private async Task CompareAndSyncDataAsync()
        {
            await Task.Run(() => CheckMySQLConnection());
        }

        private void SetButtonsEnabled(bool isEnabled)
        {
            button1.Invoke(new Action(() => button1.Enabled = isEnabled));
            button2.Invoke(new Action(() => button2.Enabled = isEnabled));
            button7.Invoke(new Action(() => button7.Enabled = isEnabled));
            button8.Invoke(new Action(() => button8.Enabled = isEnabled));
        }

    }
}
