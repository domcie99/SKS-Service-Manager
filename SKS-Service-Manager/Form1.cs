using System.Diagnostics;

namespace SKS_Service_Manager
{
    public partial class Form1 : Form
    {
        private Settings settingsForm;
        private UserList userlistForm;
        private UksList uksListForm;
        private DataBase database;

        public Form1()
        {
            InitializeComponent();

            settingsForm = new Settings(this); // Inicjalizacja formularza ustawieñ

            database = new DataBase(this);

            CheckMySQLConnection();

            uksListForm = new UksList(this);
            userlistForm = new UserList(this); // Inicjalizacja formularza listy u¿ytkowników


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
            OpenUksForm();
        }
        private void button4_Click(object sender, EventArgs e) //Edit 1
        {
            string docxFile = AppDomain.CurrentDomain.BaseDirectory + "umowy/uks.docx";


            string copydocxFile = AppDomain.CurrentDomain.BaseDirectory + "umowy/backup/uks.docx";

            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "umowy/backup");
            File.Copy(docxFile, copydocxFile, true);

            try
            {
                Process.Start("cmd", $"/c start {docxFile}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("B³¹d podczas otwierania pliku uks: " + ex.Message, "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            OpenUppzForm();
        }

        private void OpenUppzForm()
        {
            if (uksListForm == null || uksListForm.IsDisposed)
            {
                uksListForm = new UksList(this);
            }
            uksListForm.ShowDialog(); // Wyœwietlanie formularza ustawieñ
        }
    }
}
