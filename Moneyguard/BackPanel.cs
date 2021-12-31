using Firebase.Storage;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1;

namespace Moneyguard
{
    public class BackPanel : Panel
    {
        public static bool update_response=false;
        public static bool update_question_response = false;
        public Label Ora;
        private Timer timer;
        public Label Portafogli;
        public Label Cassaforte;
        public Label Portafogli_Pic;
        public Label Cassaforte_Pic;
        public Label Banca_Pic;
        public Label Calc_Pic;
        public Label Connect;
        public Pannello_StandardCalendar StandardCalendar;
        public ProprietàGiorno Panel_Giorno;
        public Panel_AltriConti AltriConti;
        public PanelRicerca Panel_Ricerca;
        public Impostazioni Panel_Impostazioni;
        public MenuStrip Menù;
        private ToolStripMenuItem Sincronizza;
        private ToolStripMenuItem Calendar;
        private ToolStripMenuItem Opzioni;
        private ToolStripMenuItem Ricerca;
        private ToolStripMenuItem ModificaTipi;
        private ToolStripMenuItem Finestra_Impostazioni;
        private ToolStripMenuItem Finestra_Backup;
        private ToolStripMenuItem Finestra_Credits;
        private ToolStripMenuItem Finestra_Updates_tool;
        private ToolStripMenuItem User;
        private ToolStripMenuItem ChangePass;
        private ToolStripMenuItem Logout;
        public Finestra_Impostazioni FinestraImpostazioni;
        public Finestra_Backup FinestraBackup;
        public Finestra_Pass FinestraPass;
        public Finestra_Credits FinestraCredits;
        public Finestra_Updates FinestraUpdates;
        public string stringhelper="null";
        public static bool ready_to_sync = false;
        public static bool go_to_impostazioni;
        public static bool go_to_ricerca;
        private Timer timer_Direzione;
        private long timing = 0;
        public static bool impostazioni = false;
        public static bool backup = false;
        public static bool credits = false;
        public static bool updates = false;
        static public bool stop = false;
        public bool altriconti = false;
        static public string font1 = "Script MT Bold";
        static public string font2 = "Algerian";
        static public string font3 = "Stka Small";
        static public string font4 = "BookmAn Old Style";
        public ToolTip tooltip;
        public frmCalculator calcolatrice;

