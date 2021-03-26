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
            public bool PopUps { get; set; }
            public bool Eliminar { get; set; }
            public bool BackUPs { get; set; }
            public bool Renombrar { get; set; }
            public bool PrimeraVez { get; set; }
            public string Entorno { get; set; }
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
                await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/PopUps", true);
                await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/Eliminar", false);
                await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/BackUPs", false);
                await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/Renombrar", false);
                await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/Entorno", "CN");
                await Conexion.Cont().SetAsync(Environment.UserName + "/Configuracion/PrimeraVez", true);

                response = await Conexion.Cont().GetAsync(Environment.UserName + "/Configuracion");
                tempConf = response.ResultAs<Config>();
            }

            return tempConf;
        }


    }
}
