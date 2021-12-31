using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1;

namespace Moneyguard
{
    public class Panello_Guidato_scelta : Panel
    {
        public List<Visual_FakeTipi> VisualFakeTipi = new List<Visual_FakeTipi>();
        public List<Visual_Tipi> VisualTipi = new List<Visual_Tipi>();
        public List<Visual_Metodi> VisualMetodi = new List<Visual_Metodi>();
        private readonly int tipi_width = 100, metodi_width = 200;
        public string attributo;
        public int passaggio = 1;
        public string tipo="";
        public string metodo="";
        private double valore;
        private Timer timer, timerexception;
        private bool readytolastelements = false;
        private double correzione = 0;

        private Label Valore_img;
        private Label Punto;
        private Label Euro;
        public TextBox Unità;
        private TextBox Centesimi;
        private Label Attributo_txt;
        public Label True, True_Opaque;
        private Label False;
        public bool noexception = false;

        static private bool enterable = false;

        public PanelAttributi AttributoPanel;
        private DateTimePicker TimePicker = new System.Windows.Forms.DateTimePicker();
        private DateTimePicker DatePicker = new System.Windows.Forms.DateTimePicker();

        ToolTip tooltip;

        public void Disposer()
        {
            timer.Dispose();
            timerexception.Dispose();
            foreach (Visual_Tipi tip in VisualTipi) { tip.Disposer(); Controls.Remove(tip); }
            foreach (Visual_Metodi tip in VisualMetodi) { tip.Disposer(); Controls.Remove(tip); }
            Valore_img.BackgroundImage.Dispose();
            Valore_img.Dispose();
            Punto.Dispose();
            Euro.Dispose();
            Unità.Dispose();
            Centesimi.Dispose();
            Attributo_txt.Dispose();
            True.BackgroundImage.Dispose();
            True_Opaque.BackgroundImage.Dispose();
            False.BackgroundImage.Dispose();
            True.Dispose();
            True_Opaque.Dispose();
            False.Dispose();
            AttributoPanel.Disposer();
            TimePicker.Dispose();
            DatePicker.Dispose();
            tooltip.Dispose();
            Dispose();
        }

        public Panello_Guidato_scelta()
        {
            DoubleBuffered = true;
            BorderStyle = BorderStyle.FixedSingle;
            AutoScroll = true;
            tooltip = new ToolTip() { AutoPopDelay = 20000,};
            int i = 0;
            foreach (int it in Input.tipi_sort)
            {
                VisualTipi.Add(new Visual_Tipi(Input.tipi[it], Associazione.IconaAssociata(Input.tipi[it])));
                Controls.Add(VisualTipi[i]);
                if (Input.tipi[it] == Input.tipi[0]) VisualTipi[i].Location = new Point(0, 0);
                i++;
            }
            i = 0;
            foreach (int it in Input.metodi_sort)
            {
                VisualFakeTipi.Add(new Visual_FakeTipi(Input.metodi[it], Associazione.MiconaAssociata(Input.metodi[it])));
                Controls.Add(VisualFakeTipi[i]);
                if (Input.metodi[it] == Input.metodi[0]) VisualFakeTipi[i].Location = new Point(0, 0);
                i++;
            }
            i = 0;
            foreach (int it in Input.metodi_sort)
            {
                VisualMetodi.Add(new Visual_Metodi(Input.metodi[it], Associazione.MiconaAssociata(Input.metodi[it])));
                Controls.Add(VisualMetodi[i]);
                if (Input.metodi[it] == Input.metodi[0]) VisualMetodi[i].Location = new Point(0, 0);
                i++;
            }
            CreateLastElements();

            timerexception = new Timer
            {
                Enabled = true,
                Interval = 100,
            };
            timerexception.Tick += new System.EventHandler(Timerexception);

            timer = new Timer
            {
                Enabled = true,
                Interval = 10,
            };
            timer.Tick += new System.EventHandler(Tick);

            Click += new EventHandler(ClickNull);
            MouseEnter += new EventHandler(MouseEntered);
        }
        
