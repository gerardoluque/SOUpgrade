<template>
  <form class="so-form" @submit.prevent="handleSubmit" novalidate>
    <div class="form-section">
      <h3 class="section-title">Información General</h3>
      <div class="form-grid">
        <div class="form-group" :class="{ error: errors.title }">
          <label>Título <span class="required">*</span></label>
          <input v-model="form.title" type="text" placeholder="Descripción breve del servicio" />
          <span v-if="errors.title" class="error-msg">{{ errors.title }}</span>
        </div>
        <div class="form-group" :class="{ error: errors.status }">
          <label>Estado <span class="required">*</span></label>
          <select v-model="form.status">
            <option v-for="opt in STATUS_OPTIONS" :key="opt.value" :value="opt.value">{{ opt.label }}</option>
          </select>
        </div>
        <div class="form-group" :class="{ error: errors.priority }">
          <label>Prioridad <span class="required">*</span></label>
          <select v-model="form.priority">
            <option v-for="opt in PRIORITY_OPTIONS" :key="opt.value" :value="opt.value">{{ opt.label }}</option>
          </select>
        </div>
        <div class="form-group form-group-full" :class="{ error: errors.description }">
          <label>Descripción <span class="required">*</span></label>
          <textarea v-model="form.description" rows="3" placeholder="Detalle del servicio requerido"></textarea>
          <span v-if="errors.description" class="error-msg">{{ errors.description }}</span>
        </div>
      </div>
    </div>

    <div class="form-section">
      <h3 class="section-title">Datos del Cliente</h3>
      <div class="form-grid">
        <div class="form-group" :class="{ error: errors.clientName }">
          <label>Nombre del Cliente <span class="required">*</span></label>
          <input v-model="form.clientName" type="text" placeholder="Nombre completo" />
          <span v-if="errors.clientName" class="error-msg">{{ errors.clientName }}</span>
        </div>
        <div class="form-group" :class="{ error: errors.clientEmail }">
          <label>Correo Electrónico <span class="required">*</span></label>
          <input v-model="form.clientEmail" type="email" placeholder="cliente@ejemplo.com" />
          <span v-if="errors.clientEmail" class="error-msg">{{ errors.clientEmail }}</span>
        </div>
        <div class="form-group" :class="{ error: errors.clientPhone }">
          <label>Teléfono</label>
          <input v-model="form.clientPhone" type="tel" placeholder="+52 000 000 0000" />
          <span v-if="errors.clientPhone" class="error-msg">{{ errors.clientPhone }}</span>
        </div>
      </div>
    </div>

    <div class="form-section">
      <h3 class="section-title">Asignación y Fechas</h3>
      <div class="form-grid">
        <div class="form-group" :class="{ error: errors.assignedTo }">
          <label>Técnico Asignado <span class="required">*</span></label>
          <input v-model="form.assignedTo" type="text" placeholder="Nombre del técnico" />
          <span v-if="errors.assignedTo" class="error-msg">{{ errors.assignedTo }}</span>
        </div>
        <div class="form-group">
          <label>Fecha Estimada de Entrega</label>
          <input v-model="form.estimatedCompletionDate" type="date" />
        </div>
        <div class="form-group" :class="{ error: errors.cost }">
          <label>Costo Estimado (MXN)</label>
          <input v-model.number="form.cost" type="number" min="0" step="0.01" placeholder="0.00" />
          <span v-if="errors.cost" class="error-msg">{{ errors.cost }}</span>
        </div>
      </div>
    </div>

    <div class="form-section">
      <h3 class="section-title">Notas Adicionales</h3>
      <div class="form-group form-group-full">
        <textarea v-model="form.notes" rows="3" placeholder="Observaciones, instrucciones especiales..."></textarea>
      </div>
    </div>

    <div class="form-actions">
      <button type="button" class="btn btn-secondary" @click="$emit('cancel')">Cancelar</button>
      <button type="submit" class="btn btn-primary" :disabled="submitting">
        <span v-if="submitting">Guardando...</span>
        <span v-else>{{ submitLabel }}</span>
      </button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { reactive, watch } from 'vue'
