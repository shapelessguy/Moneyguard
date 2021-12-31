using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1;

namespace Moneyguard
{
    public class Visualizzazione_Scaletta : Panel
    {
        public Timer acquisizione_dati;
        public DataGridView Lista_Scaletta;
        DataTable table;
        Panel Visualizzazione_Attributi;
        Panel Totali;
        public ListBox Lista_Attributi;
        Label Attributi_txt;
        Label label_selezione, label_totale;
        Label more1, more2;
        ListBox info_list, info_list_tot;
        ListBox info_list_complete, info_list_tot_complete;
        bool datasort = true;
        bool valoresort = true;
        int dim_tipologia = 10, dim_metodo = 8, dim_valore = 11;
        private ToolTip toolTip;
        string strTip_precedente = "";
        bool init = false;
        Timer timer;
        readonly Bitmap arrow_up;
        readonly Bitmap arrow_down;

        public void Disposer()
        {
            acquisizione_dati.Dispose();
            table.Dispose();
            Visualizzazione_Attributi.Dispose();
            Totali.Dispose();
            Lista_Attributi.Dispose();
            Attributi_txt.Dispose();
            //Lista_Scaletta.Dispose();
            more1.BackgroundImage.Dispose();
            more2.BackgroundImage.Dispose();
            more1.Dispose();
            more2.Dispose();
            toolTip.Hide(this);
            toolTip.Dispose();
            timer.Tick -= Timer_Mouse;
            timer.Dispose(); 
            //Dispose();
        }
        public Visualizzazione_Scaletta()
        {
            Visible = true;
            BorderStyle = BorderStyle.Fixed3D;
            BackColor = System.Drawing.Color.FloralWhite;
            AutoScroll = true;
            toolTip = new ToolTip();
            Click += ClickNull;
            acquisizione_dati = new Timer()
            {
                Enabled = true,
                Interval = 20,
            };
            Visualizzazione_Attributi = new Panel()
            {
                BackColor = Color.AliceBlue,
                AutoScroll = true,
                BorderStyle = BorderStyle.Fixed3D,
            };
            Controls.Add(Visualizzazione_Attributi);
            Lista_Attributi = new ListBox()
            {
                Font = new System.Drawing.Font(BackPanel.font1, 14, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))), BorderStyle = BorderStyle.None,
                BackColor = Color.AliceBlue,
            };
            Visualizzazione_Attributi.Controls.Add(Lista_Attributi);
            Lista_Attributi.Click += OnAttributiClick;

