namespace GestorAplicaciones.Models
{
    public class ProyectInfo
    {
        public string? idPro { get; set; }
        public string? nombrePro { get; set; }
        public string? descripcionPro { get; set; }
        public string? fechaInicio { get; set; }
        public string? fechaFinalizacion { get; set; }
        public string? esfuerzoEstimado { get; set; }
        public string? esfuerzoReal { get; set; }
        public string? idError { get; set; }
        public string? descripcionError { get; set; }
        public string? fechaError { get; set; }
        public string? horaError { get; set; }
        public string? impacto { get; set; }
        public string? serieServidor { get; set; }
        public string? codigoApp { get; set; }
        public string? cedulaEmpleado { get; set; }
        public string? nombreEmpleado { get; set; }
        public string? rol { get; set; }
    }
}
