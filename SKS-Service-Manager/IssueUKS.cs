using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Humanizer;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using Body = DocumentFormat.OpenXml.Wordprocessing.Body;
using Color = DocumentFormat.OpenXml.Wordprocessing.Color;
using DataTable = System.Data.DataTable;
using Text = DocumentFormat.OpenXml.Wordprocessing.Text;

namespace SKS_Service_Manager
{
    public partial class IssueUKS : Form
    {
        private Form1 mainForm;
        private int issueUserId;
        private int issueId;
        private Settings settingsForm;
        private bool generated = false;
        private DataBase dataBase;

        private decimal totalIntrest;
        private decimal totalBuyOut;

        private string newFile = "uks";
        private string docxFilePath;
        private string editedDocxFilePath;
        private string pdfFilePath;
        private string savedpdfFilePath;
        private string folderFilePath;
        private string libreOfficePath;

        string libreOfficeInst = "C:\\Program Files\\LibreOffice\\program\\soffice.exe";
        string libreOfficePort = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LibreOfficePortable\\App\\libreoffice\\program\\soffice.exe";


        public IssueUKS(int Id, Form1 form1)
        {
            InitializeComponent();
            Issue_Date.Value = DateTime.Now.Date;
            Pickup_Date.Value = DateTime.Now.Date.AddDays(29);

            mainForm = form1;
            settingsForm = new Settings(mainForm);
            issueId = Id;

            dataBase = mainForm.getDataBase();
            dataBase.CreateInvoicesTableIfNotExists();

            LoadLibreOffive();

            DocumentType.SelectedIndex = 0;
            FormType.SelectedIndex = 1;

            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/umowy/Wystawione");
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/umowy/backup");

            folderFilePath = AppDomain.CurrentDomain.BaseDirectory + "umowy/";

            docxFilePath = folderFilePath + newFile + ".docx";
            editedDocxFilePath = folderFilePath + "backup/" + newFile + "_new.docx";
            pdfFilePath = folderFilePath + "backup/" + newFile + "_new.pdf";
            savedpdfFilePath = folderFilePath + "Wystawione/" + newFile + "_" + issueId + ".pdf";

            Percentage.Text = settingsForm.GetPercentage().ToString();

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

                    FullName.Text = row["FullName"].ToString();
                    Company_Name.Text = row["Name"].ToString();
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

                decimal value;
                if (decimal.TryParse(textBox.Text, out value))
                {
                    // Formatuj tekst w kontrolce TextBox zawsze z dwiema cyframi po przecinku
                    textBox.Text = value.ToString("0.00");
                    Interest_ValueChanged(sender, e);
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
            LoadUser.Enabled = false;
            Save.Enabled = false;
            Print.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            decimal value = decimal.Parse(Value.Text);
            decimal total = CalculateTotalPrice(value, int.Parse(Days.Text), int.Parse(Percentage.Text));

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
                    ReplaceText(body, "#[sprzedajacy-nazwa-firmy]", Company_Name.Text);
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
                    ReplaceText(body, "#[przedmiot-wartosc-slownie]", GetValueAsText(value));

                    ReplaceText(body, "#[przedmiot-wartosc-odestki]", totalIntrest.ToString());
                    ReplaceText(body, "#[przedmiot-wartosc-calkowita]", total.ToString());
                    ReplaceText(body, "#[przedmiot-wartosc-calkowita-slownie]", GetValueAsText(value));

                    ReplaceText(body, "#[przedmiot-data-przyjecia]", Issue_Date.Value.ToString("dd-MM-yyyy"));
                    ReplaceText(body, "#[przedmiot-data-odbioru]", Pickup_Date.Value.ToString("dd-MM-yyyy"));
                    ReplaceText(body, "#[przedmiot-ilosc-dni]", Days.Text);
                    ReplaceText(body, "#[przedmiot-procent]", Percentage.Text);

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
                LoadUser.Enabled = true;
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

        public void ConvertDocxToPdf(string inputDocxFile, string outputPdfFile)
        {
            // Przykład użycia soffice.exe do konwersji DOCX do PDF
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = libreOfficePath,

                Arguments = $"--headless --convert-to pdf \"{inputDocxFile}\" --outdir \"{folderFilePath + "/backup/"}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = false
            };

            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CreateDocxFromData(DataTable data, string outputDocxFile, DateTime fromDate, DateTime toDate)
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Create(outputDocxFile, WordprocessingDocumentType.Document))
            {
                // Tworzenie głównego dokumentu Word
                MainDocumentPart mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new Document();

                // Tworzenie sekcji
                Body body = mainPart.Document.AppendChild(new Body());

                // Dodawanie nagłówka strony z informacją o okresie
                Paragraph headerParagraph = new Paragraph();
                Run headerRun = new Run();
                Text headerText = new Text($"Ewidencja kupna sprzedaży w okresie od {fromDate.ToShortDateString()} do {toDate.ToShortDateString()}");

                // Ustawianie czcionki
                RunProperties runProperties = new RunProperties();
                Bold bold = new Bold();
                FontSize fontSize = new FontSize() { Val = "34" }; // Ustawienie rozmiaru trzcionki na 24 punkty
                Justification justification2 = new Justification() { Val = JustificationValues.Center }; // Wyśrodkowanie tekstu
                ParagraphProperties paragraphProperties2 = new ParagraphProperties();
                paragraphProperties2.Append(justification2);

                runProperties.Append(bold);
                runProperties.Append(fontSize);

                headerRun.Append(runProperties);
                headerRun.Append(headerText);

                headerParagraph.Append(paragraphProperties2);
                headerParagraph.Append(headerRun);

                body.Append(headerParagraph);

                // Ustawianie orientacji na poziomą
                SectionProperties sectionProperties = new SectionProperties();
                PageSize pageSize = new PageSize() { Width = 19840U, Height = 12240U }; // Ustaw rozmiar na poziomy (landscape)
                sectionProperties.Append(pageSize);

                // Ustawianie marginesów
                PageMargin pageMargin = new PageMargin()
                {
                    Left = 720,     // 720 jednostek to 0,5 cala (połowa lewego marginesu)
                    Right = 720,    // 720 jednostek to 0,5 cala (połowa prawego marginesu)
                    Top = 720,      // 720 jednostek to 0,5 cala (połowa górnego marginesu)
                    Bottom = 720    // 720 jednostek to 0,5 cala (połowa dolnego marginesu)
                };
                sectionProperties.Append(pageMargin);

                body.Append(sectionProperties);

                // Tworzenie tabelki
                Table table = new Table();

                // Dostosowanie szerokości tabeli do rozmiaru strony A4
                TableWidth tableWidth = new TableWidth() { Width = "19840U", Type = TableWidthUnitValues.Dxa };
                table.Append(tableWidth);

                // Dodawanie stylu z obramowaniem do tabeli
                TableProperties tableProperties = new TableProperties(
                    new TableBorders(
                        new TopBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                        new BottomBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                        new LeftBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                        new RightBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                        new InsideHorizontalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 },
                        new InsideVerticalBorder() { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 6 }
                    )
                );
                table.Append(tableProperties);

                // Tworzenie nagłówków kolumn
                TableRow headerRow = new TableRow();
                foreach (DataColumn column in data.Columns)
                {
                    TableCell cell = new TableCell();
                    Paragraph paragraph = new Paragraph();

                    // Ustawianie właściwości justowania tekstu na środek
                    ParagraphProperties paragraphProperties = new ParagraphProperties();
                    Justification justification = new Justification() { Val = JustificationValues.Center };
                    paragraphProperties.Append(justification);

                    Shading shading = new Shading()
                    {
                        Val = ShadingPatternValues.Clear,
                        Color = "auto",
                        Fill = "D9D9D9" // Kolor "D9D9D9" reprezentuje szary kolor
                    };

                    paragraph.Append(paragraphProperties);
                    paragraph.Append(new Run(new Text(column.ColumnName)));
                    cell.Append(paragraph);
                    cell.Append(new TableCellProperties(shading));
                    headerRow.Append(cell);
                }
                table.Append(headerRow);

                foreach (DataRow row in data.Rows)
                {
                    TableRow dataRow = new TableRow();
                    int columnIndex = 0;

                    foreach (object item in row.ItemArray)
                    {
                        TableCell cell = new TableCell();
                        Paragraph paragraph = new Paragraph();

                        // Ustawianie właściwości justowania tekstu na środek
                        ParagraphProperties paragraphProperties = new ParagraphProperties();
                        Justification justification = new Justification() { Val = JustificationValues.Center };
                        paragraphProperties.Append(justification);

                        paragraph.Append(paragraphProperties);



                        // Formatowanie wartości jako liczba z dwoma miejscami po przecinku, jeśli to możliwe
                        if (TryFormatAsDecimal(item, out string formattedValue))
                        {
                            if (columnIndex == 8)
                            {
                                // Tworzenie nowego Run z właściwościami koloru tekstu
                                Run run = new Run();
                                RunProperties runProperties2 = new RunProperties();

                                Color color = new Color() { Val = "000000" }; // Kolor czerwony
                                if (decimal.Parse(formattedValue) < 0) { color = new Color() { Val = "CC0000" }; } // Kolor czerwony
                                if (decimal.Parse(formattedValue) > 0) { color = new Color() { Val = "00CC00" }; } // Kolor zielony

                                runProperties2.Append(color);
                                run.Append(runProperties2);

                                // Ustawienie tekstu sformatowanego Run
                                run.AppendChild(new Text(formattedValue));

                                // Dodanie Run do akapitu
                                paragraph.Append(run);
                            }
                            else
                            {
                                paragraph.Append(new Run(new Text(formattedValue)));
                            }
                        }
                        else if (columnIndex == 6 || columnIndex == 7)
                        {
                            string[] parts = item.ToString().Split(':'); // Rozdziel tekst na część przed i po dwukropku
                            if (parts.Length == 2) // Upewnij się, że są dwie części
                            {
                                string valuePart = parts[1].Trim(); // Pobierz część z wartością
                                decimal numericValue;
                                if (decimal.TryParse(valuePart, out numericValue)) // Sprawdź, czy wartość jest liczbą
                                {
                                    string formattedNumericValue = numericValue.ToString("0.00"); // Formatuj wartość do dwóch miejsc po przecinku
                                    parts[1] = formattedNumericValue; // Zaktualizuj część z wartością

                                    // Połącz ponownie części tekstu i ustaw sformatowany tekst
                                    paragraph.Append(new Run(new Text(string.Join(" : ", parts))));
                                }
                            }
                        }
                        else
                        {
                            paragraph.Append(new Run(new Text(item.ToString())));
                        }


                        cell.Append(paragraph);
                        dataRow.Append(cell);
                        columnIndex++;
                    }
                    table.Append(dataRow);
                }

                body.Append(table);

                // Zapisywanie dokumentu DOCX
                mainPart.Document.Save();
            }
        }

