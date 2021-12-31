using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1;

namespace Moneyguard
{
    public class MotoreRicerca : Panel
    {
        Label Attributi_Testo, AllAttributi_Testo;
        ToolTip toolTip = new ToolTip();
        public ListBox Attributi;
        public ListBox AllAttributi;
        private List<string> Lista_Attributi = new List<string>();
        private List<string> Lista_AllAttributi = new List<string>();
        System.Windows.Forms.Timer wait_attributi, wait_allattributi, timer;
        readonly int tipi;
        readonly bool double_win;
        static public bool underline = false;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem modifica_attributo;

        private bool attributo = false;
        string tipo = " ";
        public string parziale = " ";

        int index = -1;

        public void Disposer()
        {
            toolTip.Dispose();
            timer.Dispose();
            wait_allattributi.Dispose();
            if (wait_attributi!= null) wait_attributi.Dispose();
            Dispose();
            underline = false;
        }
        public MotoreRicerca(bool double_win, int tipi)
        {
            this.tipi = tipi;
            this.double_win = double_win;
            if(double_win) BackColor = Color.AntiqueWhite;
            else BackColor = Color.Gray;
            BorderStyle = BorderStyle.FixedSingle;
            Visible = false;

            if (double_win)
            {
                Attributi_Testo = new Label()
                {
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                    AutoSize = true,
                    Text = "Attributi tipologia corrente:",
                };
                Controls.Add(Attributi_Testo);
                wait_attributi = new System.Windows.Forms.Timer()
                {
                    Enabled = true,
                    Interval = 20,
                };
                Attributi = new ListBox()
                {
                    Font = new System.Drawing.Font(BackPanel.font1, (int)(Width * 0.08), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                };
                Attributi.MouseUp += AttributiClick;
            }


            contextMenuStrip = new ContextMenuStrip
            {
                ImageScalingSize = new System.Drawing.Size(24, 24),
                Size = new System.Drawing.Size(141, 34)
            };
            modifica_attributo = new ToolStripMenuItem
            {
                Name = "eliminaToolStripMenuItem",
                Size = new System.Drawing.Size(140, 30),
                Text = "Modifica"
            };
            contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { modifica_attributo });
            modifica_attributo.Click += new System.EventHandler(Modifica_attributo_Click);
            ContextMenuStrip = contextMenuStrip;

            AllAttributi_Testo = new Label()
            {
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                AutoSize = true,
            };
            if (tipi==1) AllAttributi_Testo.Text = "Tutte le tipologie:"; else if(tipi==2) AllAttributi_Testo.Text = "Tutti gli attributi:"; else AllAttributi_Testo.Text = "Tutti i metodi:";
            Controls.Add(AllAttributi_Testo);
            wait_allattributi = new System.Windows.Forms.Timer()
            {
                Enabled = true,
                Interval = 20,
            };
            AllAttributi = new ListBox()
            {
                Font = new System.Drawing.Font(BackPanel.font1, (int)(Width * 0.06), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
            };
            Controls.Add(Attributi);
            timer = new System.Windows.Forms.Timer()
            {
                Enabled = true,
                Interval = 10,
            };
            timer.Tick += Timer_Mouse;

            Click += ClickNull;
            Controls.Add(AllAttributi);
            AllAttributi.MouseUp += AllAttributiClick;
            ResizeForm();
        }
        void Modifica_attributo_Click(object sender, EventArgs e)
        {
            Point contexlocation = new Point(Cursor.Position.X - contextMenuStrip.PointToClient(Cursor.Position).X, Cursor.Position.Y - contextMenuStrip.PointToClient(Cursor.Position).Y);
            int index = AllAttributi.IndexFromPoint(AllAttributi.PointToClient(contexlocation));
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                Change_Attributo finestra = new Change_Attributo(AllAttributi.Items[index].ToString());
                finestra.Location = new Point(FinestraPrincipale.Finestra.Location.X + (FinestraPrincipale.Finestra.Width - finestra.Width)/2, FinestraPrincipale.Finestra.Location.Y + (FinestraPrincipale.Finestra.Height - finestra.Height) / 2);
            }
            else if (Attributi.IndexFromPoint(Attributi.PointToClient(contexlocation)) != System.Windows.Forms.ListBox.NoMatches)
            {
                index = Attributi.IndexFromPoint(Attributi.PointToClient(contexlocation));
                Change_Attributo finestra = new Change_Attributo(Attributi.Items[index].ToString());
                finestra.Location = new Point(FinestraPrincipale.Finestra.Location.X + (FinestraPrincipale.Finestra.Width - finestra.Width) / 2, FinestraPrincipale.Finestra.Location.Y + (FinestraPrincipale.Finestra.Height - finestra.Height) / 2);
            }
        }

