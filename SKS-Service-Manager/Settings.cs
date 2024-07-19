#pragma warning disable
using System.Diagnostics;

namespace SKS_Service_Manager
{
    partial class Settings : Form
    {
        private Form1 mainForm;
        private DataBaseInsert dataInsert;

        public Settings(Form1 form1)
        {
            InitializeComponent();
            mainForm = form1;
            CenterToScreen();
            LoadSettings();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            // Przypisanie wartości z formularza do ustawień aplikacji
            Properties.Settings.Default.company_name = company_name.Text;
            Properties.Settings.Default.NIP = NIP.Text;
            Properties.Settings.Default.name = name.Text;
            Properties.Settings.Default.surname = Surname.Text;
            Properties.Settings.Default.post_code = Post_Code.Text;
            Properties.Settings.Default.city = City.Text;
            Properties.Settings.Default.street_number = Street_And_Number.Text;
            Properties.Settings.Default.phone = Phone.Text;
            Properties.Settings.Default.email = EMail.Text;
            Properties.Settings.Default.krs = KRS.Text; // Dodane pole KRS
            Properties.Settings.Default.regon = REGON.Text; // Dodane pole REGON

            // Ustawienia MySQL
            Properties.Settings.Default.mysql_host = host.Text;
            Properties.Settings.Default.mysql_user = user.Text;
            Properties.Settings.Default.mysql_password = password.Text;
            Properties.Settings.Default.mysql_database = database.Text;
            Properties.Settings.Default.mysql_port = port.Text;

            try
            {
                Properties.Settings.Default.Save(); // Zapisanie ustawień
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
            this.Close(); // Zamknięcie formularza bez zapisywania zmian
        }

        private void LoadSettings()
        {
            // Wczytanie wartości do formularza z ustawień aplikacji
            company_name.Text = Properties.Settings.Default.company_name;
            NIP.Text = Properties.Settings.Default.NIP;
            name.Text = Properties.Settings.Default.name;
            Surname.Text = Properties.Settings.Default.surname;
            Post_Code.Text = Properties.Settings.Default.post_code;
            City.Text = Properties.Settings.Default.city;
            Street_And_Number.Text = Properties.Settings.Default.street_number;
            Phone.Text = Properties.Settings.Default.phone;
            EMail.Text = Properties.Settings.Default.email;
            KRS.Text = Properties.Settings.Default.krs; // Dodane pole KRS
            REGON.Text = Properties.Settings.Default.regon; // Dodane pole REGON

            // Ustawienia MySQL
            host.Text = Properties.Settings.Default.mysql_host;
            user.Text = Properties.Settings.Default.mysql_user;
            password.Text = Properties.Settings.Default.mysql_password;
            database.Text = Properties.Settings.Default.mysql_database;
            port.Text = Properties.Settings.Default.mysql_port.ToString(); // port jest typu int, więc użyj metody ToString()
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadSettings(); // Załaduj ustawienia podczas ładowania formularza
        }

        public string GetCompanyName() => Properties.Settings.Default.company_name;
        public string GetNIP() => Properties.Settings.Default.NIP;
        public string GetName() => Properties.Settings.Default.name;
        public string GetSurname() => Properties.Settings.Default.surname;
        public string GetPostCode() => Properties.Settings.Default.post_code;
        public string GetCity() => Properties.Settings.Default.city;
        public string GetStreetNumber() => Properties.Settings.Default.street_number;
        public string GetPhone() => Properties.Settings.Default.phone;
        public string GetEmail() => Properties.Settings.Default.email;
        public string GetKRS() => Properties.Settings.Default.krs; // Dodane pole KRS
        public string GetREGON() => Properties.Settings.Default.regon; // Dodane pole REGON
        public int GetPercentage() => int.Parse(Properties.Settings.Default.percentage);
        public void SetPercentage(int i) => Properties.Settings.Default.percentage = "" + i;

        // Gettery dla ustawień MySQL
        public string GetMySQLHost() => Properties.Settings.Default.mysql_host;
        public string GetMySQLUser() => Properties.Settings.Default.mysql_user;
        public string GetMySQLPassword() => Properties.Settings.Default.mysql_password;
        public string GetMySQLDatabase() => Properties.Settings.Default.mysql_database;
        public int GetMySQLPort() => int.Parse(Properties.Settings.Default.mysql_port);

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

        private void OpenFileToEdit(String file)
        {
            string docxFile = AppDomain.CurrentDomain.BaseDirectory + "umowy/" + file + ".docx";
            string copydocxFile = AppDomain.CurrentDomain.BaseDirectory + "umowy/backup/" + file + ".docx";

            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "umowy/backup");
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
            string docxFile = AppDomain.CurrentDomain.BaseDirectory + "umowy/";

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
    }
}
