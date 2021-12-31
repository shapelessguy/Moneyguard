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
    public class Visual_Attributi : Panel
    {
        private Label Image;
        private Label Attributo;
        public int index;
        public static int Index = -1;
        private readonly string image_txt;
        private readonly string tipo;
        public bool delete;

        public void Disposer()
        {
            Attributo.Dispose();
            Image.BackgroundImage.Dispose();
            Image.Dispose();
            Dispose();
        }
        public Visual_Attributi(string tipo, string image_txt, bool attributo)
        {
            DoubleBuffered = true;
            this.image_txt = image_txt;
            this.tipo = tipo;
            Image = new Label()
            {
                BackgroundImage = Funzioni_utili.TakePicture(tipo, 1),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(Image);
            if(attributo) Image.BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject(image_txt)));
            Attributo = new Label()
            {
                AutoSize = true,
                Text = tipo,
            };
            Controls.Add(Attributo);
            Image.MouseClick += new MouseEventHandler(ClickTipo);
            Attributo.MouseClick += new MouseEventHandler(ClickTipo);
            Image.MouseEnter += new EventHandler(BordoShow);
            Attributo.MouseEnter += new EventHandler(BordoShow);
            MouseEnter += new EventHandler(BordoShow);
            Image.MouseLeave += new EventHandler(BordoHide);
            Attributo.MouseLeave += new EventHandler(BordoHide);
            MouseLeave += new EventHandler(BordoHide);
        }

        public void SetSize(Size size)
        {
            Size = size;
            Image.Location = new Point(0, 0);
            Image.Width = this.Width;
            Image.Height = (int)(this.Size.Height * 0.8);
            Attributo.Location = new Point((int)(Image.Location.X + Image.Width / 2 - Attributo.Width / 2), Image.Height);
            Attributo.Font = new Font(BackPanel.font1, (int)(Width * 0.1), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void ClickTipo(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (delete)
                {
                    if (Impostazioni.tipo_metodo_delete == 1)
                    {
                        foreach (Eventi evento in Input.eventi) if (evento.Get_Attributo() == "Introito" || evento.Get_Attributo() == "Spesa") if(evento.GetTipo() == Impostazioni.tipo_delete) evento.SetTipo(Attributo.Text);
                        for (int i = 0; i < Input.tipi.Count; i++) if (Input.tipi[i] == Impostazioni.tipo_delete) {
                                Etichette.Delete_tipi_sort(i); Input.tipi.RemoveAt(i); Input.tipi_icons.RemoveAt(i); break; }
                        foreach (Etichette etichetta in FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list) if (etichetta.tipo == Impostazioni.tipo_delete)
                            { FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list.Remove(etichetta); FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi.Controls.Remove(etichetta); break; }
                    }
                    else if (Impostazioni.tipo_metodo_delete == 2)
                    {
                        foreach (Eventi evento in Input.eventi)
                        {
                            if (evento.Get_Attributo() == "Introito" || evento.Get_Attributo() == "Spesa") if(evento.GetMetodo() == Impostazioni.tipo_delete) evento.SetMetodo(Attributo.Text);
                            if (evento.Get_Attributo() == "Trasferimento")
                            {
                                if(evento.GetTipo() == Impostazioni.tipo_delete) evento.SetTipo(Attributo.Text);
                                if (evento.GetMetodo() == Impostazioni.tipo_delete) evento.SetMetodo(Attributo.Text);
                            }
                        }

                        int j = 0; 
                        for (int i = 0; i < Input.metodi.Count; i++) if (Input.metodi[i] == Impostazioni.tipo_delete)
                            { Input.totali[j] += Input.totali[i]; Input.totali_iniziali[j] += Input.totali_iniziali[i]; Etichette.Delete_metodi_sort(i); Input.totali_iniziali.RemoveAt(i); Input.metodi_icons.RemoveAt(i); Input.totali.RemoveAt(i); Input.metodi.RemoveAt(i); break; }

                        foreach (Etichette etichetta in FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list) if (etichetta.tipo == Input.metodi[j]) etichetta.SetImages();
                                foreach (Etichette etichetta in FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list) if (etichetta.tipo == Impostazioni.tipo_delete)
                            { FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list.Remove(etichetta); FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi.Controls.Remove(etichetta); break; }
                    }
                    Savings.save_all = true;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.ResizeImp();
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.PanelDelete.Hide();
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.PanelDelete.Disposer();
                }
                else
                {
                    string last_attributo = FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Scelta_attributo_txt.Text;
                    string this_attributo = Attributo.Text;
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.attributo = Attributo.Text;
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Attributo_scelta.attributo = Attributo.Text;
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Scelta_attributo_txt.Text = Attributo.Text;
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Scelta_attributo_img.BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject(image_txt)));
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Attributo_scelta.Disposer();
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.sceltametodo = true;


                    if (this_attributo == "Introito" || this_attributo == "Spesa") if (last_attributo == "Introito" || last_attributo == "Spesa") return;
                    if (this_attributo == "Trasferimento") if (last_attributo == "Trasferimento") return;
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.tipo = "";
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Scelta_tipo_txt.Text = "";
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Scelta_tipo_img.BackgroundImage = null;
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.metodo = "";
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Scelta_metodo_txt.Text = "";
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Scelta_metodo_img.BackgroundImage = null;
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Resize_NewEvento();
                }
            }
        }
        private void BordoShow(object sender, EventArgs e)
        {
            if (Index == index) return;
            Index = index;
            if (delete) { foreach (Visual_Attributi tip in FinestraPrincipale.BackPanel.Panel_Impostazioni.PanelDelete.VisualAttributi) tip.BordoHide(); }
            else { foreach (Visual_Attributi tip in FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Attributo_scelta.VisualAttributi) tip.BordoHide(); }
            BackColor = Color.LightCyan;
        }
        private void BordoHide(object sender, EventArgs e)
        {
            BordoHide();
        }
        public void BordoHide()
        {
            if (Index == index) return;
            BackColor = Color.Transparent;
        }

    }
}
