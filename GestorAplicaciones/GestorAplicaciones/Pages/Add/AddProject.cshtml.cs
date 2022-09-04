using GestorAplicaciones.Models;
using GestorAplicaciones.Pages.Show;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace GestorAplicaciones.Pages.Add
{
    public class AddProjectModel : PageModel
    {
        public BasicProyectInfo proInfo = new BasicProyectInfo();
        public List<String> listErrors = new List<String>();

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

                    String sqlSelectAllErrorIds = "SELECT id FROM Error";

                    using (SqlCommand command = new SqlCommand(sqlSelectAllErrorIds, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                String errId;

                                errId = "" + reader["id"];

                                // Add the object to the list
                                listErrors.Add(errId);
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
            proInfo.id = Request.Form["pro-id"];
            proInfo.nombre = Request.Form["pro-name"];
            proInfo.descripcion = Request.Form["pro-desc"];
            proInfo.fechaInicio = Request.Form["pro-start-date"];
            proInfo.fechaFinalizacion = Request.Form["pro-end-date"];
            proInfo.esfuerzoEstimado = Request.Form["pro-est-effort"];
            proInfo.esfuerzoReal = Request.Form["pro-real-effort"];
            proInfo.idError = Request.Form["pro-error"];

            if (proInfo.id.Length == 0 || proInfo.descripcion.Length == 0 || proInfo.nombre.Length == 0
                 || proInfo.fechaInicio.Length == 0 || proInfo.fechaFinalizacion.Length == 0 || proInfo.esfuerzoEstimado.Length == 0
                  || proInfo.esfuerzoReal.Length == 0 || proInfo.idError.Length == 0)
            {
                errorMessage = "Por favor ingrese toda la informacion solicitada";
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

                    String sqlInsertEdit = "INSERT INTO Proyecto (id, nombre, descripcion, fechaInicio, fechaFinalizacion, esfuerzoEstimado, esfuerzoReal) \r\nVALUES \r\n" +
                        "(@id, @nombre, @descripcion, @fechaInicio, @fechaFinalizacion, @esfuerzoEstimado, @esfuerzoReal) \n\n" +
                        "UPDATE Error SET idProyecto = @id WHERE id = @idError";

                    using (SqlCommand command = new SqlCommand(sqlInsertEdit, connection))
                    {
                        command.Parameters.AddWithValue("@id", proInfo.id);
                        command.Parameters.AddWithValue("@nombre", proInfo.nombre);
                        command.Parameters.AddWithValue("@descripcion", proInfo.descripcion);
                        command.Parameters.AddWithValue("@fechaInicio", proInfo.fechaInicio);
                        command.Parameters.AddWithValue("@fechaFinalizacion", proInfo.fechaFinalizacion);
                        command.Parameters.AddWithValue("@esfuerzoEstimado", proInfo.esfuerzoEstimado);
                        command.Parameters.AddWithValue("@esfuerzoReal", proInfo.esfuerzoReal);
                        command.Parameters.AddWithValue("@idError", proInfo.idError);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            proInfo.id = "";
            proInfo.nombre = "";
            proInfo.descripcion = "";
            proInfo.fechaInicio = "";
            proInfo.fechaFinalizacion = "";
            proInfo.esfuerzoEstimado = "";
            proInfo.esfuerzoReal = "";
            proInfo.idError = "";

            successMessage = "El proyecto se agrego con exito a la base de datos";
        }
    }
}
