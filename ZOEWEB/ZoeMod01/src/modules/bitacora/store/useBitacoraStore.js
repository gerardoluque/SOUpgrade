import { defineStore } from 'pinia'
import { getLogs, getLogsType } from '@mod1/services/bitacoraService.js'
import { getUsers } from '@mod1/services/userService.js'

export const useBitacoraStore = defineStore('bitacora', {
  state: () => ({
    bitacora: {
      UserIds: [],
      StartDate: new Date().toISOString().slice(0, 10),
      EndDate: new Date().toISOString().slice(0, 10),
    },
    bitacoraError: {
      UserIds: [],
      StartDate: new Date().toISOString().slice(0, 10),
      EndDate: new Date().toISOString().slice(0, 10),
    },
    columns: ['Usuario', 'Tipo de evento', 'Fecha y hora'], // Table columns
    rowsBitacora: [],
    users: [],
    usuarios: [],
    bitacoraColumnsError: [
      'Fecha y hora',
      'Tipo de evento',
      'Descripción',
      'Usuario',
    ],
    rowsBitacoraError: [],
  }),
  getters: {
    get availableUsers() {
      if (!this.usuarios) {
        return this.users
      }
      return this.users.filter(
        (user) =>
          !this.usuarios.some((selectedUser) => selectedUser.id === user.id)
      )
    },
  },
  actions: {
    async fetchBitacora() {
      try {
        const bitacora = await getLogs();
      
        this.rowsBitacora = bitacora.map((log) => ({
          'Usuario': log.userName,
          'Tipo de evento': log.eventType,
          'Fecha y hora': new Date(log.timestamp).toLocaleString(),
        }))   

     

      } catch (error) {
        
        this.loadingProgress = 0
      }
    },
    async fetchBitacoraError() {
      try {
        const bitacoraError = await getLogsType()
  
          this.rowsBitacoraError = bitacoraError.map((log) => ({
          'Fecha y hora': new Date(log.timestamp).toLocaleString(),
          'Tipo de evento': log.level,
          'Descripción': log.message,
          'Usuario': log.userName,
        }))   

      } catch (error) {
        console.log(`Error al obtener la bitacora: ${error}`)
      }
    },
    async fetchUsers() {
      try {
        const fetchedUsers = await getUsers()

        this.users = fetchedUsers.map((user) => ({
          id: user.id,
          name: user.displayName || '',
        }))
      } catch (error) {
        console.error('Error fetching users:', error)
      }
    },
    cleanBitacora() {
    this.bitacora = {
      UserIds: [],
      StartDate: new Date().toISOString().slice(0, 10),
      EndDate: new Date().toISOString().slice(0, 10),
    }
    },
    cleanBitacoraError() {
      this.bitacoraError = {
        UserIds: [],
        StartDate: new Date().toISOString().slice(0, 10),
        EndDate: new Date().toISOString().slice(0, 10),
      }
    },
  },
})
