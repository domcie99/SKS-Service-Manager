using System.Data;

namespace SKS_Service_Manager
{
    public partial class EditUser : Form
    {

        private int userIdToEdit; // Identyfikator użytkownika do edycji
        private Settings settingsForm;
        private Form1 mainForm;
        private UserList parentForm;
        private DataBase database;

        public EditUser(int userID, UserList parentForm, Form1 form1)
        {
            InitializeComponent();

            mainForm = form1;
            database = mainForm.getDataBase();

            // Inicjalizacja połączenia z bazą danych (możesz użyć istniejącego połączenia z userlist)
            this.userIdToEdit = userID;
            this.parentForm = parentForm;

            settingsForm = new Settings(mainForm);

            if (userIdToEdit == -1)
            {
                // Tryb dodawania nowego użytkownika
                Text = "Dodawanie Użytkownika";
            }
            else
            {
                // Tryb edycji istniejącego użytkownika
                Text = "Edycja Użytkownika";
                LoadUserData();
            }

        }

        private void LoadUserData()
        {
            try
            {
                DataTable userData = database.loadUserData(userIdToEdit);

                if (userData != null && userData.Rows.Count > 0)
                {
                    DataRow row = userData.Rows[0]; // Pobierz pierwszy wiersz (powinien być tylko jeden)

                    FullName.Text = row["Name"].ToString();
                    Adress.Text = row["Address"].ToString();
                    Post_Code.Text = row["PostalCode"].ToString();
                    City.Text = row["City"].ToString();
                    Phone.Text = row["Phone"].ToString();
                    EMail.Text = row["Email"].ToString();
                    DocumentType.Text = row["DocumentType"].ToString();
                    DocumentNumber.Text = row["DocumentNumber"].ToString();
                    Pesel.Text = row["Pesel"].ToString();
                    Nip.Text = row["NIP"].ToString();
                    Company_Name.Text = row["Name"].ToString();
                    Notes.Text = row["Notes"].ToString();
                }
                else
                {
                    MessageBox.Show("Nie znaleziono danych użytkownika o ID: " + userIdToEdit, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas wczytywania danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Save_Click(object sender, EventArgs e)
        {
            // Pozostała część kodu jest taka sama jak wcześniej

            // Pobierz dane z kontrolek formularza
            string fullname = FullName.Text;
            string address = Adress.Text;
            string postalCode = Post_Code.Text;
            string city = City.Text;
            string phone = Phone.Text;
            string email = EMail.Text;
            string documentType = DocumentType.Text;
            string documentNumber = DocumentNumber.Text;
            string pesel = Pesel.Text;
            string nip = Nip.Text;
            string name = Company_Name.Text;
            string notes = Notes.Text;

            try
            {
                // Ustal, czy użytkownik istnieje w bazie danych
                bool userExists = database.CheckUserExistsByPesel(pesel);

                // Wywołaj metodę UpdateUserInDatabase z klasy Database
                database.UpdateUserInDatabase(userExists, fullname, name, address, postalCode, city, phone, email, documentType, documentNumber, pesel, nip, notes);

                MessageBox.Show("Dane zostały zapisane.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                parentForm.LoadUserData();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas zapisywania danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Abort_Click(object sender, EventArgs e)
        {
            // Obsługa przycisku Anuluj - zamknij formularz
            Close();
        }

    }
}
