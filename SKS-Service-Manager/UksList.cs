using DocumentFormat.OpenXml.Bibliography;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SKS_Service_Manager
{
    public partial class UksList : Form
    {
        private IssueUKS issueUksForm;
        private Form1 mainForm;

        private MySqlConnection connection;
        private string connectionString;
        private Settings settingsForm;

        public UksList(Form1 mainForm)
        {
            InitializeComponent();

            settingsForm = new Settings(mainForm);
            connectionString = $"Server={settingsForm.GetMySQLHost()};Port={settingsForm.GetMySQLPort()};Database={settingsForm.GetMySQLDatabase()};User ID={settingsForm.GetMySQLUser()};Password={settingsForm.GetMySQLPassword()};";
            connection = new MySqlConnection(connectionString);

            issueUksForm = new IssueUKS(-1, mainForm);

            CreateInvoicesTableIfNotExists();
        }

        private void CreateInvoicesTableIfNotExists()
        {
            try
            {
                connection.Open();

                string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS UKS (
                ID INT AUTO_INCREMENT PRIMARY KEY,
                UserID INT,
                City VARCHAR(255),
                Description TEXT,
                TotalAmount DECIMAL(10, 2),
                InvoiceDate DATE,
                Notes TEXT
            );";

                MySqlCommand cmd = new MySqlCommand(createTableQuery, connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas tworzenia tabeli UKS: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }

            LoadData();
        }

        public void LoadData()
        {
            try
            {
                connection.Open();

                string query = "SELECT UKS.ID, " +
                    "UKS.City AS 'Miasto Wystawienia', " +
                    "UKS.Description AS 'Opis', " +
                    "UKS.TotalAmount AS 'Wartość', " +
                    "UKS.InvoiceDate AS 'Data Wystawienia', " +
                    "UKS.Notes AS 'Notatki', " +

                    "Users.Name AS 'Imię Nazwisko', " +
                    "Users.Address AS 'Ulica Numer', " +
                    "Users.PostalCode AS 'Kod Pocztowy', " +
                    "Users.City AS 'Miasto', " +
                    "Users.Phone AS 'Telefon', " +
                    "Users.Pesel AS 'Pesel', " +
                    "Users.NIP AS 'NIP'" +

                    "FROM UKS " +
                    "INNER JOIN Users ON UKS.UserID = Users.ID;";

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
            OpenIssueUKSForm(1);
        }

        private void OpenIssueUKSForm(int Id)
        {
            if (issueUksForm == null || issueUksForm.IsDisposed)
            {
                issueUksForm = new IssueUKS(Id, mainForm);
            }
            issueUksForm.ShowDialog();
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            // Sprawdź, czy użytkownik wybrał fakturę UKS do edycji
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Tworzymy nowy formularz IssueUKS w trybie edycji
                int selectedissueID = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;

                IssueUKS editForm = new IssueUKS(selectedissueID, mainForm);

                // Otwieramy formularz w trybie edycji
                editForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Proszę najpierw wybrać fakturę UKS do edycji.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
 