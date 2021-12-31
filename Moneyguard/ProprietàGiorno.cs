using Moneyguard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class ProprietàGiorno : Panel
    {
        public static string attributi_txt = "Gli attributi sono utili soprattutto in fase di ricerca. Solitamente si contraddistinguono gli eventi con:\n" +
                "- luogo in cu si è svolta la transazione.\n" +
                "- persone coinvolte nella transazione.\n" +
                "- oggetti della transazione.\n" +
                "- eventuali tempistiche della transazione.\n" +
                "Gli attributi si prestano in generale a qualunque tipo di gestione personalizzata.";
        public Panel AttributoPanel;
        public Label Attributo_txt;
        public TextBox Note;
        public Label Note_txt;
        public Label Giorno_txt;
        public Label Go_Calendar;
        public Label Go_Calendar_txt;
        public Label Plus;
        public Label Minus;
        public Label Trasferimento;
        public Panel Tipi;
        public List<Pulsante> panelspeciale = new List<Pulsante>();
        public Label empty;
        public MotoreRicerca PanelMotore;
        public Label button_precedente;
        public Label button_successivo;
        private Timer timer, timer_FindPulsante;
        public Timer timerPannello;
        public Timer timer_Saving;
        public int Note_something = -1;
        public int panel_corrente = -1;
        public int panel_clicked = -1;
        public bool pulsanti_selectable = true;
        public Panel_Guidato Panel_EventoGuidato;
        public Panel_NewEvento Panel_NewEvento;
        private Label Riferimento;
        private bool scrolled = false;
        public string nuovo_attributo = "";
        public readonly int size_controllo = 15;
        public int current_textbox = 0;
        static public string previoustext = "";

        static private Size taglia_piccola, taglia_grande;
        static public bool attributo_selected = false;
        static public bool time_to_save = false;
        static public bool pulsante_pending = false;
        public static bool exist = false;
        public ToolTip toolTip;

        public void Disposer()
        {
            exist = false;
            foreach (Pulsante pulsante in FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale)  pulsante.Disposer();
            toolTip.Dispose();
            Go_Calendar.BackgroundImage.Dispose();
            Trasferimento.BackgroundImage.Dispose();
            Plus.BackgroundImage.Dispose();
            Minus.BackgroundImage.Dispose();
            Go_Calendar.Dispose();
            Trasferimento.Dispose();
            Plus.Dispose();
            Minus.Dispose();
            Go_Calendar_txt.Dispose();
            Tipi.Dispose();
            Note.Dispose();
            empty.Dispose();
            timer.Tick -= Timer;
            timer.Dispose();
            timer_FindPulsante.Tick -= FindPulsante;
            timer_FindPulsante.Dispose();
            timerPannello.Tick -= TimerPannello;
            timerPannello.Dispose();
            timer_Saving.Tick -= Saving;
            timer_Saving.Dispose();
            PanelMotore.Disposer();
            if (Panel_EventoGuidato != null) Panel_EventoGuidato.Disposer();
            if (Panel_NewEvento != null) Panel_NewEvento.Disposer();
            Dispose();
            GC.Collect();
        }

        public ProprietàGiorno()
        {
            DoubleBuffered = true;
            exist = true;
            FinestraPrincipale.BackPanel.Panel_Giorno = this;
            KeyDown += new System.Windows.Forms.KeyEventHandler(FinestraPrincipale.BackPanel.Shortcuts);
            MouseHover += new EventHandler(StayNull);
            Click += new EventHandler(ClickNull);

            toolTip = new ToolTip
            {
                AutoPopDelay = 20000,
                InitialDelay = 500,
                ReshowDelay = 500
            };
            Tipi = new Panel()
            {
                AutoScroll = true,
                Location = new Point(0, 0),
                BorderStyle = BorderStyle.FixedSingle,
            };
            Controls.Add(Tipi);
            PanelMotore = new MotoreRicerca(true, 2);
            Controls.Add(PanelMotore);
            Note = new TextBox()
            {
                Multiline = true,
                ScrollBars = System.Windows.Forms.ScrollBars.Vertical,
                BorderStyle = BorderStyle.FixedSingle,
            };
            FinestraPrincipale.BackPanel.Panel_Giorno.Controls.Add(Note);
            Note_txt = new Label()
            {
                Text = "Note:",
                AutoSize = true,
            };
            Giorno_txt = new Label()
            {
                Text = "",
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            Populate_Tipi();

            Attributo_txt = new Label()
            {
                Text = "Attributi:",
                AutoSize = true,
                Visible = false,
            };
            Controls.Add(Attributo_txt);
            toolTip.SetToolTip(Attributo_txt, attributi_txt);
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
            Plus = new Label()
            {
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("Plus"))),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(Plus);
            toolTip.SetToolTip(Plus, "Nuovo Introito");
            Minus = new Label()
            {
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("Minus"))),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(Minus);
            toolTip.SetToolTip(Minus, "Nuova Spesa");
            Trasferimento = new Label()
            {
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("aicon3"))),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(Trasferimento);
            toolTip.SetToolTip(Trasferimento, "Nuovo Trasferimento");
            Trasferimento.MouseEnter += new EventHandler(Enter_Trasferimento);
            Trasferimento.MouseLeave += new EventHandler(Leave_Trasferimento);
            Plus.MouseEnter += new EventHandler(Enter_Plus);
            Plus.MouseLeave += new EventHandler(Leave_Plus);
            Minus.MouseEnter += new EventHandler(Enter_Minus);
            Minus.MouseLeave += new EventHandler(Leave_Minus);
            Controls.Add(Note_txt);
            Controls.Add(Giorno_txt);
            Location = new Point(0, 0);
            BackColor = Color.White;

            //Definizione pulsanti Indietro-Avanti
            button_precedente = new System.Windows.Forms.Label
            {
                BackColor = System.Drawing.Color.Transparent,
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("Freccia_sx"))),
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
                FlatStyle = System.Windows.Forms.FlatStyle.Popup
            };
            button_precedente.MouseClick += new MouseEventHandler(Button_precedente_Click);
            button_precedente.MouseEnter += new System.EventHandler(Button_precedente_MouseEnter);
            button_precedente.MouseLeave += new System.EventHandler(Button_precedente_MouseLeave);
            button_precedente.MouseClick += FinestraPrincipale.BackPanel.ClickNull;
            Controls.Add(button_precedente);

            button_successivo = new System.Windows.Forms.Label
            {
                BackColor = System.Drawing.Color.Transparent,
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("Freccia_dx"))),
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
                FlatStyle = System.Windows.Forms.FlatStyle.Popup
            };
            button_successivo.MouseClick += new MouseEventHandler(Button_successivo_Click);
            button_successivo.MouseEnter += new System.EventHandler(Button_successivo_MouseEnter);
            button_successivo.MouseLeave += new System.EventHandler(Button_successivo_MouseLeave);
            button_successivo.MouseClick += FinestraPrincipale.BackPanel.ClickNull;
            Controls.Add(button_successivo);
            // 
            // ProprietàGiorno
            // 
            AttributoPanel = new Panel()
            {
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false,
            };
            FinestraPrincipale.BackPanel.Panel_Giorno.Controls.Add(AttributoPanel);
            Note.BackColor = Color.AntiqueWhite;

            empty = new Label()
            {
                Text = "Vuoto",
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = false,
            };
            Size size = new Size((int)(Tipi.Size.Width * 0.8), (int)(Tipi.Size.Height / 5));

            timer = new Timer
            {
                Enabled = true,
                Interval = 10,
            };
            timer.Tick += new System.EventHandler(Timer);
            timerPannello = new Timer
            {
                Interval = 10,
            };
            timerPannello.Tick += new System.EventHandler(TimerPannello);
            timer_FindPulsante = new Timer
            {
                Interval = 1000,
                Enabled = true,
            };
            timer_FindPulsante.Tick += new System.EventHandler(FindPulsante);
            timer_Saving = new Timer
            {
                Interval = 100,
                Enabled = true,
            };
            timer_Saving.Tick += Saving;

            Note.MouseHover += new EventHandler(StayNull);
            Note_txt.MouseHover += new EventHandler(StayNull);
            Giorno_txt.MouseHover += new EventHandler(StayNull);
            Plus.MouseEnter += new EventHandler(StayNull);
            Minus.MouseEnter += new EventHandler(StayNull);
            Trasferimento.MouseEnter += new EventHandler(StayNull);
            Go_Calendar.MouseEnter += new EventHandler(StayNull);
            Go_Calendar_txt.MouseHover += new EventHandler(StayNull);
            Tipi.MouseHover += new EventHandler(StayNull);

            Tipi.Click += new EventHandler(ClickNull);
            Note_txt.Click += new EventHandler(ClickNull);
            Giorno_txt.Click += new EventHandler(ClickNull);
            Attributo_txt.Click += new EventHandler(ClickNull);
            Go_Calendar.MouseClick += new MouseEventHandler(GoCalendar);

            Tipi.MouseClick += FinestraPrincipale.BackPanel.ClickNull;
            Note_txt.MouseClick += FinestraPrincipale.BackPanel.ClickNull;
            Giorno_txt.MouseClick += FinestraPrincipale.BackPanel.ClickNull;
            Note.MouseClick += FinestraPrincipale.BackPanel.ClickNull;
            Attributo_txt.MouseClick += FinestraPrincipale.BackPanel.ClickNull;
            Go_Calendar.MouseClick += FinestraPrincipale.BackPanel.ClickNull;
            Plus.MouseClick += FinestraPrincipale.BackPanel.ClickNull;
            Minus.MouseClick += FinestraPrincipale.BackPanel.ClickNull;
            Trasferimento.MouseClick += FinestraPrincipale.BackPanel.ClickNull;
            MouseClick += FinestraPrincipale.BackPanel.ClickNull;
            empty.MouseClick += FinestraPrincipale.BackPanel.ClickNull;

            Plus.MouseClick += new MouseEventHandler(EventoGuidato1);
            Minus.MouseClick += new MouseEventHandler(EventoGuidato2);
            Trasferimento.MouseClick += new MouseEventHandler(EventoGuidato3);
            Riferimento = new Label() { Visible = false, Location = new Point(20, 10) };

            FinestraPrincipale.BackPanel.Panel_Giorno.Focus();
            Timer timer_last = new Timer() { Enabled = true, Interval = 100,}; timer_last.Tick += (o, e) => {
                /*
                Panel_EventoGuidato.Resize_Guidato(true);
                Panel_EventoGuidato.Visible = true;
                Panel_EventoGuidato.Update();
                Panel_EventoGuidato.Visible = false;
                Panel_EventoGuidato.Update();
                */
                timer_last.Stop();

            };
            BringToFront();
        }
        
        public void Populate_Tipi()
        {
            Tipi.Hide();
            Tipi.Controls.Add(Riferimento);
            Tipi.Controls.Add(empty);
            Note.Text = "";
            time_to_save = false;
            pulsante_pending = false;
            attributo_selected = false;
            int i = 0;
            foreach (var evento in Pannello_StandardCalendar.eventi_giorno)
            {
                if (evento.Get_Attributo() == "Note")
                {
                    Note.Text = evento.GetAttributo(0).Replace(Savings.spazio, "\r\n");
                    continue;
                }
                if (evento.Get_Attributo() == "Introito" || evento.Get_Attributo() == "Spesa" || evento.Get_Attributo() == "Trasferimento")
                {
                    panelspeciale.Add(new Pulsante());
                    for (int j = 0; j < evento.GetAttributi().Count; j++) panelspeciale[i].attributi.Add(evento.GetAttributo(j));
                    panelspeciale[i].attributo = evento.Get_Attributo();
                    panelspeciale[i].SetImage(Associazione.AiconaAssociata(evento.Get_Attributo()), Associazione.IconaAssociata(evento.GetTipo()), Associazione.MiconaAssociata(evento.GetMetodo()));
                    panelspeciale[i].index = evento.index;
                    if (evento.Get_Attributo() == "Trasferimento") panelspeciale[i].TextTipo(evento.GetTipo() + "\u2192" + evento.GetMetodo()); else panelspeciale[i].TextTipo(evento.GetTipo());
                    panelspeciale[i].Text(Funzioni_utili.FormatoStandard(evento.GetValore()) + " \u20AC");
                    panelspeciale[i].valore = evento.GetValore();
                    panelspeciale[i].metodo = evento.GetMetodo();
                    panelspeciale[i].data = evento.GetData();
                    panelspeciale[i].data_modifica = evento.GetData_modifica();
                    panelspeciale[i].SetTooltip();
                    Tipi.Controls.Add(panelspeciale[i].panel_tipo);
                    i++;
                }
            }
            string data_note = "";
            data_note = Date.ShowDayDate(new int[] { 0, 0, 0, FinestraPrincipale.BackPanel.StandardCalendar.giorno, FinestraPrincipale.BackPanel.StandardCalendar.mese, FinestraPrincipale.BackPanel.StandardCalendar.anno, });
            Giorno_txt.Text = data_note;
        }

        public void ResizeGiorno(bool real= true)
        {
            Resize_Tipi();
            Update();
            attributo_selected = false;
            Location = FinestraPrincipale.BackPanel.StandardCalendar.Location;
            Size = new Size(FinestraPrincipale.BackPanel.StandardCalendar.Bounds.Width, FinestraPrincipale.BackPanel.StandardCalendar.Bounds.Height);

            Giorno_txt.Font = new System.Drawing.Font(BackPanel.font1, (int)(Tipi.Size.Width * 0.03 + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Giorno_txt.Location = new Point((int)(Tipi.Size.Width * 1.25), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.03));
            Giorno_txt.Size = new Size((int)(Width * 0.24), (int)(Height * 0.065));

            Attributo_txt.Location = new Point((int)(Tipi.Size.Width * 1.03), Giorno_txt.Location.Y + (int)(Giorno_txt.Size.Height * 1.7));
            Attributo_txt.Font = new System.Drawing.Font(BackPanel.font1, (int)(Tipi.Size.Width * 0.02 + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            Go_Calendar_txt.Font = new System.Drawing.Font(BackPanel.font1, (int)(Tipi.Size.Width * 0.02 + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Go_Calendar_txt.Location = new Point((int)(FinestraPrincipale.Finestra.Size.Width * 0.9 - 100), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.01));
            Go_Calendar_Piccolo();

            Plus_Piccolo();
            Minus_Piccolo();
            Trasferimento_Piccolo();

            taglia_piccola = new Size((int)(Convert.ToDouble(Width) * 0.02), (int)(Convert.ToDouble(Width) * 0.015));
            taglia_grande = new Size((int)(Convert.ToDouble(Width) * 0.03), (int)(Convert.ToDouble(Width) * 0.020));

            if (real)
            {
                button_precedente.Location = new Point(Giorno_txt.Location.X - (int)(Giorno_txt.Size.Width * 0.2) - taglia_piccola.Width, (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.045));
                button_successivo.Location = new Point(Giorno_txt.Location.X + (int)(Giorno_txt.Size.Width * 1.2), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.045));

                Button_precedente_MouseLeave(null, null);
                Button_successivo_MouseLeave(null, null);
            }

            AttributoPanel.Location = new Point((int)(Tipi.Size.Width * 1.03), (int)(Attributo_txt.Location.Y + Attributo_txt.Height));
            
            FinestraPrincipale.BackPanel.Panel_Giorno.empty.Location = new Point((int)(Tipi.Size.Width * 0.15), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.25));
            FinestraPrincipale.BackPanel.Panel_Giorno.empty.Font = new System.Drawing.Font(BackPanel.font2, (int)(Tipi.Size.Width * 0.1 + 15), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            Note.Location = new Point((int)(Tipi.Size.Width * 1.03), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.5));
            Note.Size = new Size((int)(FinestraPrincipale.Finestra.Bounds.Width * 0.98 - Note.Location.X), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.78 - Note.Location.Y));
            Note.Font = new System.Drawing.Font(BackPanel.font1, (int)(Tipi.Size.Width * 0.02 + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            Note_txt.Font = new System.Drawing.Font(BackPanel.font1, (int)(Tipi.Size.Width * 0.03 + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Note_txt.Location = new Point((int)(Tipi.Size.Width * 1.03), Note.Location.Y - (int)(Note_txt.Height * 1.2));
            if (Panel_EventoGuidato != null)
            {
                Panel_EventoGuidato.Location = new Point(Note.Location.X, 5);
                Panel_EventoGuidato.Size = new Size(Note.Location.X + Note.Width - Panel_EventoGuidato.Location.X, Note.Location.Y + Note.Height - Panel_EventoGuidato.Location.Y);
                Panel_EventoGuidato.Resize_Guidato(true);
            }

            if (Panel_NewEvento != null) Panel_NewEvento.Resize_NewEvento();
            SuspendLayout();
            FinestraPrincipale.BackPanel.Panel_Giorno.Visible = true;
            PanelMotore.ResizeForm();
            ResumeLayout();
        }

        public void Resize_Tipi()
        {
            Tipi.Size = new Size((int)(FinestraPrincipale.Finestra.Bounds.Width * 0.35), (int)(FinestraPrincipale.BackPanel.StandardCalendar.Height - FinestraPrincipale.BackPanel.Menù.Height * 1.5));
            PanelMotore.Location = new Point(Tipi.Location.X + (int)(Tipi.Width*0.27), Tipi.Location.Y);
            PanelMotore.Size = new Size((int)(Tipi.Width * 0.73) , (int)(Tipi.Height - PanelMotore.Location.Y) );
            panelspeciale = Pulsante.Order_datacode_datacode_modifica(panelspeciale);
            Riferimento.Location = new Point(20, 10 + Tipi.AutoScrollPosition.Y); Update();
            if (panelspeciale.Count == 0) { if (empty != null) empty.Visible = true; }
            else
            {
                empty.Hide();
                panelspeciale[0].SetSize(new Size((int)(Tipi.Bounds.Width * 0.86), (int)(Tipi.Bounds.Height * 0.14)));
                panelspeciale[0].SetLocation(new Point(20, Riferimento.Location.Y + 20));
                if (panelspeciale[0].attributo == "Trasferimento") panelspeciale[0].lab_tipo.Font = new System.Drawing.Font(BackPanel.font1, (int)(Tipi.Size.Width * 0.02 + 8), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                else panelspeciale[0].lab_tipo.Font = new System.Drawing.Font(BackPanel.font1, (int)(Tipi.Size.Width * 0.02 + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                panelspeciale[0].lab_totale.Font = new System.Drawing.Font(BackPanel.font1, (int)(Tipi.Size.Width * 0.02 + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                int j = 1;
                for (int i = 1; i < panelspeciale.Count; i++)
                {
                    panelspeciale[i].SetSize(new Size((int)(Tipi.Bounds.Width * 0.86), (int)(Tipi.Bounds.Height * 0.14)));
                    panelspeciale[i].SetLocation(new Point(20, panelspeciale[i - 1].panel_tipo.Location.Y + (int)(panelspeciale[i - 1].panel_tipo.Size.Height + 5)));
                    if (panelspeciale[i].attributo == "Trasferimento") panelspeciale[i].lab_tipo.Font = new System.Drawing.Font(BackPanel.font1, (int)(Tipi.Size.Width * 0.02 + 8), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    else panelspeciale[i].lab_tipo.Font = new System.Drawing.Font(BackPanel.font1, (int)(Tipi.Size.Width * 0.02 + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    panelspeciale[i].lab_totale.Font = new System.Drawing.Font(BackPanel.font1, (int)(Tipi.Size.Width * 0.02 + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    if (panelspeciale[i].lab_tipo.Text != "") j++;
                }
                // per sicurezza
                panelspeciale[0].SetLocation(new Point(20, Riferimento.Location.Y + 20));
                for (int i = 1; i < panelspeciale.Count; i++) panelspeciale[i].SetLocation(new Point(20, panelspeciale[i - 1].panel_tipo.Location.Y + (int)(panelspeciale[i - 1].panel_tipo.Size.Height + 5)));
            }
            Tipi.Show();
        }
        public void ClickNull(object sender, EventArgs e)
        {
            ClickNull();
        }
        public void ModificaEvento(object sender, EventArgs e)
        {
            ClickNull();
            pulsanti_selectable = false;
            foreach (Control control in FinestraPrincipale.BackPanel.Panel_Giorno.Controls)
            {
                if (control == Tipi) continue; else control.Visible = false;
            }
            Panel_NewEvento = new Panel_NewEvento();
            Controls.Add(Panel_NewEvento);
        }
        public void EventoGuidato1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) EventoGuidato("Introito");
        }
        public void EventoGuidato2(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) EventoGuidato("Spesa");
        }
        public void EventoGuidato3(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) EventoGuidato("Trasferimento");
        }
        public void EventoGuidato(string attributo)
        {
            ClickNull();
            Program.widget.ClickNull();
            Panel_NewEvento.active = false;
            pulsanti_selectable = false;
            foreach (Control pulsante in Tipi.Controls) { pulsante.Refresh();}
            foreach (Control control in FinestraPrincipale.BackPanel.Panel_Giorno.Controls)
            {
                if (control == Tipi) continue; else control.Visible = false;
            }
            //SuspendLayout();
            Panel_EventoGuidato.Pannello.attributo = attributo;
            if (attributo!="Trasferimento") Panel_EventoGuidato.attributo_img.BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject(Associazione.AiconaAssociata(attributo))));
            else Panel_EventoGuidato.attributo_img.BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("Trasferimento_dx")));
            Panel_EventoGuidato.Resize_Guidato(true);
            Panel_EventoGuidato.Visible = true;
            Panel_EventoGuidato.Update();
            //ResumeLayout();
        }

        public void StayNull(object sender, EventArgs e)
        {
            if (PanelMotore.Visible) return;
            foreach(Pulsante pulsante in FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale)
            {
                for(int i = 0; i<pulsante.textbox.Count; i++)
                {
                    if (pulsante.textbox[i].Focused || pulsante.bordo==true) return;
                }
                if (FinestraPrincipale.BackPanel.Panel_Giorno.Note.Focused) return;
            }
            FinestraPrincipale.BackPanel.Panel_Giorno.Attributo_txt.Visible = false;

            if (FinestraPrincipale.BackPanel.Panel_Impostazioni != null) if (FinestraPrincipale.BackPanel.Panel_Impostazioni.Visible == false) FinestraPrincipale.BackPanel.Focus();

            FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Visible = false;
            foreach (Pulsante pulsante in FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale)
            {
                pulsante.panel_tipo.BackColor = Color.AliceBlue; pulsante.bordo = false;
                pulsante.panel_tipo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                pulsante.delete.Visible = false;
            }
            FinestraPrincipale.BackPanel.Panel_Giorno.panel_corrente = -1;
            attributo_selected = false;
        }

        public void ClickNull()
        {
            Input.LoadAttributi();
            PanelMotore.HideMotore();
            Giorno_txt.Show();
            button_precedente.Show();
            button_successivo.Show();
            FinestraPrincipale.BackPanel.Panel_Giorno.pulsanti_selectable = true;
            if (Panel_NewEvento != null) { Controls.Remove(Panel_NewEvento); Panel_NewEvento.Disposer(); }
            Panel_NewEvento.active = false;
            FinestraPrincipale.BackPanel.Panel_Giorno.Attributo_txt.Visible = false;
            FinestraPrincipale.BackPanel.Focus();
            FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Visible = false;
            foreach (Pulsante pulsante in FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale) {
                pulsante.panel_tipo.BackColor = Color.AliceBlue; pulsante.bordo = false;
                pulsante.panel_tipo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                pulsante.delete.Visible = false;
            }
            FinestraPrincipale.BackPanel.Panel_Giorno.panel_corrente = -1;
            FinestraPrincipale.BackPanel.Panel_Giorno.panel_clicked = -1;
            FinestraPrincipale.BackPanel.Focus();
            attributo_selected = false;
        }

        private void Timer(object sender, EventArgs e)
        {
            if(AttributoPanel.Controls.Count/2 > 8) AttributoPanel.AutoScroll = true; else AttributoPanel.AutoScroll = false;
            int ausiliare = 0; 
            if (AttributoPanel.Controls.Count/2 > 8) ausiliare = 8; else ausiliare = AttributoPanel.Controls.Count/2;
            if(AttributoPanel.Controls.Count > 0) FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Size = new Size((int)(FinestraPrincipale.Finestra.Bounds.Width * 0.35), AttributoPanel.Controls[0].Height * ausiliare + 5);
            else FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Size = new Size((int)(FinestraPrincipale.Finestra.Bounds.Width * 0.35), 25 * ausiliare +5);
            int j = 0;
            foreach (Pulsante panel in panelspeciale)
            {
                if (panel.textbox.Count() > 0)
                {
                    foreach (TextBox txt in panel.textbox)
                    {
                        if (txt.Focused)
                        {
                            scrolled = false;
                            attributo_selected = true;
                        }
                    }
                }
                for (int i = 0; i < panel.textbox.Count(); i++)
                {
                    if (panel.textbox[i].Focused == false) panel.textbox[i].SelectionStart = 0;
                    if (panel.textbox[i].Focused) { current_textbox = i; j++; }
                    if (panel.textbox[i].Text == "" && i != panel.textbox.Count() - 1)
                    {
                        foreach (TextBox txt in AttributoPanel.Controls.OfType<TextBox>()) if (txt.Text == "" && txt != panel.textbox[i]) txt.Focus();
                        AttributoPanel.Controls.Remove(panel.Controllo[i]);
                        AttributoPanel.Controls.Remove(panel.textbox[i]);
                        panel.textbox.Remove(panel.textbox[i]);
                        panel.Controllo.Remove(panel.Controllo[i]);
                        
                        if (i == 0) panel.textbox[0].Location = new Point(4, 0);
                        if (i == 0) panel.Controllo[0].Location = new Point(4 + size_controllo * 2, (int)(panel.textbox[0].Height * 0.3));
                    }
                }
                panel.ResizeAttributi();
            }
            NewTextbox();
            if (Note.Text != "") Note_something = 1;
        }
        private void TimerPannello(object sender, EventArgs e)
        {
            Panel_EventoGuidato = new Panel_Guidato();
            Controls.Add(Panel_EventoGuidato);
            ResizeGiorno();
            timerPannello.Tick -= TimerPannello;
        }

        public void NewTextbox()
        {
            foreach (Pulsante pulsante in panelspeciale)
            {
                if (pulsante.textbox.Count == 0 || pulsante.textbox[pulsante.textbox.Count - 1].Text != "")
                {
                    pulsante.textbox.Insert(pulsante.textbox.Count, new TextBox() { Text = "", BorderStyle = System.Windows.Forms.BorderStyle.None, });
                    pulsante.Controllo.Insert(pulsante.Controllo.Count, new Label() { BackgroundImageLayout = ImageLayout.Stretch, Visible = false, });
                    FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Controls.Add(pulsante.textbox[pulsante.textbox.Count - 1]);
                    FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Controls.Add(pulsante.Controllo[pulsante.Controllo.Count - 1]);
                    pulsante.textbox[pulsante.textbox.Count - 1].Click += TextClick;
                    pulsante.textbox[pulsante.textbox.Count - 1].KeyDown += EnterAttributo;
                    pulsante.textbox[pulsante.textbox.Count - 1].KeyUp += pulsante.CheckAttributi;
                    if (pulsante.textbox.Count > 8 && scrolled == false) ScrollToBottom(FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel); scrolled = true;
                        if (pulsante.textbox.Count > 2)
                    {
                        if (pulsante.textbox[pulsante.textbox.Count - 2].Focused) ScrollToBottom(FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel);
                    }
                    pulsante.ResizeAttributi();
                    pulsante.CheckAllAttributi();
                }
            }
        }
        public void EnterAttributo(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down) { FinestraPrincipale.BackPanel.Panel_Giorno.PanelMotore.GoDown(); e.SuppressKeyPress = true;}
            if (e.KeyCode == Keys.Up) { FinestraPrincipale.BackPanel.Panel_Giorno.PanelMotore.GoUp(); e.SuppressKeyPress = true; }
            if (e.KeyCode == Keys.Enter)
            {
                EnterAtt();
                e.SuppressKeyPress = true;
            }
        }

        public void EnterAtt()
        {
            foreach (TextBox box in AttributoPanel.Controls.OfType<TextBox>())
            {
                if (box.Focused)
                {
                    if (MotoreRicerca.underline) box.Text = PanelMotore.GetText();
                }
            }
            NewTextbox();
            PanelMotore.SetTextbox("");
            foreach (Pulsante panel in panelspeciale) if (panel.index == panel_corrente) panel.CheckAllAttributi();
            foreach (TextBox textbox in AttributoPanel.Controls.OfType<TextBox>())
            {
                if (textbox.Text == nuovo_attributo) { textbox.Focus();  }
            }
        }
        public static void ScrollToBottom(Panel p)
        {
            using (Control c = new Control() { Parent = p, Dock = DockStyle.Bottom })
            {
                p.ScrollControlIntoView(c);
                c.Parent = null;
            }
        }
        public static void ScrollToTop(Panel p)
        {
            using (Control c = new Control() { Parent = p, Dock = DockStyle.Top })
            {
                p.ScrollControlIntoView(c);
                c.Parent = null;
            }
        }

        public void RefreshTipi()
        {
            foreach(Control ctrl in Tipi.Controls.OfType<Pulsante>()) Tipi.Controls.Remove(ctrl);
            foreach (Pulsante panel in panelspeciale) Tipi.Controls.Add(panel.panel_tipo);
        }

        private void GoCalendar(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) FinestraPrincipale.BackPanel.ClosePanel_Giorno();
        }

        private void Enter_Trasferimento(object sender, EventArgs e)
        {
            Trasferimento_Grande();
        }
        private void Leave_Trasferimento(object sender, EventArgs e)
        {
            Trasferimento_Piccolo();
        }
        private void Enter_Plus(object sender, EventArgs e)
        {
            Plus_Grande();
        }
        private void Leave_Plus(object sender, EventArgs e)
        {
            Plus_Piccolo();
        }
        private void Enter_Minus(object sender, EventArgs e)
        {
            Minus_Grande();
        }
        private void Leave_Minus(object sender, EventArgs e)
        {
            Minus_Piccolo();
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
        private void Trasferimento_Grande()
        {
            Trasferimento.Location = new Point(Trasferimento.Location.X - (int)(Trasferimento.Width * ((1.2 - 1) / 2)), Trasferimento.Location.Y - (int)(Trasferimento.Height * ((1.2 - 1) / 2)));
            Trasferimento.Size = new Size((int)(Trasferimento.Width * 1.2), (int)(Trasferimento.Height * 1.2));
        }
        private void Trasferimento_Piccolo()
        {
            Trasferimento.Size = new Size((int)(Minus.Location.X + Minus.Width - Plus.Location.X), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.08));
            Trasferimento.Location = new Point((int)(Plus.Location.X), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.38));
        }
        private void Plus_Grande()
        {
            SuspendLayout();
            Plus.Location = new Point(Plus.Location.X - (int)(Plus.Width * 0.1), Plus.Location.Y - (int)(Plus.Height * 0.1));
            Plus.Size = new Size((int)(Plus.Width * 1.2), (int)(Plus.Height * 1.2));
            ResumeLayout();
        }
        private void Plus_Piccolo()
        {
            SuspendLayout();
            Plus.Size = new Size((int)(FinestraPrincipale.Finestra.Bounds.Height * 0.09), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.09));
            Plus.Location = new Point(Go_Calendar_txt.Location.X + Go_Calendar_txt.Width/2 - (int)(Plus.Width * 2.5/2), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.25));
            ResumeLayout();
        }
        private void Minus_Grande()
        {
            Minus.Location = new Point(Minus.Location.X - (int)(Minus.Width * 0.1), Minus.Location.Y - (int)(Minus.Height * 0.1));
            Minus.Size = new Size((int)(Minus.Width * 1.2), (int)(Minus.Height * 1.2));
        }
        private void Minus_Piccolo()
        {
            Minus.Size = new Size((int)(FinestraPrincipale.Finestra.Bounds.Height * 0.09), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.09));
            Minus.Location = new Point(Plus.Location.X + (int)(Plus.Width * 1.3), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.25));
        }
        private void Go_Calendar_Grande()
        {
            Go_Calendar.Size = new Size((int)(FinestraPrincipale.Finestra.Bounds.Height * 0.12), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.12));
            Go_Calendar.Location = new Point(Go_Calendar_txt.Location.X + (Go_Calendar_txt.Width - Go_Calendar.Width) / 2, (int)(FinestraPrincipale.Finestra.Bounds.Height*0.12) - Go_Calendar.Height / 2);
        }
        private void Go_Calendar_Piccolo()
        {
            Go_Calendar.Size = new Size((int)(FinestraPrincipale.Finestra.Bounds.Height * 0.1), (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.1));
            Go_Calendar.Location = new Point(Go_Calendar_txt.Location.X + (Go_Calendar_txt.Width - Go_Calendar.Width) / 2, (int)(FinestraPrincipale.Finestra.Bounds.Height * 0.12) - Go_Calendar.Height / 2);
        }

        void Saving(object sender, EventArgs e)
        {
            if (time_to_save)
            {
                time_to_save = false;
                Savings.SaveGiorno();
                Savings.SaveEvents();
                Input.LoadAttributi();
                Pannello_StandardCalendar.resize = true;
            }
        }

        void FindPulsante(object sender, EventArgs e)
        {
            if (pulsante_pending == false) return;
            ClickNull();
            Input.RefreshData();
            int[] array = new int[FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale.Count + Input.eventi.Count()];
            for (int i = 0; i < FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale.Count; i++) array[i] = FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale[i].index; int conteggio = FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale.Count;
            for (int i = 0; i < Input.eventi.Count; i++) array[i + conteggio] = Input.eventi[i].index;
            if (FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale.Count + Input.eventi.Count() == 0) array = new int[] { 1 };

            Pulsante pulsante = new Pulsante
            {
                attributo = Widget_PanelFinale.attributo,
                tipo = Widget_PanelFinale.tipo,
                index = array.Max() + 1,
                metodo = Widget_PanelFinale.metodo,
                valore = Widget_PanelFinale.valore,
                attributi = Widget_PanelFinale.textBox,
                data = new int[] { 0, Widget_PanelFinale.minuto, Widget_PanelFinale.ora, Widget_PanelFinale.giorno, Widget_PanelFinale.mese, Widget_PanelFinale.anno },
                data_modifica = Input.data_attuale,
                new_evento = true,
            };
            if (Widget_PanelFinale.attributo == "Trasferimento") pulsante.tipo = Widget_PanelFinale.tipo + "\u2192" + Widget_PanelFinale.metodo;
            FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale.Add(pulsante);
            pulsante.SetImage(Associazione.AiconaAssociata(pulsante.attributo), Associazione.IconaAssociata(pulsante.tipo), Associazione.MiconaAssociata(pulsante.metodo));
            pulsante.TextTipo(pulsante.tipo);
            pulsante.Text(Funzioni_utili.FormatoStandard(pulsante.valore) + "\u20AC");
            pulsante.SetTooltip();
            FinestraPrincipale.BackPanel.Panel_Giorno.RefreshTipi();
            FinestraPrincipale.BackPanel.Panel_Giorno.ResizeGiorno();
            FinestraPrincipale.BackPanel.Panel_Giorno.empty.Visible = false;
            pulsante_pending = false;
            ProprietàGiorno.time_to_save = true;
        }
        public void TextClick(object sender, EventArgs e)
        {
            foreach (TextBox txt in FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Controls.OfType<TextBox>())
                if (txt.Focused)
                {
                    PanelMotore.RefreshForm(Pulsante.tipo_statico, Pulsante.attributo_statico, true);
                    FinestraPrincipale.BackPanel.Panel_Giorno.PanelMotore.SetTextbox(txt.Text);
                }
            PanelMotore.BringToFront();
        }





        private void Button_precedente_Click(object sender, MouseEventArgs e)
        {
            if (FinestraPrincipale.BackPanel.altriconti) return;
            if (e.Button == MouseButtons.Left)
            {
                FinestraPrincipale.BackPanel.Move_toDay("previous");
                button_precedente.Size = taglia_grande;
            }
        }

        private void Button_successivo_Click(object sender, MouseEventArgs e)
        {
            if(FinestraPrincipale.BackPanel.altriconti) return;
            if (e.Button == MouseButtons.Left)
            {
                FinestraPrincipale.BackPanel.Move_toDay("next");
                button_successivo.Size = taglia_grande;
            }
        }
        private void Button_precedente_MouseLeave(object sender, EventArgs e)
        {
            if (FinestraPrincipale.BackPanel.altriconti) return;
            button_precedente.Size = taglia_piccola;
            button_precedente.Location = new Point(button_precedente.Location.X + (taglia_grande.Width - taglia_piccola.Width), button_precedente.Location.Y + (taglia_grande.Height - taglia_piccola.Height) / 2);
        }

        private void Button_precedente_MouseEnter(object sender, EventArgs e)
        {
            if (FinestraPrincipale.BackPanel.altriconti) return;
            button_precedente.Size = taglia_grande;
            button_precedente.Location = new Point(button_precedente.Location.X - (taglia_grande.Width - taglia_piccola.Width), button_precedente.Location.Y - (taglia_grande.Height - taglia_piccola.Height) / 2);
        }

        private void Button_successivo_MouseLeave(object sender, EventArgs e)
        {
            if (FinestraPrincipale.BackPanel.altriconti) return;
            button_successivo.Size = taglia_piccola;
            button_successivo.Location = new Point(button_successivo.Location.X, button_successivo.Location.Y + (taglia_grande.Height - taglia_piccola.Height)/2);
        }


        private void Button_successivo_MouseEnter(object sender, EventArgs e)
        {
            if (FinestraPrincipale.BackPanel.altriconti) return;
            button_successivo.Size = taglia_grande;
            button_successivo.Location = new Point(button_successivo.Location.X, button_successivo.Location.Y - (taglia_grande.Height - taglia_piccola.Height) / 2);
        }
    }
}
