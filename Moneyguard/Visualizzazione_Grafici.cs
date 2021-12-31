using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1;
using System.Windows.Forms.DataVisualization.Charting;

namespace Moneyguard
{
    public class Visualizzazione_Grafici : Panel
    {
        public Timer acquisizione_dati;
        private Chart Chart;
        Panel Chart_Panel;
        Visualizzazione_date Date;
        Label Indietro, Avanti, Su, Giu;
        ChartArea chartArea1;
        Legend legend1;
        Series series1, series2, series3;
        public Timer Date_delay, Date_delay_initial;
        Label Media_label;
        private DateTimePicker MediaPicker = new DateTimePicker();
        int media = 1;
        ToolTip tooltip;

        public Visualizzazione_Grafici()
        {
            Visible = true;
            BorderStyle = BorderStyle.Fixed3D;
            BackColor = System.Drawing.Color.AliceBlue;
            AutoScroll = true;
            Click += ClickNull;
            acquisizione_dati = new Timer()
            {
                Enabled = true,
                Interval = 20,
            };
            Chart_Panel = new Panel()
            {
                Visible = true,
                AutoScroll = true,
                BackColor = Color.AliceBlue,
            };
            Controls.Add(Chart_Panel);
            Chart_Panel.Click += ClickNull;
            tooltip = new ToolTip();
            Media_label = new Label()
            {
                AutoSize = false,
                Text = "Arrotondamento: ",
            };
            tooltip.SetToolTip(Media_label, "01: Nessun arrotondamento\n02: Visione giornaliera\n>03: Media gaussiana");
            Chart_Panel.Controls.Add(Media_label);
            Indietro = new Label()
            {
                BackgroundImage = Moneyguard.Properties.Resources.Left_t,
                BackgroundImageLayout = ImageLayout.Stretch,
                BackColor = Color.Transparent,
                AutoSize = false,
            };
            Controls.Add(Indietro);
            Avanti = new Label()
            {
                BackgroundImage = Moneyguard.Properties.Resources.Right_t,
                BackgroundImageLayout = ImageLayout.Stretch,
                BackColor = Color.Transparent,
                AutoSize = false,
            };
            Controls.Add(Avanti);
            Su = new Label()
            {
                BackgroundImage = Moneyguard.Properties.Resources.Resize_up,
                BackgroundImageLayout = ImageLayout.Stretch,
                BackColor = Color.Transparent,
                AutoSize = false,
            };
            Controls.Add(Su);
            Giu = new Label()
            {
                BackgroundImage = Moneyguard.Properties.Resources.Resize_down,
                BackgroundImageLayout = ImageLayout.Stretch,
                BackColor = Color.Transparent,
                AutoSize = false,
            };
            Controls.Add(Giu);
            Date = new Visualizzazione_date()
            {
                Visible = true,
                Location = new Point((int)((92 * Chart_Panel.Width) / 100), (int)(Chart_Panel.Height * 0.7)),
                Size = new Size(Chart_Panel.Width - (int)((92 * Chart_Panel.Width) / 100), (int)(Chart_Panel.Height * 0.29)),
            };
            Date.Click += ClickNull;
            foreach (Control control in Date.Controls) control.Click += ClickNull;
            Date_delay = new Timer()
            {
                Enabled = true,
                Interval = 10,
            };
            Date_delay_initial = new Timer()
            {
                Enabled = true,
                Interval = 50,
            };
            Chart_Panel.Controls.Add(Date);
            Indietro.MouseEnter += Indietro_Big;
            Indietro.MouseLeave += Indietro_Small;
            Avanti.MouseEnter += Avanti_Big;
            Avanti.MouseLeave += Avanti_Small;
            Su.MouseEnter += Su_Big;
            Su.MouseLeave += Su_Small;
            Giu.MouseEnter += Giu_Big;
            Giu.MouseLeave += Giu_Small;
            Indietro.Click += IndietroClick;
            Avanti.Click += AvantiClick;
            Su.Click += SuClick;
            Giu.Click += GiuClick;


            MediaPicker = new DateTimePicker()
            {
                Format = DateTimePickerFormat.Custom,
                ShowUpDown = true,
                TabIndex = 2,
                //TabStop = false,
            };
            MediaPicker.CustomFormat = "mm";
            MediaPicker.MinDate = new DateTime(2018, 1, 1, 0, 1, 0);
            MediaPicker.MaxDate = new DateTime(2018, 1, 1, 0, 50, 0);
            MediaPicker.Value = new DateTime(2018, 1, 1, 0, 1, 0);
            Chart_Panel.Controls.Add(MediaPicker);
            MediaPicker.Leave += new EventHandler(LeaveMedia);
            MediaPicker.KeyDown += new KeyEventHandler(PressEnter);


            chartArea1 = new ChartArea() { BorderDashStyle = ChartDashStyle.Solid,};
            legend1 = new Legend();
            series1 = new Series();
            series2 = new Series();
            series3 = new Series();
            Chart = new Chart() { Name = "Chart", Visible = true, BackColor = Color.AliceBlue};
            Chart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            Chart.Legends.Add(legend1);
            Chart.Location = new System.Drawing.Point(0, 0);
            Chart.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series2.ChartArea = "ChartArea1";
            series3.ChartArea = "ChartArea1";
            Chart.Series.Add(series1);
            Chart.Series.Add(series2);
            Chart.Series.Add(series3);
            Chart.Text = "chart1";
            Chart_Panel.Controls.Add(Chart);
            Chart.MouseWheel += WheelMouse;
            Chart.MouseMove += Data_MouseMove;
            Chart.MouseEnter += Data_MouseEnter;
            Chart.MouseLeave += Data_MouseLeave;

            Chart.ChartAreas[0].AxisY.Enabled = AxisEnabled.False;
            Chart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
            Chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            Chart.ChartAreas[0].AxisY2.MajorGrid.Enabled = false;
            Chart.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
            Chart.ChartAreas[0].AxisY2.LabelStyle.Format = "{0:0}\u20AC";
            
            series1.IsVisibleInLegend = false;
            series2.IsVisibleInLegend = false;
            series3.IsVisibleInLegend = false;
            Chart.ChartAreas[0].Position.Auto = false;
            series1.CustomProperties = "DrawingStyle=Wedge, PointWidth=5000, DrawSideBySide=True";
            series2.CustomProperties = "DrawingStyle=Wedge, PointWidth=5000, DrawSideBySide=True";
            series3.CustomProperties = "DrawingStyle=Wedge, PointWidth=5000, DrawSideBySide=True";
            series1.Color = Color.CadetBlue;
            series2.Color = Color.Red;
            series3.Color = Color.Green;
            FinestraPrincipale.BackPanel.KeyDown += KeyControl;
            foreach (Control controllo in Controls) controllo.BringToFront();

        }
        public void Disposer()
        {
           
            acquisizione_dati.Dispose();
            Date.Disposer();
            
            Indietro.BackgroundImage.Dispose();
            Avanti.BackgroundImage.Dispose();
            Su.BackgroundImage.Dispose();
            Giu.BackgroundImage.Dispose();
            Indietro.Dispose();
            Avanti.Dispose();
            Su.Dispose();
            Giu.Dispose();
            Date_delay.Dispose();
            Date_delay_initial.Dispose();
            
            MediaPicker.Dispose();
            //chartArea1.Dispose();
            legend1.Dispose();
            series1.Dispose();
            series2.Dispose();
            series3.Dispose();
            //Controls.Remove(Chart_Panel);
            Chart.Dispose();
            //Chart_Panel.Dispose();
            tooltip.Dispose();
            //Dispose();
        }

