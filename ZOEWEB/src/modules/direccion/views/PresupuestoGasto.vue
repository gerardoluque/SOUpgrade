<template>
  <div class="container-fluid py-4">
    <div class="card mt-4">
      <div class="card-body d-flex justify-content-between align-items-center">
        <div>
          <h4 class="mb-0">Presupuestos</h4>
          <small class="text-muted">Gestión de presupuestos por farmacia y ejercicio</small>
        </div>
        <div>
          <MaterialButton color="primary" variant="gradient" @click="openNew">Agregar Nuevo Presupuesto</MaterialButton>
        </div>
      </div>

      <div class="card-body pt-0">
        <div class="mb-3 d-flex gap-2 align-items-center">
          <label class="small mb-0">Farmacia</label>
          <MaterialComboBox v-model="filters.farmaciaId" :options="farmacias" :numeric-value="false" />
          <label class="small mb-0">Ejercicio</label>
          <MaterialInput v-model.number="filters.ejercicio" type="number" />
          <MaterialButton color="secondary" @click="load">Buscar</MaterialButton>
        </div>

        <DataTable
          :columns="columns"
          :rows="rows"
          :loading="loading"
          table-id="presupuesto-table"
          :searchable="true"
        >
          <template #row-actions="{ row }">
            <MaterialButton color="secondary" variant="gradient" class="me-2" @click="onEdit(row._raw)">Editar</MaterialButton>
            <MaterialButton color="danger" variant="outlined" @click="onDelete(row._raw)">Eliminar</MaterialButton>
          </template>
        </DataTable>

        <div v-if="!loading && !items.length" class="alert alert-info mt-3">No se encontraron presupuestos.</div>
      </div>
    </div>

    <!-- Modal for create/edit -->
    <div v-if="showModal" class="modal" :class="{ show: showModal }" style="display: block;">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">{{ editingItem.Id ? 'Editar Presupuesto' : 'Nuevo Presupuesto' }}</h5>
            <button type="button" class="btn-close" @click="closeModal"></button>
          </div>
          <div class="modal-body">
            <div class="mb-2">
              <label class="form-label small">Farmacia</label>
              <MaterialComboBox v-model="form.farmaciaId" :options="farmacias" :numeric-value="false" />
            </div>
            <div class="row">
              <div class="col-md-6 mb-2">
                <label class="form-label small">Año</label>
                <MaterialComboBox v-model="form.year" :options="yearsOptions" :numeric-value="true" />
              </div>
              <div class="col-md-6 mb-2">
                <label class="form-label small">Mes</label>
                <MaterialComboBox v-model="form.month" :options="monthsOptions" :numeric-value="true" />
              </div>
            </div>
            <div class="mb-2">
              <label class="form-label small">Presupuesto</label>
              <MaterialInput v-model.number="form.presupuesto" type="number" />
            </div>
            <div class="mb-2">
              <label class="form-label small">Observaciones</label>
              <MaterialTextarea v-model="form.observaciones" />
            </div>
          </div>
          <div class="modal-footer">
            <MaterialButton color="secondary" @click="closeModal">Cancelar</MaterialButton>
            <MaterialButton color="primary" @click="save">Guardar</MaterialButton>
          </div>
        </div>
      </div>
    </div>

  </div>
</template>

<script>
import { ref, computed, onMounted, getCurrentInstance } from 'vue'
import DataTable from '@/components/widgets/DataTable.vue'
import MaterialInput from '@/components/common/MaterialInput.vue'
import MaterialComboBox from '@/components/common/MaterialComboBox.vue'
import MaterialButton from '@/components/common/MaterialButton.vue'
import MaterialTextarea from '@/components/common/MaterialTextarea.vue'
import * as direccionService from '@/modules/direccion/services/direccionService.js'
import { useDireccionStore } from '@/modules/direccion/store/useDireccionStore'
import { storeToRefs } from 'pinia'

