using System;
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
        private userlist userlistForm;// Deklaracja zmiennej do przechowywania formularza listy u�ytkownik�w
        private Issue IssueForm;// Deklaracja zmiennej do przechowywania formularza listy u�ytkownik�w

        public Form1()
        {
            InitializeComponent();
            settingsForm = new Settings(this); // Inicjalizacja formularza ustawie�
            userlistForm = new userlist(this); // Inicjalizacja formularza listy u�ytkownik�w
            IssueForm = new Issue(-1, this);
            CheckMySQLConnection(); // Sprawdzanie po��czenia MySQL przy starcie aplikacji
        }

        private void button1_Click(object sender, EventArgs e) //Issue 1
        {
            OpenIssueForm(1);
        }
        private void button4_Click(object sender, EventArgs e) //Edit 1
        {
            // �cie�ka do pliku szablonu w folderze wyj�ciowym aplikacji
            string reportPath = Path.Combine(Application.StartupPath, "Umowa-Kupna-Sprzedazy.docx");
            string outputPath = Path.Combine(Application.StartupPath, "Zakladki.txt");

            try
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(reportPath, false))
                {
                    using (StreamWriter writer = new StreamWriter(outputPath))
                    {
                        var bookmarks = wordDoc.MainDocumentPart.RootElement.Descendants<BookmarkStart>();
                        foreach (var bookmark in bookmarks)
                        {
                            writer.WriteLine("Zak�adka: " + bookmark.Name); // Zapisz do pliku
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wyst�pi� b��d: " + ex.Message);
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
                // OpenSettingsForm();
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

        private void OpenIssueForm(int Id)
        {
            if (IssueForm == null || IssueForm.IsDisposed)
            {
                IssueForm = new Issue(Id, this);
            }
            IssueForm.ShowDialog(); // Wy�wietlanie formularza ustawie�
        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Tworzenie nowego formularza listy u�ytkownik�w i przekazanie do niego referencji do formy Settings
            if (userlistForm == null || userlistForm.IsDisposed)
            {
                userlistForm = new userlist(this); // Tworzenie nowego formularza ustawie�, je�li nie istnieje lub zosta� zamkni�ty
            }
            userlistForm.Show(); // Wy�wietlenie formularza listy u�ytkownik�w
        }
    }
}
