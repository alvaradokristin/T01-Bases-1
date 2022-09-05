using GestorAplicaciones.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace GestorAplicaciones.Pages.Show
{
    public class ShowAppsModel : PageModel
    {
        // create a list of the info that we'll received from the database
        public List<AppInfo> listApps = new List<AppInfo>();
        public void OnGet()
        {
            try
            {
                var connString = new ConnStr();
                String connectStr = connString.ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectStr))
                {
                    connection.Open();
                    String sqlSelectAll = "SELECT * FROM Aplicacion";

                    // This allow us to execute the SQL query above
                    using (SqlCommand command = new SqlCommand(sqlSelectAll, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            String tempString;
                            DateTime tempDateTime;

                            while (reader.Read())
                            {
                                AppInfo appInfo = new AppInfo();

                                // Create an appInfo onject with the info from the db
                                appInfo.codigo = "" + reader["codigo"];

                                appInfo.numPatente = "" + reader["numPatente"];

                                appInfo.nombre = "" + reader["nombre"];

                                appInfo.descripcion = "" + reader["descripcion"];

                                appInfo.tipo = "" + reader["tipo"];

                                tempString = "" + reader["fechaProduccion"];
                                if (tempString != "" && tempString != " ")
                                {
                                    tempDateTime = DateTime.Parse(tempString);
                                    appInfo.fechaProduccion = tempDateTime.ToShortDateString();
                                } else
                                {
                                    appInfo.fechaProduccion = tempString;
                                }

                                tempString = "" + reader["fechaExpiraLicencia"];
                                if (tempString != "" && tempString != " ")
                                {
                                    tempDateTime = DateTime.Parse(tempString);
                                    appInfo.fechaExpiraLicencia = tempDateTime.ToShortDateString();
                                }
                                else
                                {
                                    appInfo.fechaExpiraLicencia = tempString;
                                }

                                appInfo.codigoDepartamento = "" + reader["codigoDepartamento"];

                                // Add the object to the list
                                listApps.Add(appInfo);
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
