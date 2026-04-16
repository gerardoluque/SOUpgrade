<template>
  <div class="alertas-wrapper">
    <div class="d-flex justify-content-between align-items-center mb-3">
      <div>
        <h5 class="mb-0">Alertas y Notificaciones</h5>
        <small class="text-muted">Listado de alertas generadas por el sistema</small>
      </div>
      <div class="d-flex gap-2 align-items-center">
        <label class="small mb-0">Motivo</label>
        <select v-model="filters.motivo" class="form-select form-select-sm">
          <option value="">Todos</option>
          <option v-for="(label, key) in motivoOptions" :key="key" :value="key">{{ label }}</option>
        </select>
        <label class="small mb-0">Asunto</label>
        <select v-model="filters.asunto" class="form-select form-select-sm">
          <option value="">Todos</option>
          <option v-for="(label, key) in asuntoOptions" :key="key" :value="key">{{ label }}</option>
        </select>
        <label class="small mb-0">Leído</label>
        <select v-model="filters.leido" class="form-select form-select-sm">
          <option value="">Todos</option>
          <option :value="true">Leído</option>
          <option :value="false">No leído</option>
        </select>
        <button class="btn btn-sm btn-primary" @click="reload">Filtrar</button>
      </div>
    </div>

    <DataTable
      :columns="columns"
      :rows="rows"
      :loading="loading"
      :page-size="pageSize"
      :page-number="currentPage"
      :total-pages="totalPages"
      :total-records="totalRecords"
      :use-external-pagination="true"
      @page-change="onPageChange"
    />

    <div v-if="!loading && !rows.length" class="alert alert-info mt-3">No se encontraron alertas.</div>
  </div>
</template>

<script>
import { ref, computed, onMounted } from 'vue'
import DataTable from '@/components/widgets/DataTable.vue'
import { getAlertasPaginadas } from '@/services/alertsService'
import { AlertaMotivo, AlertaAsunto } from '@/enums/enums.js'

export default {
  name: 'AlertasYNotificacion',
  components: { DataTable },
  setup() {
    const loading = ref(false)
    const items = ref([])
    const currentPage = ref(1)
    const pageSize = ref(10)
    const totalPages = ref(1)
    const totalRecords = ref(0)

    const filters = ref({ motivo: '', asunto: '', fechaInicio: '', fechaFin: '', leido: '' })

    const motivoOptions = computed(() => {
      return Object.entries(AlertaMotivo).reduce((acc, [k, v]) => {
        acc[v] = k
        return acc
      }, {})
    })

    const asuntoOptions = computed(() => {
      return Object.entries(AlertaAsunto).reduce((acc, [k, v]) => {
        acc[v] = k
        return acc
      }, {})
    })

    const columns = ['Fecha', 'Asunto', 'Motivo', 'Mensaje', 'Leído', 'Usuario']

    const rows = computed(() => {
      return (items.value || []).map(it => ({
        Fecha: it.fechacreacion || it.fechaCreacion || it.fecha || '',
        Asunto: it.asunto ?? it.Asunto ?? it.asuntoLabel ?? '',
        Motivo: it.motivo ?? it.Motivo ?? it.motivoLabel ?? '',
        Mensaje: it.mensaje ?? it.Mensaje ?? it.descripcion ?? '',
        'Leído': (it.leido === true || it.leido === 'true') ? 'Sí' : 'No',
        Usuario: it.usuario ?? it.creadoPor ?? it.usuarioNombre ?? ''
      }))
    })

    async function load(page = 1) {
      loading.value = true
      try {
        const payload = {
          motivo: filters.value.motivo || undefined,
          asunto: filters.value.asunto || undefined,
          fechaInicio: filters.value.fechaInicio || undefined,
          fechaFin: filters.value.fechaFin || undefined,
          leido: filters.value.leido === '' ? undefined : filters.value.leido,
          pageNumber: page,
          pageSize: pageSize.value
        }
        const res = await getAlertasPaginadas(payload)
        if (res.success) {
          const data = res.data || {}
          items.value = data.items || []
          totalPages.value = Number(data.totalPages ?? data.pagination?.totalPages ?? 1) || 1
          totalRecords.value = Number(data.totalRecords ?? data.total ?? data.pagination?.totalRecords ?? items.value.length) || items.value.length
          currentPage.value = Number(data.pageNumber ?? data.page ?? page) || page
        } else {
          items.value = []
          totalPages.value = 1
          totalRecords.value = 0
        }
      } catch (e) {
        console.warn('load alertas error', e)
        items.value = []
        totalPages.value = 1
        totalRecords.value = 0
      } finally {
        loading.value = false
      }
    }

    function reload() { load(1) }
    function onPageChange(page) { load(page) }

    onMounted(() => load(1))

    return {
      loading, rows, columns, currentPage, pageSize, totalPages, totalRecords,
      filters, motivoOptions, asuntoOptions, reload, onPageChange
    }
  }
}
</script>

<style scoped>
.alertas-wrapper { background: #fff; border: 1px solid #e2e8f0; border-radius: 6px; padding: 1rem; }
.form-select-sm { min-width: 140px; }
</style>
