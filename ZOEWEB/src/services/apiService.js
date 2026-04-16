import axios from './axiosInstance'
// import { getApiHeaders } from './apiHeaders';
// import { getApiHeadersWithCorporation } from './apiHeaders';
import { useMainStore } from '@/store/useMainStore';
import localStorageService from "@/utils/localStorageService";


export async function apiRequest({
    url,
    method = 'GET',
    data = null,
    showSuccess = false,
    successMessage = '',
    customAlert = true,
    anonimus = false ,
    nocontentType = false,
    addstrings = false,
  responseType = undefined,
    
}) {

    const mainStore = useMainStore();    
    
  try {

        
       console.log("apiRequest: ",localStorageService.get("sistemaSelectedId"), localStorageService.get("coporacionSelectedId")) ;

        const response = await axios({
        url,
        method,
        data,
        anonimus,
        nocontentType,
        responseType,
        headers: { 
            'CorporacionId': localStorageService.get("coporacionSelectedId"),
            'SistemaId': localStorageService.get("sistemaSelectedId") ?? 1
          , 'Content-Type': nocontentType ? null : 'application/json'
          , 'Accept': addstrings ? '*/*' : null }
        });
       
        

        if (showSuccess && successMessage) {
        mainStore.triggerAlert({
        message: successMessage,
        color: 'success',
        icon: 'check',
        })
        }

  return {
  success: true,
  data: response.data,
  error: null,
  headers: response.headers ?? null,
  }
         
    } catch (error) {

         let message = 'Error desconocido'
        

    if (error.response) {
      // Normaliza posibles blobs de error (cuando responseType: 'blob') a texto/JSON utilizable
      try {
        if (error.response.data instanceof Blob) {
          const text = await error.response.data.text();
          try {
            const parsed = JSON.parse(text);
            // Sobrescribe el data del response para reutilizar la lógica existente
            error.response.data = parsed;
          } catch {
            // No es JSON, usar texto plano
            error.response.data = { message: text };
          }
        }
      } catch (e) {
        // No se pudo leer/parsear el blob de error; continuar con manejo genérico
        console.debug('apiRequest: error leyendo blob de respuesta', e)
      }
      // Error con respuesta del servidor
       if(error.response.status === 500) 
        { 
          message = error.response.data?.message || 'Error interno del servidor';
        }
        else
        {
          
          const { data } = error.response
          if(data != null && !data?.data && error.response.status === 400)
          {
             
            // data puede ser string o un objeto con message/error/detail
            if (typeof data === 'string') {
              message = data;
            } else {
              message = data?.message || data?.error || data?.detail || 'Solicitud inválida';
            }

            if (customAlert) {
            mainStore.triggerAlert({
            message,
            color: 'warning',
            icon: 'error',
            })
            }

            return {
            success: false,
            data: null,
            error: message,
            }
          }
          else
          {
            message =  data?.detail || data?.message || error.response.statusText;
            console.log("error api 2",error);
          }
          

         if (data?.errors) 
          {           
            const validationErrors = Object.entries(data.errors)
            .map(([field, messages]) => `${field}: ${messages.join(', ')}`)
            .join('\n')
            message += `\n${validationErrors}`
        }
       else if (error.message) 
        {
      // Error de red o timeout
        message = error.message
      }
        }
      }
      

    if (customAlert) {
      mainStore.triggerAlert({
        message,
        color: 'danger',
        icon: 'error',
      })
    }

    return {
      success: false,
      data: null,
      error: message,
      headers: error.response?.headers ?? null,
    }
         
         
    }
}
