using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FireSharp.Config;
using FireSharp.Response;
using FireSharp;
using System.IO;
using System.ComponentModel;

namespace CapitanKrik
{
    public class Configuracion
    {
        public class Config
        {
            public bool ProcesoEntrada { get; set; }
            public bool ProcesoSalida { get; set; }
            public string CarpetaSubida { get; set; }
            public string CarpetaBackUP { get; set; }
            public string CantidadArchivos { get; set; }
        }


        public static async Task<Config> GetConfig()
        {
            
            FirebaseResponse response = await Conexion.Cont().GetAsync(Environment.UserName + "/Configuracion");
            Config tempConf  = response.ResultAs<Config>();
            if (tempConf == null)
            {
                await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/CarpetaSubida", ".\\Archivos");
                await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/CarpetaBackUP", ".\\BackUPS");
                await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/CantidadArchivos", "1");
                await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/ProcesoEntrada", true);
                await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/ProcesoSalida", true);
                response = await Conexion.Cont().GetAsync(Environment.UserName + "/Configuracion");
                tempConf = response.ResultAs<Config>();
            }
            return tempConf;
        }


    }
}