import {
  STATUS_OPTIONS,
  PRIORITY_OPTIONS,
  type ServiceOrderFormData,
  type ServiceOrder,
} from '../types/serviceOrder'

const props = defineProps<{
  initialData?: Partial<ServiceOrder>
  submitting?: boolean
  submitLabel?: string
}>()

const emit = defineEmits<{
  submit: [data: ServiceOrderFormData]
  cancel: []
}>()

const form = reactive<ServiceOrderFormData>({
  title: '',
  description: '',
  status: 'Pending',
  priority: 'Medium',
  clientName: '',
  clientEmail: '',
  clientPhone: '',
  assignedTo: '',
  estimatedCompletionDate: '',
  notes: '',
  cost: 0,
})

const errors = reactive<Partial<Record<keyof ServiceOrderFormData, string>>>({})

watch(
  () => props.initialData,
  (data) => {
    if (data) {
      Object.assign(form, {
        title: data.title ?? '',
        description: data.description ?? '',
        status: data.status ?? 'Pending',
        priority: data.priority ?? 'Medium',
        clientName: data.clientName ?? '',
        clientEmail: data.clientEmail ?? '',
        clientPhone: data.clientPhone ?? '',
        assignedTo: data.assignedTo ?? '',
        estimatedCompletionDate: data.estimatedCompletionDate ?? '',
        notes: data.notes ?? '',
        cost: data.cost ?? 0,
      })
    }
  },
  { immediate: true }
)

function validate(): boolean {
  Object.keys(errors).forEach((k) => delete (errors as any)[k])

  if (!form.title.trim()) errors.title = 'El título es requerido'
  if (!form.description.trim()) errors.description = 'La descripción es requerida'
  if (!form.clientName.trim()) errors.clientName = 'El nombre del cliente es requerido'
  if (!form.clientEmail.trim()) {
    errors.clientEmail = 'El correo es requerido'
  } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.clientEmail)) {
    errors.clientEmail = 'El correo no tiene un formato válido'
  }
  if (!form.assignedTo.trim()) errors.assignedTo = 'El técnico asignado es requerido'
  if (form.cost < 0) errors.cost = 'El costo no puede ser negativo'

  return Object.keys(errors).length === 0
}

function handleSubmit() {
  if (!validate()) return
  emit('submit', { ...form })
}
</script>

<style scoped>
.so-form { display: flex; flex-direction: column; gap: 1.5rem; }
.form-section {
  background: #fff;
  border: 1px solid #e2e8f0;
  border-radius: 10px;
  padding: 1.5rem;
}
.section-title {
  font-size: 0.9rem;
  font-weight: 700;
  color: #1e40af;
  text-transform: uppercase;
  letter-spacing: 0.05em;
  margin: 0 0 1.25rem;
  padding-bottom: 0.75rem;
  border-bottom: 2px solid #dbeafe;
}
.form-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(220px, 1fr));
  gap: 1rem;
}
.form-group { display: flex; flex-direction: column; gap: 5px; }
.form-group-full { grid-column: 1 / -1; }
.form-group label {
  font-size: 0.85rem;
  font-weight: 600;
  color: #374151;
}
.required { color: #ef4444; }
.form-group input,
.form-group select,
.form-group textarea {
  padding: 8px 12px;
  border: 1.5px solid #d1d5db;
  border-radius: 6px;
  font-size: 0.9rem;
  color: #1e293b;
  background: #f9fafb;
  transition: border-color 0.15s;
  font-family: inherit;
  width: 100%;
  box-sizing: border-box;
}
.form-group input:focus,
.form-group select:focus,
.form-group textarea:focus {
  outline: none;
  border-color: #3b82f6;
  background: #fff;
}
.form-group.error input,
.form-group.error select,
.form-group.error textarea {
  border-color: #ef4444;
}
.error-msg { font-size: 0.78rem; color: #ef4444; }
.form-group textarea { resize: vertical; }
.form-actions {
  display: flex;
  gap: 0.75rem;
  justify-content: flex-end;
  padding-top: 0.5rem;
}
</style>
