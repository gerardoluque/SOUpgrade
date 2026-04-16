import { msalInstance } from "@/authConfig";
import localStorageService from "@/utils/localStorageService";



const requireAuth = (to, from, next) => {

  const isSystemUser = localStorageService.get("isSystemUser");
  const externalUser = localStorageService.get("externalUser");

if(isSystemUser === true)
  {
     if (externalUser && externalUser.token) {   
      
      next();
    } 
    else
    {
      next({ name: "2FactorQR" }); // Redirige a la página de autenticación de dos factores
    }
    return;
 }
 else
{

    const account = msalInstance.getActiveAccount();
    if (!account) {
    msalInstance.loginRedirect({
    scopes: ["User.Read"], // Cambia los scopes según tus necesidades
    redirectUri: "/pages/users/user-info",
    });
    } else {
    next();
    }


}


};

const routeBitacora = [
    {
    path: "/Bitacora",
    name: "BitacoraList",
    component: () => import("@mod1/modules/bitacora/views/bitacoraList.vue"),
    meta: {
      requiresAuth: true,
      title: "Bitacora de eventos",
      icon: "C",
    },
    beforeEnter: requireAuth,
  },
   {
    path: "/BitacoraError",
    name: "BitacoraErrorList",
    component: () => import("@mod1/modules/bitacora/views/BitacoraErrorList.vue"),
    meta: {
      requiresAuth: true,
      title: "Bitacora de errores",
      icon: "C",
    }
  }
];

export default routeBitacora;