import { createRouter, createWebHistory } from "vue-router";
import Default from "../views/dashboards/Default.vue";
import Basic from "../views/auth/signin/Basic.vue";
import UserInfo from "../views/pages/users/UserInfo.vue";
import { msalInstance } from "../authConfig";
import routeGrupos from "@mod1/modules/grupos/route/routeGrupos.js";
import routeRoles from "@mod1/modules/roles/route/routeRoles.js";
import routeProcesos from "@mod1/modules/procesos/route/routeProceso.js";
import routeBitacora from "@mod1/modules/bitacora/route/routeBitacora.js";
import routeUsuarios from "@mod1/modules/usuarios/route/routeUsuarios.js";
import route2Factor from "@/views/auth/external/2Factor/route/route2Factor.js";
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
      next({ name: "/authentication/signin/basic" }); // Redirige a la página de autenticación de dos factores
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

const baseRoutes = [
  {
    path: "/",
    name: "/",
    redirect: "/authentication/signin/basic",
    meta : {breadcrum: "Home" }
  },
   {
    path: "/auth/external/2FactorQR",
    name: "2FactorQR",
    component: () => import("@/views/auth/signin/external/2Factor/2FactorQR.vue"),            
     meta : {breadcrum: "E2F" }
  },
   {
    path: "/auth/external/2FactorAuth",
    name: "2FactorAuth",
    component: () => import("@/views/auth/signin/external/2Factor/2FactorAuth.vue"),               
     meta : {breadcrum: "E2F" }
  },
  {
    path: "/auth/external/2Factor",
    name: "External2Factor",
    component: () => import("@/views/auth/external/2Factor/views/External2Factor.vue"),            
     meta : {breadcrum: "E2F" }
  },
  {
    path: "/pages/users/user-info",
    name: "UserInfo",
    component: UserInfo,
    beforeEnter: requireAuth,
     meta : {breadcrum: "usuario" }
  },
  {
    path: "/dashboards/dashboard-default",
    name: "Default",
    component: Default,
    beforeEnter: requireAuth,
  },  
  
  {
    path: "/authentication/signin/basic",
    name: "Signin Basic",
    component: Basic ,
  },
];

const routes = [
  ...baseRoutes
  ,...routeGrupos
  ,...routeUsuarios
  ,...routeRoles
  ,...routeProcesos
  ,...routeBitacora
  ,...route2Factor,
 
    {
    path: "/:pathMatch(.*)*",
    name: "NotFound",
    component: () => import("@/views/NotFound.vue"),
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
  linkActiveClass: "active",
});

router.beforeEach((to, from, next) => {
  const routeExists = router.getRoutes().some((route) => route.name === to.name);

  if (!routeExists) {
    next({ name: "NotFound" }); 
  } else {
    // On valid route navigation, determine and persist the SistemaId for the selected process
    try {
      const userRol = localStorageService.get("userRol");
      let sistemaId = null;
      if (userRol && Array.isArray(userRol.procesos)) {
        for (const p of userRol.procesos) {
          // Match against child processes first
          if (p && Array.isArray(p.subprocesos)) {
            const found = p.subprocesos.find((sub) => sub && sub.ruta === to.name);
            if (found) {
              sistemaId = found.sistemaId || found.sistemaID || found.sistemaid || (found.sistema && (found.sistema.id || found.sistemaId)) || null;
              break;
            }
          }
          // Fallback: parent process might map directly to a route
          if (!sistemaId && p && p.ruta === to.name) {
            sistemaId = p.sistemaId || p.sistemaID || p.sistemaid || (p.sistema && (p.sistema.id || p.sistemaId)) || null;
          }
        }
      }
      if (sistemaId !== null && sistemaId !== undefined && sistemaId !== "") {
        localStorageService.set("sistemaSelectedId", sistemaId);
      }
    } catch (e) {
      // Silently ignore if structure not available; header will simply be omitted
      // console.warn('No se pudo establecer SistemaId en navegación', e);
    }
    next(); 
  }
});

export default router;
