using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SKS_Service_Manager
{
    public partial class PrintRecords : Form
    {
        private Form1 Form1;
        private DataBase dataBase;
        private IssueUKS issueUKS;

        private string inputRaportFile;
        private string outputRaportFile;

        public PrintRecords(Form1 form1)
        {
            InitializeComponent();
            this.Form1 = form1;
            dataBase = Form1.getDataBase();
            issueUKS = new IssueUKS(-1, Form1);

            IssuedCity.Items.Insert(0, "Wszystko");
            IssuedCity.Items.AddRange(dataBase.GetUniqueCities().ToArray());
            IssuedCity.SelectedIndex = 0;
            DocumentType.SelectedIndex = 0;
        }

        private void print_Click(object sender, EventArgs e)
        {

            string issuedCity = IssuedCity.Text.ToString();
            string documentType = DocumentType.Text.ToString();

            DateTime invoiceDate = IssuedDate.Value.Date;
            DateTime pickupDate = PickupDate.Value.Date;

            if (pickupDate <= invoiceDate)
            {
                MessageBox.Show("Data odbioru musi być większa niż data faktury.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                PickupDate.Focus();
                return;
            }

            inputRaportFile = AppDomain.CurrentDomain.BaseDirectory + "\\umowy\\backup\\ewidencja_" + invoiceDate.ToShortDateString().ToString() + "-" + pickupDate.ToShortDateString().ToString() + ".docx";
            outputRaportFile = AppDomain.CurrentDomain.BaseDirectory + "\\umowy\\backup\\ewidencja_" + invoiceDate.ToShortDateString().ToString() + "-" + pickupDate.ToShortDateString().ToString() + ".pdf";
            DataTable dt = dataBase.uksLoadDataByDateRange(invoiceDate, pickupDate, issuedCity, documentType);

            dataGridView1.DataSource = dt;
            try
            {
                issueUKS.CreateDocxFromData(dt, inputRaportFile);
                issueUKS.ConvertDocxToPdf(inputRaportFile, outputRaportFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd tworzenia raportu: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                try
                {
                    Process.Start("cmd", $"/c start {outputRaportFile}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Błąd podczas otwierania pliku PDF: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
