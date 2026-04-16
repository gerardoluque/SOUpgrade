<template>
  <div v-if="visible" class="entity-modal-backdrop" @click.self="cancel">
    <div ref="modalRoot" class="entity-modal">
      <div class="entity-modal-header">
        <h5>Búsqueda de {{ entityLabel }}</h5>
        <button class="close-btn" @click="cancel">&times;</button>
      </div>
      <div class="entity-modal-body">
        <!-- La barra de búsqueda ahora está embebida dentro del DataTable -->
       
        <div class="table-responsive modal-table">
         <DataTable
          :table-id="'entity-table'"
          :title="titleEntity"
          :description="descriptionEntity"
          :columns="columns"
          :rows="propsRows"
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
          @search="onDataTableSearch"
          @page-change="onDataTablePageChange"
        >
          <template #row-actions="{ row }">
            <button
              class="btn btn-sm btn-primary"
              :disabled="!canSelect(row)"
              @click="select(row)"
            >Seleccionar</button>
          </template>
          <template #header-actions>
            <slot name="header-actions"></slot>
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
                :key="'pg-'+p"
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
import { ref, computed, watch, onBeforeUnmount, nextTick } from "vue";
import DataTable from "@/components/widgets/DataTable.vue";

const props = defineProps({
  visible: Boolean,
  entityLabel: { type: String, default: "Entidad" },
  rows: { type: Array, default: () => [] },
  columns: { type: Array, default: () => [] }, // [{ key: 'nombre', label: 'Nombre' }]
  idField: { type: String, default: "id" },
  titleEntity: { type: String, default: "id" }, // campo para mostrar en el título  
  descriptionEntity: { type: String, default: "id" }, // campo para mostrar en el título  
  localSearch: { type: Boolean, default: false }, // si true filtra localmente, si false emite evento search
  totalPages: { type: Number, default: 1 }, // para paginación
  currentPage: { type: Number, default: 1 }, // para paginación
  loading: { type: Boolean, default: false },
  loadingSpinnerSrc: { type: String, default: '' },
  loadingMessage: { type: String, default: 'Obteniendo información...' }
});
const emits = defineEmits(["close", "select","update:visible","search"]);

const selected = ref(null);
const PAGE_SIZE_DEFAULT = 10;
const lastSearch = ref('');
let debounceTimer = null; // (ya no se usa, mantenido por compatibilidad mínima)
const modalRoot = ref(null)
const filterFields = ref([
 { value: 'nombre', label: 'Nombre' }
,{ value: 'matricula', label: 'Matrícula' }
,{ value: 'curp', label: 'CURP' }
,{ value: 'Expediente', label: 'Expediente' }]); // Opciones de filtro (pueden ser props o computadas)
// Computa opciones para DataTable (incluye 'Todos' si local)
const filterOptions = computed(() => {
  if (props.localSearch) {
    return [{ value: 'todos', label: 'Todos' }, ...filterFields.value];
  }
  return filterFields.value;
});


const currentPageLocal = ref(props.currentPage || 1);
const totalPagesLocal = ref(props.totalPages || 1);

// forward-prop rows (so template name doesn't conflict)
//const propsRows = computed(() => props.rows || []);

watch(() => props.currentPage, v => { currentPageLocal.value = v || 1; });
watch(() => props.totalPages, v => { totalPagesLocal.value = v || 1; });
const propsRows = computed(() => props.rows || []);


// DataTable maneja filtrado local; no se requiere filteredRows externo

function emitSearch(query, page = 1, field = null) {
  const usedField = field || (filterFields.value[0]?.value) || 'nombre';
  const payload = {
    query: query || '',
    search: query || '',
    filterField: usedField,
    page: page || 1,
    pageSize: PAGE_SIZE_DEFAULT
  };
  emits('search', payload);
}


/*
function onSearch(query) {
  // re-emit search to parent so it can call API/filter
  emits("search", query);
}
*/ 

function canSelect(row) {
  const raw = row?.existencias ?? row?.Existencias ?? row?.stock ?? row?.Stock ?? row?._stock;
  if (raw === undefined || raw === null || raw === '') return true;
  const stock = Number(raw);
  if (!Number.isFinite(stock)) return true;
  return stock > 0;
}

