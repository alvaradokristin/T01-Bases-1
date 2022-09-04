using GestorAplicaciones.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace GestorAplicaciones.Pages.Show
{
    public class ShowProjectsModel : PageModel
    {
        // create a list of the info that we'll received from the database
        public List<ProyectInfo> listProjects = new List<ProyectInfo>();
        public void OnGet()
        {
            try
            {
                var connString = new ConnStr();
                String connectStr = connString.ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectStr))
                {
                    connection.Open();

                    String sqlSelectAll = "SELECT \r\nProyecto.*,\r\nError.id AS errorId,\r\nError.descripcion AS errorDesc,\r\nError.fecha,\r\nError.hora,\r\nError.impacto,\r\nError.serieServidor,\r\nError.codigoAplicacion,\r\nEmpleadoXProyecto.cedulaEmpleado,\r\nEmpleado.nombre + ' ' + Empleado.primerApellido + ' ' + Empleado.segundoApellido AS nombreEmpleado,\r\nEmpleadoXProyecto.rol\r\nFROM Proyecto\r\nLEFT JOIN Error ON Proyecto.id = Error.idProyecto\r\nLEFT JOIN EmpleadoXProyecto ON Proyecto.id = EmpleadoXProyecto.idProyecto\r\nLEFT JOIN Empleado ON Empleado.cedula = EmpleadoXProyecto.cedulaEmpleado";

                    // This allow us to execute the SQL query above
                    using (SqlCommand command = new SqlCommand(sqlSelectAll, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            String tempString;
                            DateTime tempDateTime;

                            while (reader.Read())
                            {
                                ProyectInfo proInfo = new ProyectInfo();

                                // Create an appInfo onject with the info from the db
                                proInfo.idPro = "" + reader["id"];

                                proInfo.nombrePro = "" + reader["nombre"];

                                proInfo.descripcionPro = "" + reader["descripcion"];

                                tempString = "" + reader["fechaInicio"];
                                if (tempString != "" && tempString != " ")
                                {
                                    tempDateTime = DateTime.Parse(tempString);
                                    proInfo.fechaInicio = tempDateTime.ToShortDateString();
                                }
                                else
                                {
                                    proInfo.fechaInicio = tempString;
                                }

                                tempString = "" + reader["fechaFinalizacion"];
                                if (tempString != "" && tempString != " ")
                                {
                                    tempDateTime = DateTime.Parse(tempString);
                                    proInfo.fechaFinalizacion = tempDateTime.ToShortDateString();
                                }
                                else
                                {
                                    proInfo.fechaFinalizacion = tempString;
                                }

                                tempString = "" + reader["fecha"];
                                if (tempString != "" && tempString != " ")
                                {
                                    tempDateTime = DateTime.Parse(tempString);
                                    proInfo.fechaError = tempDateTime.ToShortDateString();
                                }
                                else
                                {
                                    proInfo.fechaError = tempString;
                                }

                                tempString = "" + reader["hora"];
                                if (tempString != "" && tempString != " ")
                                {
                                    tempDateTime = DateTime.Parse(tempString);
                                    proInfo.horaError = tempDateTime.ToString("hh:mm tt");
                                }
                                else
                                {
                                    proInfo.horaError = tempString;
                                }

                                proInfo.esfuerzoEstimado = "" + reader["esfuerzoEstimado"];

                                proInfo.esfuerzoReal = "" + reader["esfuerzoReal"];

                                proInfo.idError = "" + reader["errorId"];

                                proInfo.descripcionError = "" + reader["errorDesc"];

                                proInfo.impacto = "" + reader["impacto"];

                                proInfo.serieServidor = "" + reader["serieServidor"];

                                proInfo.codigoApp = "" + reader["codigoAplicacion"];

                                proInfo.cedulaEmpleado = "" + reader["cedulaEmpleado"];

                                proInfo.nombreEmpleado = "" + reader["nombreEmpleado"];

                                proInfo.rol = "" + reader["rol"];

                                // Add the object to the list
                                listProjects.Add(proInfo);
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
    }
}
