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
        private Form1 mainForm;
        private DataBase dataBase;
        private IssueUKS issueUksForm;
        private Settings settingsForm;
        private PrintRecords printRecords;

        private int maxRows = 35;

        DataTable dt;

        public UksList(Form1 form1)
        {
            InitializeComponent();
            CenterToScreen();

            mainForm = form1;
            dataBase = mainForm.getDataBase();

            settingsForm = new Settings(mainForm);
            printRecords = new PrintRecords(mainForm);
            issueUksForm = new IssueUKS(-1, mainForm);

            IssuedCity.Items.Insert(0, "Wszystko");
            IssuedCity.Items.AddRange(dataBase.GetUniqueCities().ToArray());

            FormType.Items.Insert(0, "Wszystko");
            FormType.SelectedIndex = 0;

            recordCount.Text = maxRows.ToString();

            SetDefaultCity();

            dataBase.CreateInvoicesTableIfNotExists();

            LoadData();

            dataGridView1.Columns[4].Width = 200;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        }

        private void SetDefaultCity()
        {
            int mainCity = -1;
            string defaultCity = settingsForm.GetCity();

            for (int i = 0; i < IssuedCity.Items.Count; i++)
            {
                if (string.Equals(IssuedCity.Items[i].ToString(), defaultCity, StringComparison.OrdinalIgnoreCase))
                {
                    mainCity = i;
                    break;
                }
            }

            if (mainCity >= 0)
            {
                IssuedCity.SelectedIndex = mainCity;
            }
            else
            {
                IssuedCity.SelectedIndex = 0; 
            }
        }

        public void LoadData()
        {
            dt = dataBase.GetLatestRecords("UKS", "InvoiceDate");
            GridInsert();
        }

        public void GridInsert()
        {
            if (dt != null)
            {
                string selectedCity = IssuedCity.SelectedItem != null ? IssuedCity.SelectedItem.ToString() : "Wszystko";
                string selectedFormType = FormType.SelectedItem != null ? FormType.SelectedItem.ToString() : "Wszystko";
                string searchPhrase = search.Text.Trim();

                var filters = new Dictionary<string, string>();

                if (selectedCity != "Wszystko")
                {
                    filters.Add("City", selectedCity);
                }

                if (selectedFormType != "Wszystko")
                {
                    filters.Add("DocumentType", selectedFormType);
                }

                string[] searchableColumns =
                {
                    "UKS.ID",
                    "UKS.City",
                    "UKS.DocumentType",
                    "UKS.Description",
                    "UKS.InvoiceDate",
                    "Users.FullName",
                    "Users.Address",
                    "UKS.Notes",
                    "Users.Pesel",
                    "Users.Phone"
                };

                dt = dataBase.GetFilteredRecords("UKS", filters, searchPhrase, searchableColumns, maxRows);

                DataView dv = dt.DefaultView;
                dv.Sort = "[Data Wystawienia] DESC";  // Używaj aliasu z zapytania SQL
                dt = dv.ToTable();

                if (dt.Rows.Count > maxRows)
                {
                    DataTable limitedDataTable = dt.AsEnumerable().Take(maxRows).CopyToDataTable();
                    dataGridView1.DataSource = limitedDataTable;
                }
                else
                {
                    dataGridView1.DataSource = dt;
                }
            }
        }



        public void SearchUserValueChange(object sender, EventArgs e)
        {
            GridInsert();
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

        private void delete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int selectedIssueID = int.Parse(dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString());

                DialogResult result = MessageBox.Show("Czy na pewno chcesz usunąć ten wpis?", "Potwierdź Usunięcie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (dataBase.DeleteUks(selectedIssueID))
                    {
                        MessageBox.Show("Wpis został pomyślnie usunięty.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Błąd podczas usuwania wpisu.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Wpis nie istnieje jeszcze w bazie danych.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            Edit_Click(sender, e);
        }

        private void Edit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string cellValue = dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString();

                int selectedissueID = int.Parse(cellValue);

                IssueUKS editForm = new IssueUKS(selectedissueID, mainForm);

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
            int margin = 20;
            dataGridView1.Width = this.ClientSize.Width - margin;
            dataGridView1.Height = this.ClientSize.Height - 100;
        }

        private void IssuedCity_TextChanged(object sender, EventArgs e)
        {
            GridInsert();
        }

        private void search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchUserValueChange(sender, e);
            }
        }
        private void recordCount_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(recordCount.Text.Trim(), out int count))
            {
                maxRows = count;
                GridInsert();
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
