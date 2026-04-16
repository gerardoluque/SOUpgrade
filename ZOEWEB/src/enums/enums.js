  export const EstadoMovimiento = Object.freeze({
    BORRADOR: 0,
    PENDIENTEAPROBAR: 1,
    APROBADO: 2,
    PARCIALMENTE : 3,
    ENTREGADO: 4,
    CANCELADO: 5
  });
    
  export const EstadoRecepcion = Object.freeze({
    PENDIENTE: 1,
    NO_RECIBIDO: 2,
    RECIBIDO: 3,
    PARCIAL: 4
  });

  export const EstadoSalida = Object.freeze({
    PENDIENTE: 1,
    ENTREGADO: 2,
    PARCIAL: 3,
    NO_ENTRAGADO: 4
  });

  export const EstadoCivil = Object.freeze({
    SOLTERO: 1,
    CASADO: 2,
    DIVORCIADO: 3,
    VIUDO: 4,
    UNION_LIBRE: 5
  });

  export const TipoPaciente = Object.freeze({
    AFILIADO: 1,
    BENEFICIARIO: 2,
    EXTERNO: 3
  });

  export const AnexoTipo = Object.freeze({
    FOTO_PERFIL: 1,
    DOCUMENTO: 2,
    OTRO: 3
  });

  export const AtencionEstatusEnum = Object.freeze({
    PROGRAMADA: 1,
    ENESPERA: 2,
    INICIADA: 3,
    COMPLETADA: 4,
    CANCELADA: 5,
     
  });

  export const TurnosEnum = Object.freeze({
    MATUTINO: 1,
    VESPERTINO: 2,
    NOCTURNO: 3
  });

  // Roles del sistema
  export const TipoRoles = Object.freeze({
    NoDefinido: 0,
    Medico: 1,
    Elemento: 2,
    Administrativo: 3,
    AdministradorSistema: 4,
    Directivo: 5
  });

  export const GraficaTipos = Object.freeze({
    Tarjeta: 1,
    Barra: 2,
    Pastel: 3,
    Lineal: 4,
    Gauge: 5
  });

  // Farmacia enums
  export const TipoArticulos = Object.freeze({
    MEDICAMENTO: 1,
    MATERIAL_CURACION: 2,
    OTRO: 3,
  });

  export const ClasificacionMedicamentos = Object.freeze({
    CONTROLADO: 1,
    ANTIBIOTICO: 2,
    NO_CONTROLADO: 3,
  });

  export const PresentacionMedicamento = Object.freeze({
    TABLETA: 1,
    CAPSULA: 2,
    JARABE: 3,
    AMPOLLAS: 4,
    UNGÜENTO: 5,
    CREMA: 6,
  });

  export const UnidadesMedida = Object.freeze({
    MG: 1,
    G: 2,
    ML: 3,
    L: 4,
    UNIDAD: 5,
  });

  export const TipoControl = Object.freeze({
    LIBRE: 0,
    CONTROLADO: 1,
    RESTRINGIDO: 2,
  });

  export const Turnos = Object.freeze([
    { id: TurnosEnum.MATUTINO, name: "Matutino"},
    { id: TurnosEnum.VESPERTINO, name: "Vespertino"},
    { id: TurnosEnum.NOCTURNO, name: "Nocturno" },
  ]);

  export const AtencionEstatus = Object.freeze([
    { id: AtencionEstatusEnum.PROGRAMADA, name: "Programada", className: "bg-monitor-Programada" },
    { id: AtencionEstatusEnum.ENESPERA, name: "En Espera", className: "bg-monitor-EnEspera" },
    { id: AtencionEstatusEnum.INICIADA, name: "Iniciada", className: "bg-monitor-Iniciada" },
    { id: AtencionEstatusEnum.COMPLETADA, name: "Completada", className: "bg-monitor-Completada" },
    { id: AtencionEstatusEnum.CANCELADA, name: "Cancelada", className: "bg-monitor-Cancelada" },    
  ]);

  // arrays para componentes dropdown
  export const EstadoCivilOptions = Object.freeze([
    { id: EstadoCivil.SOLTERO, name: "Soltero" },
    { id: EstadoCivil.CASADO, name: "Casado" },
    { id: EstadoCivil.DIVORCIADO, name: "Divorciado" },
    { id: EstadoCivil.VIUDO, name: "Viudo" },
    { id: EstadoCivil.UNION_LIBRE, name: "Unión libre" },
  ]);

  export const TipoPacienteOptions = Object.freeze([
    { id: TipoPaciente.AFILIADO, name: "Afiliado" },
    { id: TipoPaciente.BENEFICIARIO, name: "Beneficiario" },
    { id: TipoPaciente.EXTERNO, name: "Externo" },
  ]);

  export const TipoRolesOptions = Object.freeze([
    { id: TipoRoles.NoDefinido, name: "No definido" },
    { id: TipoRoles.Medico, name: "Médico" },
    { id: TipoRoles.Elemento, name: "Elemento" },
    { id: TipoRoles.Administrativo, name: "Administrativo" },
    { id: TipoRoles.AdministradorSistema, name: "Administrador Sistema" },
    { id: TipoRoles.Directivo, name: "Directivo" },
  ]);

  export const TipoArticulosOptions = Object.freeze([
    { id: TipoArticulos.MEDICAMENTO, name: "Medicamento" },
    { id: TipoArticulos.MATERIAL_CURACION, name: "Material de curación" },
    { id: TipoArticulos.OTRO, name: "Otro" },
  ]);

  export const ClasificacionMedicamentosOptions = Object.freeze([
    { id: ClasificacionMedicamentos.CONTROLADO, name: "Controlado" },
    { id: ClasificacionMedicamentos.ANTIBIOTICO, name: "Antibiótico" },
    { id: ClasificacionMedicamentos.NO_CONTROLADO, name: "No controlado" },
  ]);

  export const PresentacionMedicamentoOptions = Object.freeze([
    { id: PresentacionMedicamento.TABLETA, name: "Tableta" },
    { id: PresentacionMedicamento.CAPSULA, name: "Cápsula" },
    { id: PresentacionMedicamento.JARABE, name: "Jarabe" },
    { id: PresentacionMedicamento.AMPOLLAS, name: "Ampollas" },
    { id: PresentacionMedicamento.UNGÜENTO, name: "Ungüento" },
    { id: PresentacionMedicamento.CREMA, name: "Crema" },
  ]);

  export const UnidadesMedidaOptions = Object.freeze([
    { id: UnidadesMedida.MG, name: "mg" },
    { id: UnidadesMedida.G, name: "g" },
    { id: UnidadesMedida.ML, name: "ml" },
    { id: UnidadesMedida.L, name: "l" },
    { id: UnidadesMedida.UNIDAD, name: "Unidad" },
  ]);

  export const TipoControlOptions = Object.freeze([
    { id: TipoControl.LIBRE, name: "Libre" },
    { id: TipoControl.CONTROLADO, name: "Controlado" },
    { id: TipoControl.RESTRINGIDO, name: "Restringido" },
  ]);

  // Alertas enums
  export const AlertaMotivo = Object.freeze({
    ElementoInactivo: 1,
    ConstanciaEstudiosVencida: 2,
  });

  export const AlertaAsunto = Object.freeze({
    RegistroPaciente: 1,
    EdicionPaciente: 2,
    AsignacionCita: 3,
    AtencionConsulta: 4,
  });

  // helpers
  export function getLabelFromOptions(options, value) {
    const it = (options || []).find(x => x.id === value);
    return it ? it.name : "";
  }

  export function toOptionArray(enumObj, labelMapper = k => k.toString()) {
    return Object.keys(enumObj).map(k => ({ id: enumObj[k], name: labelMapper(k) }));
  }
