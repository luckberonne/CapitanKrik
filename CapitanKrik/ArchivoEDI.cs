using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitanKrik
{
    class ArchivoEDI
    {
        public static string[] ListEDI(Archivos.Archivo item)
        {
            StreamReader file = new StreamReader(Path.Combine(MainWindow.TempConf.CarpetaSubida, item.NombreArchivo));
            string[] lines = file.ReadLine().Split(new string[] { "'" }, StringSplitOptions.None);


            file.Close();
            return lines;
        }
        public static void MapearEDI(Archivos.Archivo item)
        {

            item.Extension = ".edi";
            foreach (var line in ListEDI(item))
            {
                if (line.Substring(0, 3) == "UNB")
                {
                    List<string> listline = new List<string>(line.Split('+'));
                    item.Emisor = listline[2].Split(':')[0];
                    item.Receptor = listline[3].Split(':')[0];
                }
                else if (line.Substring(0, 3) == "BGM")
                {
                    List<string> listline = new List<string>(line.Split('+'));
                    item.TipoDocumento = listline[1].Split(':')[0];
                    item.NumeroDocumento = listline[2];
                    break;

                }
            }

            Conexion.Consulta(item);
            Conexion.ConsultaCarp(item);
        }
    }
}
