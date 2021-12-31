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
    public class Widget_PanelFinale : Panel
    {
        public DateTimePicker TimePicker;
        public DateTimePicker DatePicker;
        private Label True;
        private Label Valore_img;
        public Label Punto;
        public Label Euro;
        public TextBox Unità;
        public TextBox Centesimi;
        public Panel AttributoPanel;
        public List<TextBox> textbox;
        public List<Label> Controllo;
        Timer timer;
        public string nuovo_attributo;
        private double val;
        static public string attributo;
        static public string tipo;
        static public string metodo;
        static public double valore;
        static public List<string> textBox;
        static public int minuto, ora, giorno, mese, anno;
        private readonly int size_controllo = 15;
        private readonly Bitmap vero;
        private readonly Bitmap falso;
        ToolTip toolTip;

        public Widget_PanelFinale()
        {
            BackColor = WidgetMoneyguard.transparent;
            True = new Label()
            {
                Visible = false,
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("True"))),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(True);
            True.MouseClick += ClickTrue;




            toolTip = new ToolTip();
            vero = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("True")));
            falso = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("False")));

            nuovo_attributo = "  \u2192 Nuovo \u2190  ";
            TimePicker = new System.Windows.Forms.DateTimePicker();
            DatePicker = new System.Windows.Forms.DateTimePicker();
            textbox = new List<TextBox>();
            textBox = new List<string>();
            Controllo = new List<Label>();
            True.MouseEnter += new EventHandler(Enter_True);
            True.MouseLeave += new EventHandler(Leave_True);

            Valore_img = new Label()
            {
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("Cassa"))),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(Valore_img);
            Punto = new Label()
            {
                Text = ",",
                AutoSize = true,
                ForeColor = WidgetMoneyguard.textcolor,
            };
            Controls.Add(Punto);
            Euro = new Label()
            {
                Text = "\u20AC",
                AutoSize = true,
                ForeColor = WidgetMoneyguard.textcolor,
            };
            Controls.Add(Euro);
            Unità = new TextBox()
            {
                Text = "",
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = HorizontalAlignment.Right,
                BackColor = WidgetMoneyguard.backcolor,
                ForeColor = WidgetMoneyguard.textcolor,
                TabIndex = 0,
            };
            Controls.Add(Unità);
            Unità.KeyDown += new KeyEventHandler(PressEnter);
            Unità.KeyUp += new KeyEventHandler(PressVirgola);
            Unità.MouseClick += new MouseEventHandler(FocusUnità);
            Centesimi = new TextBox()
            {
                Text = "",
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = WidgetMoneyguard.backcolor,
                ForeColor = WidgetMoneyguard.textcolor,
                TabIndex = 1,
            };
            Controls.Add(Centesimi);
            Centesimi.KeyDown += new KeyEventHandler(PressEnter);
            Centesimi.MouseClick += new MouseEventHandler(FocusCentesimi);


            TimePicker = new DateTimePicker()
            {
                Format = DateTimePickerFormat.Custom,
                ShowUpDown = true,
                TabIndex=3,
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

            AttributoPanel = new Panel()
            {
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = WidgetMoneyguard.backcolor,
                TabIndex=2,
            };
            Controls.Add(AttributoPanel);

            timer = new Timer
            {
                Enabled = true,
                Interval = 10,
            };
            timer.Tick += new System.EventHandler(Timer);
            timer.Tick += new System.EventHandler(Tick);
        }

        public void ResizeForm()
        {
            Size = new Size(Program.widget.panel1.Width - 10, Program.widget.Height) ;
            Location = new Point(Program.widget.panel1.Location.X, 0);

            Valore_img.Location = new Point((int)(Width * 0.05), (int)(10));
            Valore_img.Size = new Size((int)(Width * 0.2), (int)(Width * 0.2));

            Punto.Font = new System.Drawing.Font("Stka Small", (int)(Width * 0.08), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Euro.Font = new System.Drawing.Font("Stka Small", (int)(Width * 0.08), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Unità.Font = new System.Drawing.Font("Stka Small", (int)(Width * 0.08), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Centesimi.Font = new System.Drawing.Font("Stka Small", (int)(Width * 0.08), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            Unità.Location = new Point(Valore_img.Location.X + Valore_img.Width *6/5, Valore_img.Location.Y + 7); Unità.Size = new Size((int)(Valore_img.Height * 1.5), Valore_img.Height);
            Punto.Location = new Point(Unità.Location.X + Unità.Width, Unità.Location.Y + 10); Punto.Height = Unità.Height;
            Centesimi.Location = new Point(Unità.Location.X + Unità.Width + Punto.Width, Valore_img.Location.Y + 7); Centesimi.Size = new Size(Valore_img.Height * 1, Valore_img.Height);
            Euro.Location = new Point(Centesimi.Location.X + Centesimi.Width, Unità.Location.Y + 7); Euro.Height = Unità.Height;

            DatePicker.Font = new System.Drawing.Font("Stka Small", (int)(Width * 0.05 - 3), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            TimePicker.Font = new System.Drawing.Font("Script MT Bold", (int)(Width * 0.06), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            DatePicker.Location = new System.Drawing.Point(20, Valore_img.Location.Y + (int)(Valore_img.Height * 1.2));
            DatePicker.Size = new System.Drawing.Size((int)(Width * 0.9), 50);

            AttributoPanel.Location = new Point(DatePicker.Location.X, DatePicker.Location.Y + DatePicker.Height + 10);
            AttributoPanel.Size = new Size(DatePicker.Width, Height - AttributoPanel.Location.Y - 70);

            TimePicker.ShowUpDown = false;
            TimePicker.Size = new System.Drawing.Size((int)(Height * 0.18 + 50), 50);
            TimePicker.Location = new System.Drawing.Point(20, AttributoPanel.Location.Y + AttributoPanel.Height + 10);
            TimePicker.ShowUpDown = true;

            True_Piccolo();
            
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
        private void FocusUnità(object sender, MouseEventArgs e)
        {
            Unità.SelectionStart = 0;
            Unità.SelectionLength = Unità.Text.Length;
        }
        private void FocusCentesimi(object sender, MouseEventArgs e)
        {
            Centesimi.SelectionStart = 0;
            Centesimi.SelectionLength = Centesimi.Text.Length;
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
        private void ClickTrue(object sender, EventArgs e)
        {
            Click_True();
        }
        public void Click_True()
        {
            Tick(null, null);
            bool CentesimiNumeric = int.TryParse(Centesimi.Text, out int n); if (Centesimi.Text == "") { n = 0; CentesimiNumeric = true; }
            bool UnitàNumeric = int.TryParse(Unità.Text, out int m); if (Unità.Text == "") { m = 0; UnitàNumeric = true; }
            if (Centesimi.Text == "" && Unità.Text == "") UnitàNumeric = false;
            if (UnitàNumeric && CentesimiNumeric == false) { return; }
            if (True.Visible == false) return;
            Visible = false;
            Input.RefreshData();
            attributo = Program.widget.attributo;
            try { if (Impostazioni.mute == false) if (attributo == "Introito") Impostazioni.yes_snd.Play(); else if (attributo == "Spesa") Impostazioni.no_snd.Play(); else if (attributo == "Trasferimento") Impostazioni.gnè_snd.Play(); } catch (Exception) { Console.WriteLine("Errore Sound2"); };
            tipo = Program.widget.tipo;
            metodo = Program.widget.metodo;
            valore = val;
            minuto = TimePicker.Value.Minute;
            ora = TimePicker.Value.Hour;
            giorno = DatePicker.Value.Day;
            mese = DatePicker.Value.Month;
            anno = DatePicker.Value.Year;
            textBox.Clear();
            foreach (TextBox txt in textbox) if (txt.Text != nuovo_attributo && Funzioni_utili.Scremato(txt.Text)!= "") { textBox.Add(Funzioni_utili.Scremato(txt.Text));  }
            Program.widget.Size = new Size(Program.widget.Width, Program.widget.Height - Program.widget.dim_pannello);
            Program.widget.PannelloTipi.Visible = false;
            Program.widget.PannelloFakeTipi.Visible = false;
            Program.widget.PannelloMetodi.Visible = false;
            Program.widget.PannelloFinale.Visible = false;
            Program.widget.panel1.Visible = true;
            Program.widget.ClickNull();

            if (FinestraPrincipale.BackPanel.Panel_Giorno != null && FinestraPrincipale.active)
            {
                if (FinestraPrincipale.BackPanel.StandardCalendar.giorno == DatePicker.Value.Day && FinestraPrincipale.BackPanel.StandardCalendar.mese == DatePicker.Value.Month && FinestraPrincipale.BackPanel.StandardCalendar.anno == DatePicker.Value.Year)
                {
                    ProprietàGiorno.pulsante_pending = true;
                    Console.WriteLine("OK?");
                }
                else SaveEvento();
            }
            else
            {
                SaveEvento();
                Console.WriteLine("OK!");
            }
        }
        void SaveEvento()
        {
            Input.eventi.Add(new Eventi { index = Input.eventi.Count() });
            Input.eventi[Input.eventi.Count - 1].Set_Attributo(attributo);
            Input.eventi[Input.eventi.Count - 1].SetValore(valore);
            Input.eventi[Input.eventi.Count - 1].SetTipo(tipo);
            Input.eventi[Input.eventi.Count - 1].SetMetodo(metodo);
            Input.eventi[Input.eventi.Count - 1].SetData(new int[] { 0, minuto, ora, giorno, mese, anno });
            Input.eventi[Input.eventi.Count - 1].SetData_modifica(Input.data_attuale);
            foreach (string txt in textBox) Input.eventi[Input.eventi.Count - 1].AddAttributo(txt);
            FinestraPrincipale.BackPanel.StandardCalendar.Calcoli_Mese();
            Pannello_StandardCalendar.resize = true;
            Savings.SaveEvents();
        }

        public void NewTextbox()
        {
            if (textbox.Count == 0 || textbox[textbox.Count - 1].Text != nuovo_attributo)
            {
                textbox.Insert(textbox.Count, new TextBox() { Text = nuovo_attributo, BorderStyle = System.Windows.Forms.BorderStyle.None, Location = new Point(4, 0), Size = new Size((int)(AttributoPanel.Width - 30), 20), BackColor = WidgetMoneyguard.backcolor, ForeColor = WidgetMoneyguard.textcolor,});
                Controllo.Insert(Controllo.Count, new Label() { BackgroundImageLayout = ImageLayout.Stretch, Visible = false, BackColor = WidgetMoneyguard.backcolor, });
                AttributoPanel.Controls.Add(textbox[textbox.Count - 1]);
                AttributoPanel.Controls.Add(Controllo[Controllo.Count -1]);
                textbox[textbox.Count - 1].KeyDown += EnterAttributo;
                textbox[textbox.Count - 1].KeyUp += CheckAttributi;
                textbox[textbox.Count - 1].Font = new System.Drawing.Font("Script MT Bold", (int)(Width * 0.06 + 7), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                if (textbox.Count > 2) if (textbox[textbox.Count - 2].Focused) ScrollToBottom(AttributoPanel);
                ResizeAttributi();
            }
            foreach (TextBox box in textbox)
            {
                if (box.Focused == false) box.SelectionStart = 0;
                if (box.Text != nuovo_attributo) box.ForeColor = WidgetMoneyguard.textcolor;
            }
        }
        private void CheckAttributi(object sender, EventArgs e)
        {
            string testo = "";
            for (int i = 0; i < textbox.Count - 1; i++) if (textbox[i].Focused)
                {
                    testo = Funzioni_utili.Scremato(textbox[i].Text);
                    if (testo == "") { Controllo[i].Visible = false; return; }
                    if (Input.all_attributi.Contains(Funzioni_utili.Scremato(textbox[i].Text)))
                    {
                        if (Controllo[i].BackgroundImage == vero && Controllo[i].Visible) return;
                        else Controllo[i].BackgroundImage = vero;
                        toolTip.SetToolTip(Controllo[i], "Attributo già usato");
                        Controllo[i].Visible = true;
                    }
                    else
                    {
                        if (Controllo[i].BackgroundImage == falso && Controllo[i].Visible) return;
                        else Controllo[i].BackgroundImage = falso;
                        toolTip.SetToolTip(Controllo[i], "Attributo mai usato");
                        Controllo[i].Visible = true;
                    }
                }
        }

        public void ScrollToBottom(Panel p)
        {
            using (Control c = new Control() { Parent = p, Dock = DockStyle.Bottom })
            {
                p.ScrollControlIntoView(c);
                c.Parent = null;
            }
        }
        private void Timer(object sender, EventArgs e)
        {
            foreach (TextBox box in textbox)
            {
                if (box.Focused && box.Text == nuovo_attributo) if (box.SelectionStart == 0 && box.SelectionLength == box.Text.Length) return; else { box.SelectionStart = 0; box.SelectionLength = box.Text.Length; }
            }
            for (int i = 0; i < textbox.Count(); i++)
            {
                if (textbox[i].Text == nuovo_attributo || textbox[i].Text == "") if (i != textbox.Count() - 1)
                    {
                        EnterAttributo();
                        AttributoPanel.Controls.Remove(textbox[i]);
                        textbox.Remove(textbox[i]);
                        AttributoPanel.Controls.Remove(Controllo[i]);
                        Controllo.Remove(Controllo[i]);
                        if (i == 0) textbox[0].Location = new Point(4 + size_controllo * 2, 0);
                        if (i == 0) Controllo[0].Location = new Point(4, (int)(textbox[0].Height * 0.3));
                        ResizeAttributi();
                    }
            }
            NewTextbox();
        }

        private void EnterAttributo(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.Control)
            {
                Click_True();
                e.SuppressKeyPress = true;
            }
            if (e.KeyCode == Keys.Enter)
            {
                EnterAttributo();
                e.SuppressKeyPress = true;
            }
        }
        private void EnterAttributo()
        {
            foreach (TextBox box in textbox)
            {
                if (box.Focused) foreach (TextBox textbox in textbox)
                    {
                        if (textbox.Text == nuovo_attributo) textbox.Focus();
                    }
            }
        }
        private void ResizeAttributi()
        {
            for (int i = 0; i < textbox.Count; i++)
            {
                textbox[i].Size = new Size((int)(AttributoPanel.Width - 54 - size_controllo), 20);
                textbox[i].Location = new Point(4 + size_controllo * 2, textbox[0].Location.Y + textbox[0].Height * i);
                textbox[i].Font = new System.Drawing.Font("Script MT Bold", (int)(Width * 0.03 + 7), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Controllo[i].Size = new Size(size_controllo, size_controllo);
                Controllo[i].Location = new Point(4, textbox[0].Location.Y + (int)(textbox[0].Height * (i + 0.3)));
                textbox[i].BackColor = WidgetMoneyguard.backcolor;
                textbox[i].ForeColor = WidgetMoneyguard.textcolor;
            }
        }
        private void Tick(object sender, EventArgs e)
        {
            bool CentesimiNumeric = uint.TryParse(Centesimi.Text, out uint n); if (Centesimi.Text == "") { n = 0; CentesimiNumeric = true; }
            bool UnitàNumeric = uint.TryParse(Unità.Text, out uint m); if (Unità.Text == "") { m = 0; UnitàNumeric = true; }
            if (Centesimi.Text == "" && Unità.Text == "") UnitàNumeric = false;
            if (UnitàNumeric && CentesimiNumeric && Program.widget.PannelloFinale.Visible) { val = Convert.ToDouble(m) + Funzioni_utili.SetCentesimi(Centesimi.Text); if (val != 0) True.Show(); else True.Hide(); }
            else { True.Hide(); if (Program.widget.PannelloFinale.Visible) True.Hide(); }
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
            True.Size = new Size(True.Width +2, True.Height +2);
            True.Location = new Point(True.Location.X -1, True.Location.Y -1);
        }
        private void True_Piccolo()
        {
            True.Size = new Size(TimePicker.Height, TimePicker.Height);
            True.Location = new System.Drawing.Point(20 + TimePicker.Width + 40, TimePicker.Location.Y);
        }

        public void SetTime()
        {
            Input.RefreshData();
            TimePicker.Value = new DateTime(Input.data_utile[5], Input.data_utile[4], Input.data_utile[3], Input.data_utile[2], Input.data_utile[1], Input.data_utile[0]);
            DatePicker.Value = TimePicker.Value;
        }
    }
}
