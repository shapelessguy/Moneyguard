using Moneyguard.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1;

namespace Moneyguard
{
    public class PanelModificheTipi : Panel
    {
        public List<VisualModifiche> VisualTipi = new List<VisualModifiche>();
        private readonly int num_colonne = 8;
        public string tipo;
        public void Disposer()
        {
            foreach (VisualModifiche tip in VisualTipi) { tip.Disposer(); Controls.Remove(tip); }
            Dispose();
        }
        

        public PanelModificheTipi(int j)
        {
            DoubleBuffered = true;
            BorderStyle = BorderStyle.Fixed3D;
            AutoScroll = true;
            Visible = false;
            Location = new Point(0, 0);
            Size = FinestraPrincipale.BackPanel.Size;
            BackColor = Color.Azure;
            ReadIconsModificabili(j);
            ReadIcons(j);
            MouseEnter += new EventHandler(MouseEntered);
            ResizeForm();
        }
        
        public void ResizeForm()
        {
            Size = new Size((int)((FinestraPrincipale.BackPanel.Width) * 0.8), (int)(FinestraPrincipale.BackPanel.Height * 0.7));
            Location = new Point((FinestraPrincipale.BackPanel.Width - Width) / 2, (FinestraPrincipale.BackPanel.Height - Height) / 2);
            ProprietàGiorno.ScrollToTop(this);
            int i = 0, m=0, j = 0, num_file=0, colonne=0;
            foreach (VisualModifiche tip in VisualTipi)
            {
                if (tip.resources_file == 2) num_file++;
            }
            foreach (VisualModifiche tip in VisualTipi)
            {
                tip.Image.Size = new Size(tip.Image.Width, tip.Image.Height + tip.Tipo.Height);
                tip.SetSize(new Size((int)(Width / num_colonne - 5), (int)(Width / num_colonne)), 1);
                if (i < num_file) { if (i % num_colonne == 0 && i!= 0) j++; tip.Location = new Point(tip.Width * i - tip.Width * j * num_colonne, (int)(tip.Height * (j * 1.05))); i++; colonne = j; }
                else {if (m % num_colonne == 0) j++; tip.Location = new Point(tip.Width * m - tip.Width * (j - colonne -1) * num_colonne, (int)(tip.Height * (j * 1.05))); m++;  }
                tip.index = i + m;
            }
        }

        private void MouseEntered(object sender, EventArgs e)
        {
            VisualModifiche.Index = -1;
            foreach (VisualModifiche tip in VisualTipi)
            {
                tip.BordoHide();
            }
        }

        private void ReadIcons(int j)
        {
            ResourceManager MyResourceClass = new ResourceManager(typeof(Resources));
            ResourceSet resourceSet = MyResourceClass.GetResourceSet(System.Globalization.CultureInfo.CurrentUICulture, true, true);
            foreach (System.Collections.DictionaryEntry entry in resourceSet)
            {
                string resourceKey = entry.Key.ToString();
                object resource = entry.Value;
                if (resourceKey.Contains("ticon") && j == 1 && resourceKey.Contains("micon") == false) { VisualTipi.Add(new VisualModifiche("", resourceKey, 1, 1) { resource = resourceKey }); Controls.Add(VisualTipi[VisualTipi.Count - 1]); }
                if (resourceKey.Contains("micon") && j == 2) { VisualTipi.Add(new VisualModifiche("", resourceKey, 1, 2) { resource = resourceKey }); Controls.Add(VisualTipi[VisualTipi.Count - 1]); }
            }
        }
        private void ReadIconsModificabili(int j)
        {
            if (j == 1) foreach (string filename in Directory.EnumerateFiles(Input.path + @"\Icons\Tipologie"))
                {
                    if (filename.Length > 4) if (filename.Substring(filename.Length - 4, 4) == ".png")
                        { VisualTipi.Add(new VisualModifiche("", Funzioni_utili.TakeFileName(filename), 2, 1) {resource = Funzioni_utili.TakeFileName(filename) }); Controls.Add(VisualTipi[VisualTipi.Count - 1]); }
                }
            if (j == 2) foreach (string filename in Directory.EnumerateFiles(Input.path + @"\Icons\Metodi"))
                {
                    if (filename.Length > 4) if (filename.Substring(filename.Length - 4, 4) == ".png")
                        { VisualTipi.Add(new VisualModifiche("", Funzioni_utili.TakeFileName(filename), 2, 2) {resource = Funzioni_utili.TakeFileName(filename) }); Controls.Add(VisualTipi[VisualTipi.Count - 1]); }
                }

        }
    }
}
