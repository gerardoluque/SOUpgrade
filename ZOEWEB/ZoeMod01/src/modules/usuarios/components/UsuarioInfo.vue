<template>
  <div class="container-fluid py-4">
    <div class="row">
      <div class="col-12">
        <div class="multisteps-form mb-9">
          <div class="row">
            <div class="col-12 col-lg-8 mx-auto my-5"></div>
          </div>
          <div class="row">
            <div class="col-12 col-lg-8 m-auto">
              <div class="card">
                <div
                  class="card-header p-0 position-relative mt-n4 mx-3 z-index-2"
                >
                  <div
                    class="bg-gradient-success shadow-success border-radius-lg pt-4 pb-3"
                  >
                    <div class="multisteps-form__progress">
                      <button
                        class="multisteps-form__progress-btn"
                        type="button"
                        title="General"
                        :class="activeStep >= 0 ? activeClass : ''"
                        @click="activeStep = 0"
                      >
                        <span>General</span>
                      </button>
                      <button
                        class="multisteps-form__progress-btn"
                        type="button"
                        title="Configuracion"
                        :class="activeStep >= 1 ? activeClass : ''"
                        @click="activeStep = 1"
                      >
                        Configuración
                      </button>
                      <button
                        class="multisteps-form__progress-btn"
                        type="button"
                        title="Permisos"
                        :class="activeStep >= 2 ? activeClass : ''"
                        @click="activeStep = 2"
                      >
                        Permisos
                      </button>
                    
                    </div>
                  </div>
                </div>
                <div class="card-body">
                  <form class="multisteps-form__form">
                    <!--single form panel-->
                    <general :class="activeStep === 0 ? activeClass : ''" />
                    <!--single form panel-->
                    <configuracion :class="activeStep === 1 ? activeClass : ''" />
                    <!--single form panel-->
                    <permisos :class="activeStep === 2 ? activeClass : ''" />
                  </form>
                </div>
              </div>
            </div>
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


export default {
  name: "NewUser",
  components: {
    General,
    Configuracion,
    Permisos 
  },
  data() {
    return {
      showMenu: false,
      activeClass: "js-active position-relative",
      activeStep: 0,
      formSteps: 3,
    };
  },
  methods: {
    nextStep() {
      if (this.activeStep < this.formSteps) {
        this.activeStep += 1;
        
      } else {
        this.activeStep -= 1;
        
      }
    },
    prevStep() {
      if (this.activeStep > 0) {
        this.activeStep -= 1;
        
      }
    },     
}
};
</script>