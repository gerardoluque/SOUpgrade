<template>
  <div
    class="multisteps-form__panel border-radius-xl bg-white"
    data-animation="FadeIn"
  >
    <h5 class="font-weight-bolder mb-0">Configuración</h5>
    <p class="mb-0 text-sm">Configuración del Usuario</p>
    <div class="multisteps-form__content">
    <div class="multisteps-form__form">

         <div class="mb-3">           

      <label for="accountEnabled" class="form-label">Usuario AD</label>
      <material-switch id="accountEnabled" name="accountEnabled"    
       v-model:checked="user.esUsuarioAD"
      />
       

      </div>
      
      <div class="mb-3">
        <material-input
                      id="mailNickname"
                      type="text"
                      label="Usuario"                      
                      name="mailNickname"
                      autocomplete="off"
                       :isRequired="true"
                      placeholder="Ingrese el nombre del usuario"
                      v-model="user.userName"
                       :disabled="user.id != null"
                      @input="user.userName = user.userName.replace(/[^a-zA-ZáéíóúÁÉÍÓÚñÑ\\s]/g, '').toUpperCase()"

                    />
      </div>
      <div class="mb-3">
          <label :hidden="user.id == null ? true : false"  for="labelpasswordReset" class="form-label">Cambiar Contraseña</label>
        <div :hidden="user.id == null ? true : false">
          <material-switch  id="passwordReset" name="passwordReset"  
        label="Restablecer Password"            
         v-model:checked="passwordReset"
      />
      </div>
          <material-input
                   id="password"
                    :type="showPassword ? 'text' : 'password'"
                   label="Contraseña"
                      
                   name="password"
                   autocomplete="new-password"
                    :isRequired="true"
                    placeholder="Ingrese la contraseña"
                    v-model="user.password"
                    :disabled="user.id && !passwordReset"                       
                  />

                    <button
            type="button"
            class="btn btn-sm btn-outline-secondary mt-2"
            @click="togglePasswordVisibility"
          >
            {{ showPassword ? "Ocultar" : "Mostrar" }} Contraseña
          </button>
         <p v-if="passwordError" class="text-danger mt-1">{{ passwordError }}</p>

      </div>

     
      <div class="mb-3">           

      <label for="accountEnabled" class="form-label">Activo</label>
      <material-switch id="accountEnabled" name="accountEnabled"  
      permiso="Usuarios.Inactivar"  
       v-model:checked="user.activo"
      />
       

      </div>
      <div class="mb-3">           

      <label for="requiere2FA" class="form-label">Requiere Autenticación de Dos Factores (2FA)</label>
      <material-switch id="requiere2FA" name="requiere2FA"  
       v-model:checked="user.requiere2FA"
      />
       

      </div>
      <div class="mb-3">
        <label for="grupoId"    class="form-label">*Grupo</label>
        <select
          id="grupoId"          
          class="form-control"
          placeholder="Ingresa el Grupo"  
          v-model="user.grupoId"  
          v-permiso="'Usuarios.SeleccionarGrupo'"       
         >
         <option v-for="row in this.groups" :key="row.id" :value="row.id">
          {{ row.nombre }}
         </option>
        </select>
      </div>
      <div class="mb-3">
        <label for="rolId" class="form-label">*Rol</label>
        <select
          id="rolId"
          class="form-control"
          placeholder="Ingresa el Rol"
          :value="user.rolId"
          @change="handleRolChange"
          v-permiso="'Usuarios.SeleccionarRol'"
        >
         <option v-for="role in this.roles" :key="role.id" :value="role.id">
          {{ role.name }}
         </option>
        </select>
      </div>
      <div class="mb-3">
        
        <MaterialTags id="corporaciones"           
        :options="corporaciones  || []"
        v-model="selectedCorporaciones"
        label="*Corporaciones"
        />
      </div>
      <div class="mb-3">
        <material-input
                      id="inactiveTime"
                      type="number"
                      label="Tiempo Inactivo"                   
                      name="inactiveTime"                                           
                      autocomplete="off"
                      v-model="user.tiempoInactividad"
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
import { ref, onMounted,computed,watch } from "vue";
import Swal from 'sweetalert2'
import MaterialTags from "@/components/common/MaterialTags.vue";
import MaterialSwitch from "@/components/common/MaterialSwitch.vue";