        public void ResizeForm(bool initial)
        {
            SuspendLayout();
            if (passaggio == 0) { FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Close(); readytolastelements = false; }
             int num_colonne = Width / tipi_width, brek = 0;
            for (int j = 0; ; j++)
            {
                if (j * num_colonne == VisualTipi.Count() || brek == 1) break;
                if (j != 0)
                {
                    VisualTipi[j * num_colonne].Location = new Point(VisualTipi[(j - 1) * num_colonne].Location.X, VisualTipi[(j - 1) * num_colonne].Location.Y + VisualTipi[(j - 1) * num_colonne].Height);
                    VisualTipi[j * num_colonne].SetSize(new Size((Width - 20) / num_colonne, (Width - 20) / num_colonne), 2);
                }
                else
                {
                    VisualTipi[j * num_colonne].SetSize(new Size((Width - 20) / num_colonne, (Width - 20) / num_colonne), 2);
                }
                for (int i = 1; i < num_colonne; i++)
                {
                    if (i + j * num_colonne == VisualTipi.Count()) { brek = 1; break; }
                    VisualTipi[i + j * num_colonne].SetSize(new Size((Width - 20) / num_colonne, (Width - 20) / num_colonne), 2);
                    VisualTipi[i + j * num_colonne].Location = new Point(VisualTipi[i + j * num_colonne - 1].Location.X + VisualTipi[i + j * num_colonne - 1].Width, VisualTipi[i + j * num_colonne - 1].Location.Y);
                    if (passaggio == 1 && attributo != "Trasferimento") VisualTipi[i + j * num_colonne].Visible = true; else VisualTipi[i + j * num_colonne].Visible = false;
                }
                try { if (passaggio == 1 && attributo != "Trasferimento") { VisualTipi[j * num_colonne].Visible = true; tipo = ""; metodo = ""; FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.metodo_img.BackgroundImage = null; readytolastelements = false; } else VisualTipi[j * num_colonne].Visible = false; } catch (Exception) { }
            }
            for (int i = 0; i < VisualTipi.Count; i++) VisualTipi[i].index = i;
            brek = 0; num_colonne = Width / metodi_width;
            for (int j = 0; ; j++)
            {
                if (j * num_colonne == VisualFakeTipi.Count() || brek == 1) break;
                if (j != 0)
                {
                    VisualFakeTipi[j * num_colonne].Location = new Point(VisualFakeTipi[(j - 1) * num_colonne].Location.X, VisualFakeTipi[(j - 1) * num_colonne].Location.Y + VisualFakeTipi[(j - 1) * num_colonne].Height);
                    VisualFakeTipi[j * num_colonne].SetSize(new Size((Width - 20) / num_colonne, (Width - 20) / num_colonne), 2);
                }
                else
                {
                    VisualFakeTipi[j * num_colonne].SetSize(new Size((Width - 20) / num_colonne, (Width - 20) / num_colonne), 2);
                }
                for (int i = 1; i < num_colonne; i++)
                {
                    if (i + j * num_colonne == VisualFakeTipi.Count()) { brek = 1; break; }
                    VisualFakeTipi[i + j * num_colonne].SetSize(new Size((Width - 20) / num_colonne, (Width - 20) / num_colonne), 2);
                    VisualFakeTipi[i + j * num_colonne].Location = new Point(VisualFakeTipi[i + j * num_colonne - 1].Location.X + VisualFakeTipi[i + j * num_colonne - 1].Width, VisualFakeTipi[i + j * num_colonne - 1].Location.Y);
                    if (passaggio == 1 && attributo == "Trasferimento") VisualFakeTipi[i + j * num_colonne].Visible = true; else VisualFakeTipi[i + j * num_colonne].Visible = false;
                }
                if (passaggio == 1 && attributo == "Trasferimento") { VisualFakeTipi[j * num_colonne].Visible = true; tipo = ""; metodo = ""; FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.faketipo_img.Visible = false;  FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.metodo_img.BackgroundImage = null; readytolastelements = false; } else VisualFakeTipi[j * num_colonne].Visible = false;
            }
            for (int i = 0; i < VisualFakeTipi.Count; i++) VisualFakeTipi[i].index = i;
            brek = 0; num_colonne = Width / metodi_width;
            for (int j = 0; ; j++)
            {
                if (j * num_colonne == VisualMetodi.Count() || brek == 1) break;
                if (j != 0)
                {
                    VisualMetodi[j * num_colonne].Location = new Point(VisualMetodi[(j - 1) * num_colonne].Location.X, VisualMetodi[(j - 1) * num_colonne].Location.Y + VisualMetodi[(j - 1) * num_colonne].Height);
                    VisualMetodi[j * num_colonne].SetSize(new Size((Width - 20) / num_colonne, (Width - 20) / num_colonne), 2);
                }
                else
                {
                    VisualMetodi[j * num_colonne].SetSize(new Size((Width - 20) / num_colonne, (Width - 20) / num_colonne), 2);
                }
                for (int i = 1; i < num_colonne; i++)
                {
                    if (i + j * num_colonne == VisualMetodi.Count()) { brek = 1; break; }
                    VisualMetodi[i + j * num_colonne].SetSize(new Size((Width - 20) / num_colonne, (Width - 20) / num_colonne), 2);
                    VisualMetodi[i + j * num_colonne].Location = new Point(VisualMetodi[i + j * num_colonne - 1].Location.X + VisualMetodi[i + j * num_colonne - 1].Width, VisualMetodi[i + j * num_colonne - 1].Location.Y);
                    if (passaggio == 2) VisualMetodi[i + j * num_colonne].Visible = true; else VisualMetodi[i + j * num_colonne].Visible = false;
                }
                if (passaggio == 2) {VisualMetodi[j * num_colonne].Visible = true; metodo = ""; FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.metodo_img.BackgroundImage = null; readytolastelements = true; } else VisualMetodi[j * num_colonne].Visible = false;
            }
            for (int i = 0; i < VisualMetodi.Count; i++) VisualMetodi[i].index = i;
            ResizeLastElements(initial);
            if (attributo == "Trasferimento") if (passaggio >1) FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.attributo_img.Visible = true; else FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.attributo_img.Visible = false;
            string tipo_txt = ""; if (tipo != "") tipo_txt = "\u2192 " + tipo;
            string metodo_txt = ""; if (metodo != "") metodo_txt = " \u2192  Metodo: ";
            try
            {
                if (attributo != "Trasferimento")
                {
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Tipo.Text = "  " + tipo_txt;
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Metodo.Text = metodo_txt;
                }
            }
            catch (Exception) { }
            ResumeLayout();
        }

