<template>
  <div v-if="visible" class="modal-backdrop">
    <div class="modal-card">
      <div class="modal-header d-flex justify-content-between align-items-center">
        <h5 class="mb-0">{{ title }}</h5>
        <button class="btn-close" @click="$emit('close')">×</button>
      </div>
      <div class="modal-body">
        <div class="section-title">SELECCIÓN DE DATOS</div>
        <div class="row g-3 align-items-end mb-2">
          <div class="col-md-4">
            <label class="form-label">Fecha inicio</label>
            <input v-model="localFilters.fechaInicio" type="datetime-local" class="form-control" :max="localFilters.fechaFin || ''" />
          </div>
          <div class="col-md-4">
            <label class="form-label">Fecha fin</label>
            <input v-model="localFilters.fechaFin" type="datetime-local" class="form-control" :min="localFilters.fechaInicio || ''" />
          </div>
          <div class="col-md-4">
            <label class="form-label">Formato</label>
            <select v-model="localFilters.formato" class="form-control">
              <option value="PDF">PDF</option>
              <option value="CSV">CSV</option>
            </select>
          </div>
        </div>

        <div class="row g-3 align-items-end mb-2">
          <div class="col-md-4">
            <label class="form-label">Consultorio</label>
            <input v-model.number="localFilters.consultorio" type="number" min="1" step="1" class="form-control" placeholder="Todos = vacío" />
          </div>
          <div class="col-md-4">
            <label class="form-label">Clínica</label>
            <MaterialDropDown id="clinicaDropdown" v-model="localFilters.clinicaId" :options="clinicasWithAll" :numeric-value="true" selecciona-placeholder="Todos" />
          </div>
        </div>

        <div class="d-flex justify-content-between mt-3">
          <button class="btn btn-secondary" @click="$emit('close')">Cancelar</button>
          <div class="d-flex gap-2">
            <button class="btn btn-gray" :disabled="loading || !isDateRangeValid" @click="onPreview">Vista previa</button>
            <button class="btn btn-gold" :disabled="loading || !isDateRangeValid" @click="onExport">{{ loading ? 'Generando…' : 'Exportar' }}</button>
          </div>
        </div>
        <div v-if="!isDateRangeValid" class="text-danger small mt-2">La fecha de inicio no puede ser posterior a la fecha final.</div>

        <div v-if="previewData" class="mt-4">
          <h6 class="mb-2">Vista Previa</h6>
          <div v-if="Array.isArray(previewRows) && previewRows.length">
            <div class="table-responsive">
              <table class="table table-sm table-striped">
                <thead>
                  <tr>
                    <th v-for="col in previewColumns" :key="col">{{ col }}</th>
                  </tr>
                </thead>
                <tbody>
                  <tr v-for="(row, idx) in previewRows" :key="idx">
                    <td v-for="col in previewColumns" :key="col">{{ row[col] }}</td>
                  </tr>
                </tbody>
              </table>
            </div>
          </div>
          <div v-else-if="Array.isArray(previewRows) && previewRows.length === 0" class="p-3 bg-light small text-muted">No se encontró información.</div>
          <pre v-else class="bg-light p-2 small">{{ previewData }}</pre>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { defineComponent, ref, watch, computed } from 'vue'
import MaterialDropDown from '@/components/common/MaterialDropDown.vue'
import { getClinicas } from '@mod2/services/clinicaService.js'

