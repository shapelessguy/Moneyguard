using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moneyguard
{
    public partial class Recover_Pass : Form
    {
        bool recovered = false;
        public Recover_Pass()
        {
            InitializeComponent();
            label1.Text = "Domanda segreta:\n"+Impostazioni.question;
            textBox1.TextChanged += (o,e) => textBox1.BackColor = Color.White;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (recovered) return;
            if(textBox1.Text.ToLower().Trim() == Impostazioni.answer.ToLower().Trim())
            {
                label2.ForeColor = Color.Blue;
                label2.Text = "Riceverai la tua nuova password associata al database via e-mail all'indirizzo specificato";
                label2.Visible = true;
                string secret_pass = Login.RandomString(10);
                if (!FirebaseClass.SendMail(Program.email_user, "Database Password Recover",
                 "Di seguito la tua nuova password associata al database: " + secret_pass
                 ))
                {
                    label2.ForeColor = Color.Red;
                    label2.Text = "Errore sconosciuto, riprova più tardi";
                    return;
                }
                Impostazioni.pass = secret_pass;
                Impostazioni.timeout_pass = 0;
                Impostazioni.question = "";
                Impostazioni.answer = "";
                recovered = true;
            }
            else
            {
                textBox1.BackColor = Color.FromArgb(255, 0, 42);
                textBox1.SelectAll();
            }
        }
    }
}
