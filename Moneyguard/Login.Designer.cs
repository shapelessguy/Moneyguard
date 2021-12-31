namespace Moneyguard
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.UsernameTbox = new System.Windows.Forms.TextBox();
            this.registra = new System.Windows.Forms.Button();
            this.accedi = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.passTbox = new System.Windows.Forms.TextBox();
            this.offline = new System.Windows.Forms.Button();
            this.error = new System.Windows.Forms.Label();
            this.passdimenticata = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.automat = new System.Windows.Forms.CheckBox();
            this.ricorda = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.error2 = new System.Windows.Forms.Label();
            this.mess_label = new System.Windows.Forms.Label();
            this.continua = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.secretTxb = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.picture = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // UsernameTbox
            // 
            this.UsernameTbox.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UsernameTbox.Location = new System.Drawing.Point(3, 42);
            this.UsernameTbox.Name = "UsernameTbox";
            this.UsernameTbox.Size = new System.Drawing.Size(297, 33);
            this.UsernameTbox.TabIndex = 1;
            this.UsernameTbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // registra
            // 
            this.registra.Font = new System.Drawing.Font("Modern No. 20", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.registra.Location = new System.Drawing.Point(6, 241);
            this.registra.Name = "registra";
            this.registra.Size = new System.Drawing.Size(127, 34);
            this.registra.TabIndex = 0;
            this.registra.TabStop = false;
            this.registra.Text = "Registra";
            this.registra.UseVisualStyleBackColor = false;
            this.registra.Click += new System.EventHandler(this.registra_Click);
            // 
            // accedi
            // 
            this.accedi.BackColor = System.Drawing.Color.PaleTurquoise;
            this.accedi.Font = new System.Drawing.Font("Modern No. 20", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accedi.Location = new System.Drawing.Point(173, 241);
            this.accedi.Name = "accedi";
            this.accedi.Size = new System.Drawing.Size(127, 34);
            this.accedi.TabIndex = 0;
            this.accedi.TabStop = false;
            this.accedi.Text = "Accedi";
            this.accedi.UseVisualStyleBackColor = false;
            this.accedi.Click += new System.EventHandler(this.accedi_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Script MT Bold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(122, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "E-mail";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Script MT Bold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(108, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 25);
            this.label2.TabIndex = 6;
            this.label2.Text = "Password";
            // 
            // passTbox
            // 
            this.passTbox.Font = new System.Drawing.Font("Palatino Linotype", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passTbox.Location = new System.Drawing.Point(3, 116);
            this.passTbox.Name = "passTbox";
            this.passTbox.Size = new System.Drawing.Size(297, 33);
            this.passTbox.TabIndex = 2;
            this.passTbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.passTbox.UseSystemPasswordChar = true;
            // 
            // offline
            // 
            this.offline.BackColor = System.Drawing.Color.LightCyan;
            this.offline.Font = new System.Drawing.Font("Modern No. 20", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.offline.Location = new System.Drawing.Point(97, 295);
            this.offline.Name = "offline";
            this.offline.Size = new System.Drawing.Size(127, 34);
            this.offline.TabIndex = 0;
            this.offline.TabStop = false;
            this.offline.Text = "Offline";
            this.offline.UseVisualStyleBackColor = false;
            this.offline.Click += new System.EventHandler(this.offline_Click);
            // 
            // error
            // 
            this.error.BackColor = System.Drawing.Color.Azure;
            this.error.Font = new System.Drawing.Font("Modern No. 20", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.error.ForeColor = System.Drawing.Color.Red;
            this.error.Location = new System.Drawing.Point(19, 331);
            this.error.Name = "error";
            this.error.Size = new System.Drawing.Size(285, 38);
            this.error.TabIndex = 8;
            this.error.Text = "Errore nella registrazione.";
            this.error.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.error.Visible = false;
            // 
            // passdimenticata
            // 
            this.passdimenticata.Font = new System.Drawing.Font("Modern No. 20", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passdimenticata.ForeColor = System.Drawing.Color.Blue;
            this.passdimenticata.Location = new System.Drawing.Point(74, 153);
            this.passdimenticata.Name = "passdimenticata";
            this.passdimenticata.Size = new System.Drawing.Size(162, 20);
            this.passdimenticata.TabIndex = 9;
            this.passdimenticata.Text = "Password dimenticata";
            this.passdimenticata.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.passdimenticata.Click += new System.EventHandler(this.passdimenticata_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Azure;
            this.panel1.Controls.Add(this.automat);
            this.panel1.Controls.Add(this.ricorda);
            this.panel1.Controls.Add(this.passdimenticata);
            this.panel1.Controls.Add(this.error);
            this.panel1.Controls.Add(this.offline);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.passTbox);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.accedi);
            this.panel1.Controls.Add(this.registra);
            this.panel1.Controls.Add(this.UsernameTbox);
            this.panel1.Location = new System.Drawing.Point(9, 123);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(304, 371);
            this.panel1.TabIndex = 10;
            // 
            // automat
            // 
            this.automat.AutoSize = true;
            this.automat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.automat.Location = new System.Drawing.Point(34, 209);
            this.automat.Name = "automat";
            this.automat.Size = new System.Drawing.Size(185, 20);
            this.automat.TabIndex = 11;
            this.automat.Text = "Entra automaticamente";
            this.automat.UseVisualStyleBackColor = true;
            // 
            // ricorda
            // 
            this.ricorda.AutoSize = true;
            this.ricorda.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ricorda.Location = new System.Drawing.Point(34, 182);
            this.ricorda.Name = "ricorda";
            this.ricorda.Size = new System.Drawing.Size(162, 20);
            this.ricorda.TabIndex = 10;
            this.ricorda.Text = "Ricorda credenziali";
            this.ricorda.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Azure;
            this.panel2.Controls.Add(this.error2);
            this.panel2.Controls.Add(this.mess_label);
            this.panel2.Controls.Add(this.continua);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.secretTxb);
            this.panel2.Location = new System.Drawing.Point(9, 123);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(304, 370);
            this.panel2.TabIndex = 11;
            // 
            // error2
            // 
            this.error2.Font = new System.Drawing.Font("Modern No. 20", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.error2.ForeColor = System.Drawing.Color.Red;
            this.error2.Location = new System.Drawing.Point(5, 310);
            this.error2.Name = "error2";
            this.error2.Size = new System.Drawing.Size(295, 35);
            this.error2.TabIndex = 16;
            this.error2.Text = "Errore nella registrazione.";
            this.error2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.error2.Visible = false;
            // 
            // mess_label
            // 
            this.mess_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mess_label.Location = new System.Drawing.Point(5, 56);
            this.mess_label.Name = "mess_label";
            this.mess_label.Size = new System.Drawing.Size(299, 111);
            this.mess_label.TabIndex = 15;
            this.mess_label.Text = "Tra pochi istanti dovresti ricevere una e-mail all\'indirizzo di posta indicato. R" +
    "icopia il codice che troverai all\'interno del messaggio nello spazio qui sotto, " +
    "per poi continuare la registrazione.";
            this.mess_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // continua
            // 
            this.continua.BackColor = System.Drawing.Color.LightCyan;
            this.continua.Font = new System.Drawing.Font("Modern No. 20", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.continua.Location = new System.Drawing.Point(62, 252);
            this.continua.Name = "continua";
            this.continua.Size = new System.Drawing.Size(175, 39);
            this.continua.TabIndex = 14;
            this.continua.Text = "Continua";
            this.continua.UseVisualStyleBackColor = false;
            this.continua.Click += new System.EventHandler(this.continua_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.LightCyan;
            this.button2.BackgroundImage = global::Moneyguard.Properties.Resources.Freccia_sx;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.Font = new System.Drawing.Font("Modern No. 20", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(16, 8);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(52, 34);
            this.button2.TabIndex = 13;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // secretTxb
            // 
            this.secretTxb.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.secretTxb.Location = new System.Drawing.Point(36, 181);
            this.secretTxb.Name = "secretTxb";
            this.secretTxb.Size = new System.Drawing.Size(228, 49);
            this.secretTxb.TabIndex = 11;
            this.secretTxb.Text = "47390276";
            this.secretTxb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(24, 493);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(271, 10);
            this.progressBar1.TabIndex = 11;
            this.progressBar1.Visible = false;
            // 
            // picture
            // 
            this.picture.BackColor = System.Drawing.Color.Transparent;
            this.picture.Location = new System.Drawing.Point(78, 9);
            this.picture.Name = "picture";
            this.picture.Size = new System.Drawing.Size(165, 111);
            this.picture.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(291, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 14);
            this.label3.TabIndex = 13;
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.BackgroundImage = global::Moneyguard.Properties.Resources.Sfondo;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(323, 507);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.picture);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.panel2);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login";
            this.Opacity = 0.9D;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login Moneyguard";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.SystemColors.WindowFrame;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox UsernameTbox;
        private System.Windows.Forms.Button registra;
        private System.Windows.Forms.Button accedi;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox passTbox;
        private System.Windows.Forms.Button offline;
        private System.Windows.Forms.Label error;
        private System.Windows.Forms.Label passdimenticata;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox secretTxb;
        private System.Windows.Forms.Label error2;
        private System.Windows.Forms.Label mess_label;
        private System.Windows.Forms.Button continua;
        private System.Windows.Forms.CheckBox ricorda;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label picture;
        private System.Windows.Forms.CheckBox automat;
        private System.Windows.Forms.Label label3;
    }
}