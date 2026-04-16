 
import { apiRequest } from '@/services/apiService' 
  

/**
 * Obtiene la información del Corporacion desde el backend.
 */
export async function getCorporation() {
 

    const result = await apiRequest({ url: `/Corporacion` }); 
    return result.data;       

}

