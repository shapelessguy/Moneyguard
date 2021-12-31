using System;
using System.Drawing;
using System.Windows.Forms;

namespace Moneyguard
{
    partial class FinestraPrincipale
    {
        public System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                try { components.Dispose(); } catch (Exception) { Console.WriteLine("Errore Disposing"); }
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FinestraPrincipale
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(Properties.Resources.ResourceManager.GetObject("Moneyguard")));
            this.Name = "Moneyguard";
            this.Text = "Moneyguard";
            this.ResumeLayout(false);
            this.DoubleBuffered = true;
            this.Location = Impostazioni.location;
            this.StartPosition = FormStartPosition.Manual;
            if (!Impostazioni.show_calendar_when_opened) Visible = false;
            if (Impostazioni.show_maximized) WindowState = FormWindowState.Maximized;
            this.ResizeEnd += new System.EventHandler(ResizeAllWindows);
            this.MinimumSize = new Size(minimum_weight, minimum_height);
            this.ClientSize = (Size)Impostazioni.size;
        }

        #endregion


    }
}

