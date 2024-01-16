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
        private Settings settingsForm; // Deklaracja zmiennej do przechowywania formularza ustawie�
        private UserList userlistForm;// Deklaracja zmiennej do przechowywania formularza listy u�ytkownik�w
        private IssueUKS IssueForm;// Deklaracja zmiennej do przechowywania formularza listy u�ytkownik�w
        private UksList uksListForm;

        public Form1()
        {
            InitializeComponent();

            settingsForm = new Settings(this); // Inicjalizacja formularza ustawie�

            CheckMySQLConnection();

            uksListForm = new UksList(this);
            userlistForm = new UserList(this); // Inicjalizacja formularza listy u�ytkownik�w

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
                MessageBox.Show("B��d podczas otwierania pliku uks: " + ex.Message, "B��d", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenSettingsForm();
        }

        // Metoda do sprawdzania po��czenia z baz� danych MySQL
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
                MessageBox.Show($"Wyst�pi� b��d po��czenia z baz� danych: \n {ex.Message} \nSprawd� ustawienia po��czenia z baz� danych!");
                // Otwieranie formularza ustawie� w przypadku b��du
                OpenSettingsForm();
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
    }
}
