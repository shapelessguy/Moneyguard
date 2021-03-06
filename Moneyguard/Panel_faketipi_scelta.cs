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
    public class Panel_faketipi_scelta : Panel
    {
        public List<Visual_FakeTipi> VisualFakeTipi = new List<Visual_FakeTipi>();
        private readonly int num_colonne = 3;
        public string tipo;
        public void Disposer()
        {
            foreach (Visual_FakeTipi tip in VisualFakeTipi) { tip.Disposer(); Controls.Remove(tip); }
            Dispose();
        }

        public Panel_faketipi_scelta()
        {
            DoubleBuffered = true;
            BorderStyle = BorderStyle.FixedSingle;
            AutoScroll = true;
            Visible = false;
            int i = 0;
            foreach (string tipo in Input.metodi)
            {
                VisualFakeTipi.Add(new Visual_FakeTipi(tipo, Associazione.MiconaAssociata(tipo)));
                Controls.Add(VisualFakeTipi[i]);
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
            foreach (Visual_FakeTipi tip in VisualFakeTipi)
            {
                tip.Tipo.Visible = false; tip.Image.Size = new Size(tip.Image.Width, tip.Image.Height + tip.Tipo.Height);
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
            Visual_FakeTipi.Index = -1;
            foreach (Visual_FakeTipi tip in VisualFakeTipi)
            {
                tip.BordoHide();
            }
        }
    }
}
