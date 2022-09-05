using GestorAplicaciones.Models;
using GestorAplicaciones.Pages.Show;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace GestorAplicaciones.Pages.Add
{
    public class AddServerModel : PageModel
    {
        // Attributes to populate information on the website and to get the user input to update the DB
        public ServerInfo serverInfo = new ServerInfo();

        public String errorMessage = "";
        public String successMessage = "";

        // Method to get information from the DB, since we don't need to get info, this is not needed at the moment
        public void OnGet()
        {
        }

        // Method to sent information to the DB
        public void OnPost()
        {
            // Asign the data from the website input into an object/variables
            serverInfo.serie = Request.Form["server-series"];
            serverInfo.marca = Request.Form["server-brand"];
            serverInfo.modelo = Request.Form["server-model"];
            serverInfo.fechaCompra = Request.Form["server-purchase-date"];
            serverInfo.capacidadProcesamiento = Request.Form["server-process"];
            serverInfo.capacidadAlmacenamiento = Request.Form["server-storage"];
            serverInfo.memoria = Request.Form["server-memory"];

            // Verify that the necessary fileds have information
            if (serverInfo.serie.Length == 0 || serverInfo.marca.Length == 0 || serverInfo.modelo.Length == 0
                 || serverInfo.fechaCompra.Length == 0 || serverInfo.capacidadProcesamiento.Length == 0 || serverInfo.capacidadAlmacenamiento.Length == 0
                  || serverInfo.memoria.Length == 0)
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
                    String sqlInsert = "INSERT INTO Servidor (serie, marca, modelo, fechaCompra, capacidadProcesamiento, capacidadAlmacenamiento, memoria) VALUES " +
                        "(@serie, @marca, @modelo, @fechaCompra, @capacidadProcesamiento, @capacidadAlmacenamiento, @memoria)";

                    using (SqlCommand command = new SqlCommand(sqlInsert, connection))
                    {
                        // Add the data from the input to the query parameters
                        command.Parameters.AddWithValue("@serie", serverInfo.serie);
                        command.Parameters.AddWithValue("@marca", serverInfo.marca);
                        command.Parameters.AddWithValue("@modelo", serverInfo.modelo);
                        command.Parameters.AddWithValue("@fechaCompra", serverInfo.fechaCompra);
                        command.Parameters.AddWithValue("@capacidadProcesamiento", serverInfo.capacidadProcesamiento);
                        command.Parameters.AddWithValue("@capacidadAlmacenamiento", serverInfo.capacidadAlmacenamiento);
                        command.Parameters.AddWithValue("@memoria", serverInfo.memoria);

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
            serverInfo.serie = "";
            serverInfo.marca = "";
            serverInfo.modelo = "";
            serverInfo.fechaCompra = "";
            serverInfo.capacidadProcesamiento = "";
            serverInfo.capacidadAlmacenamiento = "";
            serverInfo.memoria = "";

            successMessage = "El servidor se agrego con exito a la base de datos";
        }
    }
}