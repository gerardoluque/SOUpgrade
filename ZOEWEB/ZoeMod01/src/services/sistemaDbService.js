// Simple local service to provide Sistemas list until backend is ready
// Sistemas { Clinica = 1, Facturacion = 2, Nomina = 3 }

export const SistemasEnum = Object.freeze({
  Clinica: 1,
  Facturacion: 2,
  Nomina: 3,
});

export function getSistemas() {
  return [
    { id: SistemasEnum.Clinica, name: 'Clínica' },
    { id: SistemasEnum.Facturacion, name: 'Facturación' },
    { id: SistemasEnum.Nomina, name: 'Nómina' },
  ];
}

export function getSistemaName(id) {
  const map = {
    [SistemasEnum.Clinica]: 'Clínica',
    [SistemasEnum.Facturacion]: 'Facturación',
    [SistemasEnum.Nomina]: 'Nómina',
  };
  return map[Number(id)] || '';
}
