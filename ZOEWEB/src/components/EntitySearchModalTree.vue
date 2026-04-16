<template>
  <div v-if="visible" class="entity-modal-backdrop" @click.self="cancel">
    <div class="entity-modal">
      <div class="entity-modal-header">
        <h5>Búsqueda de {{ entityLabel }}</h5>
        <button class="close-btn" @click="cancel">&times;</button>
      </div>
      <div class="entity-modal-body">
        <div class="row g-2 align-items-center mb-2">
          <div class="col">
            <input
              v-model="search"
              type="search"
              class="form-control"
              placeholder="Buscar..."
              autofocus
              @input="onSearchInput"
            />
          </div>
        </div>

        <div class="table-responsive" style="max-height: 420px; overflow-y: auto;">
          <table class="table table-sm table-bordered align-middle">
            <thead class="table-light">
              <tr>
                <th style="width:40px">Sel</th>
                <th>Paquete</th>
                <th style="width:180px">Clave</th>
                <th>Observaciones</th>
                <th style="width:120px">Exámenes</th>
              </tr>
            </thead>
            <tbody>
              <template v-for="grp in grouped" :key="'p-' + grp.parent.id">
                <tr>
                  <td class="text-center">
                    <div class="d-flex align-items-center justify-content-center" style="gap:.5rem;">
                      <button
                        class="btn btn-sm btn-outline-secondary"
                        :disabled="grp.children.length === 0"
                        :title="isExpanded(grp.parent.id) ? 'Contraer' : 'Expandir'"
                        @click="toggleExpanded(grp.parent.id)"
                      >
                        <span v-if="grp.children.length === 0">—</span>
                        <span v-else>{{ isExpanded(grp.parent.id) ? '−' : '+' }}</span>
                      </button>
                      <input
                        type="checkbox"
                        :checked="grp.children.length > 0 ? isGroupAllSelected(grp) : isSelected(grp.parent.id)"
                        title="Seleccionar"
                        @change="onParentCheckboxChange(grp, $event.target.checked)"
                      />
                    </div>
                  </td>
                  <td>{{ grp.parent.paquete }}</td>
                  <td>{{ grp.parent.clave }}</td>
                  <td>{{ grp.parent.descripcion }}</td>
                  <td class="text-center fw-semibold">{{ grp.children.length }}</td>
                </tr>
                <tr v-for="ch in grp.children" v-show="isExpanded(grp.parent.id)" :key="'c-' + ch.id">
                  <td class="text-center">
                    <input
                      type="checkbox"
                      :checked="isSelected(ch.id)"
                      title="Seleccionar"
                      @change="toggleSelected(ch, $event.target.checked)"
                    />
                  </td>
                  <td class="ps-4">• {{ ch.paquete }}</td>
                  <td>{{ ch.clave }}</td>
                  <td>{{ ch.descripcion }}</td>
                  <td></td>
                </tr>
              </template>
              <tr v-if="grouped.length === 0">
                <td colspan="5" class="text-center text-muted small">Sin resultados</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
      <div class="entity-modal-footer">
        <div class="me-auto d-flex gap-2">
          <button class="btn btn-outline-primary" @click="selectAllVisible">Seleccionar todos</button>
          <button class="btn btn-primary" :disabled="selectedIds.size === 0" @click="addSelected">Agregar examen</button>
        </div>
        <div v-if="totalPagesLocal > 1" class="pagination-wrapper">
          <button class="circle-btn nav-btn" :disabled="currentPageLocal <= 1" title="Página anterior" @click="changePage(-1)">
            <span class="arrow left"></span>
          </button>
          <div class="page-dots">
            <button
              v-for="p in totalPagesLocal"
              :key="'pg-'+p"
              class="dot-btn"
              :class="{ active: p === currentPageLocal }"
              :title="'Ir a página ' + p"
              @click="goToPage(p)"
            >{{ p }}</button>
          </div>
          <button class="circle-btn nav-btn" :disabled="currentPageLocal >= totalPagesLocal" title="Página siguiente" @click="changePage(1)">
            <span class="arrow right"></span>
          </button>
        </div>
        <button class="btn btn-secondary" @click="cancel">Cancelar</button>
      </div>
    </div>
  </div>
  