        public void CreateLastElements()
        {
            Valore_img = new Label()
            {
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("Cassa"))),
                BackgroundImageLayout = ImageLayout.Stretch,
                Visible = false,
            };
            Controls.Add(Valore_img);
            Punto = new Label()
            {
                Text = ",",
                AutoSize = true,
                Visible = false,
            };
            Controls.Add(Punto);
            Euro = new Label()
            {
                Text = "\u20AC",
                AutoSize = true,
                Visible = false,
            };
            Controls.Add(Euro);
            Unità = new TextBox()
            {
                Text = "",
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = HorizontalAlignment.Right,
                Visible = false,
                TabIndex=0,
            };
            Controls.Add(Unità);
            Unità.KeyDown += new KeyEventHandler(PressEnter);
            Unità.KeyUp += new KeyEventHandler(PressVirgola);
            Centesimi = new TextBox()
            {
                Text = "",
                BorderStyle = BorderStyle.FixedSingle,
                Visible = false,
                TabIndex=1,
            };
            Controls.Add(Centesimi);
            Centesimi.KeyDown += new KeyEventHandler(PressEnter);

            AttributoPanel = new PanelAttributi();

            Controls.Add(AttributoPanel);

            Attributo_txt = new Label()
            {
                Text = "Attributi:",
                AutoSize = true,
            };
            Controls.Add(Attributo_txt);
            tooltip.SetToolTip(Attributo_txt, ProprietàGiorno.attributi_txt);

