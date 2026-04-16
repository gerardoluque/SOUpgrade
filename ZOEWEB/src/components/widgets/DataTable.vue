<template>
  <div ref="rootRef" class="py-4 container-fluid">
    <div class="mt-4 row">
      <div class="col-12">
        <div class="card">
          <div class="card-header d-flex justify-content-between align-items-center">
            <div>
              <h5 class="mb-0">{{ title }}</h5>
              <p class="mb-0 text-sm">{{ description }}</p>
            </div>
            <!--download  csv file-->
            <div class="d-flex align-items-center gap-2" style="min-width: 300px;">
            
              <div v-if="searchable" class="search-container flex-grow-1">
                <div v-if="showAdvancedSearch" class="d-flex align-items-center gap-2">
                  <select v-model="selectedFilter" class="form-select" style="max-width:160px;">
                    <option v-for="opt in internalFilterOptions" :key="opt.value" :value="opt.value">{{ opt.label }}</option>
                  </select>
                  <input
                    ref="searchInputRef"
                    v-model="searchQuery"
                    type="text"
                    class="form-control search-input-fit"
                    autofocus
                    placeholder="Especifique texto de Busqueda..."
                    @keyup.enter="triggerSearch"
                  />
                  <material-button color="primary" variant="gradient" @click="triggerSearch">Buscar</material-button>
                </div>
                <input
                  v-else
                  ref="searchInputRef"
                  v-model="searchQuery"
                  type="text"
                  class="form-control search-input-fit"
                  autofocus
                  placeholder="Especifique texto de Busqueda..."
                />
              </div>  
              <!-- custom header actions slot (e.g., Buscar button) -->
              <slot name="header-actions"></slot>
              <div v-if="exportar">
                <material-button v-permiso="permisoExportar" color="primary" variant="gradient" @click="downloadCSV">
                  Descargar CSV
                </material-button>
              </div>
            </div>
          </div>

            <!-- Table Content with overlay while loading -->
          <div class="table-responsive position-relative">
            <!-- Overlay when loading (progress or explicit loading flag) -->
            <transition name="fade">
              <div v-if="isLoading" class="loading-overlay text-center">
                <div v-if="loadingSpinnerSrc" class="spinner-wrapper">
                  <img :src="loadingSpinnerSrc" :alt="loadingMessage" class="loading-gif" />
                </div>
                <div v-else class="spinner-dots" aria-label="Cargando">
                  <span></span><span></span><span></span>
                </div>
                <div class="loading-text mt-3 text-muted">{{ loadingMessage }}</div>
              </div>
            </transition>

            <table :id="tableId" class="table table-flush" :class="{ 'table-disabled': isLoading }">
              <thead class="thead-light">
                <tr>
                  <th
                    v-if="actionsPosition === 'start' && $slots['row-actions']"
                    :class="[
                      'text-uppercase text-secondary text-xxs font-weight-bolder opacity-7',
                      hasStickyActions ? 'sticky-column sticky-column-left' : ''
                    ]"
                  >Accion</th>
                  <th
                    v-for="(column, index) in columns" :key="index"
                    class="text-uppercase text-secondary text-xxs font-weight-bolder opacity-7">
                    {{ column }}
                  </th>
                  <th
                    v-if="actionsPosition === 'end' && $slots['row-actions']"
                    :class="[
                      'text-uppercase text-secondary text-xxs font-weight-bolder opacity-7',
                      hasStickyActions ? 'sticky-column sticky-column-right' : ''
                    ]"
                  >Accion</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(row, rowIndex) in paginatedRows" :key="rowIndex" :class="row._rowClass || ''">
                  <!-- Actions Column at start (optional) -->
                  <td
                    v-if="actionsPosition === 'start' && $slots['row-actions']"
                    :class="[
                      'text-sm font-weight-normal',
                      hasStickyActions ? 'sticky-column sticky-column-left' : ''
                    ]"
                  >
                    <slot name="row-actions" :row="row" :index="rowIndex"></slot>
                  </td>
                  <td v-for="(column, colIndex) in columns" :key="colIndex" class="text-sm font-weight-normal">
                    <template v-if="$slots[column] || $slots[column.replace(/\s+/g,'_')]">
                      <slot :name="$slots[column] ? column : column.replace(/\s+/g,'_')" :row="row" :value="row[column]" :index="rowIndex"></slot>
                    </template>
                    <template v-else-if="column === 'select'">
                      <input
                         :id="`chk-${row.id}`"
                          type="checkbox"
                          :checked="row.id === selectedId"
                          class="me-2"
                          @change="onRowSelect(row, $event.target.checked)"
                      />
                    </template>
                    <template v-else-if="column === 'activo'">
                      <span :class="row[column] ? 'tag tag-green' : 'tag tag-red'" class="font-weight-bold">
                        {{ row[column] ? 'Activo' : 'Inactivo' }}
                      </span>
                    </template>
                    <template v-else-if="column === 'subrogado'">
                      <span :class="row[column] ? 'tag tag-green' : 'tag tag-red'" class="font-weight-bold">
                        {{ row[column] ? 'SI' : 'NO' }}
                      </span>
                    </template>
                    <template v-else>
                      <div :class="isWrapped(column) ? 'wrap-text' : ''">{{ formatCell(row, column) }}</div>
                    </template>
                  </td>
                  <!-- Actions Column at end (optional) -->
                  <td
                    v-if="actionsPosition === 'end' && $slots['row-actions']"
                    :class="[
                      'text-sm font-weight-normal',
                      hasStickyActions ? 'sticky-column sticky-column-right' : ''
                    ]"
                  >
                    <slot name="row-actions" :row="row" :index="rowIndex"></slot>
                  </td>
                </tr>
              </tbody>
            </table>
            <!-- Paginación estilo dots -->
            <div v-if="uiTotalPages > 1 && showFooter" class="pagination-dots-wrapper">
              <button
                class="circle-btn nav-btn"
                :disabled="currentPage === 1 || isLoading"
                title="Página anterior"
                @click="prevPage"
              ><span class="arrow left"></span></button>
              <div class="page-dots">
                <button
                  v-for="p in visiblePages"
                  :key="'pg-'+p"
                  class="dot-btn"
                  :class="{ active: p === currentPage }"
                  :disabled="isLoading"
                  :title="'Ir a página '+p"
                  @click="goToPage(p)"
                >{{ p }}</button>
              </div>
              <button
                class="circle-btn nav-btn"
                :disabled="currentPage === uiTotalPages || isLoading"
                title="Página siguiente"
                @click="nextPage"
              ><span class="arrow right"></span></button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
 import MaterialButton from "@/components/common/MaterialButton.vue";
 import { ref, computed, watch, onMounted, onBeforeUnmount } from "vue";

