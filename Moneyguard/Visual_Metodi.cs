using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1;

namespace Moneyguard
{
    public class Visual_Metodi : Panel
    {
        public Label Image;
        public Label Metodo;
        public ToolTip tooltip;
        public int index;
        public static int Index = -1;
        private string image_txt;
        private string metodo_txt;

        public void Disposer()
        {
            Metodo.Dispose();
            Image.BackgroundImage.Dispose();
            Image.Dispose();
            tooltip.Dispose();
            Dispose();
        }
        public Visual_Metodi(string tipo, string image_txt)
        {
            tooltip = new ToolTip { AutoPopDelay = 20000, };
            DoubleBuffered = true;
            this.image_txt = image_txt;
            this.metodo_txt = tipo;
            Image = new Label()
            {
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            try { Image.BackgroundImage = Funzioni_utili.TakePicture(tipo, 2); } catch (Exception) { }
            Controls.Add(Image);
            Metodo = new Label()
            {
                AutoSize = true,
                Text = tipo,
            };
            Controls.Add(Metodo);
            BorderStyle = BorderStyle.Fixed3D;

            Image.MouseClick += new MouseEventHandler(ClickTipo);
            Metodo.MouseClick += new MouseEventHandler(ClickTipo);
            Image.MouseEnter += new EventHandler(BordoShow);
            Metodo.MouseEnter += new EventHandler(BordoShow);
            MouseEnter += new EventHandler(BordoShow);
            Image.MouseLeave += new EventHandler(BordoHide);
            Metodo.MouseLeave += new EventHandler(BordoHide);
            MouseLeave += new EventHandler(BordoHide);
        }
        public void Aggiorna(string metodo, string image_txt)
        {
            this.image_txt = image_txt;
            this.metodo_txt = metodo;
            Metodo.Text = metodo;
            Image.BackgroundImage = Funzioni_utili.TakePicture(metodo, 2);
        }

        public void SetSize(Size size, int elementi)
        {
            Size = size;
            Image.Location = new Point((int)(Width * 0.05), (int)(Width * 0.1));
            Image.Width = (int)(Width * 0.9);
            double aus = 0; if (elementi == 1) { aus = 0.8; Metodo.Visible = false; } else if (elementi == 2) { aus = 0.7; Metodo.Visible = true; }
            Image.Height = (int)(this.Size.Height * aus);
            Metodo.Location = new Point((int)(Image.Width / 2 - Metodo.Width / 2), Image.Height + Image.Location.Y + 2);
            Metodo.Font = new Font(BackPanel.font1, (int)(Width * 0.1), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            if (elementi == 1) { tooltip.SetToolTip(Image, Metodo.Text); tooltip.SetToolTip(Metodo, Metodo.Text); }
        }

        private void ClickTipo(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Program.widget.PannelloMetodi.Visible)
                {
                    Input.LoadAttributi();
                    Program.widget.PannelloFinale.SetTime();
                    Program.widget.PannelloMetodi.Visible = false;
                    Program.widget.metodo = Metodo.Text;
                    Program.widget.PannelloFinale.Visible = true;
                    Program.widget.PannelloFinale.Unità.Focus();
                }
                else if (Panel_NewEvento.active)
                {
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.metodo = Metodo.Text;
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Metodi_scelta.metodo = Metodo.Text;
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Scelta_metodo_txt.Text = Metodo.Text;
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Scelta_metodo_img.BackgroundImage = Funzioni_utili.TakePicture(Associazione.MetodoAssociato(image_txt), 2);
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Metodi_scelta.Disposer();
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.sceltametodo = true;
                }
                else if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato != null)
                {
                    ProprietàGiorno.ScrollToTop(FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello);
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.metodo = Metodo.Text;
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.metodo_img.BackgroundImage = Funzioni_utili.TakePicture(Funzioni_utili.GetTipo(Metodo.Text), 2);
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.passaggio++;
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Resize_Guidato(false);
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Resize_Guidato(false);
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.Unità.Focus();
                }
            }
        }
        private void BordoShow(object sender, EventArgs e)
        {
            if (Index == index) return;
            Index = index;
            if (Program.widget.PannelloMetodi.Visible) foreach (Visual_Metodi tip in Program.widget.PannelloMetodi.VisualMetodi) tip.BordoHide();
            else if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Visible == false) foreach (Visual_Metodi tip in FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Metodi_scelta.VisualMetodi) tip.BordoHide();
            else foreach (Visual_Metodi tip in FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.VisualMetodi) tip.BordoHide();
            BorderStyle = BorderStyle.FixedSingle;
            if (Program.widget.PannelloMetodi.Visible && Impostazioni.widget_contrasto == false) BackColor = Color.MidnightBlue;
            else BackColor = Color.LightCyan;
        }
        private void BordoHide(object sender, EventArgs e)
        {
            BordoHide();
        }
        public void BordoHide()
        {
            if (Index == index) return;
            BorderStyle = BorderStyle.Fixed3D;
            if (Program.widget.PannelloMetodi.Visible) BackColor = WidgetMoneyguard.backcolor;
            else BackColor = Color.Transparent;
        }
    }
}
