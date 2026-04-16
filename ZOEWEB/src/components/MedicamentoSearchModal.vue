<template>
  <div v-if="visible" class="entity-modal-backdrop" @click.self="cancel">
    <div ref="modalRoot" class="entity-modal">
      <div class="entity-modal-header">
        <h5>Búsqueda de Medicamento</h5>
        <button class="close-btn" @click="cancel">&times;</button>
      </div>
      <div class="entity-modal-body">
        <div class="modal-table table-responsive">
          <DataTable
            :table-id="tableId"
            :title="titleEntity"
            :description="descriptionEntity"
            :columns="displayColumns"
            :rows="filteredRows"
            :searchable="true"
            :exportar="false"
            :current-page="currentPage"
            :total-pages="totalPages"
            :filter-options="filterOptions"
            :use-external-pagination="!localSearch"
            :loading="loading"
            :loading-spinner-src="loadingSpinnerSrc"
            :loading-message="loadingMessage"
            :show-footer="false"
            :sticky-actions="true"
            @search="onDataTableSearch"
            @page-change="onDataTablePageChange"
          >
            <template #row-actions="{ row }">
              <button
                class="btn btn-sm btn-primary"
                :disabled="!canSelect(row) || isSelecting(row)"
                @click="select(row)"
              >Seleccionar</button>
            </template>
            <template #header-actions>
              <div class="med-modal-header">
                <div v-if="showControlFilter" class="control-filter">
                  <label class="form-label mb-0 small">{{ controlFilterLabel }}</label>
                  <select
                    v-model="controlFilterProxy"
                    class="form-select form-select-sm"
                    :disabled="controlFilterDisabled"
                  >
                    <option v-for="opt in normalizedControlOptions" :key="opt.value" :value="opt.value">{{ opt.label }}</option>
                  </select>
                </div>
                <slot name="header-actions"></slot>
              </div>
            </template>
          </DataTable>
        </div>
      </div>
      <div class="entity-modal-footer">
        <div class="footer-actions">
          <div v-if="totalPages > 1" class="pagination-wrapper">
            <button
              class="circle-btn nav-btn"
              :disabled="currentPageLocal <= 1"
              title="Página anterior"
              @click="changePage(-1)"
            >
              <span class="arrow left"></span>
            </button>
            <div class="page-dots">
              <button
                v-for="p in visiblePages"
                :key="'pg-' + p"
                class="dot-btn"
                :class="{ active: p === currentPageLocal }"
                :title="'Ir a página ' + p"
                @click="goToPage(p)"
              >{{ p }}</button>
            </div>
            <button
              class="circle-btn nav-btn"
              :disabled="currentPageLocal >= totalPagesLocal"
              title="Página siguiente"
              @click="changePage(1)"
            >
              <span class="arrow right"></span>
            </button>
          </div>
          <button class="btn btn-secondary" @click="cancel">Cancelar</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed, ref, watch, onBeforeUnmount, nextTick } from 'vue'
import { useMainStore } from '@/store/useMainStore'
import { TipoRoles } from '@/enums/enums.js'
import DataTable from '@/components/widgets/DataTable.vue'

const props = defineProps({
  visible: { type: Boolean, default: false },
  rows: { type: Array, default: () => [] },
  columns: { type: Array, default: () => [] },
  idField: { type: String, default: 'id' },
  titleEntity: { type: String, default: 'Buscar Medicamento' },
  descriptionEntity: { type: String, default: 'Lista de Medicamentos' },
  localSearch: { type: Boolean, default: false },
  totalPages: { type: Number, default: 1 },
  currentPage: { type: Number, default: 1 },
  loading: { type: Boolean, default: false },
  loadingSpinnerSrc: { type: String, default: '' },
  loadingMessage: { type: String, default: 'Obteniendo información...' },
  tableId: { type: String, default: 'medicamentos-search-table' },
  controlFilterOptions: { type: Array, default: () => [] },
  controlFilterValue: { type: [String, Number], default: '' },
  controlFilterLabel: { type: String, default: 'Tipo de control' },
  controlFilterDisabled: { type: Boolean, default: false },
  enableSelection: { type: Boolean, default: false }
  ,
  allowZeroStockSelection: { type: Boolean, default: false }
  ,
  allowZeroStock: { type: Boolean, default: false }
})

