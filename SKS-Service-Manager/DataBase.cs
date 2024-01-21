using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office.Word;
using MySqlConnector;
using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace SKS_Service_Manager
{
    public class DataBase
    {
        private MySqlConnection mySqlConnection;
        private SQLiteConnection sqliteConnection;
        private Settings settingsForm;
        public bool useMySQL;
        private string connectionString;

        public DataBase(Form1 mainForm)
        {
            settingsForm = new Settings(mainForm);
            connectionString = $"Server={settingsForm.GetMySQLHost()};Port={settingsForm.GetMySQLPort()};Database={settingsForm.GetMySQLDatabase()};User ID={settingsForm.GetMySQLUser()};Password={settingsForm.GetMySQLPassword()};";

            useMySQL = IsMySQLConnectionAvailable(mainForm); // Sprawdzamy dostępność połączenia MySQL

            if (useMySQL)
            {
                InitializeMySQLConnection(mainForm);
            }
            else
            {
                InitializeSQLiteConnection();
            }
            CreateInvoicesTableIfNotExists();
            CreateUsersTableIfNotExists();

        }

        public bool IsMySQLConnectionAvailable(Form1 mainForm)
        {
            try
            {
                
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void CloseConnection() {
            if (useMySQL)
            {
                mySqlConnection.Close();
            }
            else
            {
                sqliteConnection.Close();
            }
        }

        private void InitializeMySQLConnection(Form1 mainForm)
        {
            mySqlConnection = new MySqlConnection(connectionString);
        }

        private void InitializeSQLiteConnection()
        {
            string dataBaseFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "umowy");
            string dataBaseFile = Path.Combine(dataBaseFolder, "data.db");

            if (!Directory.Exists(dataBaseFolder))
            {
                Directory.CreateDirectory(dataBaseFolder);
            }

            if (!File.Exists(dataBaseFile))
            {
                SQLiteConnection.CreateFile(dataBaseFile);
            }

            string sqliteConnectionString = $"Data Source={dataBaseFile};Version=3;";
            sqliteConnection = new SQLiteConnection(sqliteConnectionString);
            //MessageBox.Show("Test", "Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void CreateInvoicesTableIfNotExists()
        {
            string createTableQuery = @"
                                    CREATE TABLE IF NOT EXISTS UKS (
                                    ID INT AUTO_INCREMENT PRIMARY KEY,
                                    DocumentType VARCHAR(50),
                                    UserID INT,
                                    City VARCHAR(255),
                                    Description TEXT,
                                    TotalAmount DECIMAL(10, 2),

                                    InvoiceDate DATE,
                                    BuyDate DATE,

                                    Days INT,
                                    Precentage INT,

                                    Fee DECIMAL(10, 2),
                                    LateFee DECIMAL(10, 2),
                                    BuyAmount DECIMAL(10, 2),

                                    DateOfReturn DATE,
                                    SaleDate DATE,
                                    SaleAmount DECIMAL(10, 2),

                                    Notes TEXT
                                    );";
            if (useMySQL)
            {
                try
                {
                    mySqlConnection.Open();
                    MySqlCommand cmd = new MySqlCommand(createTableQuery, mySqlConnection);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd podczas tworzenia tabeli UKS w MySQL: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    mySqlConnection.Close();
                }
            }
            else
            {
                try
                {
                    sqliteConnection.Open();
                    createTableQuery = createTableQuery.Replace("ID INT AUTO_INCREMENT PRIMARY KEY", "ID INTEGER PRIMARY KEY AUTOINCREMENT");
                    SQLiteCommand cmd = new SQLiteCommand(createTableQuery, sqliteConnection);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd podczas tworzenia tabeli UKS w bazie SQLite: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    sqliteConnection.Close();
                }
            }
        }

        public void CreateUsersTableIfNotExists()
        {
            string createTableQuery = @"
                            CREATE TABLE IF NOT EXISTS Users (
                            ID INT AUTO_INCREMENT PRIMARY KEY,
                            Name VARCHAR(255),
                            FullName VARCHAR(255),
                            Nip VARCHAR(255),
                            Address VARCHAR(255),
                            PostalCode VARCHAR(10),
                            City VARCHAR(255),
                            Phone VARCHAR(20),
                            Email VARCHAR(255),
                            DocumentType VARCHAR(50),
                            DocumentNumber VARCHAR(20),
                            Pesel VARCHAR(11),
                            Notes TEXT
                            );";
            if (useMySQL)
            {
                try
                {
                    mySqlConnection.Open();
                    MySqlCommand cmd = new MySqlCommand(createTableQuery, mySqlConnection);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd podczas tworzenia tabeli UKS w MySQL: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    mySqlConnection.Close();
                }
            }
            else
            {
                try
                {
                    sqliteConnection.Open();
                    createTableQuery = createTableQuery.Replace("ID INT AUTO_INCREMENT PRIMARY KEY", "ID INTEGER PRIMARY KEY AUTOINCREMENT");
                    SQLiteCommand cmd = new SQLiteCommand(createTableQuery, sqliteConnection);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd podczas tworzenia tabeli UKS w bazie SQLite: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    sqliteConnection.Close();
                }
            }
        }

        public DataTable uksLoadData()
        {
            try
            {
                string query = "SELECT UKS.ID, " +
                                "UKS.City AS 'Miasto Wystawienia', " +
                                "UKS.Description AS 'Opis', " +
                                "UKS.TotalAmount AS 'Wartość', " +
                                "UKS.InvoiceDate AS 'Data Wystawienia', " +
                                "UKS.Notes AS 'Notatki', " +

                                "Users.FullName AS 'Imię Nazwisko', " +
                                "Users.Address AS 'Ulica Numer', " +
                                "Users.PostalCode AS 'Kod Pocztowy', " +
                                "Users.City AS 'Miasto', " +
                                "Users.Phone AS 'Telefon', " +
                                "Users.Pesel AS 'Pesel', " +
                                "Users.NIP AS 'NIP', " +
                                "Users.Name AS 'Nazwa Firmy'" +

                                "FROM UKS " +
                                "INNER JOIN Users ON UKS.UserID = Users.ID;";

                DataTable dt = new DataTable();

                if (useMySQL)
                {
                    mySqlConnection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                else
                {
                    sqliteConnection.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, sqliteConnection);
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                    adapter.Fill(dt);
                }

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas odczytu danych uksLoadData: " + ex.Message);
            }
            finally
            {
                if (useMySQL)
                {
                    mySqlConnection.Close();
                }
                else
                {
                    sqliteConnection.Close();
                }
            }
            return null;
        }

        public bool DeleteUks(int uksId)
        {
            try
            {
                string deleteQuery = "DELETE FROM UKS WHERE ID = @UksId";
                int rowsAffected;
                if (useMySQL)
                {
                    mySqlConnection.Open();
                    MySqlCommand cmd = new MySqlCommand(deleteQuery, mySqlConnection);
                    cmd.Parameters.AddWithValue("@UksId", uksId);
                    rowsAffected = cmd.ExecuteNonQuery();
                }
                else
                {
                    sqliteConnection.Open();
                    SQLiteCommand cmd = new SQLiteCommand(deleteQuery, sqliteConnection);
                    cmd.Parameters.AddWithValue("@UksId", uksId);
                    rowsAffected = cmd.ExecuteNonQuery();
                }

                if (rowsAffected > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas usuwania faktury UKS: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (useMySQL)
                {
                    mySqlConnection.Close();
                }
                else
                {
                    sqliteConnection.Close();
                }
            }

            return false; // Zwracamy false w przypadku błędu lub braku faktury do usunięcia
        }

        public DataTable loadUserData(int userIdToEdit)
        {
            try
            {
                string query = "SELECT * FROM Users WHERE ID = @UserID;";
                DataTable dt = new DataTable();

                if (useMySQL)
                {
                    mySqlConnection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                    cmd.Parameters.AddWithValue("@UserID", userIdToEdit);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                else
                {
                    sqliteConnection.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, sqliteConnection);
                    cmd.Parameters.AddWithValue("@UserID", userIdToEdit);
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas odczytu danych LoadUserData: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return null;
        }

        public DataTable LoadAllUserData()
        {
            try
            {
                string query = "SELECT " +
                    "ID, " +
                    "FullName AS 'Imię Nazwisko', " +
                    "Address AS 'Ulica Numer', " +
                    "PostalCode AS 'Kod Pocztowy', " +
                    "City AS 'Miasto', " +
                    "Phone AS 'Telefon', " +
                    "Email AS 'E-Mail', " +
                    "DocumentType AS 'Typ Dokumentu', " +
                    "NIP AS 'NIP', " +
                    "Name AS 'Nazwa Firmy', " +
                    "Pesel AS 'Pesel', " +
                    "Notes AS 'Uwagi' " +
                    "FROM Users;";

                DataTable dt = new DataTable();

                if (useMySQL)
                {
                    mySqlConnection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                else
                {
                    sqliteConnection.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, sqliteConnection);
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                    adapter.Fill(dt);
                }

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas odczytu danych: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return null;
        }

        public void DeleteUserFromList(int userId)
        {
            try
            {
                string deleteQuery = "DELETE FROM Users WHERE ID = @ID;";

                if (useMySQL)
                {
                    mySqlConnection.Open();
                    MySqlCommand cmd = new MySqlCommand(deleteQuery, mySqlConnection);
                    cmd.Parameters.AddWithValue("@ID", userId);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    sqliteConnection.Open();
                    SQLiteCommand cmd = new SQLiteCommand(deleteQuery, sqliteConnection);
                    cmd.Parameters.AddWithValue("@ID", userId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas usuwania rekordu: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseConnection();
            }
        }

        public void SaveInvoiceToDatabase(DataTable invoiceData)
        {
            try
            {
                string insertInvoiceQuery = @"
                    INSERT INTO UKS (UserID, City, Description, TotalAmount, InvoiceDate, Notes)
                    VALUES (@UserID, @City, @Description, @TotalAmount, @InvoiceDate, @Notes);";

                if (useMySQL)
                {
                    mySqlConnection.Open();
                    foreach (DataRow row in invoiceData.Rows)
                    {
                        MySqlCommand cmd = new MySqlCommand(insertInvoiceQuery, mySqlConnection);
                        cmd.Parameters.AddWithValue("@UserID", row["UserID"]);
                        cmd.Parameters.AddWithValue("@City", row["City"]);
                        cmd.Parameters.AddWithValue("@Description", row["Description"]);
                        cmd.Parameters.AddWithValue("@TotalAmount", row["TotalAmount"]);
                        cmd.Parameters.AddWithValue("@InvoiceDate", row["InvoiceDate"]);
                        cmd.Parameters.AddWithValue("@Notes", row["Notes"]);
                        cmd.ExecuteScalar();
                    }
                }
                else
                {
                    sqliteConnection.Open();
                    foreach (DataRow row in invoiceData.Rows)
                    {
                        SQLiteCommand cmd = new SQLiteCommand(insertInvoiceQuery, sqliteConnection);
                        cmd.Parameters.AddWithValue("@UserID", row["UserID"]);
                        cmd.Parameters.AddWithValue("@City", row["City"]);
                        cmd.Parameters.AddWithValue("@Description", row["Description"]);
                        cmd.Parameters.AddWithValue("@TotalAmount", row["TotalAmount"]);
                        cmd.Parameters.AddWithValue("@InvoiceDate", row["InvoiceDate"]);
                        cmd.Parameters.AddWithValue("@Notes", row["Notes"]);
                        cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas zapisywania faktury do bazy danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool CheckUserExistsByPesel(string pesel)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Pesel = @Pesel;";

                if (useMySQL)
                {
                    mySqlConnection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                    cmd.Parameters.AddWithValue("@Pesel", pesel);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
                else
                {
                    sqliteConnection.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, sqliteConnection);
                    cmd.Parameters.AddWithValue("@Pesel", pesel);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas sprawdzania użytkownika w bazie danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public void UpdateUserInDatabase(bool exist, string fullName, string name, string address, string postalCode, string city, string phone, string email, string documentType, string documentNumber, string pesel, string nip, string notes)
        {
            try
            {
                string query = "";

                if (exist)
                {
                    query = @"
                UPDATE Users
                SET FullName = @FullName, Name = @Name, Address = @Address, PostalCode = @PostalCode, City = @City, Phone = @Phone, Email = @Email,
                    DocumentType = @DocumentType, DocumentNumber = @DocumentNumber, NIP = @NIP, Notes = @Notes
                WHERE Pesel = @Pesel;";
                }
                else
                {
                    query = @"
                INSERT INTO Users (FullName, Name, Address, PostalCode, City, Phone, Email, DocumentType, DocumentNumber, Pesel, NIP, Notes)
                VALUES (@FullName, @Name, @Address, @PostalCode, @City, @Phone, @Email, @DocumentType, @DocumentNumber, @Pesel, @NIP, @Notes);";
                }

                if (useMySQL)
                {
                    mySqlConnection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                    cmd.Parameters.AddWithValue("@FullName", fullName);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@PostalCode", postalCode);
                    cmd.Parameters.AddWithValue("@City", city);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@DocumentType", documentType);
                    cmd.Parameters.AddWithValue("@DocumentNumber", documentNumber);
                    cmd.Parameters.AddWithValue("@Pesel", pesel);
                    cmd.Parameters.AddWithValue("@NIP", nip);
                    cmd.Parameters.AddWithValue("@Notes", notes);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    sqliteConnection.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, sqliteConnection);
                    cmd.Parameters.AddWithValue("@FullName", fullName);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@PostalCode", postalCode);
                    cmd.Parameters.AddWithValue("@City", city);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@DocumentType", documentType);
                    cmd.Parameters.AddWithValue("@DocumentNumber", documentNumber);
                    cmd.Parameters.AddWithValue("@Pesel", pesel);
                    cmd.Parameters.AddWithValue("@NIP", nip);
                    cmd.Parameters.AddWithValue("@Notes", notes);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas aktualizacji/dodawania użytkownika do bazy danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseConnection();
            }
        }

        public DataTable LoadInvoiceData(int invoiceId)
        {
            try
            {
                string query = "SELECT * FROM UKS WHERE ID = @ID;";
                DataTable dt = new DataTable();

                if (useMySQL)
                {
                    mySqlConnection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                    cmd.Parameters.AddWithValue("@ID", invoiceId);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                else
                {
                    sqliteConnection.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, sqliteConnection);
                    cmd.Parameters.AddWithValue("@ID", invoiceId);
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas odczytu danych LoadInvoiceData: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return null;
        }

        public void UpdateInvoiceInDatabase(int invoiceId, string city, string description, decimal totalAmount, DateTime invoiceDate, string notes)
        {
            try
            {
                string query = @"
                    UPDATE UKS
                    SET City = @City, Description = @Description, TotalAmount = @TotalAmount, InvoiceDate = @InvoiceDate, Notes = @Notes
                    WHERE ID = @InvoiceID;";

                if (useMySQL)
                {
                    mySqlConnection.Open();
                    MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                    cmd.Parameters.AddWithValue("@City", city);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                    cmd.Parameters.AddWithValue("@InvoiceDate", invoiceDate);
                    cmd.Parameters.AddWithValue("@Notes", notes);
                    cmd.Parameters.AddWithValue("@InvoiceID", invoiceId);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    sqliteConnection.Open();
                    SQLiteCommand cmd = new SQLiteCommand(query, sqliteConnection);
                    cmd.Parameters.AddWithValue("@City", city);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                    cmd.Parameters.AddWithValue("@InvoiceDate", invoiceDate);
                    cmd.Parameters.AddWithValue("@Notes", notes);
                    cmd.Parameters.AddWithValue("@InvoiceID", invoiceId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas aktualizacji danych faktury w bazie danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseConnection();
            }
        }

    }
}
