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
        // Attributes to populate information on the website and to get the user input to update the DB
        public AppInfo appInfo = new AppInfo();
        public List<String> listServers = new List<String>();
        public List<String> listDept = new List<String>();

        public String errorMessage = "";
        public String successMessage = "";

        // Method to get information from the DB and use it on the website
        public void OnGet()
        {
            try
            {
                // Use the connection String to connect the web site to the DB
                var connString = new ConnStr();
                String connectStr = connString.ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectStr))
                {
                    connection.Open();

                    // Queries to be use
                    String sqlSelectAllSeries = "SELECT serie FROM Servidor";
                    String sqlSelectAllDepts = "SELECT codigo FROM Departamento";

                    using (SqlCommand command = new SqlCommand(sqlSelectAllSeries, connection))
                    {
                        // Execute the query to create a list of all server series ids
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

                    using (SqlCommand command = new SqlCommand(sqlSelectAllDepts, connection))
                    {
                        // Execute the query to create a list of all department codes
                        using (SqlDataReader readerDepts = command.ExecuteReader())
                        {
                            while (readerDepts.Read())
                            {
                                String code;

                                code = "" + readerDepts["codigo"];

                                // Add the object to the list
                                listDept.Add(code);
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

        // Method to sent information to the DB
        public void OnPost()
        {
            // Asign the data from the website input into an object/variables
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

            // Verify that the necessary fileds have information
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

                    // Query to send/edit the data to the DB
                    String sqlInsert = "INSERT INTO Aplicacion (codigo, numPatente, nombre, descripcion, tipo, fechaProduccion, fechaExpiraLicencia, codigoDepartamento) \r\nVALUES \r\n" +
                        "(@codigo, @numPatente, @nombre, @descripcion, @tipo, @fechaProduccion, @fechaExpiraLicencia, @codigoDepartamento)\n\t " +
                        "INSERT INTO ServidorXAplicacion (codigoAplicacion, serieServidor, rol) VALUES\n\t " +
                        "(@codigo, @serieServidor, @rol)";

                    using (SqlCommand command = new SqlCommand(sqlInsert, connection))
                    {
                        // Add the data from the input to the query parameters
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

                        // Execute the query
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
