using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;


namespace SKS_Service_Manager
{
    public partial class UppzList : Form
    {
        public UppzList()
        {
            InitializeComponent();
        }

        private void Add_Click(object sender, EventArgs e)
        {

        }

        static async System.Threading.Tasks.Task GetCompanyData()
        {
            // Adres URL strony do scrapingu
            string url = "https://wyszukiwarkaregon.stat.gov.pl/appBIR/index.aspx";

            try
            {
                // Inicjalizacja HttpClient do pobierania strony
                HttpClient httpClient = new HttpClient();
                string html = await httpClient.GetStringAsync(url);

                // Inicjalizacja HtmlDocument z wykorzystaniem HtmlAgilityPack
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                // Znajdź elementy na stronie przy użyciu XPath lub innych metod HtmlAgilityPack
                var tableRows = htmlDocument.DocumentNode.SelectNodes("//table[@class='tabelaZbiorczaListaJednostek']//tr");

                if (tableRows != null)
                {
                    foreach (var row in tableRows)
                    {
                        var cells = row.SelectNodes("td");
                        if (cells != null)
                        {
                            foreach (var cell in cells)
                            {
                                Console.Write(cell.InnerText.Trim() + "\t");
                            }
                            Console.WriteLine();
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Nie znaleziono tabeli na stronie.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Wystąpił błąd: " + ex.Message);
            }
        }
    }
}