</template>

<script setup>
import { computed, ref, watch } from 'vue'

const props = defineProps({
  visible: Boolean,
  entityLabel: { type: String, default: 'Entidad' },
  rows: { type: Array, default: () => [] },
  currentPage: { type: Number, default: 1 },
  totalPages: { type: Number, default: 1 },
})
const emit = defineEmits(['update:visible', 'close', 'select', 'search'])

const search = ref('')
const currentPageLocal = ref(props.currentPage || 1)
const totalPagesLocal = ref(props.totalPages || 1)
const selectedIds = ref(new Set())
const selectedItems = ref(new Map())

watch(() => props.currentPage, v => currentPageLocal.value = v || 1)
watch(() => props.totalPages, v => totalPagesLocal.value = v || 1)
watch(() => props.visible, v => {
  if (v) {
    search.value = ''
    currentPageLocal.value = props.currentPage || 1
    emit('search', { query: '', page: currentPageLocal.value })
  }
})

function toNumberIfPossible(v) {
  if (v === null || v === undefined || v === '') return null
  const n = Number(v)
  return Number.isFinite(n) ? n : String(v)
}
function normParentId(v) {
  if (v === null || v === undefined || v === '' || Number(v) === 0) return null
  const n = Number(v)
  return Number.isFinite(n) ? n : String(v)
}
function mapItem(r) {
  const rawId = r.id ?? r.Id
  const rawPadre = r.padreId ?? r.PadreId ?? r.parentId ?? r.ParentId ?? null
  const id = toNumberIfPossible(rawId)
  const padreId = normParentId(rawPadre)
  return {
    ...r,
    id: id,
    padreId: padreId,
    paquete: r.paquete ?? r.Paquete ?? r.examen ?? r.Examen ?? '',
    clave: r.clave ?? r.Clave ?? '',
    descripcion: r.descripcion ?? r.Descripcion ?? r.observaciones ?? r.Observaciones ?? ''
  }
}

const grouped = computed(() => {
  const items = Array.isArray(props.rows) ? props.rows.map(mapItem) : []
  const parents = items.filter(x => x.padreId === null)
  const byParent = items.reduce((acc, it) => {
    if (it.padreId !== null) {
      const key = String(it.padreId)
      ;(acc[key] = acc[key] || []).push(it)
    }
    return acc
  }, {})
  return parents.map(p => ({ parent: p, children: byParent[String(p.id)] || [] }))
})

// Expand/collapse state per parent id
const expandedParents = ref(new Set())
function isExpanded(parentId) { return expandedParents.value.has(parentId) }
function toggleExpanded(parentId) {
  const s = new Set(expandedParents.value)
  if (s.has(parentId)) s.delete(parentId)
  else s.add(parentId)
  expandedParents.value = s
}

let debounceTimer = null
const DEBOUNCE_MS = 300
function onSearchInput() {
  if (debounceTimer) clearTimeout(debounceTimer)
  debounceTimer = setTimeout(() => {
    emit('search', { query: search.value || '', page: 1 })
  }, DEBOUNCE_MS)
}

function changePage(delta) {
  const next = Math.max(1, currentPageLocal.value + delta)
  if (next === currentPageLocal.value) return
  currentPageLocal.value = next
  emit('search', { query: search.value || '', page: currentPageLocal.value })
}
function goToPage(p) {
  const page = Math.max(1, Math.min(p, totalPagesLocal.value))
  if (page === currentPageLocal.value) return
  currentPageLocal.value = page
  emit('search', { query: search.value || '', page })
}

function cancel() {
  emit('update:visible', false)
  emit('close')
}

