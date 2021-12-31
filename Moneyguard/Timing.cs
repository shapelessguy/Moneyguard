using System;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Drawing;

namespace Moneyguard
{
    class Timing
    {
        private static int count = 0;
        /////////////////////////////////////////////////////////////////////////  Aspetta tot ms (System, System.IO)
        public static void Wait(int ms)
        {
            Timer timer = new Timer
            {
                Enabled = true,
                Interval = ms,
            };
            timer.Tick += Aspetta;

            void Aspetta(object sender, EventArgs e)
            {
                count++;
                timer.Dispose();
            }
        }
        public static void Wait_thenShowbuttons(int ms)
        {
            Timer timer = new Timer
            {
                Enabled = true,
                Interval = ms,
            };
            timer.Tick += Aspetta;

            void Aspetta(object sender, EventArgs e)
            {
                count++;
                foreach (Label button in FinestraPrincipale.BackPanel.StandardCalendar.button) button.Show();
                timer.Dispose();
            }
        }

    }
}
