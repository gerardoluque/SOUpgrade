using System.ComponentModel.DataAnnotations;

namespace API.Domain.Catalogos
{
    public class Area
    {
        public int Id { get; set; }
        [MaxLength(100)] 
        public string Nombre { get; set; } = string.Empty;
        [MaxLength(500)] 
        public string? Descripcion { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;
        
        /// <summary>
        /// Fecha en la que se creó el paciente.
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow; // Fecha y hora en UTC

        /// <summary>
        /// Fecha de la última actualización del paciente.
        /// </summary>
        public DateTime FechaUltimaActualizacion { get; set; } = DateTime.UtcNow; // Fecha y hora en UTC
    }
}