            TimePicker = new DateTimePicker()
            {
                Format = DateTimePickerFormat.Custom,
                ShowUpDown = true,
                TabIndex=2,
                //TabStop = false,
            };
            TimePicker.CustomFormat = "HH:mm";
            Controls.Add(TimePicker);
            TimePicker.KeyDown += new KeyEventHandler(PressEnter);
            DatePicker = new DateTimePicker()
            {
                Format = DateTimePickerFormat.Custom,
                TabStop = false,
            };
            DatePicker.CustomFormat = " dddd  dd / MM / yyyy";
            Controls.Add(DatePicker);
            DatePicker.KeyDown += new KeyEventHandler(PressEnter);

            True_Opaque = new Label()
            {
                Visible = false,
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("TrueOpaque"))),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(True_Opaque);
            True = new Label()
            {
                Visible = false,
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("True"))),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(True);
            False = new Label()
            {
                Visible = false,
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("False"))),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(False);

            True.MouseClick += new MouseEventHandler(Click_True);
            False.MouseClick += new MouseEventHandler(Click_False);
            False.MouseEnter += new EventHandler(Enter_False);
            False.MouseLeave += new EventHandler(Leave_False);
            True.MouseEnter += new EventHandler(Enter_True);
            True.MouseLeave += new EventHandler(Leave_True);

            AttributoPanel.Click += new EventHandler(ClickNull);
            Valore_img.Click += new EventHandler(ClickNull);
            Attributo_txt.Click += new EventHandler(ClickNull);
            Euro.Click += new EventHandler(ClickNull);
            Punto.Click += new EventHandler(ClickNull);

            Unità.GotFocus += AttributoPanel.HideRicerca;
            Centesimi.GotFocus += AttributoPanel.HideRicerca;
            DatePicker.GotFocus += AttributoPanel.HideRicerca;
            TimePicker.GotFocus += AttributoPanel.HideRicerca;
        }

        private void ResizeLastElements(bool initial)
        {
            if (passaggio == 3)
            {
                if (readytolastelements)
                {
                    Input.RefreshData();
                    Input.LoadAttributi();
                    if (FinestraPrincipale.BackPanel.StandardCalendar.giorno == Input.data_utile[3] && FinestraPrincipale.BackPanel.StandardCalendar.mese == Input.data_utile[4] && FinestraPrincipale.BackPanel.StandardCalendar.anno == Input.data_utile[5])
                    {
                        TimePicker.Value = new DateTime(Input.data_utile[5], Input.data_utile[4], Input.data_utile[3], Input.data_utile[2], Input.data_utile[1], Input.data_utile[0]);
                    }
                    else
                    {
                        TimePicker.Value = new DateTime(FinestraPrincipale.BackPanel.StandardCalendar.anno, FinestraPrincipale.BackPanel.StandardCalendar.mese, FinestraPrincipale.BackPanel.StandardCalendar.giorno, 12, 0, 0);
                    }
                    DatePicker.Value = TimePicker.Value;

                    AttributoPanel.textbox.Clear();
                    AttributoPanel.Controllo.Clear();
                    AttributoPanel.Controls.Clear();
                    AttributoPanel.SetPanel(initial);
                    string arg1; bool arg2;
                    if (attributo == "Trasferimento") arg2 = true; else arg2 = false;
                    arg1 = Funzioni_utili.Scremato(tipo);
                    AttributoPanel.SetArgs(arg1, arg2);

                    Unità.Clear(); Centesimi.Clear();
                    readytolastelements = false;
                }
                Valore_img.Visible = true;
                Punto.Visible = true;
                Euro.Visible = true;
                Unità.Visible = true;
                Centesimi.Visible = true;
                AttributoPanel.Visible = true;
                Attributo_txt.Visible = true;
                TimePicker.Visible = true;
                DatePicker.Visible = true;
                False.Visible = true;
                True_Opaque.Visible = true;
                AutoScroll = false;
            }
            else
            {
                AutoScroll = true;
                Valore_img.Visible = false;
                Punto.Visible = false;
                Euro.Visible = false;
                Unità.Visible = false;
                Centesimi.Visible = false;
                AttributoPanel.Visible = false;
                Attributo_txt.Visible = false;
                TimePicker.Visible = false;
                DatePicker.Visible = false;
                True.Visible = false;
                True_Opaque.Visible = false;
                False.Visible = false;
                AttributoPanel.timer.Tick -= AttributoPanel.Timer;
            }

            correzione = ((double)FinestraPrincipale.Finestra.Width / (double)FinestraPrincipale.Finestra.Height - (double)FinestraPrincipale.minimum_weight / (double)FinestraPrincipale.minimum_height) * 35;
            True_Piccolo(); False_Piccolo(); TrueOpaque();

            Valore_img.Location = new Point((int)(Width * 0.15), (int)(Height * 0.05));
            Valore_img.Size = new Size((int)(Width * 0.05 + 20), (int)(Width * 0.05 + 20));

            Punto.Font = new System.Drawing.Font(BackPanel.font1, (int)(Width * 0.02 + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Euro.Font = new System.Drawing.Font(BackPanel.font1, (int)(Width * 0.02 + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Unità.Font = new System.Drawing.Font(BackPanel.font1, (int)(Width * 0.02 + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Centesimi.Font = new System.Drawing.Font(BackPanel.font1, (int)(Width * 0.02 + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            Unità.Location = new Point(Valore_img.Location.X + Valore_img.Width * 3 / 2, Valore_img.Location.Y + 7); Unità.Size = new Size(Valore_img.Height * 2, Valore_img.Height);
            Punto.Location = new Point(Unità.Location.X + Unità.Width, Unità.Location.Y + 10); Punto.Height = Unità.Height;
            Centesimi.Location = new Point(Unità.Location.X + Unità.Width + Punto.Width, Valore_img.Location.Y + 7); Centesimi.Size = new Size(Valore_img.Height * 1, Valore_img.Height);
            Euro.Location = new Point(Centesimi.Location.X + Centesimi.Width, Unità.Location.Y + 7); Euro.Height = Unità.Height;

            DatePicker.Font = new System.Drawing.Font(BackPanel.font3, (int)(Width * 0.030 - 2), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            TimePicker.Font = new System.Drawing.Font(BackPanel.font1, (int)(Width * 0.05 + 5), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            DatePicker.Location = new System.Drawing.Point(20, Valore_img.Location.Y + (int)(Valore_img.Height * 1.5));
            DatePicker.Size = new System.Drawing.Size((int)(Width * 0.58 ), 50);

            TimePicker.ShowUpDown = false;
            TimePicker.Size = new System.Drawing.Size((int)(Width * 0.18 + 50), 50);
            TimePicker.Location = new System.Drawing.Point(Width - 20 - TimePicker.Width, DatePicker.Location.Y + (DatePicker.Height - TimePicker.Height) / 2);
            TimePicker.ShowUpDown = true;

            Attributo_txt.Location = new Point(20, DatePicker.Location.Y + (int)(DatePicker.Height * 1.5));
            Attributo_txt.Font = new System.Drawing.Font(BackPanel.font1, (int)(Width * 0.02 + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            AttributoPanel.Location = new Point (20 , Attributo_txt.Location.Y + (int)(Attributo_txt.Height * 1.0));
            AttributoPanel.Size = new Size(Width - AttributoPanel.Location.X - 20, Height - (int)(Width * buttonSize) - AttributoPanel.Location.Y -10);
            
            AttributoPanel.ResizeAttributi();
        }

        public void ClickNull(object sender, EventArgs e)
        {
            FinestraPrincipale.BackPanel.Focus();
            FinestraPrincipale.BackPanel.Panel_Giorno.PanelMotore.HideMotore();
        }

        private void Click_False(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Click_False();
            }
        }
        public void Click_False()
        {
            if(FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato != null) FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Close();
            passaggio = 1;
        }
        private void Click_True(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) Click_True();
        }

        public void Click_True()
        {
            if (enterable == false) return;
            Tick(null, null);
            try { if (Impostazioni.mute == false) if (attributo == "Introito") Impostazioni.yes_snd.Play(); else if (attributo == "Spesa") Impostazioni.no_snd.Play(); else if (attributo == "Trasferimento") Impostazioni.gnè_snd.Play(); } catch (Exception) { Console.WriteLine("Errore Sound"); };
            bool CentesimiNumeric = int.TryParse(Centesimi.Text, out int n); if (Centesimi.Text == "") { n = 0; CentesimiNumeric = true; }
            bool UnitàNumeric = int.TryParse(Unità.Text, out int m); if (Unità.Text == "") { m = 0; UnitàNumeric = true; }
            if (Centesimi.Text == "" && Unità.Text == "") UnitàNumeric = false;
            if (UnitàNumeric && CentesimiNumeric == false) { return; }
            if (True.Visible == false) return;

            FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Close();
            passaggio = 1;

            Input.RefreshData();
            int[] array = new int[FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale.Count + Input.eventi.Count()];
            for (int i = 0; i < FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale.Count; i++) array[i] = FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale[i].index;  int conteggio = FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale.Count;
            for (int i = 0; i < Input.eventi.Count; i++) array[i + conteggio] = Input.eventi[i].index;
            if (FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale.Count + Input.eventi.Count() == 0) array = new int[] { 0 };

            Pulsante pulsante = new Pulsante
            {
                attributo = attributo,
                tipo = tipo,
                index = array.Max()+1,
                metodo = metodo,
                valore = valore,
                data = new int[] { 0, TimePicker.Value.Minute, TimePicker.Value.Hour, DatePicker.Value.Day, DatePicker.Value.Month, DatePicker.Value.Year },
                data_modifica = Input.data_attuale,
                new_evento = true,
            };
            if (attributo == "Trasferimento") pulsante.tipo = tipo + "\u2192" + metodo;
            foreach (TextBox txt in AttributoPanel.textbox) { if (txt.Text != AttributoPanel.nuovo_attributo && Funzioni_utili.Scremato(txt.Text) != "") pulsante.attributi.Add(Funzioni_utili.Scremato(txt.Text));}
            if (DatePicker.Value.Day == FinestraPrincipale.BackPanel.StandardCalendar.giorno && DatePicker.Value.Month == FinestraPrincipale.BackPanel.StandardCalendar.mese && DatePicker.Value.Year == FinestraPrincipale.BackPanel.StandardCalendar.anno) FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale.Add(pulsante);
            else { pulsante.SaveEvento(); }
            pulsante.SetImage(Associazione.AiconaAssociata(pulsante.attributo), Associazione.IconaAssociata(pulsante.tipo), Associazione.MiconaAssociata(pulsante.metodo));
            pulsante.TextTipo(pulsante.tipo);
            pulsante.Text(Funzioni_utili.FormatoStandard(pulsante.valore) + "\u20AC");
            pulsante.SetTooltip();
            FinestraPrincipale.BackPanel.Panel_Giorno.empty.Visible = false;

            FinestraPrincipale.BackPanel.Panel_Giorno.RefreshTipi();
            FinestraPrincipale.BackPanel.Panel_Giorno.ResizeGiorno();
            ProprietàGiorno.time_to_save = true;
        }



        private void Enter_True(object sender, EventArgs e)
        {
            True_Grande();
        }
        private void Leave_True(object sender, EventArgs e)
        {
            True_Piccolo();
        }
        private void Enter_False(object sender, EventArgs e)
        {
            False_Grande();
        }
        private void Leave_False(object sender, EventArgs e)
        {
            False_Piccolo();
        }

        private readonly double buttonSize = 0.1;
        private readonly double buttonsize = 0.07;
        private readonly double truewidth = 0.8;
        private readonly double buttonsheight = 0.92;
        private readonly double falsewidth = 0.9;
        private void True_Grande()
        {
            True.Size = new Size((int)(Width * buttonSize), (int)(Width * buttonSize));
            True.Location = new Point((int)(Width * truewidth) - True.Width / 2, (int)(Height * buttonsheight) - True.Height / 2 - (int)correzione);
        }
        private void True_Piccolo()
        {
            True.Size = new Size((int)(Width * buttonsize), (int)(Width * buttonsize));
            True.Location = new Point((int)(Width * truewidth) - True.Width / 2, (int)(Height * buttonsheight) - True.Height / 2 - (int)correzione) ;
        }
        private void TrueOpaque()
        {
            True_Opaque.Size = new Size((int)(Width * buttonsize), (int)(Width * buttonsize));
            True_Opaque.Location = new Point((int)(Width * truewidth) - True.Width / 2, (int)(Height * buttonsheight) - True.Height / 2 - (int)correzione);
        }
        private void False_Grande()
        {
            False.Size = new Size((int)(Width * buttonSize), (int)(Width * buttonSize));
            False.Location = new Point((int)(Width * falsewidth) - False.Width / 2, (int)(Height * buttonsheight) - False.Height / 2 - (int)correzione);
        }
        private void False_Piccolo()
        {
            False.Size = new Size((int)(Width * buttonsize), (int)(Width * buttonsize));
            False.Location = new Point((int)(Width * falsewidth) - False.Width / 2, (int)(Height * buttonsheight) - False.Height / 2 - (int)correzione);
        }
        private void Tick(object sender, EventArgs e)
        {
            bool CentesimiNumeric = uint.TryParse(Centesimi.Text, out uint n); if (Centesimi.Text == "") { n = 0; CentesimiNumeric = true; }
            bool UnitàNumeric = uint.TryParse(Unità.Text, out uint m); if (Unità.Text == "") { m = 0; UnitàNumeric = true; }
            if (Centesimi.Text == "" && Unità.Text == "") UnitàNumeric = false;
            if (UnitàNumeric && CentesimiNumeric && passaggio == 3) { valore = Convert.ToDouble(m) + Funzioni_utili.SetCentesimi(Centesimi.Text); if (valore != 0) { True.Show(); enterable = true; True_Opaque.Hide(); } else { True.Hide(); enterable = false; True_Opaque.Show(); } }
                else { True.Hide(); enterable = false; if(passaggio==3) True_Opaque.Show(); }
        }

        private void PressEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Tick(null, null);
                if(True.Visible) Click_True();
                e.SuppressKeyPress = true;
            }
        }
        private void PressVirgola(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Decimal || e.KeyCode == Keys.Oemcomma || e.KeyCode == Keys.OemPeriod)
            {
                Centesimi.Focus();

                for (int i = 0; i < Unità.Text.Length; i++)
                {
                    if (Unità.Text.Substring(i, 1) == "." || Unità.Text.Substring(i, 1) == ",") Unità.Text = Unità.Text.Substring(0, i);
                }
            }
        }

        private void MouseEntered(object sender, EventArgs e)
        {
            Visual_Tipi.Index = -1;
            Visual_FakeTipi.Index = -1;
            Visual_Metodi.Index = -1;
            foreach (Visual_Tipi tip in VisualTipi) tip.BordoHide();
            foreach (Visual_FakeTipi tip in VisualFakeTipi) tip.BordoHide();
            foreach (Visual_Metodi tip in VisualMetodi) tip.BordoHide();
        }

        private void Timerexception(object sender, EventArgs e)
        {
            if (noexception == false) return;
            noexception = false;
            if(Visible) Click_False();
        }
    }
}
