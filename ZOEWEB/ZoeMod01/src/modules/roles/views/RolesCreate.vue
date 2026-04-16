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
        <h3 class="mt-3 mb-0 text-center">
          {{ rol.id ? "Actualizar Rol" : "Crear Nuevo Rol" }}
        </h3>
        <p class="lead font-weight-normal opacity-8 mb-7 text-center">
          {{ rol.id ? "Edita los campos para actualizar el Rol." : "Rellena los campos para agregar un Nuevo Rol." }}
        </p>
        <div class="card">
          <div class="card-header p-0 position-relative mt-n5 mx-3 z-index-2">
            <div class="bg-gradient-primary shadow-primary border-radius-lg pt-4 pb-3">
              <div class="multisteps-form__progress">
                <button
                  class="multisteps-form__progress-btn"
                  type="button"
                  title="Información de Rol"
                  :class="activeStep >= 0 ? activeClass : ''"
                  @click="setActiveStep(0)"
                >
                  <span>Información de Rol</span>
                </button>
              </div>
            </div>
          </div>
          <div class="card-body">
            <form class="multisteps-form__form">
              <RolInfo :class="activeStep === 0 ? activeClass : ''" />
              <div class="mt-4 d-flex justify-content-between">
                <div></div>
                <MaterialButton
                  id="save-role-button"
                color="primary" 
                variant="gradient"
                  @click.prevent="handleSave"
                >
                  {{ rol.id ? "Actualizar" : "Guardar" }}
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
import RolInfo from "@mod1/modules/roles/components/RolInfo.vue";
import MaterialButton from "@/components/common/MaterialButton.vue";
import { useRolesStore } from "@mod1/modules/roles/store/useRolesStore";
import { storeToRefs } from "pinia";
import { useRouter } from "vue-router";
 import { useMainStore } from '@/store/useMainStore' 

export default {
  name: "RolesCreate",
  components: {
    RolInfo,
    MaterialButton,
  },
  setup() {
    const rolesStore = useRolesStore();
    const { rol, activeStep, activeClass } = storeToRefs(rolesStore); // Use storeToRefs for reactivity
           const router = useRouter();
            const mainStore = useMainStore();

            const navigateToList = () => {
      rolesStore.resetSelectedRol();
      router.push({ name: "RolesList" });
      };
          
    const handleSave = async () => {
       if(rol.value.procesos == null || rol.value.procesos.length == 0) {
          mainStore.triggerAlert({
                  message: "Es necesario capturar un proceso.",
                  color: "warning",
                  icon: "warning",
                  });
                  return;

      }
       let result;
      if (rol.value.id) {
       
        result = await rolesStore.updateRol();     
        
      } else {     
        result = await rolesStore.createRol();      
       
      }

      if(result)
      {
          router.push({ name: "RolesList" }); 
      }



    };
    

    return {
      rol,
      activeStep,
      activeClass,
      handleSave,
      setActiveStep: rolesStore.setActiveStep,navigateToList
    };
  },
};
</script>