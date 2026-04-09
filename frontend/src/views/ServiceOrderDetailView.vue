<template>
  <div class="detail-page">
    <div v-if="store.loading" class="loading-state">
      <div class="spinner"></div>
      <p>Cargando orden...</p>
    </div>

    <div v-else-if="store.error" class="alert alert-error">⚠️ {{ store.error }}</div>

    <template v-else-if="store.currentOrder">
      <div class="page-header">
        <div class="breadcrumb">
          <RouterLink to="/service-orders">Órdenes de Servicio</RouterLink>
          <span> / </span>
          <span>#{{ store.currentOrder.orderNumber }}</span>
        </div>
        <div class="header-actions">
          <RouterLink :to="`/service-orders/${store.currentOrder.id}/edit`" class="btn btn-secondary">
            ✏️ Editar
          </RouterLink>
          <button class="btn btn-danger" @click="showDelete = true">🗑 Eliminar</button>
        </div>
      </div>

      <div class="detail-card">
        <div class="detail-header">
          <div class="detail-title-row">
            <h1>{{ store.currentOrder.title }}</h1>
            <div class="badges">
              <StatusBadge :status="store.currentOrder.status" />
              <PriorityBadge :priority="store.currentOrder.priority" />
            </div>
          </div>
          <div class="order-meta">
            <span class="order-num">#{{ store.currentOrder.orderNumber }}</span>
            <span class="meta-sep">·</span>
            <span>Creado {{ formatDate(store.currentOrder.createdAt) }}</span>
            <span class="meta-sep">·</span>
            <span>Actualizado {{ formatDate(store.currentOrder.updatedAt) }}</span>
          </div>
        </div>

        <!-- Change Status -->
        <div class="status-change-bar">
          <label class="change-status-label">Cambiar estado:</label>
          <div class="status-buttons">
            <button
              v-for="opt in STATUS_OPTIONS"
              :key="opt.value"
              class="status-btn"
              :class="{ active: store.currentOrder.status === opt.value }"
              @click="handleStatusChange(opt.value)"
              :disabled="store.currentOrder.status === opt.value || store.loading"
            >
              {{ opt.label }}
            </button>
          </div>
        </div>

        <div class="detail-body">
          <div class="section">
            <h3 class="section-title">Descripción</h3>
            <p class="description-text">{{ store.currentOrder.description || '—' }}</p>
          </div>

          <div class="detail-grid">
            <div class="section">
              <h3 class="section-title">Datos del Cliente</h3>
              <dl class="detail-list">
                <dt>Nombre</dt><dd>{{ store.currentOrder.clientName }}</dd>
                <dt>Correo</dt><dd><a :href="`mailto:${store.currentOrder.clientEmail}`">{{ store.currentOrder.clientEmail }}</a></dd>
                <dt>Teléfono</dt><dd>{{ store.currentOrder.clientPhone || '—' }}</dd>
              </dl>
            </div>

            <div class="section">
              <h3 class="section-title">Asignación</h3>
              <dl class="detail-list">
                <dt>Técnico</dt><dd>{{ store.currentOrder.assignedTo }}</dd>
                <dt>Costo</dt><dd>${{ formatCost(store.currentOrder.cost) }} MXN</dd>
                <dt>Fecha Estimada</dt>
                <dd>{{ store.currentOrder.estimatedCompletionDate ? formatDate(store.currentOrder.estimatedCompletionDate) : '—' }}</dd>
                <dt>Fecha de Cierre</dt>
                <dd>{{ store.currentOrder.completedAt ? formatDate(store.currentOrder.completedAt) : '—' }}</dd>
              </dl>
            </div>
          </div>

          <div v-if="store.currentOrder.notes" class="section">
            <h3 class="section-title">Notas</h3>
            <p class="notes-text">{{ store.currentOrder.notes }}</p>
          </div>
        </div>
      </div>
    </template>

    <div v-else class="empty-state">
      <p>Orden no encontrada.</p>
      <RouterLink to="/service-orders" class="btn btn-primary">Volver a la lista</RouterLink>
    </div>

    <ConfirmDialog
      v-model="showDelete"
      title="Eliminar Orden"
      :message="`¿Estás seguro de que deseas eliminar esta orden? Esta acción no se puede deshacer.`"
      confirmText="Eliminar"
      @confirm="executeDelete"
    />
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useServiceOrdersStore } from '../stores/serviceOrdersStore'
import StatusBadge from '../components/StatusBadge.vue'
import PriorityBadge from '../components/PriorityBadge.vue'
import ConfirmDialog from '../components/ConfirmDialog.vue'
import { STATUS_OPTIONS, type ServiceOrderStatus } from '../types/serviceOrder'

