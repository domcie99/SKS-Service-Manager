namespace SKS_Service_Manager
{
    partial class IssueUKS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IssueUKS));
            groupBox1 = new GroupBox();
            label1 = new Label();
            Company_Name = new TextBox();
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
            label31 = new Label();
            FullName = new TextBox();
            label6 = new Label();
            LoadUser = new Button();
            groupBox2 = new GroupBox();
            label19 = new Label();
            Value = new TextBox();
            label10 = new Label();
            Description = new RichTextBox();
            label9 = new Label();
            groupBox3 = new GroupBox();
            label29 = new Label();
            SaleAmount = new TextBox();
            label30 = new Label();
            SaleDate = new DateTimePicker();
            label28 = new Label();
            DateOfReturn = new DateTimePicker();
            label27 = new Label();
            label26 = new Label();
            BuyAmount = new TextBox();
            label25 = new Label();
            LateFee = new TextBox();
            label24 = new Label();
            label23 = new Label();
            label21 = new Label();
            Fee = new TextBox();
            label22 = new Label();
            label20 = new Label();
            Comments = new RichTextBox();
            label18 = new Label();
            Percentage = new TextBox();
            label17 = new Label();
            Days = new TextBox();
            label16 = new Label();
            Pickup_Date = new DateTimePicker();
            label14 = new Label();
            Issue_Date = new DateTimePicker();
            label15 = new Label();
            Print = new Button();
            Save = new Button();
            label32 = new Label();
            FormType = new ComboBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.BackColor = Color.Transparent;
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(Company_Name);
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
            groupBox1.Controls.Add(label31);
            groupBox1.Controls.Add(FullName);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(LoadUser);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(787, 225);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Osoba Powierzająca";
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(6, 199);
            label1.Name = "label1";
            label1.Size = new Size(150, 23);
            label1.TabIndex = 114;
            label1.Text = "Nazwa";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Company_Name
            // 
            Company_Name.Location = new Point(182, 199);
            Company_Name.Name = "Company_Name";
            Company_Name.Size = new Size(200, 23);
            Company_Name.TabIndex = 97;
            // 
            // label4
            // 
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI", 12F);
            label4.Location = new Point(6, 170);
            label4.Name = "label4";
            label4.Size = new Size(150, 23);
            label4.TabIndex = 113;
            label4.Text = "NIP";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Nip
            // 
            Nip.Location = new Point(182, 170);
            Nip.Name = "Nip";
            Nip.Size = new Size(200, 23);
            Nip.TabIndex = 96;
            // 
            // Notes
            // 
            Notes.Location = new Point(587, 109);
            Notes.Name = "Notes";
            Notes.Size = new Size(200, 55);
            Notes.TabIndex = 101;
            Notes.Text = "Brak Uwag";
            // 
            // label3
            // 
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 12F);
            label3.Location = new Point(431, 22);
            label3.Name = "label3";
            label3.Size = new Size(150, 23);
            label3.TabIndex = 112;
            label3.Text = "Rodzaj Dokumentu";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // DocumentType
            // 
            DocumentType.AutoCompleteCustomSource.AddRange(new string[] { "Dowód Osobisty", "Prawo Jazdy", "Paszport" });
            DocumentType.DropDownStyle = ComboBoxStyle.DropDownList;
            DocumentType.FormattingEnabled = true;
            DocumentType.Items.AddRange(new object[] { "Dowód Osobisty", "Prawo Jazdy", "Paszport" });
            DocumentType.Location = new Point(587, 22);
            DocumentType.Name = "DocumentType";
            DocumentType.Size = new Size(200, 23);
            DocumentType.TabIndex = 98;
            // 
            // label11
            // 
            label11.BackColor = Color.Transparent;
            label11.Font = new Font("Segoe UI", 12F);
            label11.Location = new Point(431, 109);
            label11.Name = "label11";
            label11.Size = new Size(150, 23);
            label11.TabIndex = 111;
            label11.Text = "Uwagi";
            label11.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            label12.BackColor = Color.Transparent;
            label12.Font = new Font("Segoe UI", 12F);
            label12.Location = new Point(431, 80);
            label12.Name = "label12";
            label12.Size = new Size(150, 23);
            label12.TabIndex = 110;
            label12.Text = "Pesel";
            label12.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Pesel
            // 
            Pesel.Location = new Point(587, 80);
            Pesel.Name = "Pesel";
            Pesel.Size = new Size(200, 23);
            Pesel.TabIndex = 100;
            // 
            // label13
            // 
            label13.BackColor = Color.Transparent;
            label13.Font = new Font("Segoe UI", 12F);
            label13.Location = new Point(431, 51);
            label13.Name = "label13";
            label13.Size = new Size(150, 23);
            label13.TabIndex = 109;
            label13.Text = "Numer Dokumentu";
            label13.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // DocumentNumber
            // 
            DocumentNumber.Location = new Point(587, 51);
            DocumentNumber.Name = "DocumentNumber";
            DocumentNumber.Size = new Size(200, 23);
            DocumentNumber.TabIndex = 99;
            // 
            // label8
            // 
            label8.BackColor = Color.Transparent;
            label8.Font = new Font("Segoe UI", 12F);
            label8.Location = new Point(6, 141);
            label8.Name = "label8";
            label8.Size = new Size(150, 23);
            label8.TabIndex = 108;
            label8.Text = "Email";
            label8.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // EMail
            // 
            EMail.Location = new Point(182, 141);
            EMail.Name = "EMail";
            EMail.Size = new Size(200, 23);
            EMail.TabIndex = 95;
            // 
            // City
            // 
            City.Location = new Point(248, 80);
            City.Name = "City";
            City.Size = new Size(134, 23);
            City.TabIndex = 93;
            // 
            // label7
            // 
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Segoe UI", 12F);
            label7.Location = new Point(6, 112);
            label7.Name = "label7";
            label7.Size = new Size(150, 23);
            label7.TabIndex = 107;
            label7.Text = "Telefon";
            label7.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Phone
            // 
            Phone.Location = new Point(182, 112);
            Phone.Name = "Phone";
            Phone.Size = new Size(200, 23);
            Phone.TabIndex = 94;
            // 
            // label5
            // 
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 12F);
            label5.Location = new Point(6, 80);
            label5.Name = "label5";
            label5.Size = new Size(150, 23);
            label5.TabIndex = 106;
            label5.Text = "Kod Pocztowy";
            label5.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Post_Code
            // 
            Post_Code.Location = new Point(182, 80);
            Post_Code.Name = "Post_Code";
            Post_Code.PlaceholderText = "00-000";
            Post_Code.Size = new Size(60, 23);
            Post_Code.TabIndex = 92;
            Post_Code.TextAlign = HorizontalAlignment.Center;
            // 
            // label2
            // 
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(6, 51);
            label2.Name = "label2";
            label2.Size = new Size(150, 23);
            label2.TabIndex = 105;
            label2.Text = "Ulica Numer";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Adress
            // 
            Adress.Location = new Point(182, 51);
            Adress.Name = "Adress";
            Adress.Size = new Size(200, 23);
            Adress.TabIndex = 91;
            Adress.Text = "ul. ";
            // 
            // label31
            // 
            label31.BackColor = Color.Transparent;
            label31.Font = new Font("Segoe UI", 12F);
            label31.Location = new Point(6, 22);
            label31.Name = "label31";
            label31.Size = new Size(176, 23);
            label31.TabIndex = 104;
            label31.Text = "Imie Nazwisko";
            label31.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // FullName
            // 
            FullName.Location = new Point(182, 22);
            FullName.Name = "FullName";
            FullName.Size = new Size(200, 23);
            FullName.TabIndex = 90;
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
            // LoadUser
            // 
            LoadUser.BackColor = Color.Transparent;
            LoadUser.BackgroundImage = Properties.Resources.add;
            LoadUser.BackgroundImageLayout = ImageLayout.Zoom;
            LoadUser.Cursor = Cursors.Hand;
            LoadUser.FlatAppearance.BorderSize = 0;
            LoadUser.FlatStyle = FlatStyle.Flat;
            LoadUser.Location = new Point(737, 167);
            LoadUser.Name = "LoadUser";
            LoadUser.Size = new Size(50, 50);
            LoadUser.TabIndex = 102;
            LoadUser.UseVisualStyleBackColor = false;
            LoadUser.Click += Load_Click;
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
            // Value
            // 
            Value.Location = new Point(182, 81);
            Value.Name = "Value";
            Value.Size = new Size(60, 23);
            Value.TabIndex = 104;
            Value.Text = "0,00";
            Value.TextAlign = HorizontalAlignment.Right;
            Value.KeyPress += Value_KeyPress;
            Value.Leave += Value_Validation;
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
            Description.TabIndex = 103;
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
            groupBox3.Controls.Add(label29);
            groupBox3.Controls.Add(SaleAmount);
            groupBox3.Controls.Add(label30);
            groupBox3.Controls.Add(SaleDate);
            groupBox3.Controls.Add(label28);
            groupBox3.Controls.Add(DateOfReturn);
            groupBox3.Controls.Add(label27);
            groupBox3.Controls.Add(label26);
            groupBox3.Controls.Add(BuyAmount);
            groupBox3.Controls.Add(label25);
            groupBox3.Controls.Add(LateFee);
            groupBox3.Controls.Add(label24);
            groupBox3.Controls.Add(label23);
            groupBox3.Controls.Add(label21);
            groupBox3.Controls.Add(Fee);
            groupBox3.Controls.Add(label22);
            groupBox3.Controls.Add(label20);
            groupBox3.Controls.Add(Comments);
            groupBox3.Controls.Add(label18);
            groupBox3.Controls.Add(Percentage);
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
            // label29
            // 
            label29.BackColor = Color.Transparent;
            label29.Font = new Font("Segoe UI", 12F);
            label29.Location = new Point(759, 80);
            label29.Name = "label29";
            label29.Size = new Size(22, 23);
            label29.TabIndex = 113;
            label29.Text = "zł";
            label29.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // SaleAmount
            // 
            SaleAmount.Location = new Point(681, 80);
            SaleAmount.Name = "SaleAmount";
            SaleAmount.Size = new Size(77, 23);
            SaleAmount.TabIndex = 114;
            SaleAmount.Text = "0,00";
            SaleAmount.TextAlign = HorizontalAlignment.Right;
            SaleAmount.KeyPress += Value_KeyPress;
            SaleAmount.Leave += Value_Validation;
            // 
            // label30
            // 
            label30.BackColor = Color.Transparent;
            label30.Font = new Font("Segoe UI", 12F);
            label30.Location = new Point(528, 80);
            label30.Name = "label30";
            label30.Size = new Size(147, 23);
            label30.TabIndex = 111;
            label30.Text = "Kwota Sprzedaży";
            label30.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // SaleDate
            // 
            SaleDate.CustomFormat = "dd-MM-yyyy";
            SaleDate.Format = DateTimePickerFormat.Short;
            SaleDate.Location = new Point(681, 51);
            SaleDate.Name = "SaleDate";
            SaleDate.Size = new Size(100, 23);
            SaleDate.TabIndex = 113;
            // 
            // label28
            // 
            label28.BackColor = Color.Transparent;
            label28.Font = new Font("Segoe UI", 12F);
            label28.Location = new Point(528, 51);
            label28.Name = "label28";
            label28.Size = new Size(147, 23);
            label28.TabIndex = 109;
            label28.Text = "Data sprzedaży";
            label28.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // DateOfReturn
            // 
            DateOfReturn.CustomFormat = "dd-MM-yyyy";
            DateOfReturn.Format = DateTimePickerFormat.Short;
            DateOfReturn.Location = new Point(681, 22);
            DateOfReturn.Name = "DateOfReturn";
            DateOfReturn.Size = new Size(100, 23);
            DateOfReturn.TabIndex = 112;
            // 
            // label27
            // 
            label27.BackColor = Color.Transparent;
            label27.Font = new Font("Segoe UI", 12F);
            label27.Location = new Point(528, 22);
            label27.Name = "label27";
            label27.Size = new Size(110, 23);
            label27.TabIndex = 107;
            label27.Text = "Data zwrotu";
            label27.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label26
            // 
            label26.BackColor = Color.Transparent;
            label26.Font = new Font("Segoe UI", 12F);
            label26.Location = new Point(500, 80);
            label26.Name = "label26";
            label26.Size = new Size(22, 23);
            label26.TabIndex = 106;
            label26.Text = "zł";
            label26.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // BuyAmount
            // 
            BuyAmount.Location = new Point(422, 80);
            BuyAmount.Name = "BuyAmount";
            BuyAmount.Size = new Size(77, 23);
            BuyAmount.TabIndex = 111;
            BuyAmount.Text = "0,00";
            BuyAmount.TextAlign = HorizontalAlignment.Right;
            BuyAmount.KeyPress += Value_KeyPress;
            BuyAmount.Leave += Value_Validation;
            // 
            // label25
            // 
            label25.BackColor = Color.Transparent;
            label25.Font = new Font("Segoe UI", 12F);
            label25.Location = new Point(500, 51);
            label25.Name = "label25";
            label25.Size = new Size(22, 23);
            label25.TabIndex = 104;
            label25.Text = "zł";
            label25.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // LateFee
            // 
            LateFee.Location = new Point(422, 51);
            LateFee.Name = "LateFee";
            LateFee.Size = new Size(77, 23);
            LateFee.TabIndex = 110;
            LateFee.Text = "0,00";
            LateFee.TextAlign = HorizontalAlignment.Right;
            LateFee.KeyPress += Value_KeyPress;
            LateFee.Leave += Value_Validation;
            // 
            // label24
            // 
            label24.BackColor = Color.Transparent;
            label24.Font = new Font("Segoe UI", 12F);
            label24.Location = new Point(288, 80);
            label24.Name = "label24";
            label24.Size = new Size(123, 23);
            label24.TabIndex = 102;
            label24.Text = "Kwota Wykupu";
            label24.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label23
            // 
            label23.BackColor = Color.Transparent;
            label23.Font = new Font("Segoe UI", 12F);
            label23.Location = new Point(288, 51);
            label23.Name = "label23";
            label23.Size = new Size(123, 23);
            label23.TabIndex = 101;
            label23.Text = "Opłata za opoź.";
            label23.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label21
            // 
            label21.BackColor = Color.Transparent;
            label21.Font = new Font("Segoe UI", 12F);
            label21.Location = new Point(500, 22);
            label21.Name = "label21";
            label21.Size = new Size(22, 23);
            label21.TabIndex = 100;
            label21.Text = "zł";
            label21.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Fee
            // 
            Fee.Location = new Point(422, 22);
            Fee.Name = "Fee";
            Fee.Size = new Size(77, 23);
            Fee.TabIndex = 109;
            Fee.Text = "0,00";
            Fee.TextAlign = HorizontalAlignment.Right;
            Fee.KeyPress += Value_KeyPress;
            Fee.Leave += Value_Validation;
            // 
            // label22
            // 
            label22.BackColor = Color.Transparent;
            label22.Font = new Font("Segoe UI", 12F);
            label22.Location = new Point(288, 22);
            label22.Name = "label22";
            label22.Size = new Size(94, 23);
            label22.TabIndex = 98;
            label22.Text = "Opłata";
            label22.TextAlign = ContentAlignment.MiddleLeft;
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
            // Comments
            // 
            Comments.Location = new Point(182, 138);
            Comments.Name = "Comments";
            Comments.Size = new Size(585, 56);
            Comments.TabIndex = 115;
            Comments.Text = "Brak Uwag";
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
            // Percentage
            // 
            Percentage.Location = new Point(182, 109);
            Percentage.Name = "Percentage";
            Percentage.Size = new Size(60, 23);
            Percentage.TabIndex = 108;
            Percentage.Text = "0";
            Percentage.TextAlign = HorizontalAlignment.Right;
            Percentage.TextChanged += PercentageChanged;
            Percentage.KeyPress += IsInt_KeyPress;
            Percentage.Leave += Percentage_Leave;
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
            // Days
            // 
            Days.Location = new Point(182, 80);
            Days.Name = "Days";
            Days.Size = new Size(60, 23);
            Days.TabIndex = 107;
            Days.Text = "0";
            Days.TextAlign = HorizontalAlignment.Right;
            Days.KeyPress += IsInt_KeyPress;
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
            // Pickup_Date
            // 
            Pickup_Date.CustomFormat = "dd-MM-yyyy";
            Pickup_Date.Format = DateTimePickerFormat.Short;
            Pickup_Date.Location = new Point(182, 51);
            Pickup_Date.Name = "Pickup_Date";
            Pickup_Date.Size = new Size(100, 23);
            Pickup_Date.TabIndex = 106;
            Pickup_Date.ValueChanged += Issue_Date_ValueChanged;
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
            // Issue_Date
            // 
            Issue_Date.CustomFormat = "dd-MM-yyyy";
            Issue_Date.Format = DateTimePickerFormat.Short;
            Issue_Date.Location = new Point(182, 22);
            Issue_Date.Name = "Issue_Date";
            Issue_Date.Size = new Size(100, 23);
            Issue_Date.TabIndex = 105;
            Issue_Date.ValueChanged += Issue_Date_ValueChanged;
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
            // Print
            // 
            Print.BackColor = Color.Transparent;
            Print.BackgroundImage = Properties.Resources.printer;
            Print.BackgroundImageLayout = ImageLayout.Zoom;
            Print.Cursor = Cursors.Hand;
            Print.FlatAppearance.BorderSize = 0;
            Print.FlatStyle = FlatStyle.Flat;
            Print.Location = new Point(749, 583);
            Print.Name = "Print";
            Print.Size = new Size(50, 50);
            Print.TabIndex = 117;
            Print.UseVisualStyleBackColor = false;
            Print.Click += Print_Click;
            // 
            // Save
            // 
            Save.BackColor = Color.Transparent;
            Save.BackgroundImage = Properties.Resources.save;
            Save.BackgroundImageLayout = ImageLayout.Zoom;
            Save.Cursor = Cursors.Hand;
            Save.FlatAppearance.BorderSize = 0;
            Save.FlatStyle = FlatStyle.Flat;
            Save.Location = new Point(693, 583);
            Save.Name = "Save";
            Save.Size = new Size(50, 50);
            Save.TabIndex = 116;
            Save.UseVisualStyleBackColor = false;
            Save.Click += Save_Click;
            // 
            // label32
            // 
            label32.BackColor = Color.Transparent;
            label32.Font = new Font("Segoe UI", 12F);
            label32.Location = new Point(18, 595);
            label32.Name = "label32";
            label32.Size = new Size(150, 23);
            label32.TabIndex = 119;
            label32.Text = "Rodzaj Umowy";
            label32.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // FormType
            // 
            FormType.AutoCompleteCustomSource.AddRange(new string[] { "Dowód Osobisty", "Prawo Jazdy", "Paszport" });
            FormType.DropDownStyle = ComboBoxStyle.DropDownList;
            FormType.FormattingEnabled = true;
            FormType.Items.AddRange(new object[] { "Umowa Komisowa", "Umowa Kupna-Sprzedaży", "Umowa Pożyczki z Przechowaniem" });
            FormType.Location = new Point(174, 595);
            FormType.Name = "FormType";
            FormType.Size = new Size(220, 23);
            FormType.TabIndex = 118;
            FormType.SelectedValueChanged += FormType_ValueChanged;
            // 
            // IssueUKS
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.background;
            ClientSize = new Size(818, 650);
            Controls.Add(label32);
            Controls.Add(FormType);
            Controls.Add(Save);
            Controls.Add(Print);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "IssueUKS";
            Text = "Tworzenie umowy";
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
        private Button LoadUser;
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
        private TextBox Percentage;
        private Label label17;
        private TextBox Days;
        private Label label16;
        private Button Print;
        private Button Save;
        private Label label21;
        private TextBox Fee;
        private Label label22;
        private Label label26;
        private TextBox BuyAmount;
        private Label label25;
        private TextBox LateFee;
        private Label label24;
        private Label label23;
        private Label label29;
        private TextBox SaleAmount;
        private Label label30;
        private DateTimePicker SaleDate;
        private Label label28;
        private DateTimePicker DateOfReturn;
        private Label label27;
        private Label label1;
        private TextBox Company_Name;
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
        private Label label31;
        private TextBox FullName;
        private Label label32;
        private ComboBox FormType;
    }
}