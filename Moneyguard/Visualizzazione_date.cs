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
    public class Visualizzazione_date : Panel
    {
        List<Label> Raccolta_graffe_Giorni = new List<Label>();
        List<Label> Raccolta_Giorni = new List<Label>();
        List<Label> Raccolta_graffe_Mesi = new List<Label>();
        List<Label> Raccolta_Mesi = new List<Label>();
        List<Label> Raccolta_graffe_Anni = new List<Label>();
        List<Label> Raccolta_Anni = new List<Label>();
        List<Point> giorno_point = new List<Point>();
        List<Point> mese_point = new List<Point>();
        List<Point> anno_point = new List<Point>();
        List<string> giorno_nome = new List<string>();
        List<string> mese_nome = new List<string>();
        List<string> anno_nome = new List<string>();
        int separazione = 4; //pari
        int limite1 = 42, limite2 = 26, limite3 = 50;
        public Visualizzazione_date()
        {
            AutoScroll = false;
            for (int i = 0; i < limite1; i++)
            {
                Raccolta_graffe_Giorni.Add(new Label() { BackgroundImageLayout = ImageLayout.Stretch, BackgroundImage = Moneyguard.Properties.Resources.Graffa, AutoSize = false, Text = "", });
                Controls.Add(Raccolta_graffe_Giorni[i]);
            }
            for (int i = 0; i < limite1; i++)
            {
                Raccolta_Giorni.Add(new Label() { AutoSize = false, Text = "Settembre", TextAlign = ContentAlignment.TopCenter, });
                Controls.Add(Raccolta_Giorni[i]);
            }
            for (int i = 0; i < limite2; i++)
            {
                Raccolta_graffe_Mesi.Add(new Label() { BackgroundImageLayout = ImageLayout.Stretch, BackgroundImage = Moneyguard.Properties.Resources.Graffa, AutoSize = false, Text = "", });
                Controls.Add(Raccolta_graffe_Mesi[i]);
            }
            for (int i = 0; i < limite2; i++)
            {
                Raccolta_Mesi.Add(new Label() { AutoSize = false, Text = "Settembre", TextAlign = ContentAlignment.TopCenter, });
                Controls.Add(Raccolta_Mesi[i]);
            }
            for (int i = 0; i < limite3; i++)
            {
                Raccolta_graffe_Anni.Add(new Label() { BackgroundImageLayout = ImageLayout.Stretch, BackgroundImage = Moneyguard.Properties.Resources.Graffa, AutoSize = false, Text = "", });
                Controls.Add(Raccolta_graffe_Anni[i]);
            }
            for (int i = 0; i < limite3; i++)
            {
                Raccolta_Anni.Add(new Label() { AutoSize = false, Text = "Settembre", TextAlign = ContentAlignment.TopCenter, });
                Controls.Add(Raccolta_Anni[i]);
            }
        }
        public void Aggiorna(long datacode_min, long datacode_max)
        {
            giorno_point.Clear();
            giorno_nome.Clear();
            mese_point.Clear();
            mese_nome.Clear();
            anno_point.Clear();
            anno_nome.Clear();
            int num_giorni = (int)((datacode_max - datacode_min) / 86400);

            if (num_giorni < 40)
            {
                List<long> inizi_giorno = new List<long>();
                List<long> fini_giorno = new List<long>();
                long datacode_giornoin = 0;
                long datacode_giornofin = 0;
                if (datacode_min % 86400 == 0) datacode_giornoin = datacode_min; else { datacode_giornoin = datacode_min + 86400 - datacode_min % 86400; }
                if (datacode_max % 86400 == 0) datacode_giornofin = datacode_max; else { datacode_giornofin = datacode_max - datacode_max % 86400; }
                inizi_giorno.Add(datacode_giornoin);

                int giorno_collector = 0;
                for (int i = 0; i < num_giorni; i++)
                {
                    int[] data = Date.Decodifica((uint)(datacode_giornoin + i * 86400));
                    if (i == 0) giorno_nome.Add(data[3].ToString());
                    if (i != 0 && data[3] != giorno_collector)
                    {
                        fini_giorno.Add(datacode_giornoin + (i) * 86400 -2);
                        inizi_giorno.Add(datacode_giornoin + (i) * 86400);
                        giorno_nome.Add(data[3].ToString());
                    }
                    giorno_collector = data[3];
                }
                //Console.WriteLine(Date.ShowDate(Date.Decodifica((uint)datacode_giornoin)) + "__" + Date.ShowHour(Date.Decodifica((uint)datacode_giornoin)));
                //foreach (long inizio in inizi_giorno) Console.WriteLine(Date.ShowDate(Date.Decodifica((uint)inizio)) + "__"+ Date.ShowHour(Date.Decodifica((uint)inizio)));
                fini_giorno.Add(datacode_giornofin);
                //if (fini_giorno[fini_giorno.Count - 1] <= inizi_giorno[inizi_giorno.Count - 1] + 86400) { inizi_giorno.RemoveAt(inizi_giorno.Count - 1); fini_giorno.RemoveAt(fini_giorno.Count - 1); giorno_nome.RemoveAt(giorno_nome.Count - 1); }
                for (int j = 0; j < inizi_giorno.Count; j++) giorno_point.Add(new Point((int)(inizi_giorno[j]), (int)(fini_giorno[j])));
            }
            if (num_giorni < 31 * 12 * 2)
            {
                //mese_point.Add(new Point((int)datacode_min, 0));
                List<long> inizi_mese = new List<long>();
                List<long> fini_mese = new List<long>();
                long datacode_giornoin = 0;
                long datacode_giornofin = 0;
                if (datacode_min % 86400 == 0) datacode_giornoin = datacode_min; else { datacode_giornoin = datacode_min + 86400 - datacode_min % 86400; }
                if (datacode_max % 86400 == 0) datacode_giornofin = datacode_max; else { datacode_giornofin = datacode_max - datacode_max % 86400; }
                inizi_mese.Add(datacode_giornoin);

                string mese_collector = "";
                for (int i = 0; i < num_giorni; i++)
                {
                    int[] data = Date.Decodifica((uint)(datacode_giornoin + i * 86400));
                    if (i == 0) mese_nome.Add(Date.GetMesetxt(data[4]));
                    if (i != 0 && Date.GetMesetxt(data[4]) != mese_collector)
                    {
                        fini_mese.Add(datacode_giornoin + (i) * 86400 - 2);
                        inizi_mese.Add(datacode_giornoin + (i) * 86400);
                        mese_nome.Add(Date.GetMesetxt(data[4]));
                    }
                    mese_collector = Date.GetMesetxt(data[4]);
                }
                fini_mese.Add(datacode_giornofin);
                //if (fini_mese[fini_mese.Count - 1] <= inizi_mese[inizi_mese.Count - 1] + 86400) { inizi_mese.RemoveAt(inizi_mese.Count - 1); fini_mese.RemoveAt(fini_mese.Count - 1); mese_nome.RemoveAt(mese_nome.Count - 1); }
                for (int j = 0; j < inizi_mese.Count; j++) mese_point.Add(new Point((int)(inizi_mese[j]), (int)(fini_mese[j])));
            }
            if (num_giorni < 31 * 12 * 80)
            {
                //mese_point.Add(new Point((int)datacode_min, 0));
                List<long> inizi_anno = new List<long>();
                List<long> fini_anno = new List<long>();
                long datacode_giornoin = 0;
                long datacode_giornofin = 0;
                if (datacode_min % 86400 == 0) datacode_giornoin = datacode_min; else { datacode_giornoin = datacode_min + 86400 - datacode_min % 86400; }
                if (datacode_max % 86400 == 0) datacode_giornofin = datacode_max; else { datacode_giornofin = datacode_max - datacode_max % 86400; }
                inizi_anno.Add(datacode_giornoin);

                int anno_collector = 0;
                for (int i = 0; i < num_giorni; i++)
                {
                    int[] data = Date.Decodifica((uint)(datacode_giornoin + i * 86400));
                    if (i == 0) anno_nome.Add(data[5].ToString());
                    if (i != 0 && data[5] != anno_collector)
                    {
                        fini_anno.Add(datacode_giornoin + (i) * 86400 - 2);
                        inizi_anno.Add(datacode_giornoin + (i) * 86400);
                        anno_nome.Add(data[5].ToString());
                    }
                    anno_collector = data[5];
                }
                fini_anno.Add(datacode_giornofin);
                //if (fini_anno[fini_anno.Count - 1] <= inizi_anno[inizi_anno.Count - 1] + 86400) { inizi_anno.RemoveAt(inizi_anno.Count - 1); fini_anno.RemoveAt(fini_anno.Count - 1); anno_nome.RemoveAt(anno_nome.Count - 1); }
                for (int j = 0; j < inizi_anno.Count; j++) anno_point.Add(new Point((int)(inizi_anno[j]), (int)(fini_anno[j])));
            }


            double livello1 = 0;
            double livello2 = 0.3;
            double livello3 = 0.7;

            int location_giorni = (int)(Height * livello1);
            int location_mesi = (int)(Height * livello2);
            int location_anni = (int)(Height * livello3);



            for (int i = 0; i < giorno_point.Count; i++)
            {
                Raccolta_graffe_Giorni[i].Show();
                double aus1 = ((double)(giorno_point[i].Y - giorno_point[i].X) * Width);
                double aus2 = (double)(datacode_max - datacode_min);
                double aus3 = ((double)(giorno_point[i].X - datacode_min) * Width);
                //Console.WriteLine("Posizione puntatore: "+ giorno_point[i].X + "______Inizio: "+datacode_min + "   Fine: " +datacode_max + "        " + (double)(giorno_point[i].X - datacode_min)/ (double)(datacode_max - datacode_min));
                Raccolta_graffe_Giorni[i].Size = new Size((int)(aus1 / aus2) - separazione, 10);
                Raccolta_graffe_Giorni[i].Location = new Point((int)(aus3 / aus2) + separazione / 2, location_giorni);
            }
            for (int i = 0; i < giorno_point.Count; i++)
            {
                Raccolta_Giorni[i].Show();
                Raccolta_Giorni[i].Size = new Size(Raccolta_graffe_Giorni[i].Width, 30);
                Raccolta_Giorni[i].Location = new Point(Raccolta_graffe_Giorni[i].Location.X, Raccolta_graffe_Giorni[i].Location.Y + Raccolta_graffe_Giorni[i].Height);
                Raccolta_Giorni[i].Font = new Font(BackPanel.font1, (int)(Width * 0.005 + 4), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                if (Raccolta_Giorni[i].Width > 25) Raccolta_Giorni[i].Text = giorno_nome[i]; else { Raccolta_Giorni[i].Hide(); Raccolta_graffe_Giorni[i].Hide(); }
            }
            for (int i = giorno_point.Count; i < limite1; i++)
            {
                if(Raccolta_graffe_Giorni[i].Visible) { Raccolta_graffe_Giorni[i].Hide(); Raccolta_Giorni[i].Hide(); }
            }




            if(giorno_point.Count==0) location_mesi = (int)(Height * livello1); else location_mesi = (int)(Height * livello2);
            for (int i = 0; i < mese_point.Count; i++)
            {
                Raccolta_graffe_Mesi[i].Show();
                double aus1 = ((double)(mese_point[i].Y - mese_point[i].X) * Width * 1.0);
                double aus2 = (double)(datacode_max - datacode_min);
                double aus3 = ((double)(mese_point[i].X - datacode_min) * Width);
                //if(i==mese_point.Count-1) Console.WriteLine("Posizione puntatore: "+ mese_point[i].X + "______Inizio: "+datacode_min + "   Fine: " +datacode_max);
                Raccolta_graffe_Mesi[i].Size = new Size((int)(aus1 / aus2) - separazione, 10);
                Raccolta_graffe_Mesi[i].Location = new Point((int)(aus3 / aus2) + separazione/2, location_mesi);
            }
            for (int i = 0; i < mese_point.Count; i++)
            {
                Raccolta_Mesi[i].Show();
                Raccolta_Mesi[i].Size = new Size(Raccolta_graffe_Mesi[i].Width, 30);
                Raccolta_Mesi[i].Location = new Point(Raccolta_graffe_Mesi[i].Location.X, Raccolta_graffe_Mesi[i].Location.Y + Raccolta_graffe_Mesi[i].Height);
                Raccolta_Mesi[i].Font = new Font(BackPanel.font1, (int)(Width * 0.005 + 4), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                if (Raccolta_Mesi[i].Width > 50) Raccolta_Mesi[i].Text = mese_nome[i]; else if (Raccolta_Mesi[i].Width > 30) Raccolta_Mesi[i].Text = Date.GetMese_abbreviato(mese_nome[i]); else { Raccolta_Mesi[i].Hide(); Raccolta_graffe_Mesi[i].Hide(); }
            }
            for (int i = mese_point.Count; i < limite2; i++)
            {
                if (Raccolta_graffe_Mesi[i].Visible) { Raccolta_graffe_Mesi[i].Hide(); Raccolta_Mesi[i].Hide(); }
            }




            if (giorno_point.Count == 0 && mese_point.Count==0) location_anni = (int)(Height * livello1); else if (giorno_point.Count == 0) location_anni = (int)(Height * livello2); else location_anni = (int)(Height * livello3);
            for (int i = 0; i < anno_point.Count; i++)
            {
                Raccolta_graffe_Anni[i].Show();
                double aus1 = ((double)(anno_point[i].Y - anno_point[i].X) * Width * 1.0);
                double aus2 = (double)(datacode_max - datacode_min);
                double aus3 = ((double)(anno_point[i].X - datacode_min) * Width);
                //if(i==mese_point.Count-1) Console.WriteLine("Posizione puntatore: "+ mese_point[i].X + "______Inizio: "+datacode_min + "   Fine: " +datacode_max);
                Raccolta_graffe_Anni[i].Size = new Size((int)(aus1 / aus2) - separazione, 10);
                Raccolta_graffe_Anni[i].Location = new Point((int)(aus3 / aus2) + separazione / 2, location_anni);
            }
            for (int i = 0; i < anno_point.Count; i++)
            {
                Raccolta_Anni[i].Show();
                Raccolta_Anni[i].Size = new Size(Raccolta_graffe_Anni[i].Width, 30);
                Raccolta_Anni[i].Location = new Point(Raccolta_graffe_Anni[i].Location.X, Raccolta_graffe_Anni[i].Location.Y + Raccolta_graffe_Anni[i].Height);
                Raccolta_Anni[i].Font = new Font(BackPanel.font1, (int)(Width * 0.005 + 6), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                if (Raccolta_Anni[i].Width > 40) Raccolta_Anni[i].Text = anno_nome[i]; else { Raccolta_Anni[i].Hide(); Raccolta_graffe_Anni[i].Hide(); }
            }
            for (int i = anno_point.Count; i < limite3; i++)
            {
                if (Raccolta_graffe_Anni[i].Visible) { Raccolta_graffe_Anni[i].Hide(); Raccolta_Anni[i].Hide(); }
            }





        }
        

        public void Disposer()
        {
            foreach (Label label in Raccolta_graffe_Giorni) { label.BackgroundImage.Dispose(); label.Dispose(); }
            foreach (Label label in Raccolta_graffe_Mesi) { label.BackgroundImage.Dispose(); label.Dispose(); }
            foreach (Label label in Raccolta_graffe_Anni) { label.BackgroundImage.Dispose(); label.Dispose(); }
            foreach (Label label in Raccolta_Giorni) { label.Dispose(); }
            foreach (Label label in Raccolta_Mesi) { label.Dispose(); }
            foreach (Label label in Raccolta_Anni) { label.Dispose(); }
            Dispose();
        }
    }
}
