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
    public partial class Sicurezza : Form
    {
        public Sicurezza()
        {
            InitializeComponent();
            FinestraPrincipale.BackPanel.GotFocus += Closing_;
        }

        private void Closing_(object sender, EventArgs e)
        {
            FinestraPrincipale.BackPanel.GotFocus -= Closing_;
            Close();
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Close();
            Input.eventi.Clear();
            Savings.SaveEvents();
            FinestraPrincipale.BackPanel.StandardCalendar.RefreshWindow();
            try
            {
                FinestraPrincipale.BackPanel.Panel_Giorno.Tipi.Controls.Clear();
                FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale.Clear();
                FinestraPrincipale.BackPanel.Panel_Giorno.empty.Visible = true;
            }
            catch (Exception) { Console.WriteLine("Errore Button2"); }
            try
            {
                Input.eventi_scremati.Clear();
                Input.all_attributi.Clear();
                Input.tipi_scremati.Clear();
                Input.metodi_scremati.Clear();
                FinestraPrincipale.BackPanel.Panel_Ricerca.previous_tipi_inclusi.Clear();
                FinestraPrincipale.BackPanel.Panel_Ricerca.RicercaEventi();
            }
            catch (Exception) { Console.WriteLine("Errore Button3"); }
            MessageBox.Show("Tutti gli eventi sono stati eliminati");
        }
    }
}
