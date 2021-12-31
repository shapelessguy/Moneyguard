using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using System.Net.Mail;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Storage;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using HWND = System.IntPtr;
using WindowsFormsApplication1;

namespace Moneyguard
{
    public partial class Login : Form
    {
        bool ready_toquit = true;
        Timer timer;
        Point x_location;
        Size x_size;
        public Login(bool automatic)
        {
            InitializeComponent();
            Program.ready_toquit = false;
            ricorda.Checked = Properties.Settings.Default.ricordami;
            automat.Checked = Properties.Settings.Default.entra_aut;
            InitializeReally();
        }

        void InitializeReally()
        {
            if (ricorda.Checked)
            {
                UsernameTbox.Text = Properties.Settings.Default.email;
                passTbox.Text = Properties.Settings.Default.pass;
            }
            ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
            FormClosing += (o, e) => { SaveSettings(); if (ready_toquit) Program.ready_toquit = true; };

            panel1.MouseMove += Panel_MouseMove;
            panel2.MouseMove += Panel_MouseMove;
            MouseMove += Panel_MouseMove;
            picture.MouseMove += Panel_MouseMove;


            progressBar1.Value = 0;
            timer = new Timer()
            {
                Enabled = true,
                Interval = 10,
            };
            timer.Tick += Animation;
            label3.BackgroundImage = Properties.Resources.Red_X;
            picture.BackgroundImage = Properties.Resources.Cassaforte;
            label3.BackgroundImageLayout = ImageLayout.Stretch;
            picture.BackgroundImageLayout = ImageLayout.Stretch;
            //try { Program.caricamento.Visible = true; Program.caricamento.Update(); } catch (Exception) { }
            Program.caricamento_show = false;
            UsernameTbox.KeyDown += (o, e) => { if (e.KeyCode == Keys.Enter) if (UsernameTbox.Text != "" && passTbox.Text != "") {accedi_Click(null, null); e.SuppressKeyPress = true; } };
            passTbox.KeyDown += (o, e) => { if (e.KeyCode == Keys.Enter) if (UsernameTbox.Text != "" && passTbox.Text != "") {accedi_Click(null, null); e.SuppressKeyPress = true; } };
            passdimenticata.MouseEnter += (o, e) => { passdimenticata.Font = new System.Drawing.Font("Modern No. 20", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))); };
            passdimenticata.MouseLeave += (o, e) => { passdimenticata.Font = new System.Drawing.Font("Modern No. 20", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))); };
            label3.MouseEnter += (o, e) => { label3.Location = new Point(x_location.X -1, x_location.Y -1); label3.Size = new Size(x_size.Width+2, x_size.Height+2); };
            label3.MouseLeave += (o, e) => { label3.Location = new Point(x_location.X, x_location.Y); label3.Size = new Size(x_size.Width, x_size.Height); };
            x_location = label3.Location;
            x_size = label3.Size;
            FocusWin();
        }


        //IFirebaseConfig ifc = new FirebaseConfig()
        //{
        //  AuthSecret = "Pe6v73iZHcC1psVlv3Dgixq31PWQz8LbwRFE6kiZ",
        //BasePath = "https://mobilemoneyguard.firebaseio.com/",
        //};
        //IFirebaseClient client;



        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        internal static extern IntPtr SetForegroundWindow(IntPtr hWnd);
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        private void Panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        public void FocusWin()
        {
            SetForegroundWindow(this.Handle);
        }

        private async void TryAutLogin()
        {
            if (automat.Checked)
            {
                string error = await FirebaseClass.CheckFirebaseCred(Properties.Settings.Default.email, Properties.Settings.Default.pass);
                if (error != "") { FirebaseClass.token = null; InitializeReally();  return; }
                if (!FirebaseClass.FireBaseLogIn()) { InitializeReally(); return; }

                ready_toquit = false;
                Program.Id_user = FirebaseClass.token.User.LocalId;
                if(UsernameTbox.Text.Trim().ToLower()!="" && passTbox.Text.Trim() != "")
                {
                    Program.email_user = UsernameTbox.Text.Trim().ToLower();
                    Program.pass_user = passTbox.Text.Trim();
                }
                Close();
            }
        }

        private void SetErrorText(string error, Color color)
        {
            this.error.Text = error;
            this.error.ForeColor = color;
            this.error.Show();
            progressBar1.Visible = false;
            Update();
        }
        private void SetError2Text(string error, Color color)
        {
            this.error2.Text = error;
            this.error2.ForeColor = color;
            this.error2.Show();
            progressBar1.Visible = false;
            Update();
        }

        string mode = "register";
        private void ShowPanel(int number, string message)
        {
            secretTxb.Text = "";
            if (number == 1)
            {
                panel1.Visible = true;
                //panel2.Hide();
                panel1.BringToFront();
                if (message!="") SetErrorText(message, Color.Green);
            }
            else if (number == 2)
            {
                //panel2.Show();
                panel1.Visible = false;
                panel2.BringToFront();
                secretTxb.Focus();
                if (mode == "register")
                {mess_label.Text = "Tra pochi istanti dovresti ricevere una e-mail all\'indirizzo di posta indicato. R" +
                                   "icopia il codice che troverai all\'interno del messaggio nello spazio qui sotto, " +
                                   "per poi continuare la registrazione.";
                }
                else if (mode == "pass")
                {
                    mess_label.Text = "Tra pochi istanti dovresti ricevere una e-mail all\'indirizzo di posta indicato. P" +
                                      "er rendere effettive le modifiche, inserisci la password che troverai all\'interno" +
                                      "del messaggio nello spazio qui sotto.";
                }
            }
            progressBar1.Visible = false;
            Update();
        }

        int iter = 0;
        void Animation(object sender, EventArgs e)
        {
            if (progressBar1.Value > 99 && iter<100) { progressBar1.Value = 100; iter++; return; }
            if (progressBar1.Value == 100) { iter = 0; progressBar1.Value = 0; return; }
            
            if (progressBar1.Value == 0 && iter < 1) { iter++; }
            else { progressBar1.Value += 1; iter = 0; }
        }

        void ActiveProgress()
        {
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            progressBar1.BringToFront();
        }

        string secret = "";
        bool secretgen = true;
        private async void registra_Click(object sender, EventArgs e)
        {
            ActiveProgress();
            //RegistrationForm reg = new RegistrationForm();
            //reg.ShowDialog();
            this.error.Hide();
            #region Conditions
            if (string.IsNullOrWhiteSpace(UsernameTbox.Text) ||
                string.IsNullOrWhiteSpace(passTbox.Text))
            { SetErrorText("Inserire e-mail e password per effettuare la registrazione", Color.Red); return; }
            if (passTbox.Text.Length < 8) { SetErrorText("La password deve contenere almeno 8 caratteri", Color.Red); return; }
            #endregion
            string error = await FirebaseClass.CheckFirebaseCred(UsernameTbox.Text.Trim(), passTbox.Text.Trim());
            if (error == "Password errata" || error == "") { error = "E-mail già registrata"; SetErrorText(error, Color.Red); return; }

            secret = GenerateRandomSecret();
            if(secretgen) if(!FirebaseClass.SendMail(UsernameTbox.Text.Trim(), "E-mail Registration", 
                "Ecco il codice di MoneyGuard che ti serve per registrare questa e-mail: " + secret
                ))
            { SetErrorText("Errore nella registrazione. Sicuro di aver inserito una e-mail valida?", Color.Red); return; }

            secretgen = false;
            mode = "register";
            ShowPanel(2, "");

            //ready_toquit = false;
            //Close();




        }

        private string GenerateRandomSecret()
        {
            var num = new Random();
            return num.Next(1000000, 9999999).ToString();
        }
        static public string RandomString(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }

            return res.ToString();
        }

        private async void accedi_Click(object sender, EventArgs e)
        {
            ActiveProgress();
            this.error.Hide();
            #region Conditions
            if (string.IsNullOrWhiteSpace(UsernameTbox.Text) ||
                string.IsNullOrWhiteSpace(passTbox.Text))
                { SetErrorText("Inserire e-mail e password per effettuare il login", Color.Red); return; }
            if(passTbox.Text.Length < 8) { SetErrorText("La password deve contenere almeno 8 caratteri", Color.Red); return; }
            #endregion

            string error = await FirebaseClass.CheckFirebaseCred(UsernameTbox.Text.Trim().ToLower(), passTbox.Text.Trim());
            if (error != "") { SetErrorText(error, Color.Red); FirebaseClass.token = null; return; }
            if (!FirebaseClass.FireBaseLogIn()) { SetErrorText("Errore di connessione sconosciuto, riprova più tardi", Color.Red); return; }

            ready_toquit = false;
            Program.Id_user = FirebaseClass.token.User.LocalId;
            Program.email_user = UsernameTbox.Text.Trim().ToLower();
            Program.pass_user = passTbox.Text.Trim();
            Input.RefreshPaths();
            Funzioni_utili.CreazioneCartella(@"C:\ProgramData\Cyan\Moneyguard\" + Program.Id_user);

            SetErrorText("Caricamento database..", Color.Blue);
            Task<bool> task1 = FirebaseClass.UpdateData_FromStorageToLocal(true, true);
            try {await task1; }catch (Exception) { SetErrorText("Errore di connessione, riprova più tardi", Color.Red); return; }
            SetErrorText("Caricamento immagini..", Color.Blue);
            Task<bool> task2 = FirebaseClass.UpdateImages_FromStorageToLocal(false, Program.num_max_icons_tip, Program.num_max_icons_met);
            try { await task2; } catch (Exception) { SetErrorText("Errore di connessione, riprova più tardi", Color.Red); return; }


            Properties.Settings.Default.last_access_offline = false;
            Close();


            //await DownloadFile_fromStorage(Input.path + @"\file.txt", "file.txt");
            /*
            FirebaseResponse res = client.Get(@"Users/" + UsernameTbox.Text.Replace("@", "").Replace(".", ""));
            MyUser ResUser = res.ResultAs<MyUser>();

            MyUser CurUser = new MyUser()
            {
                Email = UsernameTbox.Text,
                Password = passTbox.Text,
            };
            if(MyUser.isEqual(CurUser, ResUser))
            {
                Login.SendMail(UsernameTbox.Text.Trim(), "Credentials verify", "Ciao");
            }
            else
            {
                MessageBox.Show("ERROR: " + MyUser.error);
            }
            */
        }
        private void offline_Click(object sender, EventArgs e)
        {
            ready_toquit = false;
            Properties.Settings.Default.last_access_offline = true;
            Program.Id_user = "local";
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mode = "register";
            ShowPanel(1, "");
        }

        private async void continua_Click(object sender, EventArgs e)
        {
            if (mode == "register")
            {
                if (secretTxb.Text.Trim() == secret)
                {
                    try
                    {
                        await FirebaseClass.CreateUser(UsernameTbox.Text.Trim(), passTbox.Text.Trim());
                    }
                    catch (Exception) { SetError2Text("Errore di registrazione sconosciuto, ritenta più tardi", Color.Red);}
                    mode = "register";
                    ShowPanel(1, "Registrazione avvenuta con successo!");
                    secretgen = true;
                }
                else SetError2Text("Codice di accesso non valido", Color.Red);
            }
            else if (mode == "pass")
            {
                if (secretTxb.Text.Trim() == secret_pass)
                {
                    try
                    {
                        await FirebaseClass.ChangePassword(secret_pass.Trim());
                    }
                    catch (Exception) { SetError2Text("Errore di recupero password sconosciuto, ritenta più tardi", Color.Red); }
                    mode = "pass";
                    ShowPanel(1, "Modifica password avvenuta con successo!");
                    secretgen_pass = true;
                }
                else SetError2Text("Codice di accesso non valido", Color.Red);
            }
        }

        string secret_pass = "";
        bool secretgen_pass = true;
        private async void passdimenticata_Click(object sender, EventArgs e)
        {
            ActiveProgress();
            this.error.Hide();
            #region Conditions
            if (string.IsNullOrWhiteSpace(UsernameTbox.Text))
            { SetErrorText("Inserire e-mail per il recupero della password", Color.Red); return; }
            if (passTbox.Text.Length < 8) { SetErrorText("La password deve contenere almeno 8 caratteri", Color.Red); return; }
            #endregion
            string error = await FirebaseClass.CheckFirebaseCred(UsernameTbox.Text.Trim(), passTbox.Text.Trim());
            if (error != "Password errata" && error != "") { error = "E-mail non registrata"; SetErrorText(error, Color.Red); return; }

            secret_pass = RandomString(10);
            mode = "pass";
            ShowPanel(2, "");

            if (secretgen_pass) if (!FirebaseClass.SendMail(UsernameTbox.Text.Trim(), "Password Recover",
                 "Ecco il codice di MoneyGuard che ti serve per generare una nuova password: " + secret_pass
                 ))
                { SetErrorText("Errore nella registrazione. Sicuro di aver inserito una e-mail valida?", Color.Red); return; }
            secretgen_pass = false;
        }

        private void SaveSettings()
        {
            Properties.Settings.Default.ricordami = ricorda.Checked;
            Properties.Settings.Default.entra_aut = automat.Checked;
            if (Program.email_user != "" && Program.pass_user != "")
            {
                Properties.Settings.Default.email = Program.email_user;
                Properties.Settings.Default.pass = Program.pass_user;
            }
            Properties.Settings.Default.Save();
        }        
        

        private void label3_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