export default defineComponent({
  name: 'ReportModal',
  components: { MaterialDropDown },
  props: {
    visible: { type: Boolean, default: false },
    title: { type: String, default: 'Reporte' },
    filters: { type: Object, default: () => ({}) },
    clinicas: { type: Array, default: () => [] },
    previewHandler: { type: Function, required: true },
    exportHandler: { type: Function, required: true },
  },
  emits: ['close', 'update:filters'],
  setup(props, { emit }) {
    const localFilters = ref({ ...props.filters })
    const loading = ref(false)
    const previewData = ref(null)

    const localClinicas = ref(Array.isArray(props.clinicas) && props.clinicas.length ? props.clinicas.slice() : [])

    const clinicasWithAll = computed(() => [{ id: -1, name: 'Todos' }, ...(localClinicas.value || [])])

    async function loadClinicasIfNeeded() {
      if (localClinicas.value && localClinicas.value.length) return
      try {
        const raw = await getClinicas()
        const list = Array.isArray(raw) ? raw : (raw && Array.isArray(raw.items) ? raw.items : [])
        localClinicas.value = list.map(x => ({ id: Number(x.id ?? x.clinicaId ?? x.Id ?? 0), name: x.nombre || x.name || `Clinica ${x.id ?? x.clinicaId ?? ''}` }))
      } catch (err) {
        localClinicas.value = []
        console.warn('Error cargando clínicas', err)
      }
    }
    function getTodayRange() {
      const d = new Date()
      const yyyy = d.getFullYear()
      const mm = String(d.getMonth() + 1).padStart(2, '0')
      const dd = String(d.getDate()).padStart(2, '0')
      return {
        inicio: `${yyyy}-${mm}-${dd}T00:00`,
        fin: `${yyyy}-${mm}-${dd}T23:59`
      }
    }

    watch(() => props.visible, (v) => {
      if (v) {
        localFilters.value = { ...props.filters }
        if (!localFilters.value.formato) localFilters.value.formato = 'PDF'
        // Inicializar fechas con la fecha actual si no vienen en filters
        const today = getTodayRange()
        if (!localFilters.value.fechaInicio) localFilters.value.fechaInicio = today.inicio
        if (!localFilters.value.fechaFin) localFilters.value.fechaFin = today.fin
        loadClinicasIfNeeded()
      }
    }, { immediate: true })

    watch(() => props.filters, (val, oldVal) => {
      if (val !== oldVal && props.visible) {
        localFilters.value = { ...val }
        if (!localFilters.value.formato) localFilters.value.formato = 'PDF'
      }
    })

    function normalizeConsultorio(val) {
      if (val === '' || val == null) return null
      const n = Number(val)
      if (!Number.isFinite(n) || n <= 0) return null
      return String(Math.trunc(n))
    }

    const isDateRangeValid = computed(() => {
      const inicio = localFilters.value?.fechaInicio
      const fin = localFilters.value?.fechaFin
      if (!inicio || !fin) return true
      try {
        return new Date(inicio) <= new Date(fin)
      } catch { return true }
    })

    // Auto-correct: if inicio becomes > fin, set fin = inicio
    watch(() => localFilters.value.fechaInicio, (v) => {
      if (!v) return
      const fin = localFilters.value.fechaFin
      if (fin && new Date(v) > new Date(fin)) {
        localFilters.value.fechaFin = v
      }
    })

    function syncFilters() {
      const out = { ...localFilters.value }
      out.consultorio = normalizeConsultorio(out.consultorio)
      if (out.clinicaId === -1) out.clinicaId = null
      // persist normalized values back to localFilters so handlers receive them
      localFilters.value = { ...out }
      emit('update:filters', out)
      return out
    }

    async function onPreview() {
      syncFilters()
      try {
        const res = await props.previewHandler({ ...localFilters.value })
        previewData.value = res?.value ?? res
      } catch (e) { console.warn('Preview error', e) }
    }

    async function onExport() {
      syncFilters()
      loading.value = true
      try {
        const result = await props.exportHandler({ ...localFilters.value })
        const blobCandidate = (result && result.data && result.data instanceof Blob) ? result.data : result
        if (blobCandidate instanceof Blob) {
          if (blobCandidate.type && blobCandidate.type.includes('application/json')) {
            try { const text = await blobCandidate.text(); console.warn('Export devolvió JSON en lugar de archivo:', text) } catch {console.error('Error leyendo blob JSON de exportación')}
          }
          downloadBlob(blobCandidate, localFilters.value.fileName || defaultFileName())
        } else if (result?.fileBytes && result?.contentType) {
          const byteChars = atob(result.fileBytes)
          const byteNumbers = Array.from({ length: byteChars.length }, (_, i) => byteChars.charCodeAt(i))
          const byteArray = new Uint8Array(byteNumbers)
          const blob = new Blob([byteArray], { type: result.contentType })
          downloadBlob(blob, result.fileName || defaultFileName())
        } else {
          console.warn('Formato de exportación no reconocido', result)
        }
      } catch (e) { console.warn('Export error', e) } finally { loading.value = false }
    }

    function defaultFileName() {
      const base = (localFilters.value?.fechaInicio || '').toString().slice(0,16).replace(/[:T]/g,'-') || 'reporte'
      const fmt = (localFilters.value?.formato || 'PDF').toLowerCase()
      return `Reporte_${base}.${fmt}`
    }

    function downloadBlob(blob, fileName) {
      const url = URL.createObjectURL(blob)
      const a = document.createElement('a')
      a.href = url
      a.download = fileName
      document.body.appendChild(a)
      a.click()
      a.remove()
      URL.revokeObjectURL(url)
    }

    const previewRows = computed(() => {
      // Support multiple common shapes for preview data
      if (Array.isArray(previewData.value)) return previewData.value
      if (Array.isArray(previewData.value?.rows)) return previewData.value.rows
      if (Array.isArray(previewData.value?.atenciones)) return previewData.value.atenciones
      return null
    })
    const previewColumns = computed(() => {
      const rows = previewRows.value
      if (rows && rows.length) return Object.keys(rows[0])
      return []
    })

    return { localFilters, loading, previewData, onPreview, onExport, previewRows, previewColumns, clinicasWithAll, isDateRangeValid }
  }
})
</script>

<style scoped>
.modal-backdrop { position: fixed; inset: 0; background: rgba(0,0,0,0.4); display: flex; align-items: center; justify-content: center; z-index: 1050; }
  .modal-card { background: #fff; border-radius: 8px; width: min(1100px, 98vw); max-height: 90vh; overflow: auto; box-shadow: 0 10px 30px rgba(0,0,0,0.2); }
.modal-header { padding: 0.75rem 1rem; border-bottom: 1px solid #eee; }
.modal-body { padding: 1rem; }
.btn-close { background: transparent; border: none; font-size: 1.25rem; line-height: 1; }
.btn-gold { background-color: #C9A227; border-color: #C9A227; color: #111827; }
.btn-gold:hover { background-color: #B8931F; border-color: #B8931F; color: #111827; }
.btn-gray { background-color: #6B7280; border-color: #6B7280; color: #ffffff; }
.btn-gray:hover { background-color: #4B5563; border-color: #4B5563; }
.section-title { font-weight: 700; font-size: .9rem; color: #374151; text-transform: uppercase; margin-bottom: .5rem; }
</style>