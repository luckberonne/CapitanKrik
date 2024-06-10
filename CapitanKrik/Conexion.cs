using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp;
using System.Data.SqlClient;

namespace CapitanKrik
{
    public class Conexion
    {
        public static FirebaseClient Cont()
        {
            FirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "",
                BasePath = ""

            };

            FirebaseClient cliente = new FirebaseClient(config);
            return cliente;
        }


        public static void ConsultaCarp(Archivos.Archivo dato)
        {
            SqlConnection Cnn = BaseDatos();

            Cnn.Open();
            _ = new SqlCommand();

            string query = "";
            List<string> listInvoice = new List<string>() { "NCRE", "FACT", "NDEB", "NCMiPyME", "NDMiPyME" };
            List<string> listSolc = new List<string>() { "SOLNCRE", "SOLNDEB" };

            if (listInvoice.Any(s => s.Contains(dato.TipoDocumento.Remove(dato.TipoDocumento.Length - 1))))
            {
                query = String.Concat(@"SELECT top(1) Carpeta FROM [Pxw_Security25].[dbo].[Ta_AliasEmpresaCarpeta] T1
                where T1.AliasEmpresa = '", dato.Emisor, @"' AND T1.SentidoProceso = 'U' AND (Fe_CodigoDMS ='FACTURA' or Fe_CodigoDMS ='DEF_AFIPFE')");
            }
            else if (listSolc.Any(s => s.Contains(dato.TipoDocumento)))
            {
                query = String.Concat(@"SELECT top(1) Carpeta FROM [Pxw_Security25].[dbo].[Ta_AliasEmpresaCarpeta] T1
                where T1.AliasEmpresa = '", dato.Emisor, @"' AND T1.SentidoProceso = 'U' AND Fe_CodigoDMS ='DEF_SOLICITUD_NCND'");
            }
            else if (dato.TipoDocumento == "OrdenCompra")
            {
                query = String.Concat(@"SELECT top(1) Carpeta FROM [Pxw_Security25].[dbo].[Ta_AliasEmpresaCarpeta] T1
                where T1.AliasEmpresa = '", dato.Emisor, @"' AND T1.SentidoProceso = 'U' AND Fe_CodigoDMS ='ORDERS'");
            }
            //string query = String.Concat(@"SET NOCOUNT ON
            //    DECLARE @RESULT CHAR(50) = (SELECT top(1)  T1.Func_DMS FROM[DPTArgentina].[maestros].[ServiciosDMS_FuncionesSecurity25] T1
            //    INNER JOIN[DPTArgentina].[maestros].[ServiciosDMS] T2 ON T1.IdServicioDMS = T2.Id AND T1.Productivo = 1 AND T2.ResourceKey != 'LadrilloArg' AND T2.ResourceKey != 'Manual_Simplificado' AND T2.ResourceKey != 'PUBLICA_FAC'
            //    INNER JOIN[DPTArgentina].[dbo].[TiposDocumentoComprobantes] T3 ON T1.IdTipoDocumento = t3.IdTipoDocumento AND T3.ResourceKey = '", dato.TipoDocumento, @"')
            //    CREATE TABLE #serviciosIguales (Func_DMS VARCHAR(50))
            //    IF @RESULT = 'FACTURA' OR @RESULT = 'DEF_AFIPFE'
            //    BEGIN
            //    INSERT INTO #serviciosIguales
            //    VALUES('FACTURA'),('DEF_AFIPFE')
            //    END
            //    ELSE
            //    BEGIN
            //    INSERT INTO #serviciosIguales
            //    VALUES(@RESULT)
            //    END
            //    SELECT TOP(1) Carpeta FROM[Pxw_Security25].[dbo].[Ta_AliasEmpresaCarpeta] T1
            //    INNER JOIN #serviciosIguales T2 
            //    ON T1.Fe_CodigoDMS = T2.Func_DMS AND T1.AliasEmpresa = '", dato.Emisor, @"' AND T1.SentidoProceso = 'U'
            //    DROP TABLE #serviciosIguales");


            SqlCommand command = new SqlCommand(query, Cnn);
            dato.CarpetaSubida = (string)command.ExecuteScalar();
            Cnn.Close();

        }

        public static void Consulta(Archivos.Archivo dato)
        {
            SqlConnection Cnn = BaseDatos();
            try
            {
                Cnn.Open();

            }
            catch (Exception)
            {
            }
            _ = new SqlCommand();

            string query = string.Concat(@"SELECT T2.AliasEmpresa from [DPTArgentina].[maestros].[Identificadores] T1 
                INNER JOIN[DPTArgentina].[maestros].[Clientes] T2 ON T2.Id = T1.IdCliente AND T1.CodigoIdentificacion = '"
                , dato.Emisor, "' AND (T1.IdTipoIdentificacion = 2 OR T1.IdTipoIdentificacion = 5)");

            SqlCommand command = new SqlCommand(query, Cnn);
            dato.Emisor = (string)command.ExecuteScalar();


            query = String.Concat(@"SELECT T2.AliasEmpresa from [DPTArgentina].[maestros].[Identificadores] T1 
                INNER JOIN[DPTArgentina].[maestros].[Clientes] T2 ON T2.Id = T1.IdCliente AND T1.CodigoIdentificacion = '"
                , dato.Receptor, "' AND (T1.IdTipoIdentificacion = 2 OR T1.IdTipoIdentificacion = 5)");

            command = new SqlCommand(query, Cnn);
            dato.Receptor = (string)command.ExecuteScalar();

            query = String.Concat(@"SELECT top(1)  T3.ResourceKey FROM [DPTArgentina].[dbo].[TiposDocumentoComprobantes] T3 
                WHERE (SUBSTRING(T3.Codigo, PATINDEX('%[^0]%', T3.Codigo), LEN(T3.Codigo)) = '", dato.TipoDocumento, "' OR  T3.tipo_doc_edi = '", dato.TipoDocumento, "' OR T3.Tipo_Doc_XML = '", dato.TipoDocumento, "')");

            //query = String.Concat(@"SELECT top(1)  T3.ResourceKey  FROM [DPTArgentina].[maestros].[ServiciosDMS_FuncionesSecurity25] T1
            //    INNER JOIN[DPTArgentina].[maestros].[ServiciosDMS] T2 ON T1.IdServicioDMS = T2.Id
            //    AND T1.Productivo = 1 AND T2.ResourceKey != 'LadrilloArg'
            //    INNER JOIN[DPTArgentina].[dbo].[TiposDocumentoComprobantes] T3 ON T1.IdTipoDocumento = t3.IdTipoDocumento
            //    AND(SUBSTRING(T3.Codigo, PATINDEX('%[^0]%', T3.Codigo), LEN(T3.Codigo)) = '", dato.TipoDocumento, "' OR  T3.tipo_doc_edi = '", dato.TipoDocumento, "' OR T3.Tipo_Doc_XML = '", dato.TipoDocumento, "')");


            command = new SqlCommand(query, Cnn);
            dato.TipoDocumento = (string)command.ExecuteScalar();

            Cnn.Close();
        }

        public static SqlConnection BaseDatos()
        {
            SqlConnection cnn = null;
            try
            {
                if (MainWindow.TempConf.Entorno == "CN")
                {

                    cnn = new SqlConnection("");

                }
                else if (MainWindow.TempConf.Entorno == "QA")
                {
                    cnn = new SqlConnection("");
                }
            }
            catch (Exception)
            {

                throw;
            }

            return cnn;
        }


    }
}
