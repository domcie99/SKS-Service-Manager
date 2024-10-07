using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Humanizer;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;


using Color = DocumentFormat.OpenXml.Wordprocessing.Color;

#pragma warning disable
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
        private string backupPath;
        private string libreOfficePath;
        private string ewidPath;

        private string attachmentFilePath;
        private string editedAttachmentFilePath;
        private string attachmentPdfPath;
        private string imageFilePath;

        private string issuedCity;

        private string word2Pdf = AppDomain.CurrentDomain.BaseDirectory + "convert\\word2pdf.exe";

        public IssueUKS(int Id, Form1 form1)
        {
            InitializeComponent();
            CenterToScreen();

            Issue_Date.Value = DateTime.Now.Date;
            Pickup_Date.Value = DateTime.Now.Date.AddDays(29);

            mainForm = form1;
            settingsForm = new Settings(mainForm);
            issueId = Id;

            dataBase = mainForm.getDataBase();
            dataBase.CreateInvoicesTableIfNotExists();

            DocumentType.SelectedIndex = 0;
            FormType.SelectedIndex = 1;

            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "umowy\\Wystawione");
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "umowy\\backup");

            folderFilePath = AppDomain.CurrentDomain.BaseDirectory + "umowy\\";
            backupPath = AppDomain.CurrentDomain.BaseDirectory + "umowy\\backup";

            docxFilePath = folderFilePath + newFile + ".docx";
            editedDocxFilePath = folderFilePath + "backup\\" + newFile + "_new.docx";
            pdfFilePath = folderFilePath + "backup\\" + newFile + "_new.pdf";
            savedpdfFilePath = folderFilePath + "Wystawione\\" + newFile + "_" + issueId + ".pdf";
            ewidPath = folderFilePath + "ewidencja.docx";

            attachmentFilePath = folderFilePath + "atach.docx";
            editedAttachmentFilePath = backupPath + "\\atach_new.docx";
            attachmentPdfPath = backupPath + "\\atach_new.pdf";
            imageFilePath = null;

            Percentage.Text = settingsForm.GetPercentage().ToString();
            Days.Text = settingsForm.GetDays().ToString();

            issuedCity = FormatCityName(settingsForm.GetCity());


            if (issueId > 0)
            {
                LoadInvoiceData(issueId);
            }
            Value_Validation(Value, new EventArgs());

            if (DateOfReturn.Value.Year < 1800)
            {
                DateOfReturn.CustomFormat = " ";
                DateOfReturn.Format = DateTimePickerFormat.Custom;
            }
            if (SaleDate.Value.Year < 1800)
            {
                SaleDate.CustomFormat = " ";
                SaleDate.Format = DateTimePickerFormat.Custom;
            }
        }

        private void Load_Click(object sender, EventArgs e)
        {
            // Open the userlist form to select a user
            using (UserList userListForm = new UserList(mainForm))
            {
                userListForm.setIssueVisible(true);
                userListForm.selectUser = true;
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
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != ','))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '.' || e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf('.') > -1 || (sender as TextBox).Text.IndexOf(',') > -1))
            {
                e.Handled = true;
            }
        }

        private void Value_Validation(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            decimal value = 0;

            if (!string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = textBox.Text.Replace(".", ",");

                if (decimal.TryParse(textBox.Text, out value))
                {
                    textBox.Text = value.ToString("0.00");
                }
                else
                {
                    MessageBox.Show("Nieprawidłowa wartość.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox.Text = "0,00";
                }
            }
        }

        private void Value_ValidationIntrest(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            decimal value = 0;

            if (!string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = textBox.Text.Replace(".", ",");

                if (decimal.TryParse(textBox.Text, out value))
                {
                    textBox.Text = value.ToString("0.00");
                    BuyAmount.Text = (decimal.Parse(Value.Text) + decimal.Parse(Fee.Text) + decimal.Parse(LateFee.Text) + decimal.Parse(Commision.Text)).ToString("F2");
                    
                }
                else
                {
                    MessageBox.Show("Nieprawidłowa wartość.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox.Text = "0,00";
                }
            }
        }

        private void Value_ValidationAndCalculate(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            decimal value = 0;

            if (!string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = textBox.Text.Replace(".", ",");

                if (decimal.TryParse(textBox.Text, out value))
                {
                    textBox.Text = value.ToString("0.00");
                    Interest_ValueChanged(sender, e);
                }
                else
                {
                    MessageBox.Show("Nieprawidłowa wartość.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox.Text = "0,00";
                }
            }
        }

        private void Value_Validate(object sender, EventArgs e)
        {
            Value_Validation(sender, e);

            decimal val;
            decimal.TryParse(Value.Text, out val);
            if (val > 1000)
            {

                FormType.SelectedIndex = GetFormTypeIndex("Umowa Komisowa");
            }
            else
            {
                FormType.SelectedIndex = GetFormTypeIndex("Umowa Kupna-Sprzedaży");
            }

            Commision.Text = (val * 0.10m).ToString("F2");
        }

        private void ReplaceAll(Body body, MainDocumentPart mainPart)
        {
            decimal value = decimal.Parse(Value.Text);
            decimal total = decimal.Parse(BuyAmount.Text);
            int days = int.Parse(Days.Text.ToString());

            ReplaceText(body, "#[firma-nazwa]", settingsForm.GetCompanyName());
            ReplaceText(body, "#[firma-imie-nazwisko]", settingsForm.GetName() + " " + settingsForm.GetSurname());
            ReplaceText(body, "#[firma-adres]", settingsForm.GetStreetNumber());
            ReplaceText(body, "#[firma-kod]", settingsForm.GetPostCode());
            ReplaceText(body, "#[firma-miasto]", issuedCity);
            ReplaceText(body, "#[firma-telefon]", settingsForm.GetPhone());
            ReplaceText(body, "#[firma-nip]", settingsForm.GetNIP());
            ReplaceText(body, "#[firma-krs]", settingsForm.GetKRS());
            ReplaceText(body, "#[firma-regon]", settingsForm.GetREGON());

            ReplaceText(body, "#[data-wystawienia]", Issue_Date.Value.ToString("dd-MM-yyyy"));
            ReplaceText(body, "#[data-wystawienia+7]", Issue_Date.Value.AddDays(7).ToString("dd-MM-yyyy"));
            ReplaceText(body, "#[data-wystawienia+23]", Issue_Date.Value.AddDays(23).ToString("dd-MM-yyyy"));
            ReplaceText(body, "#[data-wystawienia+30]", Issue_Date.Value.AddDays(30).ToString("dd-MM-yyyy"));

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
            ReplaceText(body, "#[przedmiot-wartosc]", value.ToString("F2"));
            ReplaceText(body, "#[przedmiot-wartosc-slownie]", GetValueAsText(value));

            ReplaceText(body, "#[przedmiot-wartosc-odestki]", totalIntrest.ToString("F2"));
            ReplaceText(body, "#[przedmiot-wartosc-calkowita]", total.ToString("F2"));
            ReplaceText(body, "#[przedmiot-wartosc-calkowita-slownie]", GetValueAsText(value));

            ReplaceText(body, "#[przedmiot-wartosc-calkowita-obliczona]", (value + decimal.Parse(Fee.Text) + decimal.Parse(LateFee.Text) + decimal.Parse(Commision.Text)).ToString("F2"));

            ReplaceText(body, "#[przedmiot-wartosc-prowizja]", Commision.Text);

            ReplaceText(body, "#[przedmiot-wartosc-i-prowizja]", (value + decimal.Parse(Commision.Text)).ToString("F2"));
            ReplaceText(body, "#[przedmiot-wartosc-i-prowizja-slownie]", GetValueAsText((value + decimal.Parse(Commision.Text))));

            ReplaceText(body, "#[przedmiot-wartosc-szacunkowa]", Estimated_Value.Text);
            ReplaceText(body, "#[przedmiot-wartosc-szacunkowa-slownie]", GetValueAsText(decimal.Parse(Estimated_Value.Text)));
            
            ReplaceText(body, "#[przedmiot-wartosc-koszt-pozyczki]", (decimal.Parse(Fee.Text) + decimal.Parse(LateFee.Text) + decimal.Parse(Commision.Text)).ToString("F2"));

            ReplaceText(body, "#[przedmiot-data-przyjecia]", Issue_Date.Value.ToString("dd-MM-yyyy"));

            ReplaceText(body, "#[przedmiot-data-odbioru]", Pickup_Date.Value.ToString("dd-MM-yyyy"));
            ReplaceText(body, "#[przedmiot-data-odbioru+7]", Pickup_Date.Value.AddDays(7).ToString("dd-MM-yyyy"));
            ReplaceText(body, "#[przedmiot-data-odbioru+23]", Pickup_Date.Value.AddDays(23).ToString("dd-MM-yyyy"));
            ReplaceText(body, "#[przedmiot-data-odbioru+30]", Pickup_Date.Value.AddDays(30).ToString("dd-MM-yyyy"));
            ReplaceText(body, "#[przedmiot-data-odbioru+37]", Pickup_Date.Value.AddDays(30).ToString("dd-MM-yyyy"));

            ReplaceText(body, "#[przedmiot-ilosc-dni]", Days.Text);
            ReplaceText(body, "#[przedmiot-procent]", Percentage.Text);

            ReplaceText(body, "#[przedmiot-oplata]", Fee.Text);
            ReplaceText(body, "#[przedmiot-oplata-dziennie]", CalculateInterestByDay(value, int.Parse(Percentage.Text)).ToString("F2"));
            ReplaceText(body, "#[przedmiot-oplata-opoznienia]", LateFee.Text);
            ReplaceText(body, "#[przedmiot-kwota-wykupu]", BuyAmount.Text);

            ReplaceText(body, "#[przedmiot-data-zwrotu]", DateOfReturn.Value.ToString("dd-MM-yyyy"));
            ReplaceText(body, "#[przedmiot-data-sprzedazy]", SaleDate.Value.ToString("dd-MM-yyyy"));
            ReplaceText(body, "#[przedmiot-kwota-sprzedazy]", SaleAmount.Text);
            ReplaceText(body, "#[przedmiot-uwagi]", Comments.Text);

            if (imageFilePath != null) ReplaceImage(mainPart, "#[obrazek-placeholder]", imageFilePath);
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
            {
                return;
            }
            LoadUser.Enabled = false;
            Save.Enabled = false;
            Print.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            OverlayForm overlay = new OverlayForm();
            overlay.ShowOverlay(this);

            try
            {

                File.Copy(docxFilePath, editedDocxFilePath, true);

                using (WordprocessingDocument doc = WordprocessingDocument.Open(editedDocxFilePath, true))
                {
                    MainDocumentPart mainPart = doc.MainDocumentPart;
                    Body body = mainPart.Document.Body;

                    ReplaceAll(body, mainPart);

                    doc.Save();
                }

                if (attachment.Checked)
                {
                    File.Copy(attachmentFilePath, editedAttachmentFilePath, true);

                    using (WordprocessingDocument doc = WordprocessingDocument.Open(editedAttachmentFilePath, true))
                    {
                        MainDocumentPart mainPart = doc.MainDocumentPart;
                        Body body = mainPart.Document.Body;

                        ReplaceAll(body, mainPart);

                        doc.Save();
                    }

                    ConvertDocxToPdf(editedAttachmentFilePath, backupPath);
                }

                ConvertDocxToPdf(editedDocxFilePath, backupPath);
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

                overlay.Close();

                UpdateUserInDatabase(dataBase.CheckUserExists(Pesel.Text, DocumentNumber.Text, Adress.Text, City.Text, FullName.Text));

                if (dataBase.CheckInvoiceExists(issueId))
                {
                    UpdateInvoiceInDatabase(issueId);
                }
                else
                {
                    int userid = dataBase.CheckUserExists(Pesel.Text, DocumentNumber.Text, Adress.Text, City.Text, FullName.Text);
                    SaveInvoiceToDatabase(userid);
                }
            }
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(FullName.Text))
            {
                MessageBox.Show("Pole 'Imię i Nazwisko' jest wymagane.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FullName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(Adress.Text) || Adress.Text.Equals("ul. ", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Pole 'Ulica i Numer' jest wymagane.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Adress.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(Post_Code.Text))
            {
                MessageBox.Show("Pole 'Kod Pocztowy' jest wymagane.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Post_Code.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(City.Text))
            {
                MessageBox.Show("Pole 'Miasto' jest wymagane.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                City.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(Description.Text))
            {
                MessageBox.Show("Pole 'Opis' jest wymagane.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Description.Focus();
                return false;
            }

            return true;
        }

        public void ReplaceImage(MainDocumentPart mainPart, string imagePlaceholder, string imagePath)
        {
            var imageParts = mainPart.ImageParts.ToList();
            ImagePart oldImagePart = null;
            string oldRelationshipId = null;

            // Znajdź obraz na podstawie tekstu alternatywnego
            var imageElements = mainPart.Document.Body.Descendants<Drawing>().Where(d =>
            {
                var title = d.Descendants<DocumentFormat.OpenXml.Drawing.Wordprocessing.DocProperties>().FirstOrDefault()?.Description?.Value;
                return title != null && title.Contains(imagePlaceholder);
            });

            foreach (var imageElement in imageElements)
            {
                var blip = imageElement.Descendants<DocumentFormat.OpenXml.Drawing.Blip>().FirstOrDefault();
                if (blip != null)
                {
                    oldRelationshipId = blip.Embed.Value;
                    oldImagePart = (ImagePart)mainPart.GetPartById(oldRelationshipId);
                    break;
                }
            }

            if (oldImagePart != null)
            {
                mainPart.DeletePart(oldImagePart);

                ImagePart newImagePart = mainPart.AddImagePart(ImagePartType.Jpeg);
                using (FileStream stream = new FileStream(imagePath, FileMode.Open))
                {
                    newImagePart.FeedData(stream);
                }

                var newRelationshipId = mainPart.GetIdOfPart(newImagePart);

                foreach (var imageElement in imageElements)
                {
                    var blip = imageElement.Descendants<DocumentFormat.OpenXml.Drawing.Blip>().FirstOrDefault();
                    if (blip != null)
                    {
                        blip.Embed.Value = newRelationshipId;
                    }
                }
            }
        }


        private void ReplaceText(Body body, string findText, string replaceText)
        {
            foreach (Text text in body.Descendants<Text>())
            {
                text.Text = text.Text.Replace(findText, replaceText);
            }
        }

        public void ConvertDocxToPdf(string inputDocxFile, string outputPdfFile)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = word2Pdf,
                Arguments = $"/source \"{inputDocxFile}\" /target \"{outputPdfFile}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();

                    string output = process.StandardOutput.ReadToEnd();
                    string errors = process.StandardError.ReadToEnd();
                    if (!string.IsNullOrEmpty(errors))
                    {
                        MessageBox.Show("Błąd: " + errors, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string raportTitle(string documentType, string realizedType)
        {
            string reportTitle = "Ewidencja ";

            switch (realizedType)
            {
                case "Zrealizowane":
                    reportTitle += "zrealizowanych ";
                    break;
                case "Niezrealizowane":
                    reportTitle += "niezrealizowanych ";
                    break;
                    // W przypadku "Wszystkie" nie dodajemy nic
            }

            switch (documentType)
            {
                case "Umowa Kupna-Sprzedaży":
                    reportTitle += "umów kupna sprzedaży";
                    break;
                case "Umowa Komisowa":
                    reportTitle += "umów komisowych";
                    break;
                case "Umowa Konsumenckiej Pożyczki Lombardowej":
                    reportTitle += "umów konsumenckiej pożyczki lombardowej";
                    break;
                case "Umowa Pożyczki z Przechowaniem":
                    reportTitle += "umów pożyczki z przechowaniem";
                    break;
                default:
                    reportTitle += "wszystkich umów";
                    break;
            }
            return reportTitle;
        }

        public void CreateDocxFromData(DataTable data, string outputDocxFile, DateTime fromDate, DateTime toDate, string documentType, string realizedType)
        {
            // Skopiowanie istniejącego dokumentu jako wzorca
            File.Copy(ewidPath, outputDocxFile, true);

            // Suma dla kolumn do zsumowania
            decimal totalAmountPaidToCustomer = 0;
            decimal totalSalesValueMinusWear = 0; // Sumowanie "Wartość sprzedaży minus zużycie"
            decimal totalReturnAmount = 0; // Kwota zwrotu
            decimal totalSaleAmount = 0; // Kwota sprzedaży
            decimal totalCommissionOrRepurchase = 0; // Kwota uzyskanej prowizji albo odkupu

            using (WordprocessingDocument doc = WordprocessingDocument.Open(outputDocxFile, true))
            {
                MainDocumentPart mainPart = doc.MainDocumentPart;
                Body body = mainPart.Document.Body;

                // Zdefiniuj tytuł w zależności od wybranego typu umowy
                string reportTitle = raportTitle(documentType, realizedType); // Przekazanie realizedType

                // Znajdź i zaktualizuj tekst nagłówka
                var headerText = body.Descendants<Text>().FirstOrDefault(t => t.Text.Contains("#[ewidencja-title]"));
                if (headerText != null)
                {
                    headerText.Text = headerText.Text.Replace("#[ewidencja-title]", $"{reportTitle} w okresie od {fromDate.ToShortDateString()} do {toDate.ToShortDateString()}");
                }

                // Znajdź i zaktualizuj tabelę
                var tableText = body.Descendants<Text>().FirstOrDefault(t => t.Text.Contains("#[ewidencja-tabela]"));
                if (tableText != null)
                {
                    var table = tableText.Ancestors<Table>().FirstOrDefault();

                    // Usuń istniejącą zawartość tabeli
                    foreach (var row in table.Elements<TableRow>().Skip(1).ToList())
                    {
                        row.Remove();
                    }

                    // Wypełnij tabelę danymi z DataTable
                    foreach (DataRow row in data.Rows)
                    {
                        TableRow dataRow = new TableRow();
                        decimal currentReturnAmount = 0; // Zmienna przechowująca kwotę zwrotu dla bieżącego wiersza

                        for (int i = 0; i < row.ItemArray.Length; i++)
                        {
                            // Pomiń kolumnę "Kwota sprzedaży" (indeks 7), ponieważ ma być połączona z datami
                            if (i == 7)
                            {
                                continue;
                            }

                            TableCell cell = new TableCell();
                            Paragraph paragraph = new Paragraph();

                            ParagraphProperties paragraphProperties = new ParagraphProperties();
                            Justification justification = new Justification() { Val = JustificationValues.Center };
                            paragraphProperties.Append(justification);

                            TableCellProperties cellProperties = new TableCellProperties();
                            TableCellVerticalAlignment verticalAlignment = new TableCellVerticalAlignment() { Val = TableVerticalAlignmentValues.Center };
                            cellProperties.Append(verticalAlignment);

                            paragraph.Append(paragraphProperties);
                            cell.Append(cellProperties);

                            Run run = new Run();
                            RunProperties runProperties = new RunProperties();
                            FontSize fontSize = new FontSize() { Val = "14" };
                            runProperties.Append(fontSize);

                            // Połączone wartości "Data zwrotu" i "Kwota zwrotu"
                            if (i == 5) // Data zwrotu
                            {
                                string dateValue = row[i].ToString();
                                string amountValue = row[3].ToString(); // Kwota zwrotu to teraz "Wartość sprzedaży minus zużycie" (indeks 3)

                                // Ukryj kwoty wynoszące 0
                                if (decimal.TryParse(amountValue, out decimal amount) && amount == 0)
                                {
                                    amountValue = "";
                                }
                                else
                                {
                                    amountValue = $"{amount:F2}";
                                }

                                if (DateTime.TryParse(dateValue, out DateTime date) && date.Year >= 1800)
                                {
                                    run.Append(runProperties);
                                    run.AppendChild(new Text($"{date:dd.MM.yyyy}"));
                                    run.AppendChild(new Break()); // Dodaj nową linię dla kwoty
                                    run.AppendChild(new Text(amountValue));

                                    // Zapisz bieżącą kwotę zwrotu do zmiennej
                                    currentReturnAmount = amount;

                                    // Sumowanie wartości zwrotu
                                    totalReturnAmount += currentReturnAmount; // Sumuj wartość zwrotu
                                }
                                else
                                {
                                    run.Append(runProperties);
                                    run.AppendChild(new Text("")); // Puste pole, jeśli data nie istnieje lub jest starsza niż rok 1800
                                }

                                paragraph.Append(run);
                            }
                            else if (i == 6) // Data sprzedaży
                            {
                                string dateValue = row[i].ToString();
                                string amountValue = row[i + 1].ToString(); // Kwota sprzedaży (która była wcześniej w kolumnie 7)

                                // Ukryj kwoty wynoszące 0
                                if (decimal.TryParse(amountValue, out decimal amount) && amount == 0)
                                {
                                    amountValue = "";
                                }
                                else
                                {
                                    amountValue = $"{amount:F2}";
                                }

                                if (DateTime.TryParse(dateValue, out DateTime date) && date.Year >= 1800)
                                {
                                    run.Append(runProperties);
                                    run.AppendChild(new Text($"{date:dd.MM.yyyy}"));
                                    run.AppendChild(new Break()); // Dodaj nową linię dla kwoty
                                    run.AppendChild(new Text(amountValue));

                                    // Sumowanie wartości sprzedaży
                                    totalSaleAmount += amount; // Sumuj wartość sprzedaży
                                }
                                else
                                {
                                    run.Append(runProperties);
                                    run.AppendChild(new Text("")); // Puste pole, jeśli data nie istnieje lub jest starsza niż rok 1800
                                }

                                paragraph.Append(run);
                            }
                            else if (i == 8) // Obliczanie prowizji
                            {
                                // Kwota zapłacona klientowi
                                decimal totalAmountPaid = decimal.TryParse(row[1].ToString(), out decimal paidAmount) ? paidAmount : 0;

                                // Sprawdź, czy mamy kwotę zwrotu lub sprzedaży
                                decimal saleAmount = decimal.TryParse(row[7].ToString(), out decimal saleAmt) ? saleAmt : 0;
                                decimal commissionValue = 0;

                                // Użyj kwoty zwrotu lub sprzedaży do obliczenia prowizji
                                if (saleAmount > 0)
                                {
                                    commissionValue = saleAmount - totalAmountPaid;
                                }
                                else if (currentReturnAmount > 0) // Użyj kwoty zwrotu, jeśli nie ma sprzedaży
                                {
                                    commissionValue = currentReturnAmount - totalAmountPaid;
                                }
                                else // Jeśli nie ma zwrotu ani sprzedaży, odejmij kwotę zapłaconą klientowi
                                {
                                    commissionValue = -totalAmountPaid;
                                }

                                // Ustaw kolor tekstu na podstawie wartości prowizji
                                Color color = new Color() { Val = "000000" }; // Domyślny kolor czarny
                                if (commissionValue < 0)
                                {
                                    color = new Color() { Val = "CC0000" }; // Kolor czerwony dla wartości ujemnych
                                }
                                else if (commissionValue > 0)
                                {
                                    color = new Color() { Val = "00CC00" }; // Kolor zielony dla wartości dodatnich
                                }

                                runProperties.Append(color);
                                run.Append(runProperties);
                                run.AppendChild(new Text(commissionValue.ToString("F2")));

                                // Dodaj wartość do sumy
                                totalCommissionOrRepurchase += commissionValue;

                                paragraph.Append(run);
                            }
                            else
                            {
                                run.Append(runProperties);
                                run.AppendChild(new Text(row[i].ToString()));
                                paragraph.Append(run);
                            }

                            cell.Append(paragraph);
                            dataRow.Append(cell);

                            // Sumowanie wartości dla wybranych kolumn
                            if (i == 1 && decimal.TryParse(row[i].ToString(), out decimal value1)) // Kwota zapłacona klientowi
                            {
                                totalAmountPaidToCustomer += value1;
                            }
                            else if (i == 3 && decimal.TryParse(row[i].ToString(), out decimal value2)) // Wartość sprzedaży minus zużycie
                            {
                                totalSalesValueMinusWear += value2;
                            }
                        }
                        table.Append(dataRow);
                    }

                    // Dodaj wiersz z podsumowaniem na końcu tabeli z cieniowaniem #BFBFBF
                    TableRow summaryRow = new TableRow();

                    summaryRow.Append(CreateSummaryCell("Suma", true));
                    summaryRow.Append(CreateSummaryCell(totalAmountPaidToCustomer.ToString("F2"), true));
                    summaryRow.Append(CreateSummaryCell("", true)); // Pusta komórka dla innych kolumn
                    summaryRow.Append(CreateSummaryCell(totalSalesValueMinusWear.ToString("F2"), true)); // Suma "Wartość sprzedaży minus zużycie"
                    summaryRow.Append(CreateSummaryCell("", true));
                    summaryRow.Append(CreateSummaryCell(totalReturnAmount.ToString("F2"), true)); // Suma zwrotów
                    summaryRow.Append(CreateSummaryCell(totalSaleAmount.ToString("F2"), true)); // Suma sprzedaży
                    summaryRow.Append(CreateSummaryCell(totalCommissionOrRepurchase.ToString("F2"), true)); // Suma prowizji
                    summaryRow.Append(CreateSummaryCell("", true));
                    table.Append(summaryRow);

                    tableText.Text = tableText.Text.Replace("#[ewidencja-tabela]", "");
                }
            }
        }

        private TableCell CreateSummaryCell(string text, bool applyShading = false)
        {
            TableCell cell = new TableCell();
            Paragraph paragraph = new Paragraph(new Run(new Text(text)));
            ParagraphProperties paragraphProperties = new ParagraphProperties();
            Justification justification = new Justification() { Val = JustificationValues.Center };
            paragraphProperties.Append(justification);
            paragraph.PrependChild(paragraphProperties);
            cell.Append(paragraph);

            // Dodanie cieniowania, jeśli opcja jest ustawiona na true
            if (applyShading)
            {
                TableCellProperties cellProperties = new TableCellProperties();
                Shading shading = new Shading()
                {
                    Fill = "BFBFBF",
                    Val = ShadingPatternValues.Clear
                };
                cellProperties.Append(shading);
                cell.Append(cellProperties);
            }

            return cell;
        }

        private void ReplaceTextInDocument(WordprocessingDocument doc, string token, string replacementText)
        {
            var body = doc.MainDocumentPart.Document.Body;
            var paragraphs = body.Descendants<Paragraph>();

            foreach (var paragraph in paragraphs)
            {
                var text = paragraph.InnerText;

                if (text.Contains(token))
                {
                    text = text.Replace(token, replacementText);
                    paragraph.RemoveAllChildren<Text>();
                    paragraph.Append(new Text(text));
                }
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
                invoiceData.Columns.Add("Commision", typeof(decimal));
                invoiceData.Columns.Add("BuyAmount", typeof(decimal));
                invoiceData.Columns.Add("DateOfReturn", typeof(DateTime));
                invoiceData.Columns.Add("SaleDate", typeof(DateTime));
                invoiceData.Columns.Add("SaleAmount", typeof(decimal));
                invoiceData.Columns.Add("EstimatedValue", typeof(decimal)); // Dodano EstimatedValue

                DataRow newRow = invoiceData.NewRow();
                newRow["UserID"] = userId;
                newRow["City"] = issuedCity;
                newRow["DocumentType"] = FormType.Text.ToString();
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
                newRow["Commision"] = decimal.Parse(Commision.Text);
                newRow["BuyAmount"] = decimal.Parse(BuyAmount.Text);
                newRow["DateOfReturn"] = DateOfReturn.Value.Date;
                newRow["SaleDate"] = SaleDate.Value.Date;
                newRow["SaleAmount"] = decimal.Parse(SaleAmount.Text);
                newRow["EstimatedValue"] = decimal.Parse(Estimated_Value.Text); // Dodano EstimatedValue

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
                // Kopiowanie i otwieranie głównego dokumentu PDF
                File.Copy(pdfFilePath, savedpdfFilePath, true);

                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/c start {pdfFilePath}",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    };
                    Process process = new Process { StartInfo = startInfo };
                    process.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd podczas otwierania pliku PDF: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Jeśli checkbox attachment jest zaznaczony, kopiowanie i otwieranie załącznika PDF
                if (attachment.Checked)
                {
                    string attachmentPdfPath = backupPath + "\\atach_new.pdf";
                    string savedAttachmentPdfPath = folderFilePath + "Wystawione\\atach_" + issueId + ".pdf";

                    File.Copy(attachmentPdfPath, savedAttachmentPdfPath, true);

                    try
                    {
                        ProcessStartInfo attachmentStartInfo = new ProcessStartInfo
                        {
                            FileName = "cmd.exe",
                            Arguments = $"/c start {attachmentPdfPath}",
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            WindowStyle = ProcessWindowStyle.Hidden
                        };
                        Process attachmentProcess = new Process { StartInfo = attachmentStartInfo };
                        attachmentProcess.Start();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Błąd podczas otwierania pliku PDF załącznika: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                generated = false;
                this.Close();
            }
            else
            {
                MessageBox.Show("Musisz pierw wygenerować dokument guzikiem Zapisz", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateUserInDatabase(int userId)
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

            // Wywołaj funkcję UpdateUserInDatabase, przekazując odpowiednie parametry
            dataBase.UpdateUserInDatabase(userId, fullName, name, address, postalCode, city, phone, email, documentType, documentNumber, pesel, nip, notes);

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
                issueUserId = Convert.ToInt32(row["UserID"]);

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
                Commision.Text = row["Commision"].ToString();
                Estimated_Value.Text = row["EstimatedValue"].ToString();
                issuedCity = FormatCityName(row["City"].ToString());

                LoadUserData(issueUserId);
            }
            else
            {
                MessageBox.Show("Nie można znaleźć danych faktury.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string FormatCityName(string city)
        {
            if (string.IsNullOrEmpty(city))
                return city;

            var words = city.Split(new[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i].ToLower();
                words[i] = char.ToUpper(word[0]) + word.Substring(1);
            }

            return string.Join(" ", words).Replace(" -", "-").Replace("- ", "-");
        }


        private void UpdateInvoiceInDatabase(int invoiceId)
        {
            string city = issuedCity;
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

            string nip = Nip.Text;

            decimal lateFee = decimal.Parse(LateFee.Text);
            decimal commision = decimal.Parse(Commision.Text);
            decimal buyAmount = decimal.Parse(BuyAmount.Text);
            DateTime dateOfReturn = DateOfReturn.Value.Date;
            DateTime saleDate = SaleDate.Value.Date;
            decimal estimatedValue = decimal.Parse(Estimated_Value.Text); // Dodano EstimatedValue

            dataBase.UpdateInvoiceInDatabase(invoiceId, userId, city, description, totalAmount, invoiceDate, notes, documentType, pickupDate, days, percentage, fee, nip, lateFee, commision, buyAmount, dateOfReturn, saleDate, saleAmount, estimatedValue);
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

            int newValue = !string.IsNullOrEmpty(Percentage.Text) ? int.Parse(Percentage.Text) : 0;
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
            int daysDifference = span.Days;

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

            decimal comm = decimal.Parse(Commision.Text);

            if (FormType.Text == "Umowa Konsumenckiej Pożyczki Lombardowej")
            {
                if (days < 8)
                {
                    days = 0;
                }
            }

            totalIntrest = intrestByDay * days;
            // Obliczamy całkowitą cenę przedmiotu
            decimal totalPrice = initialPrice + totalIntrest + comm;

            return totalPrice;
        }

        public decimal CalculateInterestByDay(decimal value, int percentage)
        {
            decimal decimalPercentage = (decimal)percentage / 100;
            decimal interest = value * decimalPercentage;
            decimal intrestByDay = interest / 30;
            return intrestByDay;
        }


        private void FormType_ValueChanged(object sender, EventArgs e)
        {
            String newFile = "error";

            string selectedText = FormType.Text;

            attachment.Visible = false;
            uploadImageButton.Visible = false;

            if (selectedText == "Umowa Kupna-Sprzedaży")
            {
                newFile = "uks";
            }
            else if (selectedText == "Umowa Komisowa")
            {
                newFile = "uk";
            }
            else if (selectedText == "Umowa Konsumenckiej Pożyczki Lombardowej")
            {
                newFile = "ukpl";
                attachment.Visible = true;
                uploadImageButton.Visible = true;
            }
            else if (selectedText == "Umowa Pożyczki z Przechowaniem")
            {
                newFile = "uppz";
            }

            folderFilePath = AppDomain.CurrentDomain.BaseDirectory + "umowy\\";

            docxFilePath = folderFilePath + newFile + ".docx";
            editedDocxFilePath = folderFilePath + "backup\\" + newFile + "_new.docx";
            pdfFilePath = folderFilePath + "backup\\" + newFile + "_new.pdf";
            savedpdfFilePath = folderFilePath + "Wystawione\\" + newFile + "_" + issueId + ".pdf";
        }

        private int GetFormTypeIndex(string documentType)
        {
            switch (documentType)
            {
                case "Umowa Kupna-Sprzedaży":
                    return 0;
                case "Umowa Komisowa":
                    return 1;
                case "Umowa Konsumenckiej Pożyczki Lombardowej":
                    return 2;
                case "Umowa Pożyczki z Przechowaniem":
                    return 3;
                default:
                    return 0;
            }
        }

        private void Interest_ValueChanged(object sender, EventArgs e)
        {
            decimal value = !string.IsNullOrEmpty(Value.Text) ? decimal.Parse(Value.Text) : 0;
            int days = !string.IsNullOrEmpty(Days.Text) ? int.Parse(Days.Text) : 0;
            int perc = !string.IsNullOrEmpty(Percentage.Text) ? int.Parse(Percentage.Text) : 0;


            if (value < 0) { value = 0; }
            if (days < 0) { days = 0; }
            if (perc < 0) { perc = 0; }


            totalBuyOut = CalculateTotalPrice(value, days, perc);

            Fee.Text = totalIntrest.ToString("F2");
            BuyAmount.Text = totalBuyOut.ToString("F2");
            Commision.Text = (value * 0.10m).ToString("F2");
        }


        private void PercentageChanged(object sender, EventArgs e)
        {
            settingsForm.SetPercentage(int.Parse(Percentage.Text.ToString()));
        }

        private void Days_TextChanged(object sender, EventArgs e)
        {
            int days = !string.IsNullOrEmpty(Days.Text) ? int.Parse(Days.Text) : 0;
            Days.Text = days.ToString();
            settingsForm.SetDays(days);
        }

        private void uploadImageButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Image Files (*.png; *.jpg; *.jpeg)|*.png; *.jpg; *.jpeg";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the path of specified file
                    imageFilePath = openFileDialog.FileName;
                    imageFileName.Text = imageFilePath.ToString();
                }
            }
        }

        private void FormType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Post_Code_Leave(object sender, EventArgs e)
        {
            string postalCode = Post_Code.Text.Replace("-", "");

            if (postalCode.Length > 5)
            {
                MessageBox.Show("Nieprawidłowy kod pocztowy. Kod pocztowy nie może zawierać więcej niż 5 cyfr.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Post_Code.Focus();
                return;
            }

            if (postalCode.Length == 5)
            {
                Post_Code.Text = postalCode.Insert(2, "-");
            }

            if (!IsValidPostalCode(Post_Code.Text))
            {
                MessageBox.Show("Nieprawidłowy kod pocztowy. Kod pocztowy powinien mieć format XX-XXX.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Post_Code.Focus();
            }
        }

        private bool IsValidPostalCode(string postalCode)
        {
            // Sprawdź, czy kod pocztowy ma format XX-XXX, gdzie X jest cyfrą
            Regex regex = new Regex(@"^\d{2}-\d{3}$");
            return regex.IsMatch(postalCode);
        }

        private void delete_Click(object sender, EventArgs e)
        {
            if (issueId > 0)
            {
                int selectedIssueID = issueId;

                DialogResult result = MessageBox.Show("Czy na pewno chcesz usunąć ten wpis?", "Potwierdź Usunięcie", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (dataBase.DeleteUks(selectedIssueID))
                    {
                        MessageBox.Show("Wpis został pomyślnie usunięty.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();

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

        private void DateOfReturn_MouseDown(object sender, MouseEventArgs e)
        {
            if (DateOfReturn.Value == new DateTime(1753, 1, 1))
            {
                DateOfReturn.Value = DateTime.Now;
            }

            DateOfReturn.CustomFormat = "dd.MM.yyyy";
        }

        private void SaleDate_MouseDown(object sender, MouseEventArgs e)
        {
            if (SaleDate.Value == new DateTime(1753, 1, 1))
            {
                SaleDate.Value = DateTime.Now;
            }

            SaleDate.CustomFormat = "dd.MM.yyyy";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateOfReturn.Value = new DateTime(1753, 1, 1);
            DateOfReturn.CustomFormat = " ";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaleDate.Value = new DateTime(1753, 1, 1);
            SaleDate.CustomFormat = " ";
        }

        private void DateOfReturn_DropDown(object sender, EventArgs e)
        {
            if (DateOfReturn.Value == new DateTime(1753, 1, 1))
            {
                Task.Delay(200).ContinueWith(_ => this.Invoke(new Action(() => SendKeys.Send("%{DOWN}"))));
            }
        }

        private void SaleDate_DropDown(object sender, EventArgs e)
        {
            if (SaleDate.Value == new DateTime(1753, 1, 1))
            {
                Task.Delay(200).ContinueWith(_ => this.Invoke(new Action(() => SendKeys.Send("%{DOWN}"))));
            }
        }
    }
}
