using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Moneyguard
{
    public class Panel_attributo_scelta : Panel
    {
        public List<Visual_Attributi> VisualAttributi = new List<Visual_Attributi>();
        public Label Delete_txt;
        private readonly int num_colonne = 3;
        public string attributo;
        public bool delete;
        public static bool exist = false;
        public void Disposer()
        {
            foreach (Visual_Attributi tip in VisualAttributi) { tip.Disposer(); Controls.Remove(tip); }
            if (delete) Delete_txt.Dispose();
            exist = false;
            Dispose();
        }

        public Panel_attributo_scelta(bool delete)
        {
            DoubleBuffered = true;
            this.delete = delete;
            BorderStyle = BorderStyle.FixedSingle;
            AutoScroll = true;
            Visible = false;
            exist = true;
            int i = 0;
            if (delete)
            {
                Delete_txt = new Label();
                Controls.Add(Delete_txt);
                num_colonne = 5;
                if(Impostazioni.tipo_metodo_delete == 1)
                {
                    foreach(string tipo in Input.tipi) if(tipo!= Impostazioni.tipo_delete)
                        {
                            VisualAttributi.Add(new Visual_Attributi(tipo, Associazione.IconaAssociata(tipo), false) {delete = true, });
                            Controls.Add(VisualAttributi[i]);
                            i++;
                        }
                    Delete_txt.Text = "Seleziona la tipologia a cui vuoi associare gli elementi di quest'ultima";
                }
                else if (Impostazioni.tipo_metodo_delete == 2)
                {
                    foreach (string tipo in Input.metodi) if (tipo != Impostazioni.tipo_delete)
                        {
                            VisualAttributi.Add(new Visual_Attributi(tipo, Associazione.MiconaAssociata(tipo), false) { delete = true, });
                            Controls.Add(VisualAttributi[i]);
                            i++;
                        }
                    Delete_txt.Text = "Seleziona il metodo a cui vuoi associare gli elementi di quest'ultimo";
                }
            }
            else
            {
                foreach (string attributo in Input.attributi)
                {
                    VisualAttributi.Add(new Visual_Attributi(attributo, Associazione.AiconaAssociata(attributo), true));
                    Controls.Add(VisualAttributi[i]);
                    i++;
                }
            }
            MouseEnter += new EventHandler(MouseEntered);
        }

        public void SetLocationY(int y)
        {
            Location = new Point(FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Scelta_tipo.Location.X - 2, y);
        }
        public void ResizeForm()
        {
            if (delete)
            {
                Delete_txt.Font = new System.Drawing.Font("Script MT Bold", (int)(FinestraPrincipale.BackPanel.Width * 0.01 + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Delete_txt.Size = new Size((int)((FinestraPrincipale.BackPanel.Width) * 0.8 - 25), (int)(FinestraPrincipale.BackPanel.Height * 0.08));
                Delete_txt.Location = new Point(0, 0);
                Size = new Size((int)((FinestraPrincipale.BackPanel.Width) * 0.8), (int)(FinestraPrincipale.BackPanel.Height * 0.6));
                Location = new Point((FinestraPrincipale.BackPanel.Width - Width) / 2, (FinestraPrincipale.BackPanel.Height - Height) / 2);
            }
            else Size = new Size((int)((FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Width - Location.X - Location.X) * 0.6 + 10), (int)(((FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Width - Location.X - Location.X) * 0.6 + 10)/2.5));
            int i = 0, j = 0;
            foreach (Visual_Attributi tip in VisualAttributi)
            {
                tip.SetSize(new Size((int)((Width / num_colonne)*0.8 + 15), (int)((Width / num_colonne)*0.9) + 15));
                if(delete) tip.Location = new Point(tip.Width * i - tip.Width * j * num_colonne, Delete_txt.Height + (int)(tip.Height * (j * 1.2)));
                else tip.Location = new Point(tip.Width * i - tip.Width * j * num_colonne, (int)(tip.Height * (j * 1.2)));
                tip.index = i;
                i++;
                if (i % num_colonne == 0) j++;
            }
            
            Visible = true;
        }
        private void MouseEntered(object sender, EventArgs e)
        {
            Visual_Attributi.Index = -1;
            foreach (Visual_Attributi tip in VisualAttributi)
            {
                tip.BordoHide();
            }
        }
    }
}
