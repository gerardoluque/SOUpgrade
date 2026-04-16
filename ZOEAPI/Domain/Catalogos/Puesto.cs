using System.ComponentModel.DataAnnotations;

namespace API.Domain.Catalogos
{
    public class Puesto
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;
        [MaxLength(500)]
        public string? Descripcion { get; set; } = string.Empty;
        public bool Activo { get; set; } = true;

        [MaxLength(5)]
        public string Nivel { get; set; } = string.Empty; // Nuevo campo para el nivel del puesto   

        [MaxLength(2)]
        public string TipoEmpleo { get; set; } = string.Empty; // Nuevo campo para el tipo de empleo (tiempo completo, medio tiempo, contrato, etc.)

        [MaxLength(5)]
        public string ClaveCategoria { get; set; } = string.Empty; // Nuevo campo para la clave de categoría del puesto

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
