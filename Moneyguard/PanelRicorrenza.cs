using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moneyguard
{
    public class PanelRicorrenza : Panel
    {
        const int perc_ricorrenza_width = 30;

        List<Etichetta_Automatica> etichette = new List<Etichetta_Automatica>();
        Label vuoto;
        Timer timer;
        Panel Pannello;
        bool initial = true;
        public PanelRicorrenza()
        {
            BackColor = Color.LightSlateGray;
            BorderStyle = BorderStyle.FixedSingle;
            Visible = false;
            Pannello = new Panel()
            {
                AutoScroll = true,
                BackColor = Color.LightSlateGray,
            };
            Controls.Add(Pannello);
            vuoto = new Label()
            {
                Text = "VUOTO",
                ForeColor = Color.Red,
                Visible = true,
            };
            Update();
            BringToFront();
            
        }

        public void Disposer()
        {
            foreach (Etichetta_Automatica etichetta in etichette) etichetta.Disposer();
            Dispose();
        }
        public void ForceRefresh()
        {
            for(int j=0; j<50; j++)
            {
                Input.eventi_aut.Add(new Eventi_Aut());
                Input.eventi_aut[Input.eventi_aut.Count - 1].Set_Attributo("Introito");
                if(j%2==0) Input.eventi_aut[Input.eventi_aut.Count - 1].SetTipo("Lezioni");
                else Input.eventi_aut[Input.eventi_aut.Count - 1].SetTipo("Colazione");
                Input.eventi_aut[Input.eventi_aut.Count - 1].SetMetodo("Portafogli");
                Input.eventi_aut[Input.eventi_aut.Count - 1].SetValore(j);
                Input.eventi_aut[Input.eventi_aut.Count - 1].SetData(new int[] { DateTime.Now.Second, DateTime.Now.Minute, DateTime.Now.Hour, 6, 10, 2019 });
                Input.eventi_aut[Input.eventi_aut.Count - 1].SetData_modifica(new int[] { DateTime.Now.Second, DateTime.Now.Minute, DateTime.Now.Hour, 6, 10, 2019 });
                Input.eventi_aut[Input.eventi_aut.Count - 1].Load();
            }
            
            etichette.Clear();
            Pannello.Controls.Clear();
            Pannello.Controls.Add(vuoto);
            foreach (Eventi_Aut evento in Input.eventi_aut) etichette.Add(new Etichetta_Automatica(evento));
            int i = 0;
            foreach (Etichetta_Automatica etichetta in etichette) { Pannello.Controls.Add(etichetta); i++; if (i == 1) etichette[0].Location = new Point(10,0); }
            if (etichette.Count == 0) { vuoto.Show(); } else vuoto.Hide();
            
        }

        public void RefreshForm()
        {
            Size = new System.Drawing.Size((int)(FinestraPrincipale.BackPanel.StandardCalendar.Size.Width * perc_ricorrenza_width / 100), (int)(FinestraPrincipale.BackPanel.StandardCalendar.Size.Height - FinestraPrincipale.BackPanel.Menù.Height - 6));
            Pannello.Size = new Size(Width - 13, Height - 20);
            Pannello.Location = new Point(10, 10);
            if (Visible) FinestraPrincipale.BackPanel.StandardCalendar.orecchietta.Location = new Point(Width - (int)(FinestraPrincipale.BackPanel.StandardCalendar.orecchietta.Width * 0.9), FinestraPrincipale.BackPanel.StandardCalendar.orecchietta.Location.Y);
            FinestraPrincipale.BackPanel.StandardCalendar.orecchietta.Update();
            ResizeForm();
        }

        public void ShowRic()
        {
            if (initial)
            {
                ForceRefresh();
            }

            Visible = true;
            RefreshForm();
            BringToFront();
            if (initial)
            {
                Aggiorna();
                LocateEtichette();
                initial = false;
            }
            Update();

        }
        public void HideRic()
        {
            FinestraPrincipale.BackPanel.StandardCalendar.orecchietta.Location = new Point( - (int)(FinestraPrincipale.BackPanel.StandardCalendar.orecchietta.Width * 0.9), FinestraPrincipale.BackPanel.StandardCalendar.orecchietta.Location.Y);
            Visible = false;
            Update();
        }

        public void ResizeForm()
        {
            vuoto.Location = new Point((int)(Pannello.Width-vuoto.Width)/2, (int)(Pannello.Height -vuoto.Height)/2);
            vuoto.BringToFront();
            foreach (Etichetta_Automatica etichetta in etichette) etichetta.ResizeForm(Pannello.Width);
            LocateEtichette();
        }

        public void LocateEtichette()
        {
            if (etichette.Count < 2) return;
            for(int i=1; i<etichette.Count; i++)
            {
                etichette[i].Location = new Point(etichette[i - 1].Location.X, etichette[i - 1].Location.Y + (int)(Width * 0.3) + 10);
            }
        }

        public void Aggiorna()
        {
            foreach (Etichetta_Automatica etichetta in etichette) etichetta.Aggiorna();
        }
    }
}
