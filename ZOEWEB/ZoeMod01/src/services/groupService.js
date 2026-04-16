import { apiRequest } from '@/services/apiService' 
import { useGruposStore } from '../modules/grupos/store/useGruposStore'
import { useMainStore } from '@/store/useMainStore'

  

export async function getGroups() {
  const store = useGruposStore();
  const mainStore = useMainStore();
  
  store.loadingProgress = 0
  const result = await apiRequest({ url: `/Grupos` });
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

export async function fetchGrupoByID() {  

    const store = useGruposStore();
    store.loadingProgress = 0
    store.loadingProgress = 20
    const result = await apiRequest({ url: `/grupos/${store.grupo.id}` });    
    return result.data; 
}

export async function createGroup() {
 
  const store = useGruposStore();
  const usuarios = store.grupo.usuarios.map((member) => member.id)
  store.grupo.id = 0;
  const newGrupo = {
  ...store.grupo,
  usuarios,
  descr: store.grupo.descr,         
  }

  

  const result = await apiRequest({

  url: `/grupos`,
  method: "POST",
  data: JSON.stringify(newGrupo),
  showSuccess: true,
  successMessage: "Registro guardado exitosamente."
  });

  return result.success;

}

export async function setGroup(groupData) {
  const result = await apiRequest({

  url: `/grupos`,
  method: "POST",
  data: JSON.stringify(groupData),
  showSuccess: true,
  successMessage: "Registro guardado exitosamente."
  });
  
  return result.success;
}

export async function updateGroup() {
    const store = useGruposStore();    
    const usuarios = store.grupo.usuarios.map((member) => member.id)
    

    const upG = {
    ...store.grupo,
    usuarios,
    descr: store.grupo.descr,
    }


    

    const result = await apiRequest({

    url: `/grupos/${upG.id}`,
    method: "PUT",
    data: JSON.stringify(upG),
    showSuccess: true,
    successMessage: "Registro guardado exitosamente."
    });



    return result.success;


}
