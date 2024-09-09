public class OverlayForm : Form
{
    public OverlayForm()
    {
        // Ustawienia dla półprzezroczystej nakładki
        this.FormBorderStyle = FormBorderStyle.None;
        this.StartPosition = FormStartPosition.Manual;
        this.BackColor = Color.Black;
        this.Opacity = 0.5; // 50% przezroczystości
        this.ShowInTaskbar = false; // Nie pokazuj okna na pasku zadań
        this.TopMost = true; // Ustaw jako okno na wierzchu
    }

    public void ShowOverlay(Form parentForm)
    {
        // Dopasuj nakładkę do widocznego obszaru formularza (ClientSize)
        this.Size = parentForm.ClientSize;

        // Ustaw lokalizację nakładki na ekranie, dopasowaną do obszaru klienta okna głównego
        this.Location = parentForm.PointToScreen(Point.Empty);

        // Ustawienie okna jako właściciela
        this.Owner = parentForm;

        // Pokaż nakładkę
        this.Show(parentForm);
    }
}
