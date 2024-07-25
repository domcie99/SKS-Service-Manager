#pragma warning disable
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace SKS_Service_Manager
{
    public partial class Settings : Form
    {
        private Form1 mainForm;
        private DataBaseInsert dataInsert;
        internal AppSettings appSettings;
        private string settingsFilePath;

        public Settings(Form1 form1)
        {
            InitializeComponent();
            mainForm = form1;
            CenterToScreen();

            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".sks_settings");
            Directory.CreateDirectory(appDataPath);
            settingsFilePath = Path.Combine(appDataPath, "settings.json");

            LoadSettings();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            appSettings.CompanyName = company_name.Text;
            appSettings.NIP = NIP.Text;
            appSettings.Name = name.Text;
            appSettings.Surname = Surname.Text;
            appSettings.PostCode = Post_Code.Text;
            appSettings.City = City.Text;
            appSettings.StreetNumber = Street_And_Number.Text;
            appSettings.Phone = Phone.Text;
            appSettings.Email = EMail.Text;
            appSettings.KRS = KRS.Text;
            appSettings.REGON = REGON.Text;
            appSettings.MySQLHost = host.Text;
            appSettings.MySQLUser = user.Text;
            appSettings.MySQLPassword = password.Text;
            appSettings.MySQLDatabase = database.Text;
            appSettings.MySQLPort = port.Text;
            appSettings.Percentage = int.Parse(percentage.Text);
            appSettings.Days = int.Parse(days.Text);

            try
            {
                SaveSettings();
                MessageBox.Show("Ustawienia zostały zapisane.", "Zapisano", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nie udało się zapisać ustawień: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Hide();
            mainForm.setDataBase();
            mainForm.CheckMySQLConnection();
            this.Close();
        }

        private void Abort_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadSettings()
        {
            if (File.Exists(settingsFilePath))
            {
                appSettings = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText(settingsFilePath));
            }
            else
            {
                appSettings = new AppSettings
                {
                    CompanyName = Properties.Settings.Default.company_name,
                    NIP = Properties.Settings.Default.NIP,
                    Name = Properties.Settings.Default.name,
                    Surname = Properties.Settings.Default.surname,
                    PostCode = Properties.Settings.Default.post_code,
                    City = Properties.Settings.Default.city,
                    StreetNumber = Properties.Settings.Default.street_number,
                    Phone = Properties.Settings.Default.phone,
                    Email = Properties.Settings.Default.email,
                    KRS = Properties.Settings.Default.krs,
                    REGON = Properties.Settings.Default.regon,
                    MySQLHost = Properties.Settings.Default.mysql_host,
                    MySQLUser = Properties.Settings.Default.mysql_user,
                    MySQLPassword = Properties.Settings.Default.mysql_password,
                    MySQLDatabase = Properties.Settings.Default.mysql_database,
                    MySQLPort = Properties.Settings.Default.mysql_port,
                    Percentage = int.Parse(Properties.Settings.Default.percentage),
                    Days = Properties.Settings.Default.days
                };
            }

            company_name.Text = appSettings.CompanyName;
            NIP.Text = appSettings.NIP;
            name.Text = appSettings.Name;
            Surname.Text = appSettings.Surname;
            Post_Code.Text = appSettings.PostCode;
            City.Text = appSettings.City;
            Street_And_Number.Text = appSettings.StreetNumber;
            Phone.Text = appSettings.Phone;
            EMail.Text = appSettings.Email;
            KRS.Text = appSettings.KRS;
            REGON.Text = appSettings.REGON;
            host.Text = appSettings.MySQLHost;
            user.Text = appSettings.MySQLUser;
            password.Text = appSettings.MySQLPassword;
            database.Text = appSettings.MySQLDatabase;
            port.Text = appSettings.MySQLPort;
            percentage.Text = appSettings.Percentage.ToString();
            days.Text = appSettings.Days.ToString();

            SaveSettings();
        }

        private void SaveSettings()
        {
            File.WriteAllText(settingsFilePath, JsonConvert.SerializeObject(appSettings));
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadSettings();
        }

        // Settery dla ustawień, które automatycznie zapisują dane
        public void SetCompanyName(string value)
        {
            appSettings.CompanyName = value;
            SaveSettings();
        }

        public void SetNIP(string value)
        {
            appSettings.NIP = value;
            SaveSettings();
        }

        public void SetName(string value)
        {
            appSettings.Name = value;
            SaveSettings();
        }

        public void SetSurname(string value)
        {
            appSettings.Surname = value;
            SaveSettings();
        }

        public void SetPostCode(string value)
        {
            appSettings.PostCode = value;
            SaveSettings();
        }

        public void SetCity(string value)
        {
            appSettings.City = value;
            SaveSettings();
        }

        public void SetStreetNumber(string value)
        {
            appSettings.StreetNumber = value;
            SaveSettings();
        }

        public void SetPhone(string value)
        {
            appSettings.Phone = value;
            SaveSettings();
        }

        public void SetEmail(string value)
        {
            appSettings.Email = value;
            SaveSettings();
        }

        public void SetKRS(string value)
        {
            appSettings.KRS = value;
            SaveSettings();
        }

        public void SetREGON(string value)
        {
            appSettings.REGON = value;
            SaveSettings();
        }

        public void SetMySQLHost(string value)
        {
            appSettings.MySQLHost = value;
            SaveSettings();
        }

        public void SetMySQLUser(string value)
        {
            appSettings.MySQLUser = value;
            SaveSettings();
        }

        public void SetMySQLPassword(string value)
        {
            appSettings.MySQLPassword = value;
            SaveSettings();
        }

        public void SetMySQLDatabase(string value)
        {
            appSettings.MySQLDatabase = value;
            SaveSettings();
        }

        public void SetMySQLPort(string value)
        {
            appSettings.MySQLPort = value;
            SaveSettings();
        }

        public void SetPercentage(int percentage)
        {
            appSettings.Percentage = percentage;
            SaveSettings();
        }

        public void SetDays(int days)
        {
            appSettings.Days = days;
            SaveSettings();
        }

        // Gettery dla ustawień
        public string GetCompanyName() => appSettings.CompanyName;
        public string GetNIP() => appSettings.NIP;
        public string GetName() => appSettings.Name;
        public string GetSurname() => appSettings.Surname;
        public string GetPostCode() => appSettings.PostCode;
        public string GetCity() => appSettings.City;
        public string GetStreetNumber() => appSettings.StreetNumber;
        public string GetPhone() => appSettings.Phone;
        public string GetEmail() => appSettings.Email;
        public string GetKRS() => appSettings.KRS;
        public string GetREGON() => appSettings.REGON;
        public int GetPercentage() => appSettings.Percentage;
        public int GetDays() => appSettings.Days;

        // Gettery dla ustawień MySQL
        public string GetMySQLHost() => appSettings.MySQLHost;
        public string GetMySQLUser() => appSettings.MySQLUser;
        public string GetMySQLPassword() => appSettings.MySQLPassword;
        public string GetMySQLDatabase() => appSettings.MySQLDatabase;
        public int GetMySQLPort() => int.Parse(appSettings.MySQLPort);

        // Metody obsługi plików umów
        private void OpenFileToEdit(string file)
        {
            string docxFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "umowy", $"{file}.docx");
            string copydocxFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "umowy", "backup", $"{file}.docx");

            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "umowy", "backup"));
            File.Copy(docxFile, copydocxFile, true);

            try
            {
                Process.Start("cmd", $"/c start {docxFile}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas otwierania pliku: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string docxFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "umowy");

            try
            {
                Process.Start("cmd", $"/c start {docxFile}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas otwierania folderu: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenDataBaseInsert();
        }

        private void OpenDataBaseInsert()
        {
            if (dataInsert == null || dataInsert.IsDisposed)
            {
                dataInsert = new DataBaseInsert(mainForm); // Tworzenie nowego formularza ustawień, jeśli nie istnieje lub został zamknięty
            }
            dataInsert.ShowDialog(); // Wyświetlanie formularza ustawień
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileToEdit("ukpl");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileToEdit("uks");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileToEdit("uk");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileToEdit("uppz");
        }
    }

    public class AppSettings
    {
        public string CompanyName { get; set; }
        public string NIP { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string StreetNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string KRS { get; set; }
        public string REGON { get; set; }
        public string MySQLHost { get; set; }
        public string MySQLUser { get; set; }
        public string MySQLPassword { get; set; }
        public string MySQLDatabase { get; set; }
        public string MySQLPort { get; set; }
        public int Percentage { get; set; }
        public int Days { get; set; }
    }
}
