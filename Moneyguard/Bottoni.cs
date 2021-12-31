using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moneyguard
{
    public class Bottoni : Label
    {
        public Label guadagno_pic, spesa_pic, trasferimento_pic, note_pic;
        public Label guadagno, spesa;
        public int index;
        public bool festivo;
        public bool attuale;
        public static int index_on = -1;
        public static Color AzzurroAcceso = Color.FromArgb(215, 245, 255);
        public static Color color_attuale = Color.FromArgb(241, 241, 121);
        public static Color color_attuale_acceso = Color.FromArgb(255, 255, 128); 
        public Bottoni()
        {
            BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            TextAlign = System.Drawing.ContentAlignment.TopCenter;
            BackgroundImageLayout = ImageLayout.Stretch;
            MouseClick += Click;
            MouseEnter += Enter;

            guadagno = new System.Windows.Forms.Label
            {
                BackColor = System.Drawing.Color.Transparent,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                AutoSize = true
            };
            spesa = new System.Windows.Forms.Label
            {
                BackColor = System.Drawing.Color.Transparent,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                AutoSize = true
            };
            Controls.Add(guadagno);
            Controls.Add(spesa);

            guadagno_pic = new System.Windows.Forms.Label
            {
                BackColor = System.Drawing.Color.Transparent,
                BackgroundImage = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("Guadagno_giorno"))),
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
            };
            spesa_pic = new System.Windows.Forms.Label
            {
                BackColor = System.Drawing.Color.Transparent,
                BackgroundImage = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("Spesa_giorno"))),
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
            };
            trasferimento_pic = new System.Windows.Forms.Label
            {
                BackColor = System.Drawing.Color.Transparent,
                BackgroundImage = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("trasferimento_giorno"))),
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
            };
            note_pic = new System.Windows.Forms.Label
            {
                BackColor = System.Drawing.Color.Transparent,
                BackgroundImage = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("Note"))),
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
            };
            Controls.Add(guadagno_pic);
            Controls.Add(spesa_pic);
            Controls.Add(trasferimento_pic);
            Controls.Add(note_pic);

            guadagno_pic.MouseClick += Click;
            spesa_pic.MouseClick += Click;
            trasferimento_pic.MouseClick += Click;
            note_pic.MouseClick += Click;
            guadagno.MouseClick += Click;
            spesa.MouseClick += Click;

            guadagno_pic.MouseClick += FinestraPrincipale.BackPanel.ClickNull;
            spesa_pic.MouseClick += FinestraPrincipale.BackPanel.ClickNull;
            trasferimento_pic.MouseClick += FinestraPrincipale.BackPanel.ClickNull;
            note_pic.MouseClick += FinestraPrincipale.BackPanel.ClickNull;
            guadagno.MouseClick += FinestraPrincipale.BackPanel.ClickNull;
            spesa.MouseClick += FinestraPrincipale.BackPanel.ClickNull;
            MouseClick += FinestraPrincipale.BackPanel.ClickNull;
        }

        public void Refresh_Bottoni()
        {
            SuspendLayout();
            guadagno_pic.SuspendLayout();
            spesa_pic.SuspendLayout();
            trasferimento_pic.SuspendLayout();
            note_pic.SuspendLayout();
            if (festivo) BackColor = Color.PowderBlue;
            else BackColor = Color.AliceBlue;
            RefreshColor();
            Size = new System.Drawing.Size((int)(FinestraPrincipale.BackPanel.StandardCalendar.Principale.Width / 7.04), (int)(FinestraPrincipale.BackPanel.StandardCalendar.Principale.Height / 6.24));
            Font = new System.Drawing.Font(BackPanel.font1, (int)(Height * 0.3), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            guadagno_pic.SetBounds(4, (int)(Height / 4 * 2 - 3), (int)(Height / 4), (int)(Height / 4));
            spesa_pic.SetBounds(4, (int)(Height / 4 * 3 - 3), (int)(Height / 4), (int)(Height / 4));
            trasferimento_pic.Size = new Size((int)(Height / 4), (int)(Height / 4));
            trasferimento_pic.Location = new Point(Width - trasferimento_pic.Width * 2, (int)(Height / 4 * 2.5));
            note_pic.Size = new Size((int)(Height / 4), (int)(Height / 4));
            note_pic.Location = new Point(Width - trasferimento_pic.Width * 2, (int)(Height / 4 * 1.2));
            guadagno.Location = new Point((int)(guadagno_pic.Width * 1.2), (int)(Height / 4 * 1.8));
            spesa.Location = new Point((int)(spesa_pic.Width * 1.2), (int)(Height / 4 * 2.8));
            guadagno.Font = new System.Drawing.Font(BackPanel.font1, (int)(Height * 0.15), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            spesa.Font = new System.Drawing.Font(BackPanel.font1, (int)(Height * 0.15), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ResumeLayout(false);
        }
        public void RefreshColor()
        {
            if(!attuale) if(BackColor == color_attuale || BackColor == color_attuale_acceso)
            {
                if (festivo) BackColor = Color.PowderBlue;
                else BackColor = Color.AliceBlue;
            }
            if (attuale) { if(index == index_on) BackColor = color_attuale_acceso; else BackColor = color_attuale; }
        }

        public void SetLabels(string guadagno, string spesa)
        {
            this.guadagno.Text = guadagno;
            this.spesa.Text = spesa;
        }
        private new void Click(object sender, MouseEventArgs e)
        {
            if (FinestraPrincipale.BackPanel.altriconti) return;
            if (e.Button == MouseButtons.Left)
            {
                Pannello_StandardCalendar.button_clicked = index;
                FinestraPrincipale.BackPanel.StandardCalendar.giorno = index + 1 - Pannello_StandardCalendar.posizione_iniziale;
                FinestraPrincipale.BackPanel.StandardCalendar.Click();
            }
        }
        private new void Enter(object sender, EventArgs e)
        {
            index_on = index;
            Pannello_StandardCalendar.wait = false;
            Selezione();
        }
        public static void Selezione()
        {
            foreach (Bottoni button in FinestraPrincipale.BackPanel.StandardCalendar.button)
            {
                if (button.index != index_on)
                {
                    if (button.attuale) button.BackColor = color_attuale; 
                    else if (button.festivo) button.BackColor = Color.PowderBlue;
                    else button.BackColor = Color.AliceBlue;
                }
                else {
                    index_on = button.index;
                    if (button.attuale) button.BackColor = color_attuale_acceso;
                    else if (button.festivo) button.BackColor = Color.PaleTurquoise;
                    else button.BackColor = AzzurroAcceso;
                }
            }
        }
        
    }
}
