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
    public class PanelRicerca:Panel
    {
        static public bool resize_datacode = true;
        static public string attributo = "";
        static public bool initialized = false;

        public int primo_filtro = 0;
        public long datacode1, datacode2;
        public Point time1, time2;
        public double valore1, valore2;
        public List<string> tipi_inclusi = new List<string>();
        public List<string> attributi_inclusi = new List<string>();
        public List<string> metodi_inclusi = new List<string>();
        public List<string> tipi_esclusi = new List<string>();
        public List<string> attributi_esclusi = new List<string>();
        public List<string> metodi_esclusi = new List<string>();
        public int previous_primo_filtro = 0;
        public long previous_datacode1, previous_datacode2;
        public Point previous_time1, previous_time2;
        public double previous_valore1, previous_valore2;
        public List<string> previous_tipi_inclusi = new List<string>();
        public List<string> previous_attributi_inclusi = new List<string>();
        public List<string> previous_metodi_inclusi = new List<string>();
        public List<string> previous_tipi_esclusi = new List<string>();
        public List<string> previous_attributi_esclusi = new List<string>();
        public List<string> previous_metodi_esclusi = new List<string>();

        public List<Eventi> eventi_filtrati = new List<Eventi>();

        bool erroredata = false;
        bool errorevalori = false;
        bool erroreore = false;

        public Label Research;
        public Label Research_Laterale;
        public Label Go_Calendar;
        public Label Go_Calendar_txt;
        public static bool active;
        Panel Laterale;
        ToolTip tooltip;
        public PanelAttributi PanelInclusione;
        public PanelAttributi PanelInclusione_Attributi;
        public PanelAttributi PanelInclusione_Metodi;
        public PanelAttributi PanelEsclusione;
        public PanelAttributi PanelEsclusione_Attributi;
        public PanelAttributi PanelEsclusione_Metodi;
        public MotoreRicerca PanelMotore_Attributi;
        public MotoreRicerca PanelMotore_Tipologie;
        public MotoreRicerca PanelMotore_Metodi;
        Label Inclusione_Testo;
        Label Esclusione_Testo;

        Label Introito, Spesa, Trasferimento, All, L_Grafici, L_Scaletta;
        Label PrimaData, SecondaData;
        Label ErroreData, ErroreOre, ErroreValori;
        Label PrimoValore, SecondoValore;
        TextBox Valore1, Valore2;
        DateTimePicker Data1, Data2, Time1, Time2;
        Timer initializePanels, showErrors;
        readonly Color backcolor_primofiltro = Color.Transparent;
        readonly Color evidenziatore_primofiltro = Color.White;
        public readonly Bitmap Introito_image = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("Plus")));
        public readonly Bitmap Spesa_image = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("Minus")));
        public readonly Bitmap Trasferimento_image = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("Trasferimento")));
        public readonly Bitmap All_image = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("Tutti_movimenti")));
        public readonly Bitmap Scaletta_image = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("Tabella")));
        public readonly Bitmap Statistiche_image = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("Statistics")));
        readonly int img_size = 50;

        public Visualizzazione_Scaletta Scaletta;
        public Visualizzazione_Grafici Grafici;

        public void Disposer()
        {
            try
            {
                if (Scaletta != null) { Scaletta.Disposer(); Console.WriteLine("Disposing Scaletta"); }
                if (Grafici != null) { Grafici.Disposer(); Console.WriteLine("Disposing Grafici"); }

                FinestraPrincipale.BackPanel.Ora.Show();
                PanelInclusione.Disposer();
                PanelInclusione_Attributi.Disposer();
                PanelInclusione_Metodi.Disposer();
                PanelEsclusione.Disposer();
                PanelEsclusione_Attributi.Disposer();
                PanelEsclusione_Metodi.Disposer();
                PanelMotore_Attributi.Disposer();
                PanelMotore_Tipologie.Disposer();
                PanelMotore_Metodi.Disposer();
                Research.BackgroundImage.Dispose();
                Research.Dispose();
                Research_Laterale.BackgroundImage.Dispose();
                Research_Laterale.Dispose();
                Go_Calendar.BackgroundImage.Dispose();
                Go_Calendar.Dispose();
                Go_Calendar_txt.Dispose();
                tooltip.Dispose();
                Laterale.Dispose();
                //Dispose();
                FinestraPrincipale.BackPanel.Controls.Remove(this);
                initializePanels.Dispose();
                showErrors.Dispose();
                BackPanel.go_to_ricerca = false;
                active = false;
                FinestraPrincipale.BackPanel.Focus();
            }
            catch (Exception) { }
        }
        public void ShowRicerca()
        {
            Input.LoadAttributi();
            Input.Scrematura_eventi();
            SuspendLayout();
            ResumeLayout(false);
            //FinestraPrincipale.BackPanel.StandardCalendar.Visible = false;
            active = true;
        }
        public void HideIcons()
        {
            FinestraPrincipale.BackPanel.Portafogli.Hide();
            FinestraPrincipale.BackPanel.Cassaforte.Hide();
            FinestraPrincipale.BackPanel.Banca_Pic.Hide();
            FinestraPrincipale.BackPanel.Calc_Pic.Hide();
            FinestraPrincipale.BackPanel.Portafogli_Pic.Hide();
            FinestraPrincipale.BackPanel.Cassaforte_Pic.Hide();
            FinestraPrincipale.BackPanel.Ora.Hide();
        }

        public PanelRicerca()
        {
            SuspendLayout();
            DoubleBuffered = true;
            Visible = false;
            SendToBack();
            if (FinestraPrincipale.BackPanel.Panel_Impostazioni != null) { BackPanel.go_to_impostazioni = false; FinestraPrincipale.BackPanel.Panel_Impostazioni.CloseImpostazioni(); }
            
            ShowRicerca();
            HideIcons();

            FinestraPrincipale.BackPanel.Controls.Add(this);
            Size = FinestraPrincipale.BackPanel.Size;
            Click += ClickNull;
            KeyDown += EnterRicerca;

            Research = new Label()
            {
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("Research"))),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(Research);
            Research_Laterale = new Label()
            {
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("Research"))),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Research.Click += ResearchClick;
            Research.MouseEnter += new EventHandler(Enter_Research);
            Research.MouseLeave += new EventHandler(Leave_Research);

            Go_Calendar = new Label()
            {
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("Calendario2"))),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(Go_Calendar);
            Go_Calendar_txt = new Label()
            {
                Text = "Al calendario",
                AutoSize = true,
                Visible = false,
            };
            Controls.Add(Go_Calendar_txt);
            Go_Calendar.MouseEnter += new EventHandler(Enter_Go_Calendar);
            Go_Calendar.MouseLeave += new EventHandler(Leave_Go_Calendar);
            Go_Calendar.MouseClick += CloseRicerca;
            Go_Calendar_txt.Font = new System.Drawing.Font("Script MT Bold", 12, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tooltip = new ToolTip
            {
                AutoPopDelay = 5000,
                InitialDelay = 500,
                ReshowDelay = 500
            };

            Laterale = new Panel()
            {
                BackColor = Color.Gray,
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = true,
                Visible = false,
            };
            Controls.Add(Laterale);
            Laterale.Location = new Point(0, FinestraPrincipale.BackPanel.Menù.Height + 1);
            Laterale.Click += ClickNull;

            Introito = new Label()
            {
                BackgroundImage = Introito_image,
                BackgroundImageLayout = ImageLayout.Stretch,
                BorderStyle = BorderStyle.FixedSingle,
            };
            Laterale.Controls.Add(Introito);
            Spesa = new Label()
            {
                BackgroundImage = Spesa_image,
                BackgroundImageLayout = ImageLayout.Stretch,
                BorderStyle = BorderStyle.FixedSingle,
            };
            Laterale.Controls.Add(Spesa);
            Trasferimento = new Label()
            {
                BackgroundImage = Trasferimento_image,
                BackgroundImageLayout = ImageLayout.Stretch,
                BorderStyle = BorderStyle.FixedSingle,
            };
            Laterale.Controls.Add(Trasferimento);
            All = new Label()
            {
                BackgroundImage = All_image,
                BackgroundImageLayout = ImageLayout.Stretch,
                BorderStyle = BorderStyle.Fixed3D,
                BackColor = evidenziatore_primofiltro,
            };
            Laterale.Controls.Add(All);

            tooltip.SetToolTip(Introito, "Introito");
            tooltip.SetToolTip(Spesa, "Spesa");
            tooltip.SetToolTip(Trasferimento, "Trasferimento");
            tooltip.SetToolTip(All, "Tutti i movimenti");
            tooltip.SetToolTip(Research, "Ricerca");
            tooltip.SetToolTip(Research_Laterale, "Ricerca");

            Introito.Size = new Size(img_size, img_size);
            Spesa.Size = new Size(img_size, img_size);
            Trasferimento.Size = new Size(img_size, img_size);
            All.Size = new Size(img_size * 3 + 20, img_size);

            Introito.Location = new Point(30, 10);
            Spesa.Location = new Point(Introito.Location.X + img_size + 10, 10);
            Trasferimento.Location = new Point(Spesa.Location.X + img_size + 10, 10);
            All.Location = new Point(30, img_size + 20);

            Introito.Click += ClickIntroito;
            Spesa.Click += ClickSpesa;
            Trasferimento.Click += ClickTrasferimento;
            All.Click += ClickAll;


            L_Grafici = new Label()
            {
                BackgroundImage = Statistiche_image,
                BackgroundImageLayout = ImageLayout.Stretch,
                BorderStyle = BorderStyle.Fixed3D,
                BackColor = evidenziatore_primofiltro,
                AutoSize = false,
                Visible = false,
            };
            Controls.Add(L_Grafici);
            L_Scaletta = new Label()
            {
                BackgroundImage = Scaletta_image,
                BackgroundImageLayout = ImageLayout.Stretch,
                BorderStyle = BorderStyle.Fixed3D,
                BackColor = evidenziatore_primofiltro,
                AutoSize=false,
                Visible = false,
            };
            Controls.Add(L_Scaletta);

            tooltip.SetToolTip(L_Grafici, "Grafico");
            tooltip.SetToolTip(L_Scaletta, "Tabella");
            L_Scaletta.Size = new Size(80, 50);
            L_Grafici.Size = new Size(80, 50);
            L_Scaletta.Click += ClickScaletta;
            L_Grafici.Click += ClickGrafici;

            PrimaData = new Label()
            {
                Text = "Eventi tra le date:",
                Font = new Font(BackPanel.font3, 12, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
            };
            Laterale.Controls.Add(PrimaData);
            PrimaData.KeyDown += EnterRicerca;
            PrimaData.KeyDown += SimplyEnterRicerca;
            SecondaData = new Label()
            {
                Text = "tra le ore:",
                Font = new Font(BackPanel.font3, 12, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
            };
            Laterale.Controls.Add(SecondaData);
            SecondaData.KeyDown += EnterRicerca;
            SecondaData.KeyDown += SimplyEnterRicerca;
            Data1 = new DateTimePicker()
            {
                Format = DateTimePickerFormat.Short,
                Font = new Font("Arial", 10, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                TabIndex = 0,
            };
            Data1.KeyDown += EnterRicerca;
            Data1.KeyDown += SimplyEnterRicerca;
            Data2 = new DateTimePicker()
            {
                Format = DateTimePickerFormat.Short,
                Font = new Font("Arial", 10, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                TabIndex = 1,
            };
            Data2.KeyDown += EnterRicerca;
            Data2.KeyDown += SimplyEnterRicerca;
            Time1 = new DateTimePicker()
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "HH:mm",
                ShowUpDown = true,
                Font = new Font("Arial", 12, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                TabIndex = 2,
            };
            Time1.KeyDown += EnterRicerca;
            Time1.KeyDown += SimplyEnterRicerca;
            Laterale.Controls.Add(Time1);
            Time2 = new DateTimePicker()
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "HH:mm",
                ShowUpDown = true,
                Font = new Font("Arial", 12, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                TabIndex = 3,
            };
            Time2.KeyDown += EnterRicerca;
            Time2.KeyDown += SimplyEnterRicerca;
            Laterale.Controls.Add(Time2);
            Data1.Size = new Size(100, Data1.Height);
            Data2.Size = new Size(100, Data2.Height);
            Laterale.Controls.Add(Data1);
            Laterale.Controls.Add(Data2);
            PrimaData.Location = new Point(10, All.Location.Y + img_size + 30);
            Data1.Location = new Point(10, PrimaData.Location.Y + 25);
            SecondaData.Location = new Point(10, Data1.Location.Y + 30);
            Data2.Location = new Point(Data1.Location.X + Data1.Width + 8, PrimaData.Location.Y + 25);
            PrimaData.Size = new Size(200, 25);
            SecondaData.Size = new Size(200, 25);
            Time1.Size = new Size(80, Time1.Height);
            Time2.Size = new Size(80, Time2.Height);
            Time1.Location = new Point(Data1.Location.X, SecondaData.Location.Y + 25);
            Time2.Location = new Point(Time1.Location.X + Time1.Width + 20, SecondaData.Location.Y + 25);
            PrimaData.Click += ClickNull;
            SecondaData.Click += ClickNull;


            PrimoValore = new Label()
            {
                Text = "da:",
                Font = new Font(BackPanel.font3, 12, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            Laterale.Controls.Add(PrimoValore);
            SecondoValore = new Label()
            {
                Text = "a:",
                Font = new Font(BackPanel.font3, 12, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                TextAlign = ContentAlignment.MiddleCenter,
            };
            Laterale.Controls.Add(SecondoValore);
            Valore1 = new TextBox()
            {
                Text = "0,00\u20AC",
                Font = new Font(BackPanel.font3, 12, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                TextAlign = HorizontalAlignment.Center,
                TabIndex = 4,
            };
            Laterale.Controls.Add(Valore1);
            Valore1.KeyDown += EnterRicerca;
            Valore1.KeyDown += SimplyEnterRicerca;
            Valore2 = new TextBox()
            {
                Text = "0,00\u20AC",
                Font = new Font(BackPanel.font3, 12, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                TextAlign = HorizontalAlignment.Center,
                TabIndex = 5,
            };
            Laterale.Controls.Add(Valore2);
            Valore2.KeyDown += SimplyEnterRicerca;
            Valore2.KeyDown += EnterRicerca;
            Valore1.LostFocus += Valore1Lost;
            Valore1.GotFocus += Valore1Got;
            Valore2.LostFocus += Valore2Lost;
            Valore2.GotFocus += Valore2Got;

            PrimoValore.Size = new Size(40, PrimoValore.Height);
            SecondoValore.Size = new Size(25, PrimoValore.Height);
            Valore1.Size = new Size(70, PrimoValore.Height);
            Valore2.Size = new Size(70, PrimoValore.Height);

            PrimoValore.Location = new Point(10, Time2.Location.Y + 50);
            Valore1.Location = new Point(PrimoValore.Location.X + PrimoValore.Width, PrimoValore.Location.Y);
            SecondoValore.Location = new Point(Valore1.Location.X + Valore1.Width, Valore1.Location.Y);
            Valore2.Location = new Point(SecondoValore.Location.X + SecondoValore.Width, SecondoValore.Location.Y);
            PrimoValore.Click += ClickNull;
            SecondoValore.Click += ClickNull;
            Valore1.Click += HideMotore;
            Valore2.Click += HideMotore;




            Inclusione_Testo = new Label()
            {
                Text = "Inclusioni",
                Font = new Font(BackPanel.font3, 12, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
            };
            Laterale.Controls.Add(Inclusione_Testo);
            Esclusione_Testo = new Label()
            {
                Text = "Esclusioni",
                Font = new Font(BackPanel.font3, 12, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
            };
            Laterale.Controls.Add(Esclusione_Testo);
            Inclusione_Testo.Click += ClickNull;
            Esclusione_Testo.Click += ClickNull;
            Inclusione_Testo.Size = new Size(100, 25);
            Esclusione_Testo.Size = new Size(100, 25);

            initializePanels = new Timer()
            {
                Enabled = true,
                Interval = 10,
            };
            initializePanels.Tick += InitializePanels;
            showErrors = new Timer()
            {
                Enabled = true,
                Interval = 50,
            };
            showErrors.Tick += ShowErrors;


            ErroreData = new Label()
            {
                Visible = false,
                Text = "La prima data deve anticipare la seconda",
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.SandyBrown,
                Font = new Font(BackPanel.font3, 10, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
            };
            Controls.Add(ErroreData);
            ErroreData.Location = new Point(250, Data1.Location.Y + Laterale.Location.Y);
            ErroreData.Size = new Size(330, 25);
            ErroreOre = new Label()
            {
                Visible = false,
                Text = "Il primo orario deve antecedere il secondo",
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.SandyBrown,
                Font = new Font(BackPanel.font3, 10, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
            };
            Controls.Add(ErroreOre);
            ErroreOre.Location = new Point(250, Time1.Location.Y + Laterale.Location.Y);
            ErroreOre.Size = new Size(330, 25);
            ErroreValori = new Label()
            {
                Visible = false,
                Text = "Il primo valore dev'essere inferiore al secondo (ammenocchè il secondo non sia 0)",
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.SandyBrown,
                Font = new Font(BackPanel.font3, 10, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
            };
            Controls.Add(ErroreValori);
            ErroreValori.Location = new Point(250, PrimoValore.Location.Y + Laterale.Location.Y);
            ErroreValori.Size = new Size(630, 25);


            PanelMotore_Tipologie = new MotoreRicerca(false, 1);
            Controls.Add(PanelMotore_Tipologie);
            PanelMotore_Attributi = new MotoreRicerca(false, 2);
            Controls.Add(PanelMotore_Attributi);
            PanelMotore_Metodi = new MotoreRicerca(false, 3);
            Controls.Add(PanelMotore_Metodi);
            ClickScaletta(null, null);
            InizializzaValori();

            ResumeLayout();
            initialized = true;
        }
        public void ResizeForm()
        {
            Size = FinestraPrincipale.Finestra.Size;
            Laterale.Size = new Size(240, FinestraPrincipale.Finestra.Height - FinestraPrincipale.BackPanel.Menù.Height - 40);
            if (Scaletta != null) { Scaletta.Size = new Size(Width - Laterale.Width, Height - Scaletta.Location.Y - 50); Scaletta.ResizeForm(); }
            if (Grafici != null) { Grafici.Size = new Size(Width - Laterale.Width, Height - Grafici.Location.Y - 50); Grafici.ResizeForm(); }

            Research_Piccolo();
            PanelInclusione.ResizeAttributi();
            PanelInclusione_Attributi.ResizeAttributi();
            PanelInclusione_Metodi.ResizeAttributi();
            PanelEsclusione.ResizeAttributi();
            PanelEsclusione_Attributi.ResizeAttributi();
            PanelEsclusione_Metodi.ResizeAttributi();

            Size sizeMotore = new Size((int)(Laterale.Width * 1.4), Laterale.Height);
            Point locationMotore = new Point(Laterale.Location.X + Laterale.Width , Laterale.Location.Y);

            PanelMotore_Attributi.Location = locationMotore;
            PanelMotore_Attributi.Size = sizeMotore;
            PanelMotore_Attributi.ResizeForm();
            PanelMotore_Tipologie.Location = locationMotore;
            PanelMotore_Tipologie.Size = sizeMotore;
            PanelMotore_Tipologie.ResizeForm();
            PanelMotore_Metodi.Location = locationMotore;
            PanelMotore_Metodi.Size = sizeMotore;
            PanelMotore_Metodi.ResizeForm();

            Laterale.Visible = true;
        }
        void InizializzaValori()
        {
            Input.RefreshData();
            Data2.Value = new DateTime(Input.data_attuale[5], Input.data_attuale[4], Input.data_attuale[3], 23, 59, 0); Time2.Value = Data2.Value;
            if (Input.eventi.Count == 0) { Data1.Value = new DateTime(Input.data_attuale[5], Input.data_attuale[4], Input.data_attuale[3], 0, 0, 0); Time1.Value = Data1.Value; }
            else { Data1.Value = new DateTime(Input.eventi[0].GetData()[5], Input.eventi[0].GetData()[4], Input.eventi[0].GetData()[3], 0, 0, 0); Time1.Value = Data1.Value; }
        }
        public void RefreshLocation()
        {
            Console.WriteLine("Refreshed");
            if (PanelInclusione == null || PanelInclusione_Attributi == null || PanelEsclusione == null || PanelEsclusione_Attributi == null || PanelInclusione_Metodi == null || PanelEsclusione_Metodi == null) return;

            Go_Calendar_txt.Location = new Point(Width - 210, 50);
            Go_Calendar_Piccolo();
            Research_Piccolo();
            L_Scaletta.Location = new Point(Laterale.Location.X + Laterale.Width + 20, Laterale.Location.Y + 70 - L_Scaletta.Height);
            L_Grafici.Location = new Point(Laterale.Location.X + Laterale.Width + 20 + (int)(L_Scaletta.Width*1), Laterale.Location.Y + 70 - L_Grafici.Height);
            L_Scaletta.Visible = true;
            L_Grafici.Visible = true;
            Inclusione_Testo.Location = new Point(40, PrimoValore.Location.Y + 50);
            PanelInclusione.Location = new System.Drawing.Point(10, Inclusione_Testo.Location.Y + Inclusione_Testo.Height + 2);
            PanelInclusione_Metodi.Location = new System.Drawing.Point(10, PanelInclusione.Location.Y + PanelInclusione.Height + 2);
            PanelInclusione_Attributi.Location = new System.Drawing.Point(10, PanelInclusione_Metodi.Location.Y + PanelInclusione_Metodi.Height + 2);
            Esclusione_Testo.Location = new System.Drawing.Point(40, PanelInclusione_Attributi.Location.Y + PanelInclusione_Attributi.Height + 12);
            PanelEsclusione.Location = new System.Drawing.Point(10, Esclusione_Testo.Location.Y + Esclusione_Testo.Height + 2);
            PanelEsclusione_Metodi.Location = new System.Drawing.Point(10, PanelEsclusione.Location.Y + PanelEsclusione.Height + 2);
            PanelEsclusione_Attributi.Location = new System.Drawing.Point(10, PanelEsclusione_Metodi.Location.Y + PanelEsclusione_Metodi.Height + 2);
            Research_Laterale_Piccolo();
            FinestraPrincipale.BackPanel.Calc_Piccolo();
            FinestraPrincipale.BackPanel.Calc_Pic.Show();
            if (Scaletta != null) Scaletta.Location = new Point(Laterale.Location.X + Laterale.Width + 20, Laterale.Location.Y + 70);
            if (Grafici != null) Grafici.Location = new Point(Laterale.Location.X + Laterale.Width + 20, Laterale.Location.Y + 70);

        }
        void ClickNull(object sender, EventArgs e)
        {
            ClickNull();
        }
        public void ClickNull()
        {
            FinestraPrincipale.BackPanel.Focus();
            HideMotore();
            if (Scaletta != null) { Scaletta.ClickNull(); }
            //if (Grafici != null) { Grafici.ClickNull(); }
        }
        public void HideMotore()
        {
            if (FinestraPrincipale.BackPanel.Panel_Ricerca != null) FinestraPrincipale.BackPanel.Panel_Ricerca.PanelMotore_Attributi.HideMotore();
            if (FinestraPrincipale.BackPanel.Panel_Ricerca != null) FinestraPrincipale.BackPanel.Panel_Ricerca.PanelMotore_Tipologie.HideMotore();
            if (FinestraPrincipale.BackPanel.Panel_Ricerca != null) FinestraPrincipale.BackPanel.Panel_Ricerca.PanelMotore_Metodi.HideMotore();
        }
        private void HideMotore(object sender, EventArgs e)
        {
            HideMotore();
        }
        private void Valore1Got(object sender, EventArgs e)
        {
            Valore1.Text = "";
        }
        private void Valore2Got(object sender, EventArgs e)
        {
            Valore2.Text = "";
        }
        private void Valore1Lost(object sender, EventArgs e)
        {
            Valore1.Text = Funzioni_utili.EstraiValore(Valore1.Text);
        }
        private void Valore2Lost(object sender, EventArgs e)
        {
            Valore2.Text = Funzioni_utili.EstraiValore(Valore2.Text);
        }
        private void ClickIntroito(object sender, EventArgs e)
        {
            ClickNull();
            Introito.BackColor = evidenziatore_primofiltro;
            Spesa.BackColor = backcolor_primofiltro;
            Trasferimento.BackColor = backcolor_primofiltro;
            All.BackColor = backcolor_primofiltro;
            Introito.BorderStyle = BorderStyle.Fixed3D;
            Spesa.BorderStyle = BorderStyle.FixedSingle;
            Trasferimento.BorderStyle = BorderStyle.FixedSingle;
            All.BorderStyle = BorderStyle.FixedSingle;
            primo_filtro = 1;
            resize_datacode = false; attributo = "Introito";
            RicercaEventi();
        }
        private void ClickSpesa(object sender, EventArgs e)
        {
            ClickNull();
            Introito.BackColor = backcolor_primofiltro;
            Spesa.BackColor = evidenziatore_primofiltro;
            Trasferimento.BackColor = backcolor_primofiltro;
            All.BackColor = backcolor_primofiltro;
            Introito.BorderStyle = BorderStyle.FixedSingle;
            Spesa.BorderStyle = BorderStyle.Fixed3D;
            Trasferimento.BorderStyle = BorderStyle.FixedSingle;
            All.BorderStyle = BorderStyle.FixedSingle;
            primo_filtro = 2;
            resize_datacode = false; attributo = "Spesa";
            RicercaEventi();
        }
        private void ClickTrasferimento(object sender, EventArgs e)
        {
            ClickNull();
            Introito.BackColor = backcolor_primofiltro;
            Spesa.BackColor = backcolor_primofiltro;
            Trasferimento.BackColor = evidenziatore_primofiltro;
            All.BackColor = backcolor_primofiltro;
            Introito.BorderStyle = BorderStyle.FixedSingle;
            Spesa.BorderStyle = BorderStyle.FixedSingle;
            Trasferimento.BorderStyle = BorderStyle.Fixed3D;
            All.BorderStyle = BorderStyle.FixedSingle;
            primo_filtro = 3;
            resize_datacode = false; attributo = "Trasferimento";
            RicercaEventi();
        }
        private void ClickAll(object sender, EventArgs e)
        {
            ClickNull();
            Introito.BackColor = backcolor_primofiltro;
            Spesa.BackColor = backcolor_primofiltro;
            Trasferimento.BackColor = backcolor_primofiltro;
            All.BackColor = evidenziatore_primofiltro;
            Introito.BorderStyle = BorderStyle.FixedSingle;
            Spesa.BorderStyle = BorderStyle.FixedSingle;
            Trasferimento.BorderStyle = BorderStyle.FixedSingle;
            All.BorderStyle = BorderStyle.Fixed3D;
            primo_filtro = 0;
            resize_datacode = false; attributo = "All";
            RicercaEventi();
        }
        Color background = Color.PaleTurquoise;
        private void ClickGrafici(object sender, EventArgs e)
        {
            ClickNull();
            L_Grafici.BackColor = background;
            L_Scaletta.BackColor = evidenziatore_primofiltro;
            //L_Grafici.BorderStyle = BorderStyle.Fixed3D;
            //L_Scaletta.BorderStyle = BorderStyle.FixedSingle;
            L_Grafici.BringToFront();
            Update();
            if (Grafici != null) {Grafici.BringToFront(); Grafici.Show();  Scaletta.Hide(); Grafici.Date_delay_initial.Tick += Grafici.DateDelayInitial;}
        }
        private void ClickScaletta(object sender, EventArgs e)
        {
            ClickNull();
            L_Grafici.BackColor = evidenziatore_primofiltro;
            L_Scaletta.BackColor = background; 
            //L_Grafici.BorderStyle = BorderStyle.FixedSingle;
            //L_Scaletta.BorderStyle = BorderStyle.Fixed3D;
            L_Scaletta.BringToFront();
            Update();
            if (Scaletta != null) { Scaletta.BringToFront(); Grafici.Hide(); Scaletta.Show(); }
        }
        public void CloseRicerca()
        {
            Console.WriteLine("---------" + Visible);
            active = false;
            BackPanel.go_to_ricerca = false;
            SendToBack();
            Visible = false;
            FinestraPrincipale.BackPanel.Calc_Piccolo();
            FinestraPrincipale.BackPanel.Calc_Pic.Hide();
            FinestraPrincipale.BackPanel.Ora.Show();
            FinestraPrincipale.BackPanel.Focus();
            //Disposer();
        }
        public void CloseRicerca(object sender, EventArgs e)
        {
            FinestraPrincipale.Finestra.Caricamento.BringToFront();
            CloseRicerca();
            FinestraPrincipale.BackPanel.ShowCalendar();
            FinestraPrincipale.Finestra.Caricamento.SendToBack();
        }
        public void GoToDay(DateTime data)
        {
            CloseRicerca();
            Timer timer = new Timer() { Enabled = true, Interval = 200 };
            timer.Tick += (o, e) =>
            {
                timer.Dispose();
                FinestraPrincipale.BackPanel.Move_toDay("", data);
                FinestraPrincipale.BackPanel.StandardCalendar.Click();
            };
        }
        private void InitializePanels(object sender , EventArgs e)
        {
            initializePanels.Tick -= InitializePanels;

            PanelInclusione = new PanelAttributi() { Visible = true, TabIndex = 6,};
            PanelInclusione.index = 0;
            PanelInclusione.SetPanel(true);
            PanelInclusione.ResizeAttributi();
            Laterale.Controls.Add(PanelInclusione);

            PanelInclusione_Attributi = new PanelAttributi() { Visible = true, TabIndex = 8, };
            PanelInclusione_Attributi.index = 1;
            PanelInclusione_Attributi.SetPanel(true);
            PanelInclusione_Attributi.ResizeAttributi();
            Laterale.Controls.Add(PanelInclusione_Attributi);

            PanelInclusione_Metodi = new PanelAttributi() { Visible = true, TabIndex = 7, };
            PanelInclusione_Metodi.index = 4;
            PanelInclusione_Metodi.SetPanel(true);
            PanelInclusione_Metodi.ResizeAttributi();
            Laterale.Controls.Add(PanelInclusione_Metodi);

            PanelEsclusione = new PanelAttributi() { Visible = true, TabIndex = 9, };
            PanelEsclusione.index = 2;
            PanelEsclusione.SetPanel(true);
            PanelEsclusione.ResizeAttributi();
            Laterale.Controls.Add(PanelEsclusione);

            PanelEsclusione_Attributi = new PanelAttributi() { Visible = true, TabIndex = 11, };
            PanelEsclusione_Attributi.index = 3;
            PanelEsclusione_Attributi.SetPanel(true);
            PanelEsclusione_Attributi.ResizeAttributi();
            Laterale.Controls.Add(PanelEsclusione_Attributi);

            PanelEsclusione_Metodi = new PanelAttributi() { Visible = true, TabIndex = 10, };
            PanelEsclusione_Metodi.index = 5;
            PanelEsclusione_Metodi.SetPanel(true);
            PanelEsclusione_Metodi.ResizeAttributi();
            Laterale.Controls.Add(PanelEsclusione_Metodi);

            Scaletta = new Visualizzazione_Scaletta
            {
                Location = new Point(260, FinestraPrincipale.BackPanel.Menù.Height + 70),
                Visible = false,
            };
            Controls.Add(Scaletta);
            Grafici = new Visualizzazione_Grafici
            {
                Location = new Point(260, FinestraPrincipale.BackPanel.Menù.Height + 70),
                Visible = false,
            };
            Controls.Add(Grafici);

            Laterale.Controls.Add(Research_Laterale);
            Research_Laterale.Click += ResearchClick;
            Research_Laterale.MouseEnter += new EventHandler(Enter_Research_Laterale);
            Research_Laterale.MouseLeave += new EventHandler(Leave_Research_Laterale);
            Research_Laterale_Piccolo();

            ResizeForm();
            Update();
        }
        public void RicercaEventi(object sender, EventArgs e)
        {
            ClickNull();
            RicercaEventi();
        }
        public void RicercaEventi()
        {
            ClickNull();
            ErroreData.Hide();
            ErroreValori.Hide();
            ErroreOre.Hide();
            System.Threading.Thread Ricerca = new System.Threading.Thread(StartThread);
            Ricerca.Start();
            Visible = true;
        }
        void StartThread()
        {
            try
            {
                if (Data1.Value.Year < 2000) datacode1 = 0;
                else datacode1 = Date.Codifica(new int[] { 0, 0, 0, Data1.Value.Day, Data1.Value.Month, Data1.Value.Year });
                if (Data2.Value.Year < 200) datacode2 = 0;
                else datacode2 = Date.Codifica(new int[] { 0, 0, 24, Data2.Value.Day, Data2.Value.Month, Data2.Value.Year });
                time1 = new Point(Time1.Value.Hour, Time1.Value.Minute);
                time2 = new Point(Time2.Value.Hour, Time2.Value.Minute);
                valore1 = Convert.ToDouble(Valore1.Text.Substring(0, Valore1.Text.Length - 1));
                valore2 = Convert.ToDouble(Valore2.Text.Substring(0, Valore2.Text.Length - 1));
                bool errore = false;
                if (datacode2 < datacode1) { errore = true; erroredata = true; }
                if (time2.X < time1.X) { errore = true; erroreore = true; } else if(time2.X == time1.X && time2.Y < time1.Y) { errore = true; erroreore = true; }
                if (valore2 < valore1 && valore2 != 0) { errore = true; errorevalori = true; }
                if (errore) return;
                tipi_inclusi.Clear();
                attributi_inclusi.Clear();
                metodi_inclusi.Clear();
                tipi_esclusi.Clear();
                attributi_esclusi.Clear();
                metodi_esclusi.Clear();
                if (PanelInclusione.Controls.OfType<TextBox>().Count() == 1) foreach (string stringa in Input.tipi_scremati) tipi_inclusi.Add(stringa);
                else foreach (TextBox txt in PanelInclusione.Controls.OfType<TextBox>()) if (!txt.Text.Contains("\u2192")) foreach (string stringa in Input.tipi_scremati) if (stringa == Funzioni_utili.Scremato(txt.Text)) tipi_inclusi.Add(Funzioni_utili.Scremato(txt.Text));
                if (PanelInclusione_Attributi.Controls.OfType<TextBox>().Count() == 1) foreach (string stringa in Input.all_attributi) attributi_inclusi.Add(stringa);
                else foreach (TextBox txt in PanelInclusione_Attributi.Controls.OfType<TextBox>()) if (!txt.Text.Contains("\u2192")) foreach (string stringa in Input.all_attributi) if (stringa == Funzioni_utili.Scremato(txt.Text)) attributi_inclusi.Add(Funzioni_utili.Scremato(txt.Text));
                if (PanelInclusione_Metodi.Controls.OfType<TextBox>().Count() == 1) foreach (string stringa in Input.metodi_scremati) metodi_inclusi.Add(stringa);
                else foreach (TextBox txt in PanelInclusione_Metodi.Controls.OfType<TextBox>()) if (!txt.Text.Contains("\u2192")) foreach (string stringa in Input.metodi_scremati) if (stringa == Funzioni_utili.Scremato(txt.Text)) metodi_inclusi.Add(Funzioni_utili.Scremato(txt.Text));
                foreach (TextBox txt in PanelEsclusione.Controls.OfType<TextBox>()) if (!txt.Text.Contains("\u2192")) foreach (string stringa in Input.tipi_scremati) if (stringa == Funzioni_utili.Scremato(txt.Text)) tipi_esclusi.Add(Funzioni_utili.Scremato(txt.Text));
                foreach (TextBox txt in PanelEsclusione_Attributi.Controls.OfType<TextBox>()) if (!txt.Text.Contains("\u2192")) foreach (string stringa in Input.all_attributi) if (stringa == Funzioni_utili.Scremato(txt.Text)) attributi_esclusi.Add(Funzioni_utili.Scremato(txt.Text));
                foreach (TextBox txt in PanelEsclusione_Metodi.Controls.OfType<TextBox>()) if (!txt.Text.Contains("\u2192")) foreach (string stringa in Input.metodi_scremati) if (stringa == Funzioni_utili.Scremato(txt.Text)) metodi_esclusi.Add(Funzioni_utili.Scremato(txt.Text));
                
                bool uguaglianza = true;
                try
                {
                    if (tipi_inclusi.Count != previous_tipi_inclusi.Count) uguaglianza = false;
                    if (attributi_inclusi.Count != previous_attributi_inclusi.Count) uguaglianza = false;
                    if (metodi_inclusi.Count != previous_metodi_inclusi.Count) uguaglianza = false;
                    if (tipi_esclusi.Count != previous_tipi_esclusi.Count) uguaglianza = false;
                    if (attributi_esclusi.Count != previous_attributi_esclusi.Count) uguaglianza = false;
                    if (metodi_esclusi.Count != previous_metodi_esclusi.Count) uguaglianza = false;
                    for (int i = 0; i < tipi_inclusi.Count; i++) if (previous_tipi_inclusi[i] != tipi_inclusi[i]) uguaglianza = false;
                    for (int i = 0; i < attributi_inclusi.Count; i++) if (previous_attributi_inclusi[i] != attributi_inclusi[i]) uguaglianza = false;
                    for (int i = 0; i < metodi_inclusi.Count; i++) if (previous_metodi_inclusi[i] != metodi_inclusi[i]) uguaglianza = false;
                    for (int i = 0; i < tipi_esclusi.Count; i++) if (previous_tipi_esclusi[i] != tipi_esclusi[i]) uguaglianza = false;
                    for (int i = 0; i < attributi_esclusi.Count; i++) if (previous_attributi_esclusi[i] != attributi_esclusi[i]) uguaglianza = false;
                    for (int i = 0; i < metodi_esclusi.Count; i++) if (previous_metodi_esclusi[i] != metodi_esclusi[i]) uguaglianza = false;
                }
                catch (Exception) { Console.WriteLine("Errore Ricerca"); uguaglianza = false; }
                if (previous_primo_filtro == primo_filtro &&
                    previous_datacode1 == datacode1 &&
                    previous_datacode2 == datacode2 &&
                    previous_time1 == time1 &&
                    previous_time2 == time2 &&
                    previous_valore1 == valore1 &&
                    previous_valore2 == valore2 &&
                    uguaglianza)
                {
                    if (Grafici != null) Grafici.acquisizione_dati.Tick += Grafici.AcquisizioneDati;
                    return;
                }
                
                previous_primo_filtro = primo_filtro;
                previous_datacode1 = datacode1;
                previous_datacode2 = datacode2;
                previous_time1 = time1;
                previous_time2 = time2;
                previous_valore1 = valore1;
                previous_valore2 = valore2;
                previous_tipi_inclusi.Clear();
                previous_attributi_inclusi.Clear();
                previous_metodi_inclusi.Clear();
                previous_tipi_esclusi.Clear();
                previous_attributi_esclusi.Clear();
                previous_metodi_esclusi.Clear();
                foreach (string stringa in tipi_inclusi) previous_tipi_inclusi.Add(stringa);
                foreach (string stringa in attributi_inclusi) previous_attributi_inclusi.Add(stringa);
                foreach (string stringa in metodi_inclusi) previous_metodi_inclusi.Add(stringa);
                foreach (string stringa in tipi_esclusi) previous_tipi_esclusi.Add(stringa);
                foreach (string stringa in attributi_esclusi) previous_attributi_esclusi.Add(stringa);
                foreach (string stringa in metodi_esclusi) previous_metodi_esclusi.Add(stringa);

                Fill_Eventi_filtrati();
                eventi_filtrati = Eventi.Order_datacode_valore(eventi_filtrati);
                if (Scaletta != null) Scaletta.acquisizione_dati.Tick += Scaletta.AcquisizioneDati;
                if (Grafici != null) Grafici.acquisizione_dati.Tick += Grafici.AcquisizioneDati;


            }
            catch (Exception) { Console.WriteLine("Errore Ricerca2"); }
        }

        public void Fill_Eventi_filtrati()
        {
            try
            {
                eventi_filtrati.Clear();
                foreach (Eventi evento in Input.eventi_scremati)
                {
                    if (evento.Get_Attributo() == "Note") continue;
                    //if (primo_filtro == 1) if (evento.attributo != "Introito") continue;
                    //if (primo_filtro == 2) if (evento.attributo != "Spesa") continue;
                    //if (primo_filtro == 3) if (evento.attributo != "Trasferimento") continue;
                    if (evento.GetDatacode() < datacode1 || evento.GetDatacode() > datacode2) continue;
                    Point ora = new Point(evento.GetData()[2], evento.GetData()[1]);
                    if (ora.X * 60 + ora.Y < time1.X * 60 + time1.Y || ora.X * 60 + ora.Y > time2.X * 60 + time2.Y) continue;
                    if (valore2 == 0) { if (evento.GetValore() < valore1) continue; }
                    else if (evento.GetValore() < valore1 || evento.GetValore() > valore2) continue;
                    if (evento.Get_Attributo() == "Trasferimento")
                    {
                        if (tipi_inclusi.Count != Input.tipi_scremati.Count) continue;
                        if (!metodi_inclusi.Contains(evento.GetTipo()) && !metodi_inclusi.Contains(evento.GetMetodo())) continue;
                        if (metodi_esclusi.Contains(evento.GetTipo()) || metodi_esclusi.Contains(evento.GetMetodo())) continue;
                    }
                    else
                    {
                        if (!tipi_inclusi.Contains(evento.GetTipo())) continue;
                        if (tipi_esclusi.Contains(evento.GetTipo())) continue;
                        if (!metodi_inclusi.Contains(evento.GetMetodo())) continue;
                        if (metodi_esclusi.Contains(evento.GetMetodo())) continue;
                    }

                    bool inc = false, esc = false;
                    if (attributi_esclusi.Count != 0)
                    {
                        foreach (string attr in evento.GetAttributi())
                        {
                            if (attributi_esclusi.Contains(attr)) { esc = true; break; }
                        }
                    }
                    if (esc) continue;
                    if (attributi_inclusi.Count != Input.all_attributi.Count)
                    {
                        foreach (string attr in evento.GetAttributi())
                        {
                            if (attributi_inclusi.Count == 0) inc = true; else if (attributi_inclusi.Contains(attr)) inc = true;
                        }
                    }
                    else { inc = true; }
                    if (!inc) continue;

                    eventi_filtrati.Add(evento);
                }
                
            }
            catch (Exception) { Console.WriteLine("Errore Filtraggio"); }
            
        }
        void ShowErrors(object sender, EventArgs e)
        {
            if (erroredata) { ErroreData.Show(); ErroreData.BringToFront(); erroredata = false; }
            if (errorevalori) { ErroreValori.Show(); ErroreValori.BringToFront(); errorevalori = false; }
            if (erroreore) { ErroreOre.Show(); ErroreOre.BringToFront(); erroreore = false; }
        }
        private void ResearchClick(object sender, EventArgs e)
        {
            ClickNull();
            RicercaEventi();
        }
        private void Enter_Research(object sender, EventArgs e)
        {
            Research_Grande();
        }
        private void Leave_Research(object sender, EventArgs e)
        {
            Research_Piccolo();
        }
        private void Research_Grande()
        {
            Research.Size = new Size(50, 50);
            Research.Location = new Point(Research.Location.X - 2, Research.Location.Y - 2);
        }
        private void Research_Piccolo()
        {
            Research.Size = new Size(46, 46);
            Research.Location = new Point(Width - 330, 42);
        }
        private void Enter_Research_Laterale(object sender, EventArgs e)
        {
            Research_Laterale_Grande();
        }
        private void Leave_Research_Laterale(object sender, EventArgs e)
        {
            Research_Laterale_Piccolo();
        }
        private void Research_Laterale_Grande()
        {
            Research_Laterale.Size = new Size(58, 58);
            Research_Laterale.Location = new Point(Research_Laterale.Location.X - 2, Research_Laterale.Location.Y - 2);
        }
        private void Research_Laterale_Piccolo()
        {
            Research_Laterale.Size = new Size(54, 54);
            Research_Laterale.Location = new Point(150, PanelEsclusione_Attributi.Location.Y + PanelEsclusione_Attributi.Height + 10);
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
            Go_Calendar.Size = new Size(50, 50);
            Go_Calendar.Location = new Point(Width - 102, 35);
        }
        private void Go_Calendar_Piccolo()
        {
            Go_Calendar.Size = new Size(46, 46);
            Go_Calendar.Location = new Point(Width - 100, 37);
        }
        private void EnterRicerca(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.Control)
            {
                RicercaEventi();
                e.SuppressKeyPress = true;
                return;
            }
        }
        private void SimplyEnterRicerca(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                RicercaEventi();
                e.SuppressKeyPress = true;
                return;
            }
        }
    }
}
