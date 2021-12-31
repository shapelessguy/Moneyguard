using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1;

namespace Moneyguard
{
    public class Impostazioni : Panel
    {
        static public string[] readText;

        static public string pass;
        static public int timeout_pass;

        static public string question;
        static public string answer;
        static public bool aut_sync;
        static public Point size;
        static public Point location;
        static public bool close_inApplicationsBar;
        static public bool show_calendar_when_opened;
        static public bool widget_visible;
        static public bool mute;
        static public bool show_maximized;
        static public int widgetZoom;
        static public bool controllidx;
        static public bool widget_contrasto;
        static public Point ora_minuto;

        //Sounds
        static public readonly Stream Yes = Properties.Resources.introito;
        static public System.Media.SoundPlayer yes_snd = new System.Media.SoundPlayer(Yes);
        static public readonly Stream No = Properties.Resources.spesa;
        static public System.Media.SoundPlayer no_snd = new System.Media.SoundPlayer(No);
        static public readonly Stream Gnè = Properties.Resources.trasferimento1;
        static public System.Media.SoundPlayer gnè_snd = new System.Media.SoundPlayer(Gnè);

        static public bool active = false;

        public PanelModificheTipi PanelTipi;
        public PanelModificheTipi PanelMetodi;
        public Panel_attributo_scelta PanelDelete;
        public Label Icons_tip;
        public Label Icons_met;
        public Label Icons_txt;
        public Label Go_Calendar;
        public Label Go_Calendar_txt;
        public Label NewTipo;
        public Label NewTipo_txt;
        public Label NewMetodo;
        public Label NewMetodo_txt;
        public Label True;
        public Etichette FakeEtichetta;

        private readonly int dim_minima = 150;
        private string filter = "All Files|*.*|Bitmap Image (.bmp)|*.bmp|Gif Image (.gif)|*.gif|JPG Image (.jpg)|*.jpeg|JPEG Image (.jpeg)|*.jpeg|Png Image (.png)|*.png|Tiff Image (.tiff)|*.tiff|Wmf Image (.wmf)|*.wmf";
        private readonly string errore = "Quasi nulla è impossibile, ma importare questa immagine lo è!";

        private Label RiferimentoTipi;
        private Label RiferimentoMetodi;

        public static int tipo_metodo_delete = 0;
        public static string tipo_delete;

        public Panel Tipi;
        public Panel Metodi;

        private Label Tipi_txt;
        private Label Metodi_txt;

        public List<Etichette> Tipi_list = new List<Etichette>();
        public List<Etichette> Metodi_list = new List<Etichette>();



        public void Disposer()
        {
            if(Etichette.to_save) Savings.SaveEvents();
            FinestraPrincipale.BackPanel.Controls.Remove(this);
            FinestraPrincipale.BackPanel.Ora.Show();
            PanelTipi.Disposer();
            PanelMetodi.Disposer();
            if(PanelDelete != null) PanelDelete.Disposer();
            Icons_tip.BackgroundImage.Dispose();
            Icons_tip.Dispose();
            Icons_met.BackgroundImage.Dispose();
            Icons_met.Dispose();
            Icons_txt.Dispose();
            Go_Calendar.BackgroundImage.Dispose();
            Go_Calendar.Dispose();
            Go_Calendar_txt.Dispose();
            NewTipo.BackgroundImage.Dispose();
            NewTipo.Dispose();
            NewTipo_txt.Dispose();
            NewMetodo.BackgroundImage.Dispose();
            NewMetodo.Dispose();
            NewMetodo_txt.Dispose();

            FakeEtichetta.Disposer();
            foreach (Etichette etichetta in Tipi_list) etichetta.Disposer();
            foreach (Etichette etichetta in Metodi_list) etichetta.Disposer();
            Input.RefreshImages();
            Tipi_list.Clear();
            Metodi_list.Clear();
            Dispose();
            BackPanel.go_to_impostazioni = false;
            active = false;
        }
        public Impostazioni()
        {
            if (FinestraPrincipale.BackPanel.Panel_Ricerca != null) FinestraPrincipale.BackPanel.Panel_Ricerca.CloseRicerca();
            FinestraPrincipale.BackPanel.Portafogli.Hide();
            FinestraPrincipale.BackPanel.Cassaforte.Hide();
            FinestraPrincipale.BackPanel.Banca_Pic.Hide();
            FinestraPrincipale.BackPanel.Calc_Pic.Hide();
            FinestraPrincipale.BackPanel.Portafogli_Pic.Hide();
            FinestraPrincipale.BackPanel.Cassaforte_Pic.Hide();
            FinestraPrincipale.BackPanel.Ora.Hide();
            SuspendLayout();
            ResumeLayout(false);
            FinestraPrincipale.BackPanel.StandardCalendar.Visible = false;
            active = true;
            FinestraPrincipale.BackPanel.Controls.Add(this);

            Size = FinestraPrincipale.BackPanel.Size;
            Tipi = new Panel()
            {
                AutoScroll=true,
            };
            Metodi = new Panel()
            {
                AutoScroll = true,
            };
            PanelTipi = new PanelModificheTipi(1);
            PanelMetodi = new PanelModificheTipi(2);

            RiferimentoTipi = new Label();
            Tipi.Controls.Add(RiferimentoTipi);
            RiferimentoMetodi = new Label();
            Metodi.Controls.Add(RiferimentoMetodi);
            RiferimentoTipi.Size = new Size(1, 1);
            RiferimentoMetodi.Size = new Size(1, 1);

            Tipi_txt = new Label()
            {
                AutoSize = true,
                Text = "Tipologie",
            };
            Controls.Add(Tipi_txt);
            Metodi_txt = new Label()
            {
                AutoSize = true,
                Text = "Metodi",
            };
            Controls.Add(Metodi_txt);

            FakeEtichetta = new Etichette() { Visible = false, fake = true,};
            FakeEtichetta.SetImages();
            FakeEtichetta.picture.BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("no_icon")));

