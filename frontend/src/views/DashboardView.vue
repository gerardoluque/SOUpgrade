<template>
  <div class="dashboard">
    <div class="page-header">
      <h1>Dashboard</h1>
      <p class="subtitle">Resumen del sistema de órdenes de servicio</p>
    </div>

    <div v-if="store.loading" class="loading-state">
      <div class="spinner"></div>
      <p>Cargando datos...</p>
    </div>

    <template v-else>
      <div v-if="store.error" class="alert alert-error">
        ⚠️ {{ store.error }} — Mostrando datos de ejemplo.
      </div>

      <!-- Status cards -->
      <div class="stats-grid">
        <div class="stat-card stat-pending" @click="goToList('Pending')">
          <div class="stat-icon">🕐</div>
          <div class="stat-info">
            <span class="stat-count">{{ store.statusCounts.Pending }}</span>
            <span class="stat-label">Pendientes</span>
          </div>
        </div>
        <div class="stat-card stat-inprogress" @click="goToList('InProgress')">
          <div class="stat-icon">⚙️</div>
          <div class="stat-info">
            <span class="stat-count">{{ store.statusCounts.InProgress }}</span>
            <span class="stat-label">En Progreso</span>
          </div>
        </div>
        <div class="stat-card stat-onhold" @click="goToList('OnHold')">
          <div class="stat-icon">⏸</div>
          <div class="stat-info">
            <span class="stat-count">{{ store.statusCounts.OnHold }}</span>
            <span class="stat-label">En Espera</span>
          </div>
        </div>
        <div class="stat-card stat-completed" @click="goToList('Completed')">
          <div class="stat-icon">✅</div>
          <div class="stat-info">
            <span class="stat-count">{{ store.statusCounts.Completed }}</span>
            <span class="stat-label">Completadas</span>
          </div>
        </div>
        <div class="stat-card stat-cancelled" @click="goToList('Cancelled')">
          <div class="stat-icon">❌</div>
          <div class="stat-info">
            <span class="stat-count">{{ store.statusCounts.Cancelled }}</span>
            <span class="stat-label">Canceladas</span>
          </div>
        </div>
      </div>

      <div class="dashboard-bottom">
        <!-- Recent orders -->
        <div class="panel">
          <div class="panel-header">
            <h2>Órdenes Recientes</h2>
            <RouterLink to="/service-orders" class="link-all">Ver todas →</RouterLink>
          </div>
          <div v-if="store.recentOrders.length === 0" class="empty-state">
            <p>No hay órdenes de servicio todavía.</p>
            <RouterLink to="/service-orders/create" class="btn btn-primary">Crear Primera Orden</RouterLink>
          </div>
          <table v-else class="recent-table">
            <thead>
              <tr>
                <th>Número</th>
                <th>Título</th>
                <th>Cliente</th>
                <th>Estado</th>
                <th>Prioridad</th>
                <th>Fecha</th>
              </tr>
            </thead>
            <tbody>
              <tr
                v-for="order in store.recentOrders"
                :key="order.id"
                class="table-row"
                @click="router.push(`/service-orders/${order.id}`)"
              >
                <td class="order-num">#{{ order.orderNumber }}</td>
                <td>{{ order.title }}</td>
                <td>{{ order.clientName }}</td>
                <td><StatusBadge :status="order.status" /></td>
                <td><PriorityBadge :priority="order.priority" /></td>
                <td class="date-cell">{{ formatDate(order.createdAt) }}</td>
              </tr>
            </tbody>
          </table>
        </div>

        <!-- Quick actions -->
        <div class="panel quick-actions-panel">
          <div class="panel-header"><h2>Acciones Rápidas</h2></div>
          <div class="quick-actions">
            <RouterLink to="/service-orders/create" class="quick-btn">
              <span class="quick-icon">➕</span>
              <span>Nueva Orden</span>
            </RouterLink>
            <RouterLink to="/service-orders" class="quick-btn">
              <span class="quick-icon">📋</span>
              <span>Ver Todas</span>
            </RouterLink>
            <RouterLink :to="{ path: '/service-orders', query: { status: 'Pending' } }" class="quick-btn">
              <span class="quick-icon">🕐</span>
              <span>Pendientes</span>
            </RouterLink>
            <RouterLink :to="{ path: '/service-orders', query: { status: 'InProgress' } }" class="quick-btn">
              <span class="quick-icon">⚙️</span>
              <span>En Progreso</span>
            </RouterLink>
          </div>

          <div class="total-section">
            <div class="total-row">
              <span>Total de Órdenes</span>
              <span class="total-value">{{ store.orders.length }}</span>
            </div>
            <div class="total-row">
              <span>Activas</span>
              <span class="total-value active">{{ store.statusCounts.Pending + store.statusCounts.InProgress }}</span>
            </div>
          </div>
        </div>
      </div>
    </template>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useServiceOrdersStore } from '../stores/serviceOrdersStore'
