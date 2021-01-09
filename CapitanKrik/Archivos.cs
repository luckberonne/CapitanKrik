using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FireSharp.Config;
using FireSharp.Response;
using FireSharp;
using System.IO;

namespace CapitanKrik
{
    public class Archivos
    {
        public class Archivo
        {
            public string NombreArchivo { get; set; }
            public string TipoDocumento { get; set; }
            public string Emisor { get; set; }
            public string Receptor { get; set; }
            public string NumeroDocumento { get; set; }

            public override string ToString()
            {
                return this.NombreArchivo;
            }
        }


        public static async Task<List<Archivo>> GetListArchivos()
        {
            List<Archivo> ListArchivos = new List<Archivo>();
            FirebaseResponse responsed = await Conexion.Cont().GetAsync(Environment.UserName + "/Configuracion");
            Configuracion.Confg conf = responsed.ResultAs<Configuracion.Confg>();

            string[] allfiles = System.IO.Directory.GetFiles(conf.CarpetaSubida, "*.*", System.IO.SearchOption.AllDirectories);


            foreach (var item in allfiles)
            {
                FileInfo info = new FileInfo(item);

                ListArchivos.Add(new Archivo() { NombreArchivo = info.Name });

            }

            return ListArchivos;

        }

    }
}
