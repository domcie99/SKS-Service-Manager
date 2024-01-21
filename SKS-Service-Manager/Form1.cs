using Microsoft.Win32;
using System.Diagnostics;
using System.IO.Compression;
using System.Net;
using static System.Net.WebRequestMethods;
using File = System.IO.File;

namespace SKS_Service_Manager
{
    public partial class Form1 : Form
    {
        private Settings settingsForm;
        private IssueUKS issueUksForm;
        private UserList userlistForm;
        private UksList uksListForm;
        private DataBase database;

        public Form1()
        {
            InitializeComponent();


            WebClient webClient = new WebClient();
            var client = new WebClient();
            if (!webClient.DownloadString("link to web host/Version.txt").Contains("1.0.0"))
            {
                if (MessageBox.Show("A new update is available! Do you want to download it?", "Demo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        if (File.Exists(@".\MyAppSetup.msi")) { File.Delete(@".\MyAppSetup.msi"); }
                        client.DownloadFile("link to web host/MyAppSetup.zip", @"MyAppSetup.zip");
                        string zipPath = @".\MyAppSetup.zip";
                        string extractPath = @".\";
                        ZipFile.ExtractToDirectory(zipPath, extractPath);
                        Process process = new Process();
                        process.StartInfo.FileName = "msiexec.exe";
                        process.StartInfo.Arguments = string.Format("/i MyAppSetup.msi");
                        this.Close();
                        process.Start();
                    }
                    catch
                    {
                    }
                }
            }


            settingsForm = new Settings(this); // Inicjalizacja formularza ustawie�
            database = new DataBase(this);

            CheckMySQLConnection();
            CheckAndShowOfficeMessage();
        }



        public static void CheckAndShowOfficeMessage()
        {
            try
            {
                using (RegistryKey key = Registry.ClassesRoot.OpenSubKey("Word.Application"))
                {
                    if (key == null)
                    {
                        DialogResult result = MessageBox.Show("Aby korzysta� z tej aplikacji, potrzebujesz zainstalowanego programu Microsoft Office. Kliknij 'OK', aby pobra� Office.", "Brak zainstalowanego Office", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                        if (result == DialogResult.OK)
                        {
                            try
                            {
                                Process.Start("cmd", $"/c start https://www.microsoft.com/pl-pl/microsoft-365/get-started-with-office-2019");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("B��d podczas otwierania pliku uks: " + ex.Message, "B��d", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception){}
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
            // Tworzenie nowego formularza listy u�ytkownik�w i przekazanie do niego referencji do formy Settings
            if (userlistForm == null || userlistForm.IsDisposed)
            {
                userlistForm = new UserList(this); // Tworzenie nowego formularza ustawie�, je�li nie istnieje lub zosta� zamkni�ty
            }
            userlistForm.Show(); // Wy�wietlenie formularza listy u�ytkownik�w
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
    }
}
