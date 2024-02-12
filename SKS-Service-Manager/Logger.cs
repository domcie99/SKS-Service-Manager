using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKS_Service_Manager
{
    public class Logger
    {
        private string logFilePath;

        public Logger(string logFileName)
        {
            string directory = AppDomain.CurrentDomain.BaseDirectory;
            logFilePath = Path.Combine(directory, logFileName);
        }

        public void LogError(string errorMessage)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    string logMessage = $"{DateTime.Now} [Error]: {errorMessage}";
                    writer.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
                // Jeśli wystąpi błąd podczas zapisywania do pliku log, wyświetl komunikat na konsoli lub innym miejscu
                MessageBox.Show($"Błąd podczas zapisywania do pliku dziennika: {ex.Message}");
            }
        }
    }
}
