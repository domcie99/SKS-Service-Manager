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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintRecords));
            print = new Button();
            DocumentType = new ComboBox();
            PickupDate = new DateTimePicker();
            label14 = new Label();
            IssuedDate = new DateTimePicker();
            label15 = new Label();
            label1 = new Label();
            label2 = new Label();
            IssuedCity = new ComboBox();
            mySqlCommand1 = new MySqlConnector.MySqlCommand();
            dataGridView1 = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
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
            // PickupDate
            // 
            PickupDate.CustomFormat = "dd-MM-yyyy";
            PickupDate.Format = DateTimePickerFormat.Short;
            PickupDate.Location = new Point(201, 133);
            PickupDate.Name = "PickupDate";
            PickupDate.Size = new Size(100, 23);
            PickupDate.TabIndex = 124;
            // 
            // label14
            // 
            label14.BackColor = Color.Transparent;
            label14.Font = new Font("Segoe UI", 12F);
            label14.Location = new Point(45, 133);
            label14.Name = "label14";
            label14.Size = new Size(150, 23);
            label14.TabIndex = 122;
            label14.Text = "Termin Odbioru";
            label14.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // IssuedDate
            // 
            IssuedDate.CustomFormat = "dd-MM-yyyy";
            IssuedDate.Format = DateTimePickerFormat.Short;
            IssuedDate.Location = new Point(201, 104);
            IssuedDate.Name = "IssuedDate";
            IssuedDate.Size = new Size(100, 23);
            IssuedDate.TabIndex = 123;
            IssuedDate.Value = new DateTime(2024, 1, 23, 17, 7, 29, 0);
            // 
            // label15
            // 
            label15.BackColor = Color.Transparent;
            label15.Font = new Font("Segoe UI", 12F);
            label15.Location = new Point(45, 104);
            label15.Name = "label15";
            label15.Size = new Size(150, 23);
            label15.TabIndex = 121;
            label15.Text = "Data Przyjęcia";
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
            // dataGridView1
            // 
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridView1.Location = new Point(27, 275);
            dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = null;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = null;
            dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle4;
            dataGridView1.Size = new Size(1388, 520);
            dataGridView1.TabIndex = 128;
            // 
            // PrintRecords
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.background;
            ClientSize = new Size(1440, 807);
            Controls.Add(dataGridView1);
            Controls.Add(IssuedCity);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(PickupDate);
            Controls.Add(label14);
            Controls.Add(IssuedDate);
            Controls.Add(label15);
            Controls.Add(DocumentType);
            Controls.Add(print);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "PrintRecords";
            Text = "Ewidencja Kupna Sprzedaży";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button print;
        private ComboBox DocumentType;
        private DateTimePicker PickupDate;
        private Label label14;
        private DateTimePicker IssuedDate;
        private Label label15;
        private Label label1;
        private Label label2;
        private ComboBox IssuedCity;
        private MySqlConnector.MySqlCommand mySqlCommand1;
        private DataGridView dataGridView1;
    }
}