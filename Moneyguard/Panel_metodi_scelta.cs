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
    public class Panel_metodi_scelta : Panel
    {
        public List<Visual_Metodi> VisualMetodi = new List<Visual_Metodi>();
        private readonly int num_colonne = 3;
        public string metodo;
        public void Disposer()
        {
            foreach (Visual_Metodi tip in VisualMetodi) { tip.Disposer(); Controls.Remove(tip); }
            Dispose();
        }

        public Panel_metodi_scelta()
        {
            DoubleBuffered = true;
            BorderStyle = BorderStyle.FixedSingle;
            AutoScroll = true;
            int i = 0;
            foreach (int it in Input.metodi_sort)
            {
                VisualMetodi.Add(new Visual_Metodi(Input.metodi[it], Associazione.MiconaAssociata(Input.metodi[it])));
                Controls.Add(VisualMetodi[i]);
                i++;
            }
            MouseEnter += new EventHandler(MouseEntered);
        }

        public void SetLocationY(int y)
        {
            Location = new Point(FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Scelta_tipo.Location.X - 2, y);
        }
        public void ResizeForm()
        {
            Size = new Size((int)((FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Width - Location.X - Location.X) * 0.6), FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Height - Location.Y - 20);
            ProprietàGiorno.ScrollToTop(this);
            int i = 0, j = 0;
            foreach (Visual_Metodi tip in VisualMetodi)
            {
                //tip.Metodo.Visible = false;
                tip.Image.Size = new Size(tip.Image.Width, tip.Image.Height + tip.Metodo.Height);
                tip.SetSize(new Size((int)(Width / num_colonne - 10), (int)(Width / num_colonne) - 10), 2);
                tip.Location = new Point(tip.Width * i - tip.Width * j * num_colonne, (int)(tip.Height * (j * 1.05)));
                tip.index = i;
                i++;
                if (i % num_colonne == 0) j++;
            }
            Visible = true;
        }
        private void MouseEntered(object sender, EventArgs e)
        {
            Visual_Metodi.Index = -1;
            foreach (Visual_Metodi tip in VisualMetodi)
            {
                tip.BordoHide();
            }
        }

    }
}
