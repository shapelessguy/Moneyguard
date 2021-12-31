using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moneyguard
{
    public partial class Latest_Version : Form
    {
        static public Latest_Version latest_version;
        public Latest_Version(bool show_remember)
        {
            latest_version = this;
            InitializeComponent();
            label1.Text = "Nuova versione di MoneyGuard - V" + Finestra_Updates.latest_version + " - disponibile. \nDesideri installarla?";
            if (!show_remember) checkBox1.Visible = false;
            checkBox1.Checked = Properties.Settings.Default.show_update_mess;
        }

        public void HideRemember()
        {
            checkBox1.Hide();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.show_update_mess = checkBox1.Checked;
            Properties.Settings.Default.Save();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            checkBox1.Visible = false;
            await Finestra_Updates.DownloadLastVersion();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
