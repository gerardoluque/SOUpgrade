import axios from 'axios'
import type { ServiceOrder, ServiceOrderFormData, ServiceOrderStatus } from '../types/serviceOrder'

const api = axios.create({
  baseURL: '/api',
  headers: { 'Content-Type': 'application/json' },
})

export const serviceOrdersApi = {
  getAll(): Promise<ServiceOrder[]> {
    return api.get<ServiceOrder[]>('/service-orders').then((r) => r.data)
  },

  getById(id: string): Promise<ServiceOrder> {
    return api.get<ServiceOrder>(`/service-orders/${id}`).then((r) => r.data)
  },

  create(data: ServiceOrderFormData): Promise<ServiceOrder> {
    return api.post<ServiceOrder>('/service-orders', data).then((r) => r.data)
  },

  update(id: string, data: ServiceOrderFormData): Promise<ServiceOrder> {
    return api.put<ServiceOrder>(`/service-orders/${id}`, data).then((r) => r.data)
  },

  delete(id: string): Promise<void> {
    return api.delete(`/service-orders/${id}`).then(() => undefined)
  },

  updateStatus(id: string, status: ServiceOrderStatus): Promise<ServiceOrder> {
    return api.patch<ServiceOrder>(`/service-orders/${id}/status`, { status }).then((r) => r.data)
  },
}
