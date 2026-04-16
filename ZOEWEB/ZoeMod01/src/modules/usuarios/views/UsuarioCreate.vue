<template>
  <div class="py-4 container-fluid">

    <div class="d-sm-flex justify-content-between">
      <div>
        <material-button color="primary" variant="gradient" @click="navigateToList">
          Regresar
        </material-button>
      </div>
    </div>

    <div class="row min-vh-80">
      <div class="col-lg-8 col-md-10 col-12 m-auto">
        <h3 class="mt-3 mb-0 text-center">{{ isCreateMode ? "Crear Nuevo Usuario" : "Editar Usuario" }} </h3>
        <p class="lead font-weight-normal opacity-8 mb-7 text-center">
          Rellena los campos para agregar un Nuevo Usuario
        </p>
        <div class="card">         
           <div class="card-header p-0 position-relative mt-n5 mx-3 z-index-2">
            <div class="bg-gradient-primary shadow-primary border-radius-lg pt-4 pb-3">
              <div class="multisteps-form__progress">

                <button class="multisteps-form__progress-btn" type="button" title="User Info">
                  <span>Información de Usuarios</span>
                </button>
                <button
                        class="multisteps-form__progress-btn"
                        type="button"
                        title="Address"
                       
                      >
                        <span>Configuración</span>
                      </button>
                          <button
                        class="multisteps-form__progress-btn"
                        type="button"
                        title="Order Info"
                      
                      >
                        <span>Permisos</span>
                      </button>
              </div>
            </div>
          </div>
          <div class="card-body">
            <form class="multisteps-form__form">
             
              <general :class="activeStep === 0 ? activeClass : ''" />
              <configuracion :class="activeStep === 1 ? activeClass : ''" />
              <permisos :class="activeStep === 2 ? activeClass : ''" />
              <div class="mt-4 d-flex justify-content-between">
              
                <MaterialButton
                  id="prev-step-button"
                  color="secondary"
                  variant="outline"
                  :disabled="activeStep === 0"
                  @click.prevent="prevStep"
                >
                  Anterior
                </MaterialButton>
               
                <MaterialButton
                  id="next-step-button"
                  color="primary"
                  variant="gradient"
                  @click.prevent="activeStep === 2 ? handleSubmit() : handleNextStep()"
                >
                {{ activeStep === 2 ? "Guardar" : "Siguiente" }}
                </MaterialButton>
              </div>              
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import General from "@mod1/modules/usuarios/components/General.vue";
import Configuracion from "@mod1/modules/usuarios/components/Configuracion.vue";
import Permisos from "@mod1/modules/usuarios/components/Permisos.vue";
import MaterialButton from "@/components/common/MaterialButton.vue";
import { useUsuarioStore } from "@mod1/modules/usuarios/store/useUsuarioStore";
import { storeToRefs } from "pinia";
import { useRouter } from "vue-router";
 import { useMainStore } from '@/store/useMainStore' 
import { onMounted,computed } from "vue";