        public void ResizeForm()
        {
            if (double_win)
            {
                Attributi_Testo.Font = new System.Drawing.Font(BackPanel.font1, (int)(Width * 0.02 + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                Attributi_Testo.Location = new System.Drawing.Point(10, 10);
                ResizeAttributi();
                AllAttributi_Testo.Location = new System.Drawing.Point(10, Attributi_Testo.Location.Y + Attributi_Testo.Height + Height / 3 + 20);
            }
            else
            {
                AllAttributi_Testo.Location = new System.Drawing.Point(10, 10);
            }
            AllAttributi_Testo.Font = new System.Drawing.Font(BackPanel.font1, (int)(Width * 0.02 + 10), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ResizeAllAttributi();
        }
        private void ResizeAttributi()
        {
            Attributi.Location = new Point(Attributi_Testo.Location.X, Attributi_Testo.Location.Y + Attributi_Testo.Height + 10);
            Attributi.Size = new Size(Width - Attributi_Testo.Location.X * 2, Height / 3);
        }
        private void ResizeAllAttributi()
        {
            AllAttributi.Location = new Point(AllAttributi_Testo.Location.X, AllAttributi_Testo.Location.Y + AllAttributi_Testo.Height + 10);
            if (double_win) AllAttributi.Size = new Size(Width - AllAttributi_Testo.Location.X * 2, (int)(Height * 0.55));
            else AllAttributi.Size = new Size(Width - AllAttributi_Testo.Location.X * 2, (int)(Height * 0.93));
        }
        
        public void RefreshForm(string tipo, bool attributo, bool visible)
        {
            tipo = Funzioni_utili.Scremato(tipo);
            underline = false;
            index = -1;
            if (visible && Visible == false) { Visible = true; BringToFront(); }
            if (this.attributo == attributo && this.tipo == tipo) return;
            this.tipo = tipo;
            this.attributo = attributo;
            Thread FindAllAttributi = new Thread(Find_AllAttributi);
            FindAllAttributi.Start();
        }
        public void Reload()
        {
            Thread FindAllAttributi = new Thread(Find_AllAttributi);
            FindAllAttributi.Start();
            Thread Find;
            if (double_win) { Find = new Thread(Find_Attributi); Find.Start(); }
        }
        public void SetTextbox(string text)
        {
            underline = false;
            if(double_win) Attributi.SelectedItem = null;
            AllAttributi.SelectedItem = null;
            index = -1;
            text = Funzioni_utili.Scremato(text);
            if (parziale == text) { return; }
            parziale = Funzioni_utili.Scremato(text);
            Thread Find;
            if (double_win) { Find = new Thread(Find_Attributi); Find.Start(); }
            Thread Find_All = new Thread(Find_AllAttributi);
            Find_All.Start();
        }
        public string GetText()
        {
            underline = false;
            index = -1;
            if (double_win)
            {
                if (Attributi.SelectedItem != null) return Convert.ToString(Attributi.SelectedItem);
                else return Convert.ToString(AllAttributi.SelectedItem);
            }
            else return Convert.ToString(AllAttributi.SelectedItem);
        }
        public void GoUp()
        {
            if (double_win)
            {
                if (Attributi.Items.Count + AllAttributi.Items.Count == 0) return;
                underline = true;
                if (index != 0) if (index == -1) index = 0; else index--;
                if (index >= Attributi.Items.Count) { AllAttributi.SelectedIndex = index - Attributi.Items.Count; Attributi.SelectedItem = null; } else { Attributi.SelectedIndex = index; AllAttributi.SelectedItem = null; }
            }
            else
            {
                if (AllAttributi.Items.Count == 0) return;
                underline = true;
                if (index != 0) if (index == -1) index = 0; else index--;
                 AllAttributi.SelectedIndex = index;
            }
        }
        public void GoDown()
        {
            if (double_win)
            {
                if (Attributi.Items.Count + AllAttributi.Items.Count == 0) return;
                underline = true;
                if (index != Attributi.Items.Count + AllAttributi.Items.Count - 1) index++;
                if (index >= Attributi.Items.Count) { AllAttributi.SelectedIndex = index - Attributi.Items.Count; Attributi.SelectedItem = null; } else { Attributi.SelectedIndex = index; AllAttributi.SelectedItem = null; }
            }
            else
            {
                if (AllAttributi.Items.Count == 0) return;
                underline = true;
                if (index != AllAttributi.Items.Count - 1) index++;
                AllAttributi.SelectedIndex = index;
            }
        }
        public void HideMotore()
        {
            toolTip.Hide(this);
            Visible = false;
            underline = false;
            index = -1;
            parziale = " ";
        }
        private void ClickNull(object sender, EventArgs e)
        {
            ClickNull();
        } 
        private void ClickNull()
        {
            underline = false;
            index = -1;
            if(double_win) Attributi.SelectedItem = null;
            AllAttributi.SelectedItem = null;
            if (FinestraPrincipale.BackPanel.Panel_Giorno != null)
            {
                if (FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Visible)
                {
                    FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Controls.OfType<TextBox>().ElementAt(FinestraPrincipale.BackPanel.Panel_Giorno.current_textbox).Focus();
                    FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Controls.OfType<TextBox>().ElementAt(FinestraPrincipale.BackPanel.Panel_Giorno.current_textbox).Select(FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Controls.OfType<TextBox>().ElementAt(FinestraPrincipale.BackPanel.Panel_Giorno.current_textbox).Text.Length, 0);
                }
                if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.Visible)
                {
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.AttributoPanel.Controls.OfType<TextBox>().ElementAt(FinestraPrincipale.BackPanel.Panel_Giorno.current_textbox).Focus();
                    FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.AttributoPanel.Controls.OfType<TextBox>().ElementAt(FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.AttributoPanel.current_textbox).Select(FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.AttributoPanel.Controls.OfType<TextBox>().ElementAt(FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.AttributoPanel.current_textbox).Text.Length, 0);
                }
            }
            else if (FinestraPrincipale.BackPanel.Panel_Ricerca != null)
            {
                if (PanelAttributi.current_index == 0)
                {
                    FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione.Controls.OfType<TextBox>().ElementAt(FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione.current_textbox).Focus();
                    FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione.Controls.OfType<TextBox>().ElementAt(FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione.current_textbox).Select(FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione.Controls.OfType<TextBox>().ElementAt(FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione.current_textbox).Text.Length, 0);
                }
                if (PanelAttributi.current_index == 1)
                {
                    FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione_Attributi.Controls.OfType<TextBox>().ElementAt(FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione_Attributi.current_textbox).Focus();
                    FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione_Attributi.Controls.OfType<TextBox>().ElementAt(FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione_Attributi.current_textbox).Select(FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione_Attributi.Controls.OfType<TextBox>().ElementAt(FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione_Attributi.current_textbox).Text.Length, 0);
                }
                if (PanelAttributi.current_index == 2)
                {
                    FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione.Controls.OfType<TextBox>().ElementAt(FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione.current_textbox).Focus();
                    FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione.Controls.OfType<TextBox>().ElementAt(FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione.current_textbox).Select(FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione.Controls.OfType<TextBox>().ElementAt(FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione.current_textbox).Text.Length, 0);
                }
                if (PanelAttributi.current_index == 3)
                {
                    FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione_Attributi.Controls.OfType<TextBox>().ElementAt(FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione_Attributi.current_textbox).Focus();
                    FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione_Attributi.Controls.OfType<TextBox>().ElementAt(FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione_Attributi.current_textbox).Select(FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione_Attributi.Controls.OfType<TextBox>().ElementAt(FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione_Attributi.current_textbox).Text.Length, 0);
                }
                if (PanelAttributi.current_index == 4)
                {
                    FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione_Metodi.Controls.OfType<TextBox>().ElementAt(FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione_Metodi.current_textbox).Focus();
                    FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione_Metodi.Controls.OfType<TextBox>().ElementAt(FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione_Metodi.current_textbox).Select(FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione_Metodi.Controls.OfType<TextBox>().ElementAt(FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione_Metodi.current_textbox).Text.Length, 0);
                }
                if (PanelAttributi.current_index == 5)
                {
                    FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione_Metodi.Controls.OfType<TextBox>().ElementAt(FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione_Metodi.current_textbox).Focus();
                    FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione_Metodi.Controls.OfType<TextBox>().ElementAt(FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione_Metodi.current_textbox).Select(FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione_Metodi.Controls.OfType<TextBox>().ElementAt(FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione_Metodi.current_textbox).Text.Length, 0);
                }
            }
        }

        private void Find_AllAttributi()
        {
            Lista_AllAttributi = new List<string>();
            try
            {
                if (tipi == 1) { foreach (string stringa in Input.tipi) if (Funzioni_utili.Scremato(stringa).Contains(parziale)) Lista_AllAttributi.Add(stringa); }
                else if (tipi == 2) {foreach (string stringa in Input.all_attributi) if (stringa.Contains(parziale)) Lista_AllAttributi.Add(stringa); }
                else foreach (string stringa in Input.metodi) if (Funzioni_utili.Scremato(stringa).Contains(parziale)) Lista_AllAttributi.Add(stringa);
            }
            catch (Exception) { Console.WriteLine("Errore Attributi"); }

            wait_allattributi.Tick += WaitAllAttributi;
        }
        private void WaitAllAttributi(object sender, EventArgs e)
        {
            wait_allattributi.Tick -= WaitAllAttributi;
            AllAttributi.SuspendLayout();
            AllAttributi.Items.Clear();
            try { foreach (string stringa in Lista_AllAttributi) AllAttributi.Items.Add(stringa); } catch (Exception) { Console.WriteLine("Errore Attributi2"); }
            AllAttributi.ResumeLayout();
        }
        private void Find_Attributi()
        {
            if (attributo == false)
            {
                Lista_Attributi = new List<string>();
                for (int i = 0; i < Input.tipi.Count; i++) if (tipo == Funzioni_utili.Scremato(Input.tipi[i])) foreach(string stringa in Input.Liste_attributi_tipi[i]) if(stringa.Contains(parziale)) Lista_Attributi.Add(stringa); 
            }
            else
            {
                Lista_Attributi = new List<string>();
                foreach (string stringa in Input.Lista_trasferimento) if (stringa.Contains(parziale)) Lista_Attributi.Add(stringa);
            }
            wait_attributi.Tick += WaitAttributi;
        }
        private void WaitAttributi(object sender, EventArgs e)
        {
            wait_attributi.Tick -= WaitAttributi;
            Attributi.SuspendLayout();
            for (int i = Attributi.Items.Count - 1; i >= 0; i--) Attributi.Items.RemoveAt(i);
            try { foreach (string stringa in Lista_Attributi) Attributi.Items.Add(stringa); } catch (Exception) { Console.WriteLine("Errore Attributi3"); }
            Attributi.ResumeLayout();
        }

        private void AttributiClick(object sender, MouseEventArgs e)
        {
            if (contextMenuStrip.Visible) return;
            int index = Attributi.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                if(FinestraPrincipale.BackPanel.Panel_Giorno != null)
                {
                    if (FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Visible)
                    {
                        int i = 0;
                        foreach (TextBox txt in FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Controls.OfType<TextBox>()) { if (i == FinestraPrincipale.BackPanel.Panel_Giorno.current_textbox) txt.Text = Attributi.Items[index].ToString(); i++; }
                        FinestraPrincipale.BackPanel.Panel_Giorno.EnterAtt();
                    }
                    if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.Visible)
                    {
                        int i = 0;
                        foreach (TextBox txt in FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.AttributoPanel.Controls.OfType<TextBox>()) { if (i == FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.AttributoPanel.current_textbox) txt.Text = Attributi.Items[index].ToString(); i++; }
                        FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.AttributoPanel.EnterAtt();
                    }
                }
            }
            else ClickNull();
            Attributi.SelectedItem = null;
        }
        private void AllAttributiClick(object sender, MouseEventArgs e)
        {
            if (contextMenuStrip.Visible) return;
            int index = AllAttributi.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                if (FinestraPrincipale.BackPanel.Panel_Giorno != null)
                {
                    if (FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Visible)
                    {
                        int i = 0;
                        foreach (TextBox txt in FinestraPrincipale.BackPanel.Panel_Giorno.AttributoPanel.Controls.OfType<TextBox>()) { if (i == FinestraPrincipale.BackPanel.Panel_Giorno.current_textbox) txt.Text = AllAttributi.Items[index].ToString(); i++; }
                        FinestraPrincipale.BackPanel.Panel_Giorno.EnterAtt();
                    }
                    if (FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.Visible)
                    {
                        int i = 0;
                        foreach (TextBox txt in FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.AttributoPanel.Controls.OfType<TextBox>()) { if (i == FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.AttributoPanel.current_textbox) txt.Text = AllAttributi.Items[index].ToString(); i++; }
                        FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.AttributoPanel.EnterAtt();
                    }
                }
                else if (FinestraPrincipale.BackPanel.Panel_Ricerca != null)
                {
                    if (PanelAttributi.current_index == 0)
                    {
                        int i = 0;
                        foreach (TextBox txt in FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione.Controls.OfType<TextBox>()) { if (i == FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione.current_textbox) txt.Text = AllAttributi.Items[index].ToString(); i++; }
                        FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione.EnterAtt();
                    }
                    if (PanelAttributi.current_index == 1)
                    {
                        int i = 0;
                        foreach (TextBox txt in FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione_Attributi.Controls.OfType<TextBox>()) { if (i == FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione_Attributi.current_textbox) txt.Text = AllAttributi.Items[index].ToString(); i++; }
                        FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione_Attributi.EnterAtt();
                    }
                    if (PanelAttributi.current_index == 2)
                    {
                        int i = 0;
                        foreach (TextBox txt in FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione.Controls.OfType<TextBox>()) { if (i == FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione.current_textbox) txt.Text = AllAttributi.Items[index].ToString(); i++; }
                        FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione.EnterAtt();
                    }
                    if (PanelAttributi.current_index == 3)
                    {
                        int i = 0;
                        foreach (TextBox txt in FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione_Attributi.Controls.OfType<TextBox>()) { if (i == FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione_Attributi.current_textbox) txt.Text = AllAttributi.Items[index].ToString(); i++; }
                        FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione_Attributi.EnterAtt();
                    }
                    if (PanelAttributi.current_index == 4)
                    {
                        int i = 0;
                        foreach (TextBox txt in FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione_Metodi.Controls.OfType<TextBox>()) { if (i == FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione_Metodi.current_textbox) txt.Text = AllAttributi.Items[index].ToString(); i++; }
                        FinestraPrincipale.BackPanel.Panel_Ricerca.PanelInclusione_Metodi.EnterAtt();
                    }
                    if (PanelAttributi.current_index == 5)
                    {
                        int i = 0;
                        foreach (TextBox txt in FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione_Metodi.Controls.OfType<TextBox>()) { if (i == FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione_Metodi.current_textbox) txt.Text = AllAttributi.Items[index].ToString(); i++; }
                        FinestraPrincipale.BackPanel.Panel_Ricerca.PanelEsclusione_Metodi.EnterAtt();
                    }
                }
            }
            else ClickNull();
            AllAttributi.SelectedItem = null;
        }




