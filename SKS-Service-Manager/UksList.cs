using MySqlConnector;
using System;
using System.Data;
using System.Data.Entity;
using System.Windows.Forms;

namespace SKS_Service_Manager
{
    public partial class UksList : Form
    {
        private IssueUKS issueUksForm;
        private Form1 mainForm;
        private DataBase dataBase;

        public UksList(Form1 form1)
        {
            InitializeComponent();
            mainForm = form1;
            dataBase = mainForm.getDataBase();

            issueUksForm = new IssueUKS(-1, mainForm);

            dataBase.CreateInvoicesTableIfNotExists();

            LoadData();
        }

        public void LoadData()
        {
            DataTable dt = dataBase.uksLoadData();

            if (dt != null)
            {
                dataGridView1.DataSource = dt;
            }
        }

        private void Add_Click(object sender, EventArgs e)
        {
            OpenIssueUKSForm(1);
            LoadData();
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
                string cellValue = dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString();

                int selectedissueID = int.Parse(cellValue);

                IssueUKS editForm = new IssueUKS(selectedissueID, mainForm);

                // Otwieramy formularz w trybie edycji
                editForm.ShowDialog();
                LoadData();
            }
            else
            {
                MessageBox.Show("Proszę najpierw wybrać fakturę UKS do edycji.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Pobierz ID wybranej faktury UKS
                int selectedIssueID = int.Parse(dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString());

                // Wyświetl potwierdzenie usuwania
                DialogResult result = MessageBox.Show("Czy na pewno chcesz usunąć tę fakturę UKS?", "Potwierdź Usunięcie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Usuń fakturę UKS z bazy danych
                    if (dataBase.DeleteUks(selectedIssueID))
                    {
                        MessageBox.Show("Faktura UKS została pomyślnie usunięta.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Odśwież dane w dataGridView po usunięciu
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Błąd podczas usuwania faktury UKS.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Proszę najpierw wybrać fakturę UKS do usunięcia.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
