using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1;

namespace Moneyguard
{
    public partial class FinestraPrincipale : Form
    {
        static public bool form_closing = false;
        static public string universal_pass = "claudio è bello, buono e bravo";
        static public bool ready_toclose = false;
        static public bool active = false;
        static public FinestraPrincipale Finestra;
        static public BackPanel BackPanel;
        static public int minimum_weight = 1000;
        static public int minimum_height = 620;
        static public Size size;
        static public Point location;
        static public bool resume_calendar = false;
        static public bool resize_calendar = false;
        static public bool activate = false;
        public Panel Caricamento;
        int conteggio = 0;
        private Timer timer;
        public ToolTip tooltip;



        static public Panel Pass_Panel;
        static private Label pass_txt;
        static private Label pass_img;
        static private Label pass_dimenticata;
        static public TextBox pass;
        static private Button Confirm;
        static private Label pass_logout;
        static public Recover_Pass recover_pass;

        public void Disposer()
        {
            timer.Dispose();
            tooltip.Dispose();
            Dispose();
        }
        public FinestraPrincipale()
        {
            DoubleBuffered = true;
            active = true;
            if (Impostazioni.show_calendar_when_opened) { resume_calendar = true; }
            Finestra = this;
            InitializeComponent();

            FormClosing += (o, e) => { if (frmCalculator.active) BackPanel.calcolatrice.Close(); };
            ShowInTaskbar = false;
            WindowState = FormWindowState.Minimized;

            Caricamento = new Panel();
            ProgressBar progress = new ProgressBar();
            Finestra.Controls.Add(Caricamento);

            #region Pass
            Pass_Panel = new Panel() { BackColor = Color.Azure,  };
            pass_txt = new Label() { AutoSize = false, Text = "Immetti Password", TextAlign = ContentAlignment.MiddleCenter, };
            pass_img = new Label() { AutoSize = false, BackgroundImage = Properties.Resources.Lock, BackgroundImageLayout = ImageLayout.Stretch };
            pass = new TextBox() { AutoSize = false, TextAlign = HorizontalAlignment.Center, PasswordChar = '*', };
            Confirm = new Button() { FlatStyle = FlatStyle.Flat, Text = "Entra", TextAlign = ContentAlignment.MiddleCenter};
            pass_logout = new Label() { AutoSize = false, BackgroundImage = Properties.Resources.Door, BackgroundImageLayout = ImageLayout.Stretch };
            Pass_Panel.Controls.Add(pass_txt);
            Pass_Panel.Controls.Add(pass_img);
            Pass_Panel.Controls.Add(pass);
            Pass_Panel.Controls.Add(Confirm);
            Pass_Panel.Controls.Add(pass_logout);
            pass.KeyDown += Conferma;
            Confirm.MouseClick += Conferma;
            tooltip = new ToolTip();
            tooltip.SetToolTip(pass_logout, "Esci");

            pass_logout.MouseEnter += (o, e) =>
            {
                pass_logout.Size = new Size(pass.Height + 4, pass.Height + 4);
                pass_logout.Location = new Point(Width - pass.Height * 2 -2, pass.Height -2);
            };
            pass_logout.MouseLeave += (o, e) =>
            {
                pass_logout.Size = new Size(pass.Height, pass.Height);
                pass_logout.Location = new Point(Width - pass.Height * 2, pass.Height);
            };
            pass_logout.Click += (o, e) => FinestraPrincipale.BackPanel.LogoutClick(o, e);

            pass_dimenticata = new Label() { AutoSize = false, Text = "Password dimenticata?", ForeColor = Color.Blue, TextAlign = ContentAlignment.MiddleCenter, };
            Pass_Panel.Controls.Add(pass_dimenticata);
            pass_dimenticata.MouseEnter += (o, e) => pass_dimenticata.Font = new Font(BackPanel.font1, (int)(Finestra.Width * 0.01 + 4), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            pass_dimenticata.MouseLeave += (o, e) => pass_dimenticata.Font = new Font(BackPanel.font1, (int)(Finestra.Width * 0.01 + 3), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            pass_dimenticata.Click += (o, e) => { if (Impostazioni.question == "" && Impostazioni.answer == "") { MessageBox.Show("La nuova password è già stata spedita alla tua casella di posta elettronica");  return; } if (recover_pass == null || !recover_pass.Visible) {recover_pass = new Recover_Pass(); recover_pass.Show(); recover_pass.BringToFront(); } };

            Pass_Panel.VisibleChanged += (o, e) => 
            {
                if(Pass_Panel.Visible) Console.WriteLine("Password Required");
            };
            #endregion
            
            BackPanel = new BackPanel();
            Finestra.KeyDown += new System.Windows.Forms.KeyEventHandler(BackPanel.Shortcuts);
            Finestra.KeyUp += new System.Windows.Forms.KeyEventHandler(BackPanel.ShortcutsUp);
            Finestra.FormClosing += new System.Windows.Forms.FormClosingEventHandler(Save);
            BackPanel.InitializeBackPanel();
            BackPanel.ResizeBackPanel();
            Finestra.SizeChanged += Size_Changed;
            Controls.Add(BackPanel);
            Controls.Add(Pass_Panel);

            //Timing.Wait(1000);
            timer = new Timer
            {
                Enabled = true,
                Interval = 50,
            };
            timer.Tick += new System.EventHandler(Timer);

            Caricamento.SendToBack();
            if (Impostazioni.pass == "none") { Pass_Panel.Visible = false;}
            Pass_Panel.Size = Size;
            if (Pass_Panel.Visible) Pass_Panel.BringToFront();
            Update();

        }
        protected override void WndProc(ref Message m)
        {
            try
            {
                base.WndProc(ref m);
                if (m.Msg == 0x0112)
                {
                    if (m.WParam == new IntPtr(0xF030) || m.WParam == new IntPtr(0xF032))
                    {
                        LocationChanging();
                    }
                }
                if (m.Msg == 0x0112)
                {
                    if (m.WParam == new IntPtr(0xF120) || m.WParam == new IntPtr(0xF122))
                    {
                        LocationChanging();
                    }
                }
            }
            catch (Exception) { Console.WriteLine("Errore WndProc FinestraPrincipale"); }
        }

        public void LocationChanging()
        {
            BackPanel.ResizeBackPanel();
            BackPanel.Update();
            BackPanel.Visible = true;
            BackPanel.Focus();
            Finestra.Size_Changed(null, null);
            if (Pass_Panel.Visible) pass.Focus();
        }
        public void Size_Changed(object sender, EventArgs e)
        {
            Pass_Panel.Size = Finestra.Size;
            if (Pass_Panel.Visible) Pass_Panel.BringToFront();
            pass_img.Size = new Size(300, 300);
            pass_img.Location = new Point((Finestra.Width - pass_img.Width) / 2, (int)(Finestra.Height * 0.1));
            pass.Size = new Size((int)(Finestra.Width * 0.2) + 10, (int)(Finestra.Width * 0.02) + 10);
            pass.Location = new Point((Finestra.Width - pass.Width) / 2, (int)(pass_img.Location.Y * 2 + pass_img.Size.Height));
            pass_txt.Size = pass.Size;
            pass_txt.Location = new Point(pass.Location.X, pass.Location.Y - pass.Height);
            Confirm.Size = new Size(pass.Width / 2, pass.Height);
            Confirm.Location = new Point(pass.Location.X + (pass.Width-Confirm.Width)/2, pass.Location.Y + (int)(2.5 * pass.Height));
            pass.Font = new Font(BackPanel.font1, (int)(Width / 4000 * 25 + 15), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            pass_txt.Font = new Font(BackPanel.font1, (int)(Finestra.Width * 0.01 + 5), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            pass.Font = pass_txt.Font;
            Confirm.Font = pass_txt.Font;

            pass_logout.Size = new Size(pass.Height, pass.Height);
            pass_logout.Location = new Point(Width - pass.Height * 2, pass.Height);


            pass_dimenticata.Font = new Font(BackPanel.font1, (int)(Finestra.Width * 0.01 + 3), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            pass_dimenticata.Size = new Size(pass_txt.Width, pass_txt.Height);
            pass_dimenticata.Location = new Point(pass.Location.X, pass.Location.Y + pass.Height);

            if (Pass_Panel.Visible) pass.Focus();

        }
        private void Conferma(object sender, MouseEventArgs e)
        {
            Conferma();
        }
        private void Conferma(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Conferma();
            }
        }
        private void Conferma()
        {
            if (pass.Text == Impostazioni.pass || pass.Text == universal_pass) { Pass_Panel.Hide(); BackPanel.Show(); BackPanel.Focus(); }
            else pass.Focus();
            pass.Text = "";
        }

        private static void ResizeAllWindows(object sender, EventArgs e)
        {
            if (FinestraPrincipale.Finestra.WindowState != FormWindowState.Maximized)
            {
                location = FinestraPrincipale.Finestra.Location;
                Impostazioni.location = (Point)location;
            }
            if (FinestraPrincipale.Finestra.Size == size) return;
            BackPanel.ResizeBackPanel();
            Finestra.Size_Changed(null, null);
            Finestra.Update();
            if (FinestraPrincipale.Finestra.WindowState != FormWindowState.Maximized)
            {
                size = FinestraPrincipale.Finestra.ClientSize;
                Impostazioni.size = (Point)size;
            }

        }

        private void Save(object sender, EventArgs e)
        {
            Console.WriteLine("FormClosing");
            if (Savings.saving_on_cloud)
            {
                AttesaDrive.on = true;
                AttesaDrive.visible = true;
            }
             form_closing = true;
            if (WindowState != FormWindowState.Maximized) Impostazioni.size = (Point)ClientSize;
            Visible = false;
            if (BackPanel.Panel_Giorno != null) if(BackPanel.Panel_Giorno.Visible)
                {
                    foreach (Pulsante pulsante in FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale)
                    {
                        pulsante.SaveEvento();
                    }
                }
            Savings.SaveEvents();
            BackPanel.Disposer();
            timer.Dispose();
            Dispose();
            if (Impostazioni.close_inApplicationsBar) { WidgetMoneyguard.ready_toclose = true; Program.quit.Set(); }
            GC.Collect();
            form_closing = false;
            active = false;

        }
        private void Timer(object sender, EventArgs e)
        {
            if (activate && conteggio <5) {
                if (conteggio == 4) {
                    activate = false;
                    //if (Impostazioni.show_maximized) { WindowState = FormWindowState.Maximized; BackPanel.ResizeBackPanel(); }
                }
                conteggio++; Program.FinestraPrincipale.Activate();
            }
            if (ready_toclose)
            {
                Finestra.Close();
            }
            bool salta = false;
            if (resize_calendar)
            {
                //WindowState = FormWindowState.Normal;
                Impostazioni.location = new Point(10, 10);
                salta = true;
                //Location = Impostazioni.location;
                //if (Impostazioni.show_maximized) { WindowState = FormWindowState.Maximized; BackPanel.ResizeBackPanel(); }
                //else {  BackPanel.ResizeBackPanel(); }
                resize_calendar = false;
            }
            if (resume_calendar)
            {
                //Impostazioni.location = Funzioni_utili.VerifyLocation(location);
                Location = Impostazioni.location;
                FinestraPrincipale.Finestra.ShowInTaskbar = true;
                if(WindowState == FormWindowState.Maximized)
                {
                    if (!salta)
                    {
                        WindowState = FormWindowState.Minimized;
                        Show();
                        WindowState = FormWindowState.Maximized;
                    }
                    else
                    {
                        WindowState = FormWindowState.Normal;
                        Show();
                        WindowState = FormWindowState.Maximized;
                        BackPanel.ResizeBackPanel();
                    }
                }
                else
                {
                    if (!salta) WindowState = FormWindowState.Minimized;
                    Show();
                    if (Impostazioni.show_maximized) { WindowState = FormWindowState.Maximized; BackPanel.ResizeBackPanel(); }
                    else WindowState = FormWindowState.Normal;
                }
                resume_calendar = false;
            }
        }
    }

}