const emits = defineEmits([
  'close',
  'select',
  'update:visible',
  'search',
  'update:controlFilterValue'
])

const PAGE_SIZE_DEFAULT = 10
const lastSearch = ref('')
let debounceTimer = null
const modalRoot = ref(null)

const filterFields = ref([{ value: 'sustanciaActiva', label: 'Nombre' }])
const filterOptions = computed(() => {
  if (props.localSearch) {
    return [{ value: 'todos', label: 'Todos' }, ...filterFields.value]
  }
  return filterFields.value
})

const currentPageLocal = ref(props.currentPage || 1)
const totalPagesLocal = ref(props.totalPages || 1)

watch(() => props.currentPage, v => { currentPageLocal.value = v || 1 })
watch(() => props.totalPages, v => { totalPagesLocal.value = v || 1 })

const enhancedRows = computed(() => {
  const sourceRows = Array.isArray(props.rows) ? props.rows : []
  return sourceRows.map(row => {
    const next = { ...row, _originalRow: row }
    if (next.TipoControl === undefined || next.TipoControl === null || next.TipoControl === '') {
      const label = row.TipoControlDescripcion
        ?? row.tipoControlDescripcion
        ?? row.TipoControl
        ?? row.tipoControl
        ?? ''
      if (label) {
        next.TipoControl = String(label)
      } else {
        const controlId = Number(row.tipocontrolid ?? row.tipoControlId ?? row.TipoControlId ?? row.TipoControlID ?? -1)
        if (Number.isFinite(controlId)) {
          if (controlId > 0) next.TipoControl = 'Controlado'
          else if (controlId === 0) next.TipoControl = 'No controlado'
        }
      }
    }
    if (next.TipoControl === undefined && next.tipoControl !== undefined) {
      next.TipoControl = next.tipoControl
    }
    // ensure presentacion comes from presentacionNombre when available
    if (!next.presentacion || String(next.presentacion).trim() === '') {
      const pres = row?.presentacionNombre ?? row?.PresentacionNombre ?? row?.presentacion ?? row?.Presentacion
      if (pres !== undefined && pres !== null) next.presentacion = String(pres)
    }
    // empaque (numeric) if provided by API
    if (next.empaque === undefined || next.empaque === null) {
      const emp = row?.empaque ?? row?.Empaque ?? row?.pack ?? row?.paquete
      if (emp !== undefined && emp !== null) next.empaque = emp
    }
    // map informacionTerapeutica field
    if (!next.informacionTerapeutica || String(next.informacionTerapeutica).trim() === '') {
      const info = row?.informacionTerapeutica ?? row?.informacion_terapeutica ?? row?.InformacionTerapeutica ?? row?.informacion
      if (info !== undefined && info !== null) next.informacionTerapeutica = String(info)
    }
    // normalize reserved and available stock fields
    const rawExist = row?.existencias ?? row?.Existencias ?? row?.stock ?? row?.Stock ?? row?._stock
    const rawReserv = row?.cantidadReservada ?? row?.CantidadReservada ?? row?.reservado ?? row?.CantidadReservada ?? 0
    const existNum = Number(rawExist)
    const reservNum = Number(rawReserv)
    const providedDispon = row?.existenciaDisponible ?? row?.ExistenciaDisponible ?? row?.existencia_disponible ?? row?.existenciaDisponible
    let disponNum = null
    if (providedDispon !== undefined && providedDispon !== null && providedDispon !== '') {
      const p = Number(providedDispon)
      disponNum = Number.isFinite(p) ? p : null
    } else if (Number.isFinite(existNum) || Number.isFinite(reservNum)) {
      const left = (Number.isFinite(existNum) ? existNum : 0) - (Number.isFinite(reservNum) ? reservNum : 0)
      disponNum = Number.isFinite(left) ? left : null
    }
    next.cantidadReservada = Number.isFinite(reservNum) ? reservNum : 0
    next.existenciaDisponible = disponNum
    // inexistente flag (boolean). If source has any variant indicating inexistence, treat as true. Null/undefined => false
    const rawInexist = row?.inexistente ?? row?.Inexistente ?? row?.esInexistente ?? row?.inexistenteFlag ?? row?.inexistenteMovimiento ?? false
    const inexBool = rawInexist === true || String(rawInexist).toLowerCase() === 'true'
    next._inexistenteFlag = !!inexBool
    // Display value: 'SI' if true, 'NO' if false
    next.inexistente = next._inexistenteFlag ? 'SI' : 'NO'
    return next
  })
})

  // Main store: used to filter medications by corporation when needed
  const mainStore = useMainStore()

  function isUserDirectivo() {
    try {
      const role = mainStore.userRol || null
      if (role) {
        // role may have id or name
        const rid = role.id ?? role.Id ?? role.rolId ?? role.rol ?? null
        if (rid !== undefined && rid !== null) {
          if (Number(rid) === Number(TipoRoles.Directivo)) return true
        }
        const name = (role.name ?? role.nombre ?? role.descr ?? '')
        if (typeof name === 'string') {
          const n = name.toLowerCase()
          if (n.includes('direct') || n.includes('direccion') || n.includes('dirección')) return true
          if (n === 'administrador 01' || n.includes('administrador 01')) return true
        }
      }
      // fallback: check userdata role id
      const udRid = mainStore.userdata?.rolId ?? mainStore.userdata?.rolid ?? null
      if (udRid !== undefined && udRid !== null) {
        if (Number(udRid) === Number(TipoRoles.Directivo)) return true
      }
      // also check userdata role name variants
      try {
        const udName = (mainStore.userdata && (mainStore.userdata.rolName ?? mainStore.userdata.rolNombre ?? (mainStore.userdata.rol && mainStore.userdata.rol.name))) || null
        if (udName && typeof udName === 'string') {
          const un = udName.toLowerCase()
          if (un.includes('direct') || un.includes('direccion') || un.includes('dirección') || un === 'administrador 01' || un.includes('administrador 01')) return true
        }
      } catch (e) { /* ignore */ }
    } catch (e) { /* ignore */ }
    return false
  }

  function rowCorporationId(row) {
    if (!row) return null
    return row.corporacionid ?? row.corporacionId ?? row.CorporacionId ?? row.corporacion ?? row.Corporacion ?? null
  }

  const filteredRows = computed(() => {
    const all = enhancedRows.value || []
    if (isUserDirectivo()) return all
    // determine user's active corporation id
    const corpId = mainStore.coporacionSelectedId || mainStore.coporacionSelectedId === 0 ? mainStore.coporacionSelectedId : (Array.isArray(mainStore.usercorporations) && mainStore.usercorporations.length === 1 ? mainStore.usercorporations[0] : null)
    if (!corpId && corpId !== 0) return all
    const key = String(corpId)
    return all.filter(r => {
      const rid = rowCorporationId(r)
      if (rid === undefined || rid === null) return false
      return String(rid) === key
    })
  })

