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
    public partial class Change_Attributo : Form
    {
        public Change_Attributo(string attributo)
        {
            InitializeComponent();
            label1.Text = attributo;
            Visible = true;
            LostFocus += Exit;
        }
        void Exit(object sender, EventArgs e)
        {
            Close();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Exit(sender, e);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            string testo = Funzioni_utili.Scremato(textBox1.Text);
            if (testo == "") label4.Show();
            foreach (string text in Input.all_attributi) if (text == testo)
                {
                    Change_Attributo_Sicurezza finestra = new Change_Attributo_Sicurezza(label1.Text, testo)
                    {
                        Location = Location
                    };
                    Close();
                    return;
                }
            foreach (Eventi evento in Input.eventi) for (int i = 0; i < evento.GetAttributi().Count; i++) { if (Funzioni_utili.Scremato(evento.GetAttributo(i)) == label1.Text) evento.SetAttributo(i, testo); }
            Input.LoadAttributi();
            if (FinestraPrincipale.BackPanel.Panel_Giorno != null)
            {
                if (FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Visible) foreach(TextBox txt in FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Controls.OfType<TextBox>()) if (Funzioni_utili.Scremato(txt.Text) == label1.Text) txt.Text = testo;
                if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.AttributoPanel.Visible) foreach (TextBox txt in FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.AttributoPanel.Controls.OfType<TextBox>()) if (Funzioni_utili.Scremato(txt.Text) == label1.Text) txt.Text = testo;
                if (FinestraPrincipale.BackPanel.Panel_Giorno.PanelMotore.parziale == label1.Text) FinestraPrincipale.BackPanel.Panel_Giorno.PanelMotore.parziale = testo;
                FinestraPrincipale.BackPanel.Panel_Giorno.PanelMotore.Reload();
            }
            Aggiorna();
            Exit(sender, e);
        }

        public static void Aggiorna()
        {
            Input.Scrematura_eventi();
            try
            {
                if (FinestraPrincipale.BackPanel.Panel_Ricerca.Visible)
                {
                    FinestraPrincipale.BackPanel.Panel_Ricerca.Fill_Eventi_filtrati();
                    FinestraPrincipale.BackPanel.Panel_Ricerca.Scaletta.Aggiorna_Attributi();
                }
            }
            catch (Exception) { }
            try
            {
                if (FinestraPrincipale.BackPanel.Panel_Ricerca.PanelMotore_Attributi.Visible) { FinestraPrincipale.BackPanel.Panel_Ricerca.PanelMotore_Attributi.AllAttributi.Items.Clear(); FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione_Attributi.CheckAllAttributi(); FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione_Attributi.CheckAllAttributi(); }
            }
            catch (Exception) { }
            try
            {
                if (FinestraPrincipale.BackPanel.Panel_Ricerca.PanelMotore_Tipologie.Visible) { FinestraPrincipale.BackPanel.Panel_Ricerca.PanelMotore_Tipologie.AllAttributi.Items.Clear(); FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione.CheckAllAttributi(); FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione.CheckAllAttributi(); }
            }
            catch (Exception) { }

            try
            {
                if (FinestraPrincipale.BackPanel.Panel_Ricerca.PanelMotore_Metodi.Visible) { FinestraPrincipale.BackPanel.Panel_Ricerca.PanelMotore_Metodi.AllAttributi.Items.Clear(); FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione_Metodi.CheckAllAttributi(); FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione_Metodi.CheckAllAttributi(); }
            }
            catch (Exception) { }
        }
    }
}
