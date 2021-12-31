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
    public class Pannello_StandardCalendar : Panel
    {
        private Label button_precedente;
        private Label button_successivo;
        private Label label_mese;
        private Label label_anno;

        private Label Guadagno_Complessivo;
        private Label Spesa_Complessiva;
        private Label Guadagno_Pic;
        private Label Spesa_Pic;
        private Label Guadagno_Txt;
        private Label Spesa_Txt;

        private Timer timer;
        
        public static bool wait = false;
        
        public Panel Principale;
        private Panel Settimana;
        private readonly Label[] label = new Label[7];
        public readonly Bottoni[] button = new Bottoni[37];
        private readonly Bottoni[] guadagno = new Bottoni[37];
        private readonly Label[] spesa = new Label[37];
        private readonly Label[] guadagnopic = new Label[37];
        private readonly Label[] spesapic = new Label[37];
        private readonly Label[] trasferimentopic = new Label[37];

        static public List<int> giorni_festivi = new List<int>();

        static public List<Eventi> eventi_giorno = new List<Eventi>();
        static private List<Eventi> eventi_mese = new List<Eventi>();

        static private double guadagno_mese;
        static private double spesa_mese;
        static readonly private double proporzione_massima = 2.5;  //  Larghezza/Altezza
        static private Size taglia_piccola, taglia_grande;
        static public int button_clicked = 0;
        static private int numero_giorni = 0;
        public int giorno = 0;
        public int mese;
        public int anno;
        static public int posizione_iniziale = 0;
        static public int lastWidth;
        static public int lastHeight;
        static public bool resize;
        public Label orecchietta;
        public PanelRicorrenza Ricorrenza;

        public void Disposer()
        {
            if (FinestraPrincipale.BackPanel.Panel_Giorno != null) FinestraPrincipale.BackPanel.Panel_Giorno.Disposer();
            Ricorrenza.Disposer();
            orecchietta.BackgroundImage.Dispose();
            orecchietta.Dispose();
            timer.Dispose();
            Dispose();
        }
        public void InitializePannelloCalendar()
        {
            DoubleBuffered = true;
            wait = false;
            FinestraPrincipale.BackPanel.StandardCalendar = this;
            mese = Input.data_utile[4]; anno = Input.data_utile[5];
            giorni_festivi.Add(6); giorni_festivi.Add(7);
            Calcoli_Mese();

            label_mese = new System.Windows.Forms.Label();
            label_anno = new System.Windows.Forms.Label();
            Guadagno_Complessivo = new System.Windows.Forms.Label();
            Spesa_Complessiva = new System.Windows.Forms.Label();
            Principale = new System.Windows.Forms.Panel();

            MouseClick += FinestraPrincipale.BackPanel.ClickNull;

            //Definizione Pannello bottoni
            Principale = new Panel();
            Principale.MouseEnter += Enter;
            Principale.MouseClick += FinestraPrincipale.BackPanel.ClickNull;
            Controls.Add(Principale);

            timer = new Timer()
            {
                Enabled = true,
                Interval = 100,
            };
            timer.Tick += TimerF;


            orecchietta = new Label
            {
                BackColor = System.Drawing.Color.Transparent,
                BackgroundImage = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("Ricorrenze"))),
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
            };
            orecchietta.Visible = false;
            Controls.Add(orecchietta);
            orecchietta.MouseEnter += (o, e) => { if (!Ricorrenza.Visible) RicorrenzeShow(); };
            orecchietta.MouseLeave += (o, e) => { if (!Ricorrenza.Visible) RicorrenzeHide(); };
            orecchietta.Click += (o, e) =>
            {
                if (Ricorrenza.Visible) Ricorrenza.HideRic();
                else { Ricorrenza.ShowRic(); Ricorrenza.BringToFront(); }
            };
            Ricorrenza = new PanelRicorrenza();
            Controls.Add(Ricorrenza);

            //Definizione elementi mensili
            Guadagno_Pic = new Label
            {
                BackColor = System.Drawing.Color.Transparent,
                BackgroundImage = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("Guadagno_Complessivo"))),
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
            };
            Controls.Add(Guadagno_Pic);
            Guadagno_Pic.MouseClick += FinestraPrincipale.BackPanel.ClickNull;

            Spesa_Pic = new Label
            {
                BackColor = System.Drawing.Color.Transparent,
                BackgroundImage = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("Spesa_Complessiva"))),
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
            };
            Controls.Add(Spesa_Pic);
            Spesa_Pic.MouseClick += FinestraPrincipale.BackPanel.ClickNull;

            Guadagno_Complessivo = new Label();
            Controls.Add(Guadagno_Complessivo);
            Guadagno_Complessivo.MouseClick += FinestraPrincipale.BackPanel.ClickNull;

            Spesa_Complessiva = new Label();
            Controls.Add(Spesa_Complessiva);
            Spesa_Complessiva.MouseClick += FinestraPrincipale.BackPanel.ClickNull;

            Guadagno_Txt = new Label
            {
                AutoSize = true,
                Text = "Introito mese"
            };
            Controls.Add(Guadagno_Txt);
            Guadagno_Txt.MouseClick += FinestraPrincipale.BackPanel.ClickNull;

            Spesa_Txt = new Label
            {
                AutoSize = true,
                Text = "Spesa mese"
            };
            Controls.Add(Spesa_Txt);
            Spesa_Txt.MouseClick += FinestraPrincipale.BackPanel.ClickNull;

            //Definizione bottoni
            for (int i = 0; i < 37; i++)
            {
                button[i] = new Bottoni
                {
                    index = i
                };
                Principale.Controls.Add(button[i]);
            }


            #region Button Listener
            /*
            button[0].MouseClick += new MouseEventHandler(Button_Click0);
            guadagno[0].MouseClick += new MouseEventHandler(Button_Click0);
            spesa[0].MouseClick += new MouseEventHandler(Button_Click0);
            button[1].MouseClick += new MouseEventHandler(Button_Click1);
            guadagno[1].MouseClick += new MouseEventHandler(Button_Click1);
            spesa[1].MouseClick += new MouseEventHandler(Button_Click1);
            button[2].MouseClick += new MouseEventHandler(Button_Click2);
            guadagno[2].MouseClick += new MouseEventHandler(Button_Click2);
            spesa[2].MouseClick += new MouseEventHandler(Button_Click2);
            button[3].MouseClick += new MouseEventHandler(Button_Click3);
            guadagno[3].MouseClick += new MouseEventHandler(Button_Click3);
            spesa[3].MouseClick += new MouseEventHandler(Button_Click3);
            button[4].MouseClick += new MouseEventHandler(Button_Click4);
            guadagno[4].MouseClick += new MouseEventHandler(Button_Click4);
            spesa[4].MouseClick += new MouseEventHandler(Button_Click4);
            button[5].MouseClick += new MouseEventHandler(Button_Click5);
            guadagno[5].MouseClick += new MouseEventHandler(Button_Click5);
            spesa[5].MouseClick += new MouseEventHandler(Button_Click5);
            button[6].MouseClick += new MouseEventHandler(Button_Click6);
            guadagno[6].MouseClick += new MouseEventHandler(Button_Click6);
            spesa[6].MouseClick += new MouseEventHandler(Button_Click6);
            button[7].MouseClick += new MouseEventHandler(Button_Click7);
            guadagno[7].MouseClick += new MouseEventHandler(Button_Click7);
            spesa[7].MouseClick += new MouseEventHandler(Button_Click7);
            button[8].MouseClick += new MouseEventHandler(Button_Click8);
            guadagno[8].MouseClick += new MouseEventHandler(Button_Click8);
            spesa[8].MouseClick += new MouseEventHandler(Button_Click8);
            button[9].MouseClick += new MouseEventHandler(Button_Click9);
            guadagno[9].MouseClick += new MouseEventHandler(Button_Click9);
            spesa[9].MouseClick += new MouseEventHandler(Button_Click9);
            button[10].MouseClick += new MouseEventHandler(Button_Click10);
            guadagno[10].MouseClick += new MouseEventHandler(Button_Click10);
            spesa[10].MouseClick += new MouseEventHandler(Button_Click10);
            button[11].MouseClick += new MouseEventHandler(Button_Click11);
            guadagno[11].MouseClick += new MouseEventHandler(Button_Click11);
            spesa[11].MouseClick += new MouseEventHandler(Button_Click11);
            button[12].MouseClick += new MouseEventHandler(Button_Click12);
            guadagno[12].MouseClick += new MouseEventHandler(Button_Click12);
            spesa[12].MouseClick += new MouseEventHandler(Button_Click12);
            button[13].MouseClick += new MouseEventHandler(Button_Click13);
            guadagno[13].MouseClick += new MouseEventHandler(Button_Click13);
            spesa[13].MouseClick += new MouseEventHandler(Button_Click13);
            button[14].MouseClick += new MouseEventHandler(Button_Click14);
            guadagno[14].MouseClick += new MouseEventHandler(Button_Click14);
            spesa[14].MouseClick += new MouseEventHandler(Button_Click14);
            button[15].MouseClick += new MouseEventHandler(Button_Click15);
            guadagno[15].MouseClick += new MouseEventHandler(Button_Click15);
            spesa[15].MouseClick += new MouseEventHandler(Button_Click15);
            button[16].MouseClick += new MouseEventHandler(Button_Click16);
            guadagno[16].MouseClick += new MouseEventHandler(Button_Click16);
            spesa[16].MouseClick += new MouseEventHandler(Button_Click16);
            button[17].MouseClick += new MouseEventHandler(Button_Click17);
            guadagno[17].MouseClick += new MouseEventHandler(Button_Click17);
            spesa[17].MouseClick += new MouseEventHandler(Button_Click17);
            button[18].MouseClick += new MouseEventHandler(Button_Click18);
            guadagno[18].MouseClick += new MouseEventHandler(Button_Click18);
            spesa[18].MouseClick += new MouseEventHandler(Button_Click18);
            button[19].MouseClick += new MouseEventHandler(Button_Click19);
            guadagno[19].MouseClick += new MouseEventHandler(Button_Click19);
            spesa[19].MouseClick += new MouseEventHandler(Button_Click19);
            button[20].MouseClick += new MouseEventHandler(Button_Click20);
            guadagno[20].MouseClick += new MouseEventHandler(Button_Click20);
            spesa[20].MouseClick += new MouseEventHandler(Button_Click20);
            button[21].MouseClick += new MouseEventHandler(Button_Click21);
            guadagno[21].MouseClick += new MouseEventHandler(Button_Click21);
            spesa[21].MouseClick += new MouseEventHandler(Button_Click21);
            button[22].MouseClick += new MouseEventHandler(Button_Click22);
            guadagno[22].MouseClick += new MouseEventHandler(Button_Click22);
            spesa[22].MouseClick += new MouseEventHandler(Button_Click22);
            button[23].MouseClick += new MouseEventHandler(Button_Click23);
            guadagno[23].MouseClick += new MouseEventHandler(Button_Click23);
            spesa[23].MouseClick += new MouseEventHandler(Button_Click23);
            button[24].MouseClick += new MouseEventHandler(Button_Click24);
            guadagno[24].MouseClick += new MouseEventHandler(Button_Click24);
            spesa[24].MouseClick += new MouseEventHandler(Button_Click24);
            button[25].MouseClick += new MouseEventHandler(Button_Click25);
            guadagno[25].MouseClick += new MouseEventHandler(Button_Click25);
            spesa[25].MouseClick += new MouseEventHandler(Button_Click25);
            button[26].MouseClick += new MouseEventHandler(Button_Click26);
            guadagno[26].MouseClick += new MouseEventHandler(Button_Click26);
            spesa[26].MouseClick += new MouseEventHandler(Button_Click26);
            button[27].MouseClick += new MouseEventHandler(Button_Click27);
            guadagno[27].MouseClick += new MouseEventHandler(Button_Click27);
            spesa[27].MouseClick += new MouseEventHandler(Button_Click27);
            button[28].MouseClick += new MouseEventHandler(Button_Click28);
            guadagno[28].MouseClick += new MouseEventHandler(Button_Click28);
            spesa[28].MouseClick += new MouseEventHandler(Button_Click28);
            button[29].MouseClick += new MouseEventHandler(Button_Click29);
            guadagno[29].MouseClick += new MouseEventHandler(Button_Click29);
            spesa[29].MouseClick += new MouseEventHandler(Button_Click29);
            button[30].MouseClick += new MouseEventHandler(Button_Click30);
            guadagno[30].MouseClick += new MouseEventHandler(Button_Click30);
            spesa[30].MouseClick += new MouseEventHandler(Button_Click30);
            button[31].MouseClick += new MouseEventHandler(Button_Click31);
            guadagno[31].MouseClick += new MouseEventHandler(Button_Click31);
            spesa[31].MouseClick += new MouseEventHandler(Button_Click31);
            button[32].MouseClick += new MouseEventHandler(Button_Click32);
            guadagno[32].MouseClick += new MouseEventHandler(Button_Click32);
            spesa[32].MouseClick += new MouseEventHandler(Button_Click32);
            button[33].MouseClick += new MouseEventHandler(Button_Click33);
            guadagno[33].MouseClick += new MouseEventHandler(Button_Click33);
            spesa[33].MouseClick += new MouseEventHandler(Button_Click33);
            button[34].MouseClick += new MouseEventHandler(Button_Click34);
            guadagno[34].MouseClick += new MouseEventHandler(Button_Click34);
            spesa[34].MouseClick += new MouseEventHandler(Button_Click34);
            button[35].MouseClick += new MouseEventHandler(Button_Click35);
            guadagno[35].MouseClick += new MouseEventHandler(Button_Click35);
            spesa[35].MouseClick += new MouseEventHandler(Button_Click35);
            button[36].MouseClick += new MouseEventHandler(Button_Click36);
            guadagno[36].MouseClick += new MouseEventHandler(Button_Click36);
            spesa[36].MouseClick += new MouseEventHandler(Button_Click36);



            trasferimentopic[0].MouseClick += new MouseEventHandler(Button_Click0);
            guadagnopic[0].MouseClick += new MouseEventHandler(Button_Click0);
            spesapic[0].MouseClick += new MouseEventHandler(Button_Click0);
            trasferimentopic[1].MouseClick += new MouseEventHandler(Button_Click1);
            guadagnopic[1].MouseClick += new MouseEventHandler(Button_Click1);
            spesapic[1].MouseClick += new MouseEventHandler(Button_Click1);
            trasferimentopic[2].MouseClick += new MouseEventHandler(Button_Click2);
            guadagnopic[2].MouseClick += new MouseEventHandler(Button_Click2);
            spesapic[2].MouseClick += new MouseEventHandler(Button_Click2);
            trasferimentopic[3].MouseClick += new MouseEventHandler(Button_Click3);
            guadagnopic[3].MouseClick += new MouseEventHandler(Button_Click3);
            spesapic[3].MouseClick += new MouseEventHandler(Button_Click3);
            trasferimentopic[4].MouseClick += new MouseEventHandler(Button_Click4);
            guadagnopic[4].MouseClick += new MouseEventHandler(Button_Click4);
            spesapic[4].MouseClick += new MouseEventHandler(Button_Click4);
            trasferimentopic[5].MouseClick += new MouseEventHandler(Button_Click5);
            guadagnopic[5].MouseClick += new MouseEventHandler(Button_Click5);
            spesapic[5].MouseClick += new MouseEventHandler(Button_Click5);
            trasferimentopic[6].MouseClick += new MouseEventHandler(Button_Click6);
            guadagnopic[6].MouseClick += new MouseEventHandler(Button_Click6);
            spesapic[6].MouseClick += new MouseEventHandler(Button_Click6);
            trasferimentopic[7].MouseClick += new MouseEventHandler(Button_Click7);
            guadagnopic[7].MouseClick += new MouseEventHandler(Button_Click7);
            spesapic[7].MouseClick += new MouseEventHandler(Button_Click7);
            trasferimentopic[8].MouseClick += new MouseEventHandler(Button_Click8);
            guadagnopic[8].MouseClick += new MouseEventHandler(Button_Click8);
            spesapic[8].MouseClick += new MouseEventHandler(Button_Click8);
            trasferimentopic[9].MouseClick += new MouseEventHandler(Button_Click9);
            guadagnopic[9].MouseClick += new MouseEventHandler(Button_Click9);
            spesapic[9].MouseClick += new MouseEventHandler(Button_Click9);
            trasferimentopic[10].MouseClick += new MouseEventHandler(Button_Click10);
            guadagnopic[10].MouseClick += new MouseEventHandler(Button_Click10);
            spesapic[10].MouseClick += new MouseEventHandler(Button_Click10);
            trasferimentopic[11].MouseClick += new MouseEventHandler(Button_Click11);
            guadagnopic[11].MouseClick += new MouseEventHandler(Button_Click11);
            spesapic[11].MouseClick += new MouseEventHandler(Button_Click11);
            trasferimentopic[12].MouseClick += new MouseEventHandler(Button_Click12);
            guadagnopic[12].MouseClick += new MouseEventHandler(Button_Click12);
            spesapic[12].MouseClick += new MouseEventHandler(Button_Click12);
            trasferimentopic[13].MouseClick += new MouseEventHandler(Button_Click13);
            guadagnopic[13].MouseClick += new MouseEventHandler(Button_Click13);
            spesapic[13].MouseClick += new MouseEventHandler(Button_Click13);
            trasferimentopic[14].MouseClick += new MouseEventHandler(Button_Click14);
            guadagnopic[14].MouseClick += new MouseEventHandler(Button_Click14);
            spesapic[14].MouseClick += new MouseEventHandler(Button_Click14);
            trasferimentopic[15].MouseClick += new MouseEventHandler(Button_Click15);
            guadagnopic[15].MouseClick += new MouseEventHandler(Button_Click15);
            spesapic[15].MouseClick += new MouseEventHandler(Button_Click15);
            trasferimentopic[16].MouseClick += new MouseEventHandler(Button_Click16);
            guadagnopic[16].MouseClick += new MouseEventHandler(Button_Click16);
            spesapic[16].MouseClick += new MouseEventHandler(Button_Click16);
            trasferimentopic[17].MouseClick += new MouseEventHandler(Button_Click17);
            guadagnopic[17].MouseClick += new MouseEventHandler(Button_Click17);
            spesapic[17].MouseClick += new MouseEventHandler(Button_Click17);
            trasferimentopic[18].MouseClick += new MouseEventHandler(Button_Click18);
            guadagnopic[18].MouseClick += new MouseEventHandler(Button_Click18);
            spesapic[18].MouseClick += new MouseEventHandler(Button_Click18);
            trasferimentopic[19].MouseClick += new MouseEventHandler(Button_Click19);
            guadagnopic[19].MouseClick += new MouseEventHandler(Button_Click19);
            spesapic[19].MouseClick += new MouseEventHandler(Button_Click19);
            trasferimentopic[20].MouseClick += new MouseEventHandler(Button_Click20);
            guadagnopic[20].MouseClick += new MouseEventHandler(Button_Click20);
            spesapic[20].MouseClick += new MouseEventHandler(Button_Click20);
            trasferimentopic[21].MouseClick += new MouseEventHandler(Button_Click21);
            guadagnopic[21].MouseClick += new MouseEventHandler(Button_Click21);
            spesapic[21].MouseClick += new MouseEventHandler(Button_Click21);
            trasferimentopic[22].MouseClick += new MouseEventHandler(Button_Click22);
            guadagnopic[22].MouseClick += new MouseEventHandler(Button_Click22);
            spesapic[22].MouseClick += new MouseEventHandler(Button_Click22);
            trasferimentopic[23].MouseClick += new MouseEventHandler(Button_Click23);
            guadagnopic[23].MouseClick += new MouseEventHandler(Button_Click23);
            spesapic[23].MouseClick += new MouseEventHandler(Button_Click23);
            trasferimentopic[24].MouseClick += new MouseEventHandler(Button_Click24);
            guadagnopic[24].MouseClick += new MouseEventHandler(Button_Click24);
            spesapic[24].MouseClick += new MouseEventHandler(Button_Click24);
            trasferimentopic[25].MouseClick += new MouseEventHandler(Button_Click25);
            guadagnopic[25].MouseClick += new MouseEventHandler(Button_Click25);
            spesapic[25].MouseClick += new MouseEventHandler(Button_Click25);
            trasferimentopic[26].MouseClick += new MouseEventHandler(Button_Click26);
            guadagnopic[26].MouseClick += new MouseEventHandler(Button_Click26);
            spesapic[26].MouseClick += new MouseEventHandler(Button_Click26);
            trasferimentopic[27].MouseClick += new MouseEventHandler(Button_Click27);
            guadagnopic[27].MouseClick += new MouseEventHandler(Button_Click27);
            spesapic[27].MouseClick += new MouseEventHandler(Button_Click27);
            trasferimentopic[28].MouseClick += new MouseEventHandler(Button_Click28);
            guadagnopic[28].MouseClick += new MouseEventHandler(Button_Click28);
            spesapic[28].MouseClick += new MouseEventHandler(Button_Click28);
            trasferimentopic[29].MouseClick += new MouseEventHandler(Button_Click29);
            guadagnopic[29].MouseClick += new MouseEventHandler(Button_Click29);
            spesapic[29].MouseClick += new MouseEventHandler(Button_Click29);
            trasferimentopic[30].MouseClick += new MouseEventHandler(Button_Click30);
            guadagnopic[30].MouseClick += new MouseEventHandler(Button_Click30);
            spesapic[30].MouseClick += new MouseEventHandler(Button_Click30);
            trasferimentopic[31].MouseClick += new MouseEventHandler(Button_Click31);
            guadagnopic[31].MouseClick += new MouseEventHandler(Button_Click31);
            spesapic[31].MouseClick += new MouseEventHandler(Button_Click31);
            trasferimentopic[32].MouseClick += new MouseEventHandler(Button_Click32);
            guadagnopic[32].MouseClick += new MouseEventHandler(Button_Click32);
            spesapic[32].MouseClick += new MouseEventHandler(Button_Click32);
            trasferimentopic[33].MouseClick += new MouseEventHandler(Button_Click33);
            guadagnopic[33].MouseClick += new MouseEventHandler(Button_Click33);
            spesapic[33].MouseClick += new MouseEventHandler(Button_Click33);
            trasferimentopic[34].MouseClick += new MouseEventHandler(Button_Click34);
            guadagnopic[34].MouseClick += new MouseEventHandler(Button_Click34);
            spesapic[34].MouseClick += new MouseEventHandler(Button_Click34);
            trasferimentopic[35].MouseClick += new MouseEventHandler(Button_Click35);
            guadagnopic[35].MouseClick += new MouseEventHandler(Button_Click35);
            spesapic[35].MouseClick += new MouseEventHandler(Button_Click35);
            button[36].MouseClick += new MouseEventHandler(Button_Click36);
            guadagnopic[36].MouseClick += new MouseEventHandler(Button_Click36);
            spesapic[36].MouseClick += new MouseEventHandler(Button_Click36);
            */
            #endregion

            //Definizione Pannello Settimana e componenti
            Settimana = new Panel();
            Settimana.MouseEnter += Enter;
            Settimana.MouseClick += FinestraPrincipale.BackPanel.ClickNull;
            Controls.Add(Settimana);
            for (int i = 0; i < 7; i++)
            {
                label[i] = new System.Windows.Forms.Label
                {
                    AutoSize = true
                };
                label[i].MouseEnter += Enter;
                if (i == 0) label[i].Text = "Lunedì";
                if (i == 1) label[i].Text = "Martedì";
                if (i == 2) label[i].Text = "Mercoledì";
                if (i == 3) label[i].Text = "Giovedì";
                if (i == 4) label[i].Text = "Venerdì";
                if (i == 5) label[i].Text = "Sabato";
                if (i == 6) label[i].Text = "Domenica";
                Settimana.Controls.Add(label[i]);
                label[i].MouseClick += FinestraPrincipale.BackPanel.ClickNull;
            }

            //Definizione pulsanti Indietro-Avanti
            button_precedente = new System.Windows.Forms.Label
            {
                BackColor = System.Drawing.Color.Transparent,
                BackgroundImage = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("Freccia_sx"))),
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
                BackgroundImage = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("Freccia_dx"))),
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
                FlatStyle = System.Windows.Forms.FlatStyle.Popup
            };
            button_successivo.MouseClick += new MouseEventHandler(Button_successivo_Click);
            button_successivo.MouseEnter += new System.EventHandler(Button_successivo_MouseEnter);
            button_successivo.MouseLeave += new System.EventHandler(Button_successivo_MouseLeave);
            button_successivo.MouseClick += FinestraPrincipale.BackPanel.ClickNull;
            Controls.Add(button_successivo);

            //Definizione label mese e anno
            label_mese.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            Controls.Add(label_mese);
            label_mese.MouseClick += FinestraPrincipale.BackPanel.ClickNull;
            label_anno.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            Controls.Add(label_anno);
            label_anno.MouseClick += FinestraPrincipale.BackPanel.ClickNull;

            RefreshWindow();
            FinestraPrincipale.size = FinestraPrincipale.BackPanel.Size;
        }
        public void RefreshWindow()
        {
            BackPanel.stop = true;
            SuspendLayout();
            Input.RefreshTotale();
            FinestraPrincipale.BackPanel.AltriConti.RefreshForm();

            for (int i = 0; i < 37; i++)
            {
                if (giorni_festivi.Contains(i % 7 + 1)) button[i].festivo = true; else button[i].festivo = false;
                button[i].SuspendLayout();
                button[i].guadagno_pic.Hide(); button[i].spesa_pic.Hide(); button[i].trasferimento_pic.Hide(); button[i].note_pic.Hide(); button[i].guadagno.Hide(); button[i].spesa.Hide();
                button[i].ResumeLayout(false);
            }
            Calcoli_Mese();
            int[] ausiliare = { 0, 0, 0, 1, mese, anno };
            int[] data_ausiliare = new int[6];
            data_ausiliare[3] = 1; data_ausiliare[4] = mese; data_ausiliare[5] = anno;
            posizione_iniziale = Date.GetIntfromGiornoSettimana(Date.GetGiornosettimanatxt(ausiliare));
            label_mese.Text = Date.GetMesetxt(mese);
            label_anno.Text = Convert.ToString(anno);
            for (int i = 1; i < 37; i++)
            {
                double introito_val = 0, spesa_val = 0, trasferimento_val = 0, note_val = 0;
                foreach (var evento in eventi_mese)
                {
                    if (evento.GetData()[3] == i && evento.Get_Attributo() == "Introito") { introito_val += evento.GetValore(); }
                    if (evento.GetData()[3] == i && evento.Get_Attributo() == "Spesa") { spesa_val += evento.GetValore(); }
                    if (evento.GetData()[3] == i && evento.Get_Attributo() == "Trasferimento") { trasferimento_val += evento.GetValore(); }
                    if (evento.GetData()[3] == i && evento.Get_Attributo() == "Note") { note_val += 1; }
                }
                if (i + posizione_iniziale - 1 < 37)
                {
                    button[i + posizione_iniziale - 1].Text = Convert.ToString(i);
                    button[i + posizione_iniziale - 1].SetLabels(Funzioni_utili.FormatoStandard(introito_val) + "\u20AC", Funzioni_utili.FormatoStandard(spesa_val) + "\u20AC");
                }
                if (introito_val != 0) {
                    button[i + posizione_iniziale - 1].guadagno.Visible = true;
                    button[i + posizione_iniziale - 1].guadagno_pic.Visible = true;
                }
                if (spesa_val != 0)
                {
                    button[i + posizione_iniziale - 1].spesa.Visible = true;
                    button[i + posizione_iniziale - 1].spesa_pic.Visible = true;
                }
                if (trasferimento_val != 0)
                {
                    button[i + posizione_iniziale - 1].trasferimento_pic.Visible = true;
                }
                if (note_val != 0)
                {
                    button[i + posizione_iniziale - 1].note_pic.Visible = true;
                }

            }
            RefreshBottoni();
            Refresh_BottomMenu();
            for (int i = 0; i < posizione_iniziale; i++) button[i].Visible = false;
            for (int i = posizione_iniziale; i < posizione_iniziale + numero_giorni; i++) button[i].Visible = true;
            for (int i = posizione_iniziale + numero_giorni; i < 37; i++) button[i].Visible = false;

            ResumeLayout();
            
        }
        public void Refresh_BottomMenu()
        {
            Guadagno_Complessivo.Text = Funzioni_utili.FormatoStandard(guadagno_mese) + "\u20AC";
            Spesa_Complessiva.Text = Funzioni_utili.FormatoStandard(spesa_mese) + "\u20AC";
            FinestraPrincipale.BackPanel.Portafogli.Text = Funzioni_utili.FormatoStandard(Input.totali[0]) + "\u20AC";
            if (Input.totali.Count() > 1) { FinestraPrincipale.BackPanel.Cassaforte.Text = Funzioni_utili.FormatoStandard(Input.totali[1]) + "\u20AC"; }
            if (Input.totali.Count() == 1) { FinestraPrincipale.BackPanel.Portafogli_Pic.Show(); FinestraPrincipale.BackPanel.Portafogli.Show(); FinestraPrincipale.BackPanel.Banca_Pic.Hide(); FinestraPrincipale.BackPanel.Cassaforte_Pic.Hide(); FinestraPrincipale.BackPanel.Cassaforte.Hide(); }
            if (Input.totali.Count() == 2) { FinestraPrincipale.BackPanel.Portafogli_Pic.Show(); FinestraPrincipale.BackPanel.Portafogli.Show(); FinestraPrincipale.BackPanel.Banca_Pic.Hide(); FinestraPrincipale.BackPanel.Cassaforte_Pic.Visible = true; FinestraPrincipale.BackPanel.Cassaforte.Visible = true; }
            if (Input.totali.Count() > 2) { FinestraPrincipale.BackPanel.Portafogli_Pic.Show(); FinestraPrincipale.BackPanel.Portafogli.Show(); FinestraPrincipale.BackPanel.Cassaforte_Pic.Visible = true; FinestraPrincipale.BackPanel.Cassaforte.Visible = true; FinestraPrincipale.BackPanel.Banca_Pic.Visible = true; }
            FinestraPrincipale.BackPanel.Calc_Pic.Visible = true;
            FinestraPrincipale.BackPanel.Portafogli_Pic.BackgroundImage = Funzioni_utili.TakePicture(Input.metodi[0], 2);
            FinestraPrincipale.BackPanel.tooltip.SetToolTip(FinestraPrincipale.BackPanel.Portafogli_Pic, Input.metodi[0]);
            FinestraPrincipale.BackPanel.tooltip.SetToolTip(FinestraPrincipale.BackPanel.Portafogli, Input.metodi[0]);
            if (Input.totali.Count > 1)
            {
                FinestraPrincipale.BackPanel.Cassaforte_Pic.BackgroundImage = Funzioni_utili.TakePicture(Input.metodi[1], 2);
                FinestraPrincipale.BackPanel.tooltip.SetToolTip(FinestraPrincipale.BackPanel.Cassaforte_Pic, Input.metodi[1]);
                FinestraPrincipale.BackPanel.tooltip.SetToolTip(FinestraPrincipale.BackPanel.Cassaforte, Input.metodi[1]);
            }
            if (FinestraPrincipale.BackPanel.Panel_Ricerca != null || FinestraPrincipale.BackPanel.Panel_Impostazioni != null)
            {
                if (PanelRicerca.active && FinestraPrincipale.BackPanel.Panel_Ricerca.Visible)
                {
                    FinestraPrincipale.BackPanel.Portafogli_Pic.Hide();
                    FinestraPrincipale.BackPanel.Portafogli.Hide();
                    FinestraPrincipale.BackPanel.Banca_Pic.Hide();
                    FinestraPrincipale.BackPanel.Calc_Pic.Hide();
                    FinestraPrincipale.BackPanel.Cassaforte_Pic.Hide();
                    FinestraPrincipale.BackPanel.Cassaforte.Hide();
                }
            }
        }
        public void RefreshBottoniColor()
        {
            for (int k = posizione_iniziale; k < numero_giorni + posizione_iniziale; k++)
                if (k - posizione_iniziale + 1 == Input.data_utile[3] && mese == Input.data_utile[4] && anno == Input.data_utile[5]) button[k].attuale = true; else button[k].attuale = false;
            foreach (Bottoni buttone in button) buttone.RefreshColor();
        }
        public void RefreshBottoni()
        {
            for (int k = posizione_iniziale; k < numero_giorni + posizione_iniziale; k++)
                if (k - posizione_iniziale + 1 == Input.data_utile[3] && mese == Input.data_utile[4] && anno == Input.data_utile[5]) button[k].attuale = true; else button[k].attuale = false;
            foreach (Bottoni buttone in button) buttone.Refresh_Bottoni();
        }
        private void ResizeForm()
        {
            Size = new System.Drawing.Size(FinestraPrincipale.BackPanel.Width, FinestraPrincipale.BackPanel.Height - FinestraPrincipale.BackPanel.Menù.Height - 5);
            Location = new Point(0, FinestraPrincipale.BackPanel.Menù.Height);
            if ((double)FinestraPrincipale.Finestra.Size.Width / (double)FinestraPrincipale.Finestra.Size.Height > (double)proporzione_massima) { FinestraPrincipale.Finestra.Width = (int)(FinestraPrincipale.Finestra.Size.Height * proporzione_massima); }
            FinestraPrincipale.BackPanel.Size = FinestraPrincipale.Finestra.Size;
            Size = new System.Drawing.Size(FinestraPrincipale.BackPanel.Width, FinestraPrincipale.BackPanel.Height - FinestraPrincipale.BackPanel.Menù.Height);
            int larghezza_forma = Width;
            int altezza_forma = Height;
            if (FinestraPrincipale.BackPanel.Panel_Giorno != null) FinestraPrincipale.BackPanel.Panel_Giorno.ResizeGiorno();
            larghezza_forma = this.Width;
            altezza_forma = this.Height;
            Principale.SetBounds((int)(Convert.ToDouble(larghezza_forma) * 0.01), (int)(Convert.ToDouble(altezza_forma) * 0.2), (int)(Convert.ToDouble(larghezza_forma) * 0.97), (int)(Convert.ToDouble(altezza_forma) * 0.75));
            Settimana.SetBounds((int)(Convert.ToDouble(larghezza_forma) * 0.05), (int)(Convert.ToDouble(altezza_forma) * 0.161), (int)(Convert.ToDouble(larghezza_forma) * 0.98), (int)(Convert.ToDouble(altezza_forma) * 0.039));

            orecchietta.Size = new Size((int)(Convert.ToDouble(altezza_forma) * 0.2), (int)(Convert.ToDouble(altezza_forma) * 0.05));
            orecchietta.Location = new Point(-(int)(orecchietta.Width * 0.9), 0);

            foreach (Bottoni bottone in button) bottone.Refresh_Bottoni();
            int j = -1, spazietto = 1;
            button[0].Location = new System.Drawing.Point(Principale.Width / 7 - button[0].Width, 0);
            for (int i = 1; i < 37; i++)
            {
                if (i % 7 == 0) { j++; button[i].Location = new Point(button[i-7].Location.X, button[i-7].Location.Y + button[i-7].Height + spazietto); }
                else button[i].Location = new System.Drawing.Point(button[i - 1].Location.X + button[i - 1].Width + spazietto, button[i - 1].Location.Y);
            }
            Update();
            for (int i = 0; i < 7; i++)
            {
                label[i].Location = new System.Drawing.Point(Principale.Width / 7 * i, 0);
                label[i].Font = new System.Drawing.Font(BackPanel.font1, (int)((double)altezza_forma / 4000 * 30 + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
            taglia_piccola = new Size((int)(Convert.ToDouble(larghezza_forma) * 0.02), (int)(1000 * 0.03));
            taglia_grande = new Size((int)(Convert.ToDouble(larghezza_forma) * 0.03), (int)(1000 * 0.045));
            button_precedente.SetBounds((int)(Convert.ToDouble(larghezza_forma) * (0.2 - 0.15)), (int)(Convert.ToDouble(altezza_forma) * 0.06 + 6), taglia_piccola.Width, taglia_piccola.Height);
            button_successivo.SetBounds((int)(Convert.ToDouble(larghezza_forma) * (0.7 - 0.15)), (int)(Convert.ToDouble(altezza_forma) * 0.06 + 6), taglia_piccola.Width, taglia_piccola.Height);

            label_mese.SetBounds((int)(Convert.ToDouble(larghezza_forma) * 0.07), (int)(Convert.ToDouble(altezza_forma) * 0.036), (int)(Convert.ToDouble(larghezza_forma) * 0.3), (int)(Convert.ToDouble(altezza_forma) * 0.12));
            label_anno.SetBounds((int)(Convert.ToDouble(larghezza_forma) * 0.40 - 50000 / larghezza_forma), (int)(Convert.ToDouble(altezza_forma) * 0.036), (int)(Convert.ToDouble(larghezza_forma) * 0.2), (int)(Convert.ToDouble(altezza_forma) * 0.12));
            label_mese.Font = new System.Drawing.Font(BackPanel.font1, (int)((double)larghezza_forma * 0.018 + 25), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label_anno.Font = new System.Drawing.Font(BackPanel.font1, (int)((double)larghezza_forma * 0.018 + 25), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            Guadagno_Complessivo.SetBounds((int)(Convert.ToDouble(larghezza_forma) * (0.68)), (int)(Convert.ToDouble(altezza_forma) * 0.09 - 5), (int)(Convert.ToDouble(larghezza_forma) * 0.14), (int)(Convert.ToDouble(altezza_forma) * 0.06));
            Spesa_Complessiva.SetBounds((int)(Convert.ToDouble(larghezza_forma) * (0.84)), (int)(Convert.ToDouble(altezza_forma) * 0.09 - 5), (int)(Convert.ToDouble(larghezza_forma) * 0.18), (int)(Convert.ToDouble(altezza_forma) * 0.06));
            Guadagno_Complessivo.Font = new System.Drawing.Font(BackPanel.font1, (int)((double)larghezza_forma / 4000 * 10 + 20), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Spesa_Complessiva.Font = new System.Drawing.Font(BackPanel.font1, (int)((double)larghezza_forma / 4000 * 10 + 20), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            Guadagno_Txt.SetBounds((int)(Convert.ToDouble(larghezza_forma) * (0.63)), (int)(Convert.ToDouble(altezza_forma) * 0.052 - 10), (int)(Convert.ToDouble(larghezza_forma) * 0.1), (int)(Convert.ToDouble(altezza_forma) * 0.1));
            Guadagno_Txt.Font = new System.Drawing.Font(BackPanel.font1, (int)((double)larghezza_forma * 0.005 + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Spesa_Txt.SetBounds((int)(Convert.ToDouble(larghezza_forma) * (0.82)), (int)(Convert.ToDouble(altezza_forma) * 0.052 - 10), (int)(Convert.ToDouble(larghezza_forma) * 0.1), (int)(Convert.ToDouble(altezza_forma) * 0.1));
            Spesa_Txt.Font = new System.Drawing.Font(BackPanel.font1, (int)((double)larghezza_forma * 0.005 + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            Guadagno_Pic.SetBounds((int)(Convert.ToDouble(larghezza_forma) * (0.64)), (int)(Convert.ToDouble(altezza_forma) * 0.07 + 5), (int)(Convert.ToDouble(larghezza_forma) * 0.03), (int)(Convert.ToDouble(altezza_forma) * 0.06));
            Spesa_Pic.SetBounds((int)(Convert.ToDouble(larghezza_forma) * (0.80)), (int)(Convert.ToDouble(altezza_forma) * 0.07 + 5), (int)(Convert.ToDouble(larghezza_forma) * 0.03), (int)(Convert.ToDouble(altezza_forma) * 0.06));
            Refresh_BottomMenu();

            Ricorrenza.RefreshForm();
            Ricorrenza.Location = new Point(0, 0);
            Ricorrenza.BringToFront();
            Update();
        }


        public new void Click()
        {
            FinestraPrincipale.BackPanel.StandardCalendar.Visible = false;
            Calcoli_Giorno();
            Input.LoadAttributi();
            FinestraPrincipale.BackPanel.Panel_Giorno = new ProprietàGiorno()
            {
                Size = FinestraPrincipale.Finestra.Size,
                Visible = false,
            };
            FinestraPrincipale.BackPanel.Controls.Add(FinestraPrincipale.BackPanel.Panel_Giorno);
            RefreshWindow(); FinestraPrincipale.BackPanel.Panel_Giorno.timerPannello.Start();
        }
        

        public void Return_Oggi()
        {
            if (Principale.Visible)
            {
                mese = Input.data_attuale[4];
                anno = Input.data_attuale[5];
                RefreshWindow();
            }
        }

        public void ResizeHelper()
        {
            ResizeForm();
        }
        
        public void Calcoli_Mese()
        {
            eventi_mese.Clear();
            eventi_mese = new List<Eventi>();
            numero_giorni = Date.ContaGiorni(mese, anno);
            guadagno_mese = 0;
            spesa_mese = 0;
            foreach (var evento in Input.eventi)
            {
                if (evento.GetData()[4] == mese && evento.GetData()[5] == anno) eventi_mese.Add(evento);
            }

            foreach (var evento in eventi_mese)
            {
                if (evento.Get_Attributo() == "Introito") guadagno_mese += evento.GetValore();
                if (evento.Get_Attributo() == "Spesa") spesa_mese += evento.GetValore();
            }

        }
        public void Calcoli_Giorno()
        {
            eventi_giorno.Clear();
            foreach (var evento in eventi_mese)
            {
                if (evento.GetData()[3] == giorno) eventi_giorno.Add(evento);
            }
        }

        private void Button_precedente_Click(object sender, MouseEventArgs e)
        {
            if (FinestraPrincipale.BackPanel.altriconti) return;
            if (e.Button == MouseButtons.Left)
            {
                SuspendLayout();
                if (mese == 1 && anno == 2000) return;
                mese--;
                if (mese == 0) { mese = 12; anno--; }
                eventi_mese.Clear();
                label_mese.Text = Date.GetMesetxt(mese);
                label_anno.Text = Convert.ToString(anno);
                RefreshWindow();
                button_precedente.Size = taglia_grande;
                ResumeLayout();
                Update();
            }
        }

        private void Button_successivo_Click(object sender, MouseEventArgs e)
        {
            if (FinestraPrincipale.BackPanel.altriconti) return;
            if (e.Button == MouseButtons.Left)
            {
                if (mese == 12 && anno == 2099) return;
                mese++;
                if (mese == 13) { mese = 1; anno++; }
                eventi_mese.Clear();
                label_mese.Text = Date.GetMesetxt(mese);
                label_anno.Text = Convert.ToString(anno);
                RefreshWindow();
                Update();
            }
        }
        private void Button_precedente_MouseLeave(object sender, EventArgs e)
        {
            if (FinestraPrincipale.BackPanel.altriconti) return;
            button_precedente.Size = taglia_piccola;
            button_precedente.Location = new Point(button_precedente.Location.X+ (taglia_grande.Width - taglia_piccola.Width), button_precedente.Location.Y + (taglia_grande.Height - taglia_piccola.Height)/2);
        }

        private void Button_precedente_MouseEnter(object sender, EventArgs e)
        {
            if (FinestraPrincipale.BackPanel.altriconti) return;
            button_precedente.Size = taglia_grande;
            button_precedente.Location = new Point(button_precedente.Location.X-(taglia_grande.Width - taglia_piccola.Width) , button_precedente.Location.Y - (taglia_grande.Height - taglia_piccola.Height)/2);
        }

        private void Button_successivo_MouseLeave(object sender, EventArgs e)
        {
            if (FinestraPrincipale.BackPanel.altriconti) return;
            button_successivo.Size = taglia_piccola;
            button_successivo.Location = new Point(button_successivo.Location.X, button_successivo.Location.Y + 6);
        }


        private void Button_successivo_MouseEnter(object sender, EventArgs e)
        {
            if (FinestraPrincipale.BackPanel.altriconti) return;
            button_successivo.Size = taglia_grande;
            button_successivo.Location = new Point(button_successivo.Location.X, button_successivo.Location.Y - 6);
        }

        private new void Enter(object sender, EventArgs e)
        {
            Bottoni.index_on = -1;
            Bottoni.Selezione();
        }
        public void Refresh_Indispensabile()
        {
            label_mese.Text = Date.GetMesetxt(mese);
            label_anno.Text = Convert.ToString(anno);
        }
        public static int[] GetGiornoMeseAnno()
        {
            int[] array = new int[3];
            array[0] = FinestraPrincipale.BackPanel.StandardCalendar.giorno;
            array[1] = FinestraPrincipale.BackPanel.StandardCalendar.mese;
            array[2] = FinestraPrincipale.BackPanel.StandardCalendar.anno;
            return array;
        }
        private void TimerF(object sender, EventArgs e)
        {
            try { if (!FinestraPrincipale.Finestra.ClientRectangle.Contains(FinestraPrincipale.Finestra.PointToClient(Cursor.Position))) if (!Ricorrenza.Visible) RicorrenzeHide(); } catch (Exception) { Console.WriteLine("EXC"); }
            if (resize && ProprietàGiorno.time_to_save == false)
            {
                resize = false;
                RefreshWindow();
            }
            if (wait) return;
            try {if (FinestraPrincipale.Finestra.ClientRectangle.Contains(FinestraPrincipale.Finestra.PointToClient(Cursor.Position)))
            {
                return;
            }
            }
            catch (Exception) { Console.WriteLine("EXC"); }
            Bottoni.index_on = -1;
            Bottoni.Selezione();

            wait = true;
        }

        private void MouseIsOverControl(object sender, EventArgs e)
        {
            if (wait) return;
            if (Principale.ClientRectangle.Contains(Principale.PointToClient(Cursor.Position)))
            {
                return;
            }
            Bottoni.index_on = -1;
            Bottoni.Selezione();
            wait = true;
        }
        public void HideButtons()
        {
            foreach (Bottoni button in button) button.BorderStyle = BorderStyle.None;
        }
        public void ShowButtons()
        {
            foreach (Bottoni button in button) button.BorderStyle = BorderStyle.FixedSingle;
        }

        private void RicorrenzeHide()
        {
            orecchietta.Location = new Point(-(int)(orecchietta.Width * 0.9), orecchietta.Location.Y);
        }
        private void RicorrenzeShow()
        {
            orecchietta.Location = new Point(0, orecchietta.Location.Y);
        }

    }
}
