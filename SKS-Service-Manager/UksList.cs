using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using MySqlConnector;
using System;
using System.Data;
using System.Data.Entity;
using System.Windows.Forms;

#pragma warning disable
namespace SKS_Service_Manager
{
    public partial class UksList : Form
    {
        private IssueUKS issueUksForm;
        private Form1 mainForm;
        private DataBase dataBase;
        private PrintRecords printRecords;

        DataTable dt;

        public UksList(Form1 form1)
        {
            InitializeComponent();
            CenterToScreen();

            mainForm = form1;
            dataBase = mainForm.getDataBase();

            issueUksForm = new IssueUKS(-1, mainForm);

            dataBase.CreateInvoicesTableIfNotExists();

            printRecords = new PrintRecords(mainForm);

            LoadData();

            dataGridView1.Columns[4].Width = 200;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        }

        public void LoadData()
        {
            dt = dataBase.uksLoadData();

            if (dt != null)
            {
                dataGridView1.DataSource = dt;
            }
        }

        public void SearchUserValueChange(object sender, EventArgs e)
        {
            string searchPhrase = search.Text.Trim(); // Pobierz frazę do wyszukiwania
            DataTable filteredUserData = dt.Clone(); // Utwórz kopię struktury userData

            foreach (DataRow row in dt.Rows)
            {
                DataRow newRow = filteredUserData.NewRow(); // Utwórz nowy wiersz w wynikowej tabeli

                foreach (DataColumn column in dt.Columns)
                {
                    if (row[column].ToString().IndexOf(searchPhrase, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        // Jeśli znaleziono dopasowanie, dodaj dane z tego wiersza do nowego wiersza w wynikowej tabeli
                        newRow.ItemArray = row.ItemArray;
                        filteredUserData.Rows.Add(newRow);
                        break; // Przeszukuj wszystkie komórki w danym wierszu
                    }
                }
            }

            // Wyświetl wyniki w DataGridView
            dataGridView1.DataSource = filteredUserData;
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

        private void print_Click(object sender, EventArgs e)
        {
            OpenPrintRecordsForm();
        }

        private void OpenPrintRecordsForm()
        {
            if (printRecords == null || printRecords.IsDisposed)
            {
                printRecords = new PrintRecords(mainForm);
            }
            printRecords.ShowDialog();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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

        private void UksList_SizeChanged(object sender, EventArgs e)
        {
            int margin = 20; // Możesz dostosować marginesy i inne wartości
            dataGridView1.Width = this.ClientSize.Width - margin;
            dataGridView1.Height = this.ClientSize.Height - 100;
        }
    }
}