function select(row) {
  if (row && !canSelect(row)) return;
  // if row provided use it, otherwise fallback to selected.value
  const payload = row || selected.value;
  if (payload) {
    emits("select", payload);
  }
  // close modal for both v-model and close listener
  emits("update:visible", false);
  emits("close");
}
function cancel() {
  emits("update:visible", false);
  emits("close");
}

function onDataTablePageChange(page) {
  currentPageLocal.value = page || 1;
  emitSearch(lastSearch.value, currentPageLocal.value);
}

function onDataTableSearch(payload) {
  // payload puede ser string (búsqueda local) u objeto (búsqueda remota)
  if (typeof payload === 'string') {
    lastSearch.value = payload;
    currentPageLocal.value = 1;
    emitSearch(lastSearch.value);
  } else if (payload && typeof payload === 'object') {
    lastSearch.value = payload.search || '';
    // Para backend la página se gestiona externamente con dots; reiniciar a 1 si cambia término
    if (payload.searchChanged) currentPageLocal.value = 1; // opcional si se añade flag
    // Preserve any filterField coming from DataTable
    emitSearch(lastSearch.value, currentPageLocal.value, payload.filterField || null);
  }
}

 

// Eliminado: la entrada local ahora la maneja DataTable directamente

function changePage(delta) {
  const next = Math.max(1, currentPageLocal.value + delta);
  if (next === currentPageLocal.value) return;
  currentPageLocal.value = next;
  emitSearch(lastSearch.value, currentPageLocal.value);
}

function goToPage(page) {
  const p = Math.max(1, Math.min(page, totalPagesLocal.value));
  if (p === currentPageLocal.value) return;
  currentPageLocal.value = p;
  emitSearch(lastSearch.value, currentPageLocal.value);
}


watch(() => props.visible, v => {
  if (v) {
    lastSearch.value = '';
    selected.value = null;
    currentPageLocal.value = props.currentPage || 1;
    emitSearch('', currentPageLocal.value);
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
});

onBeforeUnmount(() => {
  if (debounceTimer) clearTimeout(debounceTimer);
});

// expose to template
defineExpose({ currentPageLocal, totalPagesLocal });

// Solo mostrar una ventana de páginas (3-5). Por defecto 5.
const WINDOW_SIZE = 5;
const visiblePages = computed(() => {
  const total = Math.max(1, Number(totalPagesLocal.value) || 1);
  const current = Math.max(1, Number(currentPageLocal.value) || 1);
  const size = Math.max(3, WINDOW_SIZE);

  let start = Math.max(1, current - Math.floor(size / 2));
  let end = start + size - 1;
  if (end > total) {
    end = total;
    start = Math.max(1, end - size + 1);
  }
  const pages = [];
  for (let p = start; p <= end; p++) pages.push(p);
  return pages;
});

// triggerSearch ya no es necesario; DataTable emite 'search'

</script>

<style scoped>

 

.entity-modal-backdrop {
  position: fixed;
  inset: 0;
  background: rgba(0,0,0,0.35);
  z-index: 2000;
  display: flex;
  align-items: flex-start;
  justify-content: center;
}

.entity-modal-body {
  padding: 1rem 1.5rem;
  flex: 1;
  min-height: 420px; /* Aproximadamente para 10 filas */
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

/* Toolbar externa eliminada; estilos conservados para compatibilidad mínima */


.table-responsive {
  flex: 1;
}

.modal-table {
  min-height: 0;
  max-height: min(60vh, 520px);
  overflow-y: auto;
  margin-bottom: 1rem;
  padding-bottom: 0.25rem;
}
.entity-modal {
  background: #fff;
  min-width: min(900px, 94vw);
  width: min(1280px, 90vw);
  max-height: 92vh;
  margin-top: 5vh;
  border-radius: 10px;
  box-shadow: 0 8px 32px rgba(0,0,0,0.18);
  display: flex;
  flex-direction: column;
  animation: slideIn .2s;
  overflow: hidden;
}
@keyframes slideIn {
  from { transform: translateY(-30px);}
  to { transform: translateY(0);}
}
.entity-modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem 1.5rem 0.5rem 1.5rem;
  border-bottom: 1px solid #eee;
}
.entity-modal-body {
  padding: 1rem 1.5rem;
  flex: 1;
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
.close-btn {
  background: none;
  border: none;
  font-size: 1.5rem;
  cursor: pointer;
}
.table-active {
  background: #e3f2fd !important;
}

/* Pagination custom styles */
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
  opacity: .4;
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
  max-width: 420px;
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