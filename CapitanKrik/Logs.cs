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





        public static async Task<List<Log>> GetLogItems()
        {
            List<Log> ListLogs = new List<Log>();

            FirebaseResponse responsed = await Conexion.Cont().GetAsync(Environment.UserName + "/Logs");
            var conf = responsed.ResultAs<Dictionary<string, Log>>();

            if (conf != null)
            {
                foreach (KeyValuePair<string, Log> lo in conf.OrderBy(i => Int32.Parse(i.Key.Substring(3))))
                {
                    ListLogs.Add(new Log() { Mensaje = lo.Value.ToString() });
                }
            }

            

            return ListLogs;

        }

        public static async void SetLog(Log save)
        {
            FirebaseResponse responsed = await Conexion.Cont().GetAsync(Environment.UserName + "/Logs");
            var conf = responsed.ResultAs<Dictionary<string, Log>>();
            int temp = conf.Count()+1;
            await Conexion.Cont().SetAsync(Environment.UserName + "/Logs/Log" + temp + "/TipoLog", save.TipoLog);
            await Conexion.Cont().SetAsync(Environment.UserName + "/Logs/Log" + temp + "/Mensaje", save.Mensaje);
        }
    }
}
