using System.Data;

namespace SKS_Service_Manager
{
    public partial class EditUser : Form
    {

        private int userIdToEdit;
        private Settings settingsForm;
        private Form1 mainForm;
        private UserList parentForm;
        private DataBase database;

        public EditUser(int userID, UserList parentForm, Form1 form1)
        {
            InitializeComponent();
            CenterToScreen();
            mainForm = form1;
            database = mainForm.getDataBase();

            this.userIdToEdit = userID;
            this.parentForm = parentForm;

            settingsForm = new Settings(mainForm);

            if (userIdToEdit == -1)
            {
                Text = "Dodawanie Użytkownika";
            }
            else
            {
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
                    DataRow row = userData.Rows[0];

                    FullName.Text = row["FullName"].ToString();
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
            if (!ValidateFields())
            {
                return;
            }
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
                int userId = database.CheckUserExists(pesel, documentNumber, address, city, fullname);

                database.UpdateUserInDatabase(userId, fullname, name, address, postalCode, city, phone, email, documentType, documentNumber, pesel, nip, notes);

                MessageBox.Show("Dane zostały zapisane.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                parentForm.LoadUserData();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas zapisywania danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(FullName.Text))
            {
                MessageBox.Show("Pole 'Imię i Nazwisko' jest wymagane.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FullName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(Adress.Text) || Adress.Text.Equals("ul. ", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Pole 'Ulica i Numer' jest wymagane.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Adress.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(Post_Code.Text))
            {
                MessageBox.Show("Pole 'Kod Pocztowy' jest wymagane.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Post_Code.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(City.Text))
            {
                MessageBox.Show("Pole 'Miasto' jest wymagane.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                City.Focus();
                return false;
            }
            return true;
        }

        private void Abort_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
