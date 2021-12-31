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
    public class Widget_PanelImpostazioni : Panel
    {
        private Label True;
        private readonly Label Size_txt;
        public CheckBox checkbox1;
        public CheckBox checkbox2;
        public TrackBar trackBar;
        public int imp_height = 160;
        public Widget_PanelImpostazioni()
        {
            BackColor = Color.Black;
            BorderStyle = BorderStyle.FixedSingle;
            True = new Label()
            {
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("True"))),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(True);
            True.MouseClick += ClickTrue;

            trackBar = new TrackBar
            {
                Location = new System.Drawing.Point(0, 10),
                Size = new System.Drawing.Size(100, 30)
            };
            trackBar.Scroll += new System.EventHandler(TrackBar_Scroll);
            Controls.Add(trackBar);

            Size_txt = new Label()
            {
                Text = "Taglia",
                ForeColor = Color.White,
                Location = new Point(trackBar.Location.X + trackBar.Width, trackBar.Location.Y),
                Font = new Font("Script MT Bold", 14, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
            };
            Controls.Add(Size_txt);

            checkbox1 = new CheckBox()
            {
                Location = new System.Drawing.Point(10, 60),
                Size = new System.Drawing.Size(Width - 10, 30),
                Text = "Ctrl a destra",
                ForeColor = Color.White,
                Font = new Font("Script MT Bold", 14, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
            };
            Controls.Add(checkbox1);
            checkbox1.CheckedChanged += Checked1;

            checkbox2 = new CheckBox()
            {
                Location = new System.Drawing.Point(10, 90),
                Size = new System.Drawing.Size(Width - 10, 30),
                Text = "Colori chiari",
                ForeColor = Color.White,
                Font = new Font("Script MT Bold", 14, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
            };
            Controls.Add(checkbox2);
            checkbox2.CheckedChanged += Checked2;

            True.MouseEnter += new EventHandler(Enter_True);
            True.MouseLeave += new EventHandler(Leave_True);
        }

        public void ResizeForm()
        {
            Size = new Size((int)(200/3.5*3), imp_height);
            Location = new Point(Program.widget.panel1.Location.X, 100);
            trackBar.Value = Impostazioni.widgetZoom / 10;
            checkbox1.Checked = Impostazioni.controllidx;
            checkbox2.Checked = Impostazioni.widget_contrasto;
            True_Piccolo();

        }
        void Checked1(object sender, EventArgs e)
        {
            Impostazioni.controllidx = checkbox1.Checked;
            Program.widget.SemiResizeForm();
        }
        void Checked2(object sender, EventArgs e)
        {
            Impostazioni.widget_contrasto = checkbox2.Checked;
            Program.widget.SetColors();
        }

        private void PressEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Click_True();
                e.SuppressKeyPress = true;
            }
        }
        private void TrackBar_Scroll(object sender, EventArgs e)
        {
            Impostazioni.widgetZoom = trackBar.Value * 10;
            Program.widget.SemiResizeForm();
            //Program.widget.Size = new Size(Impostazioni.widgetZoom * 5 + 200, Location.Y + Height);
        }
        private void ClickTrue(object sender, EventArgs e)
        {
            Click_True();
        }
        public void Click_True()
        {
            Program.widget.ClickNull();
        }
        

        public void ScrollToBottom(Panel p)
        {
            using (Control c = new Control() { Parent = p, Dock = DockStyle.Bottom })
            {
                p.ScrollControlIntoView(c);
                c.Parent = null;
            }
        }

        private void Enter_True(object sender, EventArgs e)
        {
            True_Grande();
        }
        private void Leave_True(object sender, EventArgs e)
        {
            True_Piccolo();
        }

        private void True_Grande()
        {
            True.Size = new Size(True.Width + 2, True.Height + 2);
            True.Location = new Point(True.Location.X - 1, True.Location.Y - 1);
        }
        private void True_Piccolo()
        {
            True.Size = new Size(20, 20);
            True.Location = new System.Drawing.Point(Width - 40 , Height - 40);
        }
    }
}
