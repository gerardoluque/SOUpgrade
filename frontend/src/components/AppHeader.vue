<template>
  <header class="app-header">
    <button class="hamburger" @click="$emit('toggle-sidebar')" aria-label="Toggle menu">
      <span></span><span></span><span></span>
    </button>
    <div class="header-brand">
      <span class="brand-icon">🔧</span>
      <span class="brand-name">Órdenes de Servicio</span>
    </div>
    <div class="header-actions">
      <RouterLink to="/service-orders/create" class="btn btn-primary btn-sm">
        + Nueva Orden
      </RouterLink>
    </div>
  </header>
</template>

<script setup lang="ts">
defineEmits<{ 'toggle-sidebar': [] }>()
</script>

<style scoped>
.app-header {
  height: 60px;
  background: #1e40af;
  color: #fff;
  display: flex;
  align-items: center;
  padding: 0 1.25rem;
  gap: 1rem;
  position: sticky;
  top: 0;
  z-index: 100;
  box-shadow: 0 2px 8px rgba(0,0,0,0.15);
}
.header-brand {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 1.05rem;
  font-weight: 700;
  flex: 1;
}
.brand-icon { font-size: 1.3rem; }
.hamburger {
  background: none;
  border: none;
  cursor: pointer;
  display: flex;
  flex-direction: column;
  gap: 5px;
  padding: 4px;
}
.hamburger span {
  display: block;
  width: 22px;
  height: 2px;
  background: #fff;
  border-radius: 2px;
}
.header-actions { display: flex; align-items: center; gap: 0.75rem; }
.btn-sm { padding: 6px 14px; font-size: 0.85rem; }
</style>
