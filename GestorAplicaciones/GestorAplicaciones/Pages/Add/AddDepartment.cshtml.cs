using GestorAplicaciones.Models;
using GestorAplicaciones.Pages.Show;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace GestorAplicaciones.Pages.Add
{
    public class AddDepartmentModel : PageModel
    {
        public DeptInfo deptInfo = new DeptInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            deptInfo.codigo = Request.Form["dept-code"];
            deptInfo.nombre = Request.Form["dept-name"];
            deptInfo.cedulaJefe = Request.Form["dept-boss"];

            if (deptInfo.codigo.Length == 0 || deptInfo.nombre.Length == 0 || deptInfo.cedulaJefe.Length == 0)
            {
                errorMessage = "Por favor agregue informacion a todos los campos";
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

                    String sqlInsert = "INSERT INTO Departamento (codigo, nombre, cedulaJefe)  \r\nVALUES \r\n" +
                        "(@codigo, @nombre, @cedulaJefe)";

                    using (SqlCommand command = new SqlCommand(sqlInsert, connection))
                    {
                        command.Parameters.AddWithValue("@codigo", deptInfo.codigo);
                        command.Parameters.AddWithValue("@nombre", deptInfo.nombre);
                        command.Parameters.AddWithValue("@cedulaJefe", deptInfo.cedulaJefe);

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
            deptInfo.codigo = "";
            deptInfo.nombre = "";
            deptInfo.cedulaJefe = "";

            successMessage = "El departamento se agrego con exito a la base de datos";
        }
    }
}
