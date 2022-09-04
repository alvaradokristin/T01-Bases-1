namespace GestorAplicaciones.Models
{
    public class AppInfo
    {
        public string codigo { get; set; }
        public string numPatente { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string tipo { get; set; }
        //public DateTime? fechaProduccion { get; set; }
        public string? fechaProduccion { get; set; }
        public string? fechaExpiraLicencia { get; set; }
        public string codigoDepartamento { get; set; }
    }
}
