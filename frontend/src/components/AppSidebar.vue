<template>
  <aside class="sidebar" :class="{ open: isOpen }">
    <nav class="sidebar-nav">
      <RouterLink to="/" class="nav-item" @click="close">
        <span class="nav-icon">📊</span>
        <span class="nav-label">Dashboard</span>
      </RouterLink>
      <RouterLink to="/service-orders" class="nav-item" @click="close">
        <span class="nav-icon">📋</span>
        <span class="nav-label">Órdenes de Servicio</span>
      </RouterLink>
      <RouterLink to="/service-orders/create" class="nav-item" @click="close">
        <span class="nav-icon">➕</span>
        <span class="nav-label">Nueva Orden</span>
      </RouterLink>
    </nav>
    <div class="sidebar-footer">
      <span class="version">v1.0.0</span>
    </div>
  </aside>
  <div v-if="isOpen" class="sidebar-backdrop" @click="close"></div>
</template>

<script setup lang="ts">
defineProps<{ isOpen: boolean }>()
const emit = defineEmits<{ close: [] }>()
function close() { emit('close') }
</script>

<style scoped>
.sidebar {
  width: 240px;
  background: #1e293b;
  color: #cbd5e1;
  display: flex;
  flex-direction: column;
  min-height: 100%;
  transition: transform 0.25s ease;
  flex-shrink: 0;
}
.sidebar-nav {
  padding: 1.5rem 0;
  flex: 1;
}
.nav-item {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 0.75rem 1.5rem;
  text-decoration: none;
  color: #94a3b8;
  font-size: 0.92rem;
  font-weight: 500;
  transition: background 0.15s, color 0.15s;
  border-left: 3px solid transparent;
}
.nav-item:hover { background: rgba(255,255,255,0.05); color: #e2e8f0; }
.nav-item.router-link-active {
  background: rgba(59,130,246,0.15);
  color: #93c5fd;
  border-left-color: #3b82f6;
}
.nav-icon { font-size: 1.1rem; width: 1.5rem; text-align: center; }
.sidebar-footer {
  padding: 1rem 1.5rem;
  font-size: 0.75rem;
  color: #475569;
  border-top: 1px solid #334155;
}
.sidebar-backdrop {
  display: none;
  position: fixed;
  inset: 0;
  background: rgba(0,0,0,0.4);
  z-index: 98;
}

@media (max-width: 768px) {
  .sidebar {
    position: fixed;
    top: 60px;
    left: 0;
    bottom: 0;
    z-index: 99;
    transform: translateX(-100%);
  }
  .sidebar.open { transform: translateX(0); }
  .sidebar-backdrop { display: block; }
}
</style>
