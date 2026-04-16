import { apiRequest } from '@/services/apiService'

/**
 * Get paginated alerts from backend
 * params: { motivo, asunto, fechaInicio, fechaFin, leido, pageNumber, pageSize, sortBy, sortDirection }
 */
export async function getAlertasPaginadas(params = {}) {
  const {
    motivo = undefined,
    asunto = undefined,
    fechaInicio = undefined,
    fechaFin = undefined,
    leido = undefined,
    pageNumber = 1,
    pageSize = 10,
    sortBy = 'fechacreacion',
    sortDirection = 'asc'
  } = params || {}

  const qs = new URLSearchParams()
  if (motivo !== undefined && motivo !== null && String(motivo) !== '') qs.append('motivo', String(motivo))
  if (asunto !== undefined && asunto !== null && String(asunto) !== '') qs.append('asunto', String(asunto))
  if (fechaInicio) qs.append('fechaInicio', String(fechaInicio))
  if (fechaFin) qs.append('fechaFin', String(fechaFin))
  if (leido !== undefined && leido !== null && String(leido) !== '') qs.append('leido', String(leido))
  qs.append('pageNumber', String(Number(pageNumber) || 1))
  qs.append('pageSize', String(Number(pageSize) || 10))
  if (sortBy) qs.append('sortBy', String(sortBy))
  if (sortDirection) qs.append('sortDirection', String(sortDirection))

  const url = `/Alertas/paginadas?${qs.toString()}`
  const result = await apiRequest({ url, method: 'GET' })
  // Normalize backend shape to expected structure by views/components
  // Some endpoints return { data: [...], totalRecords, totalPages, pageNumber, pageSize }
  // while components expect { items: [...], pageNumber, pageSize, totalPages, totalRecords }
  try {
    const payload = result && result.data ? result.data : null
    if (payload) {
      const normalized = {
        items: Array.isArray(payload.items) ? payload.items : (Array.isArray(payload.data) ? payload.data : (Array.isArray(payload) ? payload : [])),
        pageNumber: Number(payload.pageNumber ?? payload.page ?? 1) || 1,
        pageSize: Number(payload.pageSize ?? payload.pageSize ?? params.pageSize) || params.pageSize || 10,
        totalPages: Number(payload.totalPages ?? payload.pagination?.totalPages ?? 1) || 1,
        totalRecords: Number(payload.totalRecords ?? payload.total ?? payload.pagination?.totalRecords ?? 0) || 0,
        raw: payload
      }
      return { ...result, data: normalized }
    }
  } catch (e) {
    // ignore normalization errors and fall back to raw result
  }
  return result
}