const displayColumns = computed(() => {
  // start from provided columns but remove any 'Tipo' column
  const base = Array.isArray(props.columns) ? [...props.columns].filter(c => String(c).toLowerCase() !== 'tipo') : []

  // If rows indicate TipoControl exists, ensure column is present
  const sample = enhancedRows.value.find(row => row.TipoControl !== undefined || row.tipoControl !== undefined)
  if (sample && !base.some(col => String(col).toLowerCase() === 'tipocontrol')) {
    const insertAfterTipo = base.findIndex(col => String(col).toLowerCase() === 'tipo')
    if (insertAfterTipo >= 0) {
      base.splice(insertAfterTipo + 1, 0, 'TipoControl')
    } else {
      base.push('TipoControl')
    }
  }

  // Ensure presentación and informacion fields exist (they may be shown as columns elsewhere)
  if (!base.some(c => String(c).toLowerCase() === 'presentacion')) {
    // don't force-add unless props originally included them; keep behavior minimal
  }

  // Insert reserved and disponibilidad columns after existencia/existencias if present
  const lowerCols = base.map(c => String(c).toLowerCase())
  const existIdx = lowerCols.findIndex(c => c === 'existencias' || c.includes('exist'))
  if (existIdx >= 0) {
    // avoid duplicates
    const toInsert = []
    if (!lowerCols.includes('cantidadreservada')) toInsert.push('cantidadReservada')
    if (!lowerCols.includes('existenciadisponible')) toInsert.push('existenciaDisponible')
    if (toInsert.length) base.splice(existIdx + 1, 0, ...toInsert)
  }

  // Ensure TipoControl and sustanciaActiva are the first two columns (in that order)
  const desiredFirst = ['TipoControl', 'sustanciaActiva']
  // remove existing occurrences (case-insensitive)
  desiredFirst.forEach(name => {
    const idx = base.findIndex(c => String(c).toLowerCase() === String(name).toLowerCase())
    if (idx >= 0) base.splice(idx, 1)
  })
  // unshift in reverse so order is preserved
  for (let i = desiredFirst.length - 1; i >= 0; i--) {
    base.unshift(desiredFirst[i])
  }

  // Add 'inexistente' column if not present so UI shows the boolean flag
  if (!base.some(c => String(c).toLowerCase() === 'inexistente')) {
    base.push('inexistente')
  }

  return base
})

