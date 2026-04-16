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
        <h3 class="mt-3 mb-0 text-center">{{selectedProceso.id != null ? "Modifica Proceso" : "Crear Nuevo Proceso"  }}</h3>
        <p class="lead font-weight-normal opacity-8 mb-7 text-center">
          {{selectedProceso.id != null ? "Rellena los campos para modificar el Proceso." : "Rellena los campos para agregar un Nuevo Proceso."  }}
          
        </p>
        <div class="card">
          <div class="card-header p-0 position-relative mt-n5 mx-3 z-index-2">
            <div class="bg-gradient-primary shadow-primary border-radius-lg pt-4 pb-3">
              <div class="multisteps-form__progress">

                <button class="multisteps-form__progress-btn" type="button" title="User Info"
                  :class="activeStep >= 0 ? activeClass : ''" @click="activeStep = 0">
                  <span>Informacion de Proceso</span>
                </button>
              </div>
            </div>
          </div>
          <div class="card-body">
            <form class="multisteps-form__form">

              <proceso-info :class="activeStep === 0 ? activeClass : ''" />
              <div class="mt-4 d-flex justify-content-between">

                <div>

                </div>

                <MaterialButton id="next-step-button" color="success" variant="gradient" 
                @click.prevent="handleSubmit">
                  {{selectedProceso.id != null ? "Modificar" : "Guardar"  }}
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
import ProcesoInfo from "@mod1/modules/procesos/components/ProcesoInfo.vue";
import MaterialButton from "@/components/common/MaterialButton.vue";
import { useProcesoStore } from "@mod1/modules/procesos/store/useProcesoStore";
import { storeToRefs } from "pinia";
import { useRouter } from "vue-router";

export default {
  name: "ProcesoCreate",
  components: {
    ProcesoInfo,
    MaterialButton,
  },
  setup() {
    const procesoStore = useProcesoStore();
    const { activeStep, activeClass,selectedProceso } = storeToRefs(procesoStore);    
    const router = useRouter();

    const navigateToList = () => {
      procesoStore.reset();
      router.push({ name: "ProcesoList" });
      };

      const handleSubmit = () => {      
      
      if(procesoStore.selectedProceso.id == null)
      {   procesoStore.createProcess(); }
      else
      {   procesoStore.updateProcess(); }
      
          navigateToList();
      }

    return {
      activeStep,
      activeClass,
      selectedProceso,
      procesoStore,
      navigateToList,
      handleSubmit
    };
  },
};
</script>