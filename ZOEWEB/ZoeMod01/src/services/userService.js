
import { useUsuarioStore } from "../modules/usuarios/store/useUsuarioStore";
import { apiRequest } from '@/services/apiService'
import { useMainStore } from '@/store/useMainStore';
import localStorageService from "@/utils/localStorageService";



/**
 * Obtiene la información completa del usuario desde el backend.
 */
export async function getUserById(id) {

  const result = await apiRequest({ url: `/Users/${id}` });

  return result.data;
}

/**
 * Obtiene la información completa del usuario desde el backend.
 */
export async function getUser() {

  const cachedUserInfo = localStorageService.get("userInfo");
   let userid =  null;
  if(localStorageService.get("isSystemUser")){
      userid = cachedUserInfo.id;
   }
  else
  {
    userid = cachedUserInfo.claims.find((claim) => claim.type === "http://schemas.microsoft.com/identity/claims/objectidentifier").value;
  }

 
  
  const result = await apiRequest({ url: `/Users/${userid}` });
   return result.data;
}

/**
 * Obtiene todos los usuarios desde el backend.
 */
export async function getUsers() {
  const store = useUsuarioStore();
  store.loadingProgress = 20;
  const mainStore = useMainStore();
  const result = await apiRequest({ url: `/Users` });

  if (!Array.isArray(result.data) || result.data.length === 0) {
    mainStore.triggerAlert({
      message: "No se encontraron usuarios.",
      color: "warning",
      icon: "warning",
    });
    store.loadingProgress = 100;
    return [];
  }



  return result.data;


}


export async function setUser(userData) {

  //Default values for userData
  userData.activo = userData.activo ? userData.activo : false;
  
  userData.nombreCompleto = userData.nombre + " " + userData.primerApellido + " " + userData.segundoApellido;
  userData.primerApellido = userData.primerApellido == '' ? null : userData.primerApellido;
  userData.segundoApellido =   userData.segundoApellido == '' ? null : userData.segundoApellido;
  userData.telefono = userData.telefono == '' ? null : userData.telefono;
  userData.tiempoInactividad = userData.tiempoInactividad ? userData.tiempoInactividad : 30;
  const corporaciones = localStorageService.get("corporaciones");

  let corporacionesIds = corporaciones.filter(x => userData.corporacionesSeleccinados.includes(x.nombre)).map(x => x.id);
  userData.corporaciones = corporacionesIds? corporacionesIds : [];

  


  const result = await apiRequest({

    url: `/Users`,
    method: "POST",
    data: userData,
    showSuccess: true,
    successMessage: "Usuario guardado exitosamente."
  });



  return result.success;


}


export async function updateUser(userData) {

  userData.activo = userData.activo ? userData.activo : false;
  const corporaciones = localStorageService.get("corporaciones");
     

  userData.nombreCompleto = userData.nombre + " " + userData.primerApellido + " " + userData.segundoApellido;
  userData.primerApellido = userData.primerApellido == '' ? null : userData.primerApellido;
  userData.segundoApellido =   userData.segundoApellido == '' ? null : userData.segundoApellido;
  userData.telefono = userData.telefono == '' ? null : userData.telefono;
  userData.tiempoInactividad = userData.tiempoInactividad ? userData.tiempoInactividad : 30;

  let corporacionesIds = corporaciones.filter(x => userData.corporacionesSeleccinados.includes(x.nombre)).map(x => x.id);



    userData.corporaciones = corporacionesIds? corporacionesIds : [];

  useUsuarioStore.userUpdated = false;

  const result = await apiRequest({

    url: `/Users/${userData.id}`,
    method: "PUT",
    data: userData,
    showSuccess: true,
    successMessage: "Usuario guardado exitosamente."
  });



  return result.success;
} 