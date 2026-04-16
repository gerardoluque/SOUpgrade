import { apiRequest } from '@/services/apiService'
import { useMainStore } from '@/store/useMainStore';


/**
 * Obtiene todos los catalogos desde el backend.
 */
export async function fetchCatalog(api,catalogName) {
  const store = useMainStore();
  
   
  

  const result = await apiRequest({ url: `/`  + api + `/` + catalogName  ,useCorporacionHeader: true  }); 

  if (!Array.isArray(result.data) || result.data.length === 0) {
      store.triggerAlert({
      message: "No se encontraron registros.",
      color: "warning",
      icon: "warning",
    });
  
    return [];
  }

  

  return result.data;


}