<template>
  <div class="fixed-plugin">
    
    <div class="shadow-lg card">
      <div class="pt-3 pb-0 bg-transparent card-header">
        <div class="float-start">
          <h5 class="mt-3 mb-0">ZOE</h5>
          <p> </p>
        </div>
        <div class="mt-4 float-end" @click="toggle">
          <button class="p-0 btn btn-link text-dark fixed-plugin-close-button">
            <i class="material-icons">clear</i>
          </button>
        </div>
        <!-- End Toggle Button -->
      </div>
      <hr class="my-1 horizontal dark" />
      <div class="pt-0 card-body pt-sm-3">
        <!-- Sidebar Backgrounds -->
        <div class="d-flex flex-column">
            <h6 class="mb-3 text-sm">{{userdata ? userdata.givenName : ''}}</h6>
            <span class="mb-2 text-xs">
              Usuario:
              <span class="text-dark font-weight-bold ms-sm-2"
                >{{userdata ? userdata.userPrincipalName : ''}}</span
              >
            </span>
            <span class="mb-2 text-xs">
              Correo Electrónico:
              <span class="text-dark ms-sm-2 font-weight-bold"
                >{{userdata ? userdata.mail : ''}}</span
              >
            </span>
            <span class="text-xs">
              Celular:
              <span class="text-dark ms-sm-2 font-weight-bold">{{userdata ? userdata.mobilePhone : ''}}</span>
            </span>
          </div>
        <div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { useMainStore } from "@/store/useMainStore";
import { storeToRefs } from "pinia";
import { activateDarkMode, deactivateDarkMode } from "@/assets/js/dark-mode";

export default {
  name: "Configurator",
  props: ["toggle"],
  setup() {
    const store = useMainStore();
    const { userdata } = storeToRefs(store);

    const sidebarColor = (color = "success") => {
      document.querySelector("#sidenav-main").setAttribute("data-color", color);
      store.setColor(color);
    };

    const sidebar = (type) => {
      store.sidebarType = type;
    };

    const setNavbarFixed = () => {
      store.navbarFixed();
    };

    const darkMode = () => {
      if (store.isDarkMode) {
        store.isDarkMode = false;
        deactivateDarkMode();
      } else {
        store.isDarkMode = true;
        activateDarkMode();
      }
    };

    const sidenavTypeOnResize = () => {
      let transparent = document.querySelector("#btn-transparent");
      let white = document.querySelector("#btn-white");
      if (transparent && white) {
      if (window.innerWidth < 1200) {
        transparent.classList.add("disabled");
        white.classList.add("disabled");
      } else {
        transparent.classList.remove("disabled");
        white.classList.remove("disabled");
      }
    } else {
          console.warn("Elementos #btn-transparent o #btn-white no encontrados.");
  }
    };
    const toggleSidenavMini = () => {
      const sidenav = document.querySelector("#sidenav-main");
      if (sidenav) {
      store.toggleSidenavMini();
      if (store.isSidenavMini) {
        sidenav.classList.add("sidenav-mini");
      } else {
        sidenav.classList.remove("sidenav-mini");
      }
    } else {
    console.warn("Elemento #sidenav-main no encontrado.");
  }
    };



    window.addEventListener("resize", sidenavTypeOnResize);
    window.addEventListener("load", sidenavTypeOnResize);

    return {
      store,
      sidebarColor,
      sidebar,
      setNavbarFixed,
      darkMode,
      sidenavTypeOnResize,
      toggleSidenavMini,
      userdata,
    };
  },
};
</script>
