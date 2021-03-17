using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitanKrik
{
    class ArchivoTXT
    {
        public static void MapearTXT(Archivos.Archivo item)
        {

            item.Extension = ".txt";
            foreach (var line in Archivos.Leer(item))
            {
                if (line.Substring(0, 3) == "010")
                {
                    List<string> listline = new List<string>(line.Split(';'));
                    item.TipoDocumento = listline[1].TrimStart(new Char[] { '0' });
                    item.NumeroDocumento = listline[2];
                }
                else if (line.Substring(0, 3) == "030")
                {
                    List<string> listline = new List<string>(line.Split(';'));
                    item.Emisor = listline[19];
                }
                else if (line.Substring(0, 3) == "040")
                {
                    List<string> listline = new List<string>(line.Split(';'));
                    item.Receptor = listline[21];


                    Conexion.Consulta(item);
                    Conexion.ConsultaCarp(item);

                    break;
                }
            }
            
        }
    }
}