        public readonly Bitmap vero = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("True")));
        public readonly Bitmap falso = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("False")));

        public void Disposer()
        {
            if (Impostazioni.active) { Panel_Impostazioni.CloseImpostazioni(); ShowCalendar(); }
            if(Panel_Ricerca!= null) Panel_Ricerca.Disposer();
            if (Connect.BackgroundImage != null) Connect.BackgroundImage.Dispose();
            Connect.Dispose();
            AltriConti.Disposer();
            StandardCalendar.Disposer();
            timer_Direzione.Dispose();
            timer.Dispose();
            vero.Dispose();
            falso.Dispose();
            tooltip.Dispose();
            Dispose();
            Program.FinestraPrincipale.Disposer();
        }
        public void InitializeBackPanel()
        {
            DoubleBuffered = true;
            impostazioni = false;
            backup = false;
            credits = false;
            updates = false;
            go_to_impostazioni = false;
            go_to_ricerca = false;
            KeyDown += new System.Windows.Forms.KeyEventHandler(FinestraPrincipale.BackPanel.Shortcuts);
            KeyUp += new System.Windows.Forms.KeyEventHandler(FinestraPrincipale.BackPanel.ShortcutsUp);

            Ora = new Label();
            Ora.MouseClick += new MouseEventHandler(Return_Oggi);
            Ora.BackColor = System.Drawing.Color.Transparent;
            Ora.AutoSize = true;
            Controls.Add(Ora);
            tooltip = new ToolTip();
            tooltip.SetToolTip(Ora, "Ritorna ad oggi");

            AltriConti = new Panel_AltriConti();
            Controls.Add(AltriConti);

            timer = new Timer
            {
                Enabled = true,
                Interval = 1000
            };
            timer.Tick += new System.EventHandler(Timer);
            timer_Direzione = new Timer
            {
                Enabled = false,
                Interval = 10
            };
            timer_Direzione.Tick += new System.EventHandler(Direzione);

            //Definizione Menù
            Menù = new MenuStrip()
            {
                GripStyle = ToolStripGripStyle.Visible,
                BackColor = Color.LightGray,
                ImageScalingSize = new System.Drawing.Size(24, 24),
                Location = new System.Drawing.Point(0, 0),
                Size = new System.Drawing.Size(2435, 33),
            };
            Controls.Add(Menù);
            Connect = new Label()
            {
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                BackgroundImageLayout = ImageLayout.Stretch,
                BackColor = Color.LightGray,
            };
            Controls.Add(Connect);
            if (Program.allow_to_connect) { Connect.BackgroundImage = Moneyguard.Properties.Resources.Verde; tooltip.SetToolTip(Connect, "Connesso in cloud"); }
            else { Connect.BackgroundImage = Moneyguard.Properties.Resources.Rosso; tooltip.SetToolTip(Connect, "Non connesso in cloud"); }

            Sincronizza = new ToolStripMenuItem() { Size = new System.Drawing.Size(90, 33), Text = "Sincronizza", Image = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("Sync"))), };
            Calendar = new ToolStripMenuItem() { Size = new System.Drawing.Size(90, 33), Text = "Calendario", Image = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("Calendario2"))), };
            Ricerca = new ToolStripMenuItem() { Size = new System.Drawing.Size(90, 33), Text = "Ricerca", Image = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("Research"))), };
            Opzioni = new ToolStripMenuItem() { Size = new System.Drawing.Size(90, 33), Text = "Opzioni" };
            ModificaTipi = new ToolStripMenuItem() { Size = new System.Drawing.Size(90, 33), Text = "Modifica Tipologie e metodi", Image = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("tipologia"))), };
            Finestra_Impostazioni = new ToolStripMenuItem() { Size = new System.Drawing.Size(90, 33), Text = "Impostazioni", Image = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("impostazioni"))), };
            Finestra_Backup = new ToolStripMenuItem() { Size = new System.Drawing.Size(90, 33), Text = "Backups", Image = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("backup"))), };
            Finestra_Credits = new ToolStripMenuItem() { Size = new System.Drawing.Size(90, 33), Text = "Crediti", Image = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("Info"))), };
            Finestra_Updates_tool = new ToolStripMenuItem() { Size = new System.Drawing.Size(90, 33), Text = "Versione", Image = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("Upgrade"))), };
            User = new ToolStripMenuItem() { Size = new System.Drawing.Size(90, 33), Text = "Account offline", Image = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("User"))), };
            if (Program.Id_user != "local") User.Text = Program.email_user;
            ChangePass = new ToolStripMenuItem() { Size = new System.Drawing.Size(90, 33), Text = "Reimposta password", Image = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("Key"))), };
            Logout = new ToolStripMenuItem() { Size = new System.Drawing.Size(90, 33), Text = "Logout", Image = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("Door"))), };
            Menù.Items.AddRange(new ToolStripItem[] { Calendar, Ricerca, Opzioni, Logout, User });
            ToolStripItem[] toolstrip = new ToolStripItem[] { Sincronizza, ChangePass, Logout };
            if (Program.Id_user == "local") toolstrip = new ToolStripItem[] { Logout };
            Opzioni.DropDownItems.AddRange(new ToolStripItem[] { ModificaTipi, Finestra_Backup, Finestra_Credits, Finestra_Updates_tool, Finestra_Impostazioni });
            User.DropDownItems.AddRange(toolstrip);
            Sincronizza.Click += GoSincronizza;
            Calendar.Click += GoCalendar;
            ModificaTipi.Click += GoImpostazioni;
            Ricerca.Click += GoRicerca;
            Finestra_Impostazioni.Click += NewFinestraImpostazioni;
            Finestra_Backup.Click += NewFinestraBackup;
            Finestra_Credits.Click += NewFinestraCredits;
            Finestra_Updates_tool.Click += NewFinestraUpdates;
            Logout.Click += LogoutClick;
            ChangePass.Click += PassChangeClick;


            //Definizione elementi BackPanel
            Portafogli_Pic = new Label
            {
                BackColor = System.Drawing.Color.Transparent,
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
            };
            Controls.Add(Portafogli_Pic);

            Cassaforte_Pic = new Label
            {
                BackColor = System.Drawing.Color.Transparent,
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
            };
            Controls.Add(Cassaforte_Pic);

            Banca_Pic = new Label
            {
                BackColor = System.Drawing.Color.Transparent,
                BackgroundImage = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("Banca"))),
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
            };
            Controls.Add(Banca_Pic);
            tooltip.SetToolTip(Banca_Pic, "Altri conti");
            Calc_Pic = new Label
            {
                BackColor = System.Drawing.Color.Transparent,
                BackgroundImage = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("calc"))),
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
            };
            Controls.Add(Calc_Pic);
            tooltip.SetToolTip(Calc_Pic, "Calcolatrice");

            Portafogli = new Label
            {
                AutoSize = true
            };
            Controls.Add(Portafogli);

            Cassaforte = new Label
            {
                AutoSize = true
            };
            Controls.Add(Cassaforte);

            MouseClick += ClickNull;

            Banca_Pic.MouseEnter += Enter_Banca;
            Banca_Pic.MouseLeave += Leave_Banca;
            Calc_Pic.MouseEnter += Enter_Calc;
            Calc_Pic.MouseLeave += Leave_Calc;

            StandardCalendar = new Pannello_StandardCalendar();
            FinestraPrincipale.BackPanel.StandardCalendar.InitializePannelloCalendar();
            Controls.Add(StandardCalendar);

            Banca_Pic.MouseClick += OpenAltriConti;
            Calc_Pic.MouseClick += OpenCalc;
            Portafogli.MouseClick += ClickNull;
            Portafogli_Pic.MouseClick += ClickNull;
            Cassaforte.MouseClick += ClickNull;
            Cassaforte_Pic.MouseClick += ClickNull;
            Ora.MouseClick += ClickNull;
            Menù.MouseClick += ClickNull;
            MouseClick += ClickNull;

            SetBounds(0, 0, FinestraPrincipale.Finestra.Size.Width, FinestraPrincipale.Finestra.Size.Height);
            AltriConti.RefreshForm();

            //Properties.Settings.Default.show_update_mess = false;
            //Properties.Settings.Default.Save();
            Timer timer1 = new Timer() { Enabled = true, Interval = 1000, }; timer1.Tick += (o, e) => { System.Threading.Thread thread = new System.Threading.Thread(CheckNewVersion); thread.Start(); timer1.Stop(); };
            Timer response = new Timer() { Enabled = true, Interval = 100, }; response.Tick += (o, e) => { if (update_question_response) { if (update_response) { update_question_response = false; update_response = false; Latest_Version.latest_version = new Latest_Version(true);Latest_Version.latest_version.Show(); response.Stop(); } } };
            //Timer initializeRicerca = new Timer() { Enabled = true, Interval = 500 };
            //initializeRicerca.Tick += (o, e) => { initializeRicerca.Dispose(); Panel_Ricerca = new PanelRicerca(); Visible = true; };
        }

        private async void CheckNewVersion()
        {
            Console.WriteLine("Checking new versions");
            if (Program.iterations == 0 && !Properties.Settings.Default.show_update_mess)
            {
                try
                {
                    Task<bool> task = Finestra_Updates.CheckNewVersion(true); await task;
                    if (task.Result) { update_response = true; }
                }
                catch (Exception) { Console.WriteLine("Can't check new versions"); }
                
                update_question_response = true;
            }
        }
        

        public void Return_Oggi(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) StandardCalendar.Return_Oggi();
        }
        
        public void ResizeBackPanel()
        {
            FinestraPrincipale.Finestra.Caricamento.Size = FinestraPrincipale.Finestra.Size;
            SetBounds(0, 0, FinestraPrincipale.Finestra.Size.Width, FinestraPrincipale.Finestra.Size.Height);
            StandardCalendar.ResizeHelper();
            AltriConti.RefreshForm();

            if (Panel_Impostazioni != null) Panel_Impostazioni.ResizeImp();
            if (PanelRicerca.active) Panel_Ricerca.ResizeForm();

            int larghezza_forma = Width;
            int altezza_forma = Height;

            Connect.Size = new Size(20, 20);
            Connect.Location = new Point(Width - Connect.Width - 20, (int)(32-Connect.Height)/2);
            Connect.Font = new System.Drawing.Font(BackPanel.font3, 10, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Connect.BringToFront();

            Ora.Location = new System.Drawing.Point((int)(larghezza_forma * 0.9 - 80), (int)(altezza_forma * 0.91 - 40));
            Ora.Font = new System.Drawing.Font(BackPanel.font1, (int)((double)larghezza_forma / 2000 * 10 + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            Portafogli_Pic.SetBounds((int)(Convert.ToDouble(larghezza_forma) * (0.37)), (int)(Convert.ToDouble(altezza_forma) * 0.87 - 13), (int)(Convert.ToDouble(larghezza_forma) * 0.03), (int)(Convert.ToDouble(altezza_forma) * 0.06));
            Cassaforte_Pic.SetBounds((int)(Convert.ToDouble(larghezza_forma) * (0.53)), (int)(Convert.ToDouble(altezza_forma) * 0.87 - 13), (int)(Convert.ToDouble(larghezza_forma) * 0.03), (int)(Convert.ToDouble(altezza_forma) * 0.06));
            Portafogli.SetBounds((int)(Convert.ToDouble(larghezza_forma) * (0.41)), (int)(Convert.ToDouble(altezza_forma) * 0.89 - 22), (int)(Convert.ToDouble(larghezza_forma) * 0.03), (int)(Convert.ToDouble(altezza_forma) * 0.06));
            Cassaforte.SetBounds((int)(Convert.ToDouble(larghezza_forma) * (0.57)), (int)(Convert.ToDouble(altezza_forma) * 0.89 - 22), (int)(Convert.ToDouble(larghezza_forma) * 0.03), (int)(Convert.ToDouble(altezza_forma) * 0.06));
            Banca_Piccolo();
            Calc_Piccolo();
            Portafogli.Font = new System.Drawing.Font(BackPanel.font1, (int)((double)larghezza_forma / 4000 * 25 + 15), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Cassaforte.Font = new System.Drawing.Font(BackPanel.font1, (int)((double)larghezza_forma / 4000 * 25 + 15), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            if (Panel_Impostazioni != null || PanelRicerca.active) { Portafogli.Hide(); Portafogli_Pic.Hide(); Cassaforte.Hide(); Cassaforte_Pic.Hide(); Banca_Pic.Hide(); Calc_Pic.Hide(); }

            int complex_size = 0;
            foreach(ToolStripItem item in Menù.Items)
            {
                complex_size += item.Size.Width;
            }
            Menù.Items[Menù.Items.Count - 1].Margin = new System.Windows.Forms.Padding(- complex_size + Connect.Location.X - Connect.Width , 0, 0, 0);
            ResumeLayout();
            //try { Program.caricamento.Visible = true; Program.caricamento.Update(); } catch (Exception) { }
            Program.caricamento_show = false;
            Update();

        }
        public void ClickNull(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                ClickNull();
            }
        }

        public void ClickNull()
        {
            AltriConti.Hide();
            StandardCalendar.Ricorrenza.HideRic();
            altriconti = false;
            if (FinestraPrincipale.BackPanel.Panel_Giorno != null) FinestraPrincipale.BackPanel.Panel_Giorno.PanelMotore.HideMotore();
            if (FinestraPrincipale.BackPanel.Panel_Giorno != null) if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato != null) if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello != null) if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.Visible) FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.Focus();
        }
        private void OpenAltriConti(object sender, MouseEventArgs e)
        {
            if (Panel_NewEvento.active) return;
            if (FinestraPrincipale.BackPanel.Panel_Giorno != null) if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Visible) return;
            if (e.Button == MouseButtons.Left)
            {
                if (altriconti) { AltriConti.Hide(); altriconti = false; return; }
                AltriConti.Show();
                ProprietàGiorno.ScrollToTop(AltriConti.Pannello);
                altriconti = true;
                AltriConti.BringToFront();
            }
        }
        public void OpenCalc(object sender, EventArgs e)
        {
            if (frmCalculator.active) calcolatrice.Focus();
            else
            {
                calcolatrice = new frmCalculator();
                calcolatrice.Show();
            }
            if (go_to_ricerca) calcolatrice.Location = new Point(Width - calcolatrice.Width - 10, Height - calcolatrice.Height - 20);
            else calcolatrice.Location = new Point((int)(Width * 0.83) - calcolatrice.Width - 10, (int)(Height*0.88) - calcolatrice.Height - 20);
        }

        private void Direzione(object sender, EventArgs e)
        {
            timing += 10;
        }

        private void Timer(object sender, EventArgs e)
        {
            Timer();
        }
        private void GoImpostazioni(object sender, EventArgs e)
        {
            if (go_to_impostazioni) return;
            go_to_impostazioni = true;
            go_to_ricerca = false;
            ClosePanel_Giorno();
        }
        private void GoRicerca(object sender, EventArgs e)
        {
            if (go_to_ricerca) return;
            go_to_ricerca = true;
            go_to_impostazioni = false;
            ClosePanel_Giorno();
        }
        private void PassChangeClick(object sender, EventArgs e)
        {
            if (FinestraPass != null) if (FinestraPass.Visible) { FinestraPass.Focus(); return; }
            FinestraPass = new Finestra_Pass();
            FinestraPass.Show();
            FinestraPass.Location = new Point(FinestraPrincipale.Finestra.Location.X + (Width - FinestraPass.Width) / 2, FinestraPrincipale.Finestra.Location.Y + (Height - FinestraPass.Height) / 2);
        }
        public void LogoutClick(object sender, EventArgs e)
        {
            Program.ready_toquit = true;
            WidgetMoneyguard.ready_toclose = true;
            Program.login = true;
            Program.aut_login = false;
            Program.FinestraPrincipale.Close();
        }
        private void NewFinestraImpostazioni(object sender, EventArgs e)
        {
            if (FinestraImpostazioni != null) if (FinestraImpostazioni.Visible) { FinestraImpostazioni.Focus(); return; }
            Input.RefreshImages();
            FinestraImpostazioni = new Finestra_Impostazioni();
            FinestraImpostazioni.Show();
            FinestraImpostazioni.Location = new Point(FinestraPrincipale.Finestra.Location.X + (Width - FinestraImpostazioni.Width) / 2, FinestraPrincipale.Finestra.Location.Y + (Height - FinestraImpostazioni.Height) / 2);
            impostazioni = true;
        }
        private void NewFinestraBackup(object sender, EventArgs e)
        {
            if (FinestraBackup != null) if (FinestraBackup.Visible) { FinestraBackup.Focus(); return; }
            FinestraBackup = new Finestra_Backup();
            FinestraBackup.Show();
            FinestraBackup.Location = new Point(FinestraPrincipale.Finestra.Location.X + (Width - FinestraBackup.Width) / 2, FinestraPrincipale.Finestra.Location.Y + (Height - FinestraBackup.Height) / 2);
            backup = true;
        }
        private void NewFinestraCredits(object sender, EventArgs e)
        {
            if (FinestraCredits != null) if (FinestraCredits.Visible) { FinestraCredits.Focus(); return; }
            FinestraCredits = new Finestra_Credits();
            FinestraCredits.label1.Text = Program.copyright;
            FinestraCredits.Show();
            FinestraCredits.Location = new Point(FinestraPrincipale.Finestra.Location.X + (Width - FinestraCredits.Width) / 2, FinestraPrincipale.Finestra.Location.Y + (Height - FinestraCredits.Height) / 2);
            credits = true;
        }
        private void NewFinestraUpdates(object sender, EventArgs e)
        {
            if (FinestraUpdates != null) if (FinestraUpdates.Visible) { FinestraUpdates.Focus(); return; }
            FinestraUpdates = new Finestra_Updates();
            FinestraUpdates.Show();
            FinestraUpdates.Location = new Point(FinestraPrincipale.Finestra.Location.X + (Width - FinestraUpdates.Width) / 2, FinestraPrincipale.Finestra.Location.Y + (Height - FinestraUpdates.Height) / 2);
            updates = true;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        int conto_pass = 0;
        int conto_sync = 0;
        int conto_check = 0;
        bool prev_connesso = false;
        private void Timer()
        {
            conto_check++;

            if (conto_check >= 2) { System.Threading.Thread thread = new System.Threading.Thread(FirebaseClass.Check_Connection); thread.Start(); conto_check = 0; }
            if (FirebaseClass.connesso && !prev_connesso) {
                    Connect.BackgroundImage = Moneyguard.Properties.Resources.Verde; tooltip.SetToolTip(Connect, "Connesso in cloud");
                    prev_connesso = FirebaseClass.connesso;
            }
            else if (!FirebaseClass.connesso && prev_connesso)
            {
                    Connect.BackgroundImage = Moneyguard.Properties.Resources.Rosso; tooltip.SetToolTip(Connect, "Non connesso in cloud");
                    prev_connesso = FirebaseClass.connesso;
            }


            if (Program.Id_user == "local") {
                Connect.BackgroundImage = Moneyguard.Properties.Resources.Rosso;
                tooltip.SetToolTip(Connect, "Modalità offline");
            }

            IntPtr activeHandle = GetForegroundWindow();
            try { if (activeHandle == FinestraPrincipale.Finestra.Handle) conto_pass = 0; else conto_pass++; } catch (Exception) { Console.WriteLine("EXC"); }
            if (conto_pass == 60 * Impostazioni.timeout_pass && Impostazioni.timeout_pass != 0) FinestraPrincipale.Pass_Panel.Visible = true;
            if (conto_sync == 60 * Program.min_to_aut_sync)
            {
                conto_sync = 0;
                if (Impostazioni.aut_sync)
                {
                    if (! FinestraPrincipale.Pass_Panel.Visible)
                    {
                        FinestraPrincipale.BackPanel.GoCalendar(null, null);
                        Program.caricamento_show = false;
                    }
                    Program.sync_by_user = false;
                    System.Threading.Thread Update1 = new System.Threading.Thread(Program.SyncData); Update1.Start();
                }
            }


            Input.data_attuale = Date.GetActualDate();
            Input.RefreshDataUtile();


            string text1 = "", text2 = "", text3 = "";
            if (Input.data_attuale[0] < 10) text3 = "0";
            if (Input.data_attuale[1] < 10) text2 = "0";
            if (Input.data_attuale[2] < 10) text1 = "0";
            Ora.Text = Convert.ToString("     " + text1 + Convert.ToString(Input.data_attuale[2]) + ":" + text2 + Convert.ToString(Input.data_attuale[1]) +
                ":" + text3 + Convert.ToString(Input.data_attuale[0] + "\n" + Input.data_attuale[3]) + " " + Date.GetMesetxt(Input.data_attuale[4]) + " " +
                Convert.ToString(Input.data_attuale[5]));
            if (stop) { stop = false; return; }
            FinestraPrincipale.BackPanel.StandardCalendar.RefreshBottoniColor();

            if(ready_to_sync)
            {
                ready_to_sync = false;
                if (FinestraPrincipale.active && !FinestraPrincipale.Pass_Panel.Visible) FinestraPrincipale.BackPanel.GoCalendar(null, null);
                Program.sync_by_user = true;
                System.Threading.Thread Update1 = new System.Threading.Thread(Program.SyncData); Update1.Start();
                Program.quit1.WaitOne();
                //if (!Program.saved_correctly) { Program.caricamento_show = false; MessageBox.Show("Per sincronizzare il database è necessario avere una connessione ad internet."); Program.caricamento_show = true; }
                try { new Input(); } catch (Exception ex) { Program.its_the_end = true; MessageBox.Show(@"Il file C:\ProgramData\Cyan\Moneyguard\DataV1.txt risulta corrotto. Ripristinalo oppure cancellalo\nEccezione: " + ex.Message); return; }
                WidgetMoneyguard.refreshpanel = true;
                if (FinestraPrincipale.active && !FinestraPrincipale.Pass_Panel.Visible) FinestraPrincipale.BackPanel.ShowCalendar();
                Program.caricamento_show = false;
            }
        }

        public void ShortcutsUp(object sender, KeyEventArgs e)
        {
            if (FinestraPrincipale.BackPanel.StandardCalendar.Visible)
            {
                if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Left) { if (FinestraPrincipale.BackPanel.AltriConti.Visible) return; StandardCalendar.RefreshWindow(); }
            }
        }
        public void Shortcuts(object sender, KeyEventArgs e)
        {
            if (FinestraPrincipale.Pass_Panel.Visible) return;
            if (e.KeyCode == Keys.Delete && Impostazioni.active) { Panel_Impostazioni.CloseImpostazioni(); return; }
            if (e.KeyCode == Keys.Delete && PanelRicerca.active) { Panel_Ricerca.CloseRicerca(); return; }
            if (Impostazioni.active || PanelRicerca.active) return;
            if (e.KeyCode == Keys.Left && FinestraPrincipale.BackPanel.StandardCalendar.Visible && FinestraPrincipale.BackPanel.altriconti==false)
            {
                if (StandardCalendar.mese == 1 && StandardCalendar.anno == 2000) return;
                    StandardCalendar.mese--;
                if (StandardCalendar.mese == 0) { StandardCalendar.mese = 12; StandardCalendar.anno--; }
                FinestraPrincipale.BackPanel.StandardCalendar.Refresh_Indispensabile();
            }
            if (e.KeyCode == Keys.Right && FinestraPrincipale.BackPanel.StandardCalendar.Visible && FinestraPrincipale.BackPanel.altriconti == false)
            {
                if (StandardCalendar.mese == 12 && StandardCalendar.anno == 2099) return;
                StandardCalendar.mese++;
                if (StandardCalendar.mese == 13) { StandardCalendar.mese = 1; StandardCalendar.anno++; }
                FinestraPrincipale.BackPanel.StandardCalendar.Refresh_Indispensabile();
            }
            if (e.KeyCode == Keys.Right && ProprietàGiorno.exist && FinestraPrincipale.BackPanel.altriconti == false)
            {
                if (!FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Visible)
                {
                    if (Panel_NewEvento.active) { if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Visible) return; }
                    Move_toDay("next");
                }
            }
            if (e.KeyCode == Keys.Left && ProprietàGiorno.exist && FinestraPrincipale.BackPanel.altriconti == false)
            {
                if (!FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Visible)
                {
                    if (Panel_NewEvento.active) { if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Visible) return; }
                    Move_toDay("previous");
                }
            }
            if (e.KeyCode == Keys.Back && Panel_NewEvento.active == true || e.KeyCode == Keys.Delete && Panel_NewEvento.active == true) { FinestraPrincipale.BackPanel.Panel_Giorno.ClickNull(); return; }
            if(FinestraPrincipale.BackPanel.Panel_Giorno != null) if (e.KeyCode == Keys.Back && FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Visible == false || e.KeyCode == Keys.Delete && FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Visible == false) { ClosePanel_Giorno(); return; }
            if (FinestraPrincipale.BackPanel.Panel_Giorno != null) if (e.KeyCode == Keys.Back && FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Visible == true || e.KeyCode == Keys.Delete && FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Visible == true)
                { FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.passaggio--; FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.ResizeForm(true); return; }
            if (e.KeyCode == Keys.Enter && ProprietàGiorno.exist)
            {
                if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.True.Visible == true) { FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.Click_True(); return; }
            }
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter && ProprietàGiorno.exist == false) StandardCalendar.Return_Oggi();
            //if (e.KeyCode == Keys.H) Helper();
        }

        public void ClosePanel_Giorno()
        {
            Console.WriteLine("Closing PanelGiorno!");
            FinestraPrincipale.Finestra.Caricamento.BringToFront();
            StandardCalendar.Visible = false;
            if (FinestraPrincipale.BackPanel.Panel_Giorno != null)
            {
                Savings.SaveGiorno();
                FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale.Clear();
                FinestraPrincipale.BackPanel.Panel_Giorno.Visible = false;
                foreach (Control controllo in Panel_Giorno.Controls)
                {
                    controllo.Dispose();
                    FinestraPrincipale.BackPanel.Controls.Remove(controllo);
                }
                FinestraPrincipale.BackPanel.Controls.Remove(Panel_Giorno);
                Panel_Giorno.Disposer();
                Panel_Giorno = null;
            }
            Savings.SaveEvents();
            if (go_to_impostazioni) {Panel_Impostazioni = new Impostazioni(); FinestraPrincipale.Finestra.Caricamento.SendToBack(); return; }
            if (go_to_ricerca)
            {
                SendToBack();
                if (!PanelRicerca.initialized) Panel_Ricerca = new PanelRicerca();
                else
                {
                    Panel_Ricerca.HideIcons();
                    Panel_Ricerca.ShowRicerca();
                    Panel_Ricerca.ResizeForm();
                    Panel_Ricerca.Visible = true;
                }
                FinestraPrincipale.Finestra.Caricamento.SendToBack();
                return;
            }
            ShowCalendar();
            FinestraPrincipale.Finestra.Caricamento.SendToBack();
        }
        public void ShowCalendar()
        {
            go_to_ricerca = false;
            go_to_impostazioni = false;
            try { StandardCalendar.Visible = true; } catch (Exception) { }
            StandardCalendar.Calcoli_Mese();
            StandardCalendar.RefreshWindow();
            StandardCalendar.giorno = 0;
        }

        private void Banca_Grande()
        {
            Banca_Pic.Location = new Point(Banca_Pic.Location.X - 2, Banca_Pic.Location.Y - 2);
            Banca_Pic.Size = new Size(Banca_Pic.Width + 4, Banca_Pic.Height + 4);
        }
        private void Banca_Piccolo()
        {
            Banca_Pic.Size = new Size((int)(Convert.ToDouble(Width) * 0.05), (int)(Convert.ToDouble(Height) * 0.08));
            Banca_Pic.Location = new Point((int)(Convert.ToDouble(Width) * (0.70)), (int)(Convert.ToDouble(Height) * 0.86 - 16));
        }
        private void Calc_Grande()
        {
            Calc_Pic.Location = new Point(Calc_Pic.Location.X - 2, Calc_Pic.Location.Y - 2);
            Calc_Pic.Size = new Size(Calc_Pic.Width + 4, Calc_Pic.Height + 4);
        }
        public void Calc_Piccolo()
        {
            if (go_to_ricerca)
            {
                Calc_Pic.Size = new Size((int)(Convert.ToDouble(Width) * 0.04), (int)(Convert.ToDouble(Height) * 0.05));
                Calc_Pic.Location = new Point((int)(Convert.ToDouble(Width) * (0.81)), (int)(Convert.ToDouble(Height) * 0.05));
            }
            else if (go_to_impostazioni) { }
            else
            {
                Calc_Pic.Size = new Size((int)(Convert.ToDouble(Width) * 0.04), (int)(Convert.ToDouble(Height) * 0.05));
                Calc_Pic.Location = new Point((int)(Convert.ToDouble(Width) * (0.77)), (int)(Convert.ToDouble(Height) * 0.88 - 16));
            }
        }
        private void Enter_Banca(object sender, EventArgs e)
        {
            if (Panel_NewEvento.active) return;
            if (FinestraPrincipale.BackPanel.Panel_Giorno != null) if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Visible) return;
            Banca_Grande();
        }
        private void Leave_Banca(object sender, EventArgs e)
        {
            if (Panel_NewEvento.active) return;
            if (FinestraPrincipale.BackPanel.Panel_Giorno != null) if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Visible) return;
            Banca_Piccolo();
        }
        private void Enter_Calc(object sender, EventArgs e)
        {
            if (Panel_NewEvento.active) return;
            if (FinestraPrincipale.BackPanel.Panel_Giorno != null) if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Visible) return;
            Calc_Grande();
        }
        private void Leave_Calc(object sender, EventArgs e)
        {
            if (Panel_NewEvento.active) return;
            if (FinestraPrincipale.BackPanel.Panel_Giorno != null) if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Visible) return;
            Calc_Piccolo();
        }

        private void Helper()
        {
            //MessageBox.Show(stringhelper);
            for (int i = 0; i < Input.tipi_sort.Count; i++) Console.WriteLine(Input.tipi_sort[i] + " " + Input.tipi[i]);
        }
        public void GoCalendar(object sender, EventArgs e)
        {
            if (FinestraPrincipale.BackPanel.StandardCalendar.Visible) return;
            if (FinestraPrincipale.BackPanel.Panel_Giorno != null) { ClosePanel_Giorno(); return; }
            if (FinestraPrincipale.BackPanel.Panel_Impostazioni != null) { FinestraPrincipale.BackPanel.Panel_Impostazioni.CloseImpostazioni(); return; }
            if (FinestraPrincipale.BackPanel.Panel_Ricerca != null) {FinestraPrincipale.BackPanel.Panel_Ricerca.CloseRicerca(sender, e); return; }
        }
        private void GoSincronizza(object sender, EventArgs e)
        {
            Program.caricamento_show = true;
            GoCalendar(null, null); 
            Update();
            ready_to_sync = true;
        }

        public void Move_toDay(string mode = "", DateTime data = new DateTime())
        {
            if (data.Year == 1)
            {
                data = new DateTime(StandardCalendar.anno, StandardCalendar.mese, StandardCalendar.giorno, 0, 0, 0);
                if (mode == "next")
                {
                    data = data.AddHours(24);
                }
                else if (mode == "previous")
                {
                    data = data.AddHours(-24);
                }
                Savings.SaveGiorno();
                Savings.SaveEvents();
            }


            StandardCalendar.anno = data.Year;
            StandardCalendar.mese = data.Month;
            StandardCalendar.giorno = data.Day;
            StandardCalendar.Calcoli_Mese();
            StandardCalendar.Calcoli_Giorno();

            try
            {
                Panel_Giorno.panelspeciale.Clear();
                Panel_Giorno.AttributoPanel.Hide();
                Panel_Giorno.Attributo_txt.Hide();
                foreach (Control controllo in Panel_Giorno.Tipi.Controls) controllo.Dispose();
                Panel_Giorno.Tipi.Controls.Clear();
                Panel_Giorno.Populate_Tipi();
                Panel_Giorno.ResizeGiorno(false);
                Panel_Giorno.Tipi.Update();
            }
            catch (Exception) { }
        }

    }
}
