using Microsoft.Win32;
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
    public partial class Finestra_Impostazioni : Form
    {
        Sicurezza sicurezza;
        RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        ToolTip tooltip;
        public Finestra_Impostazioni()
        {
            InitializeComponent();
            tooltip = new ToolTip() { AutoPopDelay = 20000, };
            FormClosing += Disposer;
        }
        private void Disposer(object sender, EventArgs e)
        {
            Disposer();
        }
        private void Disposer()
        {
            BackPanel.backup = false;
            if (sicurezza!= null) if(sicurezza.Visible) {sicurezza.Close(); sicurezza.Dispose(); }
            Dispose();
        }

        private void Finestra_Impostazioni_Load(object sender, EventArgs e)
        {
            tooltip.SetToolTip(label4, "Tempo di inattività dopo il quale viene richiesto di inserire nuovamente la password.\n" +
                "Il valore 0 contraddistingue l'assenza di timeout.");
            tooltip.SetToolTip(checkBox7, "Il database viene sincronizzato automaticamente dopo 30 minuti di inattività");
            textBox1.Font = new Font(BackPanel.font3, 10, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            textBox2.Font = textBox1.Font;
            textBox1.PasswordChar = '*';
            textBox2.PasswordChar = '*';
            if (Impostazioni.close_inApplicationsBar) checkBox1.Checked = true;
            if (Impostazioni.mute) checkBox2.Checked = true;
            if (Impostazioni.show_calendar_when_opened) checkBox3.Checked = true;
            if (Impostazioni.widget_visible) checkBox4.Checked = true;
            if (Impostazioni.show_maximized) checkBox6.Checked = true;
            if (Impostazioni.aut_sync) checkBox7.Checked = true;
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            textBox4.Visible = false;
            dateTimePicker2.Visible = false;
            if (Impostazioni.pass == "none")
            {
                button3.Text = "Imposta Password";
            }
            else
            {
                button3.Text = "Cancella Password";
            }
            dateTimePicker1.Value = new DateTime(2000,1,1,Impostazioni.ora_minuto.X, Impostazioni.ora_minuto.Y, 0);
            if (rk.GetValue("Moneyguard.exe") != null) checkBox5.Checked = true;

            textBox3.TextChanged += (o, ex) => { if (textBox3.Text != "") textBox3.BackColor = Color.White; };
            textBox4.TextChanged += (o, ex) => { if (textBox4.Text != "") textBox4.BackColor = Color.White; };
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" || textBox2.Text != "")
            {
                if (textBox1.Text != textBox2.Text) { textBox1.BackColor = Color.FromArgb(255, 0, 42); textBox2.BackColor = textBox1.BackColor; return; }
                else
                {
                    if (textBox3.Text == "") { textBox3.BackColor = Color.FromArgb(255, 0, 42); }
                    if (textBox4.Text == "") { textBox4.BackColor = Color.FromArgb(255, 0, 42); }
                    if (textBox3.Text == "" || textBox4.Text == "") return;
                    Impostazioni.pass = textBox1.Text; Impostazioni.timeout_pass = dateTimePicker2.Value.Minute;
                    Impostazioni.question = textBox4.Text; Impostazioni.answer = textBox3.Text;
                }
            }
            Hide();
            Update();
            Impostazioni.close_inApplicationsBar = checkBox1.Checked;
            Impostazioni.mute = checkBox2.Checked;
            Impostazioni.show_calendar_when_opened = checkBox3.Checked;
            Impostazioni.widget_visible = checkBox4.Checked;
            Impostazioni.aut_sync = checkBox7.Checked;
            if (checkBox5.Checked)
            {
                rk.SetValue("Moneyguard.exe", Application.ExecutablePath);
            }
            else rk.DeleteValue("Moneyguard.exe", false);
            Impostazioni.show_maximized = checkBox6.Checked;
            Impostazioni.ora_minuto = new Point(dateTimePicker1.Value.Hour, dateTimePicker1.Value.Minute);

            Savings.SaveEvents();

            Disposer();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if(sicurezza != null) if (sicurezza.Visible) return;
            sicurezza = new Sicurezza();
            sicurezza.Show();
            sicurezza.Location = new Point(Location.X + (Width - sicurezza.Width)/2, Location.Y + (Height - sicurezza.Height) / 2);
        }
        

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            Impostazioni.ora_minuto = new Point(dateTimePicker1.Value.Hour, dateTimePicker1.Value.Minute);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(button3.Text == "Imposta Password")
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                dateTimePicker2.Value = new DateTime(2018, 01, 01, 0, 0, 0); 
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
                label6.Visible = true;
                label7.Visible = true;
                dateTimePicker2.Visible = true;
                textBox1.Visible = true;
                textBox2.Visible = true;
                textBox3.Visible = true;
                textBox4.Visible = true;
            }
            else
            {
                button3.Text = "Imposta Password";
                Impostazioni.pass = "none";
                Impostazioni.timeout_pass = 0;
                Impostazioni.question = "";
                Impostazioni.answer = "";
            }
        }
        
    }
}
