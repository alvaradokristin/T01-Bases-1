using GestorAplicaciones.Pages.Show;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Globalization;
using GestorAplicaciones.Models;

namespace GestorAplicaciones.Pages.Add
{
    public class AddErrorModel : PageModel
    {
        public ErrInfo errInfo = new ErrInfo();
        public String errorMessage = "";
        public String successMessage = "";

        // create a list of the info that we'll received from the database
        public List<ErrInfo> listErrors = new List<ErrInfo>();
        public List<AppxServer> listAppServer = new List<AppxServer>();
        public List<String> listProIds = new List<String>();
        public void OnGet()
        {
            try
            {
                var connString = new ConnStr();
                String connectStr = connString.ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectStr))
                {
                    connection.Open();

                    String sqlSelectAllErrors = "SELECT * FROM Error";
                    String sqlSelectAllSerxApp = "SELECT * FROM ServidorXAplicacion";
                    String sqlSelectAllPro = "SELECT id FROM Proyecto";

                    // This allow us to execute the SQL query above
                    using (SqlCommand command = new SqlCommand(sqlSelectAllErrors, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            String tempString;
                            DateTime tempDateTime;

                            while (reader.Read())
                            {
                                ErrInfo getErrInfo = new ErrInfo();

                                // Create an appInfo onject with the info from the db
                                getErrInfo.id = "" + reader["id"];

                                getErrInfo.descripcion = "" + reader["descripcion"];

                                tempString = "" + reader["fecha"]; // Convert what we get from DB into String
                                tempDateTime = DateTime.Parse(tempString); // Convert String to DateTime
                                getErrInfo.fecha = tempDateTime.ToShortDateString(); // Convert DateTime to String with format SmallDate

                                tempString = "" + reader["hora"];
                                tempDateTime = DateTime.Parse(tempString);
                                getErrInfo.hora = tempDateTime.ToString("hh:mm tt");

                                getErrInfo.impacto = "" + reader["impacto"];

                                getErrInfo.serieServidor = "" + reader["serieServidor"];

                                getErrInfo.codigoAplicacion = "" + reader["codigoAplicacion"];

                                getErrInfo.idProyecto = "" + reader["idProyecto"];

                                // Add the object to the list
                                listErrors.Add(getErrInfo);
                            }
                        }
                    }

                    using (SqlCommand commandAxS = new SqlCommand(sqlSelectAllSerxApp, connection))
                    {
                        using (SqlDataReader readerAxS = commandAxS.ExecuteReader())
                        {

                            while (readerAxS.Read())
                            {
                                AppxServer appServerInfo = new AppxServer();

                                // Create an appInfo onject with the info from the db
                                appServerInfo.codigoAplicacion = "" + readerAxS["codigoAplicacion"];

                                appServerInfo.serieServidor = "" + readerAxS["serieServidor"];

                                appServerInfo.rol = "" + readerAxS["rol"];

                                // Add the object to the list
                                listAppServer.Add(appServerInfo);
                            }
                        }
                    }

                    using (SqlCommand commandPro = new SqlCommand(sqlSelectAllPro, connection))
                    {
                        using (SqlDataReader readerPro = commandPro.ExecuteReader())
                        {

                            while (readerPro.Read())
                            {
                                String proId;

                                proId = "" + readerPro["id"];

                                // Add the object to the list
                                listProIds.Add(proId);
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
            errInfo.id = Request.Form["error-id"];
            errInfo.descripcion = Request.Form["error-desc"];
            errInfo.fecha = Request.Form["error-date"];
            errInfo.hora = Request.Form["error-hour"];
            errInfo.impacto = Request.Form["error-impact"];
            errInfo.serieServidor = Request.Form["error-server-series"];
            errInfo.codigoAplicacion = Request.Form["error-app-code"];
            errInfo.idProyecto = Request.Form["error-pro-id"];

            if (errInfo.id.Length == 0 || errInfo.descripcion.Length == 0 || errInfo.fecha.Length == 0
                 || errInfo.hora.Length == 0 || errInfo.impacto.Length == 0 || errInfo.serieServidor.Length == 0
                  || errInfo.codigoAplicacion.Length == 0)
            {
                errorMessage = "Todos los campos deben tener informacion, solo el id del proyecto puede ir vacio";
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

                    String sqlInsert = "INSERT INTO Error (id, descripcion, fecha, hora, impacto, serieServidor, codigoAplicacion, idProyecto)  \r\nVALUES \r\n" +
                        "(@id, @descripcion, @fecha, @hora, @impacto, @serieServidor, @codigoAplicacion, @idProyecto)";

                    using (SqlCommand command = new SqlCommand(sqlInsert, connection))
                    {
                        command.Parameters.AddWithValue("@id", errInfo.id);
                        command.Parameters.AddWithValue("@descripcion", errInfo.descripcion);
                        command.Parameters.AddWithValue("@fecha", errInfo.fecha);
                        command.Parameters.AddWithValue("@hora", errInfo.hora);
                        command.Parameters.AddWithValue("@impacto", errInfo.impacto);
                        command.Parameters.AddWithValue("@serieServidor", errInfo.serieServidor);
                        command.Parameters.AddWithValue("@codigoAplicacion", errInfo.codigoAplicacion);
                        command.Parameters.AddWithValue("@idProyecto", errInfo.idProyecto);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            errInfo.id = "";
            errInfo.descripcion = "";
            errInfo.fecha = "";
            errInfo.hora = "";
            errInfo.impacto = "";
            errInfo.serieServidor = "";
            errInfo.codigoAplicacion = "";
            errInfo.idProyecto = "";

            successMessage = "El error se agrego con exito a la base de datos";
        }
    }
}
