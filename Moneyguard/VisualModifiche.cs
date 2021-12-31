using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moneyguard
{
    public class VisualModifiche : Panel
    {
        public Label Image;
        public Label Tipo;
        public int index;
        public static int Index = -1;
        private readonly string image_txt;
        public string resource;
        public int tipo_metodo;
        public int resources_file;
        static public string tipostatico;
        static public int tipo_metodostatico;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem eliminaToolStripMenuItem;
        private Timer timer_tip, timer_met;

        public void Disposer()
        {
            Tipo.Dispose();
            timer_tip.Dispose();
            timer_met.Dispose();
            if (Image.BackgroundImage!= null) Image.BackgroundImage.Dispose();
            if(Image!= null) Image.Dispose();
            Dispose();
        }
        public VisualModifiche(string tipo, string image_txt, int resources_file, int tipo_metodo)
        {
            DoubleBuffered = true;
            this.resources_file = resources_file;
            this.tipo_metodo = tipo_metodo;
            Index = -1;
            this.image_txt = image_txt;
            Image = new Label()
            {
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            try
            {
                if (resources_file == 1) Image.BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject(image_txt)));
                else if (resources_file == 2)
                {
                    StreamReader streamReader;

                    if (tipo_metodo == 1) { streamReader = new StreamReader(Input.path + @"Icons\Tipologie\" + image_txt + ".png"); Image.BackgroundImage = (Bitmap)Bitmap.FromStream(streamReader.BaseStream); streamReader.Close(); } //Image.BackgroundImage = Bitmap.FromFile(Input.path + @"Icons\Tipologie\" + image_txt + ".png");
                    if (tipo_metodo == 2) { streamReader = new StreamReader(Input.path + @"Icons\Metodi\" + image_txt + ".png"); Image.BackgroundImage = (Bitmap)Bitmap.FromStream(streamReader.BaseStream); streamReader.Close(); }//Image.BackgroundImage = Bitmap.FromFile(Input.path + @"Icons\Metodi\" + image_txt + ".png");
                }
            }
            catch (Exception) { Image.BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("no_icon"))); Console.WriteLine("Errore noIcon"); }
            Controls.Add(Image);
            Tipo = new Label()
            {
                AutoSize = true,
                Text = tipo,
            };
            Controls.Add(Tipo);
            BorderStyle = BorderStyle.Fixed3D;

            if (resources_file == 2)
            {
                contextMenuStrip1 = new ContextMenuStrip
                {
                    ImageScalingSize = new System.Drawing.Size(24, 24),
                    Name = "contextMenuStrip1",
                    Size = new System.Drawing.Size(141, 34)
                };
                eliminaToolStripMenuItem = new ToolStripMenuItem
                {
                    Name = "eliminaToolStripMenuItem",
                    Size = new System.Drawing.Size(140, 30),
                    Text = "Elimina"
                };
                contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { eliminaToolStripMenuItem });
                eliminaToolStripMenuItem.Click += new System.EventHandler(EliminaToolStripMenuItem_Click);
                ContextMenuStrip = contextMenuStrip1;
            }
            timer_tip = new Timer()
            {
                Enabled = true,
                Interval = 1000,
            };
            timer_met = new Timer()
            {
                Enabled = true,
                Interval = 1000,
            };

            Image.MouseClick += new MouseEventHandler(ClickTipo);
            Tipo.MouseClick += new MouseEventHandler(ClickTipo);
            Image.MouseEnter += new EventHandler(BordoShow);
            Tipo.MouseEnter += new EventHandler(BordoShow);
            MouseEnter += new EventHandler(BordoShow);
            Image.MouseLeave += new EventHandler(BordoHide);
            Tipo.MouseLeave += new EventHandler(BordoHide);
            MouseLeave += new EventHandler(BordoHide);
            VisibleChanged += (o, e) => { if (!Visible) save = true; };
        }

        public void SetSize(Size size, int elementi)
        {
            Size = size;
            Image.Location = new Point(0, 0);
            Image.Width = this.Width;
            double aus = 0; if (elementi == 1) { aus = 1; Tipo.Visible = false; } else if (elementi == 2) aus = 0.8;
            Image.Height = (int)(this.Size.Height * aus);
            Tipo.Location = new Point((int)(Image.Location.X + Image.Width / 2 - Tipo.Width / 2), (int)(Image.Height));
        }

        int ciclotip = 0;
        void Timer_tip(object sender, EventArgs e)
        {
            ciclotip++;
            if (ciclotip == 6) { timer_met.Tick -= Timer_met; ciclotip = 0; MessageBox.Show("Errore in fase di eliminazione del file"); return; }
            try
            {
                Program.widget.elimina_tip = Tipo.Text;
                File.Delete(Input.path + @"Icons\Tipologie\" + resource + ".png");
                Dispose();
                timer_tip.Tick -= Timer_tip;
                ciclotip = 0;
                return;
            }
            catch (Exception) { return; }
        }
        int ciclo = 0;
        void Timer_met(object sender, EventArgs e)
        {
            ciclo++;
            if (ciclo == 6) { timer_met.Tick -= Timer_met; ciclo = 0; MessageBox.Show("Errore in fase di eliminazione del file"); return; }
            try
            {
                Program.widget.elimina_met = Tipo.Text;
                File.Delete(Input.path + @"Icons\Metodi\" + resource + ".png");
                Dispose();
                timer_met.Tick -= Timer_met;
                ciclo = 0;
                return;
            }
            catch (Exception) { return; }
        }
        public static bool save = true;
        private void ClickTipo(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Etichette.fakestatico)
                {
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.FakeEtichetta.picture.BackgroundImage = Image.BackgroundImage;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.FakeEtichetta.resource = resource;
                }
                else if (tipo_metodostatico == 1)
                {
                    for (int i = 0; i < Input.tipi.Count; i++) { if (Input.tipi[i] == tipostatico) Input.tipi_icons[i] = resource; }
                }
                else if (tipo_metodostatico == 2)
                {
                    for (int i = 0; i < Input.metodi.Count; i++) { if (Input.metodi[i] == tipostatico) Input.metodi_icons[i] = resource; }
                }
                FinestraPrincipale.BackPanel.Panel_Impostazioni.FakeEtichetta.timerenter.Tick += FinestraPrincipale.BackPanel.Panel_Impostazioni.FakeEtichetta.CheckTrue;
                FinestraPrincipale.BackPanel.Panel_Impostazioni.FakeEtichetta.image = true;
                if (save) FinestraPrincipale.BackPanel.Panel_Impostazioni.RefreshIcons();
                Etichette.disabilitaclick = false;
                FinestraPrincipale.BackPanel.Panel_Impostazioni.PanelMetodi.Visible = false;
                FinestraPrincipale.BackPanel.Panel_Impostazioni.PanelTipi.Visible = false;
                FinestraPrincipale.BackPanel.Panel_Impostazioni.Update();
                if(save) Savings.SaveEvents();
                save = true;
            }
        }

        private void EliminaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tipo_metodo == 1)
            {
                Savings.icons_changed = true;
                Image.BackgroundImage.Dispose();
                for (int i=0; i< Input.tipi_icons.Count; i++) if (Input.tipi_icons[i] == resource) Input.tipi_icons[i] = "no_icon";
                Savings.SaveEvents();
                FinestraPrincipale.BackPanel.Panel_Impostazioni.RefreshIcons();
                FinestraPrincipale.BackPanel.Panel_Impostazioni.PanelTipi.Controls.Remove(this);
                FinestraPrincipale.BackPanel.Panel_Impostazioni.PanelTipi.VisualTipi.Remove(this);
                FinestraPrincipale.BackPanel.Panel_Impostazioni.PanelTipi.ResizeForm();
                timer_tip.Tick += Timer_tip;
                //File.Delete(Input.path + @"Icons\Tipologie\" + resource + ".png");
            }
            if (tipo_metodo == 2)
            {
                Savings.icons_changed = true;
                Image.BackgroundImage.Dispose();
                for (int i = 0; i < Input.metodi_icons.Count; i++) if (Input.metodi_icons[i] == resource) Input.metodi_icons[i] = "no_icon";
                Savings.SaveEvents();
                FinestraPrincipale.BackPanel.Panel_Impostazioni.RefreshIcons();
                FinestraPrincipale.BackPanel.Panel_Impostazioni.PanelMetodi.Controls.Remove(this);
                FinestraPrincipale.BackPanel.Panel_Impostazioni.PanelMetodi.VisualTipi.Remove(this);
                FinestraPrincipale.BackPanel.Panel_Impostazioni.PanelMetodi.ResizeForm();
                timer_met.Tick += Timer_met;
                //File.Delete(Input.path + @"Icons\Metodi\" + resource + ".png");
            }
            WidgetMoneyguard.refreshpanel = true;
        }

        private void BordoShow(object sender, EventArgs e)
        {
            if (Index == index) return;
            Index = index;
            if (FinestraPrincipale.BackPanel.Panel_Impostazioni.PanelTipi.Visible == true || FinestraPrincipale.BackPanel.Panel_Impostazioni.PanelMetodi.Visible == true)
            {
                foreach (VisualModifiche tip in FinestraPrincipale.BackPanel.Panel_Impostazioni.PanelTipi.VisualTipi) tip.BordoHide();
                foreach (VisualModifiche tip in FinestraPrincipale.BackPanel.Panel_Impostazioni.PanelMetodi.VisualTipi) tip.BordoHide();
            }
            BorderStyle = BorderStyle.FixedSingle;
            BackColor = Color.LightCyan;
            BordoHide();
        }
        private void BordoHide(object sender, EventArgs e)
        {
            BordoHide();
        }
        public void BordoHide()
        {
            if (Index == index) return;
            BorderStyle = BorderStyle.Fixed3D;
            if(resources_file==1) BackColor = Color.White;
            else BackColor = Color.Transparent;
        }
        
    }
}
