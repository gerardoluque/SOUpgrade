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



const routeGrupos = [
  {
    path: "/grupos",
    name: "GruposList",
    component: () => import("@mod1/modules/grupos/views/GruposList.vue"),
    meta : {breadcrum: "Grupos" },
     beforeEnter: requireAuth,

  },
  {
    path: "/grupos/create",
    name: "GruposCreate",
    component: () => import("@mod1/modules/grupos/views/GruposCreate.vue"),
    meta : {breadcrum: "Crear Grupos" },
    beforeEnter: requireAuth,
  },
  {
    path: "/grupos/edit/:id",
    name: "GruposEdit",
    component: () => import("@mod1/modules/grupos/views/GruposEdit.vue"),
    props: true,
    beforeEnter: requireAuth,
  },
];



export default routeGrupos;