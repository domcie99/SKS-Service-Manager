using MySqlConnector;
using System.Data;

namespace SKS_Service_Manager
{
    public partial class UserList : Form
    {
        private MySqlConnection connection;
        private string connectionString;
        private Form1 mainForm;
        private Settings settingsForm;
        public int issueUserId;
        private DataBase database;

        public UserList(Form1 Form1)
        {
            InitializeComponent();
            mainForm = Form1;
            settingsForm = new Settings(mainForm);

            UpdateReference();

            // Inicjalizacja połączenia z bazą danych

            LoadUserData();
        }

        public void UpdateReference()
        {
            database = mainForm.getDataBase();
        }

        public void LoadUserData()
        {
            DataTable userData = database.LoadAllUserData();

            if (userData != null)
            {
                dataGridView1.DataSource = userData;
            }
        }


        private void Add_Click(object sender, EventArgs e)
        {
            // Otwórz formularz EditUser w trybie dodawania (userIdToEdit = -1)
            EditUser addUserForm = new EditUser(-1, this, mainForm);
            addUserForm.ShowDialog();
            LoadUserData();
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            // Sprawdź, czy użytkownik wybrał wiersz w DataGridView
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Pobierz ID wybranego wiersza
                int selectedUserID = int.Parse(dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString());

                // Otwórz formularz EditUser w trybie edycji (przekazując ID użytkownika i referencję do formularza userlist)
                EditUser editUserForm = new EditUser(selectedUserID, this, mainForm);
                editUserForm.ShowDialog();
                LoadUserData();
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
                int selectedRowID = int.Parse(dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString());
                database.DeleteUserFromList(selectedRowID);

                // Usuń zaznaczony wiersz z DataGridView
                LoadUserData();

                MessageBox.Show("Rekord został usunięty.", "Usuwanie", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Wybierz wiersz do usunięcia.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void setIssueVisible(bool visible)
        {
            if (visible)
            {
                button1.Visible = true;
                label1.Visible = true;

            }
            else
            {
                button1.Visible = false;
                label1.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Sprawdź, czy użytkownik wybrał wiersz w DataGridView
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Pobierz ID wybranego wiersza
                string dataID = dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString();
                issueUserId = int.Parse(dataID);

                setIssueVisible(false);
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Wybierz użytkownika do edycji.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
