using API.Domain.Seguridad;

namespace API.DTOs.Seguridad
{
    public class DirectoryRoleDto
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public List<UserDto> Users { get; set; }
        public List<Proceso> Procesos { get; set; } // Nueva propiedad
    }
}
