namespace SKS_Service_Manager
{
    public partial class Issue : Form
    {

        private Form1 mainForm;

        public Issue(int Id, Form1 mainForm)
        {
            InitializeComponent();
        }

        private void Load_Click(object sender, EventArgs e)
        {

        }

        private void Value_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Pozwól na tylko cyfry, kropkę, Backspace oraz Control (do kopiowania i wklejania)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // Upewnij się, że jest tylko jedna kropka w polu tekstowym
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
    }
}