            Attributi_txt = new Label()
            {
                Text = "   Attributi:",
                Font = new System.Drawing.Font(BackPanel.font4, 16, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                BorderStyle = BorderStyle.None,
            };
            Visualizzazione_Attributi.Controls.Add(Attributi_txt);
            Visualizzazione_Attributi.Click += AttributiSelectionNull;

            Totali = new Panel()
            {
                BackColor = Color.SkyBlue,
                AutoScroll = true,
                BorderStyle = BorderStyle.Fixed3D,
            };
            Controls.Add(Totali);
            Totali.Click += TotaliSelectionNull_selection;


            Attributi_txt.Click += AttributiSelectionNull;
            table = new DataTable();
            Lista_Scaletta = new DataGridView()
            {
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowDrop = true,
                ReadOnly = true,
                AllowUserToResizeRows = false,
                ShowCellToolTips = false,
                BackgroundColor = Color.AliceBlue,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Visible = false,
            };
            Controls.Add(Lista_Scaletta);

            table.Columns.Add("datacode", typeof(int));
            table.Columns.Add("datacode_modifica", typeof(int));
            table.Columns.Add("ora", typeof(int));
            table.Columns.Add("valore", typeof(double));
            table.Columns.Add("index", typeof(int));
            table.Columns.Add("Data", typeof(string));
            table.Columns.Add("Categoria", typeof(string));
            table.Columns.Add("Importo", typeof(string));
            table.Columns.Add("Tipologia", typeof(string));
            table.Columns.Add("Metodo", typeof(string));
            table.Columns.Add("Orario", typeof(string));
            table.Columns.Add("Modifica", typeof(string));
            Lista_Scaletta.DataSource = table;
            Lista_Scaletta.ColumnHeadersVisible = false;
            Lista_Scaletta.ColumnHeadersHeight = 30;


            Lista_Scaletta.SelectionChanged += Selezione_Eventi;
            Lista_Scaletta.RowTemplate.Height = 27;
            Lista_Scaletta.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            Lista_Scaletta.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            Lista_Scaletta.AdvancedCellBorderStyle.Top= DataGridViewAdvancedCellBorderStyle.None;
            Lista_Scaletta.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;

            Lista_Scaletta.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            Lista_Scaletta.ColumnHeadersDefaultCellStyle.BackColor = Color.Turquoise;
            Lista_Scaletta.DefaultCellStyle.BackColor = Color.AliceBlue;
            Lista_Scaletta.DefaultCellStyle.SelectionBackColor = Color.PaleTurquoise;
            Lista_Scaletta.DefaultCellStyle.SelectionForeColor = Color.Black;
            Lista_Scaletta.EnableHeadersVisualStyles = false;

            Lista_Scaletta.Click += ClickEvento;
            Lista_Scaletta.DoubleClick += DoubleClickEvento;


            label_selezione = new Label()
            {
                Text = " Selezione:",
                Font = new System.Drawing.Font(BackPanel.font4, 16, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                BorderStyle = BorderStyle.None,
            };
            Totali.Controls.Add(label_selezione);
            label_selezione.Click += AttributiSelectionNull;
            label_selezione.Click += TotaliSelectionNull;
            label_totale = new Label()
            {
                Text = " Totale:",
                Font = new System.Drawing.Font(BackPanel.font4, 16, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                BorderStyle = BorderStyle.None,
            };
            Totali.Controls.Add(label_totale);
            label_totale.Click += AttributiSelectionNull;
            label_totale.Click += TotaliSelectionNull;
            info_list = new ListBox()
            {
                Font = new System.Drawing.Font(BackPanel.font1, 14, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                BorderStyle = BorderStyle.None,
                BackColor = Color.SkyBlue,
            };
            Totali.Controls.Add(info_list);
            info_list.Click += AttributiSelectionNull;
            info_list.Click += TotaliSelectionNull_selection;
            info_list.Click += ClickEmpty_info_list;
            info_list_tot = new ListBox()
            {
                Font = new System.Drawing.Font(BackPanel.font1, 14, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                BorderStyle = BorderStyle.None,
                BackColor = Color.SkyBlue,
            };
            Totali.Controls.Add(info_list_tot);
            info_list_tot.Click += AttributiSelectionNull;
            info_list_tot.Click += TotaliSelectionNull_tot;
            info_list_tot.Click += ClickEmpty_info_list;

            arrow_up = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("arrow_up")));
            arrow_down = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("arrow_down")));
            more1 = new Label()
            {
                BorderStyle = BorderStyle.None,
                BackgroundImage = arrow_down,
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
            };
            Totali.Controls.Add(more1);
            more1.Click += More1Click;
            more2 = new Label()
            {
                BorderStyle = BorderStyle.None,
                BackgroundImage = arrow_down,
                BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
            };
            Totali.Controls.Add(more2);
            more2.Click += More2Click;
            more1.Size = new Size(35, 22);
            more2.Size = new Size(35, 22);
            label_selezione.Size = new Size(150, 30);
            label_totale.Size = new Size(150, 30);
            more1.Location = new Point(label_selezione.Location.X + label_selezione.Width + 20, label_selezione.Location.Y + 2);


            info_list_complete = new ListBox()
            {
                Font = new System.Drawing.Font(BackPanel.font1, 14, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                BorderStyle = BorderStyle.None,
                BackColor = Color.SkyBlue,
                Visible = false,
            };
            Totali.Controls.Add(info_list_complete);
            info_list_complete.Click += AttributiSelectionNull;
            info_list_complete.Click += TotaliSelectionNull_selection;
            info_list_complete.Click += ClickEmpty_info_list;
            info_list_tot_complete = new ListBox()
            {
                Font = new System.Drawing.Font(BackPanel.font1, 14, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                BorderStyle = BorderStyle.None,
                BackColor = Color.SkyBlue,
                Visible = false,
            };
            Totali.Controls.Add(info_list_tot_complete);
            info_list_tot_complete.Click += TotaliSelectionNull_tot;
            info_list_tot_complete.Click += ClickEmpty_info_list;

            timer = new System.Windows.Forms.Timer()
            {
                Enabled = true,
                Interval = 10,
            };
            timer.Tick += Timer_Mouse;

            Lista_Scaletta.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(OnRowHeaderMouseClick);
            Lista_Scaletta.Click += HideMotore;
            table.Rows.Add(0, 0, 0, 0, 0, "", "", "", "", "", "", "");
            init = true;

            ResizeForm();
        }

        void OnRowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                if (datasort) { Lista_Scaletta.Sort(Lista_Scaletta.Columns[0], System.ComponentModel.ListSortDirection.Ascending); datasort = false; }
                else { Lista_Scaletta.Sort(Lista_Scaletta.Columns[0], System.ComponentModel.ListSortDirection.Descending); datasort = true; }
            }
            if (e.ColumnIndex == 7)
            {
                if (valoresort) { Lista_Scaletta.Sort(Lista_Scaletta.Columns[3], System.ComponentModel.ListSortDirection.Ascending); valoresort = false; }
                else { Lista_Scaletta.Sort(Lista_Scaletta.Columns[3], System.ComponentModel.ListSortDirection.Descending); valoresort = true; }
            }
            if (e.ColumnIndex == 11)
            {
                if (valoresort) { Lista_Scaletta.Sort(Lista_Scaletta.Columns[1], System.ComponentModel.ListSortDirection.Ascending); valoresort = false; }
                else { Lista_Scaletta.Sort(Lista_Scaletta.Columns[1], System.ComponentModel.ListSortDirection.Descending); valoresort = true; }
            }

        }
        public void ResizeForm()
        {
            Lista_Scaletta.Size = new Size(Width - 304, Height - 4);
            Visualizzazione_Attributi.Location = new Point(Lista_Scaletta.Width, Lista_Scaletta.Location.Y);
            Visualizzazione_Attributi.Size = new Size(265, (int)(Height * 0.3));
            Attributi_txt.Size = new Size(235, 30);
            Lista_Attributi.Location = new Point(20, 30);
            Lista_Attributi.Size = new Size(Visualizzazione_Attributi.Width - Lista_Attributi.Location.X -10 - 30, Visualizzazione_Attributi.Height - Lista_Attributi.Location.Y - 5);
            Totali.Location = new Point(Lista_Scaletta.Width, Lista_Scaletta.Location.Y + Visualizzazione_Attributi.Height);
            Totali.Size = new Size(Visualizzazione_Attributi.Width, Height - Visualizzazione_Attributi.Location.Y - Visualizzazione_Attributi.Height - 4);

            info_list.Location = new Point(3, 30);
            info_list.Size = new Size(258, (int)(Height * 0.3));
            info_list_complete.Location = info_list.Location;
            info_list_complete.Size = info_list.Size;

            label_totale.Location = new Point(0, info_list.Location.Y + info_list.Height);
            more2.Location = new Point(label_totale.Location.X + label_totale.Width + 20, label_totale.Location.Y + 2);
            info_list_tot.Location = new Point(3, label_totale.Location.Y + label_totale.Height + 5);
            info_list_tot.Size = new Size(info_list.Width, Height - info_list_tot.Location.Y - Totali.Location.Y - 15);
            info_list_tot_complete.Location = info_list_tot.Location;
            info_list_tot_complete.Size = info_list_tot.Size;

            int aus = Width; if (aus >= 1700) aus = 1700;
            Lista_Scaletta.Font = new System.Drawing.Font(BackPanel.font4, (int)(aus * 0.007 + 2), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Lista_Scaletta.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font(BackPanel.font4, (int)(aus * 0.011), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Lista_Scaletta.ColumnHeadersHeight = (int)(aus * 0.04);

            if (Lista_Scaletta.Columns.Count != 0)
            {
                int dim1 = 105;
                int dim2 = 100;
                int dim3 = dim_valore * 8;
                int dim4 = dim_tipologia * 8;
                int dim5 = dim_metodo * 8;
                int dim6 = 100;
                int dim7 = 155;
                int dim_somma = dim1 + dim2 + dim3 + dim4 + dim5 + dim6 + dim7;

                if (Lista_Scaletta.Width - 5 > dim_somma)
                {
                    dim1 = (int)(dim1 * (Lista_Scaletta.Width - 5) / dim_somma) - 3;
                    dim2 = (int)(dim2 * (Lista_Scaletta.Width - 5) / dim_somma) - 3;
                    dim3 = (int)(dim3 * (Lista_Scaletta.Width - 5) / dim_somma) - 3;
                    dim4 = (int)(dim4 * (Lista_Scaletta.Width - 5) / dim_somma) - 2;
                    dim5 = (int)(dim5 * (Lista_Scaletta.Width - 5) / dim_somma) - 2;
                    dim6 = (int)(dim6 * (Lista_Scaletta.Width - 5) / dim_somma) - 2;
                    dim7 = (int)(dim7 * (Lista_Scaletta.Width - 5) / dim_somma) - 2;
                }

                Lista_Scaletta.Columns[0].Visible = false;
                Lista_Scaletta.Columns[1].Visible = false;
                Lista_Scaletta.Columns[2].Visible = false;
                Lista_Scaletta.Columns[3].Visible = false;
                Lista_Scaletta.Columns[4].Visible = false;
                Lista_Scaletta.Columns[5].Width = dim1;
                Lista_Scaletta.Columns[6].Width = dim2;
                Lista_Scaletta.Columns[7].Width = dim3;
                Lista_Scaletta.Columns[8].Width = dim4;
                Lista_Scaletta.Columns[9].Width = dim5;
                Lista_Scaletta.Columns[10].Width = dim6;
                Lista_Scaletta.Columns[11].Width = dim7;

                Lista_Scaletta.Columns[5].SortMode = DataGridViewColumnSortMode.Programmatic;
                Lista_Scaletta.Columns[7].SortMode = DataGridViewColumnSortMode.Programmatic;
                Lista_Scaletta.Columns[11].SortMode = DataGridViewColumnSortMode.Programmatic;


                Lista_Scaletta.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                Lista_Scaletta.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Lista_Scaletta.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                Lista_Scaletta.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                Lista_Scaletta.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Lista_Scaletta.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Lista_Scaletta.Columns[11].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;

                Lista_Scaletta.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Lista_Scaletta.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Lista_Scaletta.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Lista_Scaletta.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                Lista_Scaletta.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                Lista_Scaletta.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Lista_Scaletta.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }
        }
        void ClickNull(object sender, EventArgs e)
        {
            RicercaClickNull();
        }
        void AttributiSelectionNull(object sender, EventArgs e)
        {
            Lista_Attributi.ClearSelected();
        }
        void TotaliSelectionNull(object sender, EventArgs e)
        {
            info_list.ClearSelected();
            info_list_complete.ClearSelected();
            info_list_tot.ClearSelected();
            info_list_tot_complete.ClearSelected();
        }
        void TotaliSelectionNull_selection(object sender, EventArgs e)
        {
            info_list_tot.ClearSelected();
            info_list_tot_complete.ClearSelected();
        }
        void TotaliSelectionNull_tot(object sender, EventArgs e)
        {
            info_list.ClearSelected();
            info_list_complete.ClearSelected();
        }
        void SelectionNull(object sender, EventArgs e)
        {
            Lista_Scaletta.ClearSelection();
            Lista_Attributi.ClearSelected();
            Lista_Attributi.Items.Clear();
        }
        public void RicercaClickNull()
        {
            FinestraPrincipale.BackPanel.Panel_Ricerca.ClickNull();
        }
        public void ClickNull()
        {
            Lista_Scaletta.ClearSelection();
            Lista_Attributi.ClearSelected();
            Lista_Attributi.Items.Clear();
        }
        void More1Click(object sender, EventArgs e)
        {
            info_list.ClearSelected();
            info_list_complete.ClearSelected();
            if (info_list_complete.Visible) { info_list_complete.Visible = false; info_list.Visible = true; more1.BackgroundImage = arrow_down; }
            else { info_list_complete.Visible = true; info_list.Visible = false; more1.BackgroundImage = arrow_up; }
        }
        void More2Click(object sender, EventArgs e)
        {
            info_list_tot.ClearSelected();
            info_list_tot_complete.ClearSelected();
            if (info_list_tot_complete.Visible) { info_list_tot_complete.Visible = false; info_list_tot.Visible = true; more2.BackgroundImage = arrow_down;}
            else { info_list_tot_complete.Visible = true; info_list_tot.Visible = false; more2.BackgroundImage = arrow_up; }
        }
        void HideMotore(object sender, EventArgs e)
        {
            FinestraPrincipale.BackPanel.Panel_Ricerca.HideMotore();
        }
        void ClickEvento(object sender, EventArgs e)
        {
            // if (e.RowIndex < 0) ClickNull();
        }
        void DoubleClickEvento(object sender, EventArgs e)
        {
            int[] data = Date.Decodifica(Convert.ToUInt32(Lista_Scaletta.SelectedRows[0].Cells[0].Value.ToString()));
            DateTime dataTime = new DateTime(data[5], data[4], data[3]);
            FinestraPrincipale.BackPanel.Panel_Ricerca.GoToDay(dataTime);
        }

        public void AcquisizioneDati(object sender, EventArgs e)
        {
            acquisizione_dati.Tick -= AcquisizioneDati;
            Lista_Scaletta.SuspendLayout();
            table.Clear();

            Lista_Scaletta.DataSource = null;
            int j = 0;
            foreach (Eventi evento in FinestraPrincipale.BackPanel.Panel_Ricerca.eventi_filtrati)
            {

                if (FinestraPrincipale.BackPanel.Panel_Ricerca.primo_filtro == 1) if (evento.Get_Attributo() != "Introito") continue;
                if (FinestraPrincipale.BackPanel.Panel_Ricerca.primo_filtro == 2) if (evento.Get_Attributo() != "Spesa") continue;
                if (FinestraPrincipale.BackPanel.Panel_Ricerca.primo_filtro == 3) if (evento.Get_Attributo() != "Trasferimento") continue;

                string evento_tipo = "", evento_metodo = "";
                if (evento.Get_Attributo() == "Trasferimento") { for (int i = 0; i < Input.metodi.Count; i++) if (evento.GetTipo() == Funzioni_utili.Scremato(Input.metodi[i])) evento_tipo = Input.metodi[i]; }
                else for (int i = 0; i < Input.tipi.Count; i++) if (evento.GetTipo() == Funzioni_utili.Scremato(Input.tipi[i])) evento_tipo = Input.tipi[i];
                for (int i = 0; i < Input.metodi.Count; i++) if (evento.GetMetodo() == Funzioni_utili.Scremato(Input.metodi[i])) evento_metodo = Input.metodi[i];

                string stringa_tipo; if (evento.Get_Attributo() == "Trasferimento") stringa_tipo = evento_tipo + "\u2192"; else stringa_tipo = evento_tipo + "    ";
                string stringa_metodo; if (evento.Get_Attributo() == "Trasferimento") stringa_metodo = "\u2192" + evento_metodo; else stringa_metodo = "    " + evento_metodo;
                string stringa_valore = Convert.ToDecimal(String.Format("{0:0.00}", Convert.ToDecimal(evento.GetValore()))).ToString() + "\u20AC";
                string stringa_attributo = evento.Get_Attributo();
                if(evento.Get_Attributo() == "Trasferimento") stringa_attributo = "Trasf.";

                table.Rows.Add(evento.GetDatacode(), evento.GetDatacode_modifica(), evento.GetData()[2]*60 + evento.GetData()[1], evento.GetValore(), j, Date.ShowDate(evento.GetData()),stringa_attributo, stringa_valore, stringa_tipo, stringa_metodo, Date.ShowHour(evento.GetData()), Date.ShowDate(evento.GetData_modifica()) +"  "+ Date.ShowHour(evento.GetData_modifica()));
               

                if (stringa_valore.Length > dim_valore) dim_valore = stringa_valore.Length;
                if (stringa_tipo.Length > dim_tipologia) dim_tipologia = stringa_tipo.Length;
                if (stringa_metodo.Length > dim_metodo) dim_metodo = stringa_metodo.Length;
                j++;
            }

            Lista_Scaletta.DataSource = table;
            Lista_Scaletta.Sort(Lista_Scaletta.Columns[0], System.ComponentModel.ListSortDirection.Descending);
            if(Lista_Scaletta.Rows.Count != 0) Lista_Scaletta.FirstDisplayedScrollingRowIndex = 0;
            ResizeForm();
            Lista_Scaletta.ResumeLayout(true);

            Lista_Scaletta.Visible = true;
            FinestraPrincipale.BackPanel.Panel_Ricerca.Scaletta.Visible = true;

            info_list_complete.Visible = false; info_list.Visible = true; more1.BackgroundImage = arrow_down;
            info_list_tot_complete.Visible = false; info_list_tot.Visible = true; more2.BackgroundImage = arrow_down;
            RicercaClickNull();
            SetTotali();
        }

        Point Lastpos;
        bool fermo;
        private void Timer_Mouse(object sender, EventArgs e)
        {
            if (init)
            {
                ResizeForm();
                Lista_Scaletta.ColumnHeadersVisible = true;
                
                init = false;
                FinestraPrincipale.BackPanel.Panel_Ricerca.RicercaEventi();
            }

            if (!Lista_Attributi.Visible) return;
            if (Lastpos == Cursor.Position && !fermo)
            {
                int esito = 0;
                esito = OnAttributiMouseMove();
                if(esito==0) esito = OninfolistMouseMove();
                if (esito == 0) { try { toolTip.Hide(this); } catch (Exception) { Console.WriteLine("Visualizzazione_Scaletta"); }  strTip_precedente = ""; }
                fermo = true;
            }
            if (Lastpos != Cursor.Position) { fermo = false; }
            Lastpos = Cursor.Position;
        }
        private void OnAttributiClick(object sender, EventArgs e)
        {
            TotaliSelectionNull(sender, e);
            int nIdx = Lista_Attributi.IndexFromPoint(Lista_Attributi.PointToClient(Cursor.Position));
            if (nIdx < 0)
            {
                AttributiSelectionNull(sender, e);
            }
        }
        private int OnAttributiMouseMove()
        {
            string strTip; int nIdx=0; bool active = false;
            strTip = "";

            try { nIdx = Lista_Attributi.IndexFromPoint(Lista_Attributi.PointToClient(Cursor.Position)); } catch (Exception) { Console.WriteLine("Visualizzazione_Scaletta"); return 0; }
            if ((nIdx >= 0) && (nIdx < Lista_Attributi.Items.Count))
            {
                active = true;
                strTip = Lista_Attributi.Items[nIdx].ToString();
                if (strTip != strTip_precedente)
                {
                    toolTip.Show(strTip, this, new Point(Visualizzazione_Attributi.Location.X + 2, Lista_Attributi.Location.Y + Lista_Attributi.Height - 13));
                    strTip_precedente = strTip;
                }
            }
            if (!active) return 0; else return 1;
        }
        private int OninfolistMouseMove()
        {
            int esito = 0;
            int nIdx = -1;
            try { nIdx = Lista_Attributi.IndexFromPoint(Lista_Attributi.PointToClient(Cursor.Position)); } catch (Exception) { Console.WriteLine("Visualizzazione_Scaletta"); return 0; }
            if (info_list.Visible) nIdx = info_list.IndexFromPoint(info_list.PointToClient(Cursor.Position)); if (nIdx >= 0) { esito = Selection_infolist(nIdx, info_list); return 1; }
            if (info_list_tot.Visible && nIdx<0) nIdx = info_list_tot.IndexFromPoint(info_list_tot.PointToClient(Cursor.Position)); if (nIdx >= 0) {esito = Selection_infolist(nIdx, info_list_tot); return 1; }
            if (info_list_complete.Visible && nIdx < 0) nIdx = info_list_complete.IndexFromPoint(info_list_complete.PointToClient(Cursor.Position)); if (nIdx >= 0) {esito = Selection_infolist(nIdx, info_list_complete); return 1; }
            if (info_list_tot_complete.Visible && nIdx < 0) nIdx = info_list_tot_complete.IndexFromPoint(info_list_tot_complete.PointToClient(Cursor.Position)); if (nIdx >= 0) {esito = Selection_infolist(nIdx, info_list_tot_complete); return 1; }
            return esito;
        }
        private int Selection_infolist(int nIdx, ListBox list)
        {
            bool active = false;
            string strTip = "";
            nIdx = list.IndexFromPoint(list.PointToClient(Cursor.Position));
            if (nIdx < 0) return 0;
            if (nIdx < list.Items.Count)
            {
                if (list == info_list)
                {
                }
                if (list == info_list_complete)
                {
                    if (nIdx == 1 && (string)list.Items[nIdx] != "") strTip = "Introito medio giornaliero basato sugli eventi selezionati";
                    if (nIdx == 2 && (string)list.Items[nIdx] != "") strTip = "Introito medio mensile basato sugli eventi selezionati";
                    if (nIdx == 3 && (string)list.Items[nIdx] != "") strTip = "Introito medio annuale basato sugli eventi selezionati";
                    if (nIdx == 4 && (string)list.Items[nIdx] != "") strTip = "Introito minimo tra gli eventi selezionati";
                    if (nIdx == 5 && (string)list.Items[nIdx] != "") strTip = "Introito massimo tra gli eventi selezionati";

                    if (nIdx == 7 && (string)list.Items[nIdx] != "") strTip = "Spesa media giornaliera basata sugli eventi selezionati";
                    if (nIdx == 8 && (string)list.Items[nIdx] != "") strTip = "Spesa media mensile basata sugli eventi selezionati";
                    if (nIdx == 9 && (string)list.Items[nIdx] != "") strTip = "Spesa media annuale basata sugli eventi selezionati";
                    if (nIdx == 10 && (string)list.Items[nIdx] != "") strTip = "Spesa minima tra gli eventi selezionati";
                    if (nIdx == 11 && (string)list.Items[nIdx] != "") strTip = "Spesa massima tra gli eventi selezionati";

                    if (nIdx == 13 && (string)list.Items[nIdx] != "") strTip = "Trasferimento medio giornaliero basato sugli eventi selezionati";
                    if (nIdx == 14 && (string)list.Items[nIdx] != "") strTip = "Trasferimento medio mensile basato sugli eventi selezionati";
                    if (nIdx == 15 && (string)list.Items[nIdx] != "") strTip = "Trasferimento medio annuale basato sugli eventi selezionati";
                    if (nIdx == 16 && (string)list.Items[nIdx] != "") strTip = "Trasferimento minimo tra gli eventi selezionati";
                    if (nIdx == 17 && (string)list.Items[nIdx] != "") strTip = "Trasferimento massimo tra gli eventi selezionati";
                }
                if (list == info_list_tot)
                {
                }
                if (list == info_list_tot_complete)
                {
                    if (nIdx == 1 && (string)list.Items[nIdx] != "") strTip = "Introito medio giornaliero basato sugli eventi visualizzati";
                    if (nIdx == 2 && (string)list.Items[nIdx] != "") strTip = "Introito medio mensile basato sugli eventi visualizzati";
                    if (nIdx == 3 && (string)list.Items[nIdx] != "") strTip = "Introito medio annuale basato sugli eventi visualizzati";
                    if (nIdx == 4 && (string)list.Items[nIdx] != "") strTip = "Introito minimo tra gli eventi visualizzati";
                    if (nIdx == 5 && (string)list.Items[nIdx] != "") strTip = "Introito massimo tra gli eventi visualizzati";

                    if (nIdx == 7 && (string)list.Items[nIdx] != "") strTip = "Spesa media giornaliera basata sugli eventi visualizzati";
                    if (nIdx == 8 && (string)list.Items[nIdx] != "") strTip = "Spesa media mensile basata sugli eventi visualizzati";
                    if (nIdx == 9 && (string)list.Items[nIdx] != "") strTip = "Spesa media annuale basata sugli eventi visualizzati";
                    if (nIdx == 10 && (string)list.Items[nIdx] != "") strTip = "Spesa minima tra gli eventi visualizzati";
                    if (nIdx == 11 && (string)list.Items[nIdx] != "") strTip = "Spesa massima tra gli eventi visualizzati";

                    if (nIdx == 13 && (string)list.Items[nIdx] != "") strTip = "Trasferimento medio giornaliero basato sugli eventi visualizzati";
                    if (nIdx == 14 && (string)list.Items[nIdx] != "") strTip = "Trasferimento medio mensile basato sugli eventi visualizzati";
                    if (nIdx == 15 && (string)list.Items[nIdx] != "") strTip = "Trasferimento medio annuale basato sugli eventi visualizzati";
                    if (nIdx == 16 && (string)list.Items[nIdx] != "") strTip = "Trasferimento minimo tra gli eventi visualizzati";
                    if (nIdx == 17 && (string)list.Items[nIdx] != "") strTip = "Trasferimento massimo tra gli eventi visualizzati";
                }
                active = true;
                if (strTip != strTip_precedente)
                {
                    int textWidth = TextRenderer.MeasureText(strTip, SystemFonts.CaptionFont).Width;
                    toolTip.Show(strTip, this, new Point(Visualizzazione_Attributi.Location.X - textWidth + 30, PointToClient(Cursor.Position).Y));
                    strTip_precedente = strTip;
                }
            }
            if (!active) return 0; else return 1;
        }
        public void Aggiorna_Attributi()
        {
            if (Lista_Scaletta.SelectedRows.Count == 0) { info_list.Items.Clear(); info_list_complete.Items.Clear(); return; }
            if (Lista_Scaletta.SelectedRows.Count == 1)
            {
                Lista_Attributi.Items.Clear();
                int index = (int)Lista_Scaletta.SelectedCells[4].Value;
                List<Eventi> list_given_attribute = new List<Eventi>();
                for (int j = 0; j < FinestraPrincipale.BackPanel.Panel_Ricerca.eventi_filtrati.Count; j++)
                {
                    if (PanelRicerca.attributo != "All" && PanelRicerca.attributo != "")
                    {
                        if (FinestraPrincipale.BackPanel.Panel_Ricerca.eventi_filtrati[j].Get_Attributo() == PanelRicerca.attributo) list_given_attribute.Add(FinestraPrincipale.BackPanel.Panel_Ricerca.eventi_filtrati[j]);
                    }
                    else list_given_attribute.Add(FinestraPrincipale.BackPanel.Panel_Ricerca.eventi_filtrati[j]);

                }
                for (int j = 0; j < list_given_attribute.Count; j++) if (j == index) foreach (string stringa in list_given_attribute[j].GetAttributi()) Lista_Attributi.Items.Add(stringa);
            }
            else { Lista_Attributi.Items.Clear(); }
        }
        private void Selezione_Eventi(object sender, EventArgs e)
        {
            Aggiorna_Attributi();
            info_list.Items.Clear();
            info_list_complete.Items.Clear();
            double introito_max = 0, introito_min = 999999999, spesa_max = 0, spesa_min = 999999999, trasf_max = 0, trasf_min = 999999999;
            double totale_introito = 0, totale_spesa = 0, totale_trasferimento = 0;
            int datacode1 = 0, datacode2 = 0;
            int intr = 0, spes = 0, trasf = 0, i = 0;
            foreach (DataGridViewRow row in Lista_Scaletta.SelectedRows)
            {
                if (i == 0) { datacode1 = (int)row.Cells[0].Value; }
                if ((string)row.Cells[6].Value == "Introito")
                {
                    intr++;
                    totale_introito += (double)row.Cells[3].Value;
                    if (introito_max < (double)row.Cells[3].Value) introito_max = (double)row.Cells[3].Value;
                    if (introito_min > (double)row.Cells[3].Value) introito_min = (double)row.Cells[3].Value;
                }
                if ((string)row.Cells[6].Value == "Spesa")
                {
                    spes++;
                    totale_spesa += (double)row.Cells[3].Value;
                    if (spesa_max < (double)row.Cells[3].Value) spesa_max = (double)row.Cells[3].Value;
                    if (spesa_min > (double)row.Cells[3].Value) spesa_min = (double)row.Cells[3].Value;
                }
                if ((string)row.Cells[6].Value == "Trasf.")
                {
                    trasf++;
                    totale_trasferimento += (double)row.Cells[3].Value;
                    if (trasf_max < (double)row.Cells[3].Value) trasf_max = (double)row.Cells[3].Value;
                    if (trasf_min > (double)row.Cells[3].Value) trasf_min = (double)row.Cells[3].Value;
                }
                if (datacode1 > (int)row.Cells[0].Value) datacode1 = (int)row.Cells[0].Value;
                if (datacode2 < (int)row.Cells[0].Value) datacode2 = (int)row.Cells[0].Value;
                i++;
            }

            string periodo = Date.Periodo_DataData(Date.Decodifica((uint)datacode1), Date.Decodifica((uint)datacode2));
            double conta_giorni;
            int[] valore = Date.Periodo_int_DataData(Date.Decodifica((uint)datacode1), Date.Decodifica((uint)datacode2));
            if (valore[0] == 0) conta_giorni = valore[1] * 365 + valore[2] * 30;
            else if (valore[0] == 1) conta_giorni = valore[1] * 30 + valore[2];
            else if (valore[0] == 2) conta_giorni = valore[1] + valore[2] / 24;
            else conta_giorni = 1;

            info_list.Items.Add("  Eventi selezionati: " + Lista_Scaletta.SelectedRows.Count);
            if (periodo != "") info_list.Items.Add("  Periodo: " + periodo); else info_list.Items.Add("");
            if (intr != 0) info_list.Items.Add("  Introito:  " + Funzioni_utili.FormatoStandard(totale_introito) + "\u20AC");
            if (spes != 0) info_list.Items.Add("  Spesa:  " + Funzioni_utili.FormatoStandard(totale_spesa) + "\u20AC");
            if (trasf != 0) info_list.Items.Add("  Trasferimento:  " + Funzioni_utili.FormatoStandard(totale_trasferimento) + "\u20AC");

            if (intr != 0)
            {
                info_list_complete.Items.Add("  Introito \u2192");
                info_list_complete.Items.Add("           giornaliero:  " + Funzioni_utili.FormatoStandard(totale_introito / conta_giorni) + "\u20AC");
                info_list_complete.Items.Add("           mensile:  " + Funzioni_utili.FormatoStandard(totale_introito / conta_giorni * 30) + "\u20AC");
                info_list_complete.Items.Add("           annuale:  " + Funzioni_utili.FormatoStandard(totale_introito / conta_giorni * 365) + "\u20AC");
                info_list_complete.Items.Add("           minimo:  " + Funzioni_utili.FormatoStandard(introito_min) + "\u20AC");
                info_list_complete.Items.Add("           massimo:  " + Funzioni_utili.FormatoStandard(introito_max) + "\u20AC");
            }
            if (spes != 0)
            {
                info_list_complete.Items.Add("  Spesa \u2192");
                info_list_complete.Items.Add("           giornaliera:  " + Funzioni_utili.FormatoStandard(totale_spesa / conta_giorni) + "\u20AC");
                info_list_complete.Items.Add("           mensile:  " + Funzioni_utili.FormatoStandard(totale_spesa / conta_giorni * 30) + "\u20AC");
                info_list_complete.Items.Add("           annuale:  " + Funzioni_utili.FormatoStandard(totale_spesa / conta_giorni * 365) + "\u20AC");
                info_list_complete.Items.Add("           minimo:  " + Funzioni_utili.FormatoStandard(spesa_min) + "\u20AC");
                info_list_complete.Items.Add("           massimo:  " + Funzioni_utili.FormatoStandard(spesa_max) + "\u20AC");
            }
            if (trasf != 0)
            {
                info_list_complete.Items.Add("  Trasferimento \u2192");
                info_list_complete.Items.Add("           giornaliero:  " + Funzioni_utili.FormatoStandard(totale_trasferimento / conta_giorni) + "\u20AC");
                info_list_complete.Items.Add("           mensile:  " + Funzioni_utili.FormatoStandard(totale_trasferimento / conta_giorni * 30) + "\u20AC");
                info_list_complete.Items.Add("           annuale:  " + Funzioni_utili.FormatoStandard(totale_trasferimento / conta_giorni * 365) + "\u20AC");
                info_list_complete.Items.Add("           minimo:  " + Funzioni_utili.FormatoStandard(trasf_min) + "\u20AC");
                info_list_complete.Items.Add("           massimo:  " + Funzioni_utili.FormatoStandard(trasf_max) + "\u20AC");
            }


        }
        private void SetTotali()
        {
            info_list_tot.Items.Clear();
            info_list_tot_complete.Items.Clear();
            if (Lista_Scaletta.Rows.Count == 0) return;
            double introito_max = 0, introito_min = 999999999, spesa_max = 0, spesa_min = 999999999, trasf_max = 0, trasf_min = 999999999;
            double totale_introito = 0, totale_spesa = 0, totale_trasferimento = 0;
            int datacode1 = 0, datacode2 = 0;
            int intr = 0, spes = 0, trasf = 0, i=0;
            foreach (DataGridViewRow row in Lista_Scaletta.Rows)
            {
                if (i == 0) { datacode1 = (int)row.Cells[0].Value; }
                if ((string)row.Cells[6].Value == "Introito")
                {
                    intr++;
                    totale_introito += (double)row.Cells[3].Value;
                    if (introito_max < (double)row.Cells[3].Value) introito_max = (double)row.Cells[3].Value;
                    if (introito_min > (double)row.Cells[3].Value) introito_min = (double)row.Cells[3].Value;
                }
                if ((string)row.Cells[6].Value == "Spesa")
                {
                    spes++;
                    totale_spesa += (double)row.Cells[3].Value;
                    if (spesa_max < (double)row.Cells[3].Value) spesa_max = (double)row.Cells[3].Value;
                    if (spesa_min > (double)row.Cells[3].Value) spesa_min = (double)row.Cells[3].Value;
                }
                if ((string)row.Cells[6].Value == "Trasf.")
                {
                    trasf++;
                    totale_trasferimento += (double)row.Cells[3].Value;
                    if (trasf_max < (double)row.Cells[3].Value) trasf_max = (double)row.Cells[3].Value;
                    if (trasf_min > (double)row.Cells[3].Value) trasf_min = (double)row.Cells[3].Value;
                }
                
                if (datacode1 > (int)row.Cells[0].Value) datacode1 = (int)row.Cells[0].Value;
                if (datacode2 < (int)row.Cells[0].Value) datacode2 = (int)row.Cells[0].Value;
                i++;
            }
            string periodo = Date.Periodo_DataData(Date.Decodifica((uint)datacode1), Date.Decodifica((uint)datacode2));
            double conta_giorni;
            int[] valore = Date.Periodo_int_DataData(Date.Decodifica((uint)datacode1), Date.Decodifica((uint)datacode2));
            if (valore[0] == 0) conta_giorni = valore[1] * 365 + valore[2] * 30;
            else if (valore[0] == 1) conta_giorni = valore[1] * 30 + valore[2];
            else if (valore[0] == 2) conta_giorni = valore[1] + valore[2] / 24;
            else conta_giorni = 1;

            info_list_tot.Items.Add("  Numero eventi: " + Lista_Scaletta.Rows.Count);
            info_list_tot.Items.Add("  Periodo: " + periodo);
            if (intr != 0) info_list_tot.Items.Add("  Introito:  " + Funzioni_utili.FormatoStandard(totale_introito) + "\u20AC");
            if (spes != 0) info_list_tot.Items.Add("  Spesa:  " + Funzioni_utili.FormatoStandard(totale_spesa) + "\u20AC");
            if (trasf != 0) info_list_tot.Items.Add("  Trasferimento:  " + Funzioni_utili.FormatoStandard(totale_trasferimento) + "\u20AC");

            if (intr != 0)
            {
                info_list_tot_complete.Items.Add("  Introito \u2192");
                info_list_tot_complete.Items.Add("           giornaliero:  " + Funzioni_utili.FormatoStandard(totale_introito / conta_giorni) + "\u20AC");
                info_list_tot_complete.Items.Add("           mensile:  " + Funzioni_utili.FormatoStandard(totale_introito / conta_giorni * 30) + "\u20AC");
                info_list_tot_complete.Items.Add("           annuale:  " + Funzioni_utili.FormatoStandard(totale_introito / conta_giorni * 365) + "\u20AC");
                info_list_tot_complete.Items.Add("           minimo:  " + Funzioni_utili.FormatoStandard(introito_min) + "\u20AC");
                info_list_tot_complete.Items.Add("           massimo:  " + Funzioni_utili.FormatoStandard(introito_max) + "\u20AC");
            }
            if (spes != 0)
            {
                info_list_tot_complete.Items.Add("  Spesa \u2192");
                info_list_tot_complete.Items.Add("           giornaliera:  " + Funzioni_utili.FormatoStandard(totale_spesa / conta_giorni) + "\u20AC");
                info_list_tot_complete.Items.Add("           mensile:  " + Funzioni_utili.FormatoStandard(totale_spesa / conta_giorni * 30) + "\u20AC");
                info_list_tot_complete.Items.Add("           annuale:  " + Funzioni_utili.FormatoStandard(totale_spesa / conta_giorni * 365) + "\u20AC");
                info_list_tot_complete.Items.Add("           minimo:  " + Funzioni_utili.FormatoStandard(spesa_min) + "\u20AC");
                info_list_tot_complete.Items.Add("           massimo:  " + Funzioni_utili.FormatoStandard(spesa_max) + "\u20AC");
            }
            if (trasf != 0)
            {
                info_list_tot_complete.Items.Add("  Trasferimento \u2192");
                info_list_tot_complete.Items.Add("           giornaliero:  " + Funzioni_utili.FormatoStandard(totale_trasferimento / conta_giorni) + "\u20AC");
                info_list_tot_complete.Items.Add("           mensile:  " + Funzioni_utili.FormatoStandard(totale_trasferimento / conta_giorni * 30) + "\u20AC");
                info_list_tot_complete.Items.Add("           annuale:  " + Funzioni_utili.FormatoStandard(totale_trasferimento / conta_giorni * 365) + "\u20AC");
                info_list_tot_complete.Items.Add("           minimo:  " + Funzioni_utili.FormatoStandard(trasf_min) + "\u20AC");
                info_list_tot_complete.Items.Add("           massimo:  " + Funzioni_utili.FormatoStandard(trasf_max) + "\u20AC");
            }
        }
        private void ClickEmpty_info_list(object sender, EventArgs e)
        {
            int nIdx =-1;
            if ( info_list.Visible) nIdx = info_list.IndexFromPoint(info_list.PointToClient(Cursor.Position));
            if (nIdx < 0 && info_list_tot.Visible) nIdx = info_list_tot.IndexFromPoint(info_list_tot.PointToClient(Cursor.Position));
            if (nIdx < 0 && info_list_complete.Visible) nIdx = info_list_complete.IndexFromPoint(info_list_complete.PointToClient(Cursor.Position));
            if (nIdx < 0 && info_list_tot_complete.Visible) nIdx = info_list_tot_complete.IndexFromPoint(info_list_tot_complete.PointToClient(Cursor.Position));
            if (nIdx < 0) TotaliSelectionNull(sender, e);
        }
        
    }
}
