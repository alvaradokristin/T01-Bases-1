using GestorAplicaciones.Models;
using GestorAplicaciones.Pages.Show;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace GestorAplicaciones.Pages.Add
{
    public class AddProjectEmployeeModel : PageModel
    {
        // Attributes to populate information on the website and to get the user input to update the DB
        public List<String> listProjects = new List<String>();
        public List<String> listEmp = new List<String>();
        public EmpRols newEmpRol = new EmpRols();

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
                    String sqlSelectAllProIds = "SELECT id FROM Proyecto";
                    String sqlSelectAllEmpIds = "SELECT cedula FROM Empleado";

                    using (SqlCommand command = new SqlCommand(sqlSelectAllProIds, connection))
                    {
                        // Execute the query to create a list of all the project ids
                        using (SqlDataReader readerProIds = command.ExecuteReader())
                        {
                            while (readerProIds.Read())
                            {
                                String id;

                                id = "" + readerProIds["id"];

                                // Add the object to the list
                                listProjects.Add(id);
                            }
                        }
                    }

                    using (SqlCommand command = new SqlCommand(sqlSelectAllEmpIds, connection))
                    {
                        // Execute the query to create a list of all the employees ids
                        using (SqlDataReader readerEmpIds = command.ExecuteReader())
                        {
                            while (readerEmpIds.Read())
                            {
                                String ced;

                                ced = "" + readerEmpIds["cedula"];

                                // Add the object to the list
                                listEmp.Add(ced);
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
            newEmpRol.proId = Request.Form["exp-pro-id"];
            newEmpRol.empId = Request.Form["exp-emp-name"];
            newEmpRol.empRol = Request.Form["exp-emp-rol"];

            // Verify that the necessary fileds have information
            if (newEmpRol.proId.Length == 0 || newEmpRol.empId.Length == 0 || newEmpRol.empRol.Length == 0)
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

                    // Query to send the data to the DB
                    String sqlInsert = "INSERT INTO EmpleadoXProyecto (cedulaEmpleado, idProyecto, rol) VALUES " +
                        "(@cedulaEmpleado, @idProyecto, @rol)";

                    using (SqlCommand command = new SqlCommand(sqlInsert, connection))
                    {
                        // Add the data from the input to the query parameters
                        command.Parameters.AddWithValue("@cedulaEmpleado", newEmpRol.empId);
                        command.Parameters.AddWithValue("@idProyecto", newEmpRol.proId);
                        command.Parameters.AddWithValue("@rol", newEmpRol.empRol);

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
            newEmpRol.proId = "";
            newEmpRol.empId = "";
            newEmpRol.empRol = "";

            successMessage = "Se actualizo exitosamente la base de datos de los empleado por proyecto";

            Response.Redirect("/Add/AddProjectEmployee");
        }
    }
}