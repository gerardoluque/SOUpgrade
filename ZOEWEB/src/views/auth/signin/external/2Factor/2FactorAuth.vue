<template>
  <navbar btn-background="bg-gradient-success" />
  <div
    class="page-header align-items-start min-vh-100"
    :style="{ backgroundImage: `url(${require('@/assets/img/aux-imgbg.jpg')})`, opacity: 0.7 }"
  >
    <span class="mask bg-gradient-dark opacity-6"></span>
    <div class="container my-auto">
      <div class="row">

        <div class="multisteps-form__panel border-radius-xl bg-white" data-animation="FadeIn">
        <h5 class="font-weight-bolder mb-0 text-center">Genera la autenticación del usuario</h5>
        <p class="mb-0 text-sm text-center">Escanea el siguiente código QR con tu app de autenticación</p>
        <div class="multisteps-form__content d-flex justify-content-center align-items-center" style="min-height: 350px;">
        <img
        v-if="externalUser.qrImage"
        :src="externalUser.qrImage"
        alt="Código QR"
        style="max-width: 240px; max-height: 240px; display: block; margin: auto;"
        />
        
        <span v-else class="text-muted">Cargando QR...</span>

        <MaterialButton class="m-1" color="secondary" size="md" variant="gradient" @click="cleanBitacoraError">
            Validar QR
        </MaterialButton>
        </div>
        </div>
         
      </div>
    </div>
    <footer class="footer position-absolute bottom-2 py-2 w-100">
      <div class="container">
        <div class="row align-items-center justify-content-lg-between">
          <div class="col-12 col-md-6 my-auto">
            <div class="copyright text-center text-sm text-white text-lg-start">
              © {{ new Date().getFullYear() }}
            </div>
          </div>
        </div>
      </div>
    </footer>
  </div>
</template>

<script>
import Navbar from "@/examples/PageLayout/Navbar.vue";
import MaterialButton from "@/components/common/MaterialButton.vue";
import { useMainStore } from "@/store/useMainStore";
import { storeToRefs } from "pinia";

export default {
  name: "2FactorQR",
  components: {
    Navbar,
    MaterialButton,
  },
  setup() {
    const store = useMainStore();
    const { externalUser } = storeToRefs(store);

    const toggleDisplay = () => {
      store.toggleEveryDisplay();
      store.toggleHideConfig();
    };

    return { toggleDisplay,externalUser };
  },
  beforeMount() {
    this.toggleDisplay();
  },
  beforeUnmount() {
    this.toggleDisplay();
  },
};
</script>