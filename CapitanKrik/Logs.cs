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
            foreach (var item in conf)
            {
                ListLogs.Add(new Log() { Mensaje = item.Value.ToString() });
            }

            return ListLogs;

        }

        public static async void SetLog(Log save)
        {
            FirebaseResponse responsed = await Conexion.Cont().GetAsync(Environment.UserName + "/Logs");
            var conf = responsed.ResultAs<Dictionary<string, Log>>();
            int temp = 0;
            foreach (var item in conf)
            {
                if (Int32.Parse(item.Key.Substring(3)) > temp)
                {
                    temp = Int32.Parse(item.Key.Substring(3));
                }

            }
            temp++;
            await Conexion.Cont().SetAsync(Environment.UserName + "/Logs/Log" + temp + "/TipoLog", save.TipoLog);
            await Conexion.Cont().SetAsync(Environment.UserName + "/Logs/Log" + temp + "/Mensaje", save.Mensaje);
            MainWindow.AddLogList(save);
        }
    }
}
