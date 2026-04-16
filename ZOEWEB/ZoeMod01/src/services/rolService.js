import { apiRequest } from '@/services/apiService'
import { useMainStore } from '@/store/useMainStore';
import { useRolesStore } from "../modules/roles/store/useRolesStore";

  
 
export async function getAllRol() {
  const store = useRolesStore();
  const mainStore = useMainStore();
    

    store.loadingProgress = 0

    const result = await apiRequest({ url: `/rol` });
    store.loadingProgress = 20;

    if (!Array.isArray(result.data) || result.data.length === 0) {
    mainStore.triggerAlert({
    message: "No se encontraron registros.",
    color: "warning",
    icon: "warning",
    });
    store.loadingProgress = 100;
    return [];
    }



    return result.data;
 
}
export async function fetchRolByID() {
    const store = useRolesStore();
    store.loadingProgress = 0
    store.loadingProgress = 20
    const result = await apiRequest({ url: `/rol/${store.rol.id}` });    
    return result.data;

   
}


export async function getRolById(id) {
  
    const result = await apiRequest({ url: `/rol/${id}` });    
    return result.data;
  }


export async function setRol(rolData) {
      

  const result = await apiRequest({

  url: `/Rol`,
  method: "POST",
  data: rolData,
  showSuccess: true,
  successMessage: "Registro guardado exitosamente."
  });



  return result.success;
 
}


export async function updateRol(rolData) {
   

    const result = await apiRequest({

    url: `/Rol/${rolData.id}`,
    method: "PUT",
    data: rolData,
    showSuccess: true,
    successMessage: "Registro guardado exitosamente."
    });

    
    return result.success;
 
  }



export async function deleteRol(rolData) {
 
 return await apiRequest({
    url: `/Rol/${rolData.id}`,
    method: "DELETE",
    showSuccess: true,
    successMessage: "Registro eliminado exitosamente."
  });
}