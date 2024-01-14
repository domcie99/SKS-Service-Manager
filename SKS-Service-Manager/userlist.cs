using MySqlConnector;
using System;
using System.Data;
using System.Windows.Forms;

namespace SKS_Service_Manager
{
    public partial class userlist : Form
    {
        private MySqlConnection connection;
        private string connectionString;
        private Form1 mainForm;
        private Settings settingsForm;

        public userlist(Form1 mainForm)
        {
            InitializeComponent();

            settingsForm = new Settings(mainForm);

            // Inicjalizacja połączenia z bazą danych
            connectionString = $"Server={settingsForm.GetMySQLHost()};Port={settingsForm.GetMySQLPort()};Database={settingsForm.GetMySQLDatabase()};User ID={settingsForm.GetMySQLUser()};Password={settingsForm.GetMySQLPassword()};";
            connection = new MySqlConnection(connectionString);

            userlist_Load();
        }

        private void userlist_Load()
        {
            // Tworzenie tabeli w bazie danych, jeśli nie istnieje
            try
            {
                connection.Open();

                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        ID INT AUTO_INCREMENT PRIMARY KEY,
                        Name VARCHAR(255),
                        Nip VARCHAR(255),
                        Address VARCHAR(255),
                        PostalCode VARCHAR(10),
                        City VARCHAR(255),
                        Phone VARCHAR(20),
                        Email VARCHAR(255),
                        DocumentType VARCHAR(50),
                        DocumentNumber VARCHAR(20),
                        Pesel VARCHAR(11),
                        Notes TEXT
                    );";

                MySqlCommand cmd = new MySqlCommand(createTableQuery, connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas tworzenia tabeli: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            // Wczytywanie danych z bazy danych
            LoadData();
        }

        public void LoadData()
        {
            try
            {
                connection.Open();

                string query = "SELECT * FROM Users;";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas odczytu danych: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
            // Otwórz formularz EditUser w trybie dodawania (userIdToEdit = -1)
            EditUser addUserForm = new EditUser(-1, this);
            addUserForm.ShowDialog();
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            // Sprawdź, czy użytkownik wybrał wiersz w DataGridView
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Pobierz ID wybranego wiersza
                int selectedUserID = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;

                // Otwórz formularz EditUser w trybie edycji (przekazując ID użytkownika i referencję do formularza userlist)
                EditUser editUserForm = new EditUser(selectedUserID, this);
                editUserForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Wybierz użytkownika do edycji.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    connection.Open();

                    // Pobierz ID wybranego wiersza
                    int selectedRowID = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;

                    // Usuń rekord o danym ID z bazy danych
                    string deleteQuery = "DELETE FROM Users WHERE ID = @ID;";
                    MySqlCommand cmd = new MySqlCommand(deleteQuery, connection);
                    cmd.Parameters.AddWithValue("@ID", selectedRowID);
                    cmd.ExecuteNonQuery();

                    // Usuń zaznaczony wiersz z DataGridView
                    dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);

                    MessageBox.Show("Rekord został usunięty.", "Usuwanie", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd podczas usuwania rekordu: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Wybierz wiersz do usunięcia.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
