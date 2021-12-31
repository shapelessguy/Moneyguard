using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moneyguard
{
    public class Widget_PanelTipi : Panel
    {
        public List<Visual_Tipi> VisualTipi = new List<Visual_Tipi>();
        private readonly int num_colonne = 4;
        public string tipo;
        public void Disposer()
        {
            foreach (Visual_Tipi tip in VisualTipi) { tip.Disposer(); Controls.Remove(tip); }
            Dispose();
        }

        public Widget_PanelTipi()
        {
            BorderStyle = BorderStyle.FixedSingle;
            BackColor = WidgetMoneyguard.transparent;
            AutoScroll = true;
            Visible = false;
            int i = 0;
            foreach (string tipo in Input.tipi)
            {
                VisualTipi.Add(new Visual_Tipi(tipo, Associazione.IconaAssociata(tipo)));
                Controls.Add(VisualTipi[i]);
                i++;
            }
            MouseEnter += new EventHandler(MouseEntered);
        }
        
        public void ResizeForm()
        {
            Location = new Point(Program.widget.panel1.Location.X, 0);
            Size = new Size(Program.widget.panel1.Width - 10, Program.widget.Height - 20);
            ScrollToTop(this);
            int i = 0, j = 0;
            foreach (Visual_Tipi tip in VisualTipi)
            {
                tip.Tipo.Visible = false; tip.Image.Size = new Size(tip.Image.Width, tip.Image.Height + tip.Tipo.Height);
                tip.SetSize(new Size((int)(Width / num_colonne - 5), (int)(Width / num_colonne)), 1);
                tip.Location = new Point(tip.Width * i - tip.Width * j * num_colonne, (int)(tip.Height * (j * 1.05)));
                tip.index = i;
                i++;
                if (i % num_colonne == 0) j++;
            }
        }
        private void MouseEntered(object sender, EventArgs e)
        {
            Visual_Tipi.Index = -1;
            foreach (Visual_Tipi tip in VisualTipi)
            {
                tip.BordoHide();
            }
        }

        public void ScrollToTop(Panel p)
        {
            using (Control c = new Control() { Parent = p, Dock = DockStyle.Top })
            {
                p.ScrollControlIntoView(c);
                c.Parent = null;
            }
        }
    }
}
