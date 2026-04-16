using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace API.Domain.Seguridad
{
    public class AppIdentityRole : IdentityRole
    {
        public string? Descripcion { get; set; }

        /// <summary>
        /// Valor del rol en Azure AD, por ejemplo, "rol.ejemplo".
        /// </summary>
        [Required]
        public string Value { get; set; }

        /// <summary>
        /// Indica si el rol está activo o no.
        /// </summary>
        public bool Activo { get; set; } = true;

        /// <summary>
        /// Fecha en la que se creó el rol.
        /// </summary>
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha de la última actualización del rol.
        /// </summary>
        public DateTime FechaUltimaActualizacion { get; set; } = DateTime.UtcNow;
        /// <summary>
        /// Tipo de rol, que puede ser médico, elemento o administrativo.
        ///     
        public TipoRoles TipoRol { get; set; } = 0;
    }

    public enum TipoRoles
    {
        NoDefinido = 0,
        Medico,
        Elemento,
        Administrativo,
        AdministradorSistema,
        Directivo
    }
}
