using GestorAplicaciones.Models;
using GestorAplicaciones.Pages.Show;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Net.Security;

namespace GestorAplicaciones.Pages.Add
{
    public class AddAppModel : PageModel
    {
        public AppInfo appInfo = new AppInfo();
        public List<String> listServers = new List<String>();

        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            try
            {
                var connString = new ConnStr();
                String connectStr = connString.ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectStr))
                {
                    connection.Open();

                    String sqlSelectAllSeries = "SELECT serie FROM Servidor";

                    using (SqlCommand command = new SqlCommand(sqlSelectAllSeries, connection))
                    {
                        using (SqlDataReader readerProIds = command.ExecuteReader())
                        {
                            while (readerProIds.Read())
                            {
                                String series;

                                series = "" + readerProIds["serie"];

                                // Add the object to the list
                                listServers.Add(series);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + ex.ToString());
            }
        }

        public void OnPost()
        {
            appInfo.codigo = Request.Form["app-code"];
            appInfo.numPatente = Request.Form["app-patent-num"];
            appInfo.nombre = Request.Form["app-name"];
            appInfo.descripcion = Request.Form["app-desc"];
            appInfo.tipo = Request.Form["app-type"];
            appInfo.fechaProduccion = Request.Form["app-prod-date"];
            appInfo.fechaExpiraLicencia = Request.Form["app-linc-exp"];
            appInfo.codigoDepartamento = Request.Form["app-dept"];
            String server = Request.Form["app-server"];
            String serverRol = Request.Form["app-server-rol"];

            if (appInfo.codigo.Length == 0 || appInfo.numPatente.Length == 0 ||
                appInfo.nombre.Length == 0 || appInfo.descripcion.Length == 0 || appInfo.tipo.Length == 0 ||
                appInfo.codigoDepartamento.Length == 0 || server.Length == 0 || serverRol.Length == 0)
            {
                errorMessage = "Todos los campos deben tener informacion, solo las fechas pueden estar vacias";
                return;
            }

            // Save the new data
            try
            {
                var connString = new ConnStr();
                String connectStr = connString.ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectStr))
                {
                    connection.Open();

                    String sqlInsert = "INSERT INTO Aplicacion (codigo, numPatente, nombre, descripcion, tipo, fechaProduccion, fechaExpiraLicencia, codigoDepartamento) \r\nVALUES \r\n" +
                        "(@codigo, @numPatente, @nombre, @descripcion, @tipo, @fechaProduccion, @fechaExpiraLicencia, @codigoDepartamento)\n\t " +
                        "INSERT INTO ServidorXAplicacion (codigoAplicacion, serieServidor, rol) VALUES\n\t " +
                        "(@codigo, @serieServidor, @rol)";

                    using (SqlCommand command = new SqlCommand(sqlInsert, connection))
                    {
                        command.Parameters.AddWithValue("@codigo", appInfo.codigo);
                        command.Parameters.AddWithValue("@numPatente", appInfo.numPatente);
                        command.Parameters.AddWithValue("@nombre", appInfo.nombre);
                        command.Parameters.AddWithValue("@descripcion", appInfo.descripcion);
                        command.Parameters.AddWithValue("@tipo", appInfo.tipo);
                        command.Parameters.AddWithValue("@fechaProduccion", appInfo.fechaProduccion);
                        command.Parameters.AddWithValue("@fechaExpiraLicencia", appInfo.fechaExpiraLicencia);
                        command.Parameters.AddWithValue("@codigoDepartamento", appInfo.codigoDepartamento);
                        command.Parameters.AddWithValue("@serieServidor", server);
                        command.Parameters.AddWithValue("@rol", serverRol);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            // Clear the fileds/attributes data
            appInfo.codigo = "";
            appInfo.numPatente = "";
            appInfo.nombre = "";
            appInfo.descripcion = "";
            appInfo.tipo = "";
            appInfo.fechaProduccion = "";
            appInfo.fechaExpiraLicencia = "";
            appInfo.codigoDepartamento = "";
            server = "";
            serverRol = "";

            successMessage = "La aplicacion se agrego con exito a la base de datos";

            Response.Redirect("/Add/AddApp");
        }
    }
}
