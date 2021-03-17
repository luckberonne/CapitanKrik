using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace CapitanKrik
{
    class ArchivoXML
    {
        public static void MapearXML(Archivos.Archivo item)
        {
            item.Extension = ".xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(Path.Combine(MainWindow.TempConf.CarpetaSubida, item.NombreArchivo));

            XmlElement root = doc.DocumentElement;
            item.Receptor = root.GetElementsByTagName("buyer").Item(0).ChildNodes.Item(2).InnerText;

            item.Emisor = root.GetElementsByTagName("seller").Item(0).ChildNodes.Item(3).InnerText;

            item.NumeroDocumento = root.GetElementsByTagName("invoiceIdentification").Item(0).ChildNodes.Item(0).InnerText;

            item.TipoDocumento = root.GetElementsByTagName("pay:invoice").Item(0).ChildNodes.Item(4).InnerText;


            Conexion.Consulta(item);
            Conexion.ConsultaCarp(item);


        }
    }
}