        private bool TryFormatAsDecimal(object obj, out string formattedValue)
        {
            formattedValue = null;
            if (obj == null) return false;

            CultureInfo cultureInfo = new CultureInfo("pl-PL"); // Ustaw polską kulturę, która używa przecinka jako separatora dziesiętnego
            if (decimal.TryParse(obj.ToString(), NumberStyles.Any, cultureInfo, out decimal decimalValue))
            {
                formattedValue = decimalValue.ToString("0.00", cultureInfo);
                return true;
            }

            return false;
        }

        private void SaveInvoiceToDatabase(int userId)
        {
            try
            {
                // Tworzymy nowy obiekt DataTable z odpowiednimi kolumnami
                System.Data.DataTable invoiceData = new System.Data.DataTable();
                invoiceData.Columns.Add("UserID", typeof(int));
                invoiceData.Columns.Add("DocumentType", typeof(string));
                invoiceData.Columns.Add("City", typeof(string));
                invoiceData.Columns.Add("Description", typeof(string));
                invoiceData.Columns.Add("TotalAmount", typeof(decimal));
                invoiceData.Columns.Add("InvoiceDate", typeof(DateTime));
                invoiceData.Columns.Add("BuyDate", typeof(DateTime));
                invoiceData.Columns.Add("Notes", typeof(string));
                invoiceData.Columns.Add("NIP", typeof(string));
                invoiceData.Columns.Add("Days", typeof(int));
                invoiceData.Columns.Add("Percentage", typeof(int));
                invoiceData.Columns.Add("Fee", typeof(decimal));
                invoiceData.Columns.Add("LateFee", typeof(decimal));
                invoiceData.Columns.Add("BuyAmount", typeof(decimal));
                invoiceData.Columns.Add("DateOfReturn", typeof(DateTime));
                invoiceData.Columns.Add("SaleDate", typeof(DateTime));
                invoiceData.Columns.Add("SaleAmount", typeof(decimal));

                DataRow newRow = invoiceData.NewRow();
                newRow["UserID"] = userId;
                newRow["City"] = settingsForm.GetCity();
                newRow["DocumentType"] = FormType.Text.ToString(); // Pobierz rodzaj dokumentu
                newRow["Description"] = Description.Text;
                newRow["TotalAmount"] = decimal.Parse(Value.Text);
                newRow["InvoiceDate"] = Issue_Date.Value.Date;
                newRow["BuyDate"] = Pickup_Date.Value.Date;
                newRow["Notes"] = Comments.Text;
                newRow["NIP"] = Nip.Text;
                newRow["Days"] = int.Parse(Days.Text);
                newRow["Percentage"] = int.Parse(Percentage.Text);
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
                    if (result2 == DialogResult.Yes) // Poprawiony warunek na result2
                    {
                        File.Copy(pdfFilePath, savedpdfFilePath, true);
                    }

                    if (issueId < 0)
                    {
                        // Aktualizuj dane faktury w bazie danych
                        UpdateUserInDatabase(dataBase.CheckUserExistsByPesel(Pesel.Text));
                        SaveInvoiceToDatabase(issueUserId);
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
            string name = Company_Name.Text;
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

            if (invoiceData != null && invoiceData.Rows.Count > 0)
            {
                DataRow row = invoiceData.Rows[0];

                Description.Text = row["Description"].ToString();
                Value.Text = row["TotalAmount"].ToString();
                Issue_Date.Value = Convert.ToDateTime(row["InvoiceDate"]);
                Comments.Text = row["Notes"].ToString();
                issueUserId = Convert.ToInt32(row["UserID"]); // Poprawione "USERID" na "UserID"

                // Nowe kolumny
                FormType.SelectedIndex = GetFormTypeIndex(row["DocumentType"].ToString());
                Pickup_Date.Value = Convert.ToDateTime(row["BuyDate"]);
                Days.Text = row["Days"].ToString();
                Percentage.Text = row["Percentage"].ToString();
                Fee.Text = row["Fee"].ToString();
                LateFee.Text = row["LateFee"].ToString();
                BuyAmount.Text = row["BuyAmount"].ToString();
                DateOfReturn.Value = Convert.ToDateTime(row["DateOfReturn"]);
                SaleDate.Value = Convert.ToDateTime(row["SaleDate"]);
                SaleAmount.Text = row["SaleAmount"].ToString();

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
            int userId = issueUserId;
            string description = Description.Text;
            decimal totalAmount = decimal.Parse(Value.Text);
            DateTime invoiceDate = Issue_Date.Value.Date;
            string notes = Comments.Text;
            decimal saleAmount = decimal.Parse(SaleAmount.Text);
            string documentType = FormType.Text;
            DateTime pickupDate = Pickup_Date.Value.Date;
            int days = int.Parse(Days.Text);
            int percentage = int.Parse(Percentage.Text);
            decimal fee = decimal.Parse(Fee.Text);
            decimal lateFee = decimal.Parse(LateFee.Text);
            decimal buyAmount = decimal.Parse(BuyAmount.Text);
            DateTime dateOfReturn = DateOfReturn.Value.Date;
            DateTime saleDate = SaleDate.Value.Date;

            dataBase.UpdateInvoiceInDatabase(invoiceId, userId, city, description, totalAmount, invoiceDate, notes, documentType, pickupDate, days, percentage, fee, lateFee, buyAmount, dateOfReturn, saleDate, saleAmount);
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

            if (pickupDate == issueDate)
            {
                daysDifference = 0;
            }
            // Wpisz liczbę dni w pole Days
            Days.Text = daysDifference.ToString();
        }

        private string GetValueAsText(decimal value)
        {

            int wholePart = (int)value;
            int fractionalPart = (int)((value - wholePart) * 100); // Przekształć ułamek na grosze

            string wholePartInWords = wholePart.ToWords();
            string fractionalPartInWords = fractionalPart.ToWords();

            return $"{wholePartInWords} złotych i {fractionalPartInWords} groszy";
        }

        public decimal CalculateTotalPrice(decimal initialPrice, int days, int percentage)
        {
            // Konwertujemy procent na decimal
            decimal decimalPercentage = (decimal)percentage / 100;

            // Obliczamy wartość odsetek za pełne okresy 30 dni
            decimal interest = initialPrice * decimalPercentage;

            // Obliczamy wartość odsetek za pozostałe dni
            decimal intrestByDay = interest / 30;

            totalIntrest = intrestByDay * days;
            // Obliczamy całkowitą cenę przedmiotu
            decimal totalPrice = initialPrice + totalIntrest;

            return totalPrice;
        }


        private void FormType_ValueChanged(object sender, EventArgs e)
        {
            ChangeFormType();
        }
        private void ChangeFormType()
        {
            String newFile = "error";

            if (FormType.SelectedIndex == 0)
            {
                newFile = "uk";
            }
            else if (FormType.SelectedIndex == 1)
            {
                newFile = "uks";
            }
            else if (FormType.SelectedIndex == 2)
            {
                newFile = "uppz";
            }

            folderFilePath = AppDomain.CurrentDomain.BaseDirectory + "umowy/";

            docxFilePath = folderFilePath + newFile + ".docx";
            editedDocxFilePath = folderFilePath + "backup/" + newFile + "_new.docx";
            pdfFilePath = folderFilePath + "backup/" + newFile + "_new.pdf";
            savedpdfFilePath = folderFilePath + "Wystawione/" + newFile + "_" + issueId + ".pdf";
        }
        private int GetFormTypeIndex(String text)
        {
            if ("Umowa Komisowa" == text)
            {
                return 0;
            }
            else if ("Umowa Pożyczki z Przechowaniem" == text)  // 0 - Umowa Komisowa
            {                                                   // 1 - Umowa Kupna-Sprzedaży
                return 2;                                       // 2 - Umowa Pożyczki z Przechowaniem
            }
            else
            {
                return 1;
            }
        }

        private void Interest_ValueChanged(object sender, EventArgs e)
        {
            decimal value = decimal.Parse(Value.Text);
            int days = int.Parse(Days.Text.ToString());
            int perc = int.Parse(Percentage.Text);

            if (value < 0) { value = 0; }
            if (days < 0) { days = 0; }
            if (perc < 0) { perc = 0; }

            totalBuyOut = CalculateTotalPrice(value, days, perc);

            Fee.Text = totalIntrest.ToString("F2");
            BuyAmount.Text = totalBuyOut.ToString("F2");
        }


        private void PercentageChanged(object sender, EventArgs e)
        {
            settingsForm.SetPercentage(int.Parse(Percentage.Text.ToString()));
        }

        private void IssueUKS_Load(object sender, EventArgs e)
        {

        }

        private void LoadLibreOffive() {
            if (File.Exists(libreOfficeInst))
            {
                libreOfficePath = libreOfficeInst;
            }
            else
            {
                libreOfficePath = libreOfficePort;
            }

        }
    }
}
