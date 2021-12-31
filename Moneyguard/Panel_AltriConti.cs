using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1;

namespace Moneyguard
{
    public class Panel_AltriConti : Panel
    {
        List<VisualAltriConti> Visual = new List<VisualAltriConti>();
        Label Riferimento, Altriconti_txt;
        VisualAltriConti Totale;
        public Panel Pannello;

        public void Disposer()
        {
            Riferimento.Dispose();
            foreach (var tip in Visual) tip.Disposer();
            Dispose();
        }
        public Panel_AltriConti()
        {
            DoubleBuffered = true;
            Totale = new VisualAltriConti(true)
            {
                metodo = "Totale"
            };
            Controls.Add(Totale);
            Altriconti_txt = new Label()
            {
                Text = "Altri conti:",
                TextAlign = ContentAlignment.MiddleCenter,
            };
            Controls.Add(Altriconti_txt);
            Pannello = new Panel()
            {
                AutoScroll = true,
            };
            Controls.Add(Pannello);

            Visible = false;
            BorderStyle = BorderStyle.Fixed3D;
            BackColor = System.Drawing.Color.LightGray;
            Riferimento = new Label();
            Pannello.Controls.Add(Riferimento);
            Riferimento.SetBounds(0, 0, 1, 1);

            RefreshForm();
        }

        public void RefreshForm()
        {
            Size = new Size((int)(FinestraPrincipale.BackPanel.Width * 0.35), (int)(FinestraPrincipale.BackPanel.Height * 0.6));
            Altriconti_txt.Location = new Point(0, 0);
            Altriconti_txt.Size = new Size(Width ,(int)(FinestraPrincipale.BackPanel.Height * 0.1));
            Altriconti_txt.Font = new Font(BackPanel.font1, (int)(Width * 0.07), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Pannello.Location = new Point(0, (int)(FinestraPrincipale.BackPanel.Height * 0.1));
            Pannello.Size = new Size(Width, (int)(FinestraPrincipale.BackPanel.Height * 0.38));
            foreach (var tip in Visual) { Pannello.Controls.Remove(tip); tip.Disposer(); } Visual.Clear();

            for (int i = 0; i < Input.metodi.Count; i++) {if (Input.metodi_sort[i] > 1) { Visual.Add(new VisualAltriConti(false) { metodo = Input.metodi[Input.metodi_sort[i]] }); Pannello.Controls.Add(Visual[Visual.Count-1]); } }
            foreach (var tip in Visual) if(tip != null) tip.RefreshForm();
            Totale.RefreshForm();
            ResizeForm();
        }

        public void ResizeForm()
        {
            Size = new Size((int)(FinestraPrincipale.BackPanel.Width * 0.35), (int)(FinestraPrincipale.BackPanel.Height * 0.6));
            Location = new Point((FinestraPrincipale.BackPanel.Width-Width)/2, (FinestraPrincipale.BackPanel.Height - Height) / 2 - (int)(Height*0.1));
            Totale.Location = new Point(Riferimento.Location.X, Height - (int)(Totale.Height*1.2));
            if (Visual.Count > 0) Visual[0].Location = Riferimento.Location;
            for(int i=1; i<Visual.Count; i++)
            {
                Visual[i].Location = new Point(Visual[i-1].Location.X, Visual[i - 1].Location.Y + Visual[i-1].Height);
            }
        }
    }
}
