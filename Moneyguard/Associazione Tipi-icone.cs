using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication1;

namespace Moneyguard
{
    class Associazione
    {

        public static string IconaAssociata(string tipo)
        {
            for (int i = 0; i < Input.tipi.Count(); i++)
            {
                if (Input.tipi[i] == tipo) return Input.tipi_icons[i];
            }
            return "no_icon";
        }
        public static string TipoAssociato(string tipo_icon)
        {
            for (int i = 0; i < Input.tipi.Count(); i++)
            {
                if (Input.tipi_icons[i] == tipo_icon) return Input.tipi[i];
            }
            return "no_tipo";
        }
        public static string MiconaAssociata(string metodo)
        {
            for (int i = 0; i < Input.metodi.Count(); i++)
            {
                if (Input.metodi[i] == metodo) return Input.metodi_icons[i];
            }
            return "no_icon";
        }
        public static string MetodoAssociato(string metodo_icon)
        {
            for (int i = 0; i < Input.metodi.Count(); i++)
            {
                if (Input.metodi_icons[i] == metodo_icon) return Input.metodi[i];
            }
            return "no_metodo";
        }
        public static string AiconaAssociata(string attributo)
        {
            for (int i = 0; i < Input.attributi.Count(); i++)
            {
                if (Input.attributi[i] == attributo) return Input.attributi_icons[i];
            }
            return "no_icon";
        }
        public static string CodificaAttributo(string attributo)
        {
            string stringa = "";
            if (attributo == "Introito") stringa = "1";
            if (attributo == "Spesa") stringa = "2";
            if (attributo == "Trasferimento") stringa = "3";
            if (attributo == "Note") stringa = "4";
            return stringa;
        }
        public static string DecodificaAttributo(string attributo)
        {
            string stringa = "";
            if (attributo == "1") stringa = "Introito";
            if (attributo == "2") stringa = "Spesa";
            if (attributo == "3") stringa = "Trasferimento";
            if (attributo == "4") stringa = "Note";
            return stringa;
        }
        public static string CodificaMetodo(string metodo)
        {
            string stringa = "";
            for (int i = 0; i < Input.metodi.Count; i++)
            {
                if (Funzioni_utili.Scremato(metodo) == Funzioni_utili.Scremato(Input.metodi[i])) stringa = i.ToString();
            }
            return stringa;
        }
        public static string DecodificaMetodo(string metodo)
        {
            string stringa = "";
            for (int i = 0; i < Input.metodi.Count; i++)
            {
                if (Convert.ToInt32(metodo) == i) stringa = Input.metodi[i];
            }
            return stringa;
        }
        public static string CodificaTipo(string tipo)
        {
            string stringa = "";
            for (int i = 0; i < Input.tipi.Count; i++)
            {
                if (Funzioni_utili.Scremato(tipo) == Funzioni_utili.Scremato(Input.tipi[i])) stringa = i.ToString();
            }
            return stringa;
        }
        public static string DecodificaTipo(string tipo)
        {
            string stringa = "";
            for (int i = 0; i < Input.tipi.Count; i++)
            {
                if (Convert.ToInt32(tipo) == i) stringa = Input.tipi[i];
            }
            return stringa;
        }
    }
}