        Point Lastpos;
        bool fermo;
        private void Timer_Mouse(object sender, EventArgs e)
        {
            if (!Visible || contextMenuStrip.Visible) return;
            if (Lastpos == Cursor.Position && !fermo)
            {
                OnAttributiMouseMove();
                fermo = true;
            }
            if (Lastpos != Cursor.Position) { fermo = false; }
            Lastpos = Cursor.Position;
        }
        private void OnAttributiMouseMove()
        {
            try
            {
                string strTip; int nIdx; bool active = false;
                strTip = "";
                if (double_win)
                {
                    nIdx = Attributi.IndexFromPoint(Attributi.PointToClient(Cursor.Position));
                    if ((nIdx >= 0) && (nIdx < Attributi.Items.Count))
                    {
                        active = true;
                        strTip = Attributi.Items[nIdx].ToString();
                        Rectangle bounds = Attributi.GetItemRectangle(nIdx);
                        toolTip.Show(strTip, this, new Point(10, Attributi.Location.Y + bounds.Y - 15));
                    }
                    else active = false;
                }
                strTip = "";
                nIdx = AllAttributi.IndexFromPoint(AllAttributi.PointToClient(Cursor.Position));
                if ((nIdx >= 0) && (nIdx < AllAttributi.Items.Count))
                {
                    active = true;
                    strTip = AllAttributi.Items[nIdx].ToString();
                    Rectangle bounds = AllAttributi.GetItemRectangle(nIdx);
                    toolTip.Show(strTip, this, new Point(10, AllAttributi.Location.Y + bounds.Y - 15));
                }
                if (!active) toolTip.Hide(this);
            }
            catch (Exception) { }
        }
    }
}
