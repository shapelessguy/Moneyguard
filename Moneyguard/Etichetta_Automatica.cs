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
    public class Etichetta_Automatica : Panel
    {
        private string attributo = "";
        private string metodo = "";
        private string tipo = "";

        private int[] data = null;
        private int[] data_modifica = null;
        private long datacode = 0;
        private long datacode_modifica = 0;
        private double valore = 0;
        private List<string> attributi = new List<string>();

        Eventi_Aut evento;

        public bool validation = false;
        private int recurrences = 0;

        private Label image;

        public Etichetta_Automatica(Eventi_Aut evento)
        {
            Visible = false;
            this.evento = evento;

            this.attributo = evento.Get_Attributo();
            this.metodo = evento.GetMetodo();
            this.tipo = evento.GetTipo();
            this.data = evento.GetData();
            this.data_modifica = evento.GetData_modifica();
            this.datacode = evento.GetDatacode();
            this.datacode_modifica = evento.GetDatacode_modifica();
            this.valore = evento.GetValore();
            foreach(string stringa in evento.GetAttributi()) attributi.Add(stringa);

            BackColor = Color.Red;
            image = new Label()
            {
                BackgroundImageLayout = ImageLayout.Stretch,
            };
            Controls.Add(image);
        }

        public void Disposer()
        {
            image.BackgroundImage.Dispose();
            image.Dispose();
            Dispose();
        }
        public void ResizeForm(int panel_width)
        {
            Size = new Size(panel_width - 40, (int)(panel_width * 0.3));
            image.Location = new Point(0, 0);
            image.Size = new Size(Height, Height);
            Update();
        }

        public void Aggiorna()
        {
            image.BackgroundImage = Funzioni_utili.TakePicture(this.tipo, 1);
            Visible = true;
            Update();
        }

    }
}
