import { defineStore } from 'pinia'

import {
  getGroups,
  fetchGrupoByID,
  createGroup,
  updateGroup,
  deleteGroup,
} from '@mod1/services/groupService.js'
import { getUsers } from '@mod1/services/userService.js' // Import getUserInfo
import { useMainStore } from '@/store/useMainStore.js'

export const useGruposStore = defineStore('grupos', {
  state: () => ({
    grupo: {
      id: '',
      nombre: '',
      activo: false,
      descr: '',
      usuarios: [],
    },

    users: [],
    activeStep: 0,
    activeClass: 'js-active position-relative',
    formSteps: 2,
    columns: [
      'nombre',
      'descripcion',
      'activo',
    //  'usuarios',
      'Fecha Creación',
      
      'Fecha Actualización',
    ], // Table columns
    rowsGrupos: [],
  }),
  getters: {
    get availableUsers() {
      if (!this.grupo || !this.grupo.usuarios) {
        return this.users
      }
      return this.users.filter(
        (user) =>
          !this.grupo.usuarios.some(
            (selectedUser) => selectedUser.id === user.id
          )
      )
    },
  },

  actions: {
    async fetchGrupos() {
      try {
        const grupos = await getGroups()
       
        this.rowsGrupos = grupos.map((grupo) => ({
          id: grupo.id,
          nombre: grupo.nombre,
          descripcion: grupo.descr,
          activo: grupo.activo,
          usuarios: grupo.usuarios
            ? grupo.usuarios.map((user) => user.nombreCompleto).join(', ')
            : 'No users',
          'Fecha Creación': new Date(grupo.createdDateTime).toLocaleDateString(),        
          'Fecha Actualización':  new Date(grupo.fechaActualizacion).toLocaleDateString(), 
        }))
      
      } catch (error) {
        console.error('Error fetching groups:', error)
        this.loadingProgress = 0 // Reset progress on error
      }
    },
    async fetchGrupoByID() {
      try {
        let grupobyId = await fetchGrupoByID()
           
        
        this.grupo = {
          id: grupobyId.id,
          nombre: grupobyId.nombre,
          descr: grupobyId.descr,
          activo: grupobyId.activo,
          usuarios: grupobyId.usuarios.map((usr) => ({
            id: usr.id,
            name: usr.nombreCompleto           
          })),
          'Fecha Creación': new Date(grupobyId.createdDateTime).toLocaleDateString(),        
          'Fecha Actualización':  new Date(grupobyId.fechaActualizacion).toLocaleDateString(), 
        }
      } catch (error) {
        console.error('Error fetching groups:', error)
        useMainStore().triggerAlert({
          message: error || 'Error fetching grupo',
          color: 'danger',
          icon: 'error',
        })
        this.loadingProgress = 0
      }
    },
    async createGrupo() {
      try {
      

        return await createGroup()     

      } catch (error) {
        console.error('Error creating group:', error)
      }
    },
    async updateGrupo() {
      try {      
        return await updateGroup();    
       
      } catch (error) {
        console.error('Error updating group:', error)
      }
    },
    async deleteGrupo(id) {
      try {
        await deleteGroup(id)
        this.grupos = this.grupos.filter((g) => g.id !== id)
      } catch (error) {
        console.error('Error deleting group:', error)
      }
    },

    async fetchUsers() {
      try {
        const fetchedUsers = await getUsers()
       
      
        // Map fetched users to the desired structure
        this.users = fetchedUsers.map((user) => ({
          id: user.id,
          name: user.nombreCompleto || '',
          grupoId: user.grupoId || '',
        }))    
        
       
      } catch (error) {
        console.error('Error fetching users:', error)
      }
    },  
   
   
    resetSelectedGrupo() {    
      this.grupo = {
          id: '',
          nombre: '',
          activo: false,
          descr: '',
          usuarios: [],
      }
    }
  },
})