// selecting freeze to prevent double clicks
const selectingIds = ref(new Set())
function isSelecting(row) {
  try {
    const payload = row?._originalRow ?? row
    const idVal = payload?.[props.idField]
    if (idVal === undefined || idVal === null) return false
    return selectingIds.value.has(String(idVal))
  } catch { return false }
}

const normalizedControlOptions = computed(() => {
  const base = props.controlFilterOptions && props.controlFilterOptions.length
    ? props.controlFilterOptions
    : []
  const normalized = base.map(opt => ({
    value: opt.value ?? opt.id ?? opt.key ?? '',
    label: opt.label ?? opt.name ?? opt.descripcion ?? opt.descripcionCorta ?? String(opt.value ?? opt.id ?? '')
  }))
  if (!normalized.some(opt => opt.value === '')) {
    return [{ value: '', label: 'Todos' }, ...normalized]
  }
  return normalized
})

const showControlFilter = computed(() => normalizedControlOptions.value.length > 1)

const controlFilterProxy = computed({
  get: () => String(props.controlFilterValue ?? ''),
  set: value => {
    emits('update:controlFilterValue', value)
  }
})

function emitSearch(query, page = 1) {
  const field = filterFields.value[0]?.value || 'sustanciaActiva'
  const payload = {
    query: query || '',
    search: query || '',
    filterField: field,
    page: page || 1,
    pageSize: PAGE_SIZE_DEFAULT
  }
  emits('search', payload)
}

