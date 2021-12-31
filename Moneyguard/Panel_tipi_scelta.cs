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
    public class Panel_tipi_scelta : Panel
    {
        public List<Visual_Tipi> VisualTipi = new List<Visual_Tipi>();
        private readonly int num_colonne = 8;
        public string tipo;
        public void Disposer()
        {
            foreach (Visual_Tipi tip in VisualTipi) { tip.Disposer(); Controls.Remove(tip); }
            Dispose();
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
        }

        public Panel_tipi_scelta()
        {
            DoubleBuffered = true;
            BorderStyle = BorderStyle.FixedSingle;
            AutoScroll = true;
            int i = 0;
            foreach (int it in Input.tipi_sort)
            {
                VisualTipi.Add(new Visual_Tipi(Input.tipi[it], Associazione.IconaAssociata(Input.tipi[it])));
                Controls.Add(VisualTipi[i]);
                i++;
            }
            MouseEnter += new EventHandler(MouseEntered);
        }

        public void SetLocationY(int y)
        {
            Location = new Point(FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Scelta_tipo.Location.X - 2, y);
            Update();
        }
        public void ResizeForm()
        {
            Size = new Size((int)((FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Width - Location.X) * 0.98), FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Height - Location.Y - 20);
            ProprietàGiorno.ScrollToTop(this);
            int i = 0, j = 0;
            foreach (Visual_Tipi tip in VisualTipi)
            {
                //tip.Tipo.Visible = false;
                tip.Image.Size = new Size(tip.Image.Width, tip.Image.Height + tip.Tipo.Height);
                tip.SetSize(new Size((int)(Width/num_colonne - 5), (int)(Width/num_colonne)), 1);
                tip.Location = new Point(tip.Width * i - tip.Width * j * num_colonne, (int)(tip.Height * (j*1.05)));
                tip.index = i;
                i++;
                if (i % num_colonne == 0) j++;
            }
            Visible = true;
        }

        private void MouseEntered(object sender, EventArgs e)
        {
            Visual_Tipi.Index = -1;
            foreach(Visual_Tipi tip in VisualTipi)
            {
                tip.BordoHide();
            }
        }
    }
}
