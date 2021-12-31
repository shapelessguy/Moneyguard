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
    public class VisualAltriConti : Panel
    {
        public Label picture, text_tipo;
        public string metodo;
        public double valore;
        ToolTip tooltip;
        private readonly bool totale = false;
        public void Disposer()
        {
            picture.BackgroundImage.Dispose();
            picture.Dispose();
            text_tipo.Dispose();
            tooltip.Dispose();
            Dispose();
        }

        public VisualAltriConti(bool totale)
        {
            DoubleBuffered = true;
            this.totale = totale;
            tooltip = new ToolTip() { AutoPopDelay = 20000,};
            BorderStyle = BorderStyle.None;
            picture = new Label()
            {
                BackColor = System.Drawing.Color.Transparent,
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
            };
            Controls.Add(picture);
            text_tipo = new Label()
            {
                BorderStyle = BorderStyle.None,
                TextAlign = ContentAlignment.MiddleLeft,
            };
            Controls.Add(text_tipo);
        }

        public void RefreshForm()
        {
            if (totale)
            {
                picture.BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("Tesoro")));
                valore = 0;
                foreach (double val in Input.totali) valore += val;
                text_tipo.Text = "Totale: " + Funzioni_utili.FormatoStandard(valore) + "\u20AC";
            }
            else
            {
                tooltip.SetToolTip(picture, metodo); tooltip.SetToolTip(text_tipo, metodo);
                picture.BackgroundImage = Funzioni_utili.TakePicture(metodo, 2);
                for (int i = 0; i < Input.metodi.Count; i++) if (Input.metodi[i] == metodo) valore = Input.totali[i];
                text_tipo.Text = "    " + Funzioni_utili.FormatoStandard(valore) + "\u20AC";
            }
            ResizeForm();
        }

        public void ResizeForm()
        {
            if(FinestraPrincipale.BackPanel.AltriConti != null) Size = new Size((int)(FinestraPrincipale.BackPanel.AltriConti.Pannello.Width * 0.9), (int)(FinestraPrincipale.BackPanel.AltriConti.Pannello.Height * 0.22));
            picture.Size = new Size(Height, Height);
            picture.Location = new Point((int)(Width * 0.2), 0);
            text_tipo.Size = new Size(Width - picture.Location.X - picture.Width - 10, Height);
            text_tipo.Location = new Point(picture.Location.X + picture.Width + 10, 0);
            if(totale) text_tipo.Font = new Font(BackPanel.font1, (int)(Width * 0.03 + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            else text_tipo.Font = new Font(BackPanel.font1, (int)(Width * 0.03 + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }
    }
}
