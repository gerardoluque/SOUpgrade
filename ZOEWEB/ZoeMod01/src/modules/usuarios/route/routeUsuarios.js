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


const routeUsuarios = [
  {
    path: "/usuarios",
    name: "UsuarioList",
    component: () => import("@mod1/modules/usuarios/views/UsuarioList.vue"),
      beforeEnter: requireAuth,
  },
  {
    path: "/usuarios/create",
    name: "UsuarioCreate",
    component: () => import("@mod1/modules/usuarios/views/UsuarioCreate.vue"),
      beforeEnter: requireAuth,
  },
  {
    path: "/usuarios/edit/:id",
    name: "UsuarioEdit",
    component: () => import("@mod1/modules/usuarios/views/UsuarioEdit.vue"),
    props: true,
      beforeEnter: requireAuth,
  },
];

export default routeUsuarios;