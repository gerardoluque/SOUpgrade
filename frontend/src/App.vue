<template>
  <div class="app-layout">
    <AppHeader @toggle-sidebar="sidebarOpen = !sidebarOpen" />
    <div class="app-body">
      <AppSidebar :is-open="sidebarOpen" @close="sidebarOpen = false" />
      <main class="app-main">
        <RouterView />
      </main>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import AppHeader from './components/AppHeader.vue'
import AppSidebar from './components/AppSidebar.vue'

const sidebarOpen = ref(false)
</script>

<style>
*, *::before, *::after { box-sizing: border-box; margin: 0; padding: 0; }
html, body { height: 100%; }
body {
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, sans-serif;
  background: #f1f5f9;
  color: #1e293b;
  line-height: 1.5;
}
#app { height: 100%; }
a { color: inherit; }

.app-layout { display: flex; flex-direction: column; min-height: 100vh; }
.app-body { display: flex; flex: 1; min-height: 0; }
.app-main { flex: 1; overflow-y: auto; min-width: 0; }

/* Shared utilities */
.btn {
  display: inline-flex;
  align-items: center;
  gap: 0.4rem;
  padding: 8px 18px;
  border-radius: 7px;
  font-size: 0.88rem;
  font-weight: 600;
  cursor: pointer;
  border: none;
  text-decoration: none;
  transition: background 0.15s, opacity 0.15s;
}
.btn:disabled { opacity: 0.6; cursor: not-allowed; }
.btn-primary   { background: #2563eb; color: #fff; }
.btn-primary:hover:not(:disabled)   { background: #1d4ed8; }
.btn-secondary { background: #f1f5f9; color: #374151; border: 1.5px solid #d1d5db; }
.btn-secondary:hover:not(:disabled) { background: #e2e8f0; }
.btn-danger    { background: #ef4444; color: #fff; }
.btn-danger:hover:not(:disabled)    { background: #dc2626; }

.loading-state { text-align: center; padding: 3rem; color: #64748b; display: flex; flex-direction: column; align-items: center; gap: 1rem; }
.spinner {
  width: 36px; height: 36px;
  border: 3px solid #e2e8f0;
  border-top-color: #3b82f6;
  border-radius: 50%;
  animation: spin 0.7s linear infinite;
}
@keyframes spin { to { transform: rotate(360deg); } }

.alert { padding: 0.85rem 1.25rem; border-radius: 8px; margin-bottom: 1rem; font-size: 0.88rem; }
.alert-error { background: #fef2f2; color: #b91c1c; border: 1px solid #fca5a5; }
.alert-success { background: #f0fdf4; color: #166534; border: 1px solid #86efac; }
</style>
