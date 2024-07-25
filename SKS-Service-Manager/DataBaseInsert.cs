using System;
using System.Data;
using System.Data.Entity;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;

namespace SKS_Service_Manager
{
    public partial class DataBaseInsert : Form
    {
        private string selectedFilePath;
        private string firebirdConnectionString;
        private string sqliteConnectionString;
        private Form1 mainForm;
        private DataBase dataBase;

        public event EventHandler<int> ProgressChanged;

        public DataBaseInsert(Form1 form1)
        {
            InitializeComponent();
            CenterToScreen();
            this.mainForm = form1;
            dataBase = mainForm.getDataBase();
            dataBase.ProgressChanged += Database_ProgressChanged;
            dataBase.ProgressTotal += Database_ProgressTotal;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Pliki bazy danych Firebird (*.gdb; *.bak)|*.gdb; *.bak|Wszystkie pliki (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFilePath = openFileDialog.FileName;
                textBox1.Text = selectedFilePath;
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedFilePath))
            {
                firebirdConnectionString = $"Database={selectedFilePath};User=SYSDBA;Password=masterkey;DataSource=localhost;Port=3050;";

                string folderPath = Path.GetDirectoryName(selectedFilePath);

                string dataBaseFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "umowy");
                string dataBaseFile = Path.Combine(dataBaseFolder, "data_insert.db");
                sqliteConnectionString = $"Data Source={dataBaseFile}";

                label1.Visible = true;
                await ExportFirebirdToSQLiteAsync();

                UpdateLabel($"Importowanie bazy danych: 0%");
                await Task.Run(() => dataBase.AddUsersFromGeneratedDatabaseAsync(dataBaseFile));

                MessageBox.Show("Zakończono Import Bazy danych.");
                dataBase.RefreshConnection();
            }
            else
            {
                MessageBox.Show("Najpierw wybierz plik bazy danych Firebird .gdb.");
            }
        }

        private async Task ExportFirebirdToSQLiteAsync()
        {
            await Task.Run(() =>
            {
                using (FbConnection firebirdConnection = new FbConnection(firebirdConnectionString))
                using (SQLiteConnection sqliteConnection = new SQLiteConnection(sqliteConnectionString))
                {
                    firebirdConnection.Open();
                    sqliteConnection.Open();

                    DataTable schema = firebirdConnection.GetSchema("Tables");

                    foreach (DataRow row in schema.Rows)
                    {
                        string tableName = row["TABLE_NAME"].ToString();

                        if (tableName.ToLower() != "lombard")
                        {
                            continue;
                        }

                        // Usuń istniejącą tabelę "lombard" z bazy danych SQLite, jeśli istnieje
                        using (SQLiteCommand dropTableCommand = new SQLiteCommand("DROP TABLE IF EXISTS lombard", sqliteConnection))
                        {
                            dropTableCommand.ExecuteNonQuery();
                        }

                        // Pobierz listę kolumn dla danej tabeli
                        DataTable columnsSchema = firebirdConnection.GetSchema("Columns", new string[] { null, null, tableName });

                        // Utwórz zapytanie CREATE TABLE dynamicznie na podstawie dostępnych kolumn
                        string createTableQuery = $"CREATE TABLE lombard (";
                        foreach (DataRow columnRow in columnsSchema.Rows)
                        {
                            string columnName = columnRow["COLUMN_NAME"].ToString();
                            createTableQuery += $"{columnName} TEXT, "; // Zakładałem typ danych jako TEXT, dostosuj do rzeczywistych typów danych
                        }
                        // Usuń ostatnią przecinkę
                        createTableQuery = createTableQuery.TrimEnd(',', ' ') + ")";

                        // Wykonaj zapytanie CREATE TABLE
                        using (SQLiteCommand createTableCommand = new SQLiteCommand(createTableQuery, sqliteConnection))
                        {
                            createTableCommand.ExecuteNonQuery();
                        }

                        // Utwórz zapytanie INSERT dynamicznie na podstawie dostępnych kolumn
                        string insertQuery = $"INSERT INTO lombard (";
                        foreach (DataRow columnRow in columnsSchema.Rows)
                        {
                            string columnName = columnRow["COLUMN_NAME"].ToString();
                            insertQuery += columnName + ", ";
                        }
                        // Usuń ostatnią przecinkę
                        insertQuery = insertQuery.TrimEnd(',', ' ') + ") VALUES (";

                        // Dodaj parametry do zapytania
                        for (int i = 0; i < columnsSchema.Rows.Count; i++)
                        {
                            insertQuery += $"@param{i}, ";
                        }
                        // Usuń ostatnią przecinkę
                        insertQuery = insertQuery.TrimEnd(',', ' ') + ")";

                        using (FbCommand firebirdCommand2 = new FbCommand($"SELECT COUNT(*) FROM {tableName}", firebirdConnection))
                        {
                            int recordCount = Convert.ToInt32(firebirdCommand2.ExecuteScalar());
                            int tablesProcessed = 0;

                            using (FbCommand firebirdCommand = new FbCommand($"SELECT * FROM {tableName}", firebirdConnection))
                            using (SQLiteCommand sqliteCommand = new SQLiteCommand(insertQuery, sqliteConnection))
                            {
                                using (FbDataReader reader = firebirdCommand.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        // Przypisanie parametrów do rzeczywistych wartości z wyników zapytania Firebird
                                        for (int i = 0; i < columnsSchema.Rows.Count; i++)
                                        {
                                            string columnName = columnsSchema.Rows[i]["COLUMN_NAME"].ToString();
                                            object columnValue = reader[columnName];
                                            sqliteCommand.Parameters.AddWithValue($"@param{i}", columnValue);
                                        }

                                        // Wykonaj zapytanie INSERT
                                        sqliteCommand.ExecuteNonQuery();

                                        sqliteCommand.ExecuteNonQuery();
                                        tablesProcessed++;

                                        int progressPercentage = (int)((tablesProcessed / (double)recordCount) * 100);
                                        UpdateLabel("Konwertowanie: " + progressPercentage + "%");
                                        UpdateProgressBar(progressPercentage);
                                    }
                                }
                            }
                        }

                        UpdateLabel($"Konwertowanie: 100%");
                        UpdateProgressBar(100);
                    }
                }
            });
        }

        private void Database_ProgressChanged(object sender, int progressPercentage)
        {
            // Aktualizacja paska postępu
            UpdateProgressBar(progressPercentage);
        }

        private void Database_ProgressTotal(object sender, int[] total)
        {
            // Aktualizacja etykiety
            UpdateLabel($"Importowanie bazy danych: {total[0]} / {total[1]}");
        }

        private void UpdateProgressBar(int value)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action<int>(UpdateProgressBar), value);
            }
            else
            {
                progressBar.Value = value;
            }
        }

        private void UpdateLabel(string value)
        {
            if (label1.InvokeRequired)
            {
                label1.Invoke(new Action<string>(UpdateLabel), value);
            }
            else
            {
                label1.Text = value;
            }
        }
    }
}
