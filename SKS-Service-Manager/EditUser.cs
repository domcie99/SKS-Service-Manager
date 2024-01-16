using MySqlConnector;
using System;
using System.Windows.Forms;

namespace SKS_Service_Manager
{
    public partial class EditUser : Form
    {
        private MySqlConnection connection;
        private string connectionString;
        private int userIdToEdit; // Identyfikator użytkownika do edycji
        private Settings settingsForm;
        private Form1 mainForm;
        private UserList parentForm;

        public EditUser(int userID, UserList parentForm)
        {
            InitializeComponent();


            DocumentType.Items.Add("Dowód Osobisty");
            DocumentType.Items.Add("Prawo Jazdy");
            DocumentType.Items.Add("Paszport");


            // Inicjalizacja połączenia z bazą danych (możesz użyć istniejącego połączenia z userlist)
            this.userIdToEdit = userID;
            this.parentForm = parentForm;

            settingsForm = new Settings(mainForm);
            connectionString = $"Server={settingsForm.GetMySQLHost()};Port={settingsForm.GetMySQLPort()};Database={settingsForm.GetMySQLDatabase()};User ID={settingsForm.GetMySQLUser()};Password={settingsForm.GetMySQLPassword()};";
            connection = new MySqlConnection(connectionString);

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
                connection.Open();

                // Wczytaj dane użytkownika o identyfikatorze userIdToEdit z bazy danych
                string query = "SELECT * FROM Users WHERE ID = @UserID;";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserID", userIdToEdit);

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    FullName.Text = reader["Name"].ToString();
                    Adress.Text = reader["Address"].ToString();
                    Post_Code.Text = reader["PostalCode"].ToString();
                    City.Text = reader["City"].ToString();
                    Phone.Text = reader["Phone"].ToString();
                    EMail.Text = reader["Email"].ToString();
                    DocumentType.Text = reader["DocumentType"].ToString();
                    DocumentNumber.Text = reader["DocumentNumber"].ToString();
                    Pesel.Text = reader["Pesel"].ToString();
                    Nip.Text = reader["NIP"].ToString(); // Nowe pole NIP
                    Notes.Text = reader["Notes"].ToString();
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas wczytywania danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            // Pozostała część kodu jest taka sama jak wcześniej

            // Pobierz dane z kontrolek formularza
            string name = FullName.Text;
            string address = Adress.Text;
            string postalCode = Post_Code.Text;
            string city = City.Text;
            string phone = Phone.Text;
            string email = EMail.Text;
            string documentType = DocumentType.Text;
            string documentNumber = DocumentNumber.Text;
            string pesel = Pesel.Text;
            string nip = Nip.Text; // Nowe pole NIP
            string notes = Notes.Text;

            try
            {
                connection.Open();

                if (userIdToEdit == -1)
                {
                    // Tryb dodawania nowego użytkownika
                    string insertQuery = @"
                    INSERT INTO Users (Name, Address, PostalCode, City, Phone, Email, DocumentType, DocumentNumber, Pesel, NIP, Notes)
                    VALUES (@Name, @Address, @PostalCode, @City, @Phone, @Email, @DocumentType, @DocumentNumber, @Pesel, @NIP, @Notes);";

                    MySqlCommand cmd = new MySqlCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@PostalCode", postalCode);
                    cmd.Parameters.AddWithValue("@City", city);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@DocumentType", documentType);
                    cmd.Parameters.AddWithValue("@DocumentNumber", documentNumber);
                    cmd.Parameters.AddWithValue("@Pesel", pesel);
                    cmd.Parameters.AddWithValue("@NIP", nip); // Nowe pole NIP
                    cmd.Parameters.AddWithValue("@Notes", notes);

                    cmd.ExecuteNonQuery();
                }
                else
                {
                    // Tryb edycji istniejącego użytkownika
                    string updateQuery = @"
                    UPDATE Users
                    SET Name = @Name, Address = @Address, PostalCode = @PostalCode, City = @City,
                        Phone = @Phone, Email = @Email, DocumentType = @DocumentType, DocumentNumber = @DocumentNumber,
                        Pesel = @Pesel, NIP = @NIP, Notes = @Notes
                    WHERE ID = @UserID;";

                    MySqlCommand cmd = new MySqlCommand(updateQuery, connection);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@PostalCode", postalCode);
                    cmd.Parameters.AddWithValue("@City", city);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@DocumentType", documentType);
                    cmd.Parameters.AddWithValue("@DocumentNumber", documentNumber);
                    cmd.Parameters.AddWithValue("@Pesel", pesel);
                    cmd.Parameters.AddWithValue("@NIP", nip); // Nowe pole NIP
                    cmd.Parameters.AddWithValue("@Notes", notes);
                    cmd.Parameters.AddWithValue("@UserID", userIdToEdit);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Dane zostały zapisane.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                parentForm.LoadData();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas zapisywania danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void Abort_Click(object sender, EventArgs e)
        {
            // Obsługa przycisku Anuluj - zamknij formularz
            Close();
        }

        private bool CheckUserExistsByPesel(string pesel)
        {
            try
            {
                connection.Open();

                // Sprawdź, czy istnieje użytkownik o danym numerze PESEL
                string query = "SELECT COUNT(*) FROM Users WHERE Pesel = @Pesel;";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Pesel", pesel);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas sprawdzania użytkownika w bazie danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

    }
}
