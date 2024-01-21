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

            settingsForm = new Settings(this); // Inicjalizacja formularza ustawie�

            database = new DataBase(this);

            CheckMySQLConnection();

            uksListForm = new UksList(this);
            userlistForm = new UserList(this); // Inicjalizacja formularza listy u�ytkownik�w


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
            uksListForm.ShowDialog(); // Wy�wietlanie formularza ustawie�
        }
    }
}
