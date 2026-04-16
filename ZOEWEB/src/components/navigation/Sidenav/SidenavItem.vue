<template>
  <li class="nav-item">
    <router-link class="nav-link "  data-bs-toggle="tooltip" data-bs-placement="top" :title="text"  :to="to" @click="handleClick">
       <i class="material-icons-round opacity-10 me-2"> {{ miniIcon }}</i>
    
      <span class="sidenav-normal me-3 ms-3 ps-1 text-white">
        {{ text }}
      </span>
    </router-link>
  </li>
</template>

<script>
import { SistemasEnum } from '@mod1/services/sistemaDbService';
import localStorageService from '@/utils/localStorageService';

export default {
  name: "SidenavItem",
  props: {
    to: {
      type: [Object, String],
      required: true,
    },
    miniIcon: {
      type: String,
      required: true,
    },
    text: {
      type: String,
      required: true,
    },
    sistemaId: {
      type: Number,
      required: false,
      default: SistemasEnum.GENERAL,
    },
  },
  methods: {
    handleClick() {
      try {
        // Prefer explicit prop if provided
        let selectedSistemaId = this.sistemaId;

        // If not provided, attempt to resolve from target route name
        if (selectedSistemaId === undefined || selectedSistemaId === null || selectedSistemaId === '') {
          const resolved = this.$router && this.$router.resolve ? this.$router.resolve(this.to) : null;
          const targetName = resolved && resolved.name ? resolved.name : null;
          const userRol = localStorageService.get('userRol');

          if (targetName && userRol && Array.isArray(userRol.procesos)) {
            for (const p of userRol.procesos) {
              if (p && Array.isArray(p.subprocesos)) {
                const found = p.subprocesos.find((sub) => sub && sub.ruta === targetName);
                if (found) {
                  selectedSistemaId = found.sistemaId || found.sistemaID || found.sistemaid || (found.sistema && (found.sistema.id || found.sistemaId)) || null;
                  break;
                }
              }
              if (!selectedSistemaId && p && p.ruta === targetName) {
                selectedSistemaId = p.sistemaId || p.sistemaID || p.sistemaid || (p.sistema && (p.sistema.id || p.sistemaId)) || null;
              }
            }
          }
        }

        if (selectedSistemaId !== undefined && selectedSistemaId !== null && selectedSistemaId !== '') {
          localStorageService.set('sistemaSelectedId', selectedSistemaId);
        }
      } catch (e) {
        // Swallow errors to not block navigation
        console.error("Error handling SidenavItem click:", e);
      }
    }
  }
};
</script>

<style scoped>

</style>