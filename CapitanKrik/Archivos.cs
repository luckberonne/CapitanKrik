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
    public class Archivos
    {
        public class Archivo
        {
            public string NombreArchivo { get; set; }
            public string NombreArchivoViejo { get; set; }
            public string TipoDocumento { get; set; }
            public string Emisor { get; set; }
            public string Receptor { get; set; }
            public string NumeroDocumento { get; set; }
            public string Extension { get; set; }
            public string CarpetaSubida { get; set; }

            public bool IsChecked { get; set; }


            public override string ToString()
            {
                return this.NombreArchivo;
            }
        }


        public static BindingList<Archivo> GetListArchivos()
        {

            BindingList<Archivo> ListArchivos = new BindingList<Archivo>();
            string[] allfiles = null;
            

            try
            {
                allfiles = System.IO.Directory.GetFiles(MainWindow.TempConf.CarpetaSubida, "*.*", SearchOption.AllDirectories);


            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                foreach (var item in allfiles)
                {
                    FileInfo info = new FileInfo(item);

                    ListArchivos.Add(new Archivo() { NombreArchivoViejo = info.Name, NombreArchivo = info.Name });

                }
            }

            return ListArchivos;

        }




        public static List<string> Leer(Archivo item)
        {
            string line;
            StreamReader file = new StreamReader(Path.Combine(MainWindow.TempConf.CarpetaSubida, item.NombreArchivo));
                List<string> contenido = new List<string>();
                while ((line = file.ReadLine()) != null)
                {
                    contenido.Add(line);
                }
                file.Close();

            return contenido;
        }

        public static void Elegir()
        {
            foreach (var item in MainWindow.ListArchivos)
            {
                if (item.IsChecked)
                {
                    StreamReader file = new StreamReader(Path.Combine(MainWindow.TempConf.CarpetaSubida, item.NombreArchivo));
                    string line = file.ReadLine();
                    if (line != null)
                    {
                        if (int.TryParse(line.Substring(0, 3), out _))
                        {
                            ArchivoTXT.MapearTXT(item);
                        }
                        else if (line.Substring(0, 1) == "<")
                        {
                            ArchivoXML.MapearXML(item);
                        }
                        //else if (line.Substring(0, 2) == "UN")
                        //{
                        //    ArchivoEDI.MapearEDI(item);
                        //}
                        file.Close();
                    }
                }

            }
        }

        public static void Subir()
        {
            foreach (var item in MainWindow.ListArchivos)
            {
                if (item.IsChecked)
                {
                    File.Copy(Path.Combine(MainWindow.TempConf.CarpetaSubida, item.NombreArchivo), Path.Combine(item.CarpetaSubida, item.NombreArchivo), true);
                }
            }
        }

        public static void Renombrar()
        {
            foreach (var item in MainWindow.ListArchivos)
            {
                if (item.IsChecked)
                {
                    item.NombreArchivo = item.TipoDocumento + "_" + item.Emisor + "_" + item.Receptor + "_" + item.NumeroDocumento + item.Extension;
                    File.Move(Path.Combine(MainWindow.TempConf.CarpetaSubida, item.NombreArchivoViejo), Path.Combine(MainWindow.TempConf.CarpetaSubida, item.NombreArchivo));
                }
            }
        }


        public static void Delete()
        {
            foreach (var item in MainWindow.ListArchivos)
            {
                if (item.IsChecked)
                {
                    File.Delete(Path.Combine(MainWindow.TempConf.CarpetaSubida, item.NombreArchivo));
                }

            }
        }

        public static void BackUp()
        {
            foreach (var item in MainWindow.ListArchivos)
            {
                if (item.IsChecked)
                {
                    File.Copy(Path.Combine(MainWindow.TempConf.CarpetaSubida, item.NombreArchivo), Path.Combine(MainWindow.TempConf.CarpetaBackUP, item.NombreArchivo), true);
                }
            }
        }



        public static void Procesos()
        {
            int temp = 0;
            if (MainWindow.TempConf.ProcesoEntrada)
            {
                while (MainWindow.ListArchivos.Count() != temp)
                {
                    foreach (var item in MainWindow.ListArchivos)
                    {
                        if (Directory.GetFiles(Path.Combine(item.CarpetaSubida, item.NombreArchivo), "*", SearchOption.AllDirectories).Length == 0)
                        {
                            temp++;
                        }
                    }
                }
                if (MainWindow.ListArchivos.Count() == temp)
                {

                }
            }
            if (MainWindow.TempConf.ProcesoSalida)
            {

            }
        }
    }
}
