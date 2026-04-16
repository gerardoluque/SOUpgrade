<template>
  <div v-if="visible" class="modal-backdrop">
    <div class="modal-card">
      <div class="modal-header d-flex justify-content-between align-items-center">
        <h5 class="mb-0">{{ title }}</h5>
        <button class="btn-close" @click="$emit('close')">×</button>
      </div>
      <div class="modal-body">
        <!-- Selección de datos -->
        <div class="section-title">SELECCIÓN DE DATOS</div>

        <!-- Fechas -->
        <div class="row g-3 align-items-end mb-2">
          <div class="col-md-4">
            <label class="form-label">Fecha inicio</label>
            <input v-model="localFilters.fechaInicio" type="date" class="form-control" />
          </div>
          <div class="col-md-4">
            <label class="form-label">Fecha fin</label>
            <input v-model="localFilters.fechaFin" type="date" class="form-control" />
          </div>
          <div class="col-md-4">
            <label class="form-label">Formato</label>
            <select v-model="localFilters.formato" class="form-control">
              <option value="PDF">PDF</option>
              <option value="CSV">CSV</option>
            </select>
          </div>
        </div>

        <!-- Antecedentes -->
        <div class="section-subtitle">ANTECEDENTES</div>
        <div class="row g-3 mb-2">
          <div class="col-auto"><label class="form-check">
            <input v-model="allAntecedentes" class="form-check-input" type="checkbox"> <span class="form-check-label">TODOS</span>
          </label></div>
          <div class="col-auto"><label class="form-check">
            <input v-model="localFilters.antPatologicos" class="form-check-input" type="checkbox"> <span class="form-check-label">Patológicos</span>
          </label></div>
          <div class="col-auto"><label class="form-check">
            <input v-model="localFilters.antNoPatologicos" class="form-check-input" type="checkbox"> <span class="form-check-label">No patológicos</span>
          </label></div>
          <div class="col-auto"><label class="form-check">
            <input v-model="localFilters.antFamiliares" class="form-check-input" type="checkbox"> <span class="form-check-label">Familiares</span>
          </label></div>
          <div class="col-auto"><label class="form-check">
            <input v-model="localFilters.antGinecoObstetricos" class="form-check-input" type="checkbox"> <span class="form-check-label">Gineco obstétricos</span>
          </label></div>
        </div>

        <!-- Datos de la consulta -->
        <div class="section-subtitle">DATOS DE LA CONSULTA</div>
        <div class="row g-3 mb-3">
          <div class="col-auto">
            <label class="form-check">
              <input v-model="allConsulta" class="form-check-input" type="checkbox"> <span class="form-check-label">TODOS</span>
            </label>
          </div>
          <div v-for="opt in consultaOptions" :key="opt.key" class="col-auto">
            <label class="form-check">
              <input v-model="localFilters[opt.key]" class="form-check-input" type="checkbox"> <span class="form-check-label">{{ opt.label }}</span>
            </label>
          </div>
        </div>

        <div class="d-flex justify-content-between mt-3">
          <button class="btn btn-secondary" @click="$emit('close')">Cancelar</button>
          <div class="d-flex gap-2">
            <button class="btn btn-gray" @click="onPreview">Vista previa</button>
            <button class="btn btn-gold" :disabled="loading" @click="onExport">{{ loading ? 'Generando…' : 'Exportar' }}</button>
          </div>
        </div>

        <div v-if="previewData" class="mt-4">
          <h6 class="mb-2">Vista Previa <span v-if="previewData.totalAtenciones">- Total: {{ previewData.totalAtenciones }}</span></h6>
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
          <pre v-else class="bg-light p-2 small">{{ previewData }}</pre>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { defineComponent, ref, watch, computed } from 'vue'

