using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moneyguard
{
    public class Panel_Guidato : Panel
    {
        private Panel Barra_Strumenti;
        public Label Tipo, Metodo;
        public Label attributo_img;
        public Label faketipo_img;
        public Label metodo_img;
        public Panello_Guidato_scelta Pannello;
        private Label Indietro_lab;
        public Panel_Guidato()
        {
            //DoubleBuffered = true;
            Visible = false;
            Barra_Strumenti = new Panel()
            {
                BackColor = Color.AliceBlue,
                BorderStyle = BorderStyle.Fixed3D,
            };
            Controls.Add(Barra_Strumenti);
            Tipo = new Label()
            {
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft,
                AutoSize = true,
            };
            Barra_Strumenti.Controls.Add(Tipo);
            Metodo = new Label()
            {
                TextAlign = System.Drawing.ContentAlignment.MiddleRight,
                AutoSize = true,
            };
            Barra_Strumenti.Controls.Add(Metodo);
            attributo_img = new Label()
            {
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Barra_Strumenti.Controls.Add(attributo_img);
            metodo_img = new Label()
            {
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Barra_Strumenti.Controls.Add(metodo_img);
            faketipo_img = new Label()
            {
                BackgroundImageLayout = ImageLayout.Stretch,
                //Visible = false,
            };
            Barra_Strumenti.Controls.Add(faketipo_img);
            Indietro_lab = new Label()
            {
                BackgroundImageLayout = ImageLayout.Stretch,
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("left_Arrow"))),
            };
            Barra_Strumenti.Controls.Add(Indietro_lab);
            Indietro_lab.MouseEnter += new EventHandler(Enter_Indietro);
            Indietro_lab.MouseLeave += new EventHandler(Leave_Indietro);
            Indietro_lab.MouseClick += new MouseEventHandler(Indietro);
            Pannello = new Panello_Guidato_scelta();
            Controls.Add(Pannello);
            
        }
        public void Disposer()
        {
            Dispose();
            Pannello.Disposer();
            Tipo.Dispose();
            Metodo.Dispose();
            Indietro_lab.BackgroundImage.Dispose();
            Indietro_lab.Dispose();
            if (attributo_img.BackgroundImage != null) attributo_img.BackgroundImage.Dispose();
            if (metodo_img.BackgroundImage != null) metodo_img.BackgroundImage.Dispose();
            if (faketipo_img.BackgroundImage != null) faketipo_img.BackgroundImage.Dispose();
            attributo_img.Dispose();
            metodo_img.Dispose();
            faketipo_img.Dispose();
        }

        public void Resize_Guidato(bool initial)
        {
            Barra_Strumenti.Location = new Point(0, 0);
            Barra_Strumenti.Size = new Size(Width, (int)(Height * 0.1));
            Pannello.Location = new Point(0, (int)(Barra_Strumenti.Height * 0.9));
            Pannello.Size = new Size(Width, (int)(Height * 0.9));
            int size; if (Pannello.attributo == "Trasferimento") size = 2; else size = 1;
            Indietro_Piccolo();
            if(Pannello.attributo == "Trasferimento") attributo_img.Location = new Point((int)(Width - attributo_img.Width)/2, 2); else attributo_img.Location = new Point((int)(Indietro_lab.Width * 1.5), 2);
            attributo_img.Size = new Size(((int)(Barra_Strumenti.Height * 0.9) - 2 * attributo_img.Location.Y)*size, (int)(Barra_Strumenti.Height * 0.9) - 2 * attributo_img.Location.Y);
            if (metodo_img != null) { metodo_img.Size = new Size((int)(Barra_Strumenti.Height * 0.9) - 2 * attributo_img.Location.Y, Barra_Strumenti.Height - 2 * attributo_img.Location.Y - 3);}
            Metodo.Font = new Font(BackPanel.font1, (int)(Width * 0.03 + 5), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Tipo.Font = new Font(BackPanel.font1, (int)(Width * 0.026 + 5), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Tipo.Location = new Point(attributo_img.Location.X + attributo_img.Width, (int)((Barra_Strumenti.Height * 0.9) - Tipo.Height)/2);
            Metodo.Location = new Point(Tipo.Location.X + Tipo.Width, (int)((Barra_Strumenti.Height*0.9) - Metodo.Height) / 2);
            if (metodo_img != null) { metodo_img.Location = new Point(Metodo.Location.X + Metodo.Width, 2); }
            if (Pannello.attributo == "Trasferimento")
            {
                faketipo_img.Size = new Size((int)(Barra_Strumenti.Height) - 2 * attributo_img.Location.Y, Barra_Strumenti.Height - 2 * attributo_img.Location.Y - 3);
                faketipo_img.Location = new Point(attributo_img.Location.X - attributo_img.Width, 2); faketipo_img.Visible = true;
            }
            else faketipo_img.Visible = false;
            Pannello.ResizeForm(initial);
            Pannello.Update();
        }
        public void Close()
        {
           // foreach (var panel in FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale) panel.delete_opaco.Visible = true;
            Pannello.passaggio = 1;
            FinestraPrincipale.BackPanel.Panel_Giorno.pulsanti_selectable = true;
            foreach (Control control in FinestraPrincipale.BackPanel.Panel_Giorno.Controls)
            {
                if (control != FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel && 
                    control != FinestraPrincipale.BackPanel.Panel_Giorno.Attributo_txt &&
                    control != FinestraPrincipale.BackPanel.Panel_Giorno.Go_Calendar_txt &&
                    control != FinestraPrincipale.BackPanel.Panel_Giorno.PanelMotore) control.Visible = true;
            }
            attributo_img.BackgroundImage = null;
            metodo_img.BackgroundImage = null;
            Visible = false;
            FinestraPrincipale.BackPanel.Focus();
        }
        public void Indietro(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Pannello.ClickNull(sender, e);
                Pannello.passaggio--;
                Pannello.ResizeForm(true);
            }
        }


        private void Enter_Indietro(object sender, EventArgs e)
        {
            Indietro_Grande();
        }
        private void Leave_Indietro(object sender, EventArgs e)
        {
            Indietro_Piccolo();
        }
        private void Indietro_Grande()
        {
            Indietro_lab.Location = new Point(Indietro_lab.Location.X - 2, Indietro_lab.Location.Y - 2);
            Indietro_lab.Size = new Size(Indietro_lab.Width + 4, Indietro_lab.Height + 4);
        }
        private void Indietro_Piccolo()
        {
            Indietro_lab.Location = new Point(5, 4);
            Indietro_lab.Size = new Size(((int)(Barra_Strumenti.Height * 0.8) - 2 * attributo_img.Location.Y), (int)(Barra_Strumenti.Height * 0.8) - 2 * attributo_img.Location.Y);
        }

    }

}
