<template>

<div>

 

    <!-- Mostrar el login si el usuario no está autenticado -->
    <router-view v-if="isAuthenticated" />

    <!-- Mostrar el layout principal si el usuario está autenticado -->
    <div v-else>

  <sidenav
    v-if="store.showSidenav"
    :custom_class="store.color"
    :class="[store.isRTL ? 'fixed-end' : 'fixed-start']"
  />
  <main
    class="main-content position-relative max-height-vh-100 h-100 overflow-x-hidden"
  >
  
    <!-- nav -->
    <MaterialAlert
style="
      position: fixed;
      top: 0;
      left: 0;
      width: 100vw;
      z-index: 9999;
      display: flex;
      align-items: center;
      justify-content: center;
      pointer-events: none;
      padding-top: 16px;
    "   />
    <navbar
v-if="store.showNavbar"
      :class="[store.isNavFixed ? store.navbarFixed : '', store.isAbsolute ? store.absolute : '']"
      :color="store.isAbsolute ? 'text-white opacity-8' : ''" :min-nav="store.navbarMinimize" />
    <router-view />
    <app-footer v-show="store.showFooter" />
    <configurator
:toggle="store.toggleConfigurator"
      :class="[store.showConfig ? 'show' : '', store.hideConfigButton ? 'd-none' : '']" />
  </main>
</div>
</div>
 
</template>

<script>
import Sidenav from "@/components/navigation/Sidenav/index.vue";
import Configurator from "@/examples/Configurator.vue";
import Navbar from "@/components/navigation/Sidenav/Navbar.vue";
import AppFooter from "@/examples/Footer.vue";
import MaterialAlert from "@/components/common/MaterialAlert.vue";
import { onMounted,onBeforeUnmount } from "vue";

import { useMainStore } from "@/store/useMainStore";


export default {
  name: "App",
  components: {
    Sidenav,
    Configurator,
    Navbar,
    AppFooter,
    MaterialAlert,
  },
  setup() {
    const store = useMainStore();

     const resetInactivityTimer = () => {
      store.resetInactivityTimer();
    };

    // Prevent common reload keys (F5, Ctrl+R / Cmd+R) and optionally show a warning
    const handleKeyDown = (e) => {
      try {
        const key = e.key || '';
        const code = e.keyCode || 0;
        const isF5 = key === 'F5' || code === 116;
        const isCtrlR = (e.ctrlKey || e.metaKey) && (key === 'r' || key === 'R' || code === 82);
        if (isF5 || isCtrlR) {
          // Prevent default reload
          e.preventDefault();
          e.stopPropagation();
          try {
            // Optionally show an in-app warning to the user
            if (store && typeof store.triggerAlert === 'function') {
              store.triggerAlert({ message: 'Recarga deshabilitada en la aplicación. Usa la navegación interna.', color: 'warning' });
            }
          } catch (err) {
            // ignore
          }
          return false;
        }
      } catch (err) {
        // ignore
      }
    };

    // beforeunload: prompt user if they try to close/refresh while authenticated
    const handleBeforeUnload = (e) => {
      try {
        if (!store || !store.isAuthenticated) return;
        // Modern browsers ignore custom messages, but returning a value triggers the confirmation
        e.preventDefault();
        e.returnValue = '';
        return '';
      } catch (err) {
        // ignore
      }
    };

      onMounted(() => {
      window.addEventListener("mousemove", resetInactivityTimer);
      window.addEventListener("keydown", resetInactivityTimer);
      window.addEventListener("click", resetInactivityTimer);
      // Add global handlers to block refresh keys and warn on unload when authenticated
      window.addEventListener('keydown', handleKeyDown, { capture: true });
      window.addEventListener('beforeunload', handleBeforeUnload);
      store.startInactivityTimer(); // Inicia el temporizador al cargar la app
    });

     onBeforeUnmount(() => {
      window.removeEventListener("mousemove", resetInactivityTimer);
      window.removeEventListener("keydown", resetInactivityTimer);
      window.removeEventListener("click", resetInactivityTimer);
      window.removeEventListener('keydown', handleKeyDown, { capture: true });
      window.removeEventListener('beforeunload', handleBeforeUnload);
      clearTimeout(store.inactivityTimer); // Limpia el temporizador
    });

    const initializeSidenav = () => {
      const sidenav = document.getElementsByClassName("g-sidenav-show")[0];
      if (window.innerWidth > 1200) {
        sidenav.classList.add("g-sidenav-pinned");
      }
    };

    return {
      store,
      initializeSidenav,
      

      
    };
  },
  mounted() {
    this.initializeSidenav();
  },
};
</script>

<style>
.dataTable-pagination-list .active a {
  background-image: linear-gradient(195deg,
  #3769b5 0%,
  #3551a6 100%) !important;
}
</style>