        private void LeaveMedia(object sender, EventArgs e)
        {

        }

        private void PressEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FinestraPrincipale.BackPanel.Focus();
                acquisizione_dati.Tick += AcquisizioneDati;
            }
        }

        public void ResizeForm()
        {
            Chart_Panel.Location = new Point(10, 10);
            Chart_Panel.Size = new System.Drawing.Size((int)(Width * 0.95), (int)(Height * 0.98));
            Chart.Size = new Size((int)(Chart_Panel.Width), (int)(Chart_Panel.Height * 0.6));
            Date_delay.Tick += DateDelay;
            Indietro_Small(null, null);
            Avanti_Small(null, null);
            Su_Small(null, null);
            Giu_Small(null, null); Chart.ChartAreas[0].AxisY2.LabelStyle.Font = new Font(BackPanel.font1, (int)(Width * 0.007 + 4), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            MediaPicker.ShowUpDown = false;
            Media_label.Location = new Point((int)(Width * 0.3), (int)(Height * 0.85));
            Media_label.Size = new System.Drawing.Size((int)(Width * 0.2), (int)(Width * 0.04));
            MediaPicker.Location = new Point(Media_label.Location.X + Media_label.Width, (int)(Height * 0.85));
            MediaPicker.Size = new System.Drawing.Size((int)(Width * 0.06), (int)(Width * 0.04));
            MediaPicker.Font = new System.Drawing.Font(BackPanel.font1, (int)(Width * 0.015 + 5), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            Media_label.Font = new System.Drawing.Font(BackPanel.font1, (int)(Width * 0.015 + 5), System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            MediaPicker.ShowUpDown = true;
            Chart.ChartAreas[0].Position.Width = 100;
            Chart.ChartAreas[0].Position.Height = 100;
        }
        private void ResizeData()
        {
            Date.Location = new Point( 0 , Chart.Location.Y + Chart.Height);
            Date.Size = new Size(Chart_Panel.Width - Date.Location.X - (int)((100 - Chart.ChartAreas[0].InnerPlotPosition.Width) * Chart_Panel.Width) / 100, (int)(Chart_Panel.Height * 0.2));
        }
        public void ClickNull()
        {
            FinestraPrincipale.BackPanel.Panel_Ricerca.ClickNull();
        }
        public void ClickNull(object sender, EventArgs e)
        {
            ClickNull();
        }
        long delta_datacode;
        long datacode_min_standard, datacode_max_standard;
        long datacode_min = 0, datacode_max = 0;
        long delta_datacode_var;

        public void AcquisizioneDati(object sender, EventArgs e)
        {
            acquisizione_dati.Tick -= AcquisizioneDati;
            if (PanelRicerca.resize_datacode)
            {
                foreach (Series serie in Chart.Series) serie.Points.Clear();
                if (MediaPicker.Value.Minute == 1)
                {
                    foreach (Eventi evento in FinestraPrincipale.BackPanel.Panel_Ricerca.eventi_filtrati)
                    {
                        if (evento.Get_Attributo() == "Introito") Chart.Series[2].Points.AddXY(evento.GetDatacode(), evento.GetValore());
                        else if (evento.Get_Attributo() == "Spesa") Chart.Series[1].Points.AddXY(evento.GetDatacode(), evento.GetValore());
                        else if (evento.Get_Attributo() == "Trasferimento") Chart.Series[0].Points.AddXY(evento.GetDatacode(), evento.GetValore());
                    }
                }
                else CaricaEventiMediati();
                


                media = MediaPicker.Value.Minute;
                datacode_min = FinestraPrincipale.BackPanel.Panel_Ricerca.datacode1;
                datacode_max = FinestraPrincipale.BackPanel.Panel_Ricerca.datacode2;

                delta_datacode = datacode_max - datacode_min;
                delta_datacode_var = delta_datacode;
                datacode_min_standard = datacode_min;
                datacode_max_standard = datacode_max;
            }
            else
            {
                foreach (Series serie in Chart.Series) { serie.Enabled = false; }
                if (PanelRicerca.attributo == "Introito") Chart.Series[2].Enabled = true;
                if (PanelRicerca.attributo == "Spesa") Chart.Series[1].Enabled = true;
                if (PanelRicerca.attributo == "Trasferimento") Chart.Series[0].Enabled = true;
                if (PanelRicerca.attributo == "All") foreach (Series serie in Chart.Series) { serie.Enabled = true; }

                PanelRicerca.resize_datacode = true;
            }
            ResizeForm();

            
            ResizeAxisX(datacode_min, datacode_max);
            //if (datacode_max_standard - datacode_min_standard > 86400 * 365) CheckSpaziatura(1); else CheckSpaziatura(2);
        }
        private void CaricaEventiMediati()
        {
            long giorno_in = FinestraPrincipale.BackPanel.Panel_Ricerca.datacode1;
            if (giorno_in % 86400 != 0) giorno_in -= giorno_in % 86400;
            long giorno_fin = FinestraPrincipale.BackPanel.Panel_Ricerca.datacode2;
            if (giorno_fin % 86400 != 0) giorno_in += 86400 - giorno_fin % 86400;
            int num_giorni = (int)((double)(giorno_fin - giorno_in) / 86400) + 1;


            double mu = 0;
            double sigma = (MediaPicker.Value.Minute-2+0.2);
            double[] punti_gaus = new double[8 * (int)sigma + 1];
            for (int i = (int)mu - 4 * (int)sigma; i < mu + 4 * (int)sigma + 1; i += 1)
            {
                punti_gaus[i + 4 * (int)sigma] = (gauss(i, mu, sigma));
            }
            double resto = (1 - punti_gaus.Sum()) / (8 * (int)sigma + 1);
            for (int i = 0; i < 8 * (int)sigma + 1; i += 1)
            {
                punti_gaus[i] += resto;
            }


            double[] introiti_spuri = new double[num_giorni];
            double[] spese_spuri = new double[num_giorni];
            double[] trasferimenti_spuri = new double[num_giorni];
            double[] introiti = new double[num_giorni];
            double[] spese = new double[num_giorni];
            double[] trasferimenti = new double[num_giorni];
            for (int i = 0; i < num_giorni; i++)
            {
                introiti_spuri[i] = 0;
                spese_spuri[i] = 0;
                trasferimenti_spuri[i] = 0;
                introiti[i] = 0;
                spese[i] = 0;
                trasferimenti[i] = 0;
            }

            foreach (Eventi evento in FinestraPrincipale.BackPanel.Panel_Ricerca.eventi_filtrati)
            {
                int j = (int)((double)(evento.GetDatacode() - giorno_in) / 86400);
                if (evento.Get_Attributo() == "Introito") introiti_spuri[j] += evento.GetValore();
                else if (evento.Get_Attributo() == "Spesa") spese_spuri[j] += evento.GetValore();
                else if (evento.Get_Attributo() == "Trasferimento") trasferimenti_spuri[j] += evento.GetValore();
            }

            for (int i = 0; i < num_giorni; i++)
            {
                for(int j=-4 * (int)sigma; j< 4 * (int)sigma + 1; j++)
                {
                    if (i + j >= 0 && i + j < num_giorni)
                    {
                        introiti[i + j] += introiti_spuri[i] * punti_gaus[j + 4 * (int)sigma];
                        spese[i + j] += spese_spuri[i] * punti_gaus[j + 4 * (int)sigma];
                        trasferimenti[i + j] += trasferimenti_spuri[i] * punti_gaus[j + 4 * (int)sigma];
                    }
                }
            }




                for (int i=0; i< num_giorni; i++)
            {
                if (sigma > 2)
                {
                    Chart.Series[2].Points.AddXY(i * 86400 + 42000 + giorno_in, introiti[i]);
                    Chart.Series[1].Points.AddXY(i * 86400 + 42000 + giorno_in, spese[i]);
                    Chart.Series[0].Points.AddXY(i * 86400 + 42000 + giorno_in, trasferimenti[i]);
                }
                else
                {
                    Chart.Series[2].Points.AddXY(i * 86400 + 42000 + giorno_in, introiti_spuri[i]);
                    Chart.Series[1].Points.AddXY(i * 86400 + 42000 + giorno_in, spese_spuri[i]);
                    Chart.Series[0].Points.AddXY(i * 86400 + 42000 + giorno_in, trasferimenti_spuri[i]);
                }
            }
        }
        double gauss(double x, double mu, double sigma)
        {
            var v1 = (x - mu);
            double a = 1 / (sigma * Math.Sqrt(2*Math.PI));
            var v2 = (v1 * v1) / (2 * (sigma * sigma));
            var v3 = a * Math.Exp(-v2);
            return v3;
        }

        void WheelMouse(object sender, MouseEventArgs e)
        {
            long centro = (long)Chart.ChartAreas[0].CursorX.Position;
            if (e.Delta > 0) MouseWheelUp(centro);
            else if (e.Delta < 0) MouseWheelDown(centro);
        }
        int limite_zoom = 5;
        void MouseWheelUp(long centro)
        {
            if (delta_datacode_var <= limite_zoom * 86400) return;
            datacode_min = (long)Chart.ChartAreas[0].AxisX.Minimum;
            datacode_max = (long)Chart.ChartAreas[0].AxisX.Maximum;
            double percentage = (double)(centro - datacode_min) / (double)(datacode_max - datacode_min);

            if (delta_datacode_var > limite_zoom * 86400) delta_datacode_var = (long)(delta_datacode_var * 0.8); else delta_datacode_var = 86400 * 10;
            datacode_min = centro - delta_datacode_var / 2;
            datacode_max = centro + delta_datacode_var / 2;
            datacode_min = (int)(datacode_min / 86400) * 86400;
            if (datacode_max % 86400 != 0) datacode_max += 86400;
            datacode_max = (int)(datacode_max / 86400) * 86400;
            delta_datacode_var = (int)(delta_datacode_var / 86400) * 86400;
            percentage = 0.5 - percentage;
            datacode_min = datacode_min + (long)(percentage * (double)(delta_datacode_var + 1.0 * 86400));
            datacode_max = datacode_max + (long)(percentage * (double)(delta_datacode_var - 1.0 * 86400));
            if (datacode_min < datacode_min_standard) datacode_min = datacode_min_standard;
            if (datacode_max > datacode_max_standard) datacode_max = datacode_max_standard;
            
            ResizeAxisX(datacode_min, datacode_max);
        }
        void MouseWheelDown(long centro)
        {
            datacode_min = (long)Chart.ChartAreas[0].AxisX.Minimum;
            datacode_max = (long)Chart.ChartAreas[0].AxisX.Maximum;
            double percentage = (double)(centro - datacode_min) / (double)(datacode_max - datacode_min);
            if (delta_datacode_var <= delta_datacode) delta_datacode_var = (long)(delta_datacode_var * 1.25);
            datacode_min = centro - delta_datacode_var / 2;
            datacode_max = centro + delta_datacode_var / 2;
            if (delta_datacode_var > delta_datacode) { delta_datacode_var = delta_datacode; datacode_min = 0; datacode_max = 999999999999999; }
            //if (delta_datacode_var <= limite_zoom * 86400) { datacode_min = centro - 86400 * limite_zoom; datacode_max = centro + 86400 * limite_zoom; }
            datacode_min = (int)(datacode_min / 86400) * 86400;
            if (datacode_max % 86400 != 0) datacode_max += 86400;
            datacode_max = (int)(datacode_max / 86400) * 86400;
            delta_datacode_var = (int)(delta_datacode_var / 86400) * 86400;
            percentage = 0.5 - percentage;
            datacode_min = datacode_min + (long)(percentage * (double)(delta_datacode_var + 1.0 * 86400));
            datacode_max = datacode_max + (long)(percentage * (double)(delta_datacode_var - 1.0 * 86400));
            if (datacode_min > datacode_max) { datacode_min = datacode_min_standard; datacode_max = datacode_max_standard; }
            if (datacode_min < datacode_min_standard) datacode_min = datacode_min_standard;
            if (datacode_max > datacode_max_standard) datacode_max = datacode_max_standard;

            ResizeAxisX(datacode_min, datacode_max);
        }
        private void DateDelay(object sender, EventArgs e)
        {
            Date_delay.Tick -= DateDelay;
            ResizeData();
            if(datacode_max>datacode_min) Date.Aggiorna(datacode_min, datacode_max);
        }
        public void DateDelayInitial(object sender, EventArgs e)
        {
            Date.SuspendLayout();
            Date.BringToFront();
            ResizeAxisX(datacode_min, datacode_max);
            /*ResizeAxisX(datacode_min_standard, datacode_max_standard);
            datacode_min = datacode_min_standard;
            datacode_max = datacode_max_standard;
            datacode_min = datacode_min_standard;
            datacode_max = datacode_max_standard;
            delta_datacode_var = datacode_max - datacode_min;*/
            Date.ResumeLayout();
            if (datacode_max_standard > datacode_min_standard) Date.Aggiorna(datacode_min_standard, datacode_max_standard);
            Date_delay_initial.Tick -= DateDelayInitial;
            CheckSpaziatura(0);
        }
        private void ResizeAxisX(long datacode_min, long datacode_max)
        {
            Chart.ChartAreas[0].AxisX.Minimum = datacode_min;
            Chart.ChartAreas[0].AxisX.Maximum = datacode_max;
            CheckSpaziatura(0);

            Date_delay.Tick += DateDelay;
        }
        private void ResizeAxisY2(int i)
        {/*
            double max = 0;
            foreach (Eventi evento in FinestraPrincipale.BackPanel.Panel_Ricerca.eventi_filtrati)
            {
                if (evento.datacode >= datacode_min && evento.datacode <= datacode_max)
                {
                    if (evento.valore > max) max = evento.valore;
                }
            }
            Chart.ChartAreas[0].AxisY2.Maximum = max * 1.1;
            */
            if (i > 0) Chart.ChartAreas[0].AxisY2.Maximum = Chart.ChartAreas[0].AxisY2.Maximum * 1.25;
            else Chart.ChartAreas[0].AxisY2.Maximum = Chart.ChartAreas[0].AxisY2.Maximum * 0.8;

            if (i > 0) Chart.ChartAreas[0].AxisY.Maximum = Chart.ChartAreas[0].AxisY.Maximum * 1.25;
            else Chart.ChartAreas[0].AxisY.Maximum = Chart.ChartAreas[0].AxisY.Maximum * 0.8;

            Date_delay.Tick += DateDelay;

        }
        private void CheckSpaziatura(int op)
        {
            if (op == 1)
            {
                series1.CustomProperties = "DrawingStyle=Wedge, PixelPointWidth=8";
                series2.CustomProperties = "DrawingStyle=Wedge, PixelPointWidth=8";
                series3.CustomProperties = "DrawingStyle=Wedge, PixelPointWidth=8";
                return;
            }
            if (op == 2)
            {
                series1.CustomProperties = "DrawingStyle=Wedge, PointWidth=5000";
                series2.CustomProperties = "DrawingStyle=Wedge, PointWidth=5000";
                series3.CustomProperties = "DrawingStyle=Wedge, PointWidth=5000";
                return;
            }
            //if (Chart.ChartAreas[0].AxisX.Maximum - Chart.ChartAreas[0].AxisX.Minimum > 86400 * 365)
            //{
            string side = "DrawSideBySide=";
            string point = ", PixelPointWidth=";
            string wedge = ", DrawingStyle=Wedge";
            long punto = (long)(double)((365*2* 86400)/ (datacode_max-datacode_min)) + 5;
            if (media > 2) { punto = (long)(double)((365 * 20 * 86400) / (datacode_max - datacode_min)) + 5; wedge = ""; }
            if (punto > (int)(Width*0.1)) punto = (int)(Width * 0.1);
            if (media == 2) side += "True"; else side += "False";
            point += punto.ToString();
            series1.CustomProperties = side + point + wedge;
            series2.CustomProperties = side + point + wedge;
            series3.CustomProperties = side + point + wedge;
        }
        double percentage_spostamento = 0.1;
        private void IndietroClick(object sender, EventArgs e)
        {
            if (datacode_min >= datacode_min_standard + (long)((double)(datacode_max - datacode_min) * percentage_spostamento))
            {
                long spost = (long)((double)(datacode_max - datacode_min) * percentage_spostamento);
                datacode_min -= spost;
                datacode_max -= spost;
                ResizeAxisX(datacode_min, datacode_max);
            }
            else
            {
                long spost = datacode_min - datacode_min_standard;
                datacode_min -= spost;
                datacode_max -= spost;
                ResizeAxisX(datacode_min, datacode_max);
            }
        }
        private void AvantiClick(object sender, EventArgs e)
        {
            if (datacode_max <= datacode_max_standard - (long)((double)(datacode_max - datacode_min) * percentage_spostamento))
            {
                long spost = (long)((double)(datacode_max - datacode_min) * percentage_spostamento);
                datacode_min += spost;
                datacode_max += spost;
                ResizeAxisX(datacode_min, datacode_max);
            }
            else
            {
                long spost = datacode_max_standard - datacode_max;
                datacode_min += spost;
                datacode_max += spost;
                ResizeAxisX(datacode_min, datacode_max);
            }
        }
        private void SuClick(object sender, EventArgs e)
        {
            ResizeAxisY2(-1);
        }
        private void GiuClick(object sender, EventArgs e)
        {
            ResizeAxisY2(1);
        }
        private void KeyControl(object sender, KeyEventArgs e)
        {
            if (!Visible) return;
            if (e.KeyCode == Keys.Left) IndietroClick(null, null);
            if (e.KeyCode == Keys.Right) AvantiClick(null, null);
            if (e.KeyCode == Keys.Up) ResizeAxisY2(-1);
            if (e.KeyCode == Keys.Down) ResizeAxisY2(1);
        }
        private void Data_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePoint = new Point(e.X, e.Y);
            Chart.ChartAreas[0].CursorX.SetCursorPixelPosition(mousePoint, true);
            Chart.ChartAreas[0].CursorY.SetCursorPixelPosition(mousePoint, true);
            //if (e.Button == MouseButtons.Left) Console.WriteLine("CLIKKATO:   " + (long)Chart.ChartAreas[0].CursorX.Position + "            " + (double)((long)Chart.ChartAreas[0].CursorX.Position - datacode_min)/ (double)(datacode_max - datacode_min)); 
        }
        private void Data_MouseEnter(object sender, EventArgs e)
        {
            Chart.ChartAreas[0].CursorX.LineWidth = 1;
            Chart.ChartAreas[0].CursorY.LineWidth = 1;
        }
        private void Data_MouseLeave(object sender, EventArgs e)
        {
            Chart.ChartAreas[0].CursorX.LineWidth = 0;
            Chart.ChartAreas[0].CursorY.LineWidth = 0;
        }



        float largh = 0.02F;
        float alt = 0.03F;
        float alt_button = 0.67F;
        float posX = 0.92F;

        private void Indietro_Small(object sender, EventArgs e)
        {
            Indietro.Location = new Point((int)(Chart_Panel.Width - (int)(Chart_Panel.Width * largh * 3)), (int)(Chart_Panel.Height * alt_button));
            Indietro.Size = new Size((int)(Chart_Panel.Width * largh), (int)(Chart_Panel.Width * alt));
        }
        private void Avanti_Small(object sender, EventArgs e)
        {
            Avanti.Location = new Point((int)(Chart_Panel.Width * (posX+2*alt)), (int)(Chart_Panel.Height * alt_button));
            Avanti.Size = new Size((int)(Chart_Panel.Width * largh), (int)(Chart_Panel.Width * alt));
        }
        private void Indietro_Big(object sender, EventArgs e)
        {
            Indietro.Location = new Point((int)(Chart_Panel.Width - (int)(Chart_Panel.Width * largh * 3)) - 1, (int)(Chart_Panel.Height * alt_button) - 1);
            Indietro.Size = new Size((int)(Chart_Panel.Width * largh) + 2, (int)(Chart_Panel.Width * alt) + 2);
        }
        private void Avanti_Big(object sender, EventArgs e)
        {
            Avanti.Location = new Point((int)(Chart_Panel.Width * (posX+2*alt)) - 1, (int)(Chart_Panel.Height * alt_button) - 1);
            Avanti.Size = new Size((int)(Chart_Panel.Width * largh) + 2, (int)(Chart_Panel.Width * alt) + 2);
        }

        private void Su_Small(object sender, EventArgs e)
        {
            Su.Size = new Size((int)(Chart_Panel.Width * alt), (int)(Chart_Panel.Width * largh));
            Su.Location = new Point((int)((Avanti.Location.X + Avanti.Width - Indietro.Location.X)/2 + Indietro.Location.X - (int)(Chart_Panel.Width * alt)/2), (int)(Chart_Panel.Height * alt_button) - (int)(Chart_Panel.Width * largh));
        }
        private void Giu_Small(object sender, EventArgs e)
        {
            Giu.Size = new Size((int)(Chart_Panel.Width * alt), (int)(Chart_Panel.Width * largh));
            Giu.Location = new Point((int)((Avanti.Location.X + Avanti.Width - Indietro.Location.X) / 2 + Indietro.Location.X - (int)(Chart_Panel.Width * alt) / 2), (int)(Chart_Panel.Height * alt_button) + (int)(Chart_Panel.Width * alt));
        }
        private void Su_Big(object sender, EventArgs e)
        {
            Su.Size = new Size((int)(Chart_Panel.Width * alt) + 2, (int)(Chart_Panel.Width * largh) + 2);
            Su.Location = new Point((int)((Avanti.Location.X + Avanti.Width - Indietro.Location.X) / 2 + Indietro.Location.X - (int)(Chart_Panel.Width * alt) / 2)  - 1, (int)(Chart_Panel.Height * alt_button) - (int)(Chart_Panel.Width * largh) - 1);
        }
        private void Giu_Big(object sender, EventArgs e)
        {
            Giu.Size = new Size((int)(Chart_Panel.Width * alt) + 2, (int)(Chart_Panel.Width * largh) + 2);
            Giu.Location = new Point((int)((Avanti.Location.X + Avanti.Width - Indietro.Location.X) / 2 + Indietro.Location.X - (int)(Chart_Panel.Width * alt) / 2) - 1, (int)(Chart_Panel.Height * alt_button) + (int)(Chart_Panel.Width * alt) - 1);
        }
    }
}
