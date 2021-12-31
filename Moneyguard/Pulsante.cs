using Moneyguard;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public class Pulsante : Panel
    {
        public Panel panel_tipo;
        private Label lab_image;
        public Label lab_tipo;
        public string tipo;
        public Label lab_totale;
        public Label delete;
        // public Label delete_opaco;
        public PictureBox metodoPic;
        public PictureBox transPic;
        public PictureBox attributoPic;
        public List<string> attributi = new List<string>();
        public List<TextBox> textbox = new List<TextBox>();
        public List<Label> Controllo = new List<Label>();
        public double valore;
        public string attributo;
        public string metodo;
        public int[] data, data_modifica;
        private Label new_attributo = new Label();
        public int index =-1;
        public int panelindex = 0;
        public bool new_evento = false;
        public bool bordo = false;
        public static string tipo_statico;
        public static bool attributo_statico;
        ToolTip toolTip = new ToolTip();

        private long datacode, datacode_modifica;


        public Pulsante()
        {
            Iniziare();
            //panel_tipo.MouseClick += new System.EventHandler(ClickNull);
        }

        public void Refresher()
        {
           
        }
        public void Disposer()
        {
            lab_image.BackgroundImage.Dispose();
            metodoPic.BackgroundImage.Dispose();
            lab_image.Dispose();
            lab_tipo.Dispose();
            lab_totale.Dispose();
            delete.BackgroundImage.Dispose();
            delete.Dispose();
            attributoPic.BackgroundImage.Dispose();
            attributoPic.Dispose();
            if(transPic.BackgroundImage != null) transPic.BackgroundImage.Dispose();
            transPic.Dispose();
            panel_tipo.Dispose();
            attributi.Clear();
            textbox.Clear();
            Controllo.Clear();
            new_attributo.Dispose();
        }
        private void Iniziare()
        {

            panel_tipo = new Panel()
            {
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                ForeColor = System.Drawing.SystemColors.ControlText,
                BackColor = Color.AliceBlue,
            };
            panel_tipo.Click += new EventHandler(FinestraPrincipale.BackPanel.Panel_Giorno.ClickNull);
        }

        public void SetSize(Size size)
        {
            panel_tipo.Size = size;
            lab_image.Size = new Size((int)(panel_tipo.Size.Height), (int)(panel_tipo.Size.Height));
            lab_tipo.Size = new Size(panel_tipo.Size.Width - lab_image.Size.Width, (int)(panel_tipo.Size.Height * 0.5));
            lab_totale.Size = new Size(panel_tipo.Size.Width - (int)(lab_image.Size.Width * 2.5), (int)(panel_tipo.Size.Height * 0.5));
            metodoPic.Size = new Size((int)(lab_totale.Height * 0.7), (int)(lab_totale.Height * 0.7));
            transPic.Size = metodoPic.Size;
            attributoPic.Size = metodoPic.Size;
            delete.Width = (int)(lab_tipo.Width * 0.06) + 4; delete.Height = (int)(lab_tipo.Width * 0.06) + 4;
            ResizeAttributi();
        }

        public void SetLocation(Point location)
        {
            panel_tipo.Location = location;
            lab_image.Location = new System.Drawing.Point(0, 0);
            lab_tipo.Location = new System.Drawing.Point(panel_tipo.Size.Height, 0);
            lab_totale.Location = new System.Drawing.Point(panel_tipo.Size.Height, panel_tipo.Size.Height / 2);
            metodoPic.Location = new Point((int)(panel_tipo.Size.Height + lab_totale.Width), (int)(panel_tipo.Size.Height * 0.55));
            transPic.Location = new Point((int)(metodoPic.Location.X + metodoPic.Width*1.4), (int)(panel_tipo.Size.Height * 0.55));
            attributoPic.Location = new Point((int)(transPic.Location.X + transPic.Width*1.4), (int)(panel_tipo.Size.Height * 0.55));
            delete.Location = new Point(lab_tipo.Width - (int)(delete.Width * 1.3) - 2, (int)(delete.Width * 0.2) - 2);
            if (attributo == "Trasferimento")
            {
                transPic.BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("Trasferimento_dx")));
                lab_image.BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("Trasferimento")));
                metodoPic.BackgroundImage = Funzioni_utili.TakePicture(Funzioni_utili.GetTipo(tipo), 2);
                attributoPic.BackgroundImage = Funzioni_utili.TakePicture(metodo, 2);
            } else transPic.BackgroundImage = base.BackgroundImage;
        }
        public void SetImage(string attributo_img, string tipo_img, string metodo_img)
        {
            SuspendLayout();

            lab_image = new Label()
            {
                BackgroundImage = Funzioni_utili.TakePicture(Associazione.TipoAssociato(tipo_img), 1),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            lab_tipo = new Label()
            {
            };
            lab_totale = new Label()
            {
            };
            metodoPic = new PictureBox()
            {
                BackgroundImage = Funzioni_utili.TakePicture(Associazione.MetodoAssociato(metodo_img), 2),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            attributoPic = new PictureBox()
            {
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject(attributo_img))),
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            transPic = new PictureBox()
            {
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            delete = new Label
            {
                BackgroundImageLayout = ImageLayout.Stretch,
                BackgroundImage = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("Red_X"))),
                Visible = false,
            };


            foreach (string attributo in attributi)
            {
                textbox.Add(new TextBox() { Text = attributo, BorderStyle = System.Windows.Forms.BorderStyle.None, });
                Controllo.Add(new Label() { BackgroundImageLayout = ImageLayout.Stretch, Visible = false, });
            }
            ResizeAttributi();
            panel_tipo.Controls.Add(lab_image);
            panel_tipo.Controls.Add(lab_tipo);
            panel_tipo.Controls.Add(lab_totale);
            panel_tipo.Controls.Add(metodoPic);
            panel_tipo.Controls.Add(transPic);
            panel_tipo.Controls.Add(attributoPic);
            lab_tipo.Controls.Add(delete);

            lab_image.MouseEnter += new EventHandler(EnterEvento);
            lab_tipo.MouseEnter += new EventHandler(EnterEvento);
            lab_totale.MouseEnter += new EventHandler(EnterEvento);
            panel_tipo.MouseEnter += new EventHandler(EnterEvento);
            metodoPic.MouseEnter += new EventHandler(EnterEvento);
            transPic.MouseEnter += new EventHandler(EnterEvento);
            attributoPic.MouseEnter += new EventHandler(EnterEvento);
            delete.MouseEnter += new EventHandler(EnterEvento);
            MouseEnter += new EventHandler(EnterEvento);

            lab_image.MouseClick += new MouseEventHandler(ClickEvento);
            lab_tipo.MouseClick += new MouseEventHandler(ClickEvento);
            lab_totale.MouseClick += new MouseEventHandler(ClickEvento);
            metodoPic.MouseClick += new MouseEventHandler(ClickEvento);
            transPic.MouseClick += new MouseEventHandler(ClickEvento);
            attributoPic.MouseClick += new MouseEventHandler(ClickEvento);
            panel_tipo.MouseClick += new MouseEventHandler(ClickEvento);
            MouseClick += new MouseEventHandler(ClickEvento);

            delete.MouseClick += new MouseEventHandler(DeleteEvento);

            foreach (Pulsante pulsante in FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale) pulsante.panel_tipo.BackColor = Color.AliceBlue;
            ResumeLayout();
        }
        public void SetTooltip()
        {
            string message = "Modificato:"+Date.ShowDateComplete(data_modifica);
            FinestraPrincipale.BackPanel.Panel_Giorno.toolTip.SetToolTip(lab_image, message);
            FinestraPrincipale.BackPanel.Panel_Giorno.toolTip.SetToolTip(lab_tipo, message);
            FinestraPrincipale.BackPanel.Panel_Giorno.toolTip.SetToolTip(lab_totale, message);
            FinestraPrincipale.BackPanel.Panel_Giorno.toolTip.SetToolTip(panel_tipo, message);
            FinestraPrincipale.BackPanel.Panel_Giorno.toolTip.SetToolTip(metodoPic, message);
            FinestraPrincipale.BackPanel.Panel_Giorno.toolTip.SetToolTip(transPic, message);
            FinestraPrincipale.BackPanel.Panel_Giorno.toolTip.SetToolTip(attributoPic, message);
        }
        public void TextTipo(string nome_tipo)
        {
            tipo = nome_tipo;
            lab_tipo.Text = nome_tipo;
            lab_tipo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        }
        public new void Text(string valore)
        {
            lab_totale.Text = valore;
            lab_totale.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        }
        


        private void EnterEvento(object sender, EventArgs e)
        {
            if (FinestraPrincipale.BackPanel.Panel_Giorno.pulsanti_selectable == false || ProprietàGiorno.attributo_selected) return;
            foreach (Pulsante pulsante in FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale)
            {
                for (int i = 0; i < pulsante.textbox.Count; i++)
                {
                    if (pulsante.textbox[i].Focused) return;
                }
                if(pulsante.delete != delete && pulsante.bordo == false) pulsante.delete.Visible = false;
            }

            if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato != null) if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Visible == true) return;
            if (bordo == true) panel_tipo.BorderStyle = BorderStyle.Fixed3D; else panel_tipo.BorderStyle = BorderStyle.FixedSingle;
            foreach (Pulsante panel in FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale)
            {
                if (panel.bordo == true) return;
            }
            if (attributo == "Trasferimento") { tipo_statico = attributo; attributo_statico = true; }
            else { tipo_statico = tipo; attributo_statico = false; }
            if (FinestraPrincipale.BackPanel.Panel_Giorno.panel_corrente != index)
            {
                foreach (Pulsante pulsante in FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale) pulsante.panel_tipo.BackColor = Color.AliceBlue;
                if (textbox.Count > 0)
                {
                    if (textbox.Count < 8) FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Size = new Size((int)(FinestraPrincipale.Finestra.Bounds.Width * 0.35), textbox[0].Height * textbox.Count() + 5);
                    else FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Size = new Size((int)(FinestraPrincipale.Finestra.Bounds.Width * 0.35), textbox[0].Height * 8 + 5);
                }
                else FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Size = new Size((int)(FinestraPrincipale.Finestra.Bounds.Width * 0.35), 31);
                panel_tipo.BackColor = Color.Azure;
                FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Controls.Clear();
                if (textbox.Count > 0)
                {
                    for (int i = 0; i < textbox.Count; i++)
                    {
                        FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Controls.Add(textbox[i]);
                        FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Controls.Add(Controllo[i]);
                        textbox[i].Click += FinestraPrincipale.BackPanel.Panel_Giorno.TextClick;
                        textbox[i].KeyDown += FinestraPrincipale.BackPanel.Panel_Giorno.EnterAttributo;
                        textbox[i].KeyUp += CheckAttributi;
                    }
                }
                ClearNullTextbox();
                CheckAllAttributi();
                FinestraPrincipale.BackPanel.Panel_Giorno.NewTextbox();
                if (textbox.Count > 0) textbox[0].Location = new Point(4 + FinestraPrincipale.BackPanel.Panel_Giorno.size_controllo *2, 0);
                FinestraPrincipale.BackPanel.Panel_Giorno.Attributo_txt.Visible = true;
                if (textbox.Count > 0) FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Visible = true;
                delete.Visible = true;
                FinestraPrincipale.BackPanel.Panel_Giorno.panel_corrente = index;
                FinestraPrincipale.BackPanel.Panel_Giorno.pulsanti_selectable = true;
                ProprietàGiorno.attributo_selected = false;
            }
        }

        public void ClearNullTextbox()
        {
            for (int i = textbox.Count - 1; i >= 0; i--) if (Funzioni_utili.Scremato(textbox[i].Text) == "")
                {
                    FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Controls.Remove(textbox[i]); textbox.RemoveAt(i);
                    if(Controllo[i] != null) FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Controls.Remove(Controllo[i]); Controllo.RemoveAt(i);
                }
        }
        private void ClickEvento(object sender, MouseEventArgs e)
        {
            FinestraPrincipale.BackPanel.ClickNull();
            if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato != null) if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Visible == true) return;
            if (e.Button == MouseButtons.Left)
            {
                if (FinestraPrincipale.BackPanel.Panel_Giorno.panel_clicked == index) return;
                attributi.Clear();
                foreach (TextBox txt in textbox) if (txt.Text != "") { attributi.Add(txt.Text); }
                SuspendLayout();
                foreach (Pulsante panel in FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale)
                {
                    panel.panel_tipo.BorderStyle = BorderStyle.FixedSingle;
                    panel.panel_tipo.BackColor = Color.AliceBlue;
                    if (panel.bordo) FinestraPrincipale.BackPanel.Panel_Giorno.ClickNull();
                    panel.bordo = false;
                }
                bordo = true;
                panel_tipo.Show();
                FinestraPrincipale.BackPanel.Panel_Giorno.Giorno_txt.Hide();
                FinestraPrincipale.BackPanel.Panel_Giorno.button_precedente.Hide();
                FinestraPrincipale.BackPanel.Panel_Giorno.button_successivo.Hide();
                FinestraPrincipale.BackPanel.Panel_Giorno.Attributo_txt.Visible = false;
                FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Visible = false;
                FinestraPrincipale.BackPanel.Panel_Giorno.PanelMotore.Visible = false;
                if (FinestraPrincipale.BackPanel.Panel_Giorno.panel_corrente != index)
                {
                    FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Controls.Clear();
                    for (int i = 0; i < textbox.Count; i++)
                    {
                        if (textbox.Count > 0) FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Controls.Add(textbox[i]);
                    }
                    FinestraPrincipale.BackPanel.Panel_Giorno.NewTextbox();
                    if (textbox.Count > 0) textbox[0].Location = new Point(4, 0);
                }
                FinestraPrincipale.BackPanel.Panel_Giorno.panel_corrente = index;
                FinestraPrincipale.BackPanel.Panel_Giorno.panel_clicked = index;
                FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento = new Panel_NewEvento()
                {
                    valore = valore,
                    tipo = Funzioni_utili.GetTipo(this.tipo),
                    attributo = attributo,
                    metodo = metodo,
                    data = data,
                    data_modifica = data_modifica,
                    attributi = attributi,
                    index = index,
                };
                FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento.Resize_NewEvento();
                FinestraPrincipale.BackPanel.Panel_Giorno.Controls.Add(FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento);
                ProprietàGiorno.attributo_selected = false;
                delete.Show();
                panel_tipo.BorderStyle = BorderStyle.Fixed3D;
                panel_tipo.BackColor = Color.LightCyan;

                ResumeLayout();
            }
        }

        private void DeleteShow(object sender, EventArgs e)
        {
            foreach (Pulsante panel in FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale)
            {
                if (panel.bordo == true) return;
            }
            if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento != null) return;
            if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Visible == true) return;
            delete.Visible = true;
        }
        private void DeleteLeave(object sender, EventArgs e)
        {
            foreach (Pulsante panel in FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale)
            {
                if (panel.bordo == true) return;
            }
            if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_NewEvento != null) return;
            if(FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato != null) if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Visible == true) return;
            delete.Visible = false;
        }
        public void ResizeAttributi()
        {
            for (int i = 0; i < textbox.Count; i++)
            {
                textbox[i].Size = new Size((int)(FinestraPrincipale.Finestra.Bounds.Width * 0.35 - 50 - FinestraPrincipale.BackPanel.Panel_Giorno.size_controllo * 2), 20);
                textbox[i].Location = new Point(4 + FinestraPrincipale.BackPanel.Panel_Giorno.size_controllo*2, textbox[0].Location.Y + textbox[0].Height * i);
                Controllo[i].Size = new Size(FinestraPrincipale.BackPanel.Panel_Giorno.size_controllo, FinestraPrincipale.BackPanel.Panel_Giorno.size_controllo);
                Controllo[i].Location = new Point(4, textbox[0].Location.Y + (int)(textbox[0].Height * (i + 0.3)));
                textbox[i].Font = new System.Drawing.Font(BackPanel.font1, (int)(FinestraPrincipale.Finestra.Bounds.Width * 0.005 + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
        }

        private void DeleteEvento(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                foreach (Eventi evento in Input.eventi)
                {
                    if (evento.index == index)
                    {
                        Input.eventi.Remove(evento);
                        evento.Load();
                        DateTime data = new DateTime(evento.GetData()[5], evento.GetData()[4], 1);
                        if (!Savings.months_to_save.Contains(data)) Savings.months_to_save.Add(data);
                        break;
                    }
                }
                FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale.Remove(this);
                lab_tipo.Text = "";
                FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale.Remove(this);
                FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Controls.Remove(this);
                FinestraPrincipale.BackPanel.Panel_Giorno.ClickNull();
                FinestraPrincipale.BackPanel.Panel_Giorno.Resize_Tipi();
                ProprietàGiorno.time_to_save = true;
                if (FinestraPrincipale.BackPanel.Panel_Giorno.panelspeciale.Count == 0)
                {
                    FinestraPrincipale.BackPanel.Panel_Giorno.empty.Visible = true;
                }
                Disposer();
            }
        }

        public void SaveEvento()
        {
            if (!new_evento)
            {
                attributi.Clear();
                foreach (TextBox textbox in textbox)
                {
                    if (textbox.Text != "" && Funzioni_utili.Scremato(textbox.Text) != "") attributi.Add(Funzioni_utili.Scremato(textbox.Text));
                }
            }
            foreach (Eventi evento in Input.eventi)
            {
                if (evento.index == index)
                {
                    if (attributo == "Trasferimento") tipo = Funzioni_utili.GetTipo(tipo);

                    evento.Set_Attributo(this.attributo);
                    evento.SetTipo(this.tipo);
                    evento.index = this.index;
                    evento.SetMetodo(this.metodo);
                    evento.SetValore(this.valore);
                    evento.SetData(this.data);
                    evento.SetData_modifica(this.data_modifica);
                    evento.SetAttributi(attributi);
                    //Console.WriteLine(evento.Info());

                    /*bool match = true;
                    if (attributi.Count != evento.GetAttributi().Count) match = false;
                    for (int i = 0; i < attributi.Count; i++)
                    {
                        if (Funzioni_utili.Scremato(attributi[i]) != evento.GetAttributi()[i]) match = false;
                    }
                    if (evento.validation && match) break;
                    if (evento.validation) break;
                    else
                    {
                        Input.RefreshData();
                        Input.eventi.Remove(evento);
                        Input.eventi.Add(new Eventi() { index = this.index, });
                        Input.eventi[Input.eventi.Count - 1].Set_Attributo(this.attributo);
                        Input.eventi[Input.eventi.Count - 1].SetTipo(this.tipo);
                        Input.eventi[Input.eventi.Count - 1].SetMetodo(this.metodo);
                        Input.eventi[Input.eventi.Count - 1].SetValore(this.valore);
                        Input.eventi[Input.eventi.Count - 1].SetData(this.data);
                        Input.eventi[Input.eventi.Count - 1].SetData_modifica(this.data_modifica);
                        for (int i = 0; i < attributi.Count; i++) { Input.eventi[Input.eventi.Count - 1].AddAttributo(attributi[i]); }
                        break;
                    }*/
                }
                else if(new_evento)
                {
                    if (attributo == "Trasferimento") tipo = Funzioni_utili.GetTipo(tipo);
                    Input.RefreshData();
                    Input.eventi.Add(new Eventi() { index = this.index, });
                    Input.eventi[Input.eventi.Count - 1].SetValore(this.valore);
                    Input.eventi[Input.eventi.Count - 1].SetTipo(this.tipo);
                    Input.eventi[Input.eventi.Count - 1].SetMetodo(this.metodo);
                    Input.eventi[Input.eventi.Count - 1].Set_Attributo(this.attributo);
                    Input.eventi[Input.eventi.Count - 1].SetData(this.data);
                    Input.eventi[Input.eventi.Count - 1].SetData_modifica(this.data_modifica);
                    for (int i = 0; i < attributi.Count; i++) { Console.WriteLine(attributi[i]); Input.eventi[Input.eventi.Count - 1].AddAttributo(attributi[i]); }
                    new_evento = false;
                    break;
                }
            }
            if (Input.eventi.Count() == 0)
            {
                Input.RefreshData();

                if (attributo == "Trasferimento") tipo = Funzioni_utili.GetTipo(tipo);
                Input.eventi.Add(new Eventi() { index = this.index, });
                Input.eventi[Input.eventi.Count - 1].SetValore(this.valore);
                Input.eventi[Input.eventi.Count - 1].Set_Attributo(this.attributo);
                Input.eventi[Input.eventi.Count - 1].SetTipo(this.tipo);
                Input.eventi[Input.eventi.Count - 1].SetMetodo(this.metodo);
                Input.eventi[Input.eventi.Count - 1].SetData(this.data);
                Input.eventi[Input.eventi.Count - 1].SetData_modifica(this.data_modifica);
                for (int i = 0; i < attributi.Count; i++) { Input.eventi[Input.eventi.Count - 1].AddAttributo(attributi[i]); }
                new_evento = false;
            }
        }

        public static List<Pulsante> Order_datacode_datacode_modifica(List<Pulsante> eventi)
        {
            foreach (Pulsante evento in eventi) { evento.datacode = Date.Codifica(evento.data); evento.datacode_modifica = Date.Codifica(evento.data_modifica); }
            eventi = eventi.OrderBy(o => o.datacode).ToList();

            List<List<Pulsante>> superlista = new List<List<Pulsante>>();
            int j = -1;
            if (eventi.Count > 0)
            {
                superlista.Add(new List<Pulsante>());
                superlista[0].Add(eventi[0]);
                j++;
                for (int i = 1; i < eventi.Count; i++)
                {
                    if (eventi[i].datacode != eventi[i - 1].datacode) { superlista.Add(new List<Pulsante>()); j++; }
                    superlista[j].Add(eventi[i]);
                }
            }
            for (int i = 0; i < superlista.Count; i++)
            {
                superlista[i] = superlista[i].OrderBy(o => o.datacode_modifica).ToList();
            }

            List<Pulsante> eventi_out = new List<Pulsante>();
            for (int m = 0; m < j + 1; m++)
            {
                for (int i = 0; i < superlista[m].Count; i++)
                {
                    eventi_out.Add(superlista[m][i]);
                }
            }
            List<Pulsante> eventi_out_byAscendending = new List<Pulsante>();
            for (int i=0; i<eventi_out.Count; i++)
            {
                eventi_out_byAscendending.Add(eventi_out[eventi_out.Count - i -1]);
            }
            return eventi_out_byAscendending;


        }
        public void CheckAllAttributi()
        {
            foreach(TextBox txt in textbox) if(txt.Focused && Funzioni_utili.Scremato(txt.Text)=="") return;
            for (int i = 0; i < textbox.Count -1; i++)
            {
                string stringa = Funzioni_utili.Scremato(textbox[i].Text);
                if (stringa != "")
                {
                    if (Input.all_attributi.Contains(stringa))
                    {
                        Controllo[i].BackgroundImage = FinestraPrincipale.BackPanel.vero;
                        toolTip.SetToolTip(Controllo[i], "Attributo già usato");
                        Controllo[i].Visible = true;
                    }
                    else
                    {
                        Controllo[i].BackgroundImage = FinestraPrincipale.BackPanel.falso;
                        toolTip.SetToolTip(Controllo[i], "Attributo mai usato");
                        Controllo[i].Visible = true;
                    }
                }
            }
        }
        public void CheckAttributi(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down) return;
            string testo = "";
            for (int i = 0; i < textbox.Count; i++) if (textbox[i].Focused)
                {
                    testo = Funzioni_utili.Scremato(textbox[i].Text);
                    FinestraPrincipale.BackPanel.Panel_Giorno.PanelMotore.SetTextbox(testo);
                    if (i == textbox.Count - 1) return;
                    if (testo == "") { Controllo[i].Visible = false; return; }
                    if (Input.all_attributi.Contains(Funzioni_utili.Scremato(textbox[i].Text)))
                    {
                        if (Controllo[i].BackgroundImage == FinestraPrincipale.BackPanel.vero && Controllo[i].Visible) return;
                        else Controllo[i].BackgroundImage = FinestraPrincipale.BackPanel.vero;
                        toolTip.SetToolTip(Controllo[i], "Attributo già usato");
                        Controllo[i].Visible = true;
                    }
                    else
                    {
                        if (Controllo[i].BackgroundImage == FinestraPrincipale.BackPanel.falso && Controllo[i].Visible) return;
                        else Controllo[i].BackgroundImage = FinestraPrincipale.BackPanel.falso;
                        toolTip.SetToolTip(Controllo[i], "Attributo mai usato");
                        Controllo[i].Visible = true;
                    }
                }
        }

    }
}
