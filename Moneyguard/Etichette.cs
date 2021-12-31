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
    public class Etichette : Label
    {
        public int index=-1;
        public static bool to_save = false;
        public static bool disabilitaclick = false;
        public int tipo_metodo = 0;
        public string tipo;
        public Label picture;
        public Label delete;
        public TextBox text_tipo;
        public bool fake = false;
        public bool image = false;
        private bool onechance = false;
        public Timer timerenter;
        ToolTip tooltip;

        private Label Up, Down;
        
        private Label Punto;
        private Label Euro;
        public TextBox Unità;
        public TextBox Centesimi;

        static public bool fakestatico;
        private readonly string errore = "Questo nome è già utilizzato da una seconda tipologia";
        private readonly string errore1 = "Questo nome non è valido";
        public string resource;

        public void Disposer()
        {
            FinestraPrincipale.BackPanel.Focus();
            text_tipo.Dispose();
            delete.BackgroundImage.Dispose();
            delete.Dispose();
            picture.BackgroundImage.Dispose();
            picture.Dispose();
            Unità.Dispose();
            Centesimi.Dispose();
            Punto.Dispose();
            Euro.Dispose();
            Up.BackgroundImage.Dispose();
            Down.BackgroundImage.Dispose();
            Up.Dispose();
            Down.Dispose();
            timerenter.Dispose();
            tooltip.Dispose();
            Dispose();
        }

        public Etichette()
        {
            fakestatico = false;
            disabilitaclick = false;
            BackColor = System.Drawing.Color.AliceBlue;
            BorderStyle = BorderStyle.FixedSingle;
            tooltip = new ToolTip();
            timerenter = new Timer()
            {
                Enabled = true,
                Interval = 10,
            };
            picture = new Label()
            {
                BackgroundImage = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("no_icon"))),
                BackColor = System.Drawing.Color.Transparent,
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
            };
            Controls.Add(picture);
            delete = new Label()
            {
                BackgroundImage = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("Red_X"))),
                BackColor = System.Drawing.Color.Transparent,
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
            };
            Controls.Add(delete);
            delete.MouseEnter += Enter_Delete;
            delete.MouseLeave += Leave_Delete;
            delete.MouseClick += Delete;

            Up = new Label()
            {
                BackgroundImage = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("Up_arrow"))),
                BackColor = System.Drawing.Color.Transparent,
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
            };
            Controls.Add(Up);
            Down = new Label()
            {
                BackgroundImage = new Bitmap((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("Down_arrow"))),
                BackColor = System.Drawing.Color.Transparent,
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch,
            };
            Controls.Add(Down);
            Up.MouseClick += UpClick;
            Down.MouseClick += DownClick;

            text_tipo = new TextBox()
            {
                BorderStyle = BorderStyle.None,
                BackColor = System.Drawing.Color.AliceBlue,
            };
            Controls.Add(text_tipo);
            picture.MouseClick += ClickEvent;
            text_tipo.LostFocus += ModificaTipo;
            text_tipo.KeyDown += Enter;


            Punto = new Label()
            {
                Text = ",",
                AutoSize = true,
                Visible = false,
            };
            Controls.Add(Punto);
            Euro = new Label()
            {
                Text = "\u20AC",
                AutoSize = true,
                Visible = false,
            };
            Controls.Add(Euro);
            Unità = new TextBox()
            {
                Text = "",
                BorderStyle = BorderStyle.None,
                BackColor = System.Drawing.Color.AliceBlue,
                TextAlign = HorizontalAlignment.Right,
                Visible = false,
            };
            Controls.Add(Unità);
            Centesimi = new TextBox()
            {
                Text = "00",
                BorderStyle = BorderStyle.None,
                BackColor = System.Drawing.Color.AliceBlue,
                TextAlign = HorizontalAlignment.Left,
                Visible = false,
            };
            Controls.Add(Centesimi);
            Unità.LostFocus += UnitàLost;
            Centesimi.LostFocus += CentesimiLost;
            tooltip.SetToolTip(Unità, "Importo iniziale");
            tooltip.SetToolTip(Centesimi, "Importo iniziale");
            tooltip.SetToolTip(Punto, "Importo iniziale");
            tooltip.SetToolTip(Euro, "Importo iniziale");

        }
        
        public void ResizeEtichetta()
        {
            if (fake) delete.Hide();
            Up.Size = new Size((int)(Width * 0.03), Height / 3);
            Down.Size = Up.Size;
            Up.Location = new Point(Width - Up.Width * 2, 3);
            Down.Location = new Point(Up.Location.X, Height - 3 - Down.Height);
            picture.Size = new Size(Height-6, Height-6);
            picture.Location = new Point(Up.Location.X - picture.Width - Up.Width , 3);
            Delete_Piccolo();
            text_tipo.Font =  new System.Drawing.Font(BackPanel.font1, (int)(Width * 0.01 + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            if (tipo_metodo == 1)
            {
                text_tipo.Size = new Size(picture.Location.X - (int)(Width * 0.2), Height);
                text_tipo.Location = new Point((int)(Width * 0.2), (Height - text_tipo.Height) / 2);

                Unità.Hide();
                Centesimi.Hide();
                Punto.Hide();
                Euro.Hide();
            }
            else if (tipo_metodo == 2)
            {
                Punto.Font = new System.Drawing.Font(BackPanel.font3, (int)(Width * 0.02 + 6), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Euro.Font = new System.Drawing.Font(BackPanel.font3, (int)(Width * 0.02 + 6), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Unità.Font = new System.Drawing.Font(BackPanel.font3, (int)(Width * 0.02 + 4), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Centesimi.Font = new System.Drawing.Font(BackPanel.font3, (int)(Width * 0.02 + 4), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                Unità.Size = new Size((int)(Width * 0.12), Height / 3);
                Unità.Location = new Point(delete.Location.X + delete.Width * 2, (Height - Unità.Height) / 2);
                Punto.Location = new Point(Unità.Location.X + Unità.Width, Unità.Location.Y - (int)(Height * 0.02) - 2); Punto.Height = Unità.Height;
                Centesimi.Size = new Size((int)(Width * 0.07), Unità.Height);
                Centesimi.Location = new Point(Unità.Location.X + Unità.Width + Punto.Width, Unità.Location.Y);
                Euro.Location = new Point(Centesimi.Location.X + Centesimi.Width, Punto.Location.Y); Euro.Height = Unità.Height;

                text_tipo.Size = new Size(picture.Location.X - (int)(Width * 0.4), Height);
                text_tipo.Location = new Point(Euro.Location.X + Euro.Width + (int)(Width * 0.03), (Height - text_tipo.Height) / 2);

                Unità.Show();
                Centesimi.Show();
                Punto.Show();
                Euro.Show();
            }
        }

        private void ClickEvent(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (disabilitaclick) return;
            if (fake) fakestatico = true;

            VisualModifiche.tipo_metodostatico = tipo_metodo;
            if (tipo_metodo == 1)
            {
                VisualModifiche.tipostatico = tipo;
                VisualModifiche.save = false;
                FinestraPrincipale.BackPanel.Panel_Impostazioni.PanelTipi.Visible = true;
                Update();
                FinestraPrincipale.BackPanel.Panel_Impostazioni.PanelTipi.BringToFront();
            }
            else
            {
                VisualModifiche.tipostatico = tipo;
                VisualModifiche.save = false;
                FinestraPrincipale.BackPanel.Panel_Impostazioni.PanelMetodi.Visible = true;
                Update();
                FinestraPrincipale.BackPanel.Panel_Impostazioni.PanelMetodi.BringToFront();
            }
            disabilitaclick = true;
        }
        public void SetImages()
        {
            if (fake) { text_tipo.Text = "New"; if (onechance == false) { tipo = "New"; onechance = true; } picture.BackgroundImage = picture.BackgroundImage = Funzioni_utili.TakePicture(tipo, 1); Up.Hide(); Down.Hide(); return; }
            if (tipo_metodo == 1) picture.BackgroundImage = Funzioni_utili.TakePicture(tipo, 1);
            else
            {
                picture.BackgroundImage = Funzioni_utili.TakePicture(tipo, 2);
                for (int i = 0; i < Input.metodi.Count; i++) if (Input.metodi[i] == tipo) { Unità.Text = Convert.ToString((int)Input.totali_iniziali[i]); Centesimi.Text = Convert.ToString(Funzioni_utili.GetCentesimi(Input.totali_iniziali[i])); if (Centesimi.Text.Length == 1) Centesimi.Text = "0" + Centesimi.Text; }
            }
            text_tipo.Text = tipo;
            if (Centesimi.Text == "0" || Centesimi.Text == "") Centesimi.Text = "00";
        }

        void ModificaTipo(object sender, EventArgs e)
        {
            if (fake) {  return; }
            if (tipo_metodo == 1) if (text_tipo.Text.Length > 17) { text_tipo.Text = text_tipo.Text.Substring(0, 18); }
            if (tipo_metodo == 2) if (text_tipo.Text.Length > 10) { text_tipo.Text = text_tipo.Text.Substring(0, 11); }
            if (text_tipo.Text == tipo) return;
            if(text_tipo.Text.Length<2) { text_tipo.Text = tipo; MessageBox.Show(errore1); return;}
            if (tipo_metodo == 1)
            {
                foreach (Etichette etichetta in FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list) if (etichetta.tipo == text_tipo.Text) { text_tipo.Text = tipo; MessageBox.Show(errore); return; }
                foreach (Eventi evento in Input.eventi) if (evento.Get_Attributo() == "Introito" || evento.Get_Attributo() == "Spesa") if (evento.GetTipo() == tipo) evento.SetTipo(text_tipo.Text);
                for (int i = 0; i < Input.tipi.Count; i++) if (Input.tipi[i] == tipo) Input.tipi[i] = text_tipo.Text;
            }
            if (tipo_metodo == 2)
            {
                if (tipo_metodo == 2) foreach (Etichette etichetta in FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list) if (etichetta.tipo == text_tipo.Text) { text_tipo.Text = tipo; MessageBox.Show(errore); return; }
                foreach (Eventi evento in Input.eventi)
                {
                    if (evento.Get_Attributo() == "Introito" || evento.Get_Attributo() == "Spesa") if (evento.GetMetodo() == tipo) evento.SetMetodo(text_tipo.Text);
                    if (evento.Get_Attributo() == "Trasferimento") { if (evento.GetTipo() == tipo) evento.SetTipo(text_tipo.Text); if (evento.GetMetodo() == tipo) evento.SetMetodo(text_tipo.Text); }
                }
                if (Unità.Text == "") Unità.Text = "0";
                if (Centesimi.Text == "") Centesimi.Text = "00";
                for (int i = 0; i < Input.metodi.Count; i++) if (Input.metodi[i] == tipo) { Input.metodi[i] = text_tipo.Text;}
            }
            tipo = text_tipo.Text;
            Savings.SaveEvents();
        }
        new void Enter(object sender, KeyEventArgs e)
        {
            timerenter.Tick += CheckTrue;
            if (e.KeyCode == Keys.Oemcomma) e.SuppressKeyPress = true;
            if (e.KeyCode != Keys.Delete && e.KeyCode != Keys.Back && e.KeyCode != Keys.Left && e.KeyCode != Keys.Right)
            {
                if (tipo_metodo == 1) if (text_tipo.Text.Length > 17) e.SuppressKeyPress = true;
                if (tipo_metodo == 2) if (text_tipo.Text.Length > 10) e.SuppressKeyPress = true;
            }
            if (e.KeyCode == Keys.Enter) { if (fake) { New(); } FinestraPrincipale.BackPanel.Panel_Impostazioni.ClickNull(); e.SuppressKeyPress = true; }
        }
        public void New()
        {
            Input.AdjustTotaliIniziali();
            if (FinestraPrincipale.BackPanel.Panel_Impostazioni.True.Visible == false) { return; }
            if (tipo_metodo == 1)
            {
                int max = Input.tipi_sort.Max();
                Input.tipi_sort.Add(max + 1);
                Input.tipi.Add(text_tipo.Text); Input.tipi_icons.Add(resource);
                FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list.Add(new Etichette() { tipo = text_tipo.Text, tipo_metodo = 1, });
                FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi.Controls.Add(FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list[FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list.Count - 1]);
                FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list[FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list.Count - 1].SetImages();
                FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list[FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list.Count -1].ResizeEtichetta();
                FinestraPrincipale.BackPanel.Panel_Impostazioni.ResizeImp();
                ProprietàGiorno.ScrollToBottom(FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi);
            }
            if (tipo_metodo == 2)
            {
                //for (int i = 0; i < Input.totali_iniziali.Count; i++) Console.WriteLine(Input.totali_iniziali[i]);
                //Console.WriteLine();
                //Console.WriteLine(Unità.Text);
                //Console.WriteLine();
                int max = Input.metodi_sort.Max();
                Input.metodi_sort.Add(max + 1);
                Input.metodi.Add(text_tipo.Text); Input.metodi_icons.Add(resource); Input.totali.Add(0); Input.totali_iniziali.Add(Convert.ToInt32(Unità.Text) + Funzioni_utili.SetCentesimi(Centesimi.Text)); 
                FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list.Add(new Etichette() { tipo = text_tipo.Text, tipo_metodo = 2, });
                //for (int i = 0; i < Input.totali_iniziali.Count; i++) Console.WriteLine(Input.totali_iniziali[i]);
                FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list.Count - 1].SetImages();
                FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list.Count - 1].ResizeEtichetta(); 
                FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list.Count - 1].Unità.Text = Convert.ToString((int)(Input.totali_iniziali[Input.totali_iniziali.Count - 1]));
                FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list.Count - 1].Centesimi.Text = Convert.ToString(Funzioni_utili.GetCentesimi(Input.totali_iniziali[Input.totali_iniziali.Count - 1]));
                if (Convert.ToString(Funzioni_utili.GetCentesimi(Input.totali_iniziali[Input.totali_iniziali.Count - 1])) == "0") FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list.Count - 1].Centesimi.Text = "00";
                FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi.Controls.Add(FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list.Count - 1]);
                FinestraPrincipale.BackPanel.Panel_Impostazioni.ResizeImp();
                ProprietàGiorno.ScrollToBottom(FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi);
            }
            FinestraPrincipale.BackPanel.Panel_Impostazioni.ClickNull();
            Savings.SaveEvents();
        }




        private void Enter_Delete(object sender, EventArgs e)
        {
            Delete_Grande();
        }
        private void Leave_Delete(object sender, EventArgs e)
        {
            Delete_Piccolo();
        }
        private void Delete_Grande()
        {
            delete.Size = new Size(Height / 4+2, Height / 4+2);
            delete.Location = new Point(delete.Location.X - 1, delete.Location.Y - 1);
        }
        private void Delete_Piccolo()
        {
            delete.Size = new Size(Height / 4, Height / 4);
            delete.Location = new Point((Height - delete.Height) / 2, (Height - delete.Height) / 2);
        }
        private void Delete(object sender, MouseEventArgs e)
        {
            if (Panel_attributo_scelta.exist) return;
            if(e.Button == MouseButtons.Left)
            {
                if (tipo_metodo == 1) if (FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list.Count == 1) return;
                if (tipo_metodo == 2) if (FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list.Count == 1) return;
                if (tipo_metodo == 1)
                {
                    int occorrenze = 0;
                    foreach (Eventi evento in Input.eventi) if (evento.Get_Attributo() == "Introito" || evento.Get_Attributo() == "Spesa") if (evento.GetTipo() == tipo) occorrenze ++;
                    if(occorrenze == 0)
                    {
                        for (int i = 0; i < Input.tipi.Count; i++) if (Input.tipi[i] == tipo) { Delete_tipi_sort(i); Input.tipi.RemoveAt(i); Input.tipi_icons.RemoveAt(i); break; }
                        foreach (Etichette etichetta in FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list) if (etichetta.tipo == tipo)
                            { FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list.Remove(etichetta); FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi.Controls.Remove(etichetta); break; }
                        FinestraPrincipale.BackPanel.Panel_Impostazioni.ResizeImp();
                        Savings.save_all = true;
                        return;
                    }
                }
                else if (tipo_metodo == 2)
                {
                    int occorrenze = 0;
                    foreach (Eventi evento in Input.eventi)
                    {
                        if (evento.Get_Attributo() == "Introito" || evento.Get_Attributo() == "Spesa") if (evento.GetMetodo() == tipo) occorrenze++;
                        if (evento.Get_Attributo() == "Trasferimento")
                        {
                            if (evento.GetTipo() == tipo) occorrenze++;
                            if (evento.GetMetodo() == tipo) occorrenze++;
                        }
                    }
                    if(occorrenze == 0)
                    {
                        int j = 0;
                        //for (int i = 0; i < Input.totali_iniziali.Count; i++) Console.WriteLine(Input.totali_iniziali[i]);
                        //Console.WriteLine();
                        for (int i = 0; i < Input.metodi.Count; i++) if (Input.metodi[i] == tipo)
                            { Delete_metodi_sort(i); Input.totali_iniziali.RemoveAt(i); Input.metodi_icons.RemoveAt(i); Input.totali.RemoveAt(i); Input.metodi.RemoveAt(i); break; }

                        foreach (Etichette etichetta in FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list) if (etichetta.tipo == Input.metodi[j]) etichetta.SetImages();
                        foreach (Etichette etichetta in FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list) if (etichetta.tipo == tipo)
                            { FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list.Remove(etichetta); FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi.Controls.Remove(etichetta); break; }
                        FinestraPrincipale.BackPanel.Panel_Impostazioni.ResizeImp();
                        Savings.save_all = true;
                        //for (int i = 0; i < Input.totali_iniziali.Count; i++) Console.WriteLine(Input.totali_iniziali[i]);
                        return;
                    }
                }
                Impostazioni.tipo_metodo_delete = tipo_metodo;
                Impostazioni.tipo_delete = tipo;
                FinestraPrincipale.BackPanel.Panel_Impostazioni.PanelDelete = new Panel_attributo_scelta(true);
                FinestraPrincipale.BackPanel.Panel_Impostazioni.Controls.Add(FinestraPrincipale.BackPanel.Panel_Impostazioni.PanelDelete);
                FinestraPrincipale.BackPanel.Panel_Impostazioni.PanelDelete.ResizeForm();
                FinestraPrincipale.BackPanel.Panel_Impostazioni.PanelDelete.BringToFront();
                FinestraPrincipale.BackPanel.Panel_Impostazioni.PanelDelete.Show();
            }
        }

        public static void Delete_tipi_sort(int j)
        {
            int to_remove = Input.tipi_sort[j];
            Input.tipi_sort.RemoveAt(j);
            for (int i = 0; i < Input.tipi_sort.Count; i++) if (Input.tipi_sort[i] > to_remove) Input.tipi_sort[i]--;
        }
        public static void Delete_metodi_sort(int j)
        {
            int to_remove = Input.metodi_sort[j];
            Input.metodi_sort.RemoveAt(j);
            for (int i = 0; i < Input.metodi_sort.Count; i++) if (Input.metodi_sort[i] > to_remove) Input.metodi_sort[i]--;
        }

        void UnitàLost(object sender, EventArgs e)
        {
            if (uint.TryParse(Unità.Text, out uint n)) {}
            else
            {
                Unità.Text = "0";
            }
            for (int i = 0; i < Input.metodi.Count; i++) if (Input.metodi[i] == tipo) { Input.totali_iniziali[i] = Convert.ToDouble(n) + Funzioni_utili.SetCentesimi(Centesimi.Text); }
            if (!fake) Savings.SaveEvents();
        }
        void CentesimiLost(object sender, EventArgs e)
        {
            if (uint.TryParse(Unità.Text, out uint n) && uint.TryParse(Centesimi.Text, out uint m)) {}
            else
            {
                Centesimi.Text = "0"; m = 0;
            }
            //Centesimi.Text = Convert.ToString((int)(Funzioni_utili.SetCentesimi(Centesimi.Text) * 100));
            for (int i = 0; i < Input.metodi.Count; i++) if (Input.metodi[i] == tipo) { Input.totali_iniziali[i] = Convert.ToDouble(n) + Funzioni_utili.SetCentesimi(Centesimi.Text); Console.WriteLine(Input.totali_iniziali[i]); }
            if (Centesimi.Text.Length == 1) Centesimi.Text += "0";
            if(!fake) Savings.SaveEvents();
        }

        public void CheckTrue(object sender, EventArgs e)
        {
            if (Visible == false) return;
            timerenter.Tick -= CheckTrue;
            if (image == false) return;
            FinestraPrincipale.BackPanel.Panel_Impostazioni.True.Show();
            if (tipo_metodo == 1) foreach (string tipo in Input.tipi) if (Funzioni_utili.Scremato(tipo) == Funzioni_utili.Scremato(text_tipo.Text)) FinestraPrincipale.BackPanel.Panel_Impostazioni.True.Hide();
            if (tipo_metodo == 2) foreach (string tipo in Input.metodi) if (Funzioni_utili.Scremato(tipo) == Funzioni_utili.Scremato(text_tipo.Text)) FinestraPrincipale.BackPanel.Panel_Impostazioni.True.Hide();
        }
        private void UpClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                int j = 0;
                if (tipo_metodo == 1)
                {
                    for (int i = 0; i < FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list.Count; i++) if (FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list[i].text_tipo.Text == text_tipo.Text) j = i;
                    if (j == 0) return;
                    string j_text = FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list[j].text_tipo.Text;
                    string jminus_text = FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list[j-1].text_tipo.Text;
                    string j_tipo = Input.tipi[j];
                    string jminus_tipo = Input.tipi[j - 1];
                    string j_tipoicon = Input.tipi_icons[j];
                    string jplus_tipoicon = Input.tipi_icons[j - 1];
                    //Input.tipi[j] = jminus_tipo;
                    //Input.tipi[j-1] = j_tipo;
                    //Input.tipi_icons[j] = jplus_tipoicon;
                    //Input.tipi_icons[j - 1] = j_tipoicon;
                    int temp = Input.tipi_sort[j];
                    Input.tipi_sort[j] = Input.tipi_sort[j - 1];
                    Input.tipi_sort[j - 1] = temp;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list[j].text_tipo.Text = jminus_text;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list[j].tipo = jminus_text;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list[j - 1].text_tipo.Text = j_text;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list[j - 1].tipo = j_text;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list[j].SetImages();
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list[j - 1].SetImages();
                }
                else if (tipo_metodo == 2)
                {
                    for (int i = 0; i < FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list.Count; i++) if (FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[i].text_tipo.Text == text_tipo.Text) j = i;
                    if (j == 0) return;
                    string j_text = FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j].text_tipo.Text;
                    string jminus_text = FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j - 1].text_tipo.Text;
                    string j_metodi = Input.metodi[j];
                    string jminus_metodi = Input.metodi[j - 1];
                    string j_metodiicon = Input.metodi_icons[j];
                    string jplus_metodiicon = Input.metodi_icons[j - 1];
                    string j_unità = FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j].Unità.Text;
                    string jminus_unità = FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j - 1].Unità.Text;
                    string j_centesimi = FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j].Centesimi.Text;
                    string jminus_centesimi = FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j - 1].Centesimi.Text;
                    //Input.metodi[j] = jminus_metodi;
                    //Input.metodi[j - 1] = j_metodi;
                    //Input.metodi_icons[j] = jplus_metodiicon;
                    //Input.metodi_icons[j - 1] = j_metodiicon;
                    int temp = Input.metodi_sort[j];
                    Input.metodi_sort[j] = Input.metodi_sort[j - 1];
                    Input.metodi_sort[j - 1] = temp;
                    double j_totali = Input.totali[j];
                    double j_totali_iniziali = Input.totali_iniziali[j];
                    double jminus_totali = Input.totali[j - 1];
                    double jminus_totali_iniziali = Input.totali_iniziali[j - 1];
                    //Input.totali[j] = jminus_totali;
                    //Input.totali[j - 1] = j_totali;
                    //Input.totali_iniziali[j] = jminus_totali_iniziali;
                    //Input.totali_iniziali[j - 1] = j_totali_iniziali;

                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j].text_tipo.Text = jminus_text;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j].Unità.Text = jminus_unità;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j].Centesimi.Text = jminus_centesimi;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j].tipo = jminus_text;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j - 1].text_tipo.Text = j_text;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j - 1].Unità.Text = j_unità;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j - 1].Centesimi.Text = j_centesimi;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j - 1].tipo = j_text;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j].SetImages();
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j - 1].SetImages();
                }
                to_save = true;
            }
        }
        private void DownClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int j = 0;
                if (tipo_metodo == 1)
                {
                    for (int i = 0; i < FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list.Count; i++) if (FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list[i].text_tipo.Text == text_tipo.Text) j = i;
                    if (j == FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list.Count-1) return;
                    string j_text = FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list[j].text_tipo.Text;
                    string jplus_text = FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list[j + 1].text_tipo.Text;
                    string j_tipo = Input.tipi[j];
                    string jplus_tipo = Input.tipi[j + 1];
                    string j_tipoicon = Input.tipi_icons[j];
                    string jplus_tipoicon = Input.tipi_icons[j + 1];
                    //Input.tipi[j] = jplus_tipo;
                    //Input.tipi[j + 1] = j_tipo;
                    //Input.tipi_icons[j] = jplus_tipoicon;
                    //Input.tipi_icons[j + 1] = j_tipoicon;
                    int temp = Input.tipi_sort[j];
                    Input.tipi_sort[j] = Input.tipi_sort[j + 1];
                    Input.tipi_sort[j + 1] = temp;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list[j].text_tipo.Text = jplus_text;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list[j].tipo = jplus_text;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list[j + 1].text_tipo.Text = j_text;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list[j + 1].tipo = j_text;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list[j].SetImages();
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Tipi_list[j + 1].SetImages();
                }
                else if (tipo_metodo == 2)
                {
                    for (int i = 0; i < FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list.Count; i++) if (FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[i].text_tipo.Text == text_tipo.Text) j = i;
                    if (j == FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list.Count - 1) return;
                    string j_text = FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j].text_tipo.Text;
                    string jplus_text = FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j + 1].text_tipo.Text;
                    string j_metodi = Input.metodi[j];
                    string jplus_metodi = Input.metodi[j + 1];
                    string j_metodiicon = Input.metodi_icons[j];
                    string jplus_metodiicon = Input.metodi_icons[j + 1];
                    string j_unità = FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j].Unità.Text;
                    string jplus_unità = FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j + 1].Unità.Text;
                    string j_centesimi = FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j].Centesimi.Text;
                    string jplus_centesimi = FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j + 1].Centesimi.Text;
                    //Input.metodi[j] = jplus_metodi;
                    //Input.metodi[j + 1] = j_metodi;
                    //Input.metodi_icons[j] = jplus_metodiicon;
                    //Input.metodi_icons[j + 1] = j_metodiicon;
                    int temp = Input.metodi_sort[j];
                    Input.metodi_sort[j] = Input.metodi_sort[j + 1];
                    Input.metodi_sort[j + 1] = temp;
                    double j_totali = Input.totali[j];
                    double j_totali_iniziali = Input.totali_iniziali[j];
                    double jplus_totali = Input.totali[j + 1];
                    double jplus_totali_iniziali = Input.totali_iniziali[j + 1];
                    //Input.totali[j] = jplus_totali;
                    //Input.totali[j + 1] = j_totali;
                    //Input.totali_iniziali[j] = jplus_totali_iniziali;
                    //Input.totali_iniziali[j + 1] = j_totali_iniziali;

                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j].text_tipo.Text = jplus_text;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j].Unità.Text = jplus_unità;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j].Centesimi.Text = jplus_centesimi;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j].tipo = jplus_text;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j + 1].text_tipo.Text = j_text;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j + 1].Unità.Text = j_unità;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j + 1].Centesimi.Text = j_centesimi;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j + 1].tipo = j_text;
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j].SetImages();
                    FinestraPrincipale.BackPanel.Panel_Impostazioni.Metodi_list[j + 1].SetImages();
                }
                to_save = true;
            }
        }
    }
}
