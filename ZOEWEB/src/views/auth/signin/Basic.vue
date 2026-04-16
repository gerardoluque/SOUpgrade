<template>
  <navbar btn-background="bg-gradient-success" />
  <div
    class="page-header align-items-start min-vh-100"
    :style="{ backgroundImage: `url(${require('@/assets/img/aux-imgbg.jpg')})`, opacity: 0.7 }"
  >
    <span class="mask bg-gradient-dark opacity-6"></span>
    <div class="container my-auto">
      <div class="row">
        <div class="col-lg-4 col-md-8 col-12 mx-auto"  >
          <div class="card z-index-0 fadeIn3 fadeInBottom" style="width:500px;">
            <div class="card-header p-0 position-relative mt-n4 mx-3 z-index-2" >
              <div
                class="bg-gradient-primary shadow-primary border-radius-lg py-3 pe-1"
              >
             
                <h4 class="text-white font-weight-bolder text-center mt-2 mb-0">
                  Inicio de Sesión
                </h4>
              </div>
            </div>
            <div class="card-body">
                <form role="form" class="text-start mt-2" @submit.prevent="login">

                <div class="mb-3">
                  <material-input
                    id="email"
                    v-model="externalUser.username"
                    type="text"
                    label="Usuario"
                    name="email"
                    @input="externalUser.username = externalUser.username.replace(/[^a-zA-ZáéíóúÁÉÍÓÚñÑ\s]/g, '').toUpperCase()"
                  ></material-input>
                </div>
                <div class="mb-3">
                  <material-input
                    id="password"
                    v-model="externalUser.password"
                    type="password"
                    label="Constraseña"
                    autocomplete="current-password"
                    name="password"
                    onke
                    ></material-input>
                  
                    
                </div>
                <div class="text-center">
                    <div class="d-flex justify-content-center gap-2">
                    <a
href="#" style="
                    
                    color: black;
                    text-decoration: none;
                    cursor: pointer;
                    font-size: 1rem;
                    font-weight: 500;
                    min-width: 180px;
                    height: 40px;
                    
                    display: flex;
                    align-items: center;
                    justify-content: center;
                     
                    transition: background 0.2s;
                    "  onmouseover="this.style.backgroundColor='#0078D4'"
                    onmouseout="this.style.backgroundColor='#FFFFFF'"
                    @click.prevent="loginAzure"
                   
                    >
                   <svg width="32" height="32" viewBox="0 0 32 32" style="margin-right: 8px;">
                    <rect x="2" y="2" width="12" height="12" fill="#F35325"/>
                    <rect x="18" y="2" width="12" height="12" fill="#81BC06"/>
                    <rect x="2" y="18" width="12" height="12" fill="#05A6F0"/>
                    <rect x="18" y="18" width="12" height="12" fill="#FFBA08"/>
                  </svg>
                  Iniciar con Directorio Activo.
                </a> 
                 
                  <MaterialButton       
                  class="flex-fill d-flex align-items-center justify-content-center"
                  color="primary" 
                  size="md" 
                  variant="gradient" 
                  style="min-width: 180px; height: 40px; font-size: 1rem; font-weight: 500;"
                  >                    
                  Iniciar Sesión
                  </MaterialButton>
                 
                   
                </div>
                  </div>
               
  </form>
    
             
            </div>
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
import MaterialInput from "@/components/common/MaterialInput.vue";
import { useMainStore } from "@/store/useMainStore";
import MaterialButton from "@/components/common/MaterialButton.vue";
import { storeToRefs } from "pinia";
import { useRouter } from "vue-router";


export default {
  name: "SigninBasic",
  components: {
    Navbar,
    MaterialInput,
    MaterialButton
  },
  setup() {
    const store = useMainStore();
     const router = useRouter();
  const { externalUser } = storeToRefs(store);
    const toggleDisplay = () => {
      store.toggleEveryDisplay();
      store.toggleHideConfig();
    };


  const loginAzure =  async () => {         

    router.push({ name: "UserInfo" });
  }

     const login =  async () => {   
      
        if(!externalUser.value.username || !externalUser.value.password) {
          
          store.triggerAlert({
                    message: "Por favor, completa todos los campos.",
                    color: "warning",
                    icon: "error",

                  });
     
          return;
        }
      
       const result = await store.login();
    
       if(result.success === false && result.error.includes('400')) 
       {
          externalUser.is2FaEnabled =true;
          externalUser.isAppAuthenticated =false;
          router.push({ name: "2FactorQR" });
       }
     
       if(result.success) {
        
        if(result.data.requires2FA) {
            
          externalUser.is2FaEnabled =true;
          externalUser.isAppAuthenticated =true;
          router.push({ name: "2FactorQR" });
        } else 
        {
         
          externalUser.token = result.data.token;
          externalUser.is2FaEnabled =false;
          router.push({ name: "UserInfo" });
        }       
       }
        
    };


    return { toggleDisplay,externalUser,login,loginAzure };
  },
  beforeMount() {
    this.toggleDisplay();
  },
  beforeUnmount() {
    this.toggleDisplay();
  },
};
</script>