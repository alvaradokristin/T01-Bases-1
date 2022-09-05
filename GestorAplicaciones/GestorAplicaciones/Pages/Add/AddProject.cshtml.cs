using GestorAplicaciones.Models;
using GestorAplicaciones.Pages.Show;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace GestorAplicaciones.Pages.Add
{
    public class AddProjectModel : PageModel
    {
        // Attributes to populate information on the website and to get the user input to update the DB
        public BasicProyectInfo proInfo = new BasicProyectInfo();
        public List<String> listErrors = new List<String>();

        public String errorMessage = "";
        public String successMessage = "";

        // Method to get information from the DB and use it on the website (the select dropdowns)
        public void OnGet()
        {
            try
            {
                var connString = new ConnStr();
                String connectStr = connString.ConnectionString;

                using (SqlConnection connection = new SqlConnection(connectStr))
                {
                    connection.Open();

                    // Query to be use
                    String sqlSelectAllErrorIds = "SELECT id FROM Error";

                    using (SqlCommand command = new SqlCommand(sqlSelectAllErrorIds, connection))
                    {
                        // Execute the query to get all error ids
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

        // Method to sent information to the DB
        public void OnPost()
        {
            // Asign the data from the website input into an object/variables
            proInfo.id = Request.Form["pro-id"];
            proInfo.nombre = Request.Form["pro-name"];
            proInfo.descripcion = Request.Form["pro-desc"];
            proInfo.fechaInicio = Request.Form["pro-start-date"];
            proInfo.fechaFinalizacion = Request.Form["pro-end-date"];
            proInfo.esfuerzoEstimado = Request.Form["pro-est-effort"];
            proInfo.esfuerzoReal = Request.Form["pro-real-effort"];
            proInfo.idError = Request.Form["pro-error"];

            // Verify that the necessary fileds have information
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

                    // Query to send/edit the data to the DB
                    String sqlInsertEdit = "INSERT INTO Proyecto (id, nombre, descripcion, fechaInicio, fechaFinalizacion, esfuerzoEstimado, esfuerzoReal) \r\nVALUES \r\n" +
                        "(@id, @nombre, @descripcion, @fechaInicio, @fechaFinalizacion, @esfuerzoEstimado, @esfuerzoReal) \n\n" +
                        "UPDATE Error SET idProyecto = @id WHERE id = @idError";

                    using (SqlCommand command = new SqlCommand(sqlInsertEdit, connection))
                    {
                        // Add the data from the input to the query parameters
                        command.Parameters.AddWithValue("@id", proInfo.id);
                        command.Parameters.AddWithValue("@nombre", proInfo.nombre);
                        command.Parameters.AddWithValue("@descripcion", proInfo.descripcion);
                        command.Parameters.AddWithValue("@fechaInicio", proInfo.fechaInicio);
                        command.Parameters.AddWithValue("@fechaFinalizacion", proInfo.fechaFinalizacion);
                        command.Parameters.AddWithValue("@esfuerzoEstimado", proInfo.esfuerzoEstimado);
                        command.Parameters.AddWithValue("@esfuerzoReal", proInfo.esfuerzoReal);
                        command.Parameters.AddWithValue("@idError", proInfo.idError);

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

            proInfo.id = "";
            proInfo.nombre = "";
            proInfo.descripcion = "";
            proInfo.fechaInicio = "";
            proInfo.fechaFinalizacion = "";
            proInfo.esfuerzoEstimado = "";
            proInfo.esfuerzoReal = "";
            proInfo.idError = "";

            successMessage = "El proyecto se agrego con exito a la base de datos";

            Response.Redirect("/Add/AddProject");
        }
    }
}