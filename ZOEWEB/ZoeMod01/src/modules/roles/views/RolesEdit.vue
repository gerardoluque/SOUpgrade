<template>
    <div>
      <h1>Edit Rol</h1>
      <form @submit.prevent="updateRol">
        <input v-model="grupo.name" placeholder="Rol Name" />
        <button type="submit">Actualizar</button>
      </form>
    </div>
  </template>
  
  <script>
  import { useRolesStore } from "@mod1/modules/roles/store/useRolesStore";
  import { ref, onMounted } from "vue";
  import { useRouter, useRoute } from "vue-router";
  
  export default {
    name: "RolesEdit",
    setup() {
      const rolesStore = useRolesStore();
      const grupo = ref({ name: "" });
      const router = useRouter();
      const route = useRoute();
  
      onMounted(() => {
        const grupoId = route.params.id;
        const existingRol = rolesStore.grupos.find((g) => g.id === grupoId);
        if (existingRol) {
          grupo.value = { ...existingRol };
        }
      });
  
      const updateGrupo = async () => {
        await rolesStore.updateGrupo(grupo.value);
        router.push({ name: "GruposList" });
      };
  
      return { grupo, updateGrupo };
    },
  };
  </script>