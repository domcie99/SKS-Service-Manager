using MySqlConnector;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Body = DocumentFormat.OpenXml.Wordprocessing.Body;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Word;
using Word = Microsoft.Office.Interop.Word;
using System.Diagnostics;
using Humanizer;

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

        // Ścieżka do oryginalnego dokumentu DOCX
        string docxFilePath = AppDomain.CurrentDomain.BaseDirectory + "umowy/uks.docx";

        // Ścieżka do nowego dokumentu DOCX z zastąpionymi danymi
        string editedDocxFilePath = AppDomain.CurrentDomain.BaseDirectory + "umowy/uks_new.docx";

        // Ścieżka do pliku PDF wynikowego
        string pdfFilePath = AppDomain.CurrentDomain.BaseDirectory + "umowy/uks.pdf";

        public IssueUKS(int Id, Form1 mainForm)
        {
            InitializeComponent();

            settingsForm = new Settings(mainForm);
            connectionString = $"Server={settingsForm.GetMySQLHost()};Port={settingsForm.GetMySQLPort()};Database={settingsForm.GetMySQLDatabase()};User ID={settingsForm.GetMySQLUser()};Password={settingsForm.GetMySQLPassword()};";
            connection = new MySqlConnection(connectionString);

            issueId = Id; // Przypisz identyfikator faktury UKS (może być -1, jeśli to nowa faktura)

            if (issueId > 0)
            {
                // Jeśli issueId jest większe od zera, to oznacza, że edytujemy istniejącą fakturę UKS
                LoadInvoiceData(issueId);
            }
            else
            {
                CreateInvoicesTableIfNotExists();
            }
        }

        private void Load_Click(object sender, EventArgs e)
        {
            // Open the userlist form to select a user
            using (UserList userListForm = new UserList(Form1))
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
                connection.Open();

                // Wczytaj dane użytkownika o identyfikatorze userIdToEdit z bazy danych
                string query = "SELECT * FROM Users WHERE ID = @UserID;";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@UserID", userIdToEdit);

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    FullName.Text = reader["Name"].ToString();
                    Adress.Text = reader["Address"].ToString();
                    Post_Code.Text = reader["PostalCode"].ToString();
                    City.Text = reader["City"].ToString();
                    Phone.Text = reader["Phone"].ToString();
                    EMail.Text = reader["Email"].ToString();
                    DocumentType.Text = reader["DocumentType"].ToString();
                    DocumentNumber.Text = reader["DocumentNumber"].ToString();
                    Pesel.Text = reader["Pesel"].ToString();
                    Nip.Text = reader["NIP"].ToString(); // Nowe pole NIP
                    Notes.Text = reader["Notes"].ToString();
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas wczytywania danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void Value_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Pozwól na tylko cyfry, kropkę, Backspace oraz Control(do kopiowania i wklejania)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ','))
            {
                e.Handled = true;
            }

            // Upewnij się, że jest tylko jedna kropka w polu tekstowym
            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1))
            {
                e.Handled = true;
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            Load.Enabled = false;
            Save.Enabled = false;
            Print.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

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
                    ReplaceText(body, "#[sprzedajacy-adres]", Adress.Text);
                    ReplaceText(body, "#[sprzedajacy-kod]", Post_Code.Text);
                    ReplaceText(body, "#[sprzedajacy-miasto]", City.Text);
                    ReplaceText(body, "#[sprzedajacy-doc]", DocumentType.Text);
                    ReplaceText(body, "#[sprzedajacy-numer]", DocumentNumber.Text);
                    ReplaceText(body, "#[sprzedajacy-pesel]", Pesel.Text);

                    ReplaceText(body, "#[przedmiot-opis]", Description.Text);

                    decimal value = decimal.Parse(Value.Text);

                    // Pobierz część całkowitą (złotówki) i część ułamkową (grosze)
                    int wholePart = (int)value;
                    int fractionalPart = (int)((value - wholePart) * 100); // Przekształć ułamek na grosze
 
                    string wholePartInWords = wholePart.ToWords();
                    string fractionalPartInWords = fractionalPart.ToWords();

                    ReplaceText(body, "#[przedmiot-wartosc]", value.ToString());
                    ReplaceText(body, "#[przedmiot-wartosc-slownie]", $"{wholePartInWords} złotych i {fractionalPartInWords} groszy");

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
                    UpdateUserInDatabase(CheckUserExistsByPesel(Pesel.Text)); ;
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
        }


        private void SaveInvoiceToDatabase(int userId)
        {
            try
            {
                connection.Open();

                string insertInvoiceQuery = @"
                    INSERT INTO UKS (UserID, City, Description, TotalAmount, InvoiceDate, Notes)
                    VALUES (@UserID, @City, @Description, @TotalAmount, @InvoiceDate, @Notes);
                    SELECT LAST_INSERT_ID();"; // Dodaj zapytanie do pobrania ostatnio dodanego ID

                MySqlCommand cmd = new MySqlCommand(insertInvoiceQuery, connection);
                cmd.Parameters.AddWithValue("@UserID", userId); // ID użytkownika, do którego przypisana jest faktura
                cmd.Parameters.AddWithValue("@City", settingsForm.GetCity()); // Dodaj miasto z ustawień
                cmd.Parameters.AddWithValue("@Description", Description.Text); // Opis przedmiotu
                cmd.Parameters.AddWithValue("@TotalAmount", decimal.Parse(Value.Text)); // Przyjmujemy, że Value.Text zawiera wartość faktury
                cmd.Parameters.AddWithValue("@InvoiceDate", Issue_Date.Value.Date); // Data przyjęcia
                cmd.Parameters.AddWithValue("@Notes", Notes.Text); // Uwagi

                issueId = Convert.ToInt32(cmd.ExecuteScalar());        
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas zapisywania faktury do bazy danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
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
                        File.Copy(pdfFilePath, savedpdfFilePath, true);
                    }

                    if (issueId < 0)
                    {
                        // Aktualizuj dane faktury w bazie danych
                        UpdateUserInDatabase(CheckUserExistsByPesel(Pesel.Text));
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

        private bool CheckUserExistsByPesel(string pesel)
        {
            try
            {
                connection.Open();

                // Sprawdź, czy istnieje użytkownik o danym numerze PESEL
                string query = "SELECT COUNT(*) FROM Users WHERE Pesel = @Pesel;";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@Pesel", pesel);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                return count > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas sprawdzania użytkownika w bazie danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                connection.Close();
            }
        }
        private void UpdateUserInDatabase(bool exist)
        {
            if (exist)
            {
                try
                {
                    connection.Open();

                    // Aktualizuj dane użytkownika w bazie danych na podstawie numeru PESEL
                    string updateQuery = @"
                        UPDATE Users
                        SET Name = @Name, Address = @Address, PostalCode = @PostalCode, City = @City, Phone = @Phone, Email = @Email,
                            DocumentType = @DocumentType, DocumentNumber = @DocumentNumber, NIP = @NIP, Notes = @Notes
                        WHERE Pesel = @Pesel;";

                    MySqlCommand cmd = new MySqlCommand(updateQuery, connection);
                    cmd.Parameters.AddWithValue("@Name", FullName.Text);
                    cmd.Parameters.AddWithValue("@Address", Adress.Text);
                    cmd.Parameters.AddWithValue("@PostalCode", Post_Code.Text);
                    cmd.Parameters.AddWithValue("@City", City.Text);
                    cmd.Parameters.AddWithValue("@Phone", Phone.Text);
                    cmd.Parameters.AddWithValue("@Email", EMail.Text);
                    cmd.Parameters.AddWithValue("@DocumentType", DocumentType.Text);
                    cmd.Parameters.AddWithValue("@DocumentNumber", DocumentNumber.Text);
                    cmd.Parameters.AddWithValue("@NIP", Nip.Text);
                    cmd.Parameters.AddWithValue("@Notes", Notes.Text);
                    cmd.Parameters.AddWithValue("@Pesel", Pesel.Text);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd podczas aktualizacji danych użytkownika w bazie danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                try
                {
                    connection.Open();

                    string insertUserQuery = @"
                INSERT INTO Users (Name, Address, PostalCode, City, Phone, Email, DocumentType, DocumentNumber, Pesel, NIP, Notes)
                VALUES (@Name, @Address, @PostalCode, @City, @Phone, @Email, @DocumentType, @DocumentNumber, @Pesel, @NIP, @Notes);
                SELECT LAST_INSERT_ID();"; // Dodaj zapytanie do pobrania ostatnio dodanego ID

                    MySqlCommand cmd = new MySqlCommand(insertUserQuery, connection);
                    cmd.Parameters.AddWithValue("@Name", FullName.Text);
                    cmd.Parameters.AddWithValue("@Address", Adress.Text);
                    cmd.Parameters.AddWithValue("@PostalCode", Post_Code.Text);
                    cmd.Parameters.AddWithValue("@City", City.Text);
                    cmd.Parameters.AddWithValue("@Phone", Phone.Text);
                    cmd.Parameters.AddWithValue("@Email", EMail.Text);
                    cmd.Parameters.AddWithValue("@DocumentType", DocumentType.Text);
                    cmd.Parameters.AddWithValue("@DocumentNumber", DocumentNumber.Text);
                    cmd.Parameters.AddWithValue("@Pesel", Pesel.Text);
                    cmd.Parameters.AddWithValue("@NIP", Nip.Text);
                    cmd.Parameters.AddWithValue("@Notes", Notes.Text);

                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd podczas zapisywania użytkownika do bazy danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        private void LoadInvoiceData(int invoiceId)
        {
            try
            {
                connection.Open();

                // Wczytaj dane faktury o identyfikatorze invoiceId z bazy danych
                string query = "SELECT * FROM UKS WHERE ID = @InvoiceID;";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@InvoiceID", invoiceId);

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Description.Text = reader["Description"].ToString();
                    Value.Text = reader["TotalAmount"].ToString();
                    Issue_Date.Value = Convert.ToDateTime(reader["InvoiceDate"]);
                    Notes.Text = reader["Notes"].ToString();
                    issueUserId = reader.GetInt32("USERID");
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas wczytywania danych faktury: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
                LoadUserData(issueUserId);
            }
        }
        private void UpdateInvoiceInDatabase(int invoiceId)
        {
            try
            {
                connection.Open();

                string updateInvoiceQuery = @"
            UPDATE UKS
            SET City = @City, Description = @Description, TotalAmount = @TotalAmount, InvoiceDate = @InvoiceDate, Notes = @Notes
            WHERE ID = @InvoiceID;";

                MySqlCommand cmd = new MySqlCommand(updateInvoiceQuery, connection);
                cmd.Parameters.AddWithValue("@City", City.Text);
                cmd.Parameters.AddWithValue("@Description", Description.Text);
                cmd.Parameters.AddWithValue("@TotalAmount", decimal.Parse(Value.Text));
                cmd.Parameters.AddWithValue("@InvoiceDate", Issue_Date.Value.Date);
                cmd.Parameters.AddWithValue("@Notes", Notes.Text);
                cmd.Parameters.AddWithValue("@InvoiceID", invoiceId);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas aktualizacji danych faktury w bazie danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