export default {
  name: "DataTable",
 
  components: {
    MaterialButton,
  },
  props: {
    title: {
      type: String,
      default: "DataTable",
    },
    description: {
      type: String,
      default: "",
    },
    permisoExportar: {
      type: String,
      default: "",
    },
    tableId: {
      type: String,
      required: true,
    },
    columns: {
      type: Array,
      required: true,
    },
    rows: {
      type: Array,
      required: true,
    },
    searchable: {
      type: Boolean,
      default: false,
    },
    exportar: {
      type: Boolean,
      default: false,
    },
    fixedHeight: {
      type: Boolean,
      default: true,
    },
    loadingProgress: {
      type: Number,
      default: 100,
    },     
    loadingMessage: {
      type: String,
      default: "Obteniendo información...",
    },
    loading: {
      type: Boolean,
      default: false,
    },
    loadingSpinnerSrc: {
      type: String,
      default: "",
    },
     pageSize: {
      type: Number,
      default: 10,
    },
    pageNumber: {
      type: Number,
      default: 1,
    },
    totalPages: {
      type: Number,
      default: null,
    },
    totalRecords: {
      type: Number,
      default: null,
    },
    useExternalPagination: {
      type: Boolean,
      default: false,
    },
     selectedId: {
    type: [String, Number, null],
    default: null,
  },
    actionsPosition: {
      type: String,
      default: 'end',
      validator: (v) => ['start','end'].includes(v)
    },
    stickyActions: {
      type: Boolean,
      default: false
    },
    filterOptions: {
      type: Array,
      default: () => [] // [{ value:'nombre', label:'Nombre' }]
    },
    // When this prop changes the table will clear its internal search/filter state
    searchResetKey: {
      type: [String, Number, null],
      default: null,
    },
    // Optional map of column keys to formatter functions: { FechaCreacion: (value,row) => '...' }
    columnFormatters: {
      type: Object,
      default: () => ({})
    },
    // Locale to use when formatting detected dates
    dateLocale: {
      type: String,
      default: 'es-MX'
    },
    // Regex (string or RegExp) used to detect which columns are date-like
    dateColumnPattern: {
      type: [String, RegExp],
      default: 'fecha|date|created|updated|timestamp|hora|vencimiento|nacimiento|ingreso|salida'
    },
    showFooter: {
      type: Boolean,
      default: true
    },
    pageWindowSize: {
      type: Number,
      default: 5
    }
    ,
    // columns that should allow wrapped text (array of column keys)
    wrapColumns: {
      type: Array,
      default: () => []
    }
  },
  emits: ["search", "page-change", "update:selected-id", "select-row"],
  setup(props, { emit }) {
  // compile date column regex once
  const dateColumnRegex = (typeof props.dateColumnPattern === 'string')
    ? new RegExp(props.dateColumnPattern, 'i')
    : (props.dateColumnPattern || /fecha|date|created|updated|timestamp|hora|vencimiento|nacimiento|ingreso|salida/i);
  const searchQuery = ref("");
  const searchInputRef = ref(null);
  const rootRef = ref(null);
  let intersectionObserver = null;
  // compile wrap columns set for fast lookup
  const wrapColumnsSet = computed(() => new Set((props.wrapColumns || []).map(c => String(c).toLowerCase())));
  function isWrapped(column) {
    return wrapColumnsSet.value.has(String(column).toLowerCase());
  }
  const selectedFilter = ref('todos');
  // watch for external reset trigger to clear internal search state
  watch(() => props.searchResetKey, (val, oldVal) => {
    if (val !== oldVal) {
      searchQuery.value = '';
      // reset to default filter option
      selectedFilter.value = props.useExternalPagination ? (props.filterOptions[0]?.value || 'nombre') : 'todos';
      // notify parent that search changed (external pagination expects an event)
      if (props.useExternalPagination) {
        emit('search', { filterField: selectedFilter.value, search: '', page: 1, pageSize: props.pageSize });
      }
    }
  });
  const showAdvancedSearch = computed(() => props.filterOptions.length > 0);
  const internalFilterOptions = computed(() => {
    // Cuando no hay paginación externa incluir 'Todos'
    if (!props.useExternalPagination) {
      return [{ value: 'todos', label: 'Todos' }, ...props.filterOptions];
    }
    return props.filterOptions; // sin 'Todos'
  });
  watch(() => props.filterOptions, (opts) => {
    if (opts.length > 0) {
      selectedFilter.value = props.useExternalPagination ? (opts[0]?.value || 'nombre') : 'todos';
    }
  }, { immediate: true });
  const hasStickyActions = computed(() => props.stickyActions);
    const currentPage = ref(props.pageNumber || 1);
    // Clear local search when changing pages for local (internal) pagination
    watch(currentPage, (val, old) => {
      if (val !== old && !props.useExternalPagination) {
        searchQuery.value = '';
      }
    });
    const filteredRows = computed(() => {
      const src = props.rows || [];
      if (props.useExternalPagination) {
        return src;
      }
      if (!searchQuery.value) {
        return src;
      }
      const term = searchQuery.value.toLowerCase();
      // si filtro = todos buscar en cualquier columna
      if (selectedFilter.value === 'todos' || !selectedFilter.value) {
        return src.filter((row) =>
          Object.values(row || {}).some((value) => String(value ?? '').toLowerCase().includes(term))
        );
      }
      // buscar sólo en columna específica si existe
      return src.filter(row => String((row && row[selectedFilter.value]) ?? '').toLowerCase().includes(term));
    });

    const uiTotalPages = computed(() => {
      if (props.useExternalPagination) {
        if (props.totalPages && props.totalPages > 0) {
          return props.totalPages;
        }
        const total = props.totalRecords ?? filteredRows.value.length;
        return Math.max(1, Math.ceil(total / props.pageSize));
      }
      return Math.max(1, Math.ceil(filteredRows.value.length / props.pageSize));
    });

    const paginatedRows = computed(() => {
      if (props.useExternalPagination) {
        return filteredRows.value;
      }
      const start = (currentPage.value - 1) * props.pageSize;
      return filteredRows.value.slice(start, start + props.pageSize);
    });

    // Visible pages (windowed) for dot pagination
    const visiblePages = computed(() => {
      const total = uiTotalPages.value;
      const size = Math.max(3, props.pageWindowSize);
      const current = currentPage.value;
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

    function prevPage() {
      if (isLoading.value) return;
      if (currentPage.value > 1) {
        const target = currentPage.value - 1;
        currentPage.value = target;
        emit("page-change", target);
      }
    }
    function nextPage() {
      if (isLoading.value) return;
      if (currentPage.value < uiTotalPages.value) {
        const target = currentPage.value + 1;
        currentPage.value = target;
        emit("page-change", target);
      }
    }

    // Emisión automática sólo para búsqueda local
    watch(
      () => searchQuery.value,
      () => {
        if (!props.useExternalPagination) {
          currentPage.value = 1;
          emit("search", searchQuery.value || "");
        }
      }
    );

    // Enfocar el campo de búsqueda al montar el componente
    function focusSearch() {
      try {
        if (props.searchable && searchInputRef.value && typeof searchInputRef.value.focus === 'function') {
          searchInputRef.value.focus()
          // place cursor at end
          const val = searchInputRef.value.value || '';
          searchInputRef.value.setSelectionRange && searchInputRef.value.setSelectionRange(val.length, val.length)
        }
      } catch (e) { /* noop */ }
    }

    onMounted(() => {
      // initial focus on mount
      focusSearch()

      // observe visibility changes so that focus occurs when the table becomes visible (e.g., v-show or modal)
      try {
        if (rootRef.value && typeof IntersectionObserver !== 'undefined') {
          intersectionObserver = new IntersectionObserver((entries) => {
            entries.forEach(ent => {
              if (ent.isIntersecting) {
                focusSearch()
                // disconnect after first focus to avoid repeated focusing
                try { intersectionObserver.disconnect() } catch (e) { /* noop */ }
              }
            })
          }, { threshold: 0.1 })
          intersectionObserver.observe(rootRef.value)
        }
      } catch (e) { /* noop */ }
    });

    onBeforeUnmount(() => {
      try { if (intersectionObserver) intersectionObserver.disconnect() } catch (e) { /* noop */ }
    });

    // When rows change externally, reset pagination locally but DO NOT emit search/page-change
    watch(
      () => props.rows,
      () => {
        if (!props.useExternalPagination) {
          currentPage.value = 1;
        }
      }
    );

    watch(
      () => props.pageNumber,
      (val) => {
        if (props.useExternalPagination && typeof val === "number" && val > 0) {
          currentPage.value = val;
        }
      },
      { immediate: true }
    );

    // CSV download logic
    const downloadCSV = () => {
      const rows = filteredRows.value;
      if (!rows.length) return;

      // Use columns prop for headers
      const headers = props.columns;
      const csvRows = [
        headers.join(","),
        ...rows.map(row =>
          headers.map(col => {
            const val = row[col] !== undefined ? row[col] : "";

            return `"${String(val).replace(/"/g, '""')}"`;
          }).join(",")
        )
      ];
      const csvContent = csvRows.join("\r\n");
      const blob = new Blob([csvContent], { type: "text/csv;charset=utf-8;" });
      const link = document.createElement("a");
      link.href = URL.createObjectURL(blob);
      link.setAttribute("download", props.title+".csv");
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    };

      // nuevo: emitir evento cuando se selecciona/deselecciona checkbox
    function onRowSelect(row, checked) {
      // marcar el row localmente para reflejar el estado en la UI
        if (checked) {
        // emite solo el seleccionado
        emit("select-row", { row, checked: true });
        emit("update:selected-id", row.id);
        } else {
        // si desmarcan, quita selección
        emit("select-row", { row, checked: false });
        emit("update:selected-id", null);
        }
    }

    function triggerSearch() {
      if (props.useExternalPagination) {
        // Emitir objeto con parámetros esperados por backend
        emit('search', {
          filterField: selectedFilter.value,
          search: searchQuery.value.trim(),
          page: currentPage.value,
          pageSize: props.pageSize
        });
      } else {
        // Local: ya se emite por watch, pero forzamos reset de página
        currentPage.value = 1;
      }
    }

    function goToPage(p) {
      if (isLoading.value) return;
      const pageNum = Math.max(1, Math.min(p, uiTotalPages.value));
      if (pageNum === currentPage.value) return;
      currentPage.value = pageNum;
      emit('page-change', pageNum);
    }

    const isLoading = computed(() => props.loading || props.loadingProgress < 100);

    // Cell formatting: use provided formatter, otherwise auto-format ISO/epoch dates
    // Only auto-format when the column name matches `dateColumnRegex` to avoid
    // treating numeric or currency columns as dates.
    function formatCell(row, column) {
      try {
        const formatters = props.columnFormatters || {};
        const val = row ? row[column] : undefined;
        if (formatters && typeof formatters[column] === 'function') {
          return formatters[column](val, row) ?? '';
        }
        if (val === null || val === undefined) return '';

        // If this column doesn't look like a date column, don't attempt date parsing.
        if (!dateColumnRegex.test(String(column))) {
          return String(val);
        }

        // Numbers that look like epoch millis or seconds
        if (typeof val === 'number') {
          // assume millis if > 1e12, seconds if ~1e10
          const asMillis = val > 1e12 ? val : (val > 1e9 ? val * 1000 : val);
          const dnum = new Date(asMillis);
          if (!isNaN(dnum)) {
            return dnum.toLocaleDateString(props.dateLocale, { day: '2-digit', month: '2-digit', year: 'numeric' });
          }
          return String(val);
        }
        if (typeof val === 'string') {
          const s = val.trim();
          // ISO date-ish: 2023-10-31 or 2023-10-31T12:34:56
          const isoLike = /^\d{4}-\d{2}-\d{2}(T.*)?$/;
          const digitsOnly = /^\d{10,13}$/;
          if (isoLike.test(s)) {
            const d = new Date(s);
            if (!isNaN(d)) {
              // if time present, include time
              if (s.includes('T')) {
                return d.toLocaleString(props.dateLocale, { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' });
              }
              return d.toLocaleDateString(props.dateLocale, { day: '2-digit', month: '2-digit', year: 'numeric' });
            }
          } else if (digitsOnly.test(s)) {
            const n = Number(s);
            const asMillis = s.length === 13 ? n : (s.length === 10 ? n * 1000 : n);
            const d = new Date(asMillis);
            if (!isNaN(d)) return d.toLocaleDateString(props.dateLocale, { day: '2-digit', month: '2-digit', year: 'numeric' });
          }
        }
        return String(val);
      } catch (e) {
        return row ? String(row[column]) : '';
      }
    }

    return {
      filteredRows,
      searchQuery,
      downloadCSV,
      currentPage,
      uiTotalPages,
      prevPage,
      nextPage,
      paginatedRows,
      onRowSelect,
      hasStickyActions,
      showAdvancedSearch,
      triggerSearch,
      internalFilterOptions,
      selectedFilter,
      isLoading,
      visiblePages,
      goToPage
      ,formatCell
      ,isWrapped
    };
  },
};
</script>

<style scoped>
.table-flush td {
  padding-left: 20px;
}

.tag {
  font-size: 12px;
  font-weight: bold;
  text-align: center;
  display: inline-block;
  width: 70px;
}

.tag-green {
  background-color: #28a745;
  color: white;

}

.tag-red {
  background-color: #7b7b7b;
  color: white;

}

.search-container .form-control {
  border-width: 4px !important;
  border-color: #C9A432  !important; /* opcional: color primario */
  border-radius: 6px;
}

.search-container .search-input-fit {
  width: 33ch !important;
  max-width: 100%;
}

.search-container select.form-select {
  border-width: 4px !important;
  border-color: #C9A432  !important;
  border-radius: 6px;
}

/* Ajustes barra de búsqueda avanzada para evitar que el botón se comprima y el texto se parta */
.search-container .d-flex {
  flex-wrap: nowrap;
}
.search-container .d-flex > select,
.search-container .d-flex > input {
  height: 38px;
}
.search-container .d-flex > .material-button,
.search-container .d-flex > button {
  height: 38px;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 0 18px;
  white-space: nowrap;
  flex-shrink: 0;
}

.pagination-container {
  display: flex;
  justify-content: center;
  align-items: center;
  margin: 1rem 0 0 0;
}

/* New dot pagination styles */
.pagination-dots-wrapper {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  justify-content: center;
  margin: 1rem 0 0 0;
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
.circle-btn:disabled { opacity: .4; cursor: not-allowed; }
.circle-btn:not(:disabled):hover { background: var(--bs-primary, #0d6efd); }
.circle-btn:not(:disabled):hover .arrow::before { border-color: #fff; }
.arrow { position: relative; width: 14px; height: 14px; display: inline-block; }
.arrow::before {
  content: '';
  position: absolute;
  top: 50%; left: 50%;
  width: 10px; height: 10px;
  border-top: 2px solid var(--bs-primary, #0d6efd);
  border-right: 2px solid var(--bs-primary, #0d6efd);
  transform: translate(-50%, -50%) rotate(45deg);
  transition: border-color .15s;
}
.arrow.left::before { transform: translate(-50%, -50%) rotate(225deg); }
.arrow.right::before { transform: translate(-50%, -50%) rotate(45deg); }
.page-dots { display: flex; gap: .5rem; flex-wrap: wrap; }
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

.table-disabled {
  opacity: 0.6;
  pointer-events: none;
}

.wrap-text {
  white-space: normal;
  word-break: break-word;
  overflow-wrap: anywhere;
}

.loading-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  background: rgba(255, 255, 255, 0.7);
  z-index: 10;
}

/* Spinner ring removed in favor of dot animation */
.spinner-dots {
  display: inline-block;
  width: 72px;
  height: 22px;
  text-align: center;
}
.spinner-dots span {
  display: inline-block;
  width: 14px;
  height: 14px;
  margin: 0 6px;
  /* Use system primary color variable, fall back to bootstrap var then hardcoded blue */
  background: var(--primary-color, var(--bs-primary, #0d6efd));
  border-radius: 50%;
  opacity: 0.9;
  animation: bounce 0.9s infinite ease-in-out;
}
.spinner-dots span:nth-child(1) { animation-delay: 0s; }
.spinner-dots span:nth-child(2) { animation-delay: 0.15s; }
.spinner-dots span:nth-child(3) { animation-delay: 0.3s; }
@keyframes bounce {
  0%, 80%, 100% { transform: translateY(0); opacity: 0.7 }
  40% { transform: translateY(-10px); opacity: 1 }
}

/* Fade transition for overlay */
.fade-enter-active, .fade-leave-active { transition: opacity 200ms ease; }
.fade-enter-from, .fade-leave-to { opacity: 0; }
.fade-enter-to, .fade-leave-from { opacity: 1; }
@keyframes spin {
  to { transform: rotate(360deg); }
}
.loading-gif {
  width: 72px;
  height: 72px;
  object-fit: contain;
  filter: drop-shadow(0 2px 4px rgba(0,0,0,0.12));
}

.loading-text {
  color: var(--primary-color, var(--bs-primary, #0d6efd));
  font-weight: 600;
}

.sticky-column {
  position: sticky;
  background: #fff;
  z-index: 5;
}

.sticky-column-right {
  right: 0;
  box-shadow: -4px 0 6px -4px rgba(0, 0, 0, 0.2);
}

.sticky-column-left {
  left: 0;
  box-shadow: 4px 0 6px -4px rgba(0, 0, 0, 0.2);
}

@media (max-width: 768px) {
  .search-container .search-input-fit {
    width: 100% !important;
  }
}

</style>