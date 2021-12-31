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
    public class  Visual_FakeTipi : Panel
    {
        public Label Image;
        public Label Tipo;
        public ToolTip tooltip;
        public int index;
        public static int Index = -1;
        private string image_txt;
        private string metodo_txt;

        public void Disposer()
        {
            Tipo.Dispose();
            Image.BackgroundImage.Dispose();
            Image.Dispose();
            tooltip.Dispose();
            Dispose();
        }
        public Visual_FakeTipi(string tipo, string image_txt)
        {
            tooltip = new ToolTip { AutoPopDelay = 20000, };
            DoubleBuffered = true;
            this.image_txt = image_txt;
            this.metodo_txt = tipo;
            Image = new Label()
            {
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            try { Image.BackgroundImage = Funzioni_utili.TakePicture(tipo, 1); } catch (Exception) { }
            Controls.Add(Image);
            Tipo = new Label()
            {
                AutoSize = true,
                Text = tipo,
            };
            Controls.Add(Tipo);
            BorderStyle = BorderStyle.Fixed3D;

            Image.MouseClick += new MouseEventHandler(ClickTipo);
            Tipo.MouseClick += new MouseEventHandler(ClickTipo);
            Image.MouseEnter += new EventHandler(BordoShow);
            Tipo.MouseEnter += new EventHandler(BordoShow);
            MouseEnter += new EventHandler(BordoShow);
            Image.MouseLeave += new EventHandler(BordoHide);
            Tipo.MouseLeave += new EventHandler(BordoHide);
            MouseLeave += new EventHandler(BordoHide);
        }

        public void Aggiorna(string tipo, string image_txt)
        {
            this.image_txt = image_txt;
            this.metodo_txt = tipo;
            Tipo.Text = tipo;
            Image.BackgroundImage = Funzioni_utili.TakePicture(tipo, 2);
        }

        public void SetSize(Size size, int elementi)
        {
            Size = size;
            Image.Location = new Point((int)(Width * 0.05), (int)(Width * 0.1));
            Image.Width = (int)(Width * 0.9);
            double aus = 0; if (elementi == 1) { aus = 0.8; Tipo.Visible = false; } else if (elementi == 2) { aus = 0.7; Tipo.Visible = true; }
            Image.Height = (int)(this.Size.Height * aus);
            Tipo.Location = new Point((int)(Image.Width / 2 - Tipo.Width / 2), Image.Height + Image.Location.Y + 2);
            Tipo.Font = new Font(BackPanel.font1, (int)(Width*0.1), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            if (elementi == 1) { tooltip.SetToolTip(Image, Tipo.Text); tooltip.SetToolTip(Tipo, Tipo.Text); }
        }

        private void ClickTipo(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Program.widget.PannelloFakeTipi.Visible)
                {
                    Program.widget.PannelloFakeTipi.Visible = false;
                    Program.widget.tipo = Tipo.Text;
                    Program.widget.PannelloMetodi.Visible = true;
                }
                else if (Panel_NewEvento.active)
                {
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.tipo = Tipo.Text;
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.FakeTipi_scelta.tipo = Tipo.Text;
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Scelta_tipo_txt.Text = Tipo.Text;
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Scelta_tipo_img.BackgroundImage = Funzioni_utili.TakePicture(Funzioni_utili.GetTipo(Tipo.Text), 2);
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.FakeTipi_scelta.Disposer();
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.scelta = 0;
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.sceltatipo = true;
                }
                else if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato != null)
                {
                    ProprietàGiorno.ScrollToTop(FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello);
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.tipo = Tipo.Text;
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.faketipo_img.BackgroundImage = Funzioni_utili.TakePicture(Tipo.Text, 2);
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.passaggio++;
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Resize_Guidato(true);
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Resize_Guidato(true);
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.Unità.Focus();
                }
            }
        }

        private void BordoShow(object sender, EventArgs e)
        {
            if (Index == index) return;
            Index = index;
            if (Program.widget.PannelloFakeTipi.Visible) foreach (Visual_FakeTipi tip in Program.widget.PannelloFakeTipi.VisualFakeTipi) tip.BordoHide();
            else if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Visible == false) foreach (Visual_FakeTipi tip in FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.FakeTipi_scelta.VisualFakeTipi) tip.BordoHide();
            else foreach (Visual_FakeTipi tip in FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.VisualFakeTipi) tip.BordoHide();
            BorderStyle = BorderStyle.FixedSingle;
            if (Program.widget.PannelloFakeTipi.Visible && Impostazioni.widget_contrasto == false) BackColor = Color.MidnightBlue;
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
            if (Program.widget.PannelloFakeTipi.Visible) BackColor = WidgetMoneyguard.backcolor;
            else BackColor = Color.Transparent;
        }

    }
}
