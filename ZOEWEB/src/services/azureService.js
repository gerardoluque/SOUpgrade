
 import { getApiHeaders } from "@/services/apiHeaders";
 import { handleApiError } from "@/services/errorHandler";
 import localStorageService from "@/utils/localStorageService";

const API_BASE_URL = process.env.VUE_APP_API_AZURE_URL;
const API_BASE_URL_SYSTEM = process.env.VUE_APP_API_BASE_URL;


 
export async function loginUser() {

  return handleApiError(async () => {
   const headers = await getApiHeaders();

    const cachedUserInfo = localStorageService.get("userInfo");

    const userid = cachedUserInfo.claims.find((claim) => claim.type === "http://schemas.microsoft.com/identity/claims/objectidentifier");
    const username = cachedUserInfo.name;

    const userData = {
      username: username,
      userid: userid.value,
        };

    let URL = `${API_BASE_URL}/login`;
    if(localStorageService.get("isSystemUser")){
    URL = `${API_BASE_URL_SYSTEM}/auth/login`;
    }

  
    const response = await fetch(URL, {
     headers,   
      body: JSON.stringify(userData),   
      method: "POST",
    });

    if (!response.ok) {
      throw new Error(`Error al cargar el login: ${response.statusText}`);     
    }
    
    
 
     return await response.ok;
  });
 
}


export async function logoutUser() {

  return handleApiError(async () => {
    const headers = await getApiHeaders(); 

 let URL = `${API_BASE_URL}/logout`;
  if(localStorageService.get("isSystemUser")){
    URL = `${API_BASE_URL_SYSTEM}/auth/logout`;
  }

  
    const response = await fetch(URL, {
     headers,      
      method: "POST",
    });

     

    if (!response.ok) {
      throw new Error(`Error al cargar el login: ${response.statusText}`);     
    }
      return await response.ok;
     
  });
 
}


/**
 * Obtiene la información del usuario desde el backend.
 */
export async function getUserInfo() {
 
  const headers = await getApiHeaders();
 let URL = `${API_BASE_URL}/user-info`;
  if(localStorageService.get("isSystemUser")){
    URL = `${API_BASE_URL_SYSTEM}/auth/user-info`;
  }
  
  
    const response = await fetch(URL, {
   headers,
  })

  if (!response.ok) {
    throw new Error(
      `Error al obtener la información del usuario info: ${response.statusText}`
    )
  }


  const userinfo = await response.json();
  
   localStorageService.set("userInfo", userinfo);

  return userinfo;
}