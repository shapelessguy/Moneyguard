using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moneyguard
{
    public class Widget_PanelMetodi : Panel
    {
        public List<Visual_Metodi> VisualMetodi = new List<Visual_Metodi>();
        private readonly int num_colonne = 3;
        public string metodo;
        public void Disposer()
        {
            foreach (Visual_Metodi tip in VisualMetodi) { tip.Disposer(); Controls.Remove(tip); }
            Dispose();
        }

        public Widget_PanelMetodi()
        {
            BorderStyle = BorderStyle.None;
            BackColor = WidgetMoneyguard.transparent;
            AutoScroll = true;
            Visible = false;
            int i = 0;
            foreach (string metodo in Input.metodi)
            {
                VisualMetodi.Add(new Visual_Metodi(metodo, Associazione.MiconaAssociata(metodo)));
                Controls.Add(VisualMetodi[i]);
                i++;
            }
            MouseEnter += new EventHandler(MouseEntered);
        }
        
        public void ResizeForm()
        {
            Location = new Point(Program.widget.panel1.Location.X, 0);
            Size = Program.widget.PannelloTipi.Size;
            ScrollToTop(this);
            int i = 0, j = 0;
            foreach (Visual_Metodi tip in VisualMetodi)
            {
                tip.Metodo.Visible = false; tip.Image.Size = new Size(tip.Image.Width, tip.Image.Height + tip.Metodo.Height);
                tip.SetSize(new Size((int)(Width / num_colonne - 10), (int)(Width / num_colonne) - 10), 1);
                tip.Location = new Point(tip.Width * i - tip.Width * j * num_colonne, (int)(tip.Height * (j * 1.05)));
                tip.index = i;
                i++;
                if (i % num_colonne == 0) j++;
            }
        }
        private void MouseEntered(object sender, EventArgs e)
        {
            Visual_Metodi.Index = -1;
            foreach (Visual_Metodi tip in VisualMetodi)
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
