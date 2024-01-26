namespace SKS_Service_Manager
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            button1 = new Button();
            button2 = new Button();
            button7 = new Button();
            label1 = new Label();
            label8 = new Label();
            label9 = new Label();
            label11 = new Label();
            button8 = new Button();
            label12 = new Label();
            pictureBox1 = new PictureBox();
            timer1 = new System.Windows.Forms.Timer(components);
            progressBar1 = new ProgressBar();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = Color.Transparent;
            button1.BackgroundImage = Properties.Resources.WystawFakture;
            button1.BackgroundImageLayout = ImageLayout.Zoom;
            button1.Cursor = Cursors.Hand;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Location = new Point(35, 21);
            button1.Name = "button1";
            button1.Size = new Size(100, 100);
            button1.TabIndex = 0;
            button1.UseMnemonic = false;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.Transparent;
            button2.BackgroundImage = Properties.Resources.WystawFakture;
            button2.BackgroundImageLayout = ImageLayout.Zoom;
            button2.Cursor = Cursors.Hand;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button2.Location = new Point(210, 21);
            button2.Name = "button2";
            button2.Size = new Size(100, 100);
            button2.TabIndex = 7;
            button2.UseMnemonic = false;
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button7
            // 
            button7.BackColor = Color.Transparent;
            button7.BackgroundImage = (Image)resources.GetObject("button7.BackgroundImage");
            button7.BackgroundImageLayout = ImageLayout.Zoom;
            button7.Cursor = Cursors.Hand;
            button7.FlatAppearance.BorderSize = 0;
            button7.FlatStyle = FlatStyle.Flat;
            button7.Location = new Point(398, 21);
            button7.Name = "button7";
            button7.Size = new Size(100, 100);
            button7.TabIndex = 12;
            button7.UseMnemonic = false;
            button7.UseVisualStyleBackColor = false;
            button7.Click += button7_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label1.Location = new Point(389, 124);
            label1.Name = "label1";
            label1.Size = new Size(119, 42);
            label1.TabIndex = 13;
            label1.Text = "Lista\r\nKontrahentów";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            label8.BackColor = Color.Transparent;
            label8.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label8.Location = new Point(35, 124);
            label8.Name = "label8";
            label8.Size = new Size(100, 21);
            label8.TabIndex = 20;
            label8.Text = "Wystaw";
            label8.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            label9.BackColor = Color.Transparent;
            label9.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label9.Location = new Point(210, 124);
            label9.Name = "label9";
            label9.Size = new Size(100, 21);
            label9.TabIndex = 21;
            label9.Text = "Lista";
            label9.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.BackColor = Color.Transparent;
            label11.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label11.Location = new Point(602, 124);
            label11.Name = "label11";
            label11.Size = new Size(95, 21);
            label11.TabIndex = 24;
            label11.Text = "Ustawienia";
            // 
            // button8
            // 
            button8.BackColor = Color.Transparent;
            button8.BackgroundImage = Properties.Resources.Ustawienia;
            button8.BackgroundImageLayout = ImageLayout.Zoom;
            button8.Cursor = Cursors.Hand;
            button8.FlatAppearance.BorderSize = 0;
            button8.FlatStyle = FlatStyle.Flat;
            button8.Location = new Point(597, 21);
            button8.Name = "button8";
            button8.Size = new Size(100, 100);
            button8.TabIndex = 23;
            button8.UseMnemonic = false;
            button8.UseVisualStyleBackColor = false;
            button8.Click += button8_Click;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.BackColor = Color.Transparent;
            label12.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label12.Location = new Point(457, 201);
            label12.Name = "label12";
            label12.Size = new Size(207, 21);
            label12.TabIndex = 25;
            label12.Text = "Połączenie z Bazą Danych";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = Properties.Resources.no_connection;
            pictureBox1.Location = new Point(670, 201);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(25, 25);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 26;
            pictureBox1.TabStop = false;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(12, 204);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(247, 22);
            progressBar1.TabIndex = 27;
            progressBar1.Visible = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI", 10F);
            label2.Location = new Point(12, 182);
            label2.Name = "label2";
            label2.Size = new Size(81, 19);
            label2.TabIndex = 28;
            label2.Text = "Pobieranie...";
            label2.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.background;
            ClientSize = new Size(735, 238);
            Controls.Add(label2);
            Controls.Add(progressBar1);
            Controls.Add(pictureBox1);
            Controls.Add(label12);
            Controls.Add(label11);
            Controls.Add(button8);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label1);
            Controls.Add(button7);
            Controls.Add(button2);
            Controls.Add(button1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "SKS-Service Manager";
            Shown += Form1_Shown;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private Button button7;
        private Label label1;
        private Label label8;
        private Label label9;
        private Label label11;
        private Button button8;
        private Label label12;
        private PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private ProgressBar progressBar1;
        private Label label2;
    }
}