import StatusBadge from '../components/StatusBadge.vue'
import PriorityBadge from '../components/PriorityBadge.vue'
import type { ServiceOrderStatus } from '../types/serviceOrder'

const store = useServiceOrdersStore()
const router = useRouter()

onMounted(() => store.fetchAll())

function goToList(status: ServiceOrderStatus) {
  router.push({ path: '/service-orders', query: { status } })
}
function formatDate(d: string) {
  return new Date(d).toLocaleDateString('es-ES', { day: '2-digit', month: 'short', year: 'numeric' })
}
</script>

<style scoped>
.dashboard { padding: 1.5rem; max-width: 1200px; }
.page-header { margin-bottom: 1.5rem; }
.page-header h1 { margin: 0; font-size: 1.6rem; color: #1e293b; }
.subtitle { margin: 0.25rem 0 0; color: #64748b; font-size: 0.92rem; }

.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(160px, 1fr));
  gap: 1rem;
  margin-bottom: 1.5rem;
}
.stat-card {
  background: #fff;
  border-radius: 10px;
  padding: 1.25rem;
  display: flex;
  align-items: center;
  gap: 1rem;
  cursor: pointer;
  border: 1.5px solid transparent;
  box-shadow: 0 1px 4px rgba(0,0,0,0.06);
  transition: box-shadow 0.15s, border-color 0.15s;
}
.stat-card:hover { box-shadow: 0 4px 16px rgba(0,0,0,0.1); }
.stat-icon { font-size: 2rem; }
.stat-count { display: block; font-size: 1.8rem; font-weight: 700; line-height: 1; }
.stat-label { font-size: 0.78rem; color: #64748b; text-transform: uppercase; letter-spacing: 0.04em; }
.stat-pending   { border-color: #d1d5db; } .stat-pending .stat-count   { color: #6b7280; }
.stat-inprogress{ border-color: #bfdbfe; } .stat-inprogress .stat-count{ color: #1d4ed8; }
.stat-onhold    { border-color: #fed7aa; } .stat-onhold .stat-count    { color: #b45309; }
.stat-completed { border-color: #a7f3d0; } .stat-completed .stat-count { color: #065f46; }
.stat-cancelled { border-color: #fca5a5; } .stat-cancelled .stat-count { color: #b91c1c; }

.dashboard-bottom {
  display: grid;
  grid-template-columns: 1fr 280px;
  gap: 1.25rem;
}
@media (max-width: 900px) { .dashboard-bottom { grid-template-columns: 1fr; } }

.panel {
  background: #fff;
  border-radius: 10px;
  border: 1px solid #e2e8f0;
  overflow: hidden;
}
.panel-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem 1.25rem;
  border-bottom: 1px solid #f1f5f9;
}
.panel-header h2 { margin: 0; font-size: 1rem; color: #1e293b; }
.link-all { font-size: 0.85rem; color: #3b82f6; text-decoration: none; }
.link-all:hover { text-decoration: underline; }

.recent-table { width: 100%; border-collapse: collapse; font-size: 0.88rem; }
.recent-table th {
  padding: 0.6rem 1rem;
  text-align: left;
  font-size: 0.75rem;
  font-weight: 700;
  color: #64748b;
  text-transform: uppercase;
  letter-spacing: 0.05em;
  background: #f8fafc;
}
.recent-table td { padding: 0.75rem 1rem; border-top: 1px solid #f1f5f9; }
.table-row { cursor: pointer; transition: background 0.1s; }
.table-row:hover { background: #f8fafc; }
.order-num { font-weight: 600; color: #64748b; font-size: 0.82rem; }
.date-cell { color: #94a3b8; font-size: 0.82rem; }

.empty-state {
  text-align: center;
  padding: 2.5rem 1rem;
  color: #94a3b8;
}

.quick-actions-panel { padding: 0; }
.quick-actions {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 0.75rem;
  padding: 1rem 1.25rem;
}
.quick-btn {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.4rem;
  padding: 1rem 0.5rem;
  background: #f1f5f9;
  border-radius: 8px;
  text-decoration: none;
  color: #374151;
  font-size: 0.82rem;
  font-weight: 600;
  text-align: center;
  transition: background 0.15s;
}
.quick-btn:hover { background: #dbeafe; color: #1d4ed8; }
.quick-icon { font-size: 1.4rem; }
.total-section {
  border-top: 1px solid #f1f5f9;
  padding: 0.75rem 1.25rem;
}
.total-row {
  display: flex;
  justify-content: space-between;
  padding: 0.4rem 0;
  font-size: 0.88rem;
  color: #374151;
}
.total-value { font-weight: 700; color: #1e293b; }
.total-value.active { color: #1d4ed8; }
</style>
