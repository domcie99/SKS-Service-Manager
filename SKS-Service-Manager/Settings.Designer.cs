namespace SKS_Service_Manager
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            company_name = new TextBox();
            label1 = new Label();
            label2 = new Label();
            NIP = new TextBox();
            label3 = new Label();
            Name = new TextBox();
            label4 = new Label();
            Surname = new TextBox();
            label5 = new Label();
            Post_Code = new TextBox();
            label6 = new Label();
            Street_And_Number = new TextBox();
            label7 = new Label();
            Phone = new TextBox();
            City = new TextBox();
            label8 = new Label();
            EMail = new TextBox();
            Abort = new Button();
            Save = new Button();
            SuspendLayout();
            // 
            // company_name
            // 
            company_name.Location = new Point(297, 72);
            company_name.Name = "company_name";
            company_name.Size = new Size(200, 23);
            company_name.TabIndex = 0;
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 12F);
            label1.Location = new Point(141, 72);
            label1.Name = "label1";
            label1.Size = new Size(150, 23);
            label1.TabIndex = 1;
            label1.Text = "Nazwa Firmy";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 12F);
            label2.Location = new Point(141, 101);
            label2.Name = "label2";
            label2.Size = new Size(150, 23);
            label2.TabIndex = 3;
            label2.Text = "NIP";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // NIP
            // 
            NIP.Location = new Point(297, 101);
            NIP.Name = "NIP";
            NIP.Size = new Size(200, 23);
            NIP.TabIndex = 2;
            // 
            // label3
            // 
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 12F);
            label3.Location = new Point(141, 130);
            label3.Name = "label3";
            label3.Size = new Size(150, 23);
            label3.TabIndex = 5;
            label3.Text = "Imię";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Name
            // 
            Name.Location = new Point(297, 130);
            Name.Name = "Name";
            Name.Size = new Size(200, 23);
            Name.TabIndex = 4;
            // 
            // label4
            // 
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI", 12F);
            label4.Location = new Point(141, 159);
            label4.Name = "label4";
            label4.Size = new Size(150, 23);
            label4.TabIndex = 7;
            label4.Text = "Nazwisko";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Surname
            // 
            Surname.Location = new Point(297, 159);
            Surname.Name = "Surname";
            Surname.Size = new Size(200, 23);
            Surname.TabIndex = 6;
            // 
            // label5
            // 
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 12F);
            label5.Location = new Point(141, 188);
            label5.Name = "label5";
            label5.Size = new Size(150, 23);
            label5.TabIndex = 9;
            label5.Text = "Kod Pocztowy";
            label5.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Post_Code
            // 
            Post_Code.Location = new Point(297, 188);
            Post_Code.Name = "Post_Code";
            Post_Code.PlaceholderText = "00-000";
            Post_Code.Size = new Size(60, 23);
            Post_Code.TabIndex = 8;
            Post_Code.TextAlign = HorizontalAlignment.Center;
            // 
            // label6
            // 
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Segoe UI", 12F);
            label6.Location = new Point(141, 217);
            label6.Name = "label6";
            label6.Size = new Size(150, 23);
            label6.TabIndex = 11;
            label6.Text = "Ulica Numer";
            label6.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Street_And_Number
            // 
            Street_And_Number.Location = new Point(297, 217);
            Street_And_Number.Name = "Street_And_Number";
            Street_And_Number.Size = new Size(200, 23);
            Street_And_Number.TabIndex = 10;
            // 
            // label7
            // 
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Segoe UI", 12F);
            label7.Location = new Point(141, 246);
            label7.Name = "label7";
            label7.Size = new Size(150, 23);
            label7.TabIndex = 13;
            label7.Text = "Telefon";
            label7.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Phone
            // 
            Phone.Location = new Point(297, 246);
            Phone.Name = "Phone";
            Phone.Size = new Size(200, 23);
            Phone.TabIndex = 12;
            // 
            // City
            // 
            City.Location = new Point(363, 188);
            City.Name = "City";
            City.Size = new Size(134, 23);
            City.TabIndex = 14;
            // 
            // label8
            // 
            label8.BackColor = Color.Transparent;
            label8.Font = new Font("Segoe UI", 12F);
            label8.Location = new Point(141, 275);
            label8.Name = "label8";
            label8.Size = new Size(150, 23);
            label8.TabIndex = 16;
            label8.Text = "Email";
            label8.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // EMail
            // 
            EMail.Location = new Point(297, 275);
            EMail.Name = "EMail";
            EMail.Size = new Size(200, 23);
            EMail.TabIndex = 15;
            // 
            // Abort
            // 
            Abort.BackColor = Color.Transparent;
            Abort.BackgroundImage = Properties.Resources.abort;
            Abort.BackgroundImageLayout = ImageLayout.Zoom;
            Abort.FlatAppearance.BorderSize = 0;
            Abort.FlatStyle = FlatStyle.Flat;
            Abort.Location = new Point(373, 338);
            Abort.Name = "Abort";
            Abort.Size = new Size(50, 50);
            Abort.TabIndex = 17;
            Abort.UseVisualStyleBackColor = false;
            // 
            // Save
            // 
            Save.BackColor = Color.Transparent;
            Save.BackgroundImage = Properties.Resources.save;
            Save.BackgroundImageLayout = ImageLayout.Zoom;
            Save.FlatAppearance.BorderSize = 0;
            Save.FlatStyle = FlatStyle.Flat;
            Save.Location = new Point(447, 338);
            Save.Name = "Save";
            Save.Size = new Size(50, 50);
            Save.TabIndex = 18;
            Save.UseVisualStyleBackColor = false;
            // 
            // Settings
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.background;
            ClientSize = new Size(657, 457);
            Controls.Add(Save);
            Controls.Add(Abort);
            Controls.Add(label8);
            Controls.Add(EMail);
            Controls.Add(City);
            Controls.Add(label7);
            Controls.Add(Phone);
            Controls.Add(label6);
            Controls.Add(Street_And_Number);
            Controls.Add(label5);
            Controls.Add(Post_Code);
            Controls.Add(label4);
            Controls.Add(Surname);
            Controls.Add(label3);
            Controls.Add(Name);
            Controls.Add(label2);
            Controls.Add(NIP);
            Controls.Add(label1);
            Controls.Add(company_name);
            DoubleBuffered = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Settings";
            Text = "Settings";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox company_name;
        private Label label1;
        private Label label2;
        private TextBox NIP;
        private Label label3;
        private TextBox Name;
        private Label label4;
        private TextBox Surname;
        private Label label5;
        private TextBox Post_Code;
        private Label label6;
        private TextBox Street_And_Number;
        private Label label7;
        private TextBox Phone;
        private TextBox City;
        private Label label8;
        private TextBox EMail;
        private Button Abort;
        private Button Save;
    }
}