import { apiRequest } from '@/services/apiService'
import { useMainStore } from '@/store/useMainStore'
import { useBitacoraStore } from '../modules/bitacora/store/useBitacoraStore'
 

export async function getLogs() {
  const store = useBitacoraStore();
  const mainStore = useMainStore();

     let startDate =store.bitacora.StartDate;
      let EndDate =store.bitacora.EndDate;
 
 
    store.bitacora.StartDate = store.bitacora.StartDate.split('/')
      .reverse()
      .join('-')  + "T00:00:00Z"
    store.bitacora.EndDate = store.bitacora.EndDate.split('/')
      .reverse()
      .join('-') + "T23:59:59Z"

     

      const result = await apiRequest({

      url: `/AuditLog/filtered`,
      method: "POST",
      data: JSON.stringify({
        StartDate: store.bitacora.StartDate,
        EndDate: store.bitacora.EndDate,
        UserIds: store.bitacora.UserIds
          ? store.bitacora.UserIds.map((user) => user.id)
          : [],
        PageNumber: 1,
        PageSize: 20,
        SortDirection: 'desc',
        SortBy: 'Timestamp',
      }),
       
      });

       

      if (!Array.isArray(result.data.data) || result.data.data.length === 0) {
      mainStore.triggerAlert({
      message: "No se encontraron registros.",
      color: "warning",
      icon: "warning",
      });
      store.loadingProgress = 100;
      return [];
      }

     store.bitacora.StartDate = startDate;
     store.bitacora.EndDate = EndDate;

      return result.data.data;


  
}

export async function getLogsType() 
{
  const store = useBitacoraStore();
  const mainStore = useMainStore();
  
      let startDate =store.bitacoraError.StartDate;
      let EndDate =store.bitacoraError.EndDate;

      store.bitacoraError.StartDate = store.bitacoraError.StartDate.split('/')
      .reverse()
      .join('-') + "T00:00:00Z"
      store.bitacoraError.EndDate = store.bitacoraError.EndDate.split('/')
      .reverse()
      .join('-') + "T23:59:59Z"

      const result = await apiRequest({

      url: `/Logs/filtered`,
      method: "POST",
      data: JSON.stringify({
      StartDate: store.bitacoraError.StartDate,
      EndDate: store.bitacoraError.EndDate,
      PageNumber: 1,
      PageSize: 20,
      SortDirection: 'desc',
      SortBy: '',
      }),       
      });

       

       if (!Array.isArray(result.data.data) || result.data.data.length === 0) {
      mainStore.triggerAlert({
      message: "No se encontraron registros.",
      color: "warning",
      icon: "warning",
      });
      store.loadingProgress = 100;
      return [];
      }

   
       store.bitacoraError.StartDate = startDate;
       store.bitacoraError.EndDate = EndDate;

      return result.data.data;


    
}

export async function createLog(logData) {
 
  const result = await apiRequest({

  url: `/bitacora`,
  method: "POST",
  data: logData,
  showSuccess: true,
  successMessage: "Registro guardado exitosamente."
  });



  return result.success;  
}

export async function updateLog(logData) {
 
  const result = await apiRequest({

  url: `/bitacora/${logData.id}`,
  method: "PUT",
  data: JSON.stringify(logData),
  showSuccess: true,
  successMessage: "Registro guardado exitosamente."
  });

  return result.success;
 
}
