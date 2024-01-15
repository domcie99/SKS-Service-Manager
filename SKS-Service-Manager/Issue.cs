using MySqlConnector;

namespace SKS_Service_Manager
{
    public partial class Issue : Form
    {

        private Form1 mainForm;
        private int issueUserId;
        private Form1 Form1;
        private Settings settingsForm;
        private MySqlConnection connection;
        private string connectionString;

        public Issue(int Id, Form1 mainForm)
        {
            InitializeComponent();

            settingsForm = new Settings(mainForm);
            connectionString = $"Server={settingsForm.GetMySQLHost()};Port={settingsForm.GetMySQLPort()};Database={settingsForm.GetMySQLDatabase()};User ID={settingsForm.GetMySQLUser()};Password={settingsForm.GetMySQLPassword()};";
            connection = new MySqlConnection(connectionString);

        }

        private void Load_Click(object sender, EventArgs e)
        {
            // Open the userlist form to select a user
            using (userlist userListForm = new userlist(Form1))
            {
                userListForm.setIssueVisible(true);
                if (userListForm.ShowDialog() == DialogResult.OK)
                {
                    // Retrieve the selected user data from userlist form
                    issueUserId = userListForm.issueUserId;

                    LoadUserData(issueUserId);
                }
            }
        }

        private void LoadUserData(int userIdToEdit)
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
                    Name.Text = reader["Name"].ToString();
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

        private void Value_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Pozwól na tylko cyfry, kropkę, Backspace oraz Control (do kopiowania i wklejania)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // Upewnij się, że jest tylko jedna kropka w polu tekstowym
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
    }
}