function isSelected(id) { return selectedIds.value.has(id) }
function toggleSelected(item, checked) {
  const id = item?.id
  if (id == null) return
  if (checked) {
    const newSet = new Set(selectedIds.value)
    newSet.add(id)
    selectedIds.value = newSet
    // Guardar snapshot del item actual
    const newMap = new Map(selectedItems.value)
    newMap.set(id, mapItem(item))
    selectedItems.value = newMap
  } else {
    const newSet = new Set(selectedIds.value)
    newSet.delete(id)
    selectedIds.value = newSet
    const newMap = new Map(selectedItems.value)
    newMap.delete(id)
    selectedItems.value = newMap
  }
}
function addSelected() {
  // Emitir un select por cada elemento seleccionado
  for (const id of selectedIds.value) {
    const it = selectedItems.value.get(id)
    if (it) emit('select', it)
  }
  // Limpiar y cerrar
  selectedIds.value = new Set()
  selectedItems.value = new Map()
  emit('update:visible', false)
  emit('close')
}

function isGroupAllSelected(grp) {
  if (!grp || grp.children.length === 0) return false
  return grp.children.every(ch => selectedIds.value.has(ch.id))
}
function onParentCheckboxChange(grp, checked) {
  if (!grp) return
  if (grp.children.length > 0) {
    // Toggle todos los hijos
    grp.children.forEach(ch => toggleSelected(ch, checked))
  } else {
    toggleSelected(grp.parent, checked)
  }
}
function getVisibleSelectableItems() {
  const out = []
  grouped.value.forEach(grp => {
    if (grp.children.length > 0) {
      grp.children.forEach(ch => out.push(ch))
    } else {
      out.push(grp.parent)
    }
  })
  return out
}
function selectAllVisible() {
  const items = getVisibleSelectableItems()
  items.forEach(it => toggleSelected(it, true))
}
</script>

<style scoped>
.entity-modal-backdrop { position: fixed; inset: 0; background: rgba(0,0,0,0.35); z-index: 2000; display: flex; align-items: flex-start; justify-content: center; }
.entity-modal { background: #fff; min-width: 700px; max-width: 98vw; max-height: 90vh; margin-top: 5vh; border-radius: 10px; box-shadow: 0 8px 32px rgba(0,0,0,0.18); display: flex; flex-direction: column; overflow: auto; }
.entity-modal-header { display: flex; justify-content: space-between; align-items: center; padding: 1rem 1.5rem 0.5rem 1.5rem; border-bottom: 1px solid #eee; }
.entity-modal-body { padding: 1rem 1.5rem; flex: 1; }
.entity-modal-footer { padding: 1rem 1.5rem; border-top: 1px solid #eee; display: flex; gap: 1rem; justify-content: flex-end; }
.close-btn { background: none; border: none; font-size: 1.5rem; cursor: pointer; }
.pagination-wrapper { display: flex; align-items: center; gap: .75rem; margin-right: auto; }
.circle-btn { width: 38px; height: 38px; border-radius: 50%; border: 2px solid var(--bs-primary, #0d6efd); background: #fff; display: flex; align-items: center; justify-content: center; padding: 0; }
.circle-btn:disabled { opacity: .4; cursor: not-allowed; }
.arrow { position: relative; width: 14px; height: 14px; display: inline-block; }
.arrow::before { content: ''; position: absolute; top: 50%; left: 50%; width: 10px; height: 10px; border-top: 2px solid var(--bs-primary, #0d6efd); border-right: 2px solid var(--bs-primary, #0d6efd); transform: translate(-50%, -50%) rotate(45deg); }
.arrow.left::before { transform: translate(-50%, -50%) rotate(225deg); }
.arrow.right::before { transform: translate(-50%, -50%) rotate(45deg); }
.page-dots { display: flex; gap: .5rem; flex-wrap: wrap; max-width: 420px; }
.dot-btn { min-width: 38px; height: 38px; border-radius: 50%; border: 1px solid var(--bs-primary, #0d6efd); background: #fff; color: var(--bs-primary, #0d6efd); font-size: .8rem; font-weight: 600; display: flex; align-items: center; justify-content: center; padding: 0; line-height: 1; }
.dot-btn.active, .dot-btn:hover { background: var(--bs-primary, #0d6efd); color: #fff; }
</style>
