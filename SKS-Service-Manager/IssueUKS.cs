using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Humanizer;
using Microsoft.Office.Interop.Word;
using MySqlConnector;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using Body = DocumentFormat.OpenXml.Wordprocessing.Body;
using DataTable = System.Data.DataTable;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;
using Word = Microsoft.Office.Interop.Word;

namespace SKS_Service_Manager
{
    public partial class IssueUKS : Form
    {
        private Form1 mainForm;
        private int issueUserId;
        private int issueId;
        private Form1 Form1;
        private Settings settingsForm;
        private MySqlConnection connection;
        private string connectionString;
        private bool generated = false;
        private DataBase dataBase;

        string docxFilePath = AppDomain.CurrentDomain.BaseDirectory + "umowy/uks.docx";
        string editedDocxFilePath = AppDomain.CurrentDomain.BaseDirectory + "umowy/uks_new.docx";
        string pdfFilePath = AppDomain.CurrentDomain.BaseDirectory + "umowy/uks.pdf";

        public IssueUKS(int Id, Form1 form1)
        {
            InitializeComponent();
            mainForm = form1;
            settingsForm = new Settings(mainForm);
            issueId = Id;

            dataBase = mainForm.getDataBase();
            dataBase.CreateInvoicesTableIfNotExists();

            DocumentType.Items.Add("Dowód Osobisty");
            DocumentType.Items.Add("Prawo Jazdy");
            DocumentType.Items.Add("Paszport");

            if (issueId > 0)
            {
                LoadInvoiceData(issueId);
            }
        }

        private void Load_Click(object sender, EventArgs e)
        {
            // Open the userlist form to select a user
            using (UserList userListForm = new UserList(mainForm))
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
                System.Data.DataTable userData = dataBase.loadUserData(userIdToEdit);

                if (userData != null && userData.Rows.Count > 0)
                {
                    DataRow row = userData.Rows[0]; // Assuming there is only one result row

                    FullName.Text = row["Name"].ToString();
                    Adress.Text = row["Address"].ToString();
                    Post_Code.Text = row["PostalCode"].ToString();
                    City.Text = row["City"].ToString();
                    Phone.Text = row["Phone"].ToString();
                    EMail.Text = row["Email"].ToString();
                    DocumentType.Text = row["DocumentType"].ToString();
                    DocumentNumber.Text = row["DocumentNumber"].ToString();
                    Pesel.Text = row["Pesel"].ToString();
                    Nip.Text = row["NIP"].ToString(); // New NIP field
                    Notes.Text = row["Notes"].ToString();

                    // Add code to populate new fields here:
                    Days.Text = row["Days"].ToString();
                    Precentage.Text = row["Precentage"].ToString();
                    Fee.Text = row["Fee"].ToString();
                    LateFee.Text = row["LateFee"].ToString();
                    BuyAmount.Text = row["BuyAmount"].ToString();
                    DateOfReturn.Value = Convert.ToDateTime(row["DateOfReturn"]);
                    SaleDate.Value = Convert.ToDateTime(row["SaleDate"]);
                    SaleAmount.Text = row["SaleAmount"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas wczytywania danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Value_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Pozwól na tylko cyfry, kropkę, Backspace oraz Control(do kopiowania i wklejania)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != ','))
            {
                e.Handled = true;
            }

            // Upewnij się, że jest tylko jedna kropka w polu tekstowym
            if ((e.KeyChar == '.' || e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf('.') > -1 || (sender as TextBox).Text.IndexOf(',') > -1))
            {
                e.Handled = true;
            }
        }

