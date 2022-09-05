using GestorAplicaciones.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Win32;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace GestorAplicaciones.Pages.Show
{
    public class ShowErrorsModel : PageModel
    {
        // create a list of the info that we'll received from the database
        public List<ErrInfo> listErrors = new List<ErrInfo>();
        public void OnGet()
        {
            try
            {
                var connString = new ConnStr();
                String connectStr = connString.ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectStr))
                {
                    connection.Open();

                    String sqlSelectAll = "SELECT * FROM Error";

                    // This allow us to execute the SQL query above
                    using (SqlCommand command = new SqlCommand(sqlSelectAll, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            String tempString;
                            DateTime tempDateTime;

                            while (reader.Read())
                            {
                                ErrInfo errInfo = new ErrInfo();

                                // Create an appInfo onject with the info from the db
                                errInfo.id = "" + reader["id"];

                                errInfo.descripcion = "" + reader["descripcion"];

                                tempString = "" + reader["fecha"]; // Convert what we get from DB into String
                                tempDateTime = DateTime.Parse(tempString); // Convert String to DateTime
                                errInfo.fecha = tempDateTime.ToShortDateString(); // Convert DateTime to String with format SmallDate

                                tempString = "" + reader["hora"];
                                tempDateTime = DateTime.Parse(tempString);
                                errInfo.hora = tempDateTime.ToString("hh:mm tt");

                                errInfo.impacto = "" + reader["impacto"];

                                errInfo.serieServidor = "" + reader["serieServidor"];

                                errInfo.codigoAplicacion = "" + reader["codigoAplicacion"];

                                errInfo.idProyecto = "" + reader["idProyecto"];

                                // Add the object to the list
                                listErrors.Add(errInfo);
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
