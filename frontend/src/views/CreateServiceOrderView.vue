<template>
  <div class="create-page">
    <div class="page-header">
      <div class="breadcrumb">
        <RouterLink to="/service-orders">Órdenes de Servicio</RouterLink>
        <span> / Nueva Orden</span>
      </div>
      <h1>Nueva Orden de Servicio</h1>
    </div>

    <div v-if="store.error" class="alert alert-error">⚠️ {{ store.error }}</div>

    <ServiceOrderForm
      submit-label="Crear Orden"
      :submitting="store.loading"
      @submit="handleSubmit"
      @cancel="router.push('/service-orders')"
    />
  </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router'
import { useServiceOrdersStore } from '../stores/serviceOrdersStore'
import ServiceOrderForm from '../components/ServiceOrderForm.vue'
import type { ServiceOrderFormData } from '../types/serviceOrder'

const store = useServiceOrdersStore()
const router = useRouter()

async function handleSubmit(data: ServiceOrderFormData) {
  store.clearError()
  const order = await store.createOrder(data)
  if (order) router.push(`/service-orders/${order.id}`)
}
</script>

<style scoped>
.create-page { padding: 1.5rem; max-width: 900px; }
.page-header { margin-bottom: 1.5rem; }
.breadcrumb { font-size: 0.85rem; color: #64748b; margin-bottom: 0.5rem; }
.breadcrumb a { color: #3b82f6; text-decoration: none; }
.breadcrumb a:hover { text-decoration: underline; }
.page-header h1 { margin: 0; font-size: 1.5rem; color: #1e293b; }
</style>
