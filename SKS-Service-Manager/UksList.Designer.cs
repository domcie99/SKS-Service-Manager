namespace SKS_Service_Manager
{
    partial class UksList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UksList));
            delete = new Button();
            Edit = new Button();
            Add = new Button();
            dataGridView1 = new DataGridView();
            search = new TextBox();
            label1 = new Label();
            print = new Button();
            IssuedCity = new ComboBox();
            label2 = new Label();
            label3 = new Label();
            maxRowsDt = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)maxRowsDt).BeginInit();
            SuspendLayout();
            // 
            // delete
            // 
            delete.BackColor = Color.Transparent;
            delete.BackgroundImage = Properties.Resources.delete;
            delete.BackgroundImageLayout = ImageLayout.Zoom;
            delete.FlatAppearance.BorderSize = 0;
            delete.FlatStyle = FlatStyle.Flat;
            delete.Location = new Point(124, 25);
            delete.Name = "delete";
            delete.Size = new Size(50, 50);
            delete.TabIndex = 27;
            delete.UseVisualStyleBackColor = false;
            delete.Click += delete_Click;
            // 
            // Edit
            // 
            Edit.BackColor = Color.Transparent;
            Edit.BackgroundImage = Properties.Resources.edit;
            Edit.BackgroundImageLayout = ImageLayout.Zoom;
            Edit.FlatAppearance.BorderSize = 0;
            Edit.FlatStyle = FlatStyle.Flat;
            Edit.Location = new Point(68, 25);
            Edit.Name = "Edit";
            Edit.Size = new Size(50, 50);
            Edit.TabIndex = 26;
            Edit.UseVisualStyleBackColor = false;
            Edit.Click += Edit_Click;
            // 
            // Add
            // 
            Add.BackColor = Color.Transparent;
            Add.BackgroundImage = Properties.Resources.add;
            Add.BackgroundImageLayout = ImageLayout.Zoom;
            Add.FlatAppearance.BorderSize = 0;
            Add.FlatStyle = FlatStyle.Flat;
            Add.Location = new Point(12, 25);
            Add.Name = "Add";
            Add.Size = new Size(50, 50);
            Add.TabIndex = 25;
            Add.UseVisualStyleBackColor = false;
            Add.Click += Add_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            dataGridView1.BackgroundColor = Color.Gainsboro;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridView1.Location = new Point(12, 93);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(1119, 375);
            dataGridView1.TabIndex = 28;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            // 
            // search
            // 
            search.Location = new Point(942, 64);
            search.Name = "search";
            search.Size = new Size(189, 23);
            search.TabIndex = 29;
            search.TextChanged += SearchUserValueChange;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Location = new Point(942, 46);
            label1.Name = "label1";
            label1.Size = new Size(59, 15);
            label1.TabIndex = 30;
            label1.Text = "Wyszukaj:";
            // 
            // print
            // 
            print.BackColor = Color.Transparent;
            print.BackgroundImage = Properties.Resources.printer;
            print.BackgroundImageLayout = ImageLayout.Zoom;
            print.Cursor = Cursors.Hand;
            print.FlatAppearance.BorderSize = 0;
            print.FlatStyle = FlatStyle.Flat;
            print.Location = new Point(180, 25);
            print.Name = "print";
            print.Size = new Size(50, 50);
            print.TabIndex = 118;
            print.UseVisualStyleBackColor = false;
            print.Click += print_Click;
            // 
            // IssuedCity
            // 
            IssuedCity.AutoCompleteCustomSource.AddRange(new string[] { "Dowód Osobisty", "Prawo Jazdy", "Paszport" });
            IssuedCity.DropDownStyle = ComboBoxStyle.DropDownList;
            IssuedCity.FormattingEnabled = true;
            IssuedCity.Location = new Point(590, 64);
            IssuedCity.Name = "IssuedCity";
            IssuedCity.Size = new Size(220, 23);
            IssuedCity.TabIndex = 128;
            IssuedCity.TextChanged += IssuedCity_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Location = new Point(590, 46);
            label2.Name = "label2";
            label2.Size = new Size(115, 15);
            label2.TabIndex = 129;
            label2.Text = "Miasto Wystawienia:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Location = new Point(816, 46);
            label3.Name = "label3";
            label3.Size = new Size(78, 15);
            label3.TabIndex = 131;
            label3.Text = "Max Wpisów:";
            // 
            // maxRowsDt
            // 
            maxRowsDt.Location = new Point(816, 64);
            maxRowsDt.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
            maxRowsDt.Name = "maxRowsDt";
            maxRowsDt.Size = new Size(120, 23);
            maxRowsDt.TabIndex = 132;
            maxRowsDt.ValueChanged += maxRowsDt_ValueChanged;
            // 
            // UksList
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.background;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1143, 480);
            Controls.Add(maxRowsDt);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(IssuedCity);
            Controls.Add(print);
            Controls.Add(label1);
            Controls.Add(search);
            Controls.Add(dataGridView1);
            Controls.Add(delete);
            Controls.Add(Edit);
            Controls.Add(Add);
            DoubleBuffered = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "UksList";
            Text = "Umowy Kupna-Sprzedaży";
            SizeChanged += UksList_SizeChanged;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)maxRowsDt).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button delete;
        private Button Edit;
        private Button Add;
        private DataGridView dataGridView1;
        private TextBox search;
        private Label label1;
        private Button print;
        private ComboBox IssuedCity;
        private Label label2;
        private Label label3;
        private NumericUpDown maxRowsDt;
    }
}