namespace SKS_Service_Manager
{
    partial class EditUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditUser));
            label11 = new Label();
            label12 = new Label();
            Pesel = new TextBox();
            label13 = new Label();
            DocumentNumber = new TextBox();
            Save = new Button();
            Abort = new Button();
            label8 = new Label();
            EMail = new TextBox();
            City = new TextBox();
            label7 = new Label();
            Phone = new TextBox();
            label5 = new Label();
            Post_Code = new TextBox();
            label2 = new Label();
            Adress = new TextBox();
            label1 = new Label();
            FullName = new TextBox();
            DocumentType = new ComboBox();
            label3 = new Label();
            Notes = new RichTextBox();
            label4 = new Label();
            Nip = new TextBox();
            SuspendLayout();
            // 
            // label11
            // 
            label11.BackColor = Color.Transparent;
            label11.Font = new Font("Segoe UI", 12F);
            label11.Location = new Point(443, 167);
            label11.Name = "label11";
            label11.Size = new Size(150, 23);
            label11.TabIndex = 59;
            label11.Text = "Uwagi";
            label11.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            label12.BackColor = Color.Transparent;
            label12.Font = new Font("Segoe UI", 12F);
            label12.Location = new Point(443, 138);
            label12.Name = "label12";
            label12.Size = new Size(150, 23);
            label12.TabIndex = 57;
            label12.Text = "Pesel";
            label12.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Pesel
            // 
            Pesel.Location = new Point(599, 138);
            Pesel.Name = "Pesel";
            Pesel.Size = new Size(200, 23);
            Pesel.TabIndex = 10;
            // 
            // label13
            // 
            label13.BackColor = Color.Transparent;
            label13.Font = new Font("Segoe UI", 12F);
            label13.Location = new Point(443, 109);
            label13.Name = "label13";
            label13.Size = new Size(150, 23);
            label13.TabIndex = 55;
            label13.Text = "Numer Dokumentu";
            label13.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // DocumentNumber
            // 
            DocumentNumber.Location = new Point(599, 109);
            DocumentNumber.Name = "DocumentNumber";
            DocumentNumber.Size = new Size(200, 23);
            DocumentNumber.TabIndex = 9;
            // 
            // Save
            // 
            Save.BackColor = Color.Transparent;
            Save.BackgroundImage = Properties.Resources.save;
            Save.BackgroundImageLayout = ImageLayout.Zoom;
            Save.Cursor = Cursors.Hand;
            Save.FlatAppearance.BorderSize = 0;
            Save.FlatStyle = FlatStyle.Flat;
            Save.Location = new Point(749, 243);
            Save.Name = "Save";
            Save.Size = new Size(50, 50);
            Save.TabIndex = 12;
            Save.UseVisualStyleBackColor = false;
            Save.Click += Save_Click;
            // 
            // Abort
            // 
            Abort.BackColor = Color.Transparent;
            Abort.BackgroundImage = Properties.Resources.abort;
            Abort.BackgroundImageLayout = ImageLayout.Zoom;
            Abort.Cursor = Cursors.Hand;
            Abort.FlatAppearance.BorderSize = 0;
            Abort.FlatStyle = FlatStyle.Flat;
            Abort.Location = new Point(670, 243);
            Abort.Name = "Abort";
            Abort.Size = new Size(50, 50);
            Abort.TabIndex = 48;
            Abort.UseVisualStyleBackColor = false;
            Abort.Click += Abort_Click;
            // 
            // label8
            // 
            label8.BackColor = Color.Transparent;
            label8.Font = new Font("Segoe UI", 12F);
            label8.Location = new Point(12, 228);
            label8.Name = "label8";
            label8.Size = new Size(150, 23);
            label8.TabIndex = 47;
            label8.Text = "Email";
            label8.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // EMail
            // 
            EMail.Location = new Point(194, 228);
            EMail.Name = "EMail";
            EMail.Size = new Size(200, 23);
            EMail.TabIndex = 7;
            // 
            // City
            // 
            City.Location = new Point(260, 167);
            City.Name = "City";
            City.Size = new Size(134, 23);
            City.TabIndex = 5;
            // 
            // label7
            // 
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Segoe UI", 12F);
            label7.Location = new Point(12, 199);
            label7.Name = "label7";
            label7.Size = new Size(150, 23);
            label7.TabIndex = 44;
            label7.Text = "Telefon";
            label7.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Phone
            // 
            Phone.Location = new Point(194, 199);
            Phone.Name = "Phone";
            Phone.Size = new Size(200, 23);
            Phone.TabIndex = 6;
            // 
            // label5
            // 
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 12F);
            label5.Location = new Point(12, 167);
            label5.Name = "label5";
            label5.Size = new Size(150, 23);
            label5.TabIndex = 40;
            label5.Text = "Kod Pocztowy";
            label5.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Post_Code
            // 
            Post_Code.Location = new Point(194, 167);
            Post_Code.Name = "Post_Code";
            Post_Code.PlaceholderText = "00-000";
            Post_Code.Size = new Size(60, 23);
            Post_Code.TabIndex = 4;
            Post_Code.TextAlign = HorizontalAlignment.Center;
            // 
            // label2
            // 
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(12, 138);
            label2.Name = "label2";
            label2.Size = new Size(150, 23);
            label2.TabIndex = 34;
            label2.Text = "Ulica Numer";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Adress
            // 
            Adress.Location = new Point(194, 138);
            Adress.Name = "Adress";
            Adress.Size = new Size(200, 23);
            Adress.TabIndex = 3;
            Adress.Text = "ul. ";
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(12, 80);
            label1.Name = "label1";
            label1.Size = new Size(176, 23);
            label1.TabIndex = 32;
            label1.Text = "Nazwa / Imie Nazwisko";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // FullName
            // 
            FullName.Location = new Point(194, 80);
            FullName.Name = "FullName";
            FullName.Size = new Size(200, 23);
            FullName.TabIndex = 1;
            // 
            // DocumentType
            // 
            DocumentType.AutoCompleteCustomSource.AddRange(new string[] { "Dowód Osobisty", "Prawo Jazdy", "Paszport" });
            DocumentType.FormattingEnabled = true;
            DocumentType.Location = new Point(599, 80);
            DocumentType.Name = "DocumentType";
            DocumentType.Size = new Size(200, 23);
            DocumentType.TabIndex = 8;
            // 
            // label3
            // 
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 12F);
            label3.Location = new Point(443, 80);
            label3.Name = "label3";
            label3.Size = new Size(150, 23);
            label3.TabIndex = 63;
            label3.Text = "Rodzaj Dokumentu";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Notes
            // 
            Notes.Location = new Point(599, 167);
            Notes.Name = "Notes";
            Notes.Size = new Size(200, 55);
            Notes.TabIndex = 11;
            Notes.Text = "Brak Uwag";
            // 
            // label4
            // 
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI", 12F);
            label4.Location = new Point(12, 109);
            label4.Name = "label4";
            label4.Size = new Size(150, 23);
            label4.TabIndex = 66;
            label4.Text = "NIP";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Nip
            // 
            Nip.Location = new Point(194, 109);
            Nip.Name = "Nip";
            Nip.Size = new Size(200, 23);
            Nip.TabIndex = 2;
            // 
            // EditUser
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.background;
            ClientSize = new Size(826, 315);
            Controls.Add(label4);
            Controls.Add(Nip);
            Controls.Add(Notes);
            Controls.Add(label3);
            Controls.Add(DocumentType);
            Controls.Add(label11);
            Controls.Add(label12);
            Controls.Add(Pesel);
            Controls.Add(label13);
            Controls.Add(DocumentNumber);
            Controls.Add(Save);
            Controls.Add(Abort);
            Controls.Add(label8);
            Controls.Add(EMail);
            Controls.Add(City);
            Controls.Add(label7);
            Controls.Add(Phone);
            Controls.Add(label5);
            Controls.Add(Post_Code);
            Controls.Add(label2);
            Controls.Add(Adress);
            Controls.Add(label1);
            Controls.Add(FullName);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "EditUser";
            Text = "Edycja Użytkownika";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label11;
        private Label label12;
        private TextBox Pesel;
        private Label label13;
        private TextBox DocumentNumber;
        private Button Save;
        private Button Abort;
        private Label label8;
        private TextBox EMail;
        private TextBox City;
        private Label label7;
        private TextBox Phone;
        private Label label5;
        private TextBox Post_Code;
        private Label label2;
        private TextBox Adress;
        private Label label1;
        private TextBox FullName;
        private ComboBox DocumentType;
        private Label label3;
        private RichTextBox Notes;
        private Label label4;
        private TextBox Nip;
    }
}