export default {
  components: {    
    MaterialInput,    
    MaterialTags,
    MaterialSwitch
   
  },
  name: "UsuarioInfo",
  setup() {
    const usuarioStore = useUsuarioStore();
    const { selecteduser: user,roles,groups,corporaciones } = storeToRefs(usuarioStore);

      const passwordError = ref("");
    const showPassword = ref(false);
    const passwordReset = ref(false);
    
     
          // ...existing code...
      const validatePassword = (password) => {
        // Al menos 8 caracteres, una mayúscula, una minúscula, un número y un carácter especial
        const regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$/;
        return regex.test(password);
      };

     
      watch(() => user.value.password, (newVal) => {
      if (!validatePassword(newVal)) {
      passwordError.value = "La contraseña debe tener mínimo 8 caracteres, una mayúscula, una minúscula, un número y un carácter especial.";
      } else {
      passwordError.value = "";
      }
      }); 

      const handleSubmit = () => {
      if (!validatePassword(user.value.password)) {
      passwordError.value = "La contraseña debe tener mínimo 8 caracteres, una mayúscula, una minúscula, un número y un carácter especial.";
      return;
      }
      // ...resto de tu lógica de guardado...
      };

 

    storeToRefs(usuarioStore);
   
     const selectedCorporaciones = computed({
      get() {

     return Array.isArray(user.value.corporacionesSeleccinados)
      ? user.value.corporacionesSeleccinados.map((val) => ({
          name: val,
        }))
      : [];
  },
  set(newValue) {
    user.value.corporacionesSeleccinados = Array.isArray(newValue)
      ? newValue.map(x => x.name) : [];
       
  },
});

     

    const togglePasswordVisibility = () => {
      showPassword.value = !showPassword.value;
    };

    const handleRolChange = async (ev) => {
      try {
        const newVal = ev?.target?.value;
        const currentVal = user.value?.rolId ?? null;
        if (currentVal != null && String(currentVal) !== String(newVal)) {
          const res = await Swal.fire({
            title: 'Confirmación',
            text: 'Los procesos se eliminarán, ¿quiere continuar?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí',
            cancelButtonText: 'No',
            reverseButtons: true,
          });

          if (!res.isConfirmed) {
            // revert select
            ev.target.value = currentVal;
            return;
          }

          // confirmed: set role and clear permisos
          user.value.rolId = newVal;
          user.value.usuarioPermisos = [];
          user.value.permisos = [];
        } else {
          user.value.rolId = newVal;
        }
      } catch (err) {
        console.error('handleRolChange error', err);
      }
    };


     

    onMounted(() => {
  if (!user.value) {
    user.value = []; // Initialize users if undefined


  }

  

  if (!user.corporacionesSeleccinados) {
    user.corporacionesSeleccinados = []; // Initialize users if undefined
  }

  if (!corporaciones.value) {
    corporaciones.value = []; // Inicializa corporaciones como un array vacío
  }

   

  if (!user.value.length) {
          usuarioStore.getAllGroups();
          usuarioStore.getAllRol();
          usuarioStore.getAllCorporations();
        
  }


 

});


   

    return {
      user,
      roles,
      groups,
      corporaciones,
      handleSubmit,
      passwordError,
      showPassword,
      togglePasswordVisibility,
      selectedCorporaciones,
      passwordReset,
      handleRolChange,
    };
  },
};
</script>

<style scoped>
.form-control {
  border: 4px solid #ccc !important;
  border-radius: 4px;
  background: #fff;
}

</style>