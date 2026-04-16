import { defineStore } from "pinia";
import { getUsers, setUser, updateUser,getUserById } from "@mod1/services/userService.js";
import { getAllRol,getRolById } from "@mod1/services/rolService.js";
import { getCorporation } from "@mod1/services/corporationService.js";
import { getProcess } from "@mod1/services/processService.js";
import { getGroups } from "@mod1/services/groupService.js";
import localStorageService from "@/utils/localStorageService";

 
export const useUsuarioStore = defineStore("usuario", {
  state: () => ({
    usuarios: [], 
    selecteduser: {
      id: null,
      nombreCompleto: null,
      nombre:null,
      primerApellido: null,
      segundoApellido: null,
      telefono: '',
      email: null,
      tiempoInactividad: 30,
      requiere2FA: false,
      corporaciones: [],
      rolId: null,
      grupoId: null,
      fechaCreacion: null,
      fechaUltimaActualizacion: null,
      permisos: [],     
      userName: null,      
      activo: true,
      usuarioPermisos: [],
      corporacionesSeleccinados: [],
      esUsuarioAD: false, // Indica si el usuario es de AD,
      password: null, // Contraseña del usuario, no se asigna valor por defecto
      
    }, // actual grupo seleccionado
    activeStep: 0, 
    activeClass: "js-active position-relative", 
    formSteps: 2, 
    columns: ["usuario",  "nombre", "apellidos", "celular", "correo","inactvidad","fechaCreacion","fechaActualizacion","activo"], // columnas de la tabla
    rows: [ ], // datos de la tabla
    rowUsers: [],
    roles: [], // datos de la tabla
    process:[], // datos de la tabla
    corporaciones: localStorageService.get("corporaciones") || [], // datos de la tabla
    groups: [], // datos de la tabla
    permisos: [], // datos de la tabla
    rolSelected: null, // rol seleccionado
    rolIdSelected: 0, // id del rol seleccionado
    userUpdated: false,  
  }),
  actions: {
    validUpdate(valid) {
      this.userUpdated = valid;
    },

    loadPermisos() {
      
      
      if(this.selecteduser.usuarioPermisos != null){
         this.selecteduser.permisos = this.selecteduser.usuarioPermisos.map((permiso) => ({
        UsuarioId: this.selecteduser.id ? this.selecteduser.id : "",
        RolId: this.rolIdSelected,
        ProcesoId:  this.process.find((x) => x.descr === permiso.Proceso).id.toString(),
        Acceso : JSON.stringify(permiso.Permiso),
      }));

      }
      
    },
    
     setRolId(rolId) {
      this.rolIdSelected = rolId;
    },

    async getUserById(id) {
      try {
        
        
        const response = await getUserById(id);        
        await this.getAllCorporations(); // Asegurarse de que las corporaciones están cargadas antes de asignar
         
        this.selecteduser = {
          id: response.id,
          nombreCompleto: response.nombreCompleto,
          nombre: response.nombre,
          primerApellido: response.primerApellido,
          segundoApellido: response.segundoApellido,
          telefono: response.telefono,
          email: response.email,
          tiempoInactividad: response.tiempoInactividad,
          corporaciones: response.corporaciones,
          rolId: response.rolId,
          grupoId: response.grupoId,
          fechaCreacion: response.fechaCreacion,
          fechaUltimaActualizacion: response.fechaUltimaActualizacion,
          permisos: response.permisos,
          userName: response.userName,
          password: response.password, // No se asigna valor por defecto
          activo: response.activo, // No se asigna valor por defecto,
          requiere2FA: response.requiere2FA !== undefined ? response.requiere2FA : false,
          usuarioPermisos: null,
          corporacionesSeleccinados: this.corporaciones.filter(c =>  response.corporaciones.includes(c.id)).map(c => c.name), // Filtra las corporaciones seleccionadas

        };

        
 
      } catch (error) {
        console.error(error);
      }
    },

    async getAllGroups() {
      try {
        const response = await getGroups();
        
        
        this.groups = response.filter(x => x.activo == true).map((row) => ({
          id: row.id,
          nombre: row.nombre,
          descr: row.descr,        
          activo: row.activo,        
          usuarios: row.usuarios,        
          fechaActualizacion: row.fechaActualizacion,        
          createdDateTime: row.createdDateTime,        
          
        }));


      } catch (error) {
        console.error(error);
      }
    },
    async getAllCorporations() {
      try {
        const response = await getCorporation();
        

        this.corporaciones =  Array.isArray(response) ? response.map((row) => ({
          id: row.id,
          name: row.nombre,
          descripcion: row.descripcion,        
          
        })) : [];

       

      } catch (error) {
        console.error(error);
      }
    },
    async getRolByUserSelected(id) {
      try {
        const response = await getRolById(id);  
        return response;
      } catch (error) {
        console.error(error);
      }
    },
    async getProcessByRol() {
      try {
     
        if(!this.rolIdSelected) {
          this.process = [];
          return;
        }

        const response = await getRolById(this.rolIdSelected);

        // Ensure we have an array of procesos; protect against undefined responses
        const procesos = Array.isArray(response?.procesos) ? response.procesos : [];
        console.log("Procesos fetched for rol:", procesos);
        // Skip procesos that have null/undefined 'acciones'
        const procesosConAcciones = procesos.filter((p) => p && p.acciones != null);

        this.process = procesosConAcciones.map((row) => ({
          id: row.id,
          descr: row.descr,
          tipo: row.tipo,
          icono: row.icono,
          activo: row.activo,
          ruta: row.ruta,
          subprocesos: row.subprocesos,
        }));

        

          if(this.selecteduser.id != null && this.selecteduser.id != 0)
         {       
           if(this.selecteduser.usuarioPermisos == null || this.selecteduser.usuarioPermisos.length == 0)
           {

                this.selecteduser.usuarioPermisos =   (this.selecteduser.permisos || []).map((x) => {
                const procesoRaw = procesos.find((y) => y && String(y.id) == String(x.procesoId)) || null;
                const proceso = procesoRaw && procesoRaw.acciones != null ? procesoRaw : null;
                let permisoObj = {};
                if (x && x.acceso) {
                  try { permisoObj = JSON.parse(x.acceso); } catch (e) { permisoObj = {}; }
                }
                return {
                  Proceso: proceso ? proceso.descr : "Proceso no encontrado",
                  Permiso: permisoObj,
                };
                })

            }            
         }
        
        

       
      } catch (error) {
        console.error(error);
      }
    },
    async getAllProcess() {
      try {
        const response = await getProcess();
     

        this.process = response.map((row) => ({
          id: row.id,
          descr: row.descr,
          tipo: row.tipo,
          icono: row.icono,
          activo: row.activo,
          ruta: row.ruta,
          subprocesos: row.subprocesos,
          
        }));


      } catch (error) {
        console.error(error);
      }
    },
    async getAllRol() {
      try {
        
        const response = await getAllRol();
        

        this.roles = response.filter(x => x.activo == true).map((rol) => ({
          id: rol.id,
          name: rol.name,
          descripcion: rol.descripcion,
          value: rol.value,
          activo: rol.activo,
          assignedUsers: rol.assignedUsers,
          assignedGroups: rol.assignedGroups,
          customAttributes: rol.customAttributes,
          procesos: rol.procesos,
          
          
        }));


      } catch (error) {
        console.error("Error creating user:", error);
      }
    },

    async setUser() {
      try {
        
       return await setUser(this.selecteduser);
       
      } catch (error) {
        console.log("Error creating user:", error);
      }
    },
    async updateUser() {
      try {
       
         return await updateUser(this.selecteduser);
        
      } catch (error) {
        console.error("Error updating user:", error);
      }
    },  
    
   
    async fetchUsers() {
      try {
    
        const users = await getUsers();     

        this.rowUsers = users.map((user) => ({
         activo: user.activo ? user.activo : false,
          usuario: user.userName,
          id: user.id,
          nombreusuario: user.userName,
          nombre: user.nombreCompleto,
          apellidos: user.primerApellido ?  user.primerApellido :  "" +" "+ user.segundoApellido,
          celular: user.telefono,
          correo: user.email,
          inactvidad: user.tiempoInactividad,                  
          fechaCreacion: new Date( user.fechaCreacion).toLocaleDateString(),      
          fechaActualizacion: new Date( user.fechaUltimaActualizacion).toLocaleDateString(),
 
        }));

       
      } catch (error) {
        console.error("Error fetching users:", error);
        this.loadingProgress = 0; // Reset progress on error
      }
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

     
    
    resetSelected() {
      this.selecteduser = {
        id: null,
      nombreCompleto: null,
      nombre:null,
      primerApellido: null,
      segundoApellido: null,
      telefono: '',
      email: null,
      tiempoInactividad: 30,
      corporaciones: [],
      requiere2FA: false,
      rolId: null,
      grupoId: null,
      fechaCreacion: null,
      fechaUltimaActualizacion: null,
      permisos: [],     
      userName: null,      
      activo: true,
      usuarioPermisos: [],
      corporacionesSeleccinados: [],
      esUsuarioAD: false, // Indica si el usuario es de AD,
      password: null, // Contraseña del usuario, no se asigna valor por defecto       
      };
      this.permisos = [];      
      this.rolSelected = null; // rol seleccionado
      this.rolIdSelected = 0; // id del rol seleccionado
      this.userUpdated = false; // id del rol seleccionado
    
    },
    
    updateSelected(field, value) {
      if (Object.prototype.hasOwnProperty.call(this.selectedusuario, field)) {
        this.selectedusuario[field] = value;
      }
    },
  },
});