export default {
  name: "UsuarioCreate",
  components: {
    General,
    Configuracion,
    Permisos,
    MaterialButton,
  },
  setup() {
    const usuariosStore = useUsuarioStore();
    const { activeStep, activeClass } = storeToRefs(usuariosStore);
    const { nextStep, prevStep } = usuariosStore;
    const router = useRouter();
    const mainStore = useMainStore();
    const isCreateMode = computed(() => usuariosStore.selecteduser.id == null);

     onMounted(() => {
    activeStep.value = 0;
  });

    const navigateToList = () => {
      usuariosStore.resetSelected();
       activeStep.value = 0;
      router.push({ name: "UsuarioList" });
      };

    const  handleSubmit = async  () => {
      
      usuariosStore.loadPermisos(); // Carga los permisos antes de guardar
      try
      {
         let result;
          if(usuariosStore.selecteduser.id != null)
          {      
             result =  await usuariosStore.updateUser();
           
          }
          else
          {
             result =  await usuariosStore.setUser(); 
             
          } 

         if(result){

          activeStep.value = 0; // Reinicia el paso activo a 0 después de guardar
          
          navigateToList(); // Redirige a la lista de usuarios después de guardar


         }
          
          
      }
      catch(error)
      {
         mainStore.triggerAlert({
        message:  "Ocurrió un error al guardar el usuario",
        color: "danger",
        icon: "error",
        });
         
      }
    };

    const setActiveStep= () => {
      if (validateStep()) {
          if (activeStep.value === 1) {
         
           
            usuariosStore.setRolId(usuariosStore.selecteduser.rolId);
            usuariosStore.getProcessByRol();  // Asegúrate de establecer el rolId
          }
        nextStep();
      }
    };
  const handleNextStep = () => {
      if (validateStep()) {
          if (activeStep.value === 1) {
         
            
            usuariosStore.setRolId(usuariosStore.selecteduser.rolId);
            usuariosStore.getProcessByRol();  // Asegúrate de establecer el rolId
          }
        nextStep();
      }
    };



  const validateStep = () => {
  let isValid = true;

    

    if (activeStep.value === 0) {
      // Validar campos del paso 1 (Información de Usuarios)
      if(!usuariosStore.selecteduser.nombre) {  

        

        isValid = false;       
        mainStore.triggerAlert({
        message: "Capture el nombre del usuario",
        color: "warning",
        icon: "warning",
        });
        return isValid;    
      }
      if(!usuariosStore.selecteduser.primerApellido) {  

      
      isValid = false; 
       mainStore.triggerAlert({
        message: "Capture el apellido del usuario.",
        color: "warning",
        icon: "warning",
        });
       return isValid;    
      }

      if(!usuariosStore.selecteduser.email) {  

        isValid = false; 
       mainStore.triggerAlert({
        message: "Capture el correo electronico.",
        color: "warning",
        icon: "warning",
        });
        return isValid;     


      }

       if(usuariosStore.selecteduser.email) {  

       const emailRegex =
        /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
      if (!emailRegex.test(usuariosStore.selecteduser.email)) {
          
        isValid = false; 
       mainStore.triggerAlert({
        message: "El correo no es valido",
        color: "warning",
        icon: "warning",
        });
        return isValid;     
      }
     
      }

      
      if(usuariosStore.selecteduser.telefono != null && usuariosStore.selecteduser.telefono.length > 0){

        if(usuariosStore.selecteduser.telefono.length < 10){

            isValid = false; 
       mainStore.triggerAlert({
        message: "El telefono debe tener al menos 10 caracteres.",
        color: "warning",
        icon: "warning",
        });
        return isValid;    
        }
      }

      let displayName = usuariosStore.selecteduser.nombre + " " + usuariosStore.selecteduser.primerApellido + usuariosStore.selecteduser.segundoApellido;
      if(displayName.length < 10)
      {
          isValid = false; 
        mainStore.triggerAlert({
          message:  "El nombre "+ displayName +" debe tener al menos 10 caracteres",
          color: "warning",
          icon: "warning",
          });
          return isValid;    

      }
      
  
    } else if (activeStep.value === 1) {
      // Validar campos del paso 2 (Configuración)
      if(!usuariosStore.selecteduser.userName) {  

      
       
        isValid = false; 
       mainStore.triggerAlert({
        message: "El nombre del usuario es requerido.",
        color: "warning",
        icon: "warning",
        });
       return isValid;    
      }

         if(usuariosStore.selecteduser.userName && (usuariosStore.selecteduser.userName.length < 5 || usuariosStore.selecteduser.userName.length > 15)) { 

          isValid = false; 
       mainStore.triggerAlert({
        message: "El nombre de usuario debe ser mayor a 5 caracteres y  menos de 15.",
        color: "warning",
        icon: "warning",
        });
       return isValid;    

         }

      
      if((usuariosStore.selecteduser.id == null || usuariosStore.selecteduser.id == 0) 
      && !usuariosStore.selecteduser.password) {  

      
        isValid = false; 
       mainStore.triggerAlert({
        message: "Capture la contraseña.",
        color: "warning",
        icon: "warning",
        });
       return isValid;       
      }
const regexPassword = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$/;
       if((usuariosStore.selecteduser.id == null || usuariosStore.selecteduser.id == 0) 
      && usuariosStore.selecteduser.password.length > 0) {  

      if(!regexPassword.test(usuariosStore.selecteduser.password)){
          isValid = false;
          mainStore.triggerAlert({
          message:
          "La contraseña debe tener mínimo 8 caracteres, una mayúscula, una minúscula, un número y un carácter especial.",
          color: "warning",
          icon: "warning",
          });
          return isValid;

      }
      
      }



      if(!usuariosStore.selecteduser.grupoId) {  

      
       isValid = false; 
       mainStore.triggerAlert({
        message: "Es necesario capturar un grupo",
        color: "warning",
        icon: "warning",
        });
        return isValid;     
    
    
    }
      if(!usuariosStore.selecteduser.rolId) {  

      
       isValid = false; 
       mainStore.triggerAlert({
        message: "Es necesario capturar un rol",
        color: "warning",
        icon: "warning",
        });
       return isValid;     


      }
      if(usuariosStore.selecteduser.corporacionesSeleccinados === null || usuariosStore.selecteduser.corporacionesSeleccinados.length === 0) {  

      
       isValid = false; 
       mainStore.triggerAlert({
        message: "Es necesario capturar una corporación",
        color: "warning",
        icon: "warning",
        });
        return isValid;            

      } 
    } else if (activeStep.value === 2) {
      // Validar campos del paso 3 (Permisos)
      isValid = true;  
    }

    return isValid;
  };

 
    return {
      activeStep,
      activeClass,
      nextStep,
      prevStep,
      setActiveStep,
      handleSubmit,
      handleNextStep,
      navigateToList,
      isCreateMode,
       
    };
  },
};
</script>