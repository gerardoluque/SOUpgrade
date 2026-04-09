<template>
  <Teleport to="body">
    <div v-if="modelValue" class="dialog-overlay" @click.self="$emit('update:modelValue', false)">
      <div class="dialog">
        <div class="dialog-header">
          <span class="dialog-icon">⚠️</span>
          <h3>{{ title }}</h3>
        </div>
        <p class="dialog-message">{{ message }}</p>
        <div class="dialog-actions">
          <button class="btn btn-secondary" @click="$emit('update:modelValue', false)">Cancelar</button>
          <button class="btn btn-danger" @click="confirm">{{ confirmText }}</button>
        </div>
      </div>
    </div>
  </Teleport>
</template>

<script setup lang="ts">
defineProps<{
  modelValue: boolean
  title?: string
  message?: string
  confirmText?: string
}>()

const emit = defineEmits<{
  'update:modelValue': [value: boolean]
  confirm: []
}>()

function confirm() {
  emit('confirm')
  emit('update:modelValue', false)
}
</script>

<style scoped>
.dialog-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.45);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 9999;
}
.dialog {
  background: #fff;
  border-radius: 10px;
  padding: 2rem;
  max-width: 420px;
  width: 90%;
  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.2);
}
.dialog-header {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  margin-bottom: 1rem;
}
.dialog-header h3 { margin: 0; font-size: 1.1rem; color: #1e293b; }
.dialog-icon { font-size: 1.4rem; }
.dialog-message { color: #475569; margin-bottom: 1.5rem; line-height: 1.5; }
.dialog-actions { display: flex; gap: 0.75rem; justify-content: flex-end; }
</style>
