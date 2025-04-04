﻿using MySqlConnector;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Threading.Tasks;

#pragma warning disable
namespace SKS_Service_Manager
{
    public class DataBase
    {
        private MySqlConnection mySqlConnection;
        private SQLiteConnection sqliteConnection;
        private Settings settingsForm;
        private Form1 mainForms;
        Logger logger;
        public bool useMySQL;
        private string connectionString;

        public event EventHandler<int> ProgressChanged;
        public event EventHandler<int[]> ProgressTotal;

        public DataBase(Form1 mainForm)
        {
            logger = new Logger("log.txt");
            settingsForm = new Settings(mainForm);
            connectionString = $"Server={settingsForm.GetMySQLHost()};Port={settingsForm.GetMySQLPort()};Database={settingsForm.GetMySQLDatabase()};User ID={settingsForm.GetMySQLUser()};Password={settingsForm.GetMySQLPassword()};";

            mainForms = mainForm;
            useMySQL = IsMySQLConnectionAvailable(mainForm); // Sprawdzamy dostępność połączenia MySQL

            if (useMySQL)
            {
                InitializeMySQLConnection(mainForm);
            }
            else
            {
                InitializeSQLiteConnection();
            }
            CreateUsersTableIfNotExists();
            CreateInvoicesTableIfNotExists();

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
            catch (Exception ex)
            {
                logger.LogError("IsMySQLConnectionAvailable() - Błąd podczas połączenia z bazą danych: " + ex.Message);
                return false;
            }
        }

        private void OpenConnection()
        {
            if (useMySQL)
            {
                if (mySqlConnection.State == ConnectionState.Closed)
                {
                    mySqlConnection.Open();
                }
            }
            else
            {
                if (sqliteConnection.State == ConnectionState.Closed)
                {
                    sqliteConnection.Open();
                }
            }

        }

        private void CloseConnection()
        {
            /*            if (useMySQL)
                        {
                            mySqlConnection.Close();
                        }
                        else
                        {
                            sqliteConnection.Close();
                        }*/
        }

        public void RefreshConnection()
        {
            if (useMySQL)
            {
                mySqlConnection.Close();
                InitializeMySQLConnection(mainForms);
            }
            else
            {
                sqliteConnection.Close();
                InitializeSQLiteConnection();
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
                                        UserID INT,
                                        DocumentType VARCHAR(50),
                                        City VARCHAR(100),
                                        Description TEXT,
                                        TotalAmount DECIMAL(10,2),
                                        EstimatedValue DECIMAL(10,2),
                                        InvoiceDate DATE,
                                        BuyDate DATE,
                                        Notes TEXT,
                                        NIP VARCHAR(20),
                                        Days INT,
                                        Percentage DOUBLE(10,2),
                                        Fee DECIMAL(10,2),
                                        LateFee DECIMAL(10,2),
                                        Commision DECIMAL(10,2),
                                        BuyAmount DECIMAL(10,2),
                                        DateOfReturn DATE,
                                        SaleDate DATE,
                                        SaleAmount DECIMAL(10,2),
                                        FOREIGN KEY (UserID) REFERENCES Users(ID)
                                    );
                                ";
            OpenConnection();

            if (useMySQL)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(createTableQuery, mySqlConnection);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    logger.LogError("CreateInvoicesTableIfNotExists() - Błąd podczas tworzenia struktury tabeli INVOICES w MySQL: " + ex.Message);
                }
            }
            else
            {
                try
                {
                    createTableQuery = createTableQuery.Replace("ID INT AUTO_INCREMENT PRIMARY KEY", "ID INTEGER PRIMARY KEY AUTOINCREMENT");
                    SQLiteCommand cmd = new SQLiteCommand(createTableQuery, sqliteConnection);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    logger.LogError("CreateInvoicesTableIfNotExists() - Błąd podczas tworzenia struktury tabeli INVOICES w bazie SQLite: " + ex.Message);
                }
            }

            CloseConnection();
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

