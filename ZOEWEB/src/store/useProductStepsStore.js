import { defineStore } from "pinia";

export const useProductStepsStore = defineStore("productSteps", {
  state: () => ({
    activeStep: 0, 
    activeClass: "js-active position-relative", 
    formSteps: 3,
  }),
  actions: {
    nextStep() {
      if (this.activeStep < this.formSteps) {
        this.activeStep += 1;
      }
    },
    prevStep() {
      if (this.activeStep > 0) {
        this.activeStep -= 1;
      }
    },
    setActiveStep(step) {
      if (step >= 0 && step <= this.formSteps) {
        this.activeStep = step;
      }
    },
  },
});