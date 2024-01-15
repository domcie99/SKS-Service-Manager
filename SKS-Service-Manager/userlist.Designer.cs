namespace SKS_Service_Manager
{
    partial class userlist
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(userlist));
            Add = new Button();
            Edit = new Button();
            delete = new Button();
            dataGridView1 = new DataGridView();
            button1 = new Button();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // Add
            // 
            Add.BackColor = Color.Transparent;
            Add.BackgroundImage = Properties.Resources.add;
            Add.BackgroundImageLayout = ImageLayout.Zoom;
            Add.FlatAppearance.BorderSize = 0;
            Add.FlatStyle = FlatStyle.Flat;
            Add.Location = new Point(319, 12);
            Add.Name = "Add";
            Add.Size = new Size(50, 50);
            Add.TabIndex = 19;
            Add.UseVisualStyleBackColor = false;
            Add.Click += Add_Click;
            // 
            // Edit
            // 
            Edit.BackColor = Color.Transparent;
            Edit.BackgroundImage = Properties.Resources.edit;
            Edit.BackgroundImageLayout = ImageLayout.Zoom;
            Edit.FlatAppearance.BorderSize = 0;
            Edit.FlatStyle = FlatStyle.Flat;
            Edit.Location = new Point(375, 12);
            Edit.Name = "Edit";
            Edit.Size = new Size(50, 50);
            Edit.TabIndex = 20;
            Edit.UseVisualStyleBackColor = false;
            Edit.Click += Edit_Click;
            // 
            // delete
            // 
            delete.BackColor = Color.Transparent;
            delete.BackgroundImage = Properties.Resources.delete;
            delete.BackgroundImageLayout = ImageLayout.Zoom;
            delete.FlatAppearance.BorderSize = 0;
            delete.FlatStyle = FlatStyle.Flat;
            delete.Location = new Point(431, 12);
            delete.Name = "delete";
            delete.Size = new Size(50, 50);
            delete.TabIndex = 21;
            delete.UseVisualStyleBackColor = false;
            delete.Click += delete_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.BackgroundColor = Color.Gainsboro;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 93);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(776, 345);
            dataGridView1.TabIndex = 22;
            // 
            // button1
            // 
            button1.BackColor = Color.Transparent;
            button1.BackgroundImage = Properties.Resources.EdytujFakture;
            button1.BackgroundImageLayout = ImageLayout.Zoom;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Location = new Point(738, 12);
            button1.Name = "button1";
            button1.Size = new Size(50, 50);
            button1.TabIndex = 23;
            button1.UseVisualStyleBackColor = false;
            button1.Visible = false;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Location = new Point(615, 30);
            label1.Name = "label1";
            label1.Size = new Size(114, 15);
            label1.TabIndex = 24;
            label1.Text = "Wybierz Zaznaczone";
            label1.Visible = false;
            // 
            // userlist
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.background;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(button1);
            Controls.Add(dataGridView1);
            Controls.Add(delete);
            Controls.Add(Edit);
            Controls.Add(Add);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "userlist";
            Text = "Lista Użytkowników";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button Add;
        private Button Edit;
        private Button delete;
        private DataGridView dataGridView1;
        private Button button1;
        private Label label1;
    }
}