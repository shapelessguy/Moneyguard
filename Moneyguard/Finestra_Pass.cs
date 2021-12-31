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
    public partial class Finestra_Pass : Form
    {
        Timer timer;
        public Finestra_Pass()
        {
            InitializeComponent();
            timer = new Timer()
            {
                Enabled = true,
                Interval = 10,
            };
            timer.Tick += Animation;
            label1.Text = Program.email_user;
            error.Visible = false;
            progressBar1.Visible = false;
        }

        void SetError(string error_str, Color color)
        {
            error.Text = error_str;
            error.ForeColor = color;
            error.Visible = true;
            Update();
        }

        int iter = 0;
        void Animation(object sender, EventArgs e)
        {
            if (progressBar1.Value > 99 && iter < 10) { progressBar1.Value = 100; iter++; return; }
            if (progressBar1.Value == 100) { iter = 0; progressBar1.Value = 0; return; }

            if (progressBar1.Value == 0 && iter < 1) { iter++; }
            else { progressBar1.Value += 1; iter = 0; }
        }
        void ActiveProgress()
        {
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            progressBar1.BringToFront();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != Program.pass_user) { SetError("Password errata", Color.Red); textBox1.Text = ""; return; }
            if (textBox2.Text.Trim() != textBox3.Text.Trim()) { SetError("Le due password non corrispondono", Color.Red); textBox2.Text = ""; textBox3.Text = ""; return; }
            if (textBox2.Text.Trim().Length < 8) { SetError("La nuova password deve contenere almeno 8 caratteri", Color.Red); textBox2.Text = ""; textBox3.Text = ""; return; }

            ActiveProgress();
            if (await FirebaseClass.ChangePassword(textBox2.Text.Trim())) Close();
            else { SetError("Errore nella modifica della password, riprova più tardi.", Color.Red); progressBar1.Visible = false; }
        }
    }
}