            foreach (int intero in Input.tipi_sort) Tipi_list.Add(new Etichette() { tipo = Input.tipi[intero], tipo_metodo = 1, });
            foreach (int intero in Input.metodi_sort) Metodi_list.Add(new Etichette() { tipo = Input.metodi[intero], tipo_metodo = 2, });

            //foreach (string tipo in Input.tipi) Tipi_list.Add(new Etichette() { tipo = tipo, tipo_metodo = 1, });
            //foreach (string metodo in Input.metodi) Metodi_list.Add(new Etichette() { tipo = metodo, tipo_metodo = 2,});

            foreach (Etichette etichetta in Tipi_list) { Tipi.Controls.Add(etichetta); etichetta.SetImages();}
            foreach (Etichette etichetta in Metodi_list) { Metodi.Controls.Add(etichetta); etichetta.SetImages(); }

            Controls.Add(Tipi);
            Controls.Add(Metodi);
            Controls.Add(PanelTipi);
            Controls.Add(PanelMetodi);
            Controls.Add(FakeEtichetta);
            Click += ClickNull;
            Tipi.Click += ClickNull;
            Metodi.Click += ClickNull;


            Icons_tip = new Label()
            {
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("Icone_Tipologie"))),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(Icons_tip);
            Icons_met = new Label()
            {
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("Icone_Metodi"))),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(Icons_met);
            Icons_txt = new Label()
            {
                Text = "Aggiungi icona",
                AutoSize = true,
                Visible = false,
            };
            Controls.Add(Icons_txt);
            Icons_tip.MouseEnter += new EventHandler(Enter_Icons_tip);
            Icons_tip.MouseLeave += new EventHandler(Leave_Icons_tip);
            Icons_tip.MouseClick += Icons_tipClick;
            Icons_met.MouseEnter += new EventHandler(Enter_Icons_met);
            Icons_met.MouseLeave += new EventHandler(Leave_Icons_met);
            Icons_met.MouseClick += Icons_metClick;

            Go_Calendar = new Label()
            {
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("Calendario"))),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(Go_Calendar);
            Go_Calendar_txt = new Label()
            {
                Text = "Vai al Calendario",
                AutoSize = true,
                Visible = false,
            };
            Controls.Add(Go_Calendar_txt);
            Go_Calendar.MouseEnter += new EventHandler(Enter_Go_Calendar);
            Go_Calendar.MouseLeave += new EventHandler(Leave_Go_Calendar);
            Go_Calendar.MouseClick += LeaveImpostazioni;

            NewTipo = new Label()
            {
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("tipologia"))),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(NewTipo);
            NewTipo_txt = new Label()
            {
                Text = "Nuova Tipologia",
                AutoSize = true,
                Visible = false,
            };
            Controls.Add(NewTipo_txt);
            NewTipo.MouseEnter += new EventHandler(Enter_NewTipo);
            NewTipo.MouseLeave += new EventHandler(Leave_NewTipo);
            NewTipo.MouseClick += EnterNewTipo;
            NewMetodo = new Label()
            {
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("metodo"))),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(NewMetodo);
            NewMetodo_txt = new Label()
            {
                Text = "Nuovo Metodo",
                AutoSize = true,
                Visible = false,
            };
            Controls.Add(NewMetodo_txt);
            NewMetodo.MouseEnter += new EventHandler(Enter_NewMetodo);
            NewMetodo.MouseLeave += new EventHandler(Leave_NewMetodo);
            NewMetodo.MouseClick += EnterNewMetodo;

            True = new Label()
            {
                Visible = false,
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("True"))),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(True);
            True.MouseEnter += new EventHandler(Enter_True);
            True.MouseLeave += new EventHandler(Leave_True);
            True.MouseClick += EnterTrue;

            ResumeLayout(true);
            ResizeImp();
            PanelTipi.Visible = true;
            PanelMetodi.Visible = true;
            PanelTipi.BringToFront();
            PanelMetodi.BringToFront();
            Update();
            PanelTipi.Visible = false;
            PanelMetodi.Visible = false;
        }

        public void ResizeImp()
        {
            Size = FinestraPrincipale.BackPanel.Size;
            if (Metodi_list.Count > 0) { Metodi_list[0].BackColor = System.Drawing.Color.LightCyan; Metodi_list[0].text_tipo.BackColor = System.Drawing.Color.LightCyan; Metodi_list[0].Unità.BackColor = System.Drawing.Color.LightCyan; Metodi_list[0].Centesimi.BackColor = System.Drawing.Color.LightCyan; }
            if (Metodi_list.Count > 1) { Metodi_list[1].BackColor = System.Drawing.Color.LightCyan; Metodi_list[1].text_tipo.BackColor = System.Drawing.Color.LightCyan; Metodi_list[1].Unità.BackColor = System.Drawing.Color.LightCyan; Metodi_list[1].Centesimi.BackColor = System.Drawing.Color.LightCyan; }
            Tipi_txt.Font = new System.Drawing.Font("Script MT Bold", (int)(Height * 0.01 + 15), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Metodi_txt.Font = new System.Drawing.Font("Script MT Bold", (int)(Height * 0.01 + 15), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Tipi_txt.Location = new Point(Width/4 - Tipi_txt.Width/2, 30);
            Metodi_txt.Location = new Point(Width * 3 / 4 - Metodi_txt.Width/2, Tipi_txt.Location.Y);
            Tipi.Location = new Point(10, Tipi_txt.Location.Y + Tipi_txt.Height + 10);
            Metodi.Location = new Point(Width / 2 + 10, Tipi.Location.Y);
            Tipi.Size = new Size(Width / 2 - 25, Height - Tipi.Location.Y - 40);
            Metodi.Size = new Size(Tipi.Width, (int)(Height *0.55));
            PanelTipi.ResizeForm(); PanelMetodi.ResizeForm();
            Icons_txt.Font = new System.Drawing.Font("Script MT Bold", (int)(Tipi.Size.Width * 0.02 + 6), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Icons_txt.Location = new Point((int)(Width * 0.7), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.87));
            Icons_tip_Piccolo(); Icons_met_Piccolo();
            Go_Calendar_txt.Font = new System.Drawing.Font("Script MT Bold", (int)(Tipi.Size.Width * 0.02 + 6), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Go_Calendar_txt.Location = new Point((int)(Width*0.82), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.7));
            Go_Calendar_Piccolo();
            NewTipo_txt.Font = new System.Drawing.Font("Script MT Bold", (int)(Tipi.Size.Width * 0.02 + 6), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            NewTipo_txt.Location = new Point((int)(Width * 0.55), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.7));
            NewTipo_Piccolo();
            NewMetodo_txt.Font = new System.Drawing.Font("Script MT Bold", (int)(Tipi.Size.Width * 0.02 + 6), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            NewMetodo_txt.Location = new Point((int)(Width * 0.65), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.7));
            NewMetodo_Piccolo();
            True_Piccolo();
            FakeEtichetta.Location = new Point(Metodi.Location.X, Go_Calendar.Location.Y);
            FakeEtichetta.Size = new Size(True.Location.X - FakeEtichetta.Location.X - (int)(True.Width *0.5), (int)(Tipi.Height * 0.8) / 8);
            if (Panel_attributo_scelta.exist) PanelDelete.ResizeForm();

            if (Tipi.Controls.Count != 0)
            {
                Tipi_list[0].Size = new Size((int)(Tipi.Width * 0.8), (int)(Tipi.Height * 0.8) / 8);
                Tipi_list[0].Location = new Point((int)(Tipi.Width * 0.1), RiferimentoTipi.Location.Y);
                for (int i = 1; i < Tipi_list.Count; i++)
                {
                    Tipi_list[i].Size = Tipi_list[i - 1].Size;
                    Tipi_list[i].Location = new Point(Tipi_list[i - 1].Location.X, Tipi_list[i - 1].Location.Y + (int)(Tipi_list[i - 1].Height * 1.1));
                }
            }
            if (Metodi_list.Count != 0)
            {
                Metodi_list[0].Size = new Size((int)(Tipi.Width * 0.8), (int)(Tipi.Height * 0.8) / 8);
                Metodi_list[0].Location = new Point((int)(Metodi.Width * 0.1), RiferimentoMetodi.Location.Y);
                for (int i = 1; i < Metodi_list.Count; i++)
                {
                    Metodi_list[i].Size = Metodi_list[i - 1].Size;
                    if(i==2) Metodi_list[i].Location = new Point(Metodi_list[i - 1].Location.X, Metodi_list[i - 1].Location.Y + (int)(Metodi_list[i - 1].Height * 1.8));
                    else Metodi_list[i].Location = new Point(Metodi_list[i - 1].Location.X, Metodi_list[i - 1].Location.Y + (int)(Metodi_list[i - 1].Height * 1.1));
                }
            }
            FakeEtichetta.ResizeEtichetta();
            foreach (Etichette etichetta in Tipi_list) { etichetta.ResizeEtichetta(); }
            foreach (Etichette etichetta in Metodi_list) { etichetta.ResizeEtichetta(); }
            Update();
        }

        public void RefreshIcons()
        {
            foreach (Etichette etichetta in Tipi_list) etichetta.SetImages();
            foreach (Etichette etichetta in Metodi_list) etichetta.SetImages();
        }
        

        public void CloseImpostazioni()
        {
            Disposer();
            active = false;
            FinestraPrincipale.BackPanel.ShowCalendar();
        }

        public void ClickNull(object sender, EventArgs e)
        {
            ClickNull();
        }
        public void ClickNull()
        {
            FinestraPrincipale.BackPanel.Focus();
            FakeEtichetta.picture.BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("no_icon")));
            FakeEtichetta.text_tipo.Text = "New";
            True.Hide();
            PanelTipi.Hide();
            PanelMetodi.Hide();
            FakeEtichetta.Hide();
            if(PanelDelete != null) PanelDelete.Disposer();
            Etichette.disabilitaclick = false;
        }

        private void Enter_Icons_met(object sender, EventArgs e)
        {
            Icons_txt.Text = "Aggiungi icona Metodo";
            Icons_txt.Visible = true;
            Icons_met_Grande();
        }
        private void Leave_Icons_met(object sender, EventArgs e)
        {
            Icons_txt.Visible = false;
            Icons_met_Piccolo();
        }
        private void Icons_met_Grande()
        {
            Icons_met.Size = new Size((int)(FinestraPrincipale.Finestra.Bounds.Height * 0.09), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.06));
            Icons_met.Location = new Point(Icons_met.Location.X - (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.015), Icons_met.Location.Y - (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.01));
        }
        private void Icons_met_Piccolo()
        {
            Icons_met.Size = new Size((int)(FinestraPrincipale.Finestra.Bounds.Height * 0.06), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.04));
            Icons_met.Location = new Point(Icons_txt.Location.X - (int)(Icons_met.Width * 1.3), Icons_txt.Location.Y + (Icons_txt.Height - Icons_met.Height) / 2);
        }
        private void Icons_metClick(object sender, EventArgs e)
        {
            ClickNull();
            if (Funzioni_utili.CountImgs(Program.num_max_icons_met, 2) >= 25) { Console.WriteLine("Hai raggiunto il limite massimo di icone Metodo consentite"); return; }
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = filter
            };
            try
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Input.RefreshImages();
                    Bitmap btm = new Bitmap(ofd.FileName);
                    int width = btm.Width, height = btm.Height;
                    if (width <= height && width <= dim_minima) btm = new Bitmap(btm, new Size(width, width));
                    else if (width > height && height <= dim_minima) btm = new Bitmap(btm, new Size(height, height));
                    else btm = new Bitmap(btm, new Size(dim_minima, dim_minima));
                    string name = Funzioni_utili.NewName(Program.num_max_icons_met, 2);
                    btm.Save(Input.path + @"Icons\Metodi\" + name + ".png");
                    File.SetLastWriteTime(Input.path + @"Icons\Metodi\" + name + ".png", DateTime.Now);
                    Savings.icons_changed = true;
                    btm.Dispose();
                    PanelMetodi.VisualTipi.Insert(0, new VisualModifiche("", name, 2, 2) { resource = name });
                    PanelMetodi.Controls.Add(PanelMetodi.VisualTipi[0]);
                    PanelMetodi.ResizeForm();
                    MessageBox.Show("Icona importata con successo");
                }
            }
            catch (Exception) { MessageBox.Show(errore); }
        }
        private void Enter_Icons_tip(object sender, EventArgs e)
        {
            Icons_txt.Text = "Aggiungi icona Tipologia";
            Icons_txt.Visible = true;
            Icons_tip_Grande();
        }
        private void Leave_Icons_tip(object sender, EventArgs e)
        {
            Icons_txt.Visible = false;
            Icons_tip_Piccolo();
        }
        private void Icons_tip_Grande()
        {
            Icons_tip.Size = new Size((int)(FinestraPrincipale.Finestra.Bounds.Height * 0.09), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.06));
            Icons_tip.Location = new Point(Icons_tip.Location.X - (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.015), Icons_tip.Location.Y - (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.01));
        }
        private void Icons_tip_Piccolo()
        {
            Icons_tip.Size = new Size((int)(FinestraPrincipale.Finestra.Bounds.Height * 0.06), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.04));
            Icons_tip.Location = new Point(Icons_txt.Location.X - (int)(Icons_tip.Width * 2.8), Icons_txt.Location.Y + (Icons_txt.Height - Icons_tip.Height) / 2);
        }
        private void Icons_tipClick(object sender, EventArgs e)
        {
            ClickNull();
            if (Funzioni_utili.CountImgs(Program.num_max_icons_tip, 1) >= 25) { Console.WriteLine("Hai raggiunto il limite massimo di icone Tipologia consentite"); return; }
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = filter
            };
            try
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Input.RefreshImages();
                    Bitmap btm = new Bitmap(ofd.FileName);
                    int width = btm.Width, height = btm.Height;
                    if (width <= height && width <= dim_minima) btm = new Bitmap(btm, new Size(width, width));
                    else if (width > height && height <= dim_minima) btm = new Bitmap(btm, new Size(height, height));
                    else btm = new Bitmap(btm, new Size(dim_minima, dim_minima));
                    string name = Funzioni_utili.NewName(Program.num_max_icons_tip, 1);
                    btm.Save(Input.path + @"Icons\Tipologie\" + name + ".png");
                    File.SetLastWriteTime(Input.path + @"Icons\Tipologie\" + name + ".png", DateTime.Now);
                    Savings.icons_changed = true;
                    btm.Dispose();
                    PanelTipi.VisualTipi.Insert(0, new VisualModifiche("", name, 2, 1) { resource = name });
                    PanelTipi.Controls.Add(PanelTipi.VisualTipi[0]);
                    PanelTipi.ResizeForm();
                    MessageBox.Show("Icona importata con successo");
                }
            }
            catch (Exception) { MessageBox.Show(errore); }
        }
        private void Enter_Go_Calendar(object sender, EventArgs e)
        {
            Go_Calendar_txt.Visible = true;
            Go_Calendar_Grande();
        }
        private void Leave_Go_Calendar(object sender, EventArgs e)
        {
            Go_Calendar_txt.Visible = false;
            Go_Calendar_Piccolo();
        }
        private void Go_Calendar_Grande()
        {
            Go_Calendar.Size = new Size((int)(FinestraPrincipale.Finestra.Bounds.Height * 0.1), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.1));
            Go_Calendar.Location = new Point(Go_Calendar_txt.Location.X + (Go_Calendar_txt.Width - Go_Calendar.Width) / 2, Go_Calendar_txt.Location.Y + Go_Calendar_txt.Height);
        }
        private void Go_Calendar_Piccolo()
        {
            Go_Calendar.Size = new Size((int)(FinestraPrincipale.Finestra.Bounds.Height * 0.075), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.075));
            Go_Calendar.Location = new Point(Go_Calendar_txt.Location.X + (Go_Calendar_txt.Width - Go_Calendar.Width) / 2, Go_Calendar_txt.Location.Y + Go_Calendar_txt.Height);
        }
        public void LeaveImpostazioni(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                CloseImpostazioni();
                FinestraPrincipale.BackPanel.ShowCalendar();
            }
        }

        private void Enter_NewTipo(object sender, EventArgs e)
        {
            NewTipo_txt.Visible = true;
            NewTipo_Grande();
        }
        private void Leave_NewTipo(object sender, EventArgs e)
        {
            NewTipo_txt.Visible = false;
            NewTipo_Piccolo();
        }
        private void NewTipo_Grande()
        {
            NewTipo.Size = new Size((int)(FinestraPrincipale.Finestra.Bounds.Height * 0.08), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.08));
            NewTipo.Location = new Point(NewTipo_txt.Location.X + (NewTipo_txt.Width - NewTipo.Width) / 2, NewTipo_txt.Location.Y + NewTipo_txt.Height);
        }
        private void NewTipo_Piccolo()
        {
            NewTipo.Size = new Size((int)(FinestraPrincipale.Finestra.Bounds.Height * 0.06), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.06));
            NewTipo.Location = new Point(NewTipo_txt.Location.X + (NewTipo_txt.Width - NewTipo.Width) / 2, NewTipo_txt.Location.Y + NewTipo_txt.Height + (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.01));
        }
        private void EnterNewTipo(object sender, EventArgs e)
        {
            FakeEtichetta.tipo_metodo = 1;
            FakeEtichetta.ResizeEtichetta();
            FakeEtichetta.image = false;
            FakeEtichetta.Show();
        }
        private void Enter_NewMetodo(object sender, EventArgs e)
        {
            NewMetodo_txt.Visible = true;
            NewMetodo_Grande();
        }
        private void Leave_NewMetodo(object sender, EventArgs e)
        {
            NewMetodo_txt.Visible = false;
            NewMetodo_Piccolo();
        }
        private void NewMetodo_Grande()
        {
            NewMetodo.Size = new Size((int)(FinestraPrincipale.Finestra.Bounds.Height * 0.08), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.08));
            NewMetodo.Location = new Point(NewMetodo_txt.Location.X + (NewMetodo_txt.Width - NewMetodo.Width) / 2, NewMetodo_txt.Location.Y + NewMetodo_txt.Height);
        }
        private void NewMetodo_Piccolo()
        {
            NewMetodo.Size = new Size((int)(FinestraPrincipale.Finestra.Bounds.Height * 0.06), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.06));
            NewMetodo.Location = new Point(NewMetodo_txt.Location.X + (NewMetodo_txt.Width - NewMetodo.Width) / 2, NewMetodo_txt.Location.Y + NewMetodo_txt.Height + (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.01));
        }
        private void EnterNewMetodo(object sender, EventArgs e)
        {
            FakeEtichetta.tipo_metodo = 2;
            FakeEtichetta.ResizeEtichetta();
            FakeEtichetta.image = false;
            FakeEtichetta.Unità.Text = "0";
            FakeEtichetta.Centesimi.Text = "00";
            FakeEtichetta.Show();
        }
        private void True_Grande()
        {
            True.Size = new Size(NewMetodo.Width + 4, NewMetodo.Width + 4);
            True.Location = new Point(True.Location.X - 2, True.Location.Y - 2);
        }
        private void True_Piccolo()
        {
            True.Size = NewMetodo.Size;
            True.Location = new Point(Go_Calendar.Location.X - (int)(True.Width*1.5), Go_Calendar.Location.Y + (Go_Calendar.Height-True.Height)/2);
        }
        private void Enter_True(object sender, EventArgs e)
        {
            True_Grande();
        }
        private void Leave_True(object sender, EventArgs e)
        {
            True_Piccolo();
        }
        private void EnterTrue(object sender, EventArgs e)
        {
            FakeEtichetta.New();
        }
        
    }
}
