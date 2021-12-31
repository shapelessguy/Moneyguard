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
    public class Panel_NewEvento : Panel
    {
        private Label True;
        private Label False;

        public Label Scelta_attributo;
        public Label Scelta_attributo_txt;
        public Label Scelta_attributo_img;

        public Label Scelta_tipo;
        public Label Scelta_tipo_txt;
        public Label Scelta_tipo_img;

        public Label Scelta_metodo;
        public Label Scelta_metodo_txt;
        public Label Scelta_metodo_img;

        private Label Valore_img;
        private Label Punto;
        private Label Euro;
        private TextBox Unità;
        private TextBox Centesimi;

        public Panel_attributo_scelta Attributo_scelta;
        public Panel_tipi_scelta Tipi_scelta;
        public Panel_faketipi_scelta FakeTipi_scelta;
        public Panel_metodi_scelta Metodi_scelta;
        
        public bool sceltatipo = false;
        public bool sceltametodo = false;
        public bool sceltaimporto = false;
        static private bool enterable = false;

        private DateTimePicker TimePicker = new System.Windows.Forms.DateTimePicker();
        private DateTimePicker DatePicker = new System.Windows.Forms.DateTimePicker();


        private double correzione = 0;
        public int scelta = 0;
        public double valore;
        public static bool active = false;
        private Timer timer = new Timer();

        public string attributo;
        public string tipo;
        public string metodo;
        public int[] data;
        public int[] data_modifica;
        public int index;
        public List<string> attributi;

        public Panel_NewEvento()
        {
            DoubleBuffered = true;
            active = true;
            timer.Start();
            timer.Interval = 200;
            timer.Tick += new EventHandler(Tick);
            BackColor = Color.AliceBlue;
            BorderStyle = BorderStyle.FixedSingle;
            True = new Label()
            {
                Visible = false,
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("True"))),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(True);
            False = new Label()
            {
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("False"))),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(False);
            Scelta_attributo = new Label()
            {
                Text = "Categoria \u2192",
                AutoSize = true,
            };
            Controls.Add(Scelta_attributo);
            Scelta_attributo_txt = new Label()
            {
                AutoSize = true,
            };
            Controls.Add(Scelta_attributo_txt);
            Scelta_attributo_img = new Label()
            {
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(Scelta_attributo_img);

            Scelta_tipo = new Label()
            {
                Text = "Tipologia \u2192",
                AutoSize = true,
            };
            Controls.Add(Scelta_tipo);
            Scelta_tipo_txt = new Label()
            {
                AutoSize = true,
            };
            Controls.Add(Scelta_tipo_txt);
            Scelta_tipo_img = new Label()
            {
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(Scelta_tipo_img);
            Scelta_metodo = new Label()
            {
                Text = "Metodo \u2192",
                AutoSize = true,
            };
            Controls.Add(Scelta_metodo);
            Scelta_metodo_txt = new Label()
            {
                AutoSize = true,
            };
            Controls.Add(Scelta_metodo_txt);
            Scelta_metodo_img = new Label()
            {
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(Scelta_metodo_img);
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
            };
            Controls.Add(Punto);
            Euro = new Label()
            {
                Text = "\u20AC",
                AutoSize = true,
            };
            Controls.Add(Euro);
            Unità = new TextBox()
            {
                Text = "",
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = HorizontalAlignment.Right,
                TabStop = false,
            };
            Controls.Add(Unità);
            KeyDown += new KeyEventHandler(PressEnter);
            Unità.KeyDown += new KeyEventHandler(PressEnter);
            Unità.KeyUp += new KeyEventHandler(PressVirgola);
            Unità.MouseClick += new MouseEventHandler(FocusUnità);
            Centesimi = new TextBox()
            {
                Text = "",
                BorderStyle = BorderStyle.FixedSingle,
                TabStop = false,
            };
            Controls.Add(Centesimi);
            Centesimi.KeyDown += new KeyEventHandler(PressEnter);
            Centesimi.MouseClick += new MouseEventHandler(FocusCentesimi);


            TimePicker = new DateTimePicker()
            {
                Format = DateTimePickerFormat.Custom,
                ShowUpDown = true,
                TabStop = false,
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

            Valore_img.Click += new EventHandler(Click_Null);
            Punto.Click += new EventHandler(Click_Null);
            Euro.Click += new EventHandler(Click_Null);
            Scelta_metodo_img.Click += new EventHandler(Click_Null);
            Scelta_metodo.Click += new EventHandler(Sceltametodo);
            Scelta_attributo.Click += new EventHandler(Sceltaattributo);
            Scelta_tipo.Click += new EventHandler(Sceltatipo);
            Scelta_attributo.MouseEnter += new EventHandler(EnterSceltaAttributo);
            Scelta_attributo.MouseLeave += new EventHandler(LeaveSceltaAttributo);
            Scelta_tipo.MouseEnter += new EventHandler(EnterSceltaTipo);
            Scelta_tipo.MouseLeave += new EventHandler(LeaveSceltaTipo);
            Scelta_metodo.MouseEnter += new EventHandler(EnterSceltaMetodo);
            Scelta_metodo.MouseLeave += new EventHandler(LeaveSceltaMetodo);
            False.MouseEnter += new EventHandler(Enter_False);
            False.MouseLeave += new EventHandler(Leave_False);
            True.MouseEnter += new EventHandler(Enter_True);
            True.MouseLeave += new EventHandler(Leave_True);
            False.MouseClick += new MouseEventHandler(Click_False);
            True.MouseClick += new MouseEventHandler(Click_True);
            Click += new EventHandler(Click_Null);
            

            Resize_NewEvento();
        }

        public void Disposer()
        {
            active = false;
            Valore_img.BackgroundImage.Dispose();
            Valore_img.Dispose();
            Euro.Dispose();
            Punto.Dispose();
            Unità.Dispose();
            Centesimi.Dispose();
            True.BackgroundImage.Dispose();
            True.Dispose();
            False.BackgroundImage.Dispose();
            False.Dispose();
            Scelta_tipo.Dispose();
            Scelta_tipo_txt.Dispose();
            if (Scelta_tipo_img.BackgroundImage != null) Scelta_tipo_img.BackgroundImage.Dispose();
            Scelta_tipo_img.Dispose();
            Scelta_attributo.Dispose();
            Scelta_attributo_txt.Dispose();
            if (Scelta_attributo_img.BackgroundImage != null) Scelta_attributo_img.BackgroundImage.Dispose();
            Scelta_attributo_img.Dispose();
            Scelta_metodo.Dispose();
            Scelta_metodo_txt.Dispose();
            if (Scelta_metodo_img.BackgroundImage != null) Scelta_metodo_img.BackgroundImage.Dispose();
            Scelta_metodo_img.Dispose();
            if (Tipi_scelta != null) Tipi_scelta.Disposer();
            if (FakeTipi_scelta != null) FakeTipi_scelta.Disposer();
            if (Metodi_scelta != null) Metodi_scelta.Disposer();
            timer.Tick -= Tick;
            timer.Dispose();
            Dispose();
        }

        private void Tick(object sender, EventArgs e)
        {
            bool CentesimiNumeric = uint.TryParse(Centesimi.Text, out uint n); if (Centesimi.Text == "") { n = 0; CentesimiNumeric = true; }
            bool UnitàNumeric = uint.TryParse(Unità.Text, out uint m); if (Unità.Text == "") { m = 0; UnitàNumeric = true; }
            if (Centesimi.Text == "" && Unità.Text == "") UnitàNumeric = false;
            if (UnitàNumeric && CentesimiNumeric && tipo != "" && metodo != "") { True.Show(); enterable = true; valore = Convert.ToDouble(m) + Funzioni_utili.SetCentesimi(Centesimi.Text.Replace('-', ' ')); }
            else { True.Hide(); enterable = false; }
        }

        public void Resize_NewEvento()
        {
            Location = new Point(FinestraPrincipale.BackPanel.Panel_Giorno.Tipi.Width + 10, 10);
            Size = new Size((int)((FinestraPrincipale.BackPanel.Panel_Giorno.Plus.Location.X - Location.X)* 0.95), FinestraPrincipale.BackPanel.Panel_Giorno.Note_txt.Location.Y - Location.Y);
            correzione = ((double)FinestraPrincipale.Finestra.Width / (double)FinestraPrincipale.Finestra.Height - (double)FinestraPrincipale.minimum_weight / (double)FinestraPrincipale.minimum_height) * 35;
            True_Piccolo(); False_Piccolo();

            if (attributo == "Trasferimento")
            {
                Scelta_tipo.Text = "Da";
                Scelta_metodo.Text = "a \u2192";
            }
            else
            {
                Scelta_tipo.Text = "Tipologia \u2192";
                Scelta_metodo.Text = "Metodo \u2192";
            }
            Scelta_attributo_txt.Text = attributo;
            Scelta_attributo_img.BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject(Associazione.AiconaAssociata(attributo))));
            Scelta_tipo_txt.Text = Funzioni_utili.GetTipo(tipo);
            if (tipo != "") if (attributo=="Trasferimento") Scelta_tipo_img.BackgroundImage = Funzioni_utili.TakePicture(Funzioni_utili.GetTipo(tipo), 2);
            else Scelta_tipo_img.BackgroundImage = Funzioni_utili.TakePicture(Funzioni_utili.GetTipo(tipo), 1);

            Scelta_metodo_txt.Text = metodo;
            if (metodo != "") Scelta_metodo_img.BackgroundImage = Funzioni_utili.TakePicture(metodo, 2);
            if (data != null) { TimePicker.Value = new DateTime(data[5], data[4], data[3], data[2], data[1], data[0]); DatePicker.Value = TimePicker.Value; }

            Unità.Text = Convert.ToString((int)valore);
            Centesimi.Text = Funzioni_utili.GetCentesimiString(valore);
            if (Funzioni_utili.GetCentesimi(valore) == 0) Centesimi.Text += "0";

            Scelta_attributo.Location = new Point((int)(Width * 0.05), (int)(Height * 0.05));
            Scelta_attributo.Font = new System.Drawing.Font(BackPanel.font1, (int)(Height * 0.05), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            Scelta_attributo_img.Location = new Point((int)(Scelta_attributo.Location.X + Scelta_attributo.Width + Width * 0.02), (int)(Scelta_attributo.Location.Y));
            Scelta_attributo_img.Height = Scelta_attributo.Location.Y + Scelta_attributo.Height - Scelta_attributo_img.Location.Y + 5;
            Scelta_attributo_img.Width = Scelta_attributo_img.Height;

            Scelta_attributo_txt.Location = new Point((int)(Scelta_attributo_img.Location.X + Scelta_attributo_img.Width + Width * 0.01), (int)(Scelta_attributo.Location.Y + Scelta_attributo.Height * 0.1));
            Scelta_attributo_txt.Font = new System.Drawing.Font(BackPanel.font1, (int)(Height * 0.05), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            Scelta_tipo.Location = new Point((int)(Width * 0.05), (int)(Scelta_attributo.Location.Y + Scelta_attributo.Height * 1.5));
            Scelta_tipo.Font = new System.Drawing.Font(BackPanel.font1, (int)(Height * 0.05), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            Scelta_tipo_img.Location = new Point((int)(Scelta_tipo.Location.X + Scelta_tipo.Width + Width * 0.02), (int)(Scelta_tipo.Location.Y));
            Scelta_tipo_img.Height = Scelta_tipo.Location.Y + Scelta_tipo.Height - Scelta_tipo_img.Location.Y + 5;
            Scelta_tipo_img.Width = Scelta_tipo_img.Height;

            Scelta_tipo_txt.Location = new Point((int)(Scelta_tipo_img.Location.X + Scelta_tipo_img.Width + Width * 0.01), (int)(Scelta_tipo.Location.Y + Scelta_tipo.Height * 0.1));
            Scelta_tipo_txt.Font = new System.Drawing.Font(BackPanel.font1, (int)(Height * 0.05), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            Scelta_metodo.Location = new Point((int)(Width * 0.05), (int)(Scelta_tipo.Location.Y + Scelta_tipo.Height * 1.5));
            Scelta_metodo.Font = new System.Drawing.Font(BackPanel.font1, (int)(Height * 0.05), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            Scelta_metodo_img.Location = new Point((int)(Scelta_metodo.Location.X + Scelta_metodo.Width + Width * 0.02), (int)(Scelta_metodo.Location.Y));
            Scelta_metodo_img.Height = Scelta_metodo.Location.Y + Scelta_metodo.Height - Scelta_metodo_img.Location.Y + 5;
            Scelta_metodo_img.Width = Scelta_metodo_img.Height;

            Scelta_metodo_txt.Location = new Point((int)(Scelta_metodo_img.Location.X + Scelta_metodo_img.Width + Width * 0.01), (int)(Scelta_metodo.Location.Y));
            Scelta_metodo_txt.Font = new System.Drawing.Font(BackPanel.font1, (int)(Height * 0.05), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            Valore_img.Location = new Point((int)(Width * 0.05), (int)(Scelta_metodo.Location.Y + Scelta_metodo.Height * 1.8));
            Valore_img.Size = new Size((int)(Scelta_metodo.Height * 1.4), (int)(Scelta_metodo.Height * 1.4));

            Punto.Font = new System.Drawing.Font(BackPanel.font1, (int)(Height * 0.05), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Euro.Font = new System.Drawing.Font(BackPanel.font1, (int)(Height * 0.05), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Unità.Font = new System.Drawing.Font(BackPanel.font1, (int)(Height * 0.05), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Centesimi.Font = new System.Drawing.Font(BackPanel.font1, (int)(Height * 0.05), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            Unità.Location = new Point(Valore_img.Location.X + Valore_img.Width * 3/2, Valore_img.Location.Y + 7); Unità.Size = new Size((int)(Valore_img.Height * 2.2), Valore_img.Height);
            Punto.Location = new Point(Unità.Location.X + Unità.Width, Unità.Location.Y + 10); Punto.Height = Unità.Height;
            Centesimi.Location = new Point(Unità.Location.X + Unità.Width + Punto.Width, Valore_img.Location.Y + 7); Centesimi.Size = new Size(Valore_img.Height * 1, Valore_img.Height);
            Euro.Location = new Point(Centesimi.Location.X + Centesimi.Width, Unità.Location.Y+7); Euro.Height = Unità.Height;

            DatePicker.Font = new System.Drawing.Font(BackPanel.font3, (int)(Width * 0.025 +2), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            TimePicker.Font = new System.Drawing.Font(BackPanel.font1, (int)(Height * 0.05 + 5), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            DatePicker.Location = new System.Drawing.Point(20, Valore_img.Location.Y + (int)(Valore_img.Height * 2.0));
            DatePicker.Size = new System.Drawing.Size((int)(Width * 0.58), 50);

            TimePicker.ShowUpDown = false;
            TimePicker.Size = new System.Drawing.Size((int)(Height * 0.18 + 50), 50);
            TimePicker.Location = new System.Drawing.Point((int)((Width*0.9) + 15 - TimePicker.Width), DatePicker.Location.Y + (DatePicker.Height - TimePicker.Height) / 2 - 10);
            TimePicker.ShowUpDown = true;

            ResizePanels();
        }

        private void ResizePanels()
        {
            if (Attributo_scelta != null) { Attributo_scelta.SetLocationY(FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Scelta_attributo.Location.Y + FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Scelta_attributo.Height + 5); Attributo_scelta.ResizeForm(); }
            if (Tipi_scelta != null) { Tipi_scelta.SetLocationY(FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Scelta_tipo.Location.Y + FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Scelta_tipo.Height + 5); Tipi_scelta.ResizeForm(); }
            if (FakeTipi_scelta != null) { FakeTipi_scelta.SetLocationY(FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Scelta_tipo.Location.Y + FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Scelta_tipo.Height + 5); FakeTipi_scelta.ResizeForm(); }
            if (Metodi_scelta != null) { Metodi_scelta.SetLocationY(FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Scelta_metodo.Location.Y + FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Scelta_metodo.Height + 5); Metodi_scelta.ResizeForm(); }
        }

        private void Click_False(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) Click_False();
        }

        public void Click_False()
        {
            Disposer();
            FinestraPrincipale.BackPanel.Panel_Giorno.ClickNull();
            FinestraPrincipale.BackPanel.Panel_Giorno.pulsanti_selectable = true;
            
            FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento = null;
        }
        private void Click_True(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Click_True();
            }
        }

        private void Click_True()
        {
            Tick(null, null);
            Visible = false;
            bool CentesimiNumeric = int.TryParse(Centesimi.Text, out int n); if (Centesimi.Text == "") { n = 0; CentesimiNumeric = true; }
            bool UnitàNumeric = int.TryParse(Unità.Text, out int m); if (Unità.Text == "") { m = 0; UnitàNumeric = true; }
            if (Centesimi.Text == "" && Unità.Text == "") UnitàNumeric = false;
            if (UnitàNumeric && CentesimiNumeric == false) { return; }
            if (enterable == false) return;
            Input.RefreshData();
            string[] stringa = new string[attributi.Count] ; int i = 0;
            foreach (string attr in attributi) if(attr != ""){ stringa[i] = attr; i++; }

            foreach (Pulsante puls in FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale)
            {
                if (puls.index == index)
                {
                    FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale.Remove(puls);
                    FinestraPrincipale.BackPanel.Panel_Giorno.Tipi.Controls.Remove(puls);
                    puls.Disposer();
                    break;
                }
            }
            if (DatePicker.Value.Day != FinestraPrincipale.BackPanel.StandardCalendar.giorno || DatePicker.Value.Month != FinestraPrincipale.BackPanel.StandardCalendar.mese || DatePicker.Value.Year != FinestraPrincipale.BackPanel.StandardCalendar.anno)
            {
                for (int j = 0; j < Input.eventi.Count; j++) if (Input.eventi[j].index == index) Input.eventi.Remove(Input.eventi[j]);
                Eventi evento = new Eventi()
                {
                    index = index,
                };
                evento.SetTipo(tipo);
                evento.SetMetodo(metodo);
                evento.SetValore(valore);
                evento.Set_Attributo(attributo);
                evento.SetData(new int[] { TimePicker.Value.Second, TimePicker.Value.Minute, TimePicker.Value.Hour, DatePicker.Value.Day, DatePicker.Value.Month, DatePicker.Value.Year });
                evento.SetData_modifica(Input.data_attuale);
                foreach (string txt in stringa) if (txt != "") evento.AddAttributo(txt);
                Input.eventi.Add(evento);
            }
            else
            {
                Pulsante pulsante = new Pulsante
                {
                    attributo = attributo,
                    tipo = tipo,
                    metodo = metodo,
                    valore = valore,
                    data = new int[] { TimePicker.Value.Second, TimePicker.Value.Minute, TimePicker.Value.Hour, DatePicker.Value.Day, DatePicker.Value.Month, DatePicker.Value.Year },
                    data_modifica = Input.data_attuale,
                    index = index,
                    bordo = false,
                };
                foreach (string txt in stringa) if (txt != "") pulsante.attributi.Add(txt);
                if (attributo == "Trasferimento") pulsante.tipo = this.tipo + "\u2192" + this.metodo;
                pulsante.SetImage(Associazione.AiconaAssociata(pulsante.attributo), Associazione.IconaAssociata(pulsante.tipo), Associazione.MiconaAssociata(pulsante.metodo));
                pulsante.lab_tipo.Text = pulsante.tipo;
                pulsante.TextTipo(pulsante.tipo);
                pulsante.Text(Funzioni_utili.FormatoStandard(pulsante.valore) + " \u20AC");
                pulsante.SetTooltip();
                FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale.Insert(0, pulsante);
            }
            FinestraPrincipale.BackPanel.Panel_Giorno.ResizeGiorno();
            FinestraPrincipale.BackPanel.Panel_Giorno.RefreshTipi();
            ProprietàGiorno.time_to_save = true;

            Click_False();
        }
        public void Click_Null(object sender, EventArgs e)
        {
            Click_Null();
        }
        private void Click_Null()
        {
            if (Tipi_scelta != null)
            {
                Tipi_scelta.Disposer();
            }
            if (FakeTipi_scelta != null)
            {
                FakeTipi_scelta.Disposer();
            }
            if (Metodi_scelta != null)
            {
                Metodi_scelta.Disposer();
            }
            if (Attributo_scelta != null)
            {
                Attributo_scelta.Disposer();
            }
            Focus();
            scelta = 0;
        }

        private void Sceltaattributo(object sender, EventArgs e)
        {
            if (scelta == 1) { Click_Null(); return; }
            Click_Null();
            Attributo_scelta = new Panel_attributo_scelta(false);
            ResizePanels();
            Controls.Add(Attributo_scelta);
            Attributo_scelta.BringToFront();
            Resize_NewEvento();
            scelta = 1;
            Update();
        }
        private void Sceltatipo(object sender, EventArgs e)
        {
            if (scelta == 2) { Click_Null(); return; }
            Click_Null();
            if (attributo == "Trasferimento")
            {
                FakeTipi_scelta = new Panel_faketipi_scelta();
                ResizePanels();
                Controls.Add(FakeTipi_scelta);
                FakeTipi_scelta.BringToFront();
                FakeTipi_scelta.Update();
            }
            else
            {
                Tipi_scelta = new Panel_tipi_scelta();
                ResizePanels();
                Controls.Add(Tipi_scelta);
                Tipi_scelta.BringToFront();
                Tipi_scelta.Update();
            }
            Resize_NewEvento();
            scelta = 2;
        }
        private void Sceltametodo(object sender, EventArgs e)
        {
            if (scelta == 3) { Click_Null(); return; }
            Click_Null();
            Metodi_scelta = new Panel_metodi_scelta();
            ResizePanels();
            Controls.Add(Metodi_scelta);
            Metodi_scelta.BringToFront();
            Resize_NewEvento();
            scelta = 3;
            Update();
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
        private void True_Grande()
        {
            True.Size = new Size((int)(Width * 0.12 - correzione), (int)(Width * 0.12 - correzione));
            True.Location = new Point((int)(Width * 0.77) - True.Width / 2 + (int)correzione, (int)(Height * 0.88) - True.Height / 2);
        }
        private void True_Piccolo()
        {
            True.Size = new Size((int)(Width * 0.1 - correzione), (int)(Width * 0.1 - correzione));
            True.Location = new Point((int)(Width * 0.77) - True.Width/2 + (int)correzione, (int)(Height * 0.88) - True.Height / 2);
        }
        private void False_Grande()
        {
            False.Size = new Size((int)(Width * 0.12 - correzione), (int)(Width * 0.12 - correzione));
            False.Location = new Point((int)(Width * 0.9) - False.Width / 2, (int)(Height * 0.88) - False.Height / 2);
        }
        private void False_Piccolo()
        {
            False.Size = new Size((int)(Width * 0.1 - correzione), (int)(Width * 0.1 - correzione));
            False.Location = new Point((int)(Width * 0.9) - False.Width / 2, (int)(Height * 0.88) - False.Height / 2);
        }

        private void EnterSceltaTipo(object sender, EventArgs e)
        {
            Scelta_tipo.BackColor = Color.LightYellow;
        }
        private void LeaveSceltaTipo(object sender, EventArgs e)
        {
            Scelta_tipo.BackColor = Color.Transparent;
        }
        private void EnterSceltaMetodo(object sender, EventArgs e)
        {
            Scelta_metodo.BackColor = Color.LightYellow;
        }
        private void LeaveSceltaMetodo(object sender, EventArgs e)
        {
            Scelta_metodo.BackColor = Color.Transparent;
        }
        private void EnterSceltaAttributo(object sender, EventArgs e)
        {
            Scelta_attributo.BackColor = Color.LightYellow;
        }
        private void LeaveSceltaAttributo(object sender, EventArgs e)
        {
            Scelta_attributo.BackColor = Color.Transparent;
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
    }
}
