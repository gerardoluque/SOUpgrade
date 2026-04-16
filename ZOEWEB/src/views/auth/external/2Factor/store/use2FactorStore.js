import { defineStore } from "pinia";

 
export const use2FactorStore = defineStore("External2Factor", {
  state: () => ({
    usuarios: [], 
    selecteduser: {
      
      qrImage: null,
      codenumber: null,
      username: null,
      password: null,
      token: null,
    }, // actual grupo seleccionado
    activeStep: 0, 
    activeClass: "js-active position-relative", 
    formSteps: 2,     
  }),
  actions: {
    
   async valiudateCode(code){

if(!code || code.length !== 6) {
  return false; // Invalid code length  
}
          return true;
   },

    // Step-based navigation actions
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

     
    
    reset() {
      this.selecteduser = {
         qrImage: null,
      codenumber: null,
      username: null,
      password: null,
      token: null,
      };     
    
    },
    
     
  },
});