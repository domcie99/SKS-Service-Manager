namespace SKS_Service_Manager
{
    partial class PrintRecords
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintRecords));
            print = new Button();
            DocumentType = new ComboBox();
            ToDate = new DateTimePicker();
            FromDate = new DateTimePicker();
            label15 = new Label();
            label1 = new Label();
            label2 = new Label();
            IssuedCity = new ComboBox();
            mySqlCommand1 = new MySqlConnector.MySqlCommand();
            MonthsComboBox = new ComboBox();
            label3 = new Label();
            label4 = new Label();
            minusButton = new Button();
            currentMonthButton = new Button();
            plusButton = new Button();
            SuspendLayout();
            // 
            // print
            // 
            print.BackColor = Color.Transparent;
            print.BackgroundImage = Properties.Resources.printer;
            print.BackgroundImageLayout = ImageLayout.Zoom;
            print.Cursor = Cursors.Hand;
            print.FlatAppearance.BorderSize = 0;
            print.FlatStyle = FlatStyle.Flat;
            print.Location = new Point(371, 211);
            print.Name = "print";
            print.Size = new Size(50, 50);
            print.TabIndex = 119;
            print.UseVisualStyleBackColor = false;
            print.Click += print_Click;
            // 
            // DocumentType
            // 
            DocumentType.AutoCompleteCustomSource.AddRange(new string[] { "Dowód Osobisty", "Prawo Jazdy", "Paszport" });
            DocumentType.Cursor = Cursors.Hand;
            DocumentType.DropDownStyle = ComboBoxStyle.DropDownList;
            DocumentType.FormattingEnabled = true;
            DocumentType.Items.AddRange(new object[] { "Wszystko", "Umowa Kupna-Sprzedaży", "Umowa Komisowa", "Umowa Konsumenckiej Pożyczki Lombardowej", "Umowa Pożyczki z Przechowaniem" });
            DocumentType.Location = new Point(201, 75);
            DocumentType.Name = "DocumentType";
            DocumentType.Size = new Size(220, 23);
            DocumentType.TabIndex = 120;
            // 
            // ToDate
            // 
            ToDate.Cursor = Cursors.Hand;
            ToDate.CustomFormat = "dd-MM-yyyy";
            ToDate.Format = DateTimePickerFormat.Short;
            ToDate.Location = new Point(225, 166);
            ToDate.Name = "ToDate";
            ToDate.Size = new Size(100, 23);
            ToDate.TabIndex = 124;
            // 
            // FromDate
            // 
            FromDate.Cursor = Cursors.Hand;
            FromDate.CustomFormat = "dd-MM-yyyy";
            FromDate.Format = DateTimePickerFormat.Short;
            FromDate.Location = new Point(119, 166);
            FromDate.Name = "FromDate";
            FromDate.Size = new Size(100, 23);
            FromDate.TabIndex = 123;
            FromDate.Value = new DateTime(2024, 1, 23, 17, 7, 29, 0);
            FromDate.ValueChanged += FromDate_ValueChanged;
            // 
            // label15
            // 
            label15.BackColor = Color.Transparent;
            label15.Font = new Font("Segoe UI", 12F);
            label15.Location = new Point(45, 166);
            label15.Name = "label15";
            label15.Size = new Size(68, 23);
            label15.TabIndex = 121;
            label15.Text = "Od/Do";
            label15.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(45, 46);
            label1.Name = "label1";
            label1.Size = new Size(150, 23);
            label1.TabIndex = 125;
            label1.Text = "Miasto Wystawienia";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(45, 72);
            label2.Name = "label2";
            label2.Size = new Size(150, 23);
            label2.TabIndex = 126;
            label2.Text = "Rodzaj Umowy";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // IssuedCity
            // 
            IssuedCity.AutoCompleteCustomSource.AddRange(new string[] { "Dowód Osobisty", "Prawo Jazdy", "Paszport" });
            IssuedCity.Cursor = Cursors.Hand;
            IssuedCity.DropDownStyle = ComboBoxStyle.DropDownList;
            IssuedCity.FormattingEnabled = true;
            IssuedCity.Location = new Point(201, 46);
            IssuedCity.Name = "IssuedCity";
            IssuedCity.Size = new Size(220, 23);
            IssuedCity.TabIndex = 127;
            // 
            // mySqlCommand1
            // 
            mySqlCommand1.CommandTimeout = 0;
            mySqlCommand1.Connection = null;
            mySqlCommand1.Transaction = null;
            mySqlCommand1.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // MonthsComboBox
            // 
            MonthsComboBox.Cursor = Cursors.Hand;
            MonthsComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MonthsComboBox.FormattingEnabled = true;
            MonthsComboBox.Items.AddRange(new object[] { "Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień" });
            MonthsComboBox.Location = new Point(201, 117);
            MonthsComboBox.Name = "MonthsComboBox";
            MonthsComboBox.Size = new Size(121, 23);
            MonthsComboBox.TabIndex = 128;
            MonthsComboBox.SelectedIndexChanged += MonthsComboBox_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 12F);
            label3.Location = new Point(45, 114);
            label3.Name = "label3";
            label3.Size = new Size(150, 23);
            label3.TabIndex = 129;
            label3.Text = "Wybierz miesiąc:";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI", 12F);
            label4.Location = new Point(45, 143);
            label4.Name = "label4";
            label4.Size = new Size(240, 23);
            label4.TabIndex = 130;
            label4.Text = "Lub wybierz ręcznie zakres dat:";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // minusButton
            // 
            minusButton.Cursor = Cursors.Hand;
            minusButton.Font = new Font("Arial Black", 9F, FontStyle.Bold);
            minusButton.Location = new Point(328, 116);
            minusButton.Name = "minusButton";
            minusButton.Size = new Size(29, 23);
            minusButton.TabIndex = 131;
            minusButton.Text = "-";
            minusButton.UseVisualStyleBackColor = true;
            minusButton.Click += minusButton_Click;
            // 
            // currentMonthButton
            // 
            currentMonthButton.Cursor = Cursors.Hand;
            currentMonthButton.Font = new Font("Arial Black", 9F, FontStyle.Bold);
            currentMonthButton.Location = new Point(363, 116);
            currentMonthButton.Name = "currentMonthButton";
            currentMonthButton.Size = new Size(29, 23);
            currentMonthButton.TabIndex = 132;
            currentMonthButton.Text = "0";
            currentMonthButton.UseVisualStyleBackColor = true;
            currentMonthButton.Click += currentMonthButton_Click;
            // 
            // plusButton
            // 
            plusButton.Cursor = Cursors.Hand;
            plusButton.Font = new Font("Arial Black", 9F, FontStyle.Bold);
            plusButton.Location = new Point(398, 116);
            plusButton.Name = "plusButton";
            plusButton.Size = new Size(29, 23);
            plusButton.TabIndex = 133;
            plusButton.Text = "+";
            plusButton.UseVisualStyleBackColor = true;
            plusButton.Click += plusButton_Click;
            // 
            // PrintRecords
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.background;
            ClientSize = new Size(443, 277);
            Controls.Add(plusButton);
            Controls.Add(currentMonthButton);
            Controls.Add(minusButton);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(MonthsComboBox);
            Controls.Add(IssuedCity);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(ToDate);
            Controls.Add(FromDate);
            Controls.Add(label15);
            Controls.Add(DocumentType);
            Controls.Add(print);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "PrintRecords";
            Text = "Ewidencja Kupna Sprzedaży";
            ResumeLayout(false);
        }

        #endregion

        private Button print;
        private ComboBox DocumentType;
        private DateTimePicker ToDate;
        private Label label14;
        private DateTimePicker FromDate;
        private Label label15;
        private Label label1;
        private Label label2;
        private ComboBox IssuedCity;
        private MySqlConnector.MySqlCommand mySqlCommand1;
        private ComboBox MonthsComboBox;
        private Label label3;
        private Label label4;
        private Button minusButton;
        private Button currentMonthButton;
        private Button plusButton;
    }
}