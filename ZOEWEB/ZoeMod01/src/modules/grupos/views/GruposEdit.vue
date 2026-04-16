<template>
    <div>
      <h1>Edit Grupo</h1>
      <form @submit.prevent="updateGrupo">
        <input v-model="grupo.name" placeholder="Grupo Name" />
        <button type="submit">Update</button>
      </form>
    </div>
  </template>
  
  <script>
  import { useGruposStore } from "@mod1/modules/grupos/store/useGruposStore";
  import { ref, onMounted } from "vue";
  import { useRouter, useRoute } from "vue-router";
  
  export default {
    name: "GruposEdit",
    setup() {
      const gruposStore = useGruposStore();
      const grupo = ref({ name: "" });
      const router = useRouter();
      const route = useRoute();
  
      onMounted(() => {
        const grupoId = route.params.id;
        const existingGrupo = gruposStore.grupos.find((g) => g.id === grupoId);
        if (existingGrupo) {
          grupo.value = { ...existingGrupo };
        }
      });
  
      const updateGrupo = async () => {
        await gruposStore.updateGrupo(grupo.value);
        router.push({ name: "GruposList" });
      };
  
      return { grupo, updateGrupo };
    },
  };
  </script>