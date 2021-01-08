using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FireSharp.Config;
using FireSharp.Response;
using FireSharp;

namespace CapitanKrik
{
    public class Logs
    {

        public class Log
        {
            public string TipoLog { get; set; }
            public string Mensaje { get; set; }

            public override string ToString()
            {
                return this.TipoLog + "-" + this.Mensaje;
            }
        }


        public static List<Log> ListLogs = new List<Log>();

        public static async void GetLogItems()
        {
            FirebaseResponse responsed = await Conexion.Cont().GetAsync(Environment.UserName + "/Logs");
            var conf = responsed.ResultAs<Dictionary<string, Log>>();
            foreach (var item in conf)
            {
                ListLogs.Add(new Log() { Mensaje = item.Value.ToString() });

            }

        }

        public static List<Log> GetLogs()
        {
            GetLogItems();
            return ListLogs;
        }

    }
}