            OpenConnection();
            if (useMySQL)
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(createTableQuery, mySqlConnection);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    logger.LogError("CreateUsersTableIfNotExists() - Błąd podczas tworzenia struktury tabeli USERS w MySQL: " + ex.Message);
                }
            }
            else
            {
                try
                {
                    createTableQuery = createTableQuery.Replace("ID INT AUTO_INCREMENT PRIMARY KEY", "ID INTEGER PRIMARY KEY AUTOINCREMENT");
                    SQLiteCommand cmd = new SQLiteCommand(createTableQuery, sqliteConnection);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    logger.LogError("CreateUsersTableIfNotExists() - Błąd podczas tworzenia struktury tabeli USERS w bazie SQLite: " + ex.Message);
                }
            }
            CloseConnection();
        }

        public DataTable uksLoadDataByDateRange(DateTime fromDate, DateTime toDate, string city, string documentType, bool byRealizedDate)
        {
            try
            {
                string query = "";
                if (useMySQL)
                {
                    query = "SELECT " +
                            "DATE_FORMAT(UKS.InvoiceDate, '%d.%m.%Y') AS 'Data Przyjęcia', " +
                            "CAST(UKS.TotalAmount AS decimal(10, 2)) AS 'Kwota zapłacona klientowi', " +
                            "UKS.Description AS 'Dokładny opis przedmiotów', " +
                            "CAST(UKS.BuyAmount AS decimal(10, 2)) AS 'Wartość sprzedaży minus zużycie', " +
                            "DATE_FORMAT(UKS.BuyDate, '%d.%m.%Y') AS 'Ostateczny termin do odkupu', " +
                            "DATE_FORMAT(UKS.DateOfReturn, '%d.%m.%Y') AS 'Data zwrotu', " +
                            "DATE_FORMAT(UKS.SaleDate, '%d.%m.%Y') AS 'Data sprzedaży', " +
                            "CAST(UKS.SaleAmount AS decimal(10, 2)) AS 'Kwota sprzedaży', " +
                            "CAST((UKS.SaleAmount - UKS.TotalAmount) AS decimal(10, 2)) AS 'Kwota uzyskanej prowizji albo odkupu', " +
                            "UKS.Notes AS 'Uwagi' " +
                            "FROM UKS ";

                    // Warunek dla dat realizacji (tylko po Data zwrotu lub Data sprzedaży)
                    if (byRealizedDate)
                    {
                        query += "WHERE (UKS.DateOfReturn BETWEEN @FromDate AND @ToDate OR UKS.SaleDate BETWEEN @FromDate AND @ToDate) ";
                    }
                    else // Warunek dla dat wystawienia (tylko po Data wystawienia)
                    {
                        query += "WHERE UKS.InvoiceDate BETWEEN @FromDate AND @ToDate ";
                    }

                    // Dodaj warunki dla miasta i rodzaju umowy
                    if (!string.IsNullOrEmpty(city) && city != "Wszystko")
                    {
                        query += " AND UKS.City = @City ";
                    }
                    if (!string.IsNullOrEmpty(documentType) && documentType != "Wszystko")
                    {
                        query += " AND UKS.DocumentType = @DocumentType ";
                    }

                    // Dodaj klauzulę sortowania
                    query += " ORDER BY UKS.InvoiceDate DESC";
                }
                else
                {
                    // Zapytanie dla SQLite
                    query = "SELECT " +
                            "strftime('%d.%m.%Y', UKS.InvoiceDate) AS 'Data Przyjęcia', " +
                            "CAST(UKS.TotalAmount AS decimal(10, 2)) AS 'Kwota zapłacona klientowi', " +
                            "UKS.Description AS 'Dokładny opis przedmiotów', " +
                            "CAST(UKS.BuyAmount AS decimal(10, 2)) AS 'Wartość sprzedaży minus zużycie', " +
                            "strftime('%d.%m.%Y', UKS.BuyDate) AS 'Ostateczny termin do odkupu', " +
                            "strftime('%d.%m.%Y', UKS.DateOfReturn) AS 'Data zwrotu', " +
                            "strftime('%d.%m.%Y', UKS.SaleDate) AS 'Data sprzedaży', " +
                            "CAST(UKS.SaleAmount AS decimal(10, 2)) AS 'Kwota sprzedaży', " +
                            "CAST((UKS.SaleAmount - UKS.TotalAmount) AS decimal(10, 2)) AS 'Kwota uzyskanej prowizji albo odkupu', " +
                            "UKS.Notes AS 'Uwagi' " +
                            "FROM UKS ";

                    // Warunek dla dat realizacji (tylko po Data zwrotu lub Data sprzedaży)
                    if (byRealizedDate)
                    {
                        query += "WHERE (UKS.DateOfReturn BETWEEN @FromDate AND @ToDate OR UKS.SaleDate BETWEEN @FromDate AND @ToDate) ";
                    }
                    else // Warunek dla dat wystawienia (tylko po Data wystawienia)
                    {
                        query += "WHERE UKS.InvoiceDate BETWEEN @FromDate AND @ToDate ";
                    }

                    // Dodaj warunki dla miasta i rodzaju umowy
                    if (!string.IsNullOrEmpty(city) && city != "Wszystko")
                    {
                        query += " AND UKS.City = @City ";
                    }
                    if (!string.IsNullOrEmpty(documentType) && documentType != "Wszystko")
                    {
                        query += " AND UKS.DocumentType = @DocumentType ";
                    }

                    // Dodaj klauzulę sortowania
                    query += " ORDER BY UKS.InvoiceDate DESC";
                }

                DataTable dt = new DataTable();

                OpenConnection();
                if (useMySQL)
                {
                    MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);

                    if (!string.IsNullOrEmpty(city) && city != "Wszystko") { cmd.Parameters.AddWithValue("@City", city); }
                    if (!string.IsNullOrEmpty(documentType) && documentType != "Wszystko") { cmd.Parameters.AddWithValue("@DocumentType", documentType); }

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                else
                {
                    SQLiteCommand cmd = new SQLiteCommand(query, sqliteConnection);
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);

                    if (!string.IsNullOrEmpty(city) && city != "Wszystko") { cmd.Parameters.AddWithValue("@City", city); }
                    if (!string.IsNullOrEmpty(documentType) && documentType != "Wszystko") { cmd.Parameters.AddWithValue("@DocumentType", documentType); }

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                    adapter.Fill(dt);
                }

                return dt;
            }
            catch (Exception ex)
            {
                logger.LogError("uksLoadDataByDateRange() - Błąd podczas odczytu danych uksLoadDataByDateRange: " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return null;
        }

        public List<string> GetUniqueCities()
        {
            List<string> cities = new List<string>();
            try
            {
                string query = "SELECT DISTINCT City FROM UKS";

                DataTable dt = new DataTable();

                if (useMySQL)
                {
                    using (MySqlConnection tempConnection = new MySqlConnection(connectionString))
                    {
                        tempConnection.Open();
                        MySqlCommand cmd = new MySqlCommand(query, tempConnection);
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        adapter.Fill(dt);
                    }
                }
                else
                {
                    SQLiteCommand cmd = new SQLiteCommand(query, sqliteConnection);
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                    adapter.Fill(dt);
                }

                foreach (DataRow row in dt.Rows)
                {
                    string city = row["City"].ToString();

                    // Sprawdź, czy miasto nie jest puste, a następnie sformatuj je: pierwsza litera wielka, reszta mała
                    if (!string.IsNullOrWhiteSpace(city))
                    {
                        city = char.ToUpper(city[0]) + city.Substring(1).ToLower();
                        cities.Add(city);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas pobierania unikalnych miast: " + ex.Message);
            }
            return cities;
        }



        public bool DeleteUks(int uksId)
        {
            try
            {
                string deleteQuery = "DELETE FROM UKS WHERE ID = @UksId";
                int rowsAffected;

                OpenConnection();
                if (useMySQL)
                {
                    MySqlCommand cmd = new MySqlCommand(deleteQuery, mySqlConnection);
                    cmd.Parameters.AddWithValue("@UksId", uksId);
                    rowsAffected = cmd.ExecuteNonQuery();
                }
                else
                {
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
                CloseConnection();
            }

            return false; // Zwracamy false w przypadku błędu lub braku faktury do usunięcia
        }

        public DataTable loadUserData(int userIdToEdit)
        {
            try
            {
                string query = "SELECT * FROM Users WHERE ID = @UserID;";
                DataTable dt = new DataTable();

                OpenConnection();
                if (useMySQL)
                {
                    MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                    cmd.Parameters.AddWithValue("@UserID", userIdToEdit);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                else
                {
                    SQLiteCommand cmd = new SQLiteCommand(query, sqliteConnection);
                    cmd.Parameters.AddWithValue("@UserID", userIdToEdit);
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                logger.LogError("loadUserData() - Błąd podczas odczytu danych LoadUserData: " + ex.Message);
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

                OpenConnection();
                if (useMySQL)
                {
                    MySqlCommand cmd = new MySqlCommand(deleteQuery, mySqlConnection);
                    cmd.Parameters.AddWithValue("@ID", userId);
                    cmd.ExecuteNonQuery();
                }
                else
                {
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
            INSERT INTO UKS (UserID, DocumentType, City, Description, TotalAmount, InvoiceDate, BuyDate, Notes, Days, Percentage, Fee, LateFee, Commision, BuyAmount, DateOfReturn, SaleDate, SaleAmount, EstimatedValue
                ) VALUES (
                    @UserID, @DocumentType, @City, @Description, @TotalAmount, @InvoiceDate, @BuyDate, @Notes, @Days, @Percentage, @Fee, @LateFee, @Commision, @BuyAmount, @DateOfReturn, @SaleDate, @SaleAmount, @EstimatedValue
                );";

                OpenConnection();
                if (useMySQL)
                {
                    foreach (DataRow row in invoiceData.Rows)
                    {
                        MySqlCommand cmd = new MySqlCommand(insertInvoiceQuery, mySqlConnection);
                        cmd.Parameters.AddWithValue("@UserID", row["UserID"]);
                        cmd.Parameters.AddWithValue("@DocumentType", row["DocumentType"]);
                        cmd.Parameters.AddWithValue("@City", row["City"]);
                        cmd.Parameters.AddWithValue("@Description", row["Description"]);
                        cmd.Parameters.AddWithValue("@TotalAmount", row["TotalAmount"]);
                        cmd.Parameters.AddWithValue("@EstimatedValue", row["EstimatedValue"]);
                        cmd.Parameters.AddWithValue("@InvoiceDate", row["InvoiceDate"]);
                        cmd.Parameters.AddWithValue("@BuyDate", row["BuyDate"]);
                        cmd.Parameters.AddWithValue("@Days", row["Days"]);
                        cmd.Parameters.AddWithValue("@Percentage", row["Percentage"]);
                        cmd.Parameters.AddWithValue("@Fee", row["Fee"]);
                        cmd.Parameters.AddWithValue("@LateFee", row["LateFee"]);
                        cmd.Parameters.AddWithValue("@Commision", row["Commision"]);
                        cmd.Parameters.AddWithValue("@BuyAmount", row["BuyAmount"]);
                        cmd.Parameters.AddWithValue("@DateOfReturn", row["DateOfReturn"]);
                        cmd.Parameters.AddWithValue("@SaleDate", row["SaleDate"]);
                        cmd.Parameters.AddWithValue("@SaleAmount", row["SaleAmount"]);
                        cmd.Parameters.AddWithValue("@Notes", row["Notes"]);
                        cmd.ExecuteScalar();
                    }
                }
                else
                {
                    foreach (DataRow row in invoiceData.Rows)
                    {
                        SQLiteCommand cmd = new SQLiteCommand(insertInvoiceQuery, sqliteConnection);
                        cmd.Parameters.AddWithValue("@DocumentType", row["DocumentType"]);
                        cmd.Parameters.AddWithValue("@UserID", row["UserID"]);
                        cmd.Parameters.AddWithValue("@City", row["City"]);
                        cmd.Parameters.AddWithValue("@Description", row["Description"]);
                        cmd.Parameters.AddWithValue("@TotalAmount", row["TotalAmount"]);
                        cmd.Parameters.AddWithValue("@EstimatedValue", row["EstimatedValue"]);
                        cmd.Parameters.AddWithValue("@InvoiceDate", row["InvoiceDate"]);
                        cmd.Parameters.AddWithValue("@BuyDate", row["BuyDate"]);
                        cmd.Parameters.AddWithValue("@Days", row["Days"]);
                        cmd.Parameters.AddWithValue("@Percentage", row["Percentage"]);
                        cmd.Parameters.AddWithValue("@Fee", row["Fee"]);
                        cmd.Parameters.AddWithValue("@LateFee", row["LateFee"]);
                        cmd.Parameters.AddWithValue("@Commision", row["Commision"]);
                        cmd.Parameters.AddWithValue("@BuyAmount", row["BuyAmount"]);
                        cmd.Parameters.AddWithValue("@DateOfReturn", row["DateOfReturn"]);
                        cmd.Parameters.AddWithValue("@SaleDate", row["SaleDate"]);
                        cmd.Parameters.AddWithValue("@SaleAmount", row["SaleAmount"]);
                        cmd.Parameters.AddWithValue("@Notes", row["Notes"]);
                        cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas zapisywania faktury do bazy danych SaveInvoice: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseConnection();
            }
        }

        public int CheckUserExistsByPesel(string pesel)
        {
            try
            {
                string query = "SELECT ID FROM Users WHERE Pesel = @Pesel;";

                OpenConnection();
                if (useMySQL)
                {
                    MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                    cmd.Parameters.AddWithValue("@Pesel", pesel);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                }
                else
                {
                    SQLiteCommand cmd = new SQLiteCommand(query, sqliteConnection);
                    cmd.Parameters.AddWithValue("@Pesel", pesel);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas sprawdzania użytkownika w bazie danych Pesel: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseConnection();
            }
            return -1; // Zwraca -1, jeśli użytkownik nie został znaleziony
        }

        public int CheckUserExistsByDokNr(string dokNr)
        {
            try
            {
                string query = "SELECT ID FROM Users WHERE DocumentNumber = @DokNr;";

                OpenConnection();
                if (useMySQL)
                {
                    MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                    cmd.Parameters.AddWithValue("@DokNr", dokNr);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                }
                else
                {
                    SQLiteCommand cmd = new SQLiteCommand(query, sqliteConnection);
                    cmd.Parameters.AddWithValue("@DokNr", dokNr);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas sprawdzania użytkownika w bazie danych DokNr: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseConnection();
            }
            return -1; // Zwraca -1, jeśli użytkownik nie został znaleziony
        }

        public int CheckUserExistsByNameAndAdress(string Address, string FullName)
        {
            try
            {
                string query = "SELECT ID FROM Users WHERE FullName = @FullName AND Address = @Address;";

                OpenConnection();
                if (useMySQL)
                {
                    MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                    cmd.Parameters.AddWithValue("@FullName", FullName);
                    cmd.Parameters.AddWithValue("@Address", Address);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                }
                else
                {
                    SQLiteCommand cmd = new SQLiteCommand(query, sqliteConnection);
                    cmd.Parameters.AddWithValue("@FullName", FullName);
                    cmd.Parameters.AddWithValue("@Address", Address);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas sprawdzania użytkownika w bazie danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseConnection();
            }
            return -1; // Zwraca -1, jeśli użytkownik nie został znaleziony
        }

        public int CheckUserExistsByNameAndCity(string City, string FullName)
        {
            try
            {
                string query = "SELECT ID FROM Users WHERE FullName = @FullName AND City = @City;";

                OpenConnection();
                if (useMySQL)
                {
                    MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                    cmd.Parameters.AddWithValue("@FullName", FullName);
                    cmd.Parameters.AddWithValue("@City", City);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                }
                else
                {
                    SQLiteCommand cmd = new SQLiteCommand(query, sqliteConnection);
                    cmd.Parameters.AddWithValue("@FullName", FullName);
                    cmd.Parameters.AddWithValue("@City", City);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas sprawdzania użytkownika w bazie danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseConnection();
            }
            return -1; // Zwraca -1, jeśli użytkownik nie został znaleziony
        }

        public int CheckUserExists(string Pesel, string docNumber, string Adress, string City, string FullName)
        {
            if (!string.IsNullOrEmpty(Pesel))
            {
                return CheckUserExistsByPesel(Pesel);
            }
            if (!string.IsNullOrEmpty(docNumber))
            {
                return CheckUserExistsByDokNr(docNumber);
            }
            if (!string.IsNullOrEmpty(Adress) && !string.IsNullOrEmpty(FullName))
            {
                return CheckUserExistsByNameAndAdress(Adress, FullName);
            }
            if (!string.IsNullOrEmpty(City) && !string.IsNullOrEmpty(City))
            {
                return CheckUserExistsByNameAndCity(City, FullName);
            }
            return -1; // Zwraca -1, jeśli użytkownik nie został znaleziony
        }

        public void UpdateUserInDatabase(int userId, string fullName, string name, string address, string postalCode, string city, string phone, string email, string documentType, string documentNumber, string pesel, string nip, string notes)
        {
            try
            {
                string query = "";

                if (userId != -1)
                {
                    query = @"
                UPDATE Users
                SET FullName = @FullName, Name = @Name, Address = @Address, PostalCode = @PostalCode, City = @City, Phone = @Phone, Email = @Email,
                    DocumentType = @DocumentType, DocumentNumber = @DocumentNumber, NIP = @NIP, Notes = @Notes
                WHERE ID = @UserID;";
                }
                else
                {
                    query = @"
                INSERT INTO Users (FullName, Name, Address, PostalCode, City, Phone, Email, DocumentType, DocumentNumber, Pesel, NIP, Notes)
                VALUES (@FullName, @Name, @Address, @PostalCode, @City, @Phone, @Email, @DocumentType, @DocumentNumber, @Pesel, @NIP, @Notes);";
                }

                OpenConnection();
                if (useMySQL)
                {
                    MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                    cmd.Parameters.AddWithValue("@UserID", userId);
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
                    SQLiteCommand cmd = new SQLiteCommand(query, sqliteConnection);
                    cmd.Parameters.AddWithValue("@UserID", userId);
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

                OpenConnection();
                if (useMySQL)
                {
                    MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                    cmd.Parameters.AddWithValue("@ID", invoiceId);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                else
                {
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

        public void UpdateInvoiceInDatabase(int invoiceId, int userId, string city, string description, decimal totalAmount, DateTime invoiceDate, string notes, string documentType, DateTime buyDate, int days, double percentage, decimal fee, string nip, decimal lateFee, decimal commision, decimal buyAmount, DateTime dateOfReturn, DateTime saleDate, decimal saleAmount, decimal estimatedValue)
        {
            try
            {
                string query = @"
                    UPDATE UKS
                    SET
                        UserID = @UserID,
                        DocumentType = @DocumentType,
                        City = @City,
                        Description = @Description,
                        TotalAmount = @TotalAmount,
                        InvoiceDate = @InvoiceDate,
                        BuyDate = @BuyDate,
                        Notes = @Notes,
                        NIP = @NIP,
                        Days = @Days,
                        Percentage = @Percentage,
                        Fee = @Fee,
                        LateFee = @LateFee,
                        Commision = @Commision,
                        BuyAmount = @BuyAmount,
                        DateOfReturn = @DateOfReturn,
                        SaleDate = @SaleDate,
                        SaleAmount = @SaleAmount,
                        EstimatedValue = @EstimatedValue -- Dodano EstimatedValue
                    WHERE ID = @InvoiceId;
                ";

                OpenConnection();
                if (useMySQL)
                {
                    MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                    cmd.Parameters.AddWithValue("@City", city);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                    cmd.Parameters.AddWithValue("@EstimatedValue", estimatedValue);
                    cmd.Parameters.AddWithValue("@InvoiceDate", invoiceDate);
                    cmd.Parameters.AddWithValue("@Notes", notes);
                    cmd.Parameters.AddWithValue("@DocumentType", documentType);
                    cmd.Parameters.AddWithValue("@BuyDate", buyDate);
                    cmd.Parameters.AddWithValue("@Days", days);
                    cmd.Parameters.AddWithValue("@Percentage", Math.Round(percentage, 2));
                    cmd.Parameters.AddWithValue("@Fee", fee);
                    cmd.Parameters.AddWithValue("@NIP", nip);
                    cmd.Parameters.AddWithValue("@LateFee", lateFee);
                    cmd.Parameters.AddWithValue("@Commision", commision);
                    cmd.Parameters.AddWithValue("@BuyAmount", buyAmount);
                    cmd.Parameters.AddWithValue("@DateOfReturn", dateOfReturn);
                    cmd.Parameters.AddWithValue("@SaleDate", saleDate);
                    cmd.Parameters.AddWithValue("@SaleAmount", saleAmount);
                    cmd.Parameters.AddWithValue("@InvoiceID", invoiceId);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    SQLiteCommand cmd = new SQLiteCommand(query, sqliteConnection);
                    cmd.Parameters.AddWithValue("@City", city);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                    cmd.Parameters.AddWithValue("@EstimatedValue", estimatedValue);
                    cmd.Parameters.AddWithValue("@InvoiceDate", invoiceDate);
                    cmd.Parameters.AddWithValue("@Notes", notes);
                    cmd.Parameters.AddWithValue("@DocumentType", documentType);
                    cmd.Parameters.AddWithValue("@BuyDate", buyDate);
                    cmd.Parameters.AddWithValue("@Days", days);
                    cmd.Parameters.AddWithValue("@Percentage", Math.Round(percentage, 2));
                    cmd.Parameters.AddWithValue("@Fee", fee);
                    cmd.Parameters.AddWithValue("@NIP", nip);
                    cmd.Parameters.AddWithValue("@LateFee", lateFee);
                    cmd.Parameters.AddWithValue("@Commision", commision);
                    cmd.Parameters.AddWithValue("@BuyAmount", buyAmount);
                    cmd.Parameters.AddWithValue("@DateOfReturn", dateOfReturn);
                    cmd.Parameters.AddWithValue("@SaleDate", saleDate);
                    cmd.Parameters.AddWithValue("@SaleAmount", saleAmount);
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

        public bool CheckInvoiceExists(int id)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM UKS WHERE ID = @ID;";

                OpenConnection();
                if (useMySQL)
                {
                    MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                    cmd.Parameters.AddWithValue("@ID", id);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
                else
                {
                    SQLiteCommand cmd = new SQLiteCommand(query, sqliteConnection);
                    cmd.Parameters.AddWithValue("@ID", id);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas sprawdzania umów w bazie danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public bool CheckInvoiceExists(string description, decimal totalAmount)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM UKS WHERE Description = @Description AND TotalAmount = @TotalAmount;";

                OpenConnection();
                if (useMySQL)
                {
                    MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
                else
                {
                    SQLiteCommand cmd = new SQLiteCommand(query, sqliteConnection);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas sprawdzania umów w bazie danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }

        public async Task AddUsersFromGeneratedDatabaseAsync(string generatedDatabaseFilePath)
        {
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test-log.txt");

            try
            {
                string generatedDatabaseConnectionString = $"Data Source={generatedDatabaseFilePath};Version=3;";
                SQLiteConnection generatedDatabaseConnection = new SQLiteConnection(generatedDatabaseConnectionString);
                generatedDatabaseConnection.Open();

                string countQuery = "SELECT COUNT(*) FROM lombard";
                SQLiteCommand countCommand = new SQLiteCommand(countQuery, generatedDatabaseConnection);

                int totalCount = Convert.ToInt32(countCommand.ExecuteScalar());
                int processedCount = 0;

                string query = "SELECT KLIENT_NAZWISKO_IMIE, KLIENT_KOD_POCZT, KLIENT_MIEJSCOW, KLIENT_ULICA, KLIENT_TEL_KONTAKT, KLIENT_DOK_TYP, KLIENT_DOK_NR, KLIENT_PESEL, FIRMA_MIEJSCOW, PRZEDMIOT_OPIS, PRZEDMIOT_WARTOSC, DATA_PRZYJECIA, TERMIN_ODBIORU, UMOWA_ILOSC_DNI, UMOWA_PROCENT, OPLATA, OPLATA_OPOZNIENIE, KWOTA_WYKUPU, FAKT_ODBIOR_DATA, SPRZEDAZ_DATA, SPRZEDAZ_KWOTA, UWAGI FROM lombard";

                SQLiteCommand command = new SQLiteCommand(query, generatedDatabaseConnection);
                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    try
                    {
                        string nazwiskoImie = reader["KLIENT_NAZWISKO_IMIE"].ToString();
                        string odwroconeImieNazwisko = string.Empty;
                        string[] parts = nazwiskoImie.Split(' ');
                        if (parts.Length >= 2)
                        {
                            odwroconeImieNazwisko = parts[1] + " " + parts[0];
                        }
                        else if (parts.Length == 1)
                        {
                            odwroconeImieNazwisko = parts[0];
                        }

                        string kodPocztowy = reader["KLIENT_KOD_POCZT"].ToString();
                        string miejscowosc = reader["KLIENT_MIEJSCOW"].ToString();
                        string ulica = reader["KLIENT_ULICA"].ToString();
                        string telefon = reader["KLIENT_TEL_KONTAKT"].ToString();
                        string dokumentTyp = reader["KLIENT_DOK_TYP"].ToString();
                        string dokumentNumer = reader["KLIENT_DOK_NR"].ToString();
                        string pesel = reader["KLIENT_PESEL"].ToString();
                        string City = reader["FIRMA_MIEJSCOW"].ToString();
                        string Description = reader["PRZEDMIOT_OPIS"].ToString();
                        string TotalAmount = reader["PRZEDMIOT_WARTOSC"].ToString().Replace(".", ",");
                        string InvoiceDate = reader["DATA_PRZYJECIA"].ToString();
                        string BuyDate = reader["TERMIN_ODBIORU"].ToString();
                        string Days = reader["UMOWA_ILOSC_DNI"].ToString();
                        string PercentageString = reader["UMOWA_PROCENT"].ToString().Replace(".", ",");

                        // Zmiana na double i zaokrąglenie
                        double Percentage = Math.Round(string.IsNullOrEmpty(PercentageString) ? 0 : Convert.ToDouble(PercentageString), 2);

                        string Fee = reader["OPLATA"].ToString().Replace(".", ",");
                        string LateFee = reader["OPLATA_OPOZNIENIE"].ToString().Replace(".", ",");
                        string BuyAmount = reader["KWOTA_WYKUPU"].ToString().Replace(".", ",");
                        string DateOfReturn = reader["FAKT_ODBIOR_DATA"].ToString();
                        string SaleDate = reader["SPRZEDAZ_DATA"].ToString();
                        string SaleAmount = reader["SPRZEDAZ_KWOTA"].ToString().Replace(".", ",");
                        string Notes = reader["UWAGI"].ToString();

                        int userid = CheckUserExistsByNameAndCity(miejscowosc, odwroconeImieNazwisko);

                        if (userid == -1)
                        {
                            UpdateUserInDatabase(userid, odwroconeImieNazwisko, "", ulica, kodPocztowy, miejscowosc, telefon, "", "Dowód Osobisty", dokumentNumer, "", "", "");
                            userid = CheckUserExists(pesel, dokumentNumer, ulica, miejscowosc, odwroconeImieNazwisko);
                        }

                        if (userid != -1 && !CheckInvoiceExists(Description, string.IsNullOrEmpty(TotalAmount) ? 0 : decimal.Parse(TotalAmount)))
                        {
                            System.Data.DataTable invoiceData = new System.Data.DataTable();
                            invoiceData.Columns.Add("UserID", typeof(int));
                            invoiceData.Columns.Add("DocumentType", typeof(string));
                            invoiceData.Columns.Add("City", typeof(string));
                            invoiceData.Columns.Add("Description", typeof(string));
                            invoiceData.Columns.Add("TotalAmount", typeof(decimal));
                            invoiceData.Columns.Add("EstimatedValue", typeof(decimal));
                            invoiceData.Columns.Add("Commision", typeof(decimal));
                            invoiceData.Columns.Add("InvoiceDate", typeof(DateTime));
                            invoiceData.Columns.Add("BuyDate", typeof(DateTime));
                            invoiceData.Columns.Add("Notes", typeof(string));
                            invoiceData.Columns.Add("Days", typeof(int));
                            invoiceData.Columns.Add("Percentage", typeof(double));  // Zmiana typu na double
                            invoiceData.Columns.Add("Fee", typeof(decimal));
                            invoiceData.Columns.Add("LateFee", typeof(decimal));
                            invoiceData.Columns.Add("BuyAmount", typeof(decimal));
                            invoiceData.Columns.Add("DateOfReturn", typeof(DateTime));
                            invoiceData.Columns.Add("SaleDate", typeof(DateTime));
                            invoiceData.Columns.Add("SaleAmount", typeof(decimal));
                            invoiceData.Columns.Add("NIP", typeof(string));

                            DataRow newRow = invoiceData.NewRow();
                            newRow["UserID"] = userid;
                            newRow["City"] = City;
                            newRow["DocumentType"] = string.IsNullOrEmpty(TotalAmount) ? "Umowa Kupna-Sprzedaży" : (decimal.Parse(TotalAmount) > 1000 ? "Umowa Komisowa" : "Umowa Kupna-Sprzedaży");
                            newRow["Description"] = Description;
                            newRow["TotalAmount"] = string.IsNullOrEmpty(TotalAmount) ? 0 : decimal.Parse(TotalAmount);
                            newRow["EstimatedValue"] = 0;
                            newRow["Commision"] = 0;

                            DateTime invoiceDateTime = DateTime.TryParse(InvoiceDate, out DateTime parsedInvoiceDate) ? parsedInvoiceDate : new DateTime(1753, 1, 1);
                            DateTime buyDateTime = DateTime.TryParse(BuyDate, out DateTime parsedBuyDate) ? parsedBuyDate : new DateTime(1753, 1, 1);

                            newRow["InvoiceDate"] = invoiceDateTime;
                            newRow["BuyDate"] = buyDateTime.Equals(invoiceDateTime) ? buyDateTime.AddMonths(1) : buyDateTime;

                            newRow["Notes"] = Notes;

                            newRow["Days"] = string.IsNullOrEmpty(Days) || int.Parse(Days) == 0 ? 30 : int.Parse(Days);
                            newRow["Percentage"] = Percentage;  // Zmienione z int na double
                            newRow["Fee"] = string.IsNullOrEmpty(Fee) ? 0 : decimal.Parse(Fee);
                            newRow["LateFee"] = string.IsNullOrEmpty(LateFee) ? 0 : decimal.Parse(LateFee);
                            newRow["BuyAmount"] = string.IsNullOrEmpty(BuyAmount) ? 0 : decimal.Parse(BuyAmount);
                            newRow["DateOfReturn"] = DateTime.TryParse(DateOfReturn, out DateTime dateOfReturnDateTime) ? (object)dateOfReturnDateTime : (object)new DateTime(1753, 1, 1);
                            newRow["SaleDate"] = DateTime.TryParse(SaleDate, out DateTime saleDateTime) ? (object)saleDateTime : (object)new DateTime(1753, 1, 1);
                            newRow["SaleAmount"] = string.IsNullOrEmpty(SaleAmount) ? 0 : decimal.Parse(SaleAmount);
                            newRow["NIP"] = null;

                            invoiceData.Rows.Add(newRow);

                            SaveInvoiceToDatabase(invoiceData);
                        }

                        processedCount++;

                        int progressPercentage = (int)((processedCount / (double)totalCount) * 100);

                        OnProgressChanged(progressPercentage);
                        OnProgressTotalChanged(processedCount, totalCount);
                    }
                    catch (Exception ex)
                    {
                        string errorMessage = $"Error processing record: {ex.Message}";
                        File.AppendAllText(logFilePath, errorMessage + Environment.NewLine);
                    }
                }

                generatedDatabaseConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas importu bazy danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseConnection();
            }
        }


        protected virtual void OnProgressChanged(int progressPercentage)
        {
            double progress = Math.Round(progressPercentage / 100.0, 2);
            ProgressChanged?.Invoke(this, (int)(progress * 100));
        }


        protected virtual void OnProgressTotalChanged(int current, int total)
        {
            int[] progress = { current, total };
            ProgressTotal?.Invoke(this, progress);
        }

        public DataTable GetLatestRecords(string tableName, string dateColumn)
        {
            string query = $"SELECT UKS.ID, " +
                           "UKS.InvoiceDate AS 'Data Wystawienia', " +
                           "UKS.DocumentType AS 'Typ Umowy', " +
                           "UKS.City AS 'Miasto Wystawienia', " +
                           "UKS.Description AS 'Opis', " +
                           "UKS.TotalAmount AS 'Wartość', " +
                           "UKS.Notes AS 'Notatki', " +
                           "Users.FullName AS 'Imię Nazwisko', " +
                           "Users.Address AS 'Ulica Numer', " +
                           "Users.PostalCode AS 'Kod Pocztowy', " +
                           "Users.City AS 'Miasto', " +
                           "Users.Phone AS 'Telefon', " +
                           "Users.Pesel AS 'Pesel', " +
                           "Users.NIP AS 'NIP', " +
                           "Users.Name AS 'Nazwa Firmy' " +
                           $"FROM {tableName} " +
                           "INNER JOIN Users ON UKS.UserID = Users.ID " +
                           $"ORDER BY {dateColumn} DESC LIMIT 25;"; // Ograniczenie do 25 rekordów

            DataTable dataTable = new DataTable();

            try
            {
                OpenConnection();

                if (useMySQL) // Jeśli używasz MySQL
                {
                    MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dataTable);
                }
                else // Jeśli używasz SQLite
                {
                    SQLiteCommand cmd = new SQLiteCommand(query, sqliteConnection);
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                    adapter.Fill(dataTable);
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas pobierania rekordów: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        public DataTable GetFilteredRecords(string tableName, Dictionary<string, string> filters, string searchValue = "", string[] searchableColumns = null, int count = 25)
        {
            string query = $"SELECT UKS.ID, " +
                           "UKS.InvoiceDate AS 'Data Wystawienia', " +
                           "UKS.DocumentType AS 'Typ Umowy', " +
                           "UKS.City AS 'Miasto Wystawienia', " +
                           "UKS.Description AS 'Opis', " +
                           "UKS.TotalAmount AS 'Wartość', " +
                           "UKS.Notes AS 'Notatki', " +
                           "Users.FullName AS 'Imię Nazwisko', " +
                           "Users.Address AS 'Ulica Numer', " +
                           "Users.PostalCode AS 'Kod Pocztowy', " +
                           "Users.City AS 'Miasto', " +
                           "UKS.DateOfReturn AS 'Data Zwrotu', " +
                           "UKS.SaleDate AS 'Data Sprzedaży', " +
                           "UKS.SaleAmount AS 'Wartość Sprzedaży', " +
                           "Users.Phone AS 'Telefon', " +
                           "Users.Pesel AS 'Pesel', " +
                           "Users.NIP AS 'NIP', " +
                           "Users.Name AS 'Nazwa Firmy' " +
                           $"FROM {tableName} " +
                           "INNER JOIN Users ON UKS.UserID = Users.ID WHERE 1=1";

            // Dodawanie filtrów do zapytania
            foreach (var filter in filters)
            {
                query += $" AND {tableName}.{filter.Key} = @{filter.Key}";
            }

            // Dodawanie warunków wyszukiwania
            if (!string.IsNullOrEmpty(searchValue) && searchableColumns != null && searchableColumns.Length > 0)
            {
                string[] searchWords = searchValue.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                query += " AND (";

                var searchConditions = new List<string>();
                foreach (var word in searchWords)
                {
                    var wordConditions = searchableColumns.Select(col => $"{col} LIKE @searchValue_{word}");
                    searchConditions.Add($"({string.Join(" OR ", wordConditions)})");
                }

                query += string.Join(" AND ", searchConditions);
                query += ")";
            }

            // Sortowanie i ograniczenie liczby wyników
            query += $" ORDER BY UKS.InvoiceDate DESC LIMIT {count}";

            DataTable dataTable = new DataTable();
            try
            {
                OpenConnection();

                if (useMySQL) // Jeśli używasz MySQL
                {
                    MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                    foreach (var filter in filters)
                    {
                        cmd.Parameters.AddWithValue($"@{filter.Key}", filter.Value);
                    }

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        string[] searchWords = searchValue.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var word in searchWords)
                        {
                            cmd.Parameters.AddWithValue($"@searchValue_{word}", "%" + word + "%");
                        }
                    }

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dataTable);
                }
                else // Jeśli używasz SQLite
                {
                    SQLiteCommand cmd = new SQLiteCommand(query, sqliteConnection);
                    foreach (var filter in filters)
                    {
                        cmd.Parameters.AddWithValue($"@{filter.Key}", filter.Value);
                    }

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        string[] searchWords = searchValue.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var word in searchWords)
                        {
                            cmd.Parameters.AddWithValue($"@searchValue_{word}", "%" + word + "%");
                        }
                    }

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                    adapter.Fill(dataTable);
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas filtrowania danych: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }


        public DataTable GetAlphabeticalRecords(string tableName, string sortColumn)
        {
            string query = $"SELECT * FROM {tableName} ORDER BY {sortColumn} ASC LIMIT 25;";

            DataTable dataTable = new DataTable();
            try
            {
                OpenConnection();

                if (useMySQL) // Jeśli używasz MySQL
                {
                    MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dataTable);
                }
                else // Jeśli używasz SQLite
                {
                    SQLiteCommand cmd = new SQLiteCommand(query, sqliteConnection);
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                    adapter.Fill(dataTable);
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas pobierania rekordów: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }

        public DataTable GetFilteredUsers(string searchValue = "", string[] searchableColumns = null, int count = 25)
        {
            string query = "SELECT " +
                           "ID, " +
                           "FullName AS 'Imię Nazwisko', " +
                           "Address AS 'Ulica Numer', " +
                           "PostalCode AS 'Kod Pocztowy', " +
                           "City AS 'Miasto', " +
                           "Phone AS 'Telefon', " +
                           "Email AS 'E-Mail', " +
                           "DocumentType AS 'Typ Dok.', " +
                           "DocumentNumber AS 'Numer Dok.', " +
                           "NIP AS 'NIP', " +
                           "Name AS 'Nazwa Firmy', " +
                           "Pesel AS 'Pesel', " +
                           "Notes AS 'Uwagi' " +
                           "FROM Users WHERE 1=1";

            if (!string.IsNullOrEmpty(searchValue) && searchableColumns != null && searchableColumns.Length > 0)
            {
                string[] searchWords = searchValue.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                query += " AND (";

                var searchConditions = new List<string>();
                foreach (var word in searchWords)
                {
                    var wordConditions = searchableColumns.Select(col => $"{col} LIKE @searchValue_{word}");
                    searchConditions.Add($"({string.Join(" OR ", wordConditions)})");
                }

                query += string.Join(" AND ", searchConditions);
                query += ")";
            }

            query += $" ORDER BY FullName ASC LIMIT {count}";

            DataTable dt = new DataTable();

            try
            {
                OpenConnection();

                if (useMySQL) // MySQL obsługa
                {
                    MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        string[] searchWords = searchValue.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var word in searchWords)
                        {
                            cmd.Parameters.AddWithValue($"@searchValue_{word}", "%" + word + "%");
                        }
                    }

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                else // SQLite obsługa
                {
                    SQLiteCommand cmd = new SQLiteCommand(query, sqliteConnection);

                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        string[] searchWords = searchValue.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var word in searchWords)
                        {
                            cmd.Parameters.AddWithValue($"@searchValue_{word}", "%" + word + "%");
                        }
                    }

                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                    adapter.Fill(dt);
                }

                return dt;
            }
            catch (Exception ex)
            {
                logger.LogError($"GetFilteredUsers() - Błąd podczas odczytu danych: {ex.Message}");
                return null;
            }
            finally
            {
                CloseConnection();
            }
        }


    }
}