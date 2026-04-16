<template>
    <div>
      <nav>
        <ul>
          <li v-for="process in userProcesses" :key="process.id">{{ process.name }}</li>
        </ul>
      </nav>
      <router-view />
    </div>
  </template>
  
  <script>
  export default {
    data() {
      return {
        userProcesses: [], // Procesos del usuario
      };
    },
    async created() {
      // Simula la carga de procesos del usuario después de autenticarse
      this.userProcesses = await this.fetchUserProcesses();
    },
    methods: {
      async fetchUserProcesses() {
        // Aquí puedes realizar una llamada a tu API para obtener los procesos del usuario
        return [
          { id: 1, name: "Proceso 1" },
          { id: 2, name: "Proceso 2" },
          { id: 3, name: "Proceso 3" },
        ];
      },
    },
  };
  </script>