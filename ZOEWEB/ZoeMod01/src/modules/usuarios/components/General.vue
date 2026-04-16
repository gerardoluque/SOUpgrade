<template>
  <div
    class="multisteps-form__panel border-radius-xl bg-white"
    data-animation="FadeIn"
  >
    <h5 class="font-weight-bolder mb-0">General</h5>
    <p class="mb-0 text-sm">Información General del Usuario</p>
    <div class="multisteps-form__content">
    <div>
      <div class="mb-3">
        <MaterialInput
                      id="givenName"
                      type="text"
                      label="Nombre"                     
                      name="givenName"
                      autocomplete="off"
                       
                      v-model="user.nombre"
                      :isRequired="true"
                      placeholder="Ingrese el nombre"
                      @input="user.nombre = user.nombre.replace(/[^a-zA-ZáéíóúÁÉÍÓÚñÑ\s]/g, '').toUpperCase()"
                    />
      </div>
     
      <div class="mb-3">        
        <MaterialInput
                      id="lastname"
                      type="text"
                      label="Primer Apellido"
                       placeholder="Ingrese el apellido"
                      name="lastname"
                     :isRequired="true"
                      autocomplete="off"
                       v-model="user.primerApellido"
                      @input="user.primerApellido = user.primerApellido.replace(/[^a-zA-ZáéíóúÁÉÍÓÚñÑ\s]/g, '').toUpperCase()"
                    />
      </div>
      <div class="mb-3">        
        <MaterialInput
                      id="surname"
                      type="text"
                      label="Segundo Apellido"
                      placeholder=""
                      name="surname"
                      autocomplete="off"
                      v-model="user.segundoApellido"
                      @input="user.segundoApellido = user.segundoApellido.replace(/[^a-zA-ZáéíóúÁÉÍÓÚñÑ\s]/g, '').toUpperCase()"
                    />
      </div>
      <div class="mb-3">
        <MaterialInput
                      id="mail"
                      type="email"
                      label="Correo Electrónico"                     
                      name="mail"
                   :isRequired="true"
                 autocomplete="off"
                      placeholder="Ingrese el correo electronico."
                      v-model="user.email"
                    
                     
                    />
                  
      </div>  
     
      <div class="mb-3">
        <MaterialInput
            id="mobilePhone"
            type="phone"           
            label="Celular"           
          v-model="user.telefono"
          autocomplete="off"
           @input="user.telefono = user.telefono.replace(/[^0-9]/g, '')"
          />

      </div>
       
      
    </div>
  </div>
</div>
</template>

<script>
import { useUsuarioStore } from "@mod1/modules/usuarios/store/useUsuarioStore";
import { storeToRefs } from "pinia";
import MaterialInput from "@/components/common/MaterialInput.vue";
 
 
export default {
  components: {    
    MaterialInput,
    
  },
  name: "UsuarioInfo",
  setup() {
    const usuarioStore = useUsuarioStore();
    const { selecteduser: user } = storeToRefs(usuarioStore);

    


    return { user  };
  },
};
</script>