<template>
  <div class="edit-page">
    <div class="page-header">
      <div class="breadcrumb">
        <RouterLink to="/service-orders">Órdenes de Servicio</RouterLink>
        <span v-if="store.currentOrder">
          <span> / </span>
          <RouterLink :to="`/service-orders/${store.currentOrder.id}`">#{{ store.currentOrder.orderNumber }}</RouterLink>
        </span>
        <span> / Editar</span>
      </div>
      <h1>Editar Orden de Servicio</h1>
    </div>

    <div v-if="store.loading && !store.currentOrder" class="loading-state">
      <div class="spinner"></div>
      <p>Cargando orden...</p>
    </div>

    <div v-else-if="store.error && !store.currentOrder" class="alert alert-error">⚠️ {{ store.error }}</div>

    <template v-else-if="store.currentOrder">
      <div v-if="store.error" class="alert alert-error">⚠️ {{ store.error }}</div>
      <ServiceOrderForm
        :initial-data="store.currentOrder"
        submit-label="Guardar Cambios"
        :submitting="store.loading"
        @submit="handleSubmit"
        @cancel="router.push(`/service-orders/${store.currentOrder!.id}`)"
      />
    </template>

    <div v-else class="empty-state">
      <p>Orden no encontrada.</p>
      <RouterLink to="/service-orders" class="btn btn-primary">Volver a la lista</RouterLink>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useServiceOrdersStore } from '../stores/serviceOrdersStore'
import ServiceOrderForm from '../components/ServiceOrderForm.vue'
import type { ServiceOrderFormData } from '../types/serviceOrder'

const store = useServiceOrdersStore()
const route = useRoute()
const router = useRouter()

onMounted(() => store.fetchById(route.params.id as string))

async function handleSubmit(data: ServiceOrderFormData) {
  if (!store.currentOrder) return
  store.clearError()
  const updated = await store.updateOrder(store.currentOrder.id, data)
  if (updated) router.push(`/service-orders/${updated.id}`)
}
</script>

<style scoped>
.edit-page { padding: 1.5rem; max-width: 900px; }
.page-header { margin-bottom: 1.5rem; }
.breadcrumb { font-size: 0.85rem; color: #64748b; margin-bottom: 0.5rem; }
.breadcrumb a { color: #3b82f6; text-decoration: none; }
.breadcrumb a:hover { text-decoration: underline; }
.page-header h1 { margin: 0; font-size: 1.5rem; color: #1e293b; }
.empty-state { text-align: center; padding: 3rem; display: flex; flex-direction: column; align-items: center; gap: 1rem; color: #94a3b8; }
</style>
