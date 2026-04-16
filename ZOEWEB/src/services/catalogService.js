import { apiRequest } from '@/services/apiService'
import { useMainStore } from '@/store/useMainStore';
import { getFarmacias } from '@mod2/services/medicamentoService.js'
import { getClinicas } from '@mod2/services/clinicaService.js'


/**
 * Obtiene todos los catalogos desde el backend.
 */
export async function fetchCatalog(api,catalogName) {
  const store = useMainStore();
  

    const storageKey = `catalogos:${api}:${catalogName}`;
    // If a cached copy exists, return it immediately (prefer local cache)
    try {
      const cached = localStorage.getItem(storageKey);
      if (cached) {
        return JSON.parse(cached);
      }
    } catch (e) {
      // ignore localStorage read errors and continue to fetch
    }

    // Try to fetch from backend; on success save to localStorage.
    try {
      const result = await apiRequest({ url: `/${api}/${catalogName}`, useCorporacionHeader: true });

      if (!Array.isArray(result.data) || result.data.length === 0) {
        store.triggerAlert({
          message: "No se encontraron el catalogo " + catalogName,
          color: "warning",
          icon: "warning",
        });

        // If we have a cached copy, return it as a fallback
        try {
          const cached = localStorage.getItem(storageKey);
          if (cached) return JSON.parse(cached);
        } catch { /* ignore localStorage errors */ }

        return [];
      }

      // Save a copy in localStorage for faster subsequent loads / offline
      try {
        localStorage.setItem(storageKey, JSON.stringify(result.data));
      } catch (e) {
        // ignore storage errors (quota, private mode, etc.)
        console.warn('Could not save catalog to localStorage', e);
      }

      return result.data;
    } catch (err) {
      // On network error, try to return cached data if available
      try {
        const cached = localStorage.getItem(storageKey);
        if (cached) {
          return JSON.parse(cached);
        }
      } catch { /* ignore */ }

      // Otherwise rethrow or notify
      console.error('Error fetching catalog', api, catalogName, err);
      store.triggerAlert({
        message: `Error cargando catálogo ${catalogName}`,
        color: 'danger',
        icon: 'error'
      });
      return [];
    }

}

/**
 * Busca la clínica por corporación entre una lista de clínicas.
 * @param {Array} clinicasArr lista de clínicas (cada item puede tener `corporacionId` o variantes)
 * @param {String|Number} corporacionId id de la corporación (opcional). Si no se provee, intenta usar la selección del `mainStore`.
 * @returns {Object|null} clínica encontrada o null
 */
export async function getClinica(clinicasArr = [], corporacionId = null) {
  const store = useMainStore(); 

  if (!clinicasArr || (Array.isArray(clinicasArr) && clinicasArr.length === 0)) {
    // If no clinics provided, try to fetch from catalog functions which may be async.
    try {
      const cls = getClinicas();
      const resolved = (cls && typeof cls.then === 'function') ? await cls : cls
      
      clinicasArr = Array.isArray(resolved)
        ? resolved.map(c => ({ id: c.id, name: c.nombre || c.name || `Clínica ${c.id}`, corporacionId: c.corporacionId ?? c.CorporacionId ?? c.idCorporacion ?? c.Corporacion ?? null }))
        : []
    } catch (__) {
      clinicasArr = []
    }
  }

  let corp = corporacionId
  if (corp === null || corp === undefined || corp === '') {
    corp = store?.coporacionSelectedId ?? store?.coporacionSelected ?? (Array.isArray(store?.userdata?.corporaciones) ? store.userdata.corporaciones[0] : null)
  }
  if (corp === null || corp === undefined || corp === '') return null
  try {
    const key = String(corp);
    
    return (Array.isArray(clinicasArr) ? clinicasArr : []).find(c => String(c.corporacionid ??  c.corporacionId ?? c.CorporacionId ?? c.idCorporacion ?? c.Corporacion ?? '') === key) || null
  } catch (e) {
    return null
  }
}

/**
 * Busca la farmacia asociada a la clínica (derivada por corporación) entre una lista de farmacias.
 * @param {Array} clinicasArr lista de clínicas
 * @param {Array} farmaciasArr lista de farmacias
 * @param {String|Number} corporacionId id de la corporación (opcional)
 * @returns {Object|null} farmacia encontrada o null
 */
export async function getFarmacia(clinicasArr = [], farmaciasArr = [], corporacionId = null) {
  // Resolve clinicas/farmacias if caller passed promises or if internal helpers return promises
  if (!clinicasArr || (Array.isArray(clinicasArr) && clinicasArr.length === 0)) {
    // attempt to load clinicas via helper
    try {
      const cls = getClinicas()
      const resolved = (cls && typeof cls.then === 'function') ? await cls : cls
      clinicasArr = Array.isArray(resolved)
        ? resolved.map(c => ({ id: c.id, name: c.nombre || c.name || `Clínica ${c.id}`, corporacionId: c.corporacionId ?? c.CorporacionId ?? c.idCorporacion ?? c.Corporacion ?? null }))
        : []
    } catch (_) {
      clinicasArr = []
    }
  }

  // If farmaciasArr is empty, try to load from getFarmacias helper which is async
  if (!farmaciasArr || (Array.isArray(farmaciasArr) && farmaciasArr.length === 0)) {
    try {
      const farms = getFarmacias()
      const resolvedF = (farms && typeof farms.then === 'function') ? await farms : farms
      farmaciasArr = Array.isArray(resolvedF)
        ? resolvedF.map(x => ({ id: x.value ?? x.id ?? x.farmaciaId ?? x.farmacia, name: x.name ?? x.farmaciaNombre ?? x.name ?? x.nombre ?? `Farmacia ${x.value ?? x.id}`, clinicaId: x.clinicaId ?? x.ClinicaId ?? x.idClinica ?? x.Clinica ?? null }))
        : []
    } catch (_) {
      farmaciasArr = []
    }
  }

  const clinica = await getClinica(clinicasArr, corporacionId)
  if (!clinica) return null
  try {
    console.log('getFarmacia - clinica found:', clinica);
    const clinicaId = String(clinica.id ?? clinica.clinicaId ?? clinica.idClinica ?? clinica.ClinicaId ?? '');
    return (Array.isArray(farmaciasArr) ? farmaciasArr : []).find(f => String(f.clinicaId ?? f.ClinicaId ?? f.idClinica ?? f.Clinica ?? '') === clinicaId) || null
  } catch (e) {
    return null
  }
}

/**
 * Devuelve la farmacia seleccionada por el usuario (si existe) desde el main store.
 * Útil para pasar como parámetro por defecto en servicios.
 */
export function getUserFarmacia() {
  try {
    const store = useMainStore()
    const farm = store?.farmaciaSelectedId ?? store?.farmaciaSelected ?? null
    if (farm === undefined || farm === null || farm === '') return null
    const asNum = Number(farm)
    return Number.isFinite(asNum) && asNum > 0 ? asNum : farm
  } catch (e) {
    return null
  }
}