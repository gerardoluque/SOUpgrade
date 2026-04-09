<template>
  <div class="so-list-page">
    <div class="page-header">
      <div>
        <h1>Órdenes de Servicio</h1>
        <p class="subtitle">{{ filteredOrders.length }} orden(es) encontrada(s)</p>
      </div>
      <RouterLink to="/service-orders/create" class="btn btn-primary">+ Nueva Orden</RouterLink>
    </div>

    <!-- Filters -->
    <div class="filters-bar">
      <div class="search-wrap">
        <span class="search-icon">🔍</span>
        <input
          v-model="search"
          type="text"
          class="search-input"
          placeholder="Buscar por número, título o cliente..."
        />
      </div>
      <select v-model="filterStatus" class="filter-select">
        <option value="">Todos los estados</option>
        <option v-for="opt in STATUS_OPTIONS" :key="opt.value" :value="opt.value">{{ opt.label }}</option>
      </select>
      <select v-model="filterPriority" class="filter-select">
        <option value="">Todas las prioridades</option>
        <option v-for="opt in PRIORITY_OPTIONS" :key="opt.value" :value="opt.value">{{ opt.label }}</option>
      </select>
      <select v-model="sortBy" class="filter-select">
        <option value="createdAt-desc">Más reciente</option>
        <option value="createdAt-asc">Más antiguo</option>
        <option value="priority-desc">Mayor prioridad</option>
        <option value="status-asc">Estado A-Z</option>
      </select>
      <button v-if="hasFilters" class="btn btn-secondary btn-sm" @click="clearFilters">Limpiar</button>
    </div>

    <div v-if="store.loading" class="loading-state">
      <div class="spinner"></div>
      <p>Cargando órdenes...</p>
    </div>

    <div v-else-if="store.error" class="alert alert-error">⚠️ {{ store.error }}</div>

    <template v-else>
      <div v-if="filteredOrders.length === 0" class="empty-state">
        <p>No se encontraron órdenes.</p>
        <RouterLink v-if="!hasFilters" to="/service-orders/create" class="btn btn-primary">
          Crear Primera Orden
        </RouterLink>
        <button v-else class="btn btn-secondary" @click="clearFilters">Limpiar filtros</button>
      </div>

      <!-- Table view -->
      <div v-else class="table-wrapper">
        <table class="orders-table">
          <thead>
            <tr>
              <th>Número</th>
              <th>Título</th>
              <th>Cliente</th>
              <th>Estado</th>
              <th>Prioridad</th>
              <th>Técnico</th>
              <th>Fecha</th>
              <th>Costo</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="order in filteredOrders"
              :key="order.id"
              class="order-row"
              @click="router.push(`/service-orders/${order.id}`)"
            >
              <td class="num-cell">#{{ order.orderNumber }}</td>
              <td class="title-cell">{{ order.title }}</td>
              <td>{{ order.clientName }}</td>
              <td><StatusBadge :status="order.status" /></td>
              <td><PriorityBadge :priority="order.priority" /></td>
              <td>{{ order.assignedTo }}</td>
              <td class="date-cell">{{ formatDate(order.createdAt) }}</td>
              <td class="cost-cell">${{ formatCost(order.cost) }}</td>
              <td class="actions-cell" @click.stop>
                <div class="row-actions">
                  <RouterLink :to="`/service-orders/${order.id}`" class="btn-icon" title="Ver">👁</RouterLink>
                  <RouterLink :to="`/service-orders/${order.id}/edit`" class="btn-icon" title="Editar">✏️</RouterLink>
                  <button class="btn-icon btn-icon-danger" title="Eliminar" @click="confirmDelete(order)">🗑</button>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </template>

    <ConfirmDialog
      v-model="showDeleteDialog"
      title="Eliminar Orden"
      :message="`¿Estás seguro de que deseas eliminar la orden #${deleteTarget?.orderNumber}? Esta acción no se puede deshacer.`"
      confirmText="Eliminar"
      @confirm="executeDelete"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useServiceOrdersStore } from '../stores/serviceOrdersStore'
import StatusBadge from '../components/StatusBadge.vue'
import PriorityBadge from '../components/PriorityBadge.vue'
import ConfirmDialog from '../components/ConfirmDialog.vue'
import {
  STATUS_OPTIONS,
  PRIORITY_OPTIONS,
  type ServiceOrder,
  type ServiceOrderStatus,
  type ServiceOrderPriority,
} from '../types/serviceOrder'

const store = useServiceOrdersStore()
const router = useRouter()
const route = useRoute()

const search = ref('')
const filterStatus = ref<ServiceOrderStatus | ''>('')
const filterPriority = ref<ServiceOrderPriority | ''>('')
const sortBy = ref('createdAt-desc')
const showDeleteDialog = ref(false)
const deleteTarget = ref<ServiceOrder | null>(null)

const PRIORITY_ORDER: Record<string, number> = { Critical: 4, High: 3, Medium: 2, Low: 1 }

const hasFilters = computed(() => search.value || filterStatus.value || filterPriority.value)

