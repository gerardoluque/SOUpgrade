export type ServiceOrderStatus = 'Pending' | 'InProgress' | 'OnHold' | 'Completed' | 'Cancelled'
export type ServiceOrderPriority = 'Low' | 'Medium' | 'High' | 'Critical'

export interface ServiceOrder {
  id: string
  orderNumber: string
  title: string
  description: string
  status: ServiceOrderStatus
  priority: ServiceOrderPriority
  createdAt: string
  updatedAt: string
  clientName: string
  clientEmail: string
  clientPhone: string
  assignedTo: string
  estimatedCompletionDate?: string
  completedAt?: string
  notes: string
  cost: number
}

export interface ServiceOrderFormData {
  title: string
  description: string
  status: ServiceOrderStatus
  priority: ServiceOrderPriority
  clientName: string
  clientEmail: string
  clientPhone: string
  assignedTo: string
  estimatedCompletionDate?: string
  notes: string
  cost: number
}

export interface StatusCount {
  Pending: number
  InProgress: number
  OnHold: number
  Completed: number
  Cancelled: number
}

export const STATUS_LABELS: Record<ServiceOrderStatus, string> = {
  Pending: 'Pendiente',
  InProgress: 'En Progreso',
  OnHold: 'En Espera',
  Completed: 'Completado',
  Cancelled: 'Cancelado',
}

export const PRIORITY_LABELS: Record<ServiceOrderPriority, string> = {
  Low: 'Baja',
  Medium: 'Media',
  High: 'Alta',
  Critical: 'Crítica',
}

export const STATUS_OPTIONS: { value: ServiceOrderStatus; label: string }[] = [
  { value: 'Pending', label: 'Pendiente' },
  { value: 'InProgress', label: 'En Progreso' },
  { value: 'OnHold', label: 'En Espera' },
  { value: 'Completed', label: 'Completado' },
  { value: 'Cancelled', label: 'Cancelado' },
]

export const PRIORITY_OPTIONS: { value: ServiceOrderPriority; label: string }[] = [
  { value: 'Low', label: 'Baja' },
  { value: 'Medium', label: 'Media' },
  { value: 'High', label: 'Alta' },
  { value: 'Critical', label: 'Crítica' },
]
