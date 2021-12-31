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
    public partial class Change_Attributo_Sicurezza : Form
    {
        string attributo;
        string newattributo;
        public Change_Attributo_Sicurezza(string attributo, string newattributo)
        {
            InitializeComponent();
            LostFocus += Exit;
            this.attributo = attributo;
            this.newattributo = newattributo;
            Visible = true;
        }

        void Exit(object sender, EventArgs e)
        {
            Change_Attributo finestra = new Change_Attributo(attributo)
            {
                Location = Location
            };
            Close();
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            Exit(sender, e);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            foreach (Eventi evento in Input.eventi) for (int i = 0; i < evento.GetAttributi().Count; i++) { if (Funzioni_utili.Scremato(evento.GetAttributo(i)) == attributo) evento.SetAttributo(i, newattributo); }
            Input.LoadAttributi();
            if (FinestraPrincipale.BackPanel.Panel_Giorno != null)
            {
                if (FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Visible) foreach (TextBox txt in FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Controls.OfType<TextBox>()) if (Funzioni_utili.Scremato(txt.Text) == attributo) txt.Text = newattributo;
                if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.AttributoPanel.Visible) foreach (TextBox txt in FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.AttributoPanel.Controls.OfType<TextBox>()) if (Funzioni_utili.Scremato(txt.Text) == attributo) txt.Text = newattributo;
                if (FinestraPrincipale.BackPanel.Panel_Giorno.PanelMotore.parziale == attributo) FinestraPrincipale.BackPanel.Panel_Giorno.PanelMotore.parziale = newattributo;
                FinestraPrincipale.BackPanel.Panel_Giorno.PanelMotore.Reload();
            }
            Change_Attributo.Aggiorna();
            Close();
        }
    }
}
