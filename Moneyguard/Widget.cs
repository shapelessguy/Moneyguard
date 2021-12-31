using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace Moneyguard
{
    public partial class WidgetMoneyguard : Form
    {
        static System.ComponentModel.IContainer componentsNotify;
        static public System.Windows.Forms.NotifyIcon notifyIcon1;
        static public bool ready_toclose = false;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public Widget_PanelTipi PannelloTipi;
        public Widget_PanelFakeTipi PannelloFakeTipi;
        public Widget_PanelMetodi PannelloMetodi;
        public Widget_PanelFinale PannelloFinale;
        public Widget_PanelImpostazioni PannelloImpostazioni;
        Timer timer, timer_MouseLeaved, timer_Visibility, timer_save;
        static public bool active = false;
        public int dim_pannello = 300;
        public string attributo;
        public string tipo;
        public string metodo;
        public static Color transparent = Color.DarkBlue;
        public static Color backcolor = Color.Black;
        public static Color textcolor = Color.White;
        static public Point location;
        static public Size size;
        static public bool refreshpanel;
        public bool widget_visible = false;
        public bool clicknulled = false;
        private bool allowshowdisplay = false;
        public string elimina_tip = "";
        public string elimina_met = "";


        [DllImport("User32.dll")]
        static extern IntPtr FindWindow(String lpClassName, String lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll")]
        static extern int SetParent(IntPtr hWndChild, IntPtr hWndNewParent);


        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public void Disposer()
        {
            PannelloTipi.Disposer();
            PannelloFakeTipi.Disposer();
            PannelloMetodi.Disposer();
            Dispose();
        }
        public WidgetMoneyguard()
        {
            componentsNotify = new System.ComponentModel.Container();
            widget_visible = Impostazioni.widget_visible;
            Impostazioni.widget_visible = false;
            
            InitializeComponent();
            GotFocus += GetFocus;
            refreshpanel = false;
            ready_toclose = false;
            ShowInTaskbar = false;
            FormClosing += new System.Windows.Forms.FormClosingEventHandler(MenuExit_Click);
            notifyIcon1 = new System.Windows.Forms.NotifyIcon(componentsNotify)
            {
                BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Warning,
                BalloonTipText = "ciao",
                BalloonTipTitle = "asdd",
                Icon = (System.Drawing.Icon)(Properties.Resources.ResourceManager.GetObject("Moneyguard")),
                Text = "Moneyguard",
                Visible = true,
            };
            notifyIcon1.MouseClick += new MouseEventHandler(NotifyIcon_MouseRightClick);
            TrayMenuContext();
            timer = new Timer
            {
                Enabled = true,
                Interval = 100
            };
            timer.Tick += new System.EventHandler(Timer);
            timer_Visibility = new Timer
            {
                Enabled = true,
                Interval = 1200,
            };
            timer_Visibility.Tick += new System.EventHandler(Timer_Visibility);
            /*
            timer_save = new Timer
            {
                Enabled = true,
                Interval = 1000* 60 * min,   // * (GoogleDrive.limite_secondi+5),
            };
            timer_save.Tick += new System.EventHandler(Timer_Save);*/

            CreateElements();
            active = true;
            ResizeForm();
            ResizeForm();
            Savings.months_to_save = new List<DateTime>();
        }
        
        
        void NotifyIcon_MouseRightClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TrayMenuContext();
            }
            if (e.Button == MouseButtons.Left)
            {
                FinestraPrincipale.resume_calendar = true;
                if (FinestraPrincipale.active == false)
                {
                    Program.point1.Set();
                }
            }
        }

        void TrayMenuContext()
        {
            notifyIcon1.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            notifyIcon1.ContextMenuStrip.Items.Add("Riposiziona la finestra", null, MenuResizeCalendar_Click);
            notifyIcon1.ContextMenuStrip.Items.Add("Riposiziona il Gadget", null, MenuResizeWidget_Click);
            notifyIcon1.ContextMenuStrip.Items.Add("Mostra la finestra", (System.Drawing.Bitmap)(Properties.Resources.ResourceManager.GetObject("Calendario2")), MenuShowCalendar_Click);
            notifyIcon1.ContextMenuStrip.Items.Add("Esci", (System.Drawing.Bitmap)(Properties.Resources.ResourceManager.GetObject("False")), MenuExit_Click);
        }
        void MenuResizeCalendar_Click(object sender, EventArgs e)
        {
            MenuShowCalendar_Click();
            FinestraPrincipale.resize_calendar = true;
        }
        void MenuResizeWidget_Click(object sender, EventArgs e)
        {
            if (FinestraPrincipale.Finestra.WindowState == FormWindowState.Minimized) location = new Point(100, 100);
            else { location = new Point(FinestraPrincipale.Finestra.Location.X + (FinestraPrincipale.Finestra.Width - Width) / 2, FinestraPrincipale.Finestra.Location.Y + (FinestraPrincipale.Finestra.Height - Height) / 2); }
            Location = location;
            BringToFront();
        }
        void MenuShowCalendar_Click(object sender, EventArgs e)
        {
            MenuShowCalendar_Click();
        }
        void MenuShowCalendar_Click()
        {
            FinestraPrincipale.resume_calendar = true;
            if (FinestraPrincipale.active == false)
            {
                Program.point1.Set();
            }
        }
        void MenuExit_Click(object sender, EventArgs e)
        {
            Program.caricamento_show = true;
            FinestraPrincipale.ready_toclose = true;
            Program.quit.WaitOne();
            Program.ready_toquit = true;
            Program.point1.Set();
            Application.ExitThread();
        }
        void MenuExit_Click()
        {
        }
        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            timer.Tick -= Timer;
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }


            location = Location;
            timer.Tick += Timer;
        }
        bool inizio = true;
        private void GetFocus(object sender, EventArgs e)
        {
            //if(allowshowdisplay) location = Location;
        }
        private void Timer(object sender, EventArgs e)
        {
            if (allowshowdisplay == false) allowshowdisplay = true;
            if (inizio) Location = location;
            inizio = false;
            bool aus = false;
            location = Location;
            if(Focused) Location = location;
            foreach (TextBox txt in Program.widget.PannelloFinale.textbox) if(txt.Focused) { aus = true; break; }
            if (Focused || PannelloFinale.Unità.Focused || PannelloFinale.Centesimi.Focused || PannelloFinale.TimePicker.Focused || PannelloFinale.DatePicker.Focused || aus || PannelloImpostazioni.trackBar.Focused
                 || PannelloImpostazioni.checkbox1.Focused || PannelloImpostazioni.checkbox2.Focused) {; } else
            {
                ClickNull();
            }
            if(Program.widget.PannelloFinale.Visible == false)
            {
                Program.widget.PannelloFinale.Unità.Text = "";
                Program.widget.PannelloFinale.Centesimi.Text = "";
                Program.widget.PannelloFinale.textbox.Clear();
                Program.widget.PannelloFinale.Controllo.Clear();
                Program.widget.PannelloFinale.AttributoPanel.Controls.Clear();
            }
            if (Controlli.ClientRectangle.Contains(Controlli.PointToClient(Cursor.Position)))
            {
                if (Moving.Visible == false) { Moving.Visible = true; Settings.Visible = true; }
            }
            else { if (Moving.Visible) { Moving.Visible = false; Settings.Visible = false; } }
            if (File.Exists(Input.path_moneyguard + "Recall.txt")) { File.Delete(Input.path_moneyguard + "Recall.txt"); MenuShowCalendar_Click(); }
            if (refreshpanel && size.Height == Size.Height) { RefreshVisual(); refreshpanel = false; }
            if (ready_toclose) Application.ExitThread();
            if (Visible == false && Impostazioni.widget_visible == false) return;
            if (Visible == true && Impostazioni.widget_visible == true) return;
            if (Impostazioni.widget_visible == true) Visible = true; else Visible = false;
            if (elimina_tip != "")
            {
                for (int i = 0; i < PannelloTipi.VisualTipi.Count; i++)
                {
                    if (PannelloTipi.VisualTipi[i].Tipo.Text == elimina_tip) PannelloTipi.VisualTipi[i].Aggiorna(PannelloTipi.VisualTipi[i].Tipo.Text, Associazione.IconaAssociata(tipo));
                }
            }
            if (elimina_met != "")
            {
                for (int i = 0; i < PannelloMetodi.VisualMetodi.Count; i++)
                {
                    if (PannelloMetodi.VisualMetodi[i].Metodo.Text == elimina_met) PannelloMetodi.VisualMetodi[i].Aggiorna(PannelloMetodi.VisualMetodi[i].Metodo.Text, Associazione.MiconaAssociata(tipo));
                }
                for (int i = 0; i < PannelloFakeTipi.VisualFakeTipi.Count; i++)
                {
                    if (PannelloFakeTipi.VisualFakeTipi[i].Tipo.Text == elimina_met) PannelloFakeTipi.VisualFakeTipi[i].Aggiorna(PannelloFakeTipi.VisualFakeTipi[i].Tipo.Text, Associazione.MiconaAssociata(tipo));
                }
            }
        }

        private void Timer_Visibility(object sender, EventArgs e)
        {
            timer_Visibility.Tick -= Timer_Visibility;
            Impostazioni.widget_visible = widget_visible;
            ResizeForm();
            FinestraPrincipale.activate = true;

        }

        public void ResizeForm()
        {
            size = new Size(Impostazioni.widgetZoom * 5 + 200, (int)((Impostazioni.widgetZoom * 5 + 200) / 3.5));
            Size = size;
            panel1.Size = new Size(Height * 3, Height);
            Controlli_(); Moving_(); Settings_(); Introito_Piccolo(); Spesa_Piccolo(); Trasferimento_Piccolo();
        }
        public void SemiResizeForm()
        {
            size = new Size(Impostazioni.widgetZoom * 5 + 200, (int)((Impostazioni.widgetZoom * 5 + 200) / 3.5));
            panel1.Size = new Size((int)((Impostazioni.widgetZoom * 5 + 200) / 3.5) * 3, (int)((Impostazioni.widgetZoom * 5 + 200) / 3.5));
            Controlli_(); Moving_(); Settings_(); Introito_Piccolo(); Spesa_Piccolo(); Trasferimento_Piccolo();
        }


        public Label Introito, Spesa, Trasferimento, Moving, Settings, Controlli;
        void CreateElements()
        {
            Introito = new Label()
            {
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("aicon1"))),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            panel1.Controls.Add(Introito);
            Spesa = new Label()
            {
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("aicon2"))),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            panel1.Controls.Add(Spesa);
            Trasferimento = new Label()
            {
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("aicon3"))),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            panel1.Controls.Add(Trasferimento);
            Controlli = new Label();
            Controls.Add(Controlli);
            Controlli.Click += ClickNull;
            panel1.Click += ClickNull;
            Click += ClickNull;
            Moving = new Label()
            {
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("drag"))),
                BackgroundImageLayout = ImageLayout.Stretch,
                Visible = false,
            };
            Controlli.Controls.Add(Moving);
            Settings = new Label()
            {
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("impostazioni"))),
                BackgroundImageLayout = ImageLayout.Stretch,
                Visible = false,
            };
            Controlli.Controls.Add(Settings);
            Settings.MouseClick += ClickImpostazioni;

            timer_MouseLeaved = new Timer
            {
                Enabled = true,
                Interval = 1
            };

            Introito_Piccolo();
            Spesa_Piccolo();
            Trasferimento_Piccolo();
            Controlli_(); Moving_(); Settings_();
            Moving.MouseMove += Panel1_MouseMove;
            Controlli.MouseEnter += MouseEntered;

            Introito.MouseDown += Evento1;
            Spesa.MouseDown += Evento2;
            Trasferimento.MouseDown += Evento3;

            Introito.MouseEnter += Introito_Grande;
            Introito.MouseLeave += Introito_Piccolo;
            Spesa.MouseEnter += Spesa_Grande;
            Spesa.MouseLeave += Spesa_Piccolo;
            Trasferimento.MouseEnter += Trasferimento_Grande;
            Trasferimento.MouseLeave += Trasferimento_Piccolo;

            PannelloTipi = new Widget_PanelTipi();
            PannelloFakeTipi = new Widget_PanelFakeTipi();
            PannelloMetodi = new Widget_PanelMetodi();
            PannelloFinale = new Widget_PanelFinale();
            PannelloImpostazioni = new Widget_PanelImpostazioni();
            Controls.Add(PannelloTipi);
            Controls.Add(PannelloFakeTipi);
            Controls.Add(PannelloMetodi);
            Controls.Add(PannelloFinale);
            Controls.Add(PannelloImpostazioni);
            PannelloTipi.Click += ClickNull;
            PannelloMetodi.Click += ClickNull;
            PannelloFakeTipi.Click += ClickNull;
            PannelloFinale.Click += ClickNull;

            Controlli.Visible = true;

        }
        void MouseEntered(object sender, EventArgs e)
        {
            Moving.Visible = true;
            Settings.Visible = true;
        }
        private void ClickNull(object sender, EventArgs e)
        {
            ClickNull();
        }
        public void ClickNull()
        {
            if (clicknulled) return;
            clicknulled = true;
            Size = size;
            PannelloTipi.ResizeForm();
            PannelloFakeTipi.ResizeForm();
            PannelloMetodi.ResizeForm();
            PannelloFinale.ResizeForm();
            PannelloImpostazioni.ResizeForm();
            PannelloTipi.Visible = false;
            PannelloFakeTipi.Visible = false;
            PannelloMetodi.Visible = false;
            PannelloFinale.Visible = false;
            PannelloImpostazioni.Visible = false;
            panel1.Visible = true;
        }
        public void Evento1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) NewEvento("Introito");
        }
        public void Evento2(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) NewEvento("Spesa");
        }
        public void Evento3(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) NewEvento("Trasferimento");
        }
        void NewEvento(string attributo)
        {
            SuspendLayout();
            clicknulled = false;
            try { FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.noexception = true; } catch (Exception) { }
            foreach (Visual_FakeTipi tip in PannelloFakeTipi.VisualFakeTipi) { tip.BackColor = backcolor; }
            foreach (Visual_Tipi tip in PannelloTipi.VisualTipi) { tip.BackColor = backcolor; }
            foreach (Visual_Metodi tip in PannelloMetodi.VisualMetodi) { tip.BackColor = backcolor; }
            this.attributo = attributo;
            panel1.Visible = false;
            Size = new Size(size.Width, size.Height + dim_pannello);
            PannelloImpostazioni.Visible = false;
            PannelloTipi.ResizeForm();
            PannelloFakeTipi.ResizeForm();
            PannelloMetodi.ResizeForm();
            PannelloFinale.ResizeForm();
            PannelloImpostazioni.ResizeForm();
            if (attributo == "Trasferimento") { PannelloFakeTipi.Visible = true; } else { PannelloTipi.Visible = true; }
            ResumeLayout();
        }
        void ClickImpostazioni(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (PannelloImpostazioni.Visible) return;
            ClickNull();
            clicknulled = false;
            SuspendLayout();
            Size = new Size(700, (int)(700 / 3.5) + PannelloImpostazioni.imp_height);
            PannelloImpostazioni.ResizeForm();

            PannelloImpostazioni.Visible = true;
            PannelloImpostazioni.BringToFront();
            ResumeLayout();
        }
        void Introito_Grande(object sender, EventArgs e)
        {
            Introito.Size = new Size(panel1.Height, panel1.Height);
            Introito.Location = new Point(0, 0);
        }
        void Introito_Piccolo(object sender, EventArgs e)
        {
            Introito_Piccolo();
        }
        void Introito_Piccolo()
        {
            Introito.Size = new Size(panel1.Height - 6, panel1.Height - 6);
            Introito.Location = new Point(3, 3);
        }
        void Spesa_Grande(object sender, EventArgs e)
        {
            Spesa.Size = new Size(panel1.Height, panel1.Height);
            Spesa.Location = new Point(panel1.Height, 0);
        }
        void Spesa_Piccolo(object sender, EventArgs e)
        {
            Spesa_Piccolo();
        }
        void Spesa_Piccolo()
        {
            Spesa.Size = new Size(panel1.Height - 6, panel1.Height - 6);
            Spesa.Location = new Point(panel1.Height + 3, 3);
        }
        void Trasferimento_Grande(object sender, EventArgs e)
        {
            Trasferimento.Size = new Size(panel1.Height, panel1.Height);
            Trasferimento.Location = new Point(panel1.Height*2, 0);
        }
        void Trasferimento_Piccolo(object sender, EventArgs e)
        {
            Trasferimento_Piccolo();
        }
        void Trasferimento_Piccolo()
        {
            Trasferimento.Size = new Size(panel1.Height - 6, panel1.Height - 6);
            Trasferimento.Location = new Point(panel1.Height*2 + 3, 3);
        }
        void Controlli_()
        {
            Controlli.Size = new Size(size.Width - panel1.Height * 3, (int)((size.Width - panel1.Height * 3) * 0.8) + 3 * (Impostazioni.widgetZoom / 10 + 5));
            if (Impostazioni.controllidx) { panel1.Location = new Point(0, 0); Controlli.Location = new Point(panel1.Width, 0); }
            else { panel1.Location = new Point(Controlli.Width, 0); Controlli.Location = new Point(0, 0); }
        }
        void Moving_()
        {
            Moving.Size = new Size((int)((size.Width - panel1.Height * 3) * 0.4), (int)((size.Width - panel1.Height * 3) * 0.4));
            Moving.Location = new Point((Impostazioni.widgetZoom / 10 + 5), (Impostazioni.widgetZoom / 10 + 5));
            Moving.BringToFront();
        }
        void Settings_()
        {
            Settings.Size = new Size((int)((size.Width - panel1.Height * 3) * 0.4), (int)((size.Width - panel1.Height * 3) * 0.4));
            Settings.Location = new Point((Impostazioni.widgetZoom / 10 + 5), Controlli.Height - (Impostazioni.widgetZoom / 10 + 5) - Settings.Height);
        }

        void RefreshVisual()
        {
            int i = 0; foreach (var tip in PannelloMetodi.VisualMetodi) tip.Disposer(); PannelloMetodi.VisualMetodi.Clear(); PannelloMetodi.Controls.Clear();
            foreach (string metodo in Input.metodi)
            {
                PannelloMetodi.VisualMetodi.Add(new Visual_Metodi(metodo, Associazione.MiconaAssociata(metodo)));
                PannelloMetodi.Controls.Add(PannelloMetodi.VisualMetodi[i]);
                i++;
            }
            i = 0; foreach (var tip in PannelloTipi.VisualTipi) tip.Disposer(); PannelloTipi.VisualTipi.Clear(); PannelloTipi.Controls.Clear();
            foreach (string tipo in Input.tipi)
            {
                PannelloTipi.VisualTipi.Add(new Visual_Tipi(tipo, Associazione.IconaAssociata(tipo)));
                PannelloTipi.Controls.Add(PannelloTipi.VisualTipi[i]);
                i++;
            }
            i = 0; foreach (var tip in PannelloFakeTipi.VisualFakeTipi) tip.Disposer(); PannelloFakeTipi.VisualFakeTipi.Clear(); PannelloFakeTipi.Controls.Clear();
            foreach (string faketipo in Input.metodi)
            {
                PannelloFakeTipi.VisualFakeTipi.Add(new Visual_FakeTipi(faketipo, Associazione.MiconaAssociata(faketipo)));
                PannelloFakeTipi.Controls.Add(PannelloFakeTipi.VisualFakeTipi[i]);
                i++;
            }
        }

        private void Timer_Save(object sender, EventArgs e)
        {
            Program.sync_by_user = false;
            System.Threading.Thread Update1 = new System.Threading.Thread(Program.SyncData); Update1.Start();
        }

        public void SetColors()
        {
            if (Impostazioni.widget_contrasto)
            {
                transparent = Color.Beige;
                backcolor = Color.White;
                textcolor = Color.Black;
            }
            else
            {
                transparent = Color.DarkBlue;
                backcolor = Color.Black;
                textcolor = Color.White;
            }
            AggiornaColors();
        }
        public void AggiornaColors()
        {
            TransparencyKey = transparent;
            panel1.BackColor = transparent;
            BackColor = transparent;
            PannelloFakeTipi.BackColor = transparent;
            PannelloTipi.BackColor = transparent;
            PannelloMetodi.BackColor = transparent;
            PannelloFinale.BackColor = transparent;
            PannelloFinale.AttributoPanel.BackColor = backcolor;
            PannelloFinale.Unità.BackColor = backcolor;
            PannelloFinale.Centesimi.BackColor = backcolor;
            PannelloFinale.Unità.ForeColor = textcolor;
            PannelloFinale.Centesimi.ForeColor = textcolor;
            PannelloFinale.Punto.ForeColor = textcolor;
            PannelloFinale.Euro.ForeColor = textcolor;
        }
        protected override void SetVisibleCore(bool value)
        {
            try { base.SetVisibleCore(allowshowdisplay ? value : allowshowdisplay); } catch (Exception) { }
        }

    }
}
