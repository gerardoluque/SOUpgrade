<template>
    <div>
      <h1>Editar Proceso</h1>
      <form @submit.prevent="updateProceso">
        <input v-model="proceso.descr" placeholder="Proceso Descripcion" />
        <button type="submit">Actualizar</button>
      </form>
    </div>
  </template>
  
  <script>
  import { useProcesoStore } from "@mod1/modules/procesos/store/useProcesoStore";
  import { ref, onMounted } from "vue";
  import { useRouter, useRoute } from "vue-router";
  
  export default {
    name: "ProcesoEdit",
    setup() {
      const procesoStore = useProcesoStore();
      const proceso = ref({ descr: "" });
      const router = useRouter();
      const route = useRoute();
  
      onMounted(() => {
        const procesoid = route.params.id;
        const existing = procesoStore.proceso.find((g) => g.id === procesoid);
        if (existing) {
          proceso.value = { ...existing };
        }
      });
  
      const updateProceso = async () => {
        await procesoStore.updateProcess(proceso.value);
        router.push({ name: "ProcesoList" });
      };
  
      return { proceso, updateProceso };
    },
  };
  </script>