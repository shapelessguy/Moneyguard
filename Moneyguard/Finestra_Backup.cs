using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1;

namespace Moneyguard
{
    public partial class Finestra_Backup : Form
    {
        ToolTip tooltip;
        Timer timer;
        private bool newbackups = false;
        private List<Google.Apis.Drive.v3.Data.File> files_in_backup;
        public Finestra_Backup()
        {
            InitializeComponent();
            tooltip = new ToolTip() { AutoPopDelay = 20000, };
            FormClosing += Disposer;
            timer = new Timer()
            {
                Enabled = true,
                Interval = 50,
            };
            timer.Tick += TimerTick;
            Aggiorna_Locale();
            //DriveThread = new System.Threading.Thread(Drive);
            //DriveThread.Start();
            listBox1.Click += Click1;
            listBox2.Click += Click2;
            label1.Click += Click3;
            label2.Click += Click3;
            Click += Click3;

        }
        private void Disposer(object sender, EventArgs e)
        {
            Disposer();
        }
        private void Disposer()
        {
            BackPanel.backup = false;
            timer.Dispose();
            Dispose();
        }

        private void Click1(object sender, EventArgs e)
        {
            listBox2.SelectedItem = null;
        }
        private void Click2(object sender, EventArgs e)
        {
            listBox1.SelectedItem = null;
        }
        private void Click3(object sender, EventArgs e)
        {
            listBox1.SelectedItem = null;
            listBox2.SelectedItem = null;
        }
        private void Aggiorna_Locale()
        {
            string[] files = Directory.GetFiles(Input.path + @"\Backups\");
            foreach (string stringa in files) try { listBox1.Items.Add(Funzioni_utili.DecodData(Path.GetFileNameWithoutExtension(stringa))); } catch (Exception) { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                DialogResult dr = MessageBox.Show("Sicuro di voler continuare? Il database attuale potrebbe essere perso.", "Verifica", MessageBoxButtons.YesNo);
                switch (dr)
                {
                    case DialogResult.Yes: break;
                    case DialogResult.No: return;
                }
                try
                {
                    string item = (string)listBox1.SelectedItem;
                    item = Funzioni_utili.CodData(item);
                    //File.Delete(Input.path_in);
                    //File.Copy(Input.path + @"\Backups\" + item + ".txt", Input.path_in);
                    //File.SetCreationTime(Input.path_in, DateTime.Now);
                    Program.caricamento_show = true;
                    //bool previous_shown = FinestraPrincipale.BackPanel.StandardCalendar.Visible;
                    try { new Input(Input.path + @"\Backups\" + item + ".txt"); } catch (Exception) {
                        Console.WriteLine("Cannot read backup file..");  new Input();
                        Program.caricamento_show = false;
                    }
                    if (Impostazioni.pass != "none") { FinestraPrincipale.Pass_Panel.Show(); FinestraPrincipale.Pass_Panel.BringToFront(); FinestraPrincipale.Pass_Panel.Update(); }
                    foreach (string filename in Directory.GetFiles(Input.path + "Data"))
                    {
                        try { File.Delete(filename); } catch (Exception) { }
                    }
                    //FinestraPrincipale.Finestra.Hide();
                    Hide();
                    Framments framments = new Framments(); framments.SaveAllFramments();

                    //File.CreateText(Input.path + "Backup.txt");
                    //dr = MessageBox.Show("Al prossimo avvio di MoneyGuard sarà ripristinato il database selezionato", "Verifica");
                    Program.caricamento_show = false;
                    WidgetMoneyguard.refreshpanel = true;
                    //dr = MessageBox.Show("Il database è stato ripristinato correttamente", "Ripristino");
                    //Program.fast_reboot = true;
                    //FinestraPrincipale.Finestra.Close();
                    FinestraPrincipale.BackPanel.GoCalendar(null, null);
                    Close();
                    //FinestraPrincipale.Finestra.Close();
                    //Savings.block_saving = true;
                    //Program.ready_toquit = true;
                    //WidgetMoneyguard.ready_toclose = true;
                    return;
                }
                catch (Exception) { MessageBox.Show("Errore nel ripristino del backup"); }
            }
            /*
            if (listBox2.SelectedItem != null)
            {
                try
                {
                    File.Delete(Input.path_in);
                    GoogleDrive.DownloadFileToDrive(GoogleDrive.service, files_in_backup[listBox2.SelectedIndex].Id, Input.path_in, files_in_backup[listBox2.SelectedIndex].ModifiedTime, "", "");
                    dr = MessageBox.Show("Al prossimo avvio di MoneyGuard sarà ripristinato il database selezionato", "Verifica");
                    FinestraPrincipale.Finestra.Close();
                    Savings.block_saving = true;
                    Program.ready_toquit = true;
                    return;
                }
                catch (Exception) { MessageBox.Show("Errore nel ripristino del backup. Procedere manualmente copiando nella cartella ProgramData/Cyan/Moneyguard il file selezionato"); }
            }
            */

        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null || listBox2.SelectedItem != null || (string)listBox2.SelectedItem != "Caricamento backups..") button1.Enabled = true; else button1.Enabled = false;
            if (newbackups)
            {
                newbackups = false;
                listBox2.Items.Clear();
                foreach (var file in files_in_backup) try { listBox2.Items.Add(Funzioni_utili.DecodData(Path.GetFileNameWithoutExtension(file.Name))); } catch (Exception) { }
            }
        }

        private void Drive()
        {
            listBox2.Items.Add("Caricamento backups..");
            //if (Impostazioni.always_offline) return;
            files_in_backup = new List<Google.Apis.Drive.v3.Data.File>();

            try { GoogleDrive.GetAllDriveFiles(); } catch (Exception) { MessageBox.Show("Errore nel download da Drive"); return; }

            Console.WriteLine("Display all files in Backup");
            for (int i = 0; i < GoogleDrive.allfiles.Count; i++)
                if (GoogleDrive.allfiles[i].Parents != null)
                {
                    if (GoogleDrive.allfiles[i].Parents[0] == GoogleDrive.backups_id) files_in_backup.Add(GoogleDrive.allfiles[i]);
                }
            newbackups = true;
        }
    }
}
