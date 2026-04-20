<template>
  <span class="priority-badge" :class="`priority-${priority.toLowerCase()}`">
    {{ PRIORITY_LABELS[priority] }}
  </span>
</template>

<script setup lang="ts">
import { type ServiceOrderPriority, PRIORITY_LABELS } from '../types/serviceOrder'
defineProps<{ priority: ServiceOrderPriority }>()
</script>

<style scoped>
.priority-badge {
  display: inline-block;
  padding: 3px 10px;
  border-radius: 12px;
  font-size: 0.78rem;
  font-weight: 600;
  letter-spacing: 0.02em;
  text-transform: uppercase;
  white-space: nowrap;
}
.priority-low      { background: #d1fae5; color: #065f46; }
.priority-medium   { background: #fef9c3; color: #854d0e; }
.priority-high     { background: #fed7aa; color: #c2410c; }
.priority-critical { background: #fee2e2; color: #b91c1c; }
</style>
