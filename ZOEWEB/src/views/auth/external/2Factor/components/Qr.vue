<template>
<div class="multisteps-form__panel border-radius-xl bg-white" data-animation="FadeIn">
    <h5 class="font-weight-bolder mb-0 text-center">Genera la autenticación del usuario</h5>
    <p class="mb-0 text-sm text-center">Escanea el siguiente código QR con tu app de autenticación</p>
    <div class="multisteps-form__content d-flex justify-content-center align-items-center" style="min-height: 350px;">
      <img
        v-if="selecteduser.qrImage"
        :src="selecteduser.qrImage"
        alt="Código QR"
        style="max-width: 240px; max-height: 240px; display: block; margin: auto;"
      />
      <span v-else class="text-muted">Cargando QR...</span>
    </div>
  </div>
</template>

<script>    
import { storeToRefs } from "pinia";
import { use2FactorStore } from "@../../../src/views/auth/external/2Factor/store/use2FactorStore";
 import { onMounted } from "vue";


export default {
  name: "Qr",
  components: {    
     
    
  },
  setup() {
     const use2Factor = use2FactorStore();
    const { selecteduser } = storeToRefs(use2Factor);

    onMounted(() => {
     ;
          use2Factor.setActiveStep(1);

  });


    return { selecteduser  };
  },
};

</script>

 