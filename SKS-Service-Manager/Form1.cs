using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MySqlConnector;

namespace SKS_Service_Manager
{
    public partial class Form1 : Form
    {
        private Settings settingsForm; // Deklaracja zmiennej do przechowywania formularza ustawieñ
        private UserList userlistForm;// Deklaracja zmiennej do przechowywania formularza listy u¿ytkowników
        private IssueUKS IssueForm;// Deklaracja zmiennej do przechowywania formularza listy u¿ytkowników
        private UksList uksListForm;

        public Form1()
        {
            InitializeComponent();

            settingsForm = new Settings(this); // Inicjalizacja formularza ustawieñ

            CheckMySQLConnection();

            uksListForm = new UksList(this);
            userlistForm = new UserList(this); // Inicjalizacja formularza listy u¿ytkowników

            IssueForm = new IssueUKS(-1, this);
        }

        private void button1_Click(object sender, EventArgs e) //Issue 1
        {
            OpenUksForm();
        }
        private void button4_Click(object sender, EventArgs e) //Edit 1
        {
            string docxFile = AppDomain.CurrentDomain.BaseDirectory + "umowy/uks.docx";
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
            string host = settingsForm.GetMySQLHost();
            string user = settingsForm.GetMySQLUser();
            string password = settingsForm.GetMySQLPassword();
            string database = settingsForm.GetMySQLDatabase();
            int port = settingsForm.GetMySQLPort();

            string connectionString = $"Server={host};Port={port};Database={database};User ID={user};Password={password};";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    if (connection.State == System.Data.ConnectionState.Open)
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
            }
            catch (MySqlException ex)
            {
                // Ustawienie obrazu na no-connection.png
                pictureBox1.Image = Properties.Resources.no_connection;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                MessageBox.Show($"Wyst¹pi³ b³¹d po³¹czenia z baz¹ danych: \n {ex.Message} \nSprawdŸ ustawienia po³¹czenia z baz¹ danych!");
                // Otwieranie formularza ustawieñ w przypadku b³êdu
                OpenSettingsForm();
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
    }
}
