using API.Application.Anexos.Commands;

using API.Application.Seguridad.Procesos.Commands;

using API.Domain;
using API.Domain.Anexos;
using API.Domain.Audit;

using API.Domain.Seguridad;
using API.DTOs.Anexos;
using API.DTOs.AuditLogs;

using API.DTOs.Seguridad;
using AutoMapper;

namespace API.Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Grupo, GroupDto>();

            CreateMap<Usuario, UserDto>()
                .ForMember(dest => dest.Requiere2FA, opt => opt.MapFrom(src => src.AppUserIdentity.TwoFactorEnabled))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AppUserIdentity.Email))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.AppUserIdentity.UserName))
                .ForMember(dest => dest.Corporaciones, opt => opt.MapFrom(src => src.UsuarioCorporaciones.Select(s => s.CorporacionId).ToList()));

            CreateMap<AppIdentityRole, ApplicationRoleDto>();

            CreateMap<Corporacion, CorporacionDTO>();

            CreateMap<AuditLog, AuditLogDto>();
            CreateMap<LogEntry, LogEntryDto>();

            CreateMap<Permiso, PermisoDTO>();
            CreateMap<PermisoDTO, Permiso>();

            CreateMap<Grupo, Grupo>();
            CreateMap<Proceso, ProcesoDto>()
                .ForMember(dest => dest.Subprocesos, opt => opt.MapFrom(src => src.Subprocesos));

            // Commands -> Proceso
            CreateMap<CreateProceso.Command, Proceso>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaUltimaActualizacion, opt => opt.Ignore())
                .ForMember(dest => dest.Activo, opt => opt.Ignore())
                .ForMember(dest => dest.Subprocesos, opt => opt.Ignore())
                .ForMember(dest => dest.ProcesoPadre, opt => opt.Ignore());

            CreateMap<UpdateProceso.Command, Proceso>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaUltimaActualizacion, opt => opt.Ignore())
                .ForMember(dest => dest.ProcesoPadreId, opt => opt.Ignore())
                .ForMember(dest => dest.Subprocesos, opt => opt.Ignore())
                .ForMember(dest => dest.ProcesoPadre, opt => opt.Ignore());

          

            // Command to Entity (ArchivoBlob handled in handler)
            CreateMap<CreateAnexo.Command, Anexo>()
                .ForMember(dest => dest.Blob, opt => opt.Ignore());
            CreateMap<UpdateAnexo.Command, Anexo>()
                .ForMember(dest => dest.Blob, opt => opt.Ignore());

            CreateMap<Anexo, AnexoDTO>()
                .ForMember(dest => dest.BlobBase64, 
                    opt => opt.MapFrom(src => src.Blob != null ? Convert.ToBase64String(src.Blob) : null));

           
        }
    }
}