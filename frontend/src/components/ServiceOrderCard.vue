<template>
  <div class="order-card" @click="$emit('view', order)">
    <div class="card-header">
      <span class="order-number">#{{ order.orderNumber }}</span>
      <StatusBadge :status="order.status" />
    </div>
    <h3 class="card-title">{{ order.title }}</h3>
    <div class="card-meta">
      <span>👤 {{ order.clientName }}</span>
      <PriorityBadge :priority="order.priority" />
    </div>
    <div class="card-footer">
      <span class="card-date">{{ formatDate(order.createdAt) }}</span>
      <div class="card-actions" @click.stop>
        <button class="btn-icon" title="Ver" @click="$emit('view', order)">👁</button>
        <button class="btn-icon" title="Editar" @click="$emit('edit', order)">✏️</button>
        <button class="btn-icon btn-icon-danger" title="Eliminar" @click="$emit('delete', order)">🗑</button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import StatusBadge from './StatusBadge.vue'
import PriorityBadge from './PriorityBadge.vue'
import type { ServiceOrder } from '../types/serviceOrder'

defineProps<{ order: ServiceOrder }>()
defineEmits<{
  view: [order: ServiceOrder]
  edit: [order: ServiceOrder]
  delete: [order: ServiceOrder]
}>()

function formatDate(d: string) {
  return new Date(d).toLocaleDateString('es-ES', { day: '2-digit', month: 'short', year: 'numeric' })
}
</script>

<style scoped>
.order-card {
  background: #fff;
  border-radius: 10px;
  border: 1px solid #e2e8f0;
  padding: 1rem 1.25rem;
  cursor: pointer;
  transition: box-shadow 0.15s, border-color 0.15s;
}
.order-card:hover {
  box-shadow: 0 4px 16px rgba(0,0,0,0.1);
  border-color: #93c5fd;
}
.card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.5rem;
}
.order-number { font-size: 0.82rem; color: #64748b; font-weight: 600; }
.card-title { margin: 0 0 0.75rem; font-size: 0.97rem; color: #1e293b; font-weight: 600; line-height: 1.3; }
.card-meta {
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: 0.85rem;
  color: #64748b;
  margin-bottom: 0.75rem;
}
.card-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-top: 1px solid #f1f5f9;
  padding-top: 0.6rem;
}
.card-date { font-size: 0.8rem; color: #94a3b8; }
.card-actions { display: flex; gap: 4px; }
.btn-icon {
  background: none;
  border: 1px solid #e2e8f0;
  border-radius: 6px;
  padding: 4px 8px;
  cursor: pointer;
  font-size: 0.9rem;
  transition: background 0.1s;
}
.btn-icon:hover { background: #f1f5f9; }
.btn-icon-danger:hover { background: #fee2e2; border-color: #fca5a5; }
</style>
