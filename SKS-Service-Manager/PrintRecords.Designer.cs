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
            label14 = new Label();
            FromDate = new DateTimePicker();
            label15 = new Label();
            label1 = new Label();
            label2 = new Label();
            IssuedCity = new ComboBox();
            mySqlCommand1 = new MySqlConnector.MySqlCommand();
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
            print.Location = new Point(371, 106);
            print.Name = "print";
            print.Size = new Size(50, 50);
            print.TabIndex = 119;
            print.UseVisualStyleBackColor = false;
            print.Click += print_Click;
            // 
            // DocumentType
            // 
            DocumentType.AutoCompleteCustomSource.AddRange(new string[] { "Dowód Osobisty", "Prawo Jazdy", "Paszport" });
            DocumentType.DropDownStyle = ComboBoxStyle.DropDownList;
            DocumentType.FormattingEnabled = true;
            DocumentType.Items.AddRange(new object[] { "Wszystko", "Umowa Komisowa", "Umowa Kupna-Sprzedaży", "Umowa Pożyczki z Przechowaniem" });
            DocumentType.Location = new Point(201, 75);
            DocumentType.Name = "DocumentType";
            DocumentType.Size = new Size(220, 23);
            DocumentType.TabIndex = 120;
            // 
            // ToDate
            // 
            ToDate.CustomFormat = "dd-MM-yyyy";
            ToDate.Format = DateTimePickerFormat.Short;
            ToDate.Location = new Point(201, 133);
            ToDate.Name = "ToDate";
            ToDate.Size = new Size(100, 23);
            ToDate.TabIndex = 124;
            // 
            // label14
            // 
            label14.BackColor = Color.Transparent;
            label14.Font = new Font("Segoe UI", 12F);
            label14.Location = new Point(45, 133);
            label14.Name = "label14";
            label14.Size = new Size(150, 23);
            label14.TabIndex = 122;
            label14.Text = "Zakres do";
            label14.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // FromDate
            // 
            FromDate.CustomFormat = "dd-MM-yyyy";
            FromDate.Format = DateTimePickerFormat.Short;
            FromDate.Location = new Point(201, 104);
            FromDate.Name = "FromDate";
            FromDate.Size = new Size(100, 23);
            FromDate.TabIndex = 123;
            FromDate.Value = new DateTime(2024, 1, 23, 17, 7, 29, 0);
            // 
            // label15
            // 
            label15.BackColor = Color.Transparent;
            label15.Font = new Font("Segoe UI", 12F);
            label15.Location = new Point(45, 104);
            label15.Name = "label15";
            label15.Size = new Size(150, 23);
            label15.TabIndex = 121;
            label15.Text = "Zakres Od";
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
            // PrintRecords
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.background;
            ClientSize = new Size(486, 193);
            Controls.Add(IssuedCity);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(ToDate);
            Controls.Add(label14);
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
    }
}