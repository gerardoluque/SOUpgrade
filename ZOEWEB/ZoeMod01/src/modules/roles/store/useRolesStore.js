import { defineStore } from 'pinia'

import {
  getAllRol,
  setRol,
  fetchRolByID,
  updateRol,
} from '@mod1/services/rolService.js'
import { getProcess } from '@mod1/services/processService.js'


export const useRolesStore = defineStore('roles', {
  state: () => ({
    rol: {
      id: '',
      name: '',
      activo: false,
      descripcion: '',
      tipoRol: 0,
      procesos: [],
    },
    procesos: [],
    activeStep: 0,
    activeClass: 'js-active position-relative',
    formSteps: 2,
    columns: [
      'nombre',
      'Tipo Rol',
      'descripcion',
      'activo',
      // "procesos",
      'Fecha Creación',
      'Fecha Actualización',
    ], // Table columns
    rowsRoles: [], // Table rows],
  }),
  actions: {

    async fetchProcesos() {   
      try {
        
        const procesos = await getProcess()

        let pross = procesos.map((proceso) => ({
          id: proceso.id,
          name: proceso.descr,
          tipo: proceso.tipo,
          activo: proceso.activo,
        }))

        this.procesos = pross.filter((proceso) => proceso.activo === true);
      } catch (error) {
 
        this.loadingProgress = 0
      }
    },
    async fetchRolByID() {   
      try {
        let rolbyId = await fetchRolByID()
        this.rol = {
          ...rolbyId,
          nombre: rolbyId.name,
          descripcion: rolbyId.descripcion,
          activo: rolbyId.activo,
          procesos: rolbyId.procesos.map((proceso) => ({
            id: proceso.id,
            name: proceso.descr,
            tipo: proceso.tipo,
            activo: proceso.activo,
          })),
          'Fecha Creación': rolbyId.fechaCreacion,
          'Fecha Actualización': rolbyId.fechaUltimaActualizacion,
        }
      } catch (error) {
          
        this.loadingProgress = 0
      }
    },
    async fetchRoles() {       
      try {
        const roles = await getAllRol()
        // Resolver etiqueta de tipo rol si viene como entero
        const tipoName = (v) => {
          const map = { 0: 'No definido', 1: 'Médico', 2: 'Elemento', 3: 'Administrativo' };
          return map[Number(v)] ?? '';
        };

        this.rowsRoles = roles.map((rol) => ({
          id: rol.id,
          nombre: rol.name,
          'Tipo Rol': tipoName(rol.tipoRol ?? rol.tiporol ?? rol.TipoRol),
          descripcion: rol.descripcion,
          activo: rol.activo,
          //  procesos: rol.procesos
          //    ? rol.procesos.map((proceso) => proceso.descr).join(', ')
          //    : 'No procesos',
          'Fecha Creación': new Date(rol.fechaCreacion).toLocaleDateString(),
          'Fecha Actualización': new Date(rol.fechaUltimaActualizacion).toLocaleDateString(),
        }))
      } catch (error) {
 
        this.loadingProgress = 0
      }
    },
    async createRol() {
      try {
        const procesos = this.rol.procesos.map((member) => member.id)
     
        const newRol = {
          nombre: this.rol.name,
          descr: this.rol.descripcion,                 
          Definicion: 'system.' + this.rol.descripcion.trim().replace(/\s+/g, ''),  
          Procesos: procesos,
          tipoRol: Number(this.rol.tipoRol) || 0,
        }
        
        return await setRol(newRol)
      } catch (error) {
        console.log(error || 'An error occurred while creating groups.')
      }
    },
    async updateRol() {
      try {
        const procesos = this.rol.procesos.map((member) => member.id)
        
        const updateR = {
          id: this.rol.id,
          nombre: this.rol.name,
          descr: this.rol.descripcion,
          activo: this.rol.activo,
          Procesos: procesos,
          definicion: 'system.' + this.rol.descripcion,
          tipoRol: Number(this.rol.tipoRol) || 0,
           
        }

        return await updateRol(updateR)
      } catch (error) {
        console.log(error || 'An error occurred while Updating groups.')
      }
    },
    
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

    resetSelectedRol() {
      this.rol = {
        id: '',
        activo: false,
        name: '',
        description: '',
        users: 0,
      }
    },
  },
})
