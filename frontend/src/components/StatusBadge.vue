<template>
  <span class="status-badge" :class="`status-${status.toLowerCase()}`">
    {{ STATUS_LABELS[status] }}
  </span>
</template>

<script setup lang="ts">
import { type ServiceOrderStatus, STATUS_LABELS } from '../types/serviceOrder'
defineProps<{ status: ServiceOrderStatus }>()
</script>

<style scoped>
.status-badge {
  display: inline-block;
  padding: 3px 10px;
  border-radius: 12px;
  font-size: 0.78rem;
  font-weight: 600;
  letter-spacing: 0.02em;
  text-transform: uppercase;
  white-space: nowrap;
}
.status-pending   { background: #e9e9e9; color: #555; }
.status-inprogress{ background: #dbeafe; color: #1d4ed8; }
.status-onhold    { background: #fef3c7; color: #b45309; }
.status-completed { background: #d1fae5; color: #065f46; }
.status-cancelled { background: #fee2e2; color: #b91c1c; }
</style>