export default {
  name: 'PresupuestoGasto',
  components: { DataTable, MaterialInput, MaterialComboBox, MaterialButton, MaterialTextarea },
  setup() {
    const vm = getCurrentInstance()
    const direccionStore = useDireccionStore()
    const { farmacias } = storeToRefs(direccionStore)

    const loading = ref(false)
    const items = ref([])
    const showModal = ref(false)
    const editingItem = ref({})
    const today = new Date()
    const defaultYear = today.getFullYear()
    const defaultMonth = today.getMonth() + 1
    const form = ref({ Id: 0, farmaciaId: '', year: defaultYear, month: defaultMonth, presupuesto: 0, observaciones: '' })

    const filters = ref({ farmaciaId: '', ejercicio: defaultYear })

    const columns = ['Farmacia', 'Ejercicio', 'Fecha de Creación', 'Presupuesto', 'Gasto', 'Porcentaje']
    const yearsOptions = Array.from({ length: 6 }, (_, i) => ({ value: defaultYear + i, name: String(defaultYear + i) }))
    const monthsOptions = [
      { value: 1, name: '01 - Enero' },
      { value: 2, name: '02 - Febrero' },
      { value: 3, name: '03 - Marzo' },
      { value: 4, name: '04 - Abril' },
      { value: 5, name: '05 - Mayo' },
      { value: 6, name: '06 - Junio' },
      { value: 7, name: '07 - Julio' },
      { value: 8, name: '08 - Agosto' },
      { value: 9, name: '09 - Septiembre' },
      { value: 10, name: '10 - Octubre' },
      { value: 11, name: '11 - Noviembre' },
      { value: 12, name: '12 - Diciembre' }
    ]

    const rows = computed(() => (items.value || []).map(it => ({
      Farmacia: it.farmaciaNombre ?? it.farmacia ?? it.FarmaciaNombre ?? String(it.farmaciaId || it.FarmaciaId || ''),
      Ejercicio: it.ejercicio ?? it.Ejercicio ?? '',
      'Fecha de Creación': vm?.proxy?.$fmt?.formatDateTime?.(
        it.fechaCreacion ?? it.fechacreacion ?? it.FechaCreacion ?? it.createdAt ?? it.CreatedAt ?? ''
      ) || '',
      Presupuesto: formatCurrency(it.presupuesto ?? it.Presupuesto ?? 0),
      Gasto: formatCurrency(it.gasto ?? it.Gasto ?? 0),
      Porcentaje: `${Math.round((Number(it.porcentaje) || Number(it.porcentajePresupuesto) || 0) * 100) / 100}%`,
      _raw: it
    })))

    function formatCurrency(n) { return (typeof n === 'number' ? n : Number(n || 0)).toLocaleString('es-MX', { style: 'currency', currency: 'MXN' }) }

    async function load() {
      if (!filters.value.farmaciaId || !filters.value.ejercicio) {
        items.value = []
        return
      }
      loading.value = true
      try {
        const res = await direccionService.getPresupuesto({ farmaciaId: filters.value.farmaciaId, ejercicio: filters.value.ejercicio })
        if (!res) items.value = []
        else items.value = Array.isArray(res) ? res : [res]
      } catch (err) {
        console.warn('load presupuesto error', err)
        items.value = []
      } finally { loading.value = false }
    }

    function openNew() {
      editingItem.value = {}
      form.value = { Id: 0, farmaciaId: filters.value.farmaciaId || '', year: filters.value.ejercicio || defaultYear, month: defaultMonth, presupuesto: 0, observaciones: '' }
      showModal.value = true
    }

    function closeModal() { showModal.value = false }

    function onEdit(raw) {
      editingItem.value = raw || {}
      const rawEj = raw?.ejercicio ?? raw?.Ejercicio ?? null
      let y = defaultYear, m = defaultMonth
      if (rawEj != null) {
        const s = String(rawEj)
        if (s.length >= 6) {
          const yy = Number(s.slice(0,4))
          const mm = Number(s.slice(-2))
          if (!Number.isNaN(yy)) y = yy
          if (!Number.isNaN(mm)) m = mm
        } else if (s.length === 4) {
          const yy = Number(s)
          if (!Number.isNaN(yy)) y = yy
          m = defaultMonth
        }
      }
      form.value = {
        Id: raw?.Id ?? raw?.id ?? 0,
        farmaciaId: (raw?.farmaciaId ?? raw?.FarmaciaId ?? raw?.farmacia ?? filters.value.farmaciaId) || '',
        year: y,
        month: m,
        presupuesto: raw?.presupuesto ?? raw?.Presupuesto ?? 0,
        observaciones: raw?.observaciones ?? raw?.Observaciones ?? ''
      }
      showModal.value = true
    }

    async function save() {
      const year = Number(form.value.year) || defaultYear
      const month = Number(form.value.month) || defaultMonth
      const mm = String(month).padStart(2, '0')
      const ejercicioCombined = Number(`${year}${mm}`)
      const payload = {
        Id: Number(form.value.Id) || 0,
        FarmaciaId: form.value.farmaciaId,
        Ejercicio: ejercicioCombined,
        Presupuesto: Number(form.value.presupuesto) || 0,
        Observaciones: form.value.observaciones ?? null
      }
      loading.value = true
      try {
        const res = await direccionService.createOrUpdatePresupuesto(payload)
        if (res) {
          await load()
          closeModal()
        }
      } catch (err) {
        console.warn('save presupuesto error', err)
      } finally { loading.value = false }
    }

    async function onDelete(raw) {
      if (!raw) return
      const id = raw.Id ?? raw.id ?? raw.PresupuestoId ?? raw.presupuestoId ?? null
      if (!id) return alert('No se encontró el Id del presupuesto a eliminar')
      if (!confirm('¿Eliminar presupuesto? Esta acción no se puede deshacer.')) return
      loading.value = true
      try {
        // Intentamos marcar como eliminado mediante el endpoint de CreateOrUpdate
        const res = await direccionService.createOrUpdatePresupuesto({ Id: Number(id) || 0, Eliminado: true })
        if (res) await load()
      } catch (err) {
        console.warn('delete presupuesto error', err)
      } finally { loading.value = false }
    }

    onMounted(async () => {
      // prefill farmacias from store
      if (!farmacias.value || !farmacias.value.length) {
        // Try load farmacias via store helper if available
        try { await direccionStore.loadAll() } catch (e) { void e }
      }
      if (farmacias.value && farmacias.value.length) {
        filters.value.farmaciaId = filters.value.farmaciaId || farmacias.value[0].value
      }
      filters.value.ejercicio = filters.value.ejercicio || new Date().getFullYear()
      await load()
    })

    return { loading, items, columns, rows, farmacias, filters, load, openNew, showModal, form, save, closeModal, onEdit, onDelete, yearsOptions, monthsOptions }
  }
}
</script>

<style scoped>
.modal { background: rgba(0,0,0,0.4); }
.modal.show { display: block; }
</style>
