import { defineStore } from 'pinia'

import {
  getProcess,
  setProcess,
  updateProcess,
  deleteProcess,
} from '@mod1/services/processService.js'
 
export const useProcesoStore = defineStore('proceso', {
  state: () => ({
    proceso: [],  
    selectedProceso: {
      id: null,
      descr: '',
      tipo: '',
      activo: '',
      ruta: '',
      sistema: 0,
      procesoPadreId: null,
      subprocesos: [], 
      acciones: []     
    },   
    activeStep: 0,
    activeClass: 'js-active position-relative',
    formSteps: 2,
    columns: [
      'id',
      'nombre',
      'Sistema',
      'tipo',
      'icono',     
      'ruta',    
      'activo',  
    ], // Table columns
    rowsProceso: [],
    listaProceso:[]
  }),
  
  

  actions: {
    async fetchProceso() {
      try {
       
        this.proceso = await getProcess()
        
        this.listaProceso = this.proceso;
      
        this.listaProceso = this.proceso.map((val) => ({

          id: val.id,
          name: val.descr,
          tipo: val.tipo == "A" ? 'AGRUPADOR' : 'PROCESO',
          icono: val.icono,
          activo: val.activo,
          ruta: val.ruta,      
          procesoPadreId: val.procesoPadreId,    


        }));

        this.rowsProceso = this.proceso.map((val) => ({
          id : val.id,
          nombre: val.descr,
          'Sistema': '', // placeholder until backend provides it
          tipo: val.tipo == "A" ? 'AGRUPADOR' : 'PROCESO',
          icono: val.icono,
          activo: val.activo,
          ruta: val.ruta,         
          procesoPadreId: val.procesoPadreId,   
        }));

     

      } catch (error) {
        console.error('Error fetching:', error)
        this.loadingProgress = 0 // Reset progress on error
      }
    },
    async createProcess() {
      try 
      {
       
     
        
 const procesoCreate = {

          id: this.selectedProceso.id,
          descr: this.selectedProceso.nombre,          
          tipo: this.selectedProceso.tipo  == 'AGRUPADOR' ? 'A' : 'P',
          activo: this.selectedProceso.activo,
          ruta: this.selectedProceso.ruta != null ? this.selectedProceso.ruta : "" ,
          procesoPadreId: this.selectedProceso.procesoPadreId,
          icono: this.selectedProceso.icono  != null ? this.selectedProceso.icono : "",
          SubProcesoIds: this.selectedProceso.tipo == "P" ? [] : (this.selectedProceso.subprocesos.length > 0 ? this.selectedProceso.subprocesos.map(x => x.id) : [])  ,      
          

        };

      
        await setProcess(procesoCreate)
        this.fetchProceso();
      } catch (error) {
        console.error('Error creating:', error)
      }
    },
    async updateProcess() {
      try {        

        const procesoToUpdate = {

          id: this.selectedProceso.id,
          descr: this.selectedProceso.nombre,
          tipo: this.selectedProceso.tipo  == 'AGRUPADOR' ? 'A' : 'P',
          activo: this.selectedProceso.activo,
          ruta: this.selectedProceso.ruta != null ? this.selectedProceso.ruta : "" ,
          procesoPadreId: this.selectedProceso.procesoPadreId,
          icono: this.selectedProceso.icono  != null ? this.selectedProceso.icono : "",
          SubProcesoIds: this.selectedProceso.tipo == "P" ? [] : (this.selectedProceso.subprocesos.length > 0 ? this.selectedProceso.subprocesos.map(x => x.id) : [])  ,      
          

        };

       
         await updateProcess(procesoToUpdate)

         this.fetchProceso();
       
      } catch (error) {
        console.error('Error updating:', error)
      }
    },
    async deleteProcess(id) {
      try {
        await deleteProcess(id)
        this.proceso = this.proceso.filter((g) => g.id !== id)
      } catch (error) {
        console.error('Error deleting:', error)
      }
    },

    reset() {
      

      this.proceso = {
        id: null,
        descr: '',
        tipo: '',
        activo: '',
        ruta: '',
        procesoPadreId: null,
        subprocesos: [],    
        acciones: []     
      }
      
      this.selectedProceso = {
        id: null,
        descr: '',
        tipo: '',
        activo: '',
        ruta: '',
        procesoPadreId: null,
        subprocesos: [],  
        acciones: []         
      }

        this.fetchProceso();
    },
    // Step-based navigation actions
    nextStep() {
      if (this.activeStep < this.formSteps) {
        this.activeStep += 1
      }
    },
    prevStep() {
      if (this.activeStep > 0) {
        this.activeStep -= 1
      }
    },
    setActiveStep(step) {
      if (step >= 0 && step <= this.formSteps) {
        this.activeStep = step
      }
    },
    // Action to reset the selectedProcess
    resetSelected() {
      this.selectedProceso = {
        id: null,
        descr: '',
        tipo: '',
        activo: '',
        ruta: '',
        procesoPadreId: null,
        subprocesos: [],      
      }
    },
    // Action to update the selectedProcess
    updateSelected(field, value) {
      if (Object.prototype.hasOwnProperty.call(this.selectedProceso, field)) {
        this.selectedProceso[field] = value
      }
    },
  },
})
