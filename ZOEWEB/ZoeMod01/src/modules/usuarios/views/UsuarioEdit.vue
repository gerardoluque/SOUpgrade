<template>
    <div>
      <h1>Edit Usuario</h1>
      <form @submit.prevent="updateUsuario">
        <input v-model="grupo.name" placeholder="Grupo Name" />
        <button type="submit">Update</button>
      </form>
    </div>
  </template>
  
  <script>
  import { useUsuarioStore } from "@mod1/modules/usuarios/store/useUsuarioStore";
  import { ref, onMounted } from "vue";
  import {  useRoute } from "vue-router";
  
  export default {
    name: "UsuarioEdit",
    setup() {
      const usuarioStore = useUsuarioStore();
      const usuario = ref({ name: "" });
     
      const route = useRoute();
  
      onMounted(() => {
        const userid = route.params.id;
        const existingUsuario = usuarioStore.usuarios.find((g) => g.id === userid);
        if (existingUsuario) {
          usuario.value = { ...existingUsuario };
        }
      });
  
     
  
      return { usuario  };
    },
  };
  </script>