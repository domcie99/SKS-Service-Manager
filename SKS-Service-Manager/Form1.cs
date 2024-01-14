using System;
using System.IO;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace SKS_Service_Manager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // �cie�ka do pliku szablonu w folderze wyj�ciowym aplikacji
            string reportPath = Path.Combine(Application.StartupPath, "Umowa-Kupna-Sprzedazy.docx");
            string outputPath = Path.Combine(Application.StartupPath, "Zakladki.txt");

            try
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(reportPath, false))
                {
                    using (StreamWriter writer = new StreamWriter(outputPath))
                    {
                        var bookmarks = wordDoc.MainDocumentPart.RootElement.Descendants<BookmarkStart>();
                        foreach (var bookmark in bookmarks)
                        {
                            writer.WriteLine("Zak�adka: " + bookmark.Name); // Zapisz do pliku
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wyst�pi� b��d: " + ex.Message);
            }
        }
    }
}
