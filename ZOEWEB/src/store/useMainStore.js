import { defineStore } from "pinia";
import { msalInstance } from "@/authConfig"; // Importa la instancia de MSAL
import { getUser } from "@mod1/services/userService.js";
import { loginUser, logoutUser, getUserInfo } from "@/services/azureService";
import { getRolById } from '@mod1/services/rolService.js'
import Swal from 'sweetalert2'
import router from "@/router";
import { getCorporation } from "@mod1/services/corporationService.js";
import localStorageService from "@/utils/localStorageService";
import { enable2Fa, login,verify2f } from "../services/authService";
// Helper to decode JWT payload (no external deps)
function parseJwt(token) {
  try {
    const parts = token.split('.');
    if (parts.length < 2) return null;
    const payload = parts[1].replace(/-/g, '+').replace(/_/g, '/');
    const padded = payload.padEnd(payload.length + (4 - (payload.length % 4)) % 4, '=');
    const json = atob(padded);
    return JSON.parse(json);
  } catch (e) {
    return null;
  }
}
 
export const useMainStore = defineStore("main", {
  state: () => ({
    isSidenavFixed: false, // Tracks whether the sidenav is fixed
    isDarkMode: false, // Tracks dark mode state
    sidebarType: "bg-gradient-dark", // Default sidebar type
    color: "primary", //
    userInfo: localStorageService.get("userInfo") || [],
    userdata: localStorageService.get("userdata") || [],
    allProcess: [],
    userRol: localStorageService.get("userRol") || null,
    currentRoute: null,
    navItems: localStorageService.get("navItems") || [],
    hideConfigButton: false,
    isPinned: true,
    showConfig: false,
    isRTL: false,
    isNavFixed: false,
    isAbsolute: false,
    showNavs: true,
    showSidenav: true,
    showNavbar: true,
    showFooter: true,
    showMain: true,
    navbarFixed:
      "position-sticky blur shadow-blur left-auto top-1 z-index-sticky px-0 mx-4",
    absolute: "position-absolute px-4 mx-0 w-100 z-index-2",
    alert: {
      message: "",
      color: "danger",
      icon: "error",
      visible: false,
    },

    inactivityTimeout: localStorageService.get("inactivityTimeout") || 30000, // Tiempo de inactividad en milisegundos (30 seg pba)
    inactivityTimer: null, // Referencia al temporizador de inactividad
    isAuthenticated: localStorageService.get("isAuthenticated") || false,
    coporacionSelected: localStorageService.get("coporacionSelected") || '',
    coporacionSelectedId: localStorageService.get("coporacionSelectedId") || '',
    clinicaSelected: localStorageService.get('clinicaSelected') || '',
    clinicaSelectedId: localStorageService.get('clinicaSelectedId') || '',
    farmaciaSelected: localStorageService.get('farmaciaSelected') || '',
    farmaciaSelectedId: localStorageService.get('farmaciaSelectedId') || '',
    usercorporations: localStorageService.get("usercorporations") || [],
    corporaciones: localStorageService.get("corporaciones") || [],
    userPermisos: localStorageService.get("userPermisos") || [],
    externalUser: localStorageService.get("externalUser") || {

      qrImage: null,
      codenumber: null, 
      username: '',
      password: '',
      token: null,
      isAppAuthenticated: true,
      is2FaEnabled: false,
    },
    isSystemUser: localStorageService.get("isSystemUser") || false
    
  }),
  actions: {

    triggerAlert({ message, color = "danger", icon = "error" }) {
      this.alert = {
        message,
        color,
        icon,
        visible: true,
      };

      setTimeout(() => {
        this.clearAlert();
      }, 5000);
    },

    clearAlert() {
      this.alert = {
        message: "",
        color: "danger",
        icon: "error",
        visible: false,
      };
    },

    toggleSidenavMini() {
      this.isSidenavMini = !this.isSidenavMini;
    },

    toggleSidenavFixed() {
      this.isSidenavFixed = !this.isSidenavFixed;
    },
    toggleConfigurator() {
      this.showConfig = !this.showConfig;
    },
    toggleEveryDisplay() {
      this.showNavbar = !this.showNavbar;
      this.showSidenav = !this.showSidenav;
      this.showFooter = !this.showFooter;
    },
    toggleHideConfig() {
      this.hideConfigButton = !this.hideConfigButton;
    },
    setIsPinned(payload) {
      this.isPinned = payload;
    },
    setColor(payload) {
      this.color = payload;
    },
    navbarMinimize() {
      const sidenavShow = document.getElementsByClassName("g-sidenav-show")[0];

      if (sidenavShow.classList.contains("g-sidenav-pinned")) {
        sidenavShow.classList.remove("g-sidenav-pinned");
        sidenavShow.classList.add("g-sidenav-hidden");
        this.isPinned = true;
      } else {
        sidenavShow.classList.remove("g-sidenav-hidden");
        sidenavShow.classList.add("g-sidenav-pinned");
        this.isPinned = false;
      }
    },
    navbarFixed() {
      this.isNavFixed = !this.isNavFixed;
    },
    setCurrentRoute(route) {
      this.currentRoute = route;
    },

    setAuthenticated(state) {
      this.isAuthenticated = state;
      localStorageService.set("isAuthenticated", this.isAuthenticated);      
    },

    startInactivityTimer() {

      if (!this.isAuthenticated) return;
      // Limpia cualquier temporizador existente
      if (this.inactivityTimer) {
        clearTimeout(this.inactivityTimer);
      }

      // Configura un nuevo temporizador
      this.inactivityTimer = setTimeout(() => {
        this.handleInactivity();
      }, this.inactivityTimeout);
    },
    handleInactivity() {
      if (!this.isAuthenticated) return;
      // Lógica para manejar la inactividad (redirigir al login)
          this.setAuthenticated(false);
        Swal.fire({
          title: "Sesión Expirada",
          text: "Has sido redirigido al login por inactividad.",
          icon: "warning",
          timer: 30000, // Opcional: tiempo en milisegundos antes de que se cierre automáticamente
          timerProgressBar: true, // Muestra una barra de progreso del temporizador
          confirmButtonText: "Aceptar",
          allowOutsideClick: false, // Evita que se cierre al hacer clic fuera
        }).then(() => {
          this.logout(); // Llama a la acción de logout
        });
  

    },
    resetInactivityTimer() {
      if (!this.isAuthenticated) return;
      this.startInactivityTimer(); // Reinicia el temporizador
    },


    async loginUser() {
      try {

        if (!this.isAuthenticated) {
          await loginUser();
           this.setAuthenticated(true);
        }

      }
      catch (error) {
        console.error("Error al iniciar sesión:", error);
      }

    },
    async logout() {
      try {

        this.setAuthenticated(false);
        try {
          
        
          
          localStorageService.remove("navItems");
          localStorageService.remove("coporacionSelected");          
          localStorageService.remove("usercorporations");
          localStorageService.remove("corporaciones");
          localStorageService.remove("userdata");
          localStorageService.remove("userInfo");
       

          try {
            // Detener el temporizador de inactividad
              await logoutUser(); // Llama a la función de cierre de sesión
            }
           catch (error) {
            console.error("Error al detener el temporizador de inactividad:", error); 
           }
          
          if(!this.isSystemUser){

            await msalInstance.logoutRedirect({
            postLogoutRedirectUri: process.env.VUE_APP_MSAL_POST_LOGOUT_REDIRECT_URI, // Cambia según tu entorno
          });          
          
          }
          else
          {
            router.push({ name: "/" }); // Redirige al login
          }
            
            this.isSystemUser = false;

            localStorageService.remove("externalUser");
            localStorageService.remove("isSystemUser");           
            localStorageService.clear();       
            this.userInfo = null;
            this.externalUser = {
            qrImage: null,
            codenumber: null, 
            username: '',
            password: '',
            token: null,
            isAppAuthenticated: true,
            is2FaEnabled: false,
            };
            

          
        } catch (error) {
          console.error("Error al cerrar sesión:", error);
        }





      } catch (error) {
        console.error("Error al cerrar sesión:", error);
      }
    },
    async getRol(id) {


      //this.userRol = localStorageService.get("userRol") || null;
      //if (this.userRol == null) {
        this.userRol = await getRolById(id);
        localStorageService.set("userRol",  this.userRol);

      //}

      this.allProcess = this.userRol.procesos;

    },

    async fetchProcess() {

      const permisosDelRolUsuario = this.userdata.permisos.map((p) => p.procesoId);
 
      const processByRol = this.userRol.procesos.filter((process) =>
        permisosDelRolUsuario.includes(process.id)
        && process.activo == true
      );

      const listadoPadres = processByRol.map((p) => p.procesoPadreId);
      const procesosPadreDelRol = this.userRol.procesos.filter((process) =>
        listadoPadres.includes(process.id)
        && process.activo == true
      );




      const permisoProcesoPorUsuario = [];
       procesosPadreDelRol.forEach(element => {
      
       

        element.subprocesos.forEach(subelement => {
           const permiso = this.userdata.permisos.find(x => x.procesoId == subelement.id);
          permisoProcesoPorUsuario.push(
            {
              procesoid: subelement.id,
              nombreModulo: subelement.descr,
              modulo: subelement.ruta,
              accion: permiso ? JSON.parse(permiso.acceso) : null // Solo parsea si existe
              //accion: 'x'

            })
        });
      });
      this.userPermisos = permisoProcesoPorUsuario;
      localStorageService.set("userPermisos", permisoProcesoPorUsuario);
 
      /*Esto crea la navegacion*/
      if (procesosPadreDelRol.length > 0) {

        
        const nuevosItems = procesosPadreDelRol.map((pb) => ({
          title: pb.descr,
          icon: pb.icono ? pb.icono : "settings",
          sistemaid: pb.sistemaId,
          children: pb.subprocesos.map((sub) =>
          ({
            name: sub.ruta != null ? sub.ruta : "",
            text: sub.descr,
            miniIcon: sub.icono ? sub.icono : "settings",
            sistemaid: pb.sistemaId,
          }))

        }));
        // Validate navItems against router
        const validRoutes = router.getRoutes().map((route) => route.name);
        nuevosItems.forEach((item) => {
          item.children = item.children.filter((child) =>
            validRoutes.includes(child.name)
          );
        });
 

        this.navItems = [];
        localStorageService.remove("navItems");
        this.navItems = [...this.navItems, ...nuevosItems];
      }
      else {

        this.triggerAlert({
          message: "No se encontraron procesos para el usuario inciado.",
          color: "warning",
          icon: "warning",
        });
      }

      localStorageService.set("navItems",  this.navItems);




    },


    async getUserInfo() {
      try {


        //this.corporaciones = localStorageService.get("corporaciones");
        //if (this.corporaciones == null) {
        this.coporacionSelected= null;
        
          this.corporaciones = await getCorporation();
          localStorageService.set("corporaciones", this.corporaciones);
        //}
        
        this.userInfo = localStorageService.get("userInfo");
        if (this.userInfo == null) {
          this.userInfo = await getUserInfo();
           await this.loginUser();
        }


        //this.userdata = localStorageService.get("userdata");
         
        //if (this.userdata == null) {
          
        this.userdata = await getUser();
           
          if (!this.userdata.activo) {
            Swal.fire({
              title: "Usuario Deshabilitado",
              text: "Has sido redirigido al login porque el usuario se encuentra inactivo.",
              icon: "warning",
              confirmButtonText: "Aceptar",
              allowOutsideClick: false, // Evita que se cierre al hacer clic fuera
            }).then(() => {
              this.logout();
            });

            return;

         }

          localStorageService.set("userdata", this.userdata);
          localStorageService.set("usercorporations", this.userdata.corporaciones);
          this.inactivityTimeout = this.userdata.tiempoInactividad ? (this.userdata.tiempoInactividad * 60) * 1000 : 30000;
          localStorageService.set("inactivityTimeout", this.inactivityTimeout);
          await this.getRol(this.userdata.rolId);
        //}
       /* else {
          this.usercorporations = this.userdata.corporaciones;
          localStorageService.set("usercorporations",this.userdata.corporaciones);
          this.inactivityTimeout = this.userdata.tiempoInactividad ? (this.userdata.tiempoInactividad * 60) * 1000 : 30000; // Default to 100 seconds if not set
          localStorageService.set("inactivityTimeout",this.inactivityTimeout);
        }
          */

       
        
        if (this.coporacionSelected == null || this.coporacionSelected == '') {
          if (this.userdata.corporaciones.length > 0) {
            if (this.userdata.corporaciones.length > 1) 
              {

                 let corporacionesSeleccionar = this.corporaciones.filter(c => this.userdata.corporaciones.includes(c.id)).map(c => c.nombre);

              
              // Muestra un SweetAlert para seleccionar una corporación 
              const { value: corporacion } = await Swal.fire({
                title: "Selecciona una corporacion",
                input: "select",
                inputOptions: corporacionesSeleccionar,
                inputPlaceholder: "Selecciona la corporacion!",
                showCancelButton: false,
                allowOutsideClick: false, // Evita que se cierre al hacer clic fuera
                inputValidator: (value) => {
                  return new Promise((resolve) => {
                    const intValue = parseInt(value, 10);

                    if (!value || isNaN(intValue)) {
                      resolve("Necesitas seleccionar una corporación válida.");
                    } else {

                      resolve();
                    }
                  });
                }

              });

              this.coporacionSelected = this.corporaciones.find(c => c.id === this.userdata.corporaciones[corporacion]).nombre;
              this.coporacionSelectedId = this.corporaciones.find(c => c.id === this.userdata.corporaciones[corporacion]).id;
            }
            else {
              this.coporacionSelected = this.corporaciones.find(c => c.id === this.userdata.corporaciones[0]).nombre;
              this.coporacionSelectedId = this.corporaciones.find(c => c.id === this.userdata.corporaciones[0]).id;
            
            }
   
            localStorageService.set("coporacionSelected", this.coporacionSelected);            
            localStorageService.set("coporacionSelectedId", this.coporacionSelectedId);                                    
          }

        }
        this.fetchProcess();

      } catch (error) {
        console.log("Error al obtener la información del usuario:", error);
      }
    },

    async login() {

      

      this.setAuthenticated(false);   
   
         const resultLogin = await login(this.externalUser); 
         if (resultLogin.success) {
             
          if(resultLogin.data.is2FaEnabled)
          {
            this.externalUser.is2FaEnabled = true;
            this.externalUser.isAppAuthenticated = true;  
             this.setAuthenticated(false);   
          }
          else
          {
              this.isSystemUser = true;
              // store token and refreshToken when provided by API
              this.externalUser.token = resultLogin.data.token;
            
              if (resultLogin.data.refreshToken) {
                this.externalUser.refreshToken = resultLogin.data.refreshToken;
              }
              // decode token to get expiration (exp) and store as milliseconds
              try {
                const payload = parseJwt(resultLogin.data.token);
                if (payload && payload.exp) {
                  this.externalUser.tokenExpires = payload.exp * 1000;
                }
              } catch (e) {
                // ignore
              }
              localStorageService.set("externalUser", this.externalUser);
              localStorageService.set("isSystemUser", this.isSystemUser);   
              this.setAuthenticated(true);
          }     
          
        }
        else
        {

          if(!resultLogin.error.includes('400')){

                this.triggerAlert({
                message: "No se encontro el Usuario o Password",
                color: "warning",
                icon: "error",

                });
          }

       
      }
        

        return resultLogin;
      
    },
    async enable2Fa() {     
     
       const resultLogin =  enable2Fa(this.externalUser); 
        if (resultLogin.success) {
          
          this.externalUser.is2FaEnabled = true;
          this.externalUser.isAppAuthenticated = false
          this.externalUser.qrImage = resultLogin.data.qrCodeImage;
          localStorageService.set("externalUser", this.externalUser);       
          
        }
        return resultLogin;
      
    },
     async verify2Fa() {     
     
       const resultLogin = await verify2f(this.externalUser); 
        if (resultLogin.success) {
          
          this.externalUser.is2FaEnabled = true;
          this.externalUser.isAppAuthenticated = false
          this.externalUser.token = resultLogin.data.token;
          this.isSystemUser = true; 
             
          // decode token expiration and persist
          try {
            const payload = parseJwt(resultLogin.data.token);
            if (payload && payload.exp) {
              this.externalUser.tokenExpires = payload.exp * 1000;
            }
          } catch (e) {
            // ignore
          }
          localStorageService.set("externalUser", this.externalUser);
          localStorageService.set("isSystemUser", this.isSystemUser);      
          this.setAuthenticated(true);
        
        }
        return resultLogin;
      
    },
  },

});