function canSelect(row) {
  // If parent didn't enable selection, disable selection entirely
  if (!props.enableSelection) return false

  // If the row is marked 'inexistente' (true), selection is disabled
  try {
    const rflag = (row && (row._inexistenteFlag === true)) || (row && row._originalRow && row._originalRow._inexistenteFlag === true)
    if (rflag) return false
  } catch (e) { /* ignore */ }

  // prefer normalized existenciaDisponible from enhancedRows; fall back to legacy existencias
  const proxy = row?._originalRow ? (row.existenciaDisponible !== undefined ? row.existenciaDisponible : null) : null
  if (proxy !== null && proxy !== undefined) {
    const num = Number(proxy)
    if (!Number.isFinite(num)) return true
    return num > 0 || props.allowZeroStockSelection || props.allowZeroStock
  }

  const sourceRow = row?._originalRow ?? row
  const raw = sourceRow?.existenciaDisponible ?? sourceRow?.ExistenciaDisponible ?? sourceRow?.existencias ?? sourceRow?.Existencias ?? sourceRow?.stock ?? sourceRow?.Stock ?? sourceRow?._stock
  if (raw === undefined || raw === null || raw === '') return true
  const stock = Number(raw)
  if (!Number.isFinite(stock)) return true
  return stock > 0 || props.allowZeroStockSelection || props.allowZeroStock
}

function select(row) {
  if (row && !canSelect(row)) return
  const payload = row?._originalRow ?? row
  const idVal = payload?.[props.idField]
  const key = idVal !== undefined && idVal !== null ? String(idVal) : null
  try {
    if (key) selectingIds.value.add(key)
    if (payload) {
      emits('select', payload)
    }
  } finally {
    // ensure modal closes; clear selecting state after next tick so button remains frozen briefly
    emits('update:visible', false)
    emits('close')
    if (key) setTimeout(() => selectingIds.value.delete(key), 250)
  }
}

function cancel() {
  emits('update:visible', false)
  emits('close')
}

function onDataTablePageChange(page) {
  currentPageLocal.value = page || 1
  emitSearch(lastSearch.value, currentPageLocal.value)
}

function onDataTableSearch(payload) {
  if (typeof payload === 'string') {
    lastSearch.value = payload
    currentPageLocal.value = 1
    emitSearch(lastSearch.value)
  } else if (payload && typeof payload === 'object') {
    lastSearch.value = payload.search || ''
    emitSearch(lastSearch.value, currentPageLocal.value)
  }
}

function changePage(delta) {
  const next = Math.max(1, currentPageLocal.value + delta)
  if (next === currentPageLocal.value) return
  currentPageLocal.value = next
  emitSearch(lastSearch.value, currentPageLocal.value)
}

function goToPage(page) {
  const p = Math.max(1, Math.min(page, totalPagesLocal.value))
  if (p === currentPageLocal.value) return
  currentPageLocal.value = p
  emitSearch(lastSearch.value, currentPageLocal.value)
}

watch(() => props.visible, v => {
  if (v) {
    lastSearch.value = ''
    currentPageLocal.value = props.currentPage || 1
    emitSearch('', currentPageLocal.value)
    // Ensure the search input inside the DataTable receives focus when modal opens
    nextTick(() => {
      try {
        const root = modalRoot?.value
        if (root) {
          const input = root.querySelector("input[placeholder='Buscar...']")
          if (input && typeof input.focus === 'function') {
            input.focus()
            const val = input.value || ''
            input.setSelectionRange && input.setSelectionRange(val.length, val.length)
          }
        }
      } catch (e) { /* noop */ }
    })
  }
})

onBeforeUnmount(() => {
  if (debounceTimer) clearTimeout(debounceTimer)
})

const WINDOW_SIZE = 5
const visiblePages = computed(() => {
  const total = Math.max(1, Number(totalPagesLocal.value) || 1)
  const current = Math.max(1, Number(currentPageLocal.value) || 1)
  const size = Math.max(3, WINDOW_SIZE)

  let start = Math.max(1, current - Math.floor(size / 2))
  let end = start + size - 1
  if (end > total) {
    end = total
    start = Math.max(1, end - size + 1)
  }
  const pages = []
  for (let p = start; p <= end; p++) pages.push(p)
  return pages
})
</script>

<style scoped>
.entity-modal-backdrop {
  position: fixed;
  inset: 0;
  background: rgba(0,0,0,0.35);
  z-index: 2100;
  display: flex;
  align-items: flex-start;
  justify-content: center;
}

