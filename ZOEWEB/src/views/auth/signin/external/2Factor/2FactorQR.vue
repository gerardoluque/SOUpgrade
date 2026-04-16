<template>
  <navbar btn-background="bg-gradient-success" />
  <div
    class="page-header align-items-start min-vh-100"
    :style="{ backgroundImage: `url(${require('@/assets/img/aux-imgbg.jpg')})`, opacity: 0.7 }"
  >
    <span class="mask bg-gradient-dark opacity-6"></span>
    <div class="container my-auto">
      <div class="row">

          <div class="col-lg-4 col-md-8 col-12 mx-auto">
          <div class="card z-index-0 fadeIn3 fadeInBottom"  style="width:500px;">
            <div class="card-header p-0 position-relative mt-n4 mx-3 z-index-2">
              <div 
                class="bg-gradient-primary shadow-primary border-radius-lg py-3 pe-1"
              >
                <h4 class="text-white font-weight-bolder text-center mt-2 mb-0">
                  Autenticación del usuario
                </h4>
                
                 
            </div>
 
               <div v-if="mostrarQr" id="mostrarQr" class="text-center">
                  <p class="mb-2 text-sm text-center">Escanea el siguiente código QR con tu app de autenticación y captura el numero de validación.</p>
                  <img
                  v-if="mostrarQr"
                  :src="external.qrImage"
                  alt="Código QR"
                  style="max-width: 240px; max-height: 240px; display:inline; margin: auto;"
                  />
                  <span v-else class="text-muted">Cargando QR...</span>
              
                </div>
                <div v-else class="text-center">
                <p class="mb-2 text-sm text-center" style="font-size: 1.1rem; font-weight: 500; text-align: left;">
                  Capture el código de verificación generado por su aplicación de autenticación.
                  Si no tiene la alta en su aplicación,
                  <a
href="#" style="color: #0078D4; text-align: left;   
                  text-decoration: underline; cursor: pointer; font-size: 1.1rem; font-weight: 500; " @click.prevent="getQr">
                  presione aquí para ver el QR
                  </a>.
                </p>

                 
                
                </div>
                 <form role="form" class="text-start mt-2" @submit.prevent="verify2Fa"> 
               <div class="mb-3">
                  <material-input 
                    id="codeNumber"
                    v-model="external.codenumber"
                    type="numeric"                   
                    label="Código de Verificación"
                    name="codeNumber"
                  />
                </div>

                
                 <div class="text-center">
                    <div class="d-flex justify-content-center gap-2">

                  <MaterialButton
class="m-2 flex-fill" type="button"   color="secondary" size="md" 
                  variant="gradient" style="min-width: 180px;"  @click="returnLogin">                    
                  Volver al Login
                  </MaterialButton>
                 
                  <MaterialButton
class="m-2 flex-fill" color="primary" 
                  size="md" variant="gradient" 
                   type="submit" 
                  style="min-width: 180px;">                    
                  Validar Codigo
                  </MaterialButton>
                    
                </div>
                  </div>
              </form>

        
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
   </div>
     <pre>{{ external.value ? external.value : 'Sin datos de usuario externo' }}</pre>
</template>

<script>
import Navbar from "@/examples/PageLayout/Navbar.vue";
import MaterialButton from "@/components/common/MaterialButton.vue";
import MaterialInput from "@/components/common/MaterialInput.vue";
import { useRouter } from "vue-router";

import { useMainStore } from "@/store/useMainStore";
import { storeToRefs } from "pinia";
import { onMounted,ref } from "vue";

export default {
  name: "2FactorQR",
  components: {
    Navbar,
    MaterialButton,
    MaterialInput,
  },
  setup() {
    const store = useMainStore();
    const { externalUser : external } = storeToRefs(store);
const router = useRouter();
    const toggleDisplay = () => {
      store.toggleEveryDisplay();
      store.toggleHideConfig();
    };

      const mostrarQr = ref(false);


    const returnLogin = () => {
         router.push({ name: "/" });
    };

     const verify2Fa = async () => {

      const valid = await store.verify2Fa();
      if(valid.success) {
        external.value.isAppAuthenticated = true;
        external.tokem = valid.data.token;
        external.value.is2FaEnabled = true;
        mostrarQr.value = false;
          
        router.push({ name: "UserInfo" });
      } else {
        external.value.isAppAuthenticated = false;
        mostrarQr.value = true;
      } 
     };

    const getQr = async () => {
      const result = await store.enable2Fa();
     
      if(result.success) 
      {
      
        external.value.qrImage = result.data.qrCodeImage;
        external.value.isAppAuthenticated = false;
        mostrarQr.value = true;
       
      } else {
        external.value.qrImage = null;
        external.value.isAppAuthenticated = true;
        mostrarQr.value = false;
      } 
      
    }

     onMounted(async () => {
      if(!external.value.isAppAuthenticated) {
          await store.enable2Fa();
      }
        
    });


    return { toggleDisplay,external,returnLogin,getQr,mostrarQr,verify2Fa };
  },
  beforeMount() {
    this.toggleDisplay();
  },
  beforeUnmount() {
    this.toggleDisplay();
  },
};
</script>