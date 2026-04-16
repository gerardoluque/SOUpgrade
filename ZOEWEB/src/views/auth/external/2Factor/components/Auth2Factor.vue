<template>
  <div class="d-flex flex-column justify-content-center align-items-center" style="min-height: 60vh;">
    <h5 class="mb-3 text-center">Verificación en dos pasos</h5>
    <p class="mb-4 text-center">Ingresa el código generado por tu app autenticadora</p>
    <form class="w-100" style="max-width: 350px;" @submit.prevent="handleSubmit">
      <div class="mb-3">
        <input
          v-model="code"
          type="text"
          maxlength="6"
          class="form-control text-center"
          placeholder="Código de autenticación"
          autocomplete="one-time-code"
          required
        />
      </div>
      <button type="submit" class="btn btn-primary w-100">Validar código</button>
    </form>
    <div v-if="error" class="alert alert-danger mt-3 text-center">
      {{ error }}
    </div>
  </div>
</template>

<script>    
import { storeToRefs } from "pinia";
import { use2FactorStore } from "@../../../src/views/auth/external/2Factor/store/use2FactorStore";
 import { ref } from "vue";
 


export default {
  name: "Auth2Factor",
  components: {    
     
    
  },
  setup() {
     const use2Factor = use2FactorStore();
    const { selecteduser } = storeToRefs(use2Factor);
    const code = ref("");
    const error = ref("");

    const handleSubmit = async () => {
      error.value = "";
      // Aquí deberías llamar a tu API o acción para validar el código
      // Ejemplo ficticio:
      const isValid = await use2Factor.validateCode(code.value);
      if (!isValid) {
        error.value = "El código ingresado no es válido. Intenta de nuevo.";
      } else {
        // Redirige o realiza la acción de acceso
      }
    };

    return { selecteduser, code, error, handleSubmit };   


     
  },
};

</script>

 