export default defineComponent({
  name: 'ReportModalAdvanced',
  props: {
    visible: { type: Boolean, default: false },
    title: { type: String, default: 'Reporte' },
    filters: { type: Object, default: () => ({}) },
    // Functions: preview(filters) => Promise<any>, export(filters) => Promise<Blob|any>
    previewHandler: { type: Function, required: true },
    exportHandler: { type: Function, required: true },
  },
  emits: ['close', 'update:filters'],
  setup(props, { emit }) {
    const localFilters = ref({ ...props.filters })
    const consultaOptions = [
      { key: 'clinica', label: 'Clínica' },
      { key: 'especialidad', label: 'Especialidad' },
      { key: 'talla', label: 'Talla' },
      { key: 'peso', label: 'Peso' },
      { key: 'signosVitales', label: 'Signos vitales' },
      { key: 'exploracionFisica', label: 'Exploración física' },
      { key: 'examenes', label: 'Exámenes' },
      { key: 'receta', label: 'Receta' },
      { key: 'indicaciones', label: 'Indicaciones' },
      { key: 'diagnostico', label: 'Diagnóstico' },
      { key: 'cie10', label: 'CIE-10' },
      { key: 'pronostico', label: 'Pronóstico' },
      { key: 'seguimiento', label: 'Seguimiento' },
      { key: 'medico', label: 'Médico' },
    ]
    const loading = ref(false)
    const previewData = ref(null)

    // Inicializa los filtros locales y limpia la vista previa al abrir/cerrar el modal
    watch(() => props.visible, (v) => {
      if (v) {
        localFilters.value = { ...props.filters }
        if (!localFilters.value.formato) localFilters.value.formato = 'PDF'
        previewData.value = null
      } else {
        // Al cerrar, limpiar cualquier información previa
        previewData.value = null
      }
    }, { immediate: true })

    watch(() => props.filters, (val, oldVal) => {
      if (val !== oldVal && props.visible) {
        localFilters.value = { ...val }
        if (!localFilters.value.formato) localFilters.value.formato = 'PDF'
      }
    })

    function syncFilters() { emit('update:filters', { ...localFilters.value }) }

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
      const f = localFilters.value?.fechaInicio?.slice(0,10) || 'reporte'
      const fmt = (localFilters.value?.formato || 'PDF').toLowerCase()
      return `ReporteAtenciones_${f}.${fmt}`
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
      if (Array.isArray(previewData.value)) return previewData.value
      if (Array.isArray(previewData.value?.atenciones)) return previewData.value.atenciones
      return null
    })
    const previewColumns = computed(() => {
      const rows = previewRows.value
      if (rows && rows.length) return Object.keys(rows[0])
      return []
    })

    const antecedentesKeys = ['antPatologicos','antNoPatologicos','antFamiliares','antGinecoObstetricos']
    const allAntecedentes = computed({
      get: () => antecedentesKeys.every(k => !!localFilters.value[k]),
      set: (val) => antecedentesKeys.forEach(k => localFilters.value[k] = !!val)
    })

    const consultaKeys = consultaOptions.map(o => o.key)
    const allConsulta = computed({
      get: () => consultaKeys.every(k => !!localFilters.value[k]),
      set: (val) => consultaKeys.forEach(k => localFilters.value[k] = !!val)
    })

    return { localFilters, consultaOptions, loading, previewData, onPreview, onExport, previewRows, previewColumns, allAntecedentes, allConsulta }
  }
})
</script>

<style scoped>
.modal-backdrop { position: fixed; inset: 0; background: rgba(0,0,0,0.4); display: flex; align-items: center; justify-content: center; z-index: 1050; }
.modal-card { background: #fff; border-radius: 8px; width: min(1100px, 95vw); max-height: 90vh; overflow: auto; box-shadow: 0 10px 30px rgba(0,0,0,0.2); }
.modal-header { padding: 0.75rem 1rem; border-bottom: 1px solid #eee; }
.modal-body { padding: 1rem; }
.btn-close { background: transparent; border: none; font-size: 1.25rem; line-height: 1; }
.btn-gold { background-color: #C9A227; border-color: #C9A227; color: #111827; }
.btn-gold:hover { background-color: #B8931F; border-color: #B8931F; color: #111827; }
.btn-gray { background-color: #6B7280; border-color: #6B7280; color: #ffffff; }
.btn-gray:hover { background-color: #4B5563; border-color: #4B5563; }
.section-title { font-weight: 700; font-size: .9rem; color: #374151; text-transform: uppercase; margin-bottom: .5rem; }
.section-subtitle { font-weight: 600; font-size: .85rem; color: #6B7280; text-transform: uppercase; margin: .75rem 0 .25rem 0; }
.form-check { display: inline-flex; align-items: center; gap: .35rem; }
</style>