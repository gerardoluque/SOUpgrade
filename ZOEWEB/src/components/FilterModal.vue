<template>
  <div v-if="visible" class="filter-modal-backdrop" @click.self="close">
    <div class="filter-modal">
      <div class="filter-modal-header">
        <h5>Filtros</h5>
        <button class="close-btn" @click="close">&times;</button>
      </div>
      <div class="filter-modal-body">
        <slot />
      </div>
      <div class="filter-modal-footer">
        <button class="btn btn-primary" @click="apply">Aplicar</button>
        <button class="btn btn-secondary" @click="close">Cancelar</button>
      </div>
    </div>
  </div>
</template>

<script setup>
const { visible } = defineProps({
  visible: Boolean
});
const emits = defineEmits(['close', 'apply']);


function close() {
  emits('close');
}
function apply() {
  emits('apply');
}
</script>

<style scoped>
.filter-modal-backdrop {
  position: fixed;
  inset: 0;
  background: rgba(0,0,0,0.35);
  z-index: 2000;
  display: flex;
  align-items: flex-start;
  justify-content: flex-end;
}
.filter-modal {
  background: #fff;
  width: 340px;
  max-width: 100vw;
  min-height: 100vh;
  box-shadow: -2px 0 16px rgba(0,0,0,0.15);
  padding: 0;
  display: flex;
  flex-direction: column;
  animation: slideIn .3s;
}
@keyframes slideIn {
  from { transform: translateX(100%);}
  to { transform: translateX(0);}
}
.filter-modal-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 1rem 1.5rem;
  border-bottom: 1px solid #eee;
}
.filter-modal-body {
  flex: 1;
  padding: 1.5rem;
  overflow-y: auto;
}
.filter-modal-footer {
  padding: 1rem 1.5rem;
  border-top: 1px solid #eee;
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
}
.close-btn {
  background: none;
  border: none;
  font-size: 1.5rem;
  cursor: pointer;
}
</style>