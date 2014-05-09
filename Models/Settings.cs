using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SelfDC.Models
{
    class Settings
    {
        public string CodiceCliente;        //  default: 000000         Codice del cliente
        public string OrdineFileName;       //	default: ordine.txt	    Nome del file ordine
        public string InventarioFilename;   //	default: inventa.txt	Nome del file dati Inventario
        public string EtichettaFilename;    //	default: etichet.txt	Nome del file dati etichette

        public Settings()
        {
            CodiceCliente = "000000";
            OrdineFileName = "ordine.txt";
            InventarioFilename = "inventa.txt";
            EtichettaFilename = "etichet.txt";
        }

        public void LoadFromFile(string FileName)
        {
            if (File.Exists(FileName))
            {
                StreamReader sr = new StreamReader(FileName);
                CodiceCliente = sr.ReadLine();
                OrdineFileName = sr.ReadLine();
                InventarioFilename = sr.ReadLine();
                EtichettaFilename = sr.ReadLine();
                sr.Close();
            }
        }

        public void SaveToFile(string FileName)
        {
            StreamWriter sw = new StreamWriter(FileName);
            sw.WriteLine(CodiceCliente);
            sw.WriteLine(OrdineFileName);
            sw.WriteLine(InventarioFilename);
            sw.WriteLine(EtichettaFilename);
            sw.Close();
        }
    }
}