const store = useServiceOrdersStore()
const route = useRoute()
const router = useRouter()
const showDelete = ref(false)

onMounted(() => store.fetchById(route.params.id as string))

async function handleStatusChange(status: ServiceOrderStatus) {
  if (!store.currentOrder) return
  await store.changeStatus(store.currentOrder.id, status)
}

async function executeDelete() {
  if (!store.currentOrder) return
  const ok = await store.deleteOrder(store.currentOrder.id)
  if (ok) router.push('/service-orders')
}

function formatDate(d: string) {
  return new Date(d).toLocaleDateString('es-ES', { day: '2-digit', month: 'long', year: 'numeric' })
}
function formatCost(n: number) {
  return n.toLocaleString('es-MX', { minimumFractionDigits: 2, maximumFractionDigits: 2 })
}
</script>

<style scoped>
.detail-page { padding: 1.5rem; max-width: 900px; }
.page-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.25rem;
}
.breadcrumb { font-size: 0.88rem; color: #64748b; }
.breadcrumb a { color: #3b82f6; text-decoration: none; }
.breadcrumb a:hover { text-decoration: underline; }
.header-actions { display: flex; gap: 0.75rem; }

.detail-card {
  background: #fff;
  border-radius: 12px;
  border: 1px solid #e2e8f0;
  overflow: hidden;
}
.detail-header { padding: 1.5rem; border-bottom: 1px solid #f1f5f9; }
.detail-title-row {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 1rem;
  flex-wrap: wrap;
}
.detail-title-row h1 { margin: 0; font-size: 1.4rem; color: #1e293b; flex: 1; }
.badges { display: flex; gap: 0.5rem; flex-wrap: wrap; }
.order-meta { margin-top: 0.5rem; font-size: 0.82rem; color: #64748b; display: flex; flex-wrap: wrap; gap: 0.25rem; }
.order-num { font-weight: 700; }
.meta-sep { color: #cbd5e1; }

.status-change-bar {
  padding: 1rem 1.5rem;
  background: #f8fafc;
  border-bottom: 1px solid #f1f5f9;
  display: flex;
  align-items: center;
  gap: 1rem;
  flex-wrap: wrap;
}
.change-status-label { font-size: 0.85rem; font-weight: 600; color: #374151; white-space: nowrap; }
.status-buttons { display: flex; flex-wrap: wrap; gap: 0.5rem; }
.status-btn {
  padding: 5px 14px;
  border: 1.5px solid #d1d5db;
  border-radius: 20px;
  background: #fff;
  font-size: 0.8rem;
  cursor: pointer;
  transition: background 0.15s, border-color 0.15s;
}
.status-btn:hover:not(:disabled) { background: #dbeafe; border-color: #93c5fd; }
.status-btn.active { background: #1d4ed8; color: #fff; border-color: #1d4ed8; }
.status-btn:disabled { opacity: 0.6; cursor: default; }

.detail-body { padding: 1.5rem; display: flex; flex-direction: column; gap: 1.5rem; }
.detail-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 1.5rem; }
@media (max-width: 640px) { .detail-grid { grid-template-columns: 1fr; } }

.section-title {
  font-size: 0.78rem;
  font-weight: 700;
  color: #1e40af;
  text-transform: uppercase;
  letter-spacing: 0.06em;
  margin: 0 0 0.75rem;
  padding-bottom: 0.5rem;
  border-bottom: 2px solid #dbeafe;
}
.description-text, .notes-text {
  color: #374151;
  line-height: 1.6;
  margin: 0;
  white-space: pre-line;
}
.detail-list {
  display: grid;
  grid-template-columns: auto 1fr;
  gap: 0.4rem 1rem;
  margin: 0;
  font-size: 0.88rem;
}
.detail-list dt { font-weight: 600; color: #64748b; white-space: nowrap; }
.detail-list dd { margin: 0; color: #1e293b; word-break: break-word; }
.detail-list a { color: #3b82f6; text-decoration: none; }
.detail-list a:hover { text-decoration: underline; }

.empty-state { text-align: center; padding: 3rem; display: flex; flex-direction: column; align-items: center; gap: 1rem; color: #94a3b8; }
</style>
