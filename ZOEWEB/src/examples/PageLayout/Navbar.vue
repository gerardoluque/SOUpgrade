<template>
  <!-- Navbar -->
  <nav
class="top-0 navbar navbar-expand-lg position-absolute z-index-3"
    :class="isBlur ? isBlur : 'shadow-none my-3 navbar-transparent w-100 mt-4'">
    <div class="container" :class="isBlur && 'px-1'">
      <router-link
class="navbar-brand font-weight-bolder ms-lg-0 ms-3" :class="darkMode ? 'text-black' : 'text-white'"
        to="/"></router-link>
      <button
class="shadow-none navbar-toggler ms-2" type="button" data-bs-toggle="collapse"
        data-bs-target="#navigation" aria-controls="navigation" aria-expanded="false" aria-label="Toggle navigation">
        <span class="mt-2 navbar-toggler-icon">
          <span class="navbar-toggler-bar bar1"></span>
          <span class="navbar-toggler-bar bar2"></span>
          <span class="navbar-toggler-bar bar3"></span>
        </span>
      </button>
      <div id="navigation" class="pt-3 pb-2 collapse navbar-collapse w-100 py-lg-0">
        <ul class="mx-auto navbar-nav navbar-nav-hover">
       
        </ul>
      </div>
    </div>
  </nav>
  <!-- End Navbar -->
</template>

<script>
import downArrWhite from "@/assets/img/down-arrow-white.svg";
import downArrBlack from "@/assets/img/down-arrow-dark.svg";
import { useMainStore } from "@/store/useMainStore";
import { storeToRefs } from "pinia";

export default {
  name: "Navbar",
  props: {
    btnBackground: {
      type: String,
      default: "bg-white",
    },
    isBlur: {
      type: String,
      default: "",
    },
    darkMode: {
      type: Boolean,
      default: false,
    },
  },
  setup() {
    const store = useMainStore();
    const { currentRoute } = storeToRefs(store); // Access currentRoute from Pinia store

    return {
      downArrWhite,
      downArrBlack,
      currentRoute,
    };
  },
  computed: {
    darkModes() {
      return {
        "text-dark": this.darkMode,
      };
    },
  },
};
</script>