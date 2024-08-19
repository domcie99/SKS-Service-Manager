using MySqlConnector;
using System.Data;

#pragma warning disable
namespace SKS_Service_Manager
{
    public partial class UserList : Form
    {
        private Form1 mainForm;
        private Settings settingsForm;
        public int issueUserId;
        private DataBase database;
        DataTable userData;
        public bool selectUser = false;
        private int maxRows = 35;

        public UserList(Form1 Form1)
        {
            InitializeComponent();
            CenterToScreen();

            mainForm = Form1;
            settingsForm = new Settings(mainForm);
            UpdateReference();
            LoadUserData();

            recordCount.Text = maxRows.ToString();

            // Ustawienie AutoSizeColumnsMode dla DataGridView
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        public void UpdateReference()
        {
            database = mainForm.getDataBase();
        }

        public void LoadUserData()
        {
            userData = database.GetAlphabeticalRecords("Users", "FullName");

            if (userData != null)
            {
                dataGridView1.DataSource = userData;
            }
        }


        public void SearchUserValueChange(object sender, EventArgs e)
        {
            string searchPhrase = search.Text.Trim();
            FilterData(searchPhrase);
        }

        private void FilterData(string searchValue)
        {
            string[] searchableColumns =
            {
                "FullName",
                "Address",
                "PostalCode",
                "City",
                "Phone",
                "Email",
                "DocumentType",
                "DocumentNumber",
                "NIP",
                "Name",
                "Pesel",
                "Notes"
            };

            DataTable filteredUserData = database.GetFilteredUsers(searchValue, searchableColumns, maxRows);

            if (filteredUserData != null)
            {
                dataGridView1.DataSource = filteredUserData;
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
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string dataID = dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString();
                issueUserId = int.Parse(dataID);

                setIssueVisible(false);
                DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Wybierz użytkownika.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UserList_SizeChanged(object sender, EventArgs e)
        {
            int margin = 20;
            dataGridView1.Width = this.ClientSize.Width - margin;
            dataGridView1.Height = this.ClientSize.Height - 100;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (selectUser)
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    string dataID = dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString();
                    issueUserId = int.Parse(dataID);

                    setIssueVisible(false);
                    selectUser = false;
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("Wybierz użytkownika.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int selectedUserID = int.Parse(dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString());

                    EditUser editUserForm = new EditUser(selectedUserID, this, mainForm);
                    editUserForm.ShowDialog();
                    LoadUserData();
                }
                else
                {
                    MessageBox.Show("Wybierz użytkownika do edycji.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LoadLatestRecords()
        {
            DataTable latestRecords = database.GetLatestRecords("Users", "DateColumn");
        }

        private void recordCount_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(recordCount.Text.Trim(), out int count))
            {
                maxRows = count;
                SearchUserValueChange(sender, e);
            }
            else
            {
                MessageBox.Show("Blad niewiadomo jaki", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void recordCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
