namespace SKS_Service_Manager
{
    partial class DataBaseInsert
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
            button1 = new Button();
            button2 = new Button();
            openFileDialog1 = new OpenFileDialog();
            textBox1 = new TextBox();
            progressBar = new ProgressBar();
            label1 = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(12, 12);
            button1.Name = "button1";
            button1.Size = new Size(122, 23);
            button1.TabIndex = 0;
            button1.Text = "Wybierz Plik";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(429, 12);
            button2.Name = "button2";
            button2.Size = new Size(113, 23);
            button2.TabIndex = 1;
            button2.Text = "Importuj Baze";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 41);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(530, 23);
            textBox1.TabIndex = 3;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(12, 110);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(530, 39);
            progressBar.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 92);
            label1.Name = "label1";
            label1.Size = new Size(0, 15);
            label1.TabIndex = 5;
            // 
            // DataBaseInsert
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(559, 161);
            Controls.Add(label1);
            Controls.Add(progressBar);
            Controls.Add(textBox1);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "DataBaseInsert";
            Text = "DataBaseInsert";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private OpenFileDialog openFileDialog1;
        private TextBox textBox1;
        private ProgressBar progressBar;
        private Label label1;
    }
}