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
          {{ grupo.id ? "Actualizar Grupo" : "Crear Nuevo Grupo" }}
        </h3>
        <p class="lead font-weight-normal opacity-8 mb-7 text-center">
          {{ grupo.id ? "Edita los campos para actualizar el Grupo." : "Rellena los campos para agregar un Grupo." }}
        </p>
        <div class="card">
          <div class="card-header p-0 position-relative mt-n5 mx-3 z-index-2">
            <div class="bg-gradient-primary shadow-primary border-radius-lg pt-4 pb-3">
              <div class="multisteps-form__progress">

                <button class="multisteps-form__progress-btn" type="button" title="User Info"
                  :class="activeStep >= 0 ? activeClass : ''" @click="activeStep = 0">
                  <span>Información de Grupo</span>
                </button>
              </div>
            </div>
          </div>
          <div class="card-body">
            <form class="multisteps-form__form">

              <grupo-info :class="activeStep === 0 ? activeClass : ''" />
              <div class="mt-4 d-flex justify-content-between">

                <div>

                </div>

                <MaterialButton
                  id="save-grupo-button"
                 color="primary" 
                 variant="gradient"
                  @click.prevent="handleSave"
                >
                  {{ grupo.id ? "Actualizar" : "Guardar" }}
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
import GrupoInfo from "@mod1/modules/grupos/components/GrupoInfo.vue";
import MaterialButton from "@/components/common/MaterialButton.vue";
import { useGruposStore } from "@mod1/modules/grupos/store/useGruposStore";
import { storeToRefs } from "pinia";
import { useRouter } from "vue-router";
 
export default {
  name: "GruposCreate",
  components: {
    GrupoInfo,
    MaterialButton,
  },
  setup() {
    const gruposStore = useGruposStore();
    const { grupo } = storeToRefs(gruposStore); // Use storeToRefs for reactivity
        const router = useRouter();
      
         const navigateToList = () => {
      gruposStore.resetSelectedGrupo();
      router.push({ name: "GruposList" });
      };

    const handleSave = async () => {

       let result;
      if (grupo.value.id) {
       
        result = await gruposStore.updateGrupo();            
          
      } else {     
        result =  await gruposStore.createGrupo();             
      }

      
       if(result)
      {
          router.push({ name: "GruposList" }); 
      }
    };

    return {
      grupo,
   
      handleSave,
      gruposStore,
      navigateToList,
    };
  },
};
</script>