.entity-modal {
  background: #fff;
  min-width: min(900px, 94vw);
  width: min(1280px, 90vw);
  max-height: 92vh;
  margin-top: 4vh;
  border-radius: 10px;
  box-shadow: 0 12px 48px rgba(0,0,0,0.18);
  display: flex;
  flex-direction: column;
  animation: slideIn .2s;
  overflow: hidden;
}

@keyframes slideIn {
  from { transform: translateY(-24px); }
  to { transform: translateY(0); }
}

.entity-modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem 1.5rem 0.75rem 1.5rem;
  border-bottom: 1px solid #eee;
}

.entity-modal-body {
  padding: 1rem 1.5rem;
  flex: 1;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.modal-table {
  flex: 1;
  max-height: min(60vh, 540px);
  overflow-y: auto;
}

.entity-modal-footer {
  padding: 1rem 1.5rem;
  border-top: 1px solid #eee;
  display: flex;
  flex-direction: column;
  gap: 1rem;
  background: #fff;
  position: sticky;
  bottom: 0;
  z-index: 5;
  box-shadow: 0 -6px 12px rgba(0,0,0,0.05);
}

.footer-actions {
  width: 100%;
  display: flex;
  flex-wrap: wrap;
  gap: 1rem;
  justify-content: space-between;
  align-items: center;
}

.pagination-wrapper {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  flex-wrap: wrap;
}

.circle-btn {
  width: 38px;
  height: 38px;
  border-radius: 50%;
  border: 2px solid var(--bs-primary, #0d6efd);
  background: #fff;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 0;
  transition: background .15s, color .15s;
}

.circle-btn:disabled {
  opacity: .35;
  cursor: not-allowed;
}

.circle-btn:not(:disabled):hover {
  background: var(--bs-primary, #0d6efd);
}

.circle-btn:not(:disabled):hover .arrow::before {
  border-color: #fff;
}

.arrow {
  position: relative;
  width: 14px;
  height: 14px;
  display: inline-block;
}

.arrow::before {
  content: '';
  position: absolute;
  top: 50%;
  left: 50%;
  width: 10px;
  height: 10px;
  border-top: 2px solid var(--bs-primary, #0d6efd);
  border-right: 2px solid var(--bs-primary, #0d6efd);
  transform: translate(-50%, -50%) rotate(45deg);
  transition: border-color .15s;
}

.arrow.left::before { transform: translate(-50%, -50%) rotate(225deg); }
.arrow.right::before { transform: translate(-50%, -50%) rotate(45deg); }

.page-dots {
  display: flex;
  gap: .5rem;
  flex-wrap: wrap;
  max-width: 480px;
}

.dot-btn {
  min-width: 38px;
  height: 38px;
  border-radius: 50%;
  border: 1px solid var(--bs-primary, #0d6efd);
  background: #fff;
  color: var(--bs-primary, #0d6efd);
  font-size: .8rem;
  font-weight: 600;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 0;
  line-height: 1;
  transition: background .15s, color .15s, border-color .15s;
}

.dot-btn:hover { background: var(--bs-primary, #0d6efd); color: #fff; }
.dot-btn.active { background: var(--bs-primary, #0d6efd); color:#fff; }
.dot-btn:disabled { opacity:.4; cursor:not-allowed; }

.close-btn {
  background: none;
  border: none;
  font-size: 1.5rem;
  cursor: pointer;
}

.med-modal-header {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  flex-wrap: wrap;
}

.control-filter {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

@media (max-width: 1024px) {
  .footer-actions {
    flex-direction: column;
    align-items: stretch;
  }
  .footer-actions > .btn {
    width: 100%;
  }
  .pagination-wrapper {
    width: 100%;
    justify-content: center;
  }
}

@media (max-width: 768px) {
  .entity-modal {
    min-width: 0;
    width: 96vw;
  }
  .pagination-wrapper .circle-btn,
  .pagination-wrapper .dot-btn {
    flex: 0 0 38px;
  }
}
</style>
