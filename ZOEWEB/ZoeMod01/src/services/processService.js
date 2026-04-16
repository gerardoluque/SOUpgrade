 
import { apiRequest } from '@/services/apiService' 
import { useMainStore } from '@/store/useMainStore'


 

export async function setProcess(processData) {

    const result = await apiRequest({

    url: `/Proceso`,
    method: "POST",
    data: JSON.stringify(processData),
    showSuccess: true,
    successMessage: "Registro guardado exitosamente."
    });

    return result.success;   
 
}

export async function updateProcess(processData) {

    const result = await apiRequest({

    url: `/Proceso/${processData.id}`,
    method: "PUT",
    data: JSON.stringify(processData),
    showSuccess: true,
    successMessage: "Registro guardado exitosamente."
    });

    return result.success; 
 
}


export async function deleteProcess(id) {

  const result = await apiRequest({
    url: `/Proceso/${id}`,
    method: "DELETE",
    showSuccess: true,
    successMessage: "Registro eliminado exitosamente."
  });

  return result.success;
 
}

/**
 * Obtiene la información del usuario desde el backend.
 */
export async function getProcess() {
    const mainStore = useMainStore();

    const result = await apiRequest({ url: `/Proceso` });

    if (!Array.isArray(result.data) || result.data.length === 0) {
    mainStore.triggerAlert({
    message: "No se encontraron registros.",
    color: "warning",
    icon: "warning",
    });

    return [];
    }
    return result.data;    

    
}

