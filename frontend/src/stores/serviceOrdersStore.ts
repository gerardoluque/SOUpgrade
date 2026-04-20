import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { serviceOrdersApi } from '../api/serviceOrdersApi'
import type { ServiceOrder, ServiceOrderFormData, ServiceOrderStatus } from '../types/serviceOrder'

export const useServiceOrdersStore = defineStore('serviceOrders', () => {
  const orders = ref<ServiceOrder[]>([])
  const currentOrder = ref<ServiceOrder | null>(null)
  const loading = ref(false)
  const error = ref<string | null>(null)

  const statusCounts = computed(() => ({
    Pending: orders.value.filter((o) => o.status === 'Pending').length,
    InProgress: orders.value.filter((o) => o.status === 'InProgress').length,
    OnHold: orders.value.filter((o) => o.status === 'OnHold').length,
    Completed: orders.value.filter((o) => o.status === 'Completed').length,
    Cancelled: orders.value.filter((o) => o.status === 'Cancelled').length,
  }))

  const recentOrders = computed(() =>
    [...orders.value]
      .sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime())
      .slice(0, 5)
  )

  async function fetchAll() {
    loading.value = true
    error.value = null
    try {
      orders.value = await serviceOrdersApi.getAll()
    } catch (e: any) {
      error.value = e?.response?.data?.message || e.message || 'Error al cargar las órdenes'
    } finally {
      loading.value = false
    }
  }

  async function fetchById(id: string) {
    loading.value = true
    error.value = null
    currentOrder.value = null
    try {
      currentOrder.value = await serviceOrdersApi.getById(id)
    } catch (e: any) {
      error.value = e?.response?.data?.message || e.message || 'Error al cargar la orden'
    } finally {
      loading.value = false
    }
  }

  async function createOrder(data: ServiceOrderFormData): Promise<ServiceOrder | null> {
    loading.value = true
    error.value = null
    try {
      const order = await serviceOrdersApi.create(data)
      orders.value.unshift(order)
      return order
    } catch (e: any) {
      error.value = e?.response?.data?.message || e.message || 'Error al crear la orden'
      return null
    } finally {
      loading.value = false
    }
  }

  async function updateOrder(id: string, data: ServiceOrderFormData): Promise<ServiceOrder | null> {
    loading.value = true
    error.value = null
    try {
      const updated = await serviceOrdersApi.update(id, data)
      const idx = orders.value.findIndex((o) => o.id === id)
      if (idx !== -1) orders.value[idx] = updated
      if (currentOrder.value?.id === id) currentOrder.value = updated
      return updated
    } catch (e: any) {
      error.value = e?.response?.data?.message || e.message || 'Error al actualizar la orden'
      return null
    } finally {
      loading.value = false
    }
  }

  async function deleteOrder(id: string): Promise<boolean> {
    loading.value = true
    error.value = null
    try {
      await serviceOrdersApi.delete(id)
      orders.value = orders.value.filter((o) => o.id !== id)
      if (currentOrder.value?.id === id) currentOrder.value = null
      return true
    } catch (e: any) {
      error.value = e?.response?.data?.message || e.message || 'Error al eliminar la orden'
      return false
    } finally {
      loading.value = false
    }
  }

  async function changeStatus(id: string, status: ServiceOrderStatus): Promise<boolean> {
    loading.value = true
    error.value = null
    try {
      const updated = await serviceOrdersApi.updateStatus(id, status)
      const idx = orders.value.findIndex((o) => o.id === id)
      if (idx !== -1) orders.value[idx] = updated
      if (currentOrder.value?.id === id) currentOrder.value = updated
      return true
    } catch (e: any) {
      error.value = e?.response?.data?.message || e.message || 'Error al cambiar el estado'
      return false
    } finally {
      loading.value = false
    }
  }

  function clearError() {
    error.value = null
  }

  return {
    orders,
    currentOrder,
    loading,
    error,
    statusCounts,
    recentOrders,
    fetchAll,
    fetchById,
    createOrder,
    updateOrder,
    deleteOrder,
    changeStatus,
    clearError,
  }
})