const filteredOrders = computed(() => {
  let list = [...store.orders]

  if (search.value) {
    const q = search.value.toLowerCase()
    list = list.filter(
      (o) =>
        o.orderNumber.toLowerCase().includes(q) ||
        o.title.toLowerCase().includes(q) ||
        o.clientName.toLowerCase().includes(q)
    )
  }
  if (filterStatus.value) list = list.filter((o) => o.status === filterStatus.value)
  if (filterPriority.value) list = list.filter((o) => o.priority === filterPriority.value)

  list.sort((a, b) => {
    switch (sortBy.value) {
      case 'createdAt-asc': return new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime()
      case 'createdAt-desc': return new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
      case 'priority-desc': return (PRIORITY_ORDER[b.priority] || 0) - (PRIORITY_ORDER[a.priority] || 0)
      case 'status-asc': return a.status.localeCompare(b.status)
      default: return 0
    }
  })
  return list
})

onMounted(() => {
  store.fetchAll()
  if (route.query.status) filterStatus.value = route.query.status as ServiceOrderStatus
})

watch(() => route.query.status, (val) => {
  if (val) filterStatus.value = val as ServiceOrderStatus
})

function clearFilters() {
  search.value = ''
  filterStatus.value = ''
  filterPriority.value = ''
}

function confirmDelete(order: ServiceOrder) {
  deleteTarget.value = order
  showDeleteDialog.value = true
}

async function executeDelete() {
  if (!deleteTarget.value) return
  await store.deleteOrder(deleteTarget.value.id)
  deleteTarget.value = null
}

function formatDate(d: string) {
  return new Date(d).toLocaleDateString('es-ES', { day: '2-digit', month: 'short', year: 'numeric' })
}
function formatCost(n: number) {
  return n.toLocaleString('es-MX', { minimumFractionDigits: 2, maximumFractionDigits: 2 })
}
</script>

<style scoped>
.so-list-page { padding: 1.5rem; max-width: 1300px; }
.page-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 1.25rem;
}
.page-header h1 { margin: 0; font-size: 1.6rem; color: #1e293b; }
.subtitle { margin: 0.25rem 0 0; color: #64748b; font-size: 0.88rem; }

.filters-bar {
  display: flex;
  flex-wrap: wrap;
  gap: 0.75rem;
  margin-bottom: 1.25rem;
  background: #fff;
  padding: 1rem;
  border-radius: 10px;
  border: 1px solid #e2e8f0;
}
.search-wrap {
  position: relative;
  flex: 1;
  min-width: 200px;
}
.search-icon {
  position: absolute;
  left: 10px;
  top: 50%;
  transform: translateY(-50%);
  font-size: 0.9rem;
}
.search-input {
  width: 100%;
  padding: 8px 12px 8px 34px;
  border: 1.5px solid #d1d5db;
  border-radius: 6px;
  font-size: 0.88rem;
  box-sizing: border-box;
}
.search-input:focus { outline: none; border-color: #3b82f6; }
.filter-select {
  padding: 8px 12px;
  border: 1.5px solid #d1d5db;
  border-radius: 6px;
  font-size: 0.88rem;
  background: #f9fafb;
  cursor: pointer;
}
.filter-select:focus { outline: none; border-color: #3b82f6; }

.table-wrapper { overflow-x: auto; background: #fff; border-radius: 10px; border: 1px solid #e2e8f0; }
.orders-table { width: 100%; border-collapse: collapse; font-size: 0.88rem; min-width: 900px; }
.orders-table th {
  padding: 0.7rem 1rem;
  text-align: left;
  font-size: 0.75rem;
  font-weight: 700;
  color: #64748b;
  text-transform: uppercase;
  letter-spacing: 0.05em;
  background: #f8fafc;
  white-space: nowrap;
}
.orders-table td { padding: 0.75rem 1rem; border-top: 1px solid #f1f5f9; vertical-align: middle; }
.order-row { cursor: pointer; transition: background 0.1s; }
.order-row:hover { background: #f8fafc; }
.num-cell { font-weight: 600; color: #64748b; font-size: 0.82rem; white-space: nowrap; }
.title-cell { max-width: 200px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
.date-cell { color: #94a3b8; font-size: 0.82rem; white-space: nowrap; }
.cost-cell { font-weight: 600; color: #1e293b; white-space: nowrap; }
.actions-cell { white-space: nowrap; }
.row-actions { display: flex; gap: 4px; }
.btn-icon {
  background: none;
  border: 1px solid #e2e8f0;
  border-radius: 6px;
  padding: 4px 8px;
  cursor: pointer;
  font-size: 0.9rem;
  text-decoration: none;
  display: inline-block;
  transition: background 0.1s;
}
.btn-icon:hover { background: #f1f5f9; }
.btn-icon-danger:hover { background: #fee2e2; border-color: #fca5a5; }

.empty-state { text-align: center; padding: 3rem; color: #94a3b8; display: flex; flex-direction: column; align-items: center; gap: 1rem; }
</style>
