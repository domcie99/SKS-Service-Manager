using System.Data;
using System.Diagnostics;

#pragma warning disable
namespace SKS_Service_Manager
{
    public partial class PrintRecords : Form
    {
        private Form1 Form1;
        private DataBase dataBase;
        private IssueUKS issueUKS;

        private string inputRaportFile;
        private string outputRaportPath;
        private string outputRaportFile;

        public PrintRecords(Form1 form1)
        {
            InitializeComponent();
            CenterToScreen();

            FromDate.Value = DateTime.Now.Date.AddDays(-29);
            ToDate.Value = DateTime.Now.Date;

            this.Form1 = form1;
            dataBase = Form1.getDataBase();
            issueUKS = new IssueUKS(-1, Form1);

            IssuedCity.Items.Insert(0, "Wszystko");
            IssuedCity.Items.AddRange(dataBase.GetUniqueCities().ToArray());

            // Pobranie domyślnego miasta z ustawień
            Settings settingsForm = new Settings(Form1);
            string defaultCity = settingsForm.GetCity();

            // Znajdź miasto, ignorując wielkość liter
            int defaultCityIndex = -1;

            for (int i = 0; i < IssuedCity.Items.Count; i++)
            {
                if (string.Equals(IssuedCity.Items[i].ToString(), defaultCity, StringComparison.OrdinalIgnoreCase))
                {
                    defaultCityIndex = i;
                    break;
                }
            }

            // Ustaw domyślne miasto, jeśli istnieje
            if (defaultCityIndex >= 0)
            {
                IssuedCity.SelectedIndex = defaultCityIndex;
            }
            else
            {
                IssuedCity.SelectedIndex = 0; // Jeśli nie ma domyślnego miasta, ustaw na "Wszystko"
            }

            DocumentType.SelectedIndex = 0;
        }

        private void print_Click(object sender, EventArgs e)
        {
            print.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            OverlayForm overlay = new OverlayForm();
            overlay.ShowOverlay(this);

            string issuedCity = IssuedCity.Text.ToString();
            string documentType = DocumentType.Text.ToString();
            DateTime fromDate = FromDate.Value.Date;
            DateTime toDate = ToDate.Value.Date;

            if (toDate <= fromDate)
            {
                MessageBox.Show("Data odbioru musi być większa niż data faktury.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ToDate.Focus();
                return;
            }

            inputRaportFile = AppDomain.CurrentDomain.BaseDirectory + "\\umowy\\backup\\ewidencja_" + fromDate.ToShortDateString() + "-" + toDate.ToShortDateString() + ".docx";
            outputRaportPath = AppDomain.CurrentDomain.BaseDirectory + "\\umowy\\Wystawione";
            outputRaportFile = AppDomain.CurrentDomain.BaseDirectory + "\\umowy\\Wystawione\\ewidencja_" + fromDate.ToShortDateString() + "-" + toDate.ToShortDateString() + ".pdf";

            DataTable dt = dataBase.uksLoadDataByDateRange(fromDate, toDate, issuedCity, documentType);

            bool onlyRealized = cbOnlyRealized.Checked;

            if (onlyRealized)
            {
                var realizedRecords = dt.AsEnumerable()
                    .Where(row =>
                    {
                        DateTime saleDate, returnDate;

                        bool saleDateParsed = DateTime.TryParse(row.Field<string>("Data sprzedaży"), out saleDate);
                        bool saleDateValid = saleDateParsed && saleDate.Year >= 1800;

                        bool returnDateParsed = DateTime.TryParse(row.Field<string>("Data zwrotu"), out returnDate);
                        bool returnDateValid = returnDateParsed && returnDate.Year >= 1800;

                        return saleDateValid || returnDateValid;
                    })
                    .CopyToDataTable();

                dt = realizedRecords;
            }


            try
            {
                issueUKS.CreateDocxFromData(dt, inputRaportFile, fromDate, toDate, documentType, onlyRealized);
                issueUKS.ConvertDocxToPdf(inputRaportFile, outputRaportPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd tworzenia raportu: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                overlay.Close();
                this.Cursor = Cursors.Default;
                print.Enabled = true;

                try
                {
                    Process.Start("cmd", $"/c start {outputRaportFile}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd podczas otwierania pliku PDF: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void FromDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void MonthsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MonthsComboBox.SelectedIndex != -1)
            {
                int selectedMonth = MonthsComboBox.SelectedIndex + 1;
                int year = DateTime.Now.Year;

                DateTime firstDayOfMonth = new DateTime(year, selectedMonth, 1);
                DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                FromDate.Value = firstDayOfMonth;
                ToDate.Value = lastDayOfMonth;
            }
        }

        private void minusButton_Click(object sender, EventArgs e)
        {
            if (MonthsComboBox.SelectedIndex == -1)
            {
                MonthsComboBox.SelectedIndex = DateTime.Now.Month - 1;
            }
            else
            {
                int currentIndex = MonthsComboBox.SelectedIndex;
                if (currentIndex > 0)
                {
                    MonthsComboBox.SelectedIndex = currentIndex - 1;
                }
            }
        }

        private void currentMonthButton_Click(object sender, EventArgs e)
        {
            MonthsComboBox.SelectedIndex = DateTime.Now.Month - 1;
        }

        private void plusButton_Click(object sender, EventArgs e)
        {
            if (MonthsComboBox.SelectedIndex == -1)
            {
                MonthsComboBox.SelectedIndex = DateTime.Now.Month - 1;
            }
            else
            {
                int currentIndex = MonthsComboBox.SelectedIndex;
                if (currentIndex < 11)
                {
                    MonthsComboBox.SelectedIndex = currentIndex + 1;
                }
            }
        }
    }
}
