namespace Moneyguard
{
    partial class RegistrationForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.passTbox = new System.Windows.Forms.TextBox();
            this.UsernameTbox = new System.Windows.Forms.TextBox();
            this.nameTbox = new System.Windows.Forms.TextBox();
            this.GenderCbox = new System.Windows.Forms.ComboBox();
            this.nicTbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(164, 263);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 34);
            this.button1.TabIndex = 6;
            this.button1.Text = "Register";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // passTbox
            // 
            this.passTbox.Location = new System.Drawing.Point(91, 74);
            this.passTbox.Name = "passTbox";
            this.passTbox.Size = new System.Drawing.Size(292, 20);
            this.passTbox.TabIndex = 5;
            // 
            // UsernameTbox
            // 
            this.UsernameTbox.Location = new System.Drawing.Point(91, 28);
            this.UsernameTbox.Name = "UsernameTbox";
            this.UsernameTbox.Size = new System.Drawing.Size(292, 20);
            this.UsernameTbox.TabIndex = 4;
            // 
            // nameTbox
            // 
            this.nameTbox.Location = new System.Drawing.Point(91, 162);
            this.nameTbox.Name = "nameTbox";
            this.nameTbox.Size = new System.Drawing.Size(292, 20);
            this.nameTbox.TabIndex = 7;
            // 
            // GenderCbox
            // 
            this.GenderCbox.FormattingEnabled = true;
            this.GenderCbox.Items.AddRange(new object[] {
            "Male",
            "Female"});
            this.GenderCbox.Location = new System.Drawing.Point(91, 117);
            this.GenderCbox.Name = "GenderCbox";
            this.GenderCbox.Size = new System.Drawing.Size(292, 21);
            this.GenderCbox.TabIndex = 8;
            // 
            // nicTbox
            // 
            this.nicTbox.Location = new System.Drawing.Point(91, 206);
            this.nicTbox.Name = "nicTbox";
            this.nicTbox.Size = new System.Drawing.Size(292, 20);
            this.nicTbox.TabIndex = 9;
            // 
            // RegistrationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 327);
            this.Controls.Add(this.nicTbox);
            this.Controls.Add(this.GenderCbox);
            this.Controls.Add(this.nameTbox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.passTbox);
            this.Controls.Add(this.UsernameTbox);
            this.Name = "RegistrationForm";
            this.Text = "RegistrationForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox passTbox;
        private System.Windows.Forms.TextBox UsernameTbox;
        private System.Windows.Forms.TextBox nameTbox;
        private System.Windows.Forms.ComboBox GenderCbox;
        private System.Windows.Forms.TextBox nicTbox;
    }
}