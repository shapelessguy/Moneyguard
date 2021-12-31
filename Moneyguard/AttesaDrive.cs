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
    public partial class AttesaDrive : Form
    {
        Timer timer_in;
        private int ciclo = 0;
        static public bool uscita = false;
        static public bool uscita_alarm = false;
        private bool allowshowdisplay = false;
        public static bool visible = true;
        private static bool button_pressed = false;
        public static bool attesa_closing = false;
        public static int function = 0;
        public static bool on = false;
        public static bool download_images = false;
        public static bool upload_images = false;
        public AttesaDrive()
        {
            function++;
            InitializeComponent();
            timer_in = new Timer()
            {
                Enabled = true,
                Interval = 100,
            };
            timer_in.Tick += Timer;
            FormClosing += Form_Closing;
            label2.BackgroundImage = Moneyguard.Properties.Resources.Verde;
            label2.BackgroundImageLayout = ImageLayout.Stretch;
            ciclo_alarm = -1;
            ciclo = 0;
            if (function > 1)
            {
                label1.Text = "Caricamento su Drive";
                button1.Text = "Esci";
            }
            label2.Location = new Point(40, 35);
        }

        public static int ciclo_alarm = -1;
        private void Timer(object sender, EventArgs e)
        {
            ciclo++;
            if(ciclo_alarm<0) Animazione(ciclo);
            if (ciclo == 1)
            {
                allowshowdisplay = true;
                visible = true;
            }
            if (!visible) Visible = false; else Visible = true;
            if (download_images && function ==1)
            {
                label1.Text = "Scaricamento delle icone";
            }
            if (upload_images && function>1)
            {
                label1.Text = "Caricamento delle icone";
            }
            if (uscita)
            {
                timer_in.Tick -= Timer;
                timer_in.Dispose();
                allowshowdisplay = false;
                Visible = false;
                Close();
                uscita = false;
                //Application.Exit();
            }
            if (uscita_alarm)
            {
                uscita_alarm = false;
                ciclo_alarm = ciclo;
            }
            if (ciclo_alarm>=0)
            {
                label2.Location = new Point(40,35);
                label2.BackgroundImage = Moneyguard.Properties.Resources.False;
                ciclo_alarm++;
                //label2.Hide();
                if (function > 1)
                {
                    label1.Text = "I dati non sono stati salvati correttamente in Google Drive. Riprova ad aprire Moneyguard quando disponi di una connessione internet";
                    button1.Hide();
                    label2.Location = new Point(20, 35);
                    label1.Location = new Point(40, 0);
                    label1.Size = new Size(Size.Width - 40, Size.Height - 30);
                }
                else label1.Text = "Impossibile connettersi";
                if (ciclo_alarm == 10 * 2)
                {
                    ciclo_alarm = -1;
                    uscita = true;
                }
            }
            else
            {
                label2.BackgroundImage = Moneyguard.Properties.Resources.Verde;
                button1.Show();
                label1.Location = new Point(58, 17);
                label1.Size = new Size(321, 58);
                if (function > 1)
                {
                    label1.Text = "Caricamento su Drive";
                    if (upload_images) label1.Text = "Caricamento delle icone";
                }
                else
                {
                    label1.Text = "Connessione a Google Drive";
                    if (download_images) label1.Text = "Scaricamento delle icone";
                }
            }
        }
        protected override void SetVisibleCore(bool value)
        {
            try { base.SetVisibleCore(allowshowdisplay ? value : allowshowdisplay); } catch (Exception) { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button_pressed = true;
            timer_in.Tick -= Timer;
            timer_in = null;
            Close();
            GoogleDrive.connesso = false;
        }

        private void Form_Closing(object sender, EventArgs e)
        {
            if ((button_pressed || uscita) && function == 1) return;
            attesa_closing = true;
        }

        private void Animazione(int t)
        {
            int xi = label2.Location.X, yi = label2.Location.Y;
            double temp = (double)t;
            temp = t / 20;
            xi = (int)(10 * Math.Cos(t)) + 40;
            yi = (int)(10 * Math.Sin(t)) + 35;
            label2.Location = new Point(xi, yi);
        }
    }
}
