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
    public class PanelAttributi : Panel
    {

        private readonly int size_controllo = 15;
        private Bitmap vero = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("True")));
        private Bitmap falso = new Bitmap((System.Drawing.Image)(Moneyguard.Properties.Resources.ResourceManager.GetObject("False")));
        string spunta_vero = "Attributo già usato";
        string spunta_falso = "Attributo mai usato";
        public List<TextBox> textbox = new List<TextBox>();
        public List<Label> Controllo = new List<Label>();
        public Timer timer, timer_Ricerca;
        private MotoreRicerca PanelMotore;
        ToolTip toolTip = new ToolTip();
        public int current_textbox = -1;
        public int index = -1;
        int emSize = 12;
        static public int current_index;
        public string nuovo_attributo = "  \u2192 Nuovo \u2190  ";
        string arg1=" "; bool arg2 = false;
        public void Disposer()
        {
            timer.Dispose();
            if(timer_Ricerca!= null) timer_Ricerca.Dispose();
            vero.Dispose();
            falso.Dispose();
            textbox.Clear();
            Controllo.Clear();
            Dispose();
        }

        public PanelAttributi()
        {
            DoubleBuffered = true;
            AutoScroll = true;
            BorderStyle = BorderStyle.FixedSingle;
            Visible = false;
            BackColor = Color.White;
            timer = new Timer
            {
                Enabled = true,
                Interval = 10,
            };
            timer.Tick += new System.EventHandler(Timer);
        }

        public void SetPanel(bool initial)
        {
            if (index == -1) { PanelMotore = FinestraPrincipale.BackPanel.Panel_Giorno.PanelMotore; emSize = 16; }
            else
            {
                if (index == 0 || index == 2) PanelMotore = FinestraPrincipale.BackPanel.Panel_Ricerca.PanelMotore_Tipologie;
                else if (index == 4 || index == 5) PanelMotore = FinestraPrincipale.BackPanel.Panel_Ricerca.PanelMotore_Metodi;
                else PanelMotore = FinestraPrincipale.BackPanel.Panel_Ricerca.PanelMotore_Attributi;
            }
            PanelMotore.RefreshForm(arg1, arg2, initial);
            if (index == 0) { nuovo_attributo = " \u2192 Includi tipologia"; spunta_vero = "OK"; spunta_falso = "Tipologia inesistente"; }
            else if (index == 1) { nuovo_attributo = " \u2192 Includi attributo"; spunta_vero = "OK"; spunta_falso = "Attributo inesistente"; }
            else if (index == 2) { nuovo_attributo = " \u2192 Escludi tipologia"; spunta_vero = "OK"; spunta_falso = "Tipologia inesistente"; }
            else if (index == 3) { nuovo_attributo = " \u2192 Escludi attributo"; spunta_vero = "OK"; spunta_falso = "Attributo inesistente"; }
            else if (index == 4) { nuovo_attributo = " \u2192 Includi metodo"; spunta_vero = "OK"; spunta_falso = "Metodo inesistente"; }
            else if (index == 5) { nuovo_attributo = " \u2192 Escludi metodo"; spunta_vero = "OK"; spunta_falso = "Metodo inesistente"; }
            Controls.Clear();
            textbox.Clear();
            Controllo.Clear();
            textbox.Insert(textbox.Count, new TextBox() { Text = nuovo_attributo, BorderStyle = System.Windows.Forms.BorderStyle.None, });
            textbox[0].Click += TextClick;
            textbox[0].GotFocus += Ricerca;
            textbox[0].ForeColor = Color.FromArgb(30, Color.Gainsboro);
            textbox[0].KeyDown += EnterAttributo;
            textbox[0].KeyUp += CheckAttributi;
            textbox[0].Font = new System.Drawing.Font(BackPanel.font1, emSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Controllo.Insert(Controllo.Count, new Label() { BackgroundImageLayout = ImageLayout.Stretch, Visible = false, });
            Controls.Add(textbox[0]);
            Controls.Add(Controllo[0]);
            timer = new Timer
            {
                Enabled = true,
                Interval = 10,
            };
            timer.Tick += new System.EventHandler(Timer);
        }

        public void ResizeAttributi()
        {
            if (index != -1) { Size = new Size(200, textbox.Count * 22); try { FinestraPrincipale.BackPanel.Panel_Ricerca.RefreshLocation(); } catch (Exception) { Console.WriteLine("Errore Resize Attributi"); } }
            for (int i = 0; i < textbox.Count; i++)
            {
                if (index != -1) textbox[i].Size = new Size((int)(Width - 30 - size_controllo), 20);
                else textbox[i].Size = new Size((int)(Width - 54 - size_controllo), 20);
                Controllo[i].Size = new Size(size_controllo, size_controllo);
                textbox[i].Location = new Point(4 + size_controllo * 2, textbox[0].Location.Y + textbox[0].Height * i);
                Controllo[i].Location = new Point(4, textbox[0].Location.Y + (int)(textbox[0].Height * (i+0.3 - 0.2 * (16 - emSize) / 4)));
                textbox[i].Font = new System.Drawing.Font(BackPanel.font1, emSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }
        }
        public void NewTextbox()
        {
            if (textbox.Count == 0 || textbox[textbox.Count - 1].Text != nuovo_attributo)
            {
                textbox.Insert(textbox.Count, new TextBox() { Text = nuovo_attributo, BorderStyle = System.Windows.Forms.BorderStyle.None, });
                Controllo.Insert(Controllo.Count, new Label() { BackgroundImageLayout = ImageLayout.Stretch, Visible = false, });
                Controls.Add(textbox[textbox.Count - 1]);
                Controls.Add(Controllo[Controllo.Count - 1]);
                textbox[textbox.Count - 1].Click += TextClick;
                textbox[textbox.Count - 1].GotFocus += Ricerca;
                textbox[textbox.Count - 1].KeyDown += EnterAttributo;
                textbox[textbox.Count - 1].KeyUp += CheckAttributi;
                textbox[textbox.Count - 1].ForeColor = Color.FromArgb(30, Color.Gainsboro);
                ResizeAttributi();
                if (textbox.Count > 2) if (textbox[textbox.Count - 2].Focused) ScrollToBottom(this);
            }
            foreach (TextBox box in textbox)
            {
                if (box.Focused == false) box.SelectionStart = 0;
                if (box.Text != nuovo_attributo) box.ForeColor = Color.Black;
            }
        }

        public void ScrollToBottom(Panel p)
        {
            using (Control c = new Control() { Parent = p, Dock = DockStyle.Bottom })
            {
                p.ScrollControlIntoView(c);
                c.Parent = null;
            }
        }

        public void Timer(object sender, EventArgs e)
        {
            foreach (TextBox box in textbox)
            {
                if (box.Focused && box.Text == nuovo_attributo) if (box.SelectionStart == 0 && box.SelectionLength == box.Text.Length) return; else { box.SelectionStart = 0; box.SelectionLength = box.Text.Length; }
            }
            for (int i = 0; i < textbox.Count(); i++)
            {
                if (textbox[i].Focused) { current_textbox = i; current_index = index; }
                if (textbox[i].Text == nuovo_attributo || textbox[i].Text == "") if (i != textbox.Count() - 1)
                    {
                        Controls.Remove(textbox[i]);
                        textbox.RemoveAt(i);
                        Controls.Remove(Controllo[i]);
                        Controllo.RemoveAt(i);
                        if (i == 0) textbox[0].Location = new Point(0, 0);
                        foreach (TextBox box in textbox) if (box.Text == nuovo_attributo) box.Focus();
                        ResizeAttributi();
                    }
            }
            NewTextbox();
            
        }

        private void EnterAttributo(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.Control)
            {
                if(FinestraPrincipale.BackPanel.Panel_Giorno!= null) FinestraPrincipale.BackPanel.Panel_Giorno.Panel_EventoGuidato.Pannello.Click_True();
                if (FinestraPrincipale.BackPanel.Panel_Ricerca != null) FinestraPrincipale.BackPanel.Panel_Ricerca.RicercaEventi();
                e.SuppressKeyPress = true;
                return;
            }
            if (e.KeyCode == Keys.Down) { PanelMotore.GoDown(); e.SuppressKeyPress = true; }
            if (e.KeyCode == Keys.Up) { PanelMotore.GoUp(); e.SuppressKeyPress = true; }
            if (e.KeyCode == Keys.Back)
            {

                foreach (TextBox box in Controls.OfType<TextBox>())
                {
                    if (box.Focused)
                    {
                        if (box.Text == nuovo_attributo) { e.SuppressKeyPress = true; return; }
                    }
                }
            }

            if (e.KeyCode == Keys.Enter)
            {
                EnterAtt();
                e.SuppressKeyPress = true;
            }
        }
        public void EnterAtt()
        {
            foreach (TextBox box in textbox)
            {
                if (box.Focused)
                {
                    if (MotoreRicerca.underline) box.Text = PanelMotore.GetText();
                }
            }
            NewTextbox();
            for (int i = 0; i < textbox.Count(); i++)
            {
                if (textbox[i].Text == nuovo_attributo) { textbox[i].Focus(); current_textbox = i; current_index = index; }
            }
            PanelMotore.SetTextbox("");
            CheckAllAttributi();
        }
        private void CheckAttributi(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Up || e.KeyCode == Keys.Down) return;
            string testo = "";
            for (int i = 0; i < textbox.Count - 1; i++) if (textbox[i].Focused)
                {
                    testo = Funzioni_utili.Scremato(textbox[i].Text);
                    if (textbox[i].Text == nuovo_attributo) testo = "";
                    PanelMotore.SetTextbox(testo);
                    if (testo == "") { Controllo[i].Visible = false; return; }
                    if(index == -1 || index == 1 || index == 3)
                    {
                        if (Input.all_attributi.Contains(testo))
                        {
                            if (Controllo[i].BackgroundImage == vero && Controllo[i].Visible) return;
                            else Controllo[i].BackgroundImage = vero;
                            toolTip.SetToolTip(Controllo[i], spunta_vero);
                            Controllo[i].Visible = true;
                        }
                        else
                        {
                            if (Controllo[i].BackgroundImage == falso && Controllo[i].Visible) return;
                            else Controllo[i].BackgroundImage = falso;
                            toolTip.SetToolTip(Controllo[i], spunta_falso);
                            Controllo[i].Visible = true;
                        }
                    }
                    else if (index == 0 || index == 2)
                    {
                        if (Input.tipi_scremati.Contains(testo))
                        {
                            if (Controllo[i].BackgroundImage == vero && Controllo[i].Visible) return;
                            else Controllo[i].BackgroundImage = vero;
                            toolTip.SetToolTip(Controllo[i], spunta_vero);
                            Controllo[i].Visible = true;
                        }
                        else
                        {
                            if (Controllo[i].BackgroundImage == falso && Controllo[i].Visible) return;
                            else Controllo[i].BackgroundImage = falso;
                            toolTip.SetToolTip(Controllo[i], spunta_falso);
                            Controllo[i].Visible = true;
                        }
                    }
                    else
                    {
                        if (Input.metodi_scremati.Contains(testo))
                        {
                            if (Controllo[i].BackgroundImage == vero && Controllo[i].Visible) return;
                            else Controllo[i].BackgroundImage = vero;
                            toolTip.SetToolTip(Controllo[i], spunta_vero);
                            Controllo[i].Visible = true;
                        }
                        else
                        {
                            if (Controllo[i].BackgroundImage == falso && Controllo[i].Visible) return;
                            else Controllo[i].BackgroundImage = falso;
                            toolTip.SetToolTip(Controllo[i], spunta_falso);
                            Controllo[i].Visible = true;
                        }
                    }
                }
        }
        public void CheckAllAttributi()
        {
            foreach (TextBox txt in textbox) if (txt.Focused && Funzioni_utili.Scremato(txt.Text) == "") return;
            for (int i = 0; i < textbox.Count - 1; i++)
            {
                string testo = Funzioni_utili.Scremato(textbox[i].Text);
                if (testo != "" && testo != nuovo_attributo)
                {
                    if (index == -1 || index == 1 || index == 3)
                    {
                        if (Input.all_attributi.Contains(testo))
                        {
                            Controllo[i].BackgroundImage = FinestraPrincipale.BackPanel.vero;
                            toolTip.SetToolTip(Controllo[i], spunta_vero);
                            Controllo[i].Visible = true;
                            ResizeAttributi();
                        }
                        else
                        {
                            Controllo[i].BackgroundImage = FinestraPrincipale.BackPanel.falso;
                            toolTip.SetToolTip(Controllo[i], spunta_falso);
                            Controllo[i].Visible = true;
                            ResizeAttributi();
                        }
                    }
                    else if(index == 0 || index == 2)
                    {
                        if (Input.tipi_scremati.Contains(testo))
                        {
                            Controllo[i].BackgroundImage = FinestraPrincipale.BackPanel.vero;
                            toolTip.SetToolTip(Controllo[i], spunta_vero);
                            Controllo[i].Visible = true;
                            ResizeAttributi();
                        }
                        else
                        {
                            Controllo[i].BackgroundImage = FinestraPrincipale.BackPanel.falso;
                            toolTip.SetToolTip(Controllo[i], spunta_falso);
                            Controllo[i].Visible = true;
                            ResizeAttributi();
                        }
                    }
                    else
                    {
                        if (Input.metodi_scremati.Contains(testo))
                        {
                            Controllo[i].BackgroundImage = vero;
                            toolTip.SetToolTip(Controllo[i], spunta_vero);
                            Controllo[i].Visible = true;
                        }
                        else
                        {
                            Controllo[i].BackgroundImage = falso;
                            toolTip.SetToolTip(Controllo[i], spunta_falso);
                            Controllo[i].Visible = true;
                        }
                    }
                }
            }
        }
        void Ricerca(object sender, EventArgs e)
        {
            foreach (TextBox txt in Controls.OfType<TextBox>()) if (txt.Focused)
                {
                    PanelMotore.RefreshForm(arg1, arg2, true);
                    if (index != -1)
                    {
                        if (PanelMotore != FinestraPrincipale.BackPanel.Panel_Ricerca.PanelMotore_Attributi) FinestraPrincipale.BackPanel.Panel_Ricerca.PanelMotore_Attributi.HideMotore();
                        if (PanelMotore != FinestraPrincipale.BackPanel.Panel_Ricerca.PanelMotore_Tipologie) FinestraPrincipale.BackPanel.Panel_Ricerca.PanelMotore_Tipologie.HideMotore();
                        if (PanelMotore != FinestraPrincipale.BackPanel.Panel_Ricerca.PanelMotore_Metodi) FinestraPrincipale.BackPanel.Panel_Ricerca.PanelMotore_Metodi.HideMotore();
                    }
                    if (txt.Text == nuovo_attributo) { PanelMotore.SetTextbox(""); }
                    else PanelMotore.SetTextbox(Funzioni_utili.Scremato(txt.Text));
                }
        }
        public void HideRicerca(object sender, EventArgs e)
        {
            bool domanda = false;
            foreach (TextBox txt in Controls.OfType<TextBox>()) if (txt.Focused) domanda = true;
            if (domanda) return;
            PanelMotore.HideMotore();
        }
        public void TextClick(object sender, EventArgs e)
        {
            Ricerca(sender, e);
        }
        public void SetArgs(string arg1, bool arg2)
        {
            this.arg1 = arg1;
            this.arg2 = arg2;
        }
    }
}
