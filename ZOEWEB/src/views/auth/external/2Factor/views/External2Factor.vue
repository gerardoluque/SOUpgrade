<template>
  <div class="py-4 container-fluid">
    <div class="row min-vh-80">
      <div class="col-lg-8 col-md-10 col-12 m-auto">
        <h3 class="mt-3 mb-0 text-center">Autenticacion Usuario Externo</h3>
        <p class="lead font-weight-normal opacity-8 mb-7 text-center">
          Sigue los pasos para completar la autenticacion del usuario externo.
        </p>
        <div class="card">
          <div class="card-body">
            <form class="multisteps-form__form">
              <Qr v-if="activeStep === 0" :class="activeClass" />
              <Auth2Factor v-if="activeStep === 1" :class="activeClass" />
              <div class="mt-4 d-flex justify-content-between">
                <MaterialButton
                  id="prev-step-button"
                  color="secondary"
                  variant="outline"
                  :disabled="activeStep === 0"
                  @click.prevent="prevStep"
                >
                  Previo
                </MaterialButton>
                <MaterialButton
                  id="next-step-button"
                  color="primary"
                  variant="gradient"
                  @click.prevent="activeStep === 1 ? handleSubmit() : handleNextStep()"
                >
                  {{ activeStep === 1 ? "Terminar" : "Siguiente" }}
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
import Auth2Factor from "../components/Auth2Factor.vue";
import Qr from "../components/Qr.vue";
import MaterialButton from "@/components/common/MaterialButton.vue";
import { use2FactorStore } from "../store/use2FactorStore";
import { storeToRefs } from "pinia";
import { onMounted } from "vue";

export default {
  name: "External2Factor",
  components: {
    Qr,
    Auth2Factor,
    MaterialButton,
  },
  setup() {
    const use2Factor = use2FactorStore();
    const { activeStep, activeClass, nextStep, prevStep } = storeToRefs(use2Factor);

    const handleNextStep = () => {
      nextStep.value();
    };

    const handleSubmit = () => {
      // Aquí va la lógica de submit final
    };

    onMounted(() => {
      activeStep.value = 0;
      use2Factor.setActiveStep(activeStep.value);
    });

    return {
      activeStep,
      activeClass,
      nextStep,
      prevStep,
      handleNextStep,
      handleSubmit,
    };
  },
};
</script>