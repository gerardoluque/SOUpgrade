 import { apiRequest } from '@/services/apiService'
 

export async function login(userlogin) {

   const userData = {
    Username: userlogin.username,
    Password: userlogin.password    
  };
  
     const result = await apiRequest({

    url: `/auth/login`,
    method: "POST",
    data: userData,
    showSuccess: false,
    successMessage: "Usuario autenticado correctamente.",
    anonimus    : true,
    customAlert : false,
  });  
  
 

  return result;

}


export async function enable2Fa(userlogin) {

   const userData = {
    Username: userlogin.username     
  };
  
    const result = await apiRequest({

    url: `/auth/enable-2fa`,
    method: "POST",
    data: userData,
    showSuccess: false,
    successMessage: "Usuario autenticado correctamente.",
    anonimus    : true,
    customAlert : true,
  });  
  
  

  return result;

}


export async function verify2f(userlogin) {

   const userData = {
    Username: userlogin.username,
    token: userlogin.codenumber     
  };
  
    const result = await apiRequest({

    url: `/auth/verify-2fa`,
    method: "POST",
    data: userData,
    showSuccess: false,
    successMessage: "Usuario autenticado correctamente.",
    anonimus    : true,
    customAlert : true,
  });  
  
  

  return result;

}