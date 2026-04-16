<template>
    <div class="login-container">
      <h1>Iniciar Sesión</h1>
      <button @click="login">Iniciar Sesión con Azure AD</button>
    </div>
  </template>
  
  <script>
  import { msalInstance } from "../authConfig";
  
  export default {
    methods: {
      async login() {
        try {
          await msalInstance.loginRedirect({
            scopes: ["User.Read"], // Cambia los scopes según tus necesidades
          });
        } catch (error) {
          console.error("Error al iniciar sesión:", error);
        }
      },
    },
  };
  </script>
  
  <style>
  .login-container {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    height: 100vh;
  }
  </style>