namespace SKS_Service_Manager
{
    partial class Issue
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
            groupBox1 = new GroupBox();
            label6 = new Label();
            Load = new Button();
            label4 = new Label();
            Nip = new TextBox();
            Notes = new RichTextBox();
            label3 = new Label();
            DocumentType = new ComboBox();
            label11 = new Label();
            label12 = new Label();
            Pesel = new TextBox();
            label13 = new Label();
            DocumentNumber = new TextBox();
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
            Name = new TextBox();
            groupBox2 = new GroupBox();
            Value = new TextBox();
            label10 = new Label();
            Description = new RichTextBox();
            label9 = new Label();
            groupBox3 = new GroupBox();
            Issue_Date = new DateTimePicker();
            label15 = new Label();
            Pickup_Date = new DateTimePicker();
            label14 = new Label();
            label16 = new Label();
            Days = new TextBox();
            textBox1 = new TextBox();
            label17 = new Label();
            label18 = new Label();
            label19 = new Label();
            Comments = new RichTextBox();
            label20 = new Label();
            button1 = new Button();
            button2 = new Button();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.Transparent;
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(Load);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(Nip);
            groupBox1.Controls.Add(Notes);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(DocumentType);
            groupBox1.Controls.Add(label11);
            groupBox1.Controls.Add(label12);
            groupBox1.Controls.Add(Pesel);
            groupBox1.Controls.Add(label13);
            groupBox1.Controls.Add(DocumentNumber);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(EMail);
            groupBox1.Controls.Add(City);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(Phone);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(Post_Code);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(Adress);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(Name);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(795, 225);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Osoba Powierzająca";
            // 
            // label6
            // 
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Segoe UI", 12F);
            label6.Location = new Point(615, 179);
            label6.Name = "label6";
            label6.Size = new Size(116, 23);
            label6.TabIndex = 89;
            label6.Text = "Wybierz z bazy";
            label6.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Load
            // 
            Load.BackColor = Color.Transparent;
            Load.BackgroundImage = Properties.Resources.add;
            Load.BackgroundImageLayout = ImageLayout.Zoom;
            Load.Cursor = Cursors.Hand;
            Load.FlatAppearance.BorderSize = 0;
            Load.FlatStyle = FlatStyle.Flat;
            Load.Location = new Point(737, 167);
            Load.Name = "Load";
            Load.Size = new Size(50, 50);
            Load.TabIndex = 88;
            Load.UseVisualStyleBackColor = false;
            Load.Click += Load_Click;
            // 
            // label4
            // 
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI", 12F);
            label4.Location = new Point(0, 48);
            label4.Name = "label4";
            label4.Size = new Size(150, 23);
            label4.TabIndex = 87;
            label4.Text = "NIP";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Nip
            // 
            Nip.Location = new Point(182, 48);
            Nip.Name = "Nip";
            Nip.Size = new Size(200, 23);
            Nip.TabIndex = 68;
            // 
            // Notes
            // 
            Notes.Location = new Point(587, 106);
            Notes.Name = "Notes";
            Notes.Size = new Size(200, 55);
            Notes.TabIndex = 77;
            Notes.Text = "";
            // 
            // label3
            // 
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 12F);
            label3.Location = new Point(431, 19);
            label3.Name = "label3";
            label3.Size = new Size(150, 23);
            label3.TabIndex = 86;
            label3.Text = "Rodzaj Dokumentu";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // DocumentType
            // 
            DocumentType.AutoCompleteCustomSource.AddRange(new string[] { "Dowód Osobisty", "Prawo Jazdy", "Paszport" });
            DocumentType.FormattingEnabled = true;
            DocumentType.Location = new Point(587, 19);
            DocumentType.Name = "DocumentType";
            DocumentType.Size = new Size(200, 23);
            DocumentType.TabIndex = 74;
            // 
            // label11
            // 
            label11.BackColor = Color.Transparent;
            label11.Font = new Font("Segoe UI", 12F);
            label11.Location = new Point(431, 106);
            label11.Name = "label11";
            label11.Size = new Size(150, 23);
            label11.TabIndex = 85;
            label11.Text = "Notatki";
            label11.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            label12.BackColor = Color.Transparent;
            label12.Font = new Font("Segoe UI", 12F);
            label12.Location = new Point(431, 77);
            label12.Name = "label12";
            label12.Size = new Size(150, 23);
            label12.TabIndex = 84;
            label12.Text = "Pesel";
            label12.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Pesel
            // 
            Pesel.Location = new Point(587, 77);
            Pesel.Name = "Pesel";
            Pesel.Size = new Size(200, 23);
            Pesel.TabIndex = 76;
            // 
            // label13
            // 
            label13.BackColor = Color.Transparent;
            label13.Font = new Font("Segoe UI", 12F);
            label13.Location = new Point(431, 48);
            label13.Name = "label13";
            label13.Size = new Size(150, 23);
            label13.TabIndex = 83;
            label13.Text = "Numer Dokumentu";
            label13.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // DocumentNumber
            // 
            DocumentNumber.Location = new Point(587, 48);
            DocumentNumber.Name = "DocumentNumber";
            DocumentNumber.Size = new Size(200, 23);
            DocumentNumber.TabIndex = 75;
            // 
            // label8
            // 
            label8.BackColor = Color.Transparent;
            label8.Font = new Font("Segoe UI", 12F);
            label8.Location = new Point(0, 167);
            label8.Name = "label8";
            label8.Size = new Size(150, 23);
            label8.TabIndex = 82;
            label8.Text = "Email";
            label8.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // EMail
            // 
            EMail.Location = new Point(182, 167);
            EMail.Name = "EMail";
            EMail.Size = new Size(200, 23);
            EMail.TabIndex = 73;
            // 
            // City
            // 
            City.Location = new Point(248, 106);
            City.Name = "City";
            City.Size = new Size(134, 23);
            City.TabIndex = 71;
            // 
            // label7
            // 
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Segoe UI", 12F);
            label7.Location = new Point(0, 138);
            label7.Name = "label7";
            label7.Size = new Size(150, 23);
            label7.TabIndex = 81;
            label7.Text = "Telefon";
            label7.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Phone
            // 
            Phone.Location = new Point(182, 138);
            Phone.Name = "Phone";
            Phone.Size = new Size(200, 23);
            Phone.TabIndex = 72;
            // 
            // label5
            // 
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 12F);
            label5.Location = new Point(0, 106);
            label5.Name = "label5";
            label5.Size = new Size(150, 23);
            label5.TabIndex = 80;
            label5.Text = "Kod Pocztowy";
            label5.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Post_Code
            // 
            Post_Code.Location = new Point(182, 106);
            Post_Code.Name = "Post_Code";
            Post_Code.PlaceholderText = "00-000";
            Post_Code.Size = new Size(60, 23);
            Post_Code.TabIndex = 70;
            Post_Code.TextAlign = HorizontalAlignment.Center;
            // 
            // label2
            // 
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(0, 77);
            label2.Name = "label2";
            label2.Size = new Size(150, 23);
            label2.TabIndex = 79;
            label2.Text = "Ulica Numer";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Adress
            // 
            Adress.Location = new Point(182, 77);
            Adress.Name = "Adress";
            Adress.Size = new Size(200, 23);
            Adress.TabIndex = 69;
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(0, 19);
            label1.Name = "label1";
            label1.Size = new Size(176, 23);
            label1.TabIndex = 78;
            label1.Text = "Nazwa / Imie Nazwisko";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Name
            // 
            Name.Location = new Point(182, 19);
            Name.Name = "Name";
            Name.Size = new Size(200, 23);
            Name.TabIndex = 67;
            // 
            // groupBox2
            // 
            groupBox2.BackColor = Color.Transparent;
            groupBox2.Controls.Add(label19);
            groupBox2.Controls.Add(Value);
            groupBox2.Controls.Add(label10);
            groupBox2.Controls.Add(Description);
            groupBox2.Controls.Add(label9);
            groupBox2.Location = new Point(12, 243);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(787, 112);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Przedmiot";
            // 
            // Value
            // 
            Value.Location = new Point(182, 81);
            Value.Name = "Value";
            Value.Size = new Size(60, 23);
            Value.TabIndex = 90;
            Value.Text = "0.0";
            Value.TextAlign = HorizontalAlignment.Right;
            Value.KeyPress += Value_KeyPress;
            // 
            // label10
            // 
            label10.BackColor = Color.Transparent;
            label10.Font = new Font("Segoe UI", 12F);
            label10.Location = new Point(0, 81);
            label10.Name = "label10";
            label10.Size = new Size(150, 23);
            label10.TabIndex = 88;
            label10.Text = "Wartość";
            label10.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Description
            // 
            Description.Location = new Point(182, 19);
            Description.Name = "Description";
            Description.Size = new Size(605, 56);
            Description.TabIndex = 86;
            Description.Text = "";
            // 
            // label9
            // 
            label9.BackColor = Color.Transparent;
            label9.Font = new Font("Segoe UI", 12F);
            label9.Location = new Point(0, 19);
            label9.Name = "label9";
            label9.Size = new Size(150, 23);
            label9.TabIndex = 87;
            label9.Text = "Opis Przedmiotu";
            label9.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // groupBox3
            // 
            groupBox3.BackColor = Color.Transparent;
            groupBox3.Controls.Add(label20);
            groupBox3.Controls.Add(Comments);
            groupBox3.Controls.Add(label18);
            groupBox3.Controls.Add(textBox1);
            groupBox3.Controls.Add(label17);
            groupBox3.Controls.Add(Days);
            groupBox3.Controls.Add(label16);
            groupBox3.Controls.Add(Pickup_Date);
            groupBox3.Controls.Add(label14);
            groupBox3.Controls.Add(Issue_Date);
            groupBox3.Controls.Add(label15);
            groupBox3.Location = new Point(12, 361);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(787, 216);
            groupBox3.TabIndex = 91;
            groupBox3.TabStop = false;
            // 
            // Issue_Date
            // 
            Issue_Date.CustomFormat = "dd-MM-yyyy";
            Issue_Date.Format = DateTimePickerFormat.Short;
            Issue_Date.Location = new Point(182, 22);
            Issue_Date.Name = "Issue_Date";
            Issue_Date.Size = new Size(100, 23);
            Issue_Date.TabIndex = 88;
            // 
            // label15
            // 
            label15.BackColor = Color.Transparent;
            label15.Font = new Font("Segoe UI", 12F);
            label15.Location = new Point(6, 22);
            label15.Name = "label15";
            label15.Size = new Size(150, 23);
            label15.TabIndex = 87;
            label15.Text = "Data Przyjęcia";
            label15.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Pickup_Date
            // 
            Pickup_Date.CustomFormat = "dd-MM-yyyy";
            Pickup_Date.Format = DateTimePickerFormat.Short;
            Pickup_Date.Location = new Point(182, 51);
            Pickup_Date.Name = "Pickup_Date";
            Pickup_Date.Size = new Size(100, 23);
            Pickup_Date.TabIndex = 90;
            // 
            // label14
            // 
            label14.BackColor = Color.Transparent;
            label14.Font = new Font("Segoe UI", 12F);
            label14.Location = new Point(6, 51);
            label14.Name = "label14";
            label14.Size = new Size(150, 23);
            label14.TabIndex = 89;
            label14.Text = "Termin Odbioru";
            label14.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label16
            // 
            label16.BackColor = Color.Transparent;
            label16.Font = new Font("Segoe UI", 12F);
            label16.Location = new Point(6, 80);
            label16.Name = "label16";
            label16.Size = new Size(150, 23);
            label16.TabIndex = 91;
            label16.Text = "Ilośc Dni";
            label16.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Days
            // 
            Days.Location = new Point(182, 80);
            Days.Name = "Days";
            Days.Size = new Size(60, 23);
            Days.TabIndex = 92;
            Days.Text = "0";
            Days.TextAlign = HorizontalAlignment.Right;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(182, 109);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(60, 23);
            textBox1.TabIndex = 94;
            textBox1.Text = "0";
            textBox1.TextAlign = HorizontalAlignment.Right;
            // 
            // label17
            // 
            label17.BackColor = Color.Transparent;
            label17.Font = new Font("Segoe UI", 12F);
            label17.Location = new Point(6, 109);
            label17.Name = "label17";
            label17.Size = new Size(150, 23);
            label17.TabIndex = 93;
            label17.Text = "Procent zwyżki";
            label17.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label18
            // 
            label18.BackColor = Color.Transparent;
            label18.Font = new Font("Segoe UI", 12F);
            label18.Location = new Point(242, 109);
            label18.Name = "label18";
            label18.Size = new Size(28, 23);
            label18.TabIndex = 95;
            label18.Text = "%";
            label18.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label19
            // 
            label19.BackColor = Color.Transparent;
            label19.Font = new Font("Segoe UI", 12F);
            label19.Location = new Point(242, 81);
            label19.Name = "label19";
            label19.Size = new Size(22, 23);
            label19.TabIndex = 94;
            label19.Text = "zł";
            label19.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Comments
            // 
            Comments.Location = new Point(182, 138);
            Comments.Name = "Comments";
            Comments.Size = new Size(585, 56);
            Comments.TabIndex = 96;
            Comments.Text = "";
            // 
            // label20
            // 
            label20.BackColor = Color.Transparent;
            label20.Font = new Font("Segoe UI", 12F);
            label20.Location = new Point(6, 148);
            label20.Name = "label20";
            label20.Size = new Size(150, 23);
            label20.TabIndex = 97;
            label20.Text = "Uwagi";
            label20.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // button1
            // 
            button1.BackColor = Color.Transparent;
            button1.BackgroundImage = Properties.Resources.printer;
            button1.BackgroundImageLayout = ImageLayout.Zoom;
            button1.Cursor = Cursors.Hand;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Location = new Point(749, 583);
            button1.Name = "button1";
            button1.Size = new Size(50, 50);
            button1.TabIndex = 92;
            button1.UseVisualStyleBackColor = false;
            // 
            // button2
            // 
            button2.BackColor = Color.Transparent;
            button2.BackgroundImage = Properties.Resources.save;
            button2.BackgroundImageLayout = ImageLayout.Zoom;
            button2.Cursor = Cursors.Hand;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Location = new Point(693, 583);
            button2.Name = "button2";
            button2.Size = new Size(50, 50);
            button2.TabIndex = 93;
            button2.UseVisualStyleBackColor = false;
            // 
            // Issue
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.background;
            ClientSize = new Size(818, 650);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Label label4;
        private TextBox Nip;
        private RichTextBox Notes;
        private Label label3;
        private ComboBox DocumentType;
        private Label label11;
        private Label label12;
        private TextBox Pesel;
        private Label label13;
        private TextBox DocumentNumber;
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
        private TextBox Name;
        private Button Load;
        private Label label6;
        private GroupBox groupBox2;
        private TextBox Value;
        private Label label10;
        private RichTextBox Description;
        private Label label9;
        private GroupBox groupBox3;
        private DateTimePicker Pickup_Date;
        private Label label15;
        private DateTimePicker Issue_Date;
        private Label label14;
        private Label label19;
        private Label label20;
        private RichTextBox Comments;
        private Label label18;
        private TextBox textBox1;
        private Label label17;
        private TextBox Days;
        private Label label16;
        private Button button1;
        private Button button2;
    }
}