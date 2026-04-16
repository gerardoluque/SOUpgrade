<template>
  <div
    class="multisteps-form__panel border-radius-xl bg-white"
    data-animation="FadeIn"
  >
    <h5 class="font-weight-bolder mb-0">Informacion Usuario</h5>
    <p class="mb-0 text-sm"></p>
    <div class="multisteps-form__content">
      <div v-if="user">
      <div class="row mt-3">
        <div class="col-12 col-sm-6">
          <material-input id="firstName" v-model="user.givenName" label="Nombre" variant="static" disabled="true"       />
        </div>
        <div class="col-12 col-sm-6 mt-3 mt-sm-0">
          <material-input id="lastName" v-model="user.lastName" label="Apellido Paterno" variant="dynamic" disabled="true"       />
        </div>
      </div>
      <div class="row mt-3">
        <div class="col-12 col-sm-6">
          <material-input id="company" v-model="user.userPrincipalName" label="Usuario"  variant="dynamic" disabled="true"      />
        </div>
        <div class="col-12 col-sm-6 mt-3 mt-sm-0">
          <material-input
            id="email"
            v-model="user.mail"
            type="email"                                  
            label="Correo Electrónico" 
            variant="dynamic"  
            disabled="true"               
          />
          
        </div>
      </div>
      <div class="row mt-3">
        <div class="col-12 col-sm-6">
          <material-input
            id="phone"
            v-model="user.mobilePhone"
            type="phone"
            label="Celular"
            variant="dynamic"     
            disabled="true"      
          />
        </div>
      </div>
      </div>
      <div v-else>
        <p>Cargando información del usuario...</p>
      </div>
    </div>
  </div>
</template>

<script>
import MaterialInput from "@/components/common/MaterialInput.vue";
import { useMainStore } from "@/store/useMainStore.js";
import { storeToRefs } from "pinia";

export default {
  name: "UserInfo",
  components: {
    MaterialInput,
    
  },
  setup() {
    const store = useMainStore();
    const { userdata: user } = storeToRefs(store);
    try {
      
      store.getUserInfo();
    } catch (error) {  console.log("Error fetching user info:", error);
      
    }
    

     
    

    return { user};

  },
  
  
};
</script>
