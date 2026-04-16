import { msalInstance } from "../../../../../src/authConfig";
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



const routeProceso = [
  {
    path: "/proceso",
    name: "ProcesoList",
    component: () => import("@mod1/modules/procesos/views/ProcesoList.vue"),
     beforeEnter: requireAuth,
  },
  {
    path: "/proceso/create",
    name: "ProcesoCreate",
    component: () => import("@mod1/modules/procesos/views/ProcesoCreate.vue"),
     beforeEnter: requireAuth,
  },
  {
    path: "/proceso/edit/:id",
    name: "ProcesoEdit",
    component: () => import("@mod1/modules/procesos/views/ProcesoEdit.vue"),
     beforeEnter: requireAuth,
    props: true,
  },
];

export default routeProceso;