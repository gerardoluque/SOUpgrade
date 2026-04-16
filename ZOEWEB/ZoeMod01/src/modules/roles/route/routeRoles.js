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

const routeRoles = [
  {
    path: "/roles",
    name: "RolesList",
    component: () => import("@mod1/modules/roles/views/RolesList.vue"),
      beforeEnter: requireAuth,
  },
  {
    path: "/roles/create",
    name: "RolesCreate",
    component: () => import("@mod1/modules/roles/views/RolesCreate.vue"),
      beforeEnter: requireAuth,
  },
  {
    path: "/roles/edit/:id",
    name: "RolesEdit",
    component: () => import("@mod1/modules/roles/views/RolesEdit.vue"),
      beforeEnter: requireAuth,
    props: true,
  },
];

export default routeRoles;