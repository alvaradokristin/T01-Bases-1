namespace GestorAplicaciones.Models
{
    public class ConnStr
    {
        // This will be the connection to the database, copy and paste from the connection on the Server Explorer
        private String connectionString = "Data Source=KRISTIN;Initial Catalog=gestorapp;Integrated Security=True";
        public String ConnectionString
        {
            get { return connectionString; }
            set { connectionString = value; }
        }
    }
}
