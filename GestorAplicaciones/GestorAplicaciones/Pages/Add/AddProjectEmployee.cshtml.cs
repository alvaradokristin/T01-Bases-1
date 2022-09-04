using GestorAplicaciones.Models;
using GestorAplicaciones.Pages.Show;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace GestorAplicaciones.Pages.Add
{
    public class AddProjectEmployeeModel : PageModel
    {
        public List<String> listProjects = new List<String>();
        public List<String> listEmp = new List<String>();
        public EmpRols newEmpRol = new EmpRols();

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

                    String sqlSelectAllProIds = "SELECT id FROM Proyecto";
                    String sqlSelectAllEmpIds = "SELECT cedula FROM Empleado";

                    using (SqlCommand command = new SqlCommand(sqlSelectAllProIds, connection))
                    {
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

        public void OnPost()
        {
            newEmpRol.proId = Request.Form["exp-pro-id"];
            newEmpRol.empId = Request.Form["exp-emp-name"];
            newEmpRol.empRol = Request.Form["exp-emp-rol"];

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

                    String sqlInsert = "INSERT INTO EmpleadoXProyecto (cedulaEmpleado, idProyecto, rol) VALUES " +
                        "(@cedulaEmpleado, @idProyecto, @rol)";

                    using (SqlCommand command = new SqlCommand(sqlInsert, connection))
                    {
                        command.Parameters.AddWithValue("@cedulaEmpleado", newEmpRol.empId);
                        command.Parameters.AddWithValue("@idProyecto", newEmpRol.proId);
                        command.Parameters.AddWithValue("@rol", newEmpRol.empRol);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            newEmpRol.proId = "";
            newEmpRol.empId = "";
            newEmpRol.empRol = "";

            successMessage = "Se actualizo exitosamente la base de datos de los empleado por proyecto";

            Response.Redirect("/Add/AddProjectEmployee");
        }
    }
}