        private void Value_Validation(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (!string.IsNullOrWhiteSpace(textBox.Text))
            {
                // Zamień kropkę na przecinek
                textBox.Text = textBox.Text.Replace(".", ",");

                // Sprawdź, czy istnieje przecinek
                if (!textBox.Text.Contains(","))
                {
                    // Jeśli brak przecinka, dodaj ".00" na końcu
                    textBox.Text += ",00";
                }
                else
                {
                    // Jeśli jest przecinek, sprawdź, czy są dwie cyfry po przecinku
                    string[] parts = textBox.Text.Split(',');
                    if (parts.Length == 2 && parts[1].Length == 1)
                    {
                        // Jeśli jest tylko jedna cyfra po przecinku, dodaj "0" na końcu
                        textBox.Text += "0";
                    }
                }

                decimal value;
                if (decimal.TryParse(textBox.Text, out value))
                {

                }
                else
                {
                    // Jeśli wartość nie jest poprawna, wyświetl komunikat o błędzie
                    MessageBox.Show("Nieprawidłowa wartość.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox.Text = "0,00"; // Ustaw wartość na domyślną lub na poprzednią
                }
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            Load.Enabled = false;
            Save.Enabled = false;
            Print.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            decimal value = decimal.Parse(Value.Text);

            int wholePart = (int)value;
            int fractionalPart = (int)((value - wholePart) * 100); // Przekształć ułamek na grosze

            string wholePartInWords = wholePart.ToWords();
            string fractionalPartInWords = fractionalPart.ToWords();

            try
            {
                // Skopiuj oryginalny plik DOCX do miejsca docelowego
                File.Copy(docxFilePath, editedDocxFilePath, true);

                // Otwieramy skopiowany dokument DOCX
                using (WordprocessingDocument doc = WordprocessingDocument.Open(editedDocxFilePath, true))
                {
                    MainDocumentPart mainPart = doc.MainDocumentPart;
                    Body body = mainPart.Document.Body;

                    ReplaceText(body, "#[firma-nazwa]", settingsForm.GetCompanyName());
                    ReplaceText(body, "#[firma-imie-nazwisko]", settingsForm.GetName() + " " + settingsForm.GetSurname());
                    ReplaceText(body, "#[firma-adres]", settingsForm.GetStreetNumber());
                    ReplaceText(body, "#[firma-kod]", settingsForm.GetPostCode());
                    ReplaceText(body, "#[firma-miasto]", settingsForm.GetCity());
                    ReplaceText(body, "#[firma-telefon]", settingsForm.GetPhone());
                    ReplaceText(body, "#[firma-nip]", settingsForm.GetNIP());
                    ReplaceText(body, "#[data-wystawienia]", Issue_Date.Value.ToString("dd-MM-yyyy"));

                    ReplaceText(body, "#[sprzedajacy-imie-nazwisko]", FullName.Text);
                    ReplaceText(body, "#[sprzedajacy-nazwa-firmy]", CompanyName.Text);
                    ReplaceText(body, "#[sprzedajacy-adres]", Adress.Text);
                    ReplaceText(body, "#[sprzedajacy-kod]", Post_Code.Text);
                    ReplaceText(body, "#[sprzedajacy-miasto]", City.Text);
                    ReplaceText(body, "#[sprzedajacy-telefon]", Phone.Text);
                    ReplaceText(body, "#[sprzedajacy-email]", EMail.Text);
                    ReplaceText(body, "#[sprzedajacy-nip]", Nip.Text);
                    ReplaceText(body, "#[sprzedajacy-rodzaj-dok]", DocumentType.Text);
                    ReplaceText(body, "#[sprzedajacy-numer-dok]", DocumentNumber.Text);
                    ReplaceText(body, "#[sprzedajacy-pesel]", Pesel.Text);
                    ReplaceText(body, "#[sprzedajacy-uwagi]", Notes.Text);

                    ReplaceText(body, "#[przedmiot-opis]", Description.Text);
                    ReplaceText(body, "#[przedmiot-wartosc]", value.ToString());
                    ReplaceText(body, "#[przedmiot-wartosc-slownie]", $"{wholePartInWords} złotych i {fractionalPartInWords} groszy");

                    ReplaceText(body, "#[przedmiot-data-przyjecia]", Issue_Date.Value.ToString("dd-MM-yyyy"));
                    ReplaceText(body, "#[przedmiot-data-odbioru]", Pickup_Date.Value.ToString("dd-MM-yyyy"));
                    ReplaceText(body, "#[przedmiot-ilosc-dni]", Days.Text);
                    ReplaceText(body, "#[przedmiot-procent]", Precentage.Text);

                    ReplaceText(body, "#[przedmiot-oplata]", Fee.Text);
                    ReplaceText(body, "#[przedmiot-oplata-opoznienia]", LateFee.Text);
                    ReplaceText(body, "#[przedmiot-kwota-wykupu]", BuyAmount.Text);

                    ReplaceText(body, "#[przedmiot-data-zwrotu]", DateOfReturn.Value.ToString("dd-MM-yyyy"));
                    ReplaceText(body, "#[przedmiot-data-sprzedazy]", SaleDate.Value.ToString("dd-MM-yyyy"));
                    ReplaceText(body, "#[przedmiot-kwota-sprzedazy]", SaleAmount.Text);

                    ReplaceText(body, "#[przedmiot-uwagi]", Comments.Text);

                    doc.Save();
                }

                ConvertDocxToPdf(editedDocxFilePath, pdfFilePath);
                MessageBox.Show("Dokument został wygenerowany, można drukować", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas generowania dokumentu: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                Load.Enabled = true;
                Save.Enabled = true;
                Print.Enabled = true;
                generated = true;
                if (issueId > 0)
                {
                    // Aktualizuj dane faktury w bazie danych
                    UpdateInvoiceInDatabase(issueId);
                    UpdateUserInDatabase(dataBase.CheckUserExistsByPesel(Pesel.Text)); ;
                }
            }
        }

        private void ReplaceText(Body body, string findText, string replaceText)
        {
            foreach (Text text in body.Descendants<Text>())
            {
                if (text.Text.Contains(findText))
                {
                    text.Text = text.Text.Replace(findText, replaceText);
                }
            }
        }

        private void ConvertDocxToPdf(string docxFilePath, string pdfFilePath)
        {
            // Create an instance of Word.exe
            _Application oWord = new Word.Application
            {

                // Make this instance of word invisible (Can still see it in the taskmgr).
                Visible = false
            };

            // Interop requires objects.
            object oMissing = System.Reflection.Missing.Value;
            object isVisible = true;
            object readOnly = true;     // Does not cause any word dialog to show up
            //object readOnly = false;  // Causes a word object dialog to show at the end of the conversion
            object oInput = docxFilePath;
            object oOutput = pdfFilePath;
            object oFormat = WdSaveFormat.wdFormatPDF;

            // Load a document into our instance of word.exe
            _Document oDoc = oWord.Documents.Open(
                ref oInput, ref oMissing, ref readOnly, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref isVisible, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                );

            // Make this document the active document.
            oDoc.Activate();

            // Save this document using Word
            oDoc.SaveAs(ref oOutput, ref oFormat, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing
                );

            // Always close Word.exe.
            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
        }

        private void SaveInvoiceToDatabase(int userId)
        {
            try
            {
                System.Data.DataTable invoiceData = new System.Data.DataTable();
                invoiceData.Columns.Add("UserID", typeof(int));
                invoiceData.Columns.Add("City", typeof(string));
                invoiceData.Columns.Add("Description", typeof(string));
                invoiceData.Columns.Add("TotalAmount", typeof(decimal));
                invoiceData.Columns.Add("InvoiceDate", typeof(DateTime));
                invoiceData.Columns.Add("Notes", typeof(string));

                // Add new fields to the DataTable:
                invoiceData.Columns.Add("NIP", typeof(string));
                invoiceData.Columns.Add("Days", typeof(int));
                invoiceData.Columns.Add("Precentage", typeof(int));
                invoiceData.Columns.Add("Fee", typeof(decimal));
                invoiceData.Columns.Add("LateFee", typeof(decimal));
                invoiceData.Columns.Add("BuyAmount", typeof(decimal));
                invoiceData.Columns.Add("DateOfReturn", typeof(DateTime));
                invoiceData.Columns.Add("SaleDate", typeof(DateTime));
                invoiceData.Columns.Add("SaleAmount", typeof(decimal));

                DataRow newRow = invoiceData.NewRow();
                newRow["UserID"] = userId;
                newRow["City"] = settingsForm.GetCity();
                newRow["Description"] = Description.Text;
                newRow["TotalAmount"] = decimal.Parse(Value.Text);
                newRow["InvoiceDate"] = Issue_Date.Value.Date;
                newRow["Notes"] = Comments.Text;

                // Add values for the new fields:
                newRow["NIP"] = Nip.Text;
                newRow["Days"] = int.Parse(Days.Text);
                newRow["Precentage"] = int.Parse(Precentage.Text);
                newRow["Fee"] = decimal.Parse(Fee.Text);
                newRow["LateFee"] = decimal.Parse(LateFee.Text);
                newRow["BuyAmount"] = decimal.Parse(BuyAmount.Text);
                newRow["DateOfReturn"] = DateOfReturn.Value.Date;
                newRow["SaleDate"] = SaleDate.Value.Date;
                newRow["SaleAmount"] = decimal.Parse(SaleAmount.Text);

                invoiceData.Rows.Add(newRow);

                dataBase.SaveInvoiceToDatabase(invoiceData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas zapisywania faktury do bazy danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Print_Click(object sender, EventArgs e)
        {
            if (generated)
            {
                // Wyświetl MessageBox z pytaniem
                DialogResult result = MessageBox.Show("Czy wszystkie dane się zgadzają?", "Potwierdzenie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    DialogResult result2 = MessageBox.Show("Czy chcesz zapisać plik pdf w katalogu?", "Potwierdzenie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        string savedpdfFilePath = AppDomain.CurrentDomain.BaseDirectory + "/umowy/Wystawione/uks_" + issueId + ".pdf";
                        Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/umowy/Wystawione");
                        File.Copy(pdfFilePath, savedpdfFilePath, true);
                    }

                    if (issueId < 0)
                    {
                        // Aktualizuj dane faktury w bazie danych
                        UpdateUserInDatabase(dataBase.CheckUserExistsByPesel(Pesel.Text));
                        SaveInvoiceToDatabase(issueUserId); ;
                    }

                    try
                    {
                        Process.Start("cmd", $"/c start {pdfFilePath}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Błąd podczas otwierania pliku PDF: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    generated = false;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Musisz pierw wygenerować dokument guzikiem Zapisz", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateUserInDatabase(bool exist)
        {
            // Pobierz dane z pól formularza, które chcesz zaktualizować
            string fullName = FullName.Text;
            string name = CompanyName.Text;
            string address = Adress.Text;
            string postalCode = Post_Code.Text;
            string city = City.Text;
            string phone = Phone.Text;
            string email = EMail.Text;
            string documentType = DocumentType.Text;
            string documentNumber = DocumentNumber.Text;
            string pesel = Pesel.Text;
            string nip = Nip.Text;
            string notes = Notes.Text;

            // Sprawdź, czy użytkownik o danym numerze PESEL istnieje
            bool userExists = dataBase.CheckUserExistsByPesel(pesel);

            // Wywołaj funkcję UpdateUserInDatabase, przekazując odpowiednie parametry
            dataBase.UpdateUserInDatabase(userExists, fullName, name, address, postalCode, city, phone, email, documentType, documentNumber, pesel, nip, notes);

        }

        private void LoadInvoiceData(int invoiceId)
        {
            DataTable invoiceData = dataBase.LoadInvoiceData(invoiceId);

            if (invoiceData != null)
            {
                DataRow row = invoiceData.Rows[0];

                Description.Text = row["Description"].ToString();
                Value.Text = row["TotalAmount"].ToString();
                Issue_Date.Value = Convert.ToDateTime(row["InvoiceDate"]);
                Comments.Text = row["Notes"].ToString();
                issueUserId = Convert.ToInt32(row["USERID"]);

                LoadUserData(issueUserId);
            }
            else
            {
                MessageBox.Show("Nie można znaleźć danych faktury.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateInvoiceInDatabase(int invoiceId)
        {
            string city = City.Text;
            string description = Description.Text;
            decimal totalAmount = decimal.Parse(Value.Text);
            DateTime invoiceDate = Issue_Date.Value.Date;
            string notes = Comments.Text;

            dataBase.UpdateInvoiceInDatabase(invoiceId, city, description, totalAmount, invoiceDate, notes);
        }

        private void IsInt_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            // Pozwól na tylko cyfry, Backspace i klawisz Delete
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void Percentage_Leave(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            int newValue = int.Parse(textBox.Text.ToString());
            // Sprawdź, czy wartość mieści się w zakresie od 0 do 100
            if (newValue < 0)
            {
                newValue = 0;
            }
            else if (newValue > 100)
            {
                newValue = 100;
            }

            // Uaktualnij pole tekstowe z nową wartością
            textBox.Text = newValue.ToString();
        }

        private void Issue_Date_ValueChanged(object sender, EventArgs e)
        {
            DateTime issueDate = Issue_Date.Value;
            DateTime pickupDate = Pickup_Date.Value;

            // Oblicz liczbę dni pomiędzy datami
            TimeSpan span = pickupDate - issueDate;
            int daysDifference = span.Days + 1;

            if(pickupDate==issueDate)
            {
                daysDifference = 0;
            }
            // Wpisz liczbę dni w pole Days
            Days.Text = daysDifference.ToString();
        }
    }
}
