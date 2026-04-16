<template>
  <nav
id="navbarBlur" class="shadow-none navbar navbar-main navbar-expand-lg border-radius-xl" v-bind="$attrs"
    data-scroll="true" :class="[store.isNavFixed ? 'position-sticky blur shadow-blur top-1 z-index-sticky' : '', store.isAbsolute ? 'mt-4' : 'mt-4']">
    <div class="px-4 py-1 container-fluid">
    
      <div id="navbar" class="mt-2 collapse navbar-collapse mt-sm-0 me-md-0" :class="store.isRTL ? 'px-0' : 'me-sm-4'">
        <a href="#" class="p-0 nav-link text-body" @click.prevent="minNav">
          <div class="sidenav-toggler-inner">
            <i class="sidenav-toggler-line" :class="color ? 'bg-black' : ''"></i>
            <i class="sidenav-toggler-line" :class="color ? 'bg-black' : ''"></i>
            <i class="sidenav-toggler-line" :class="color ? 'bg-black' : ''"></i>
          </div>
        </a>
      </div>
      <div id="navbar" class="mt-2 collapse navbar-collapse mt-sm-0 me-md-0 me-sm-4" :class="store.isRTL ? 'px-0' : 'me-sm-4'">
        <div class="pe-md-3 d-flex align-items-center" :class="store.isRTL ? 'me-md-auto' : 'ms-md-auto'"></div>
        <ul class="navbar-nav justify-content-end">
           
          
         
          
        </ul>
      </div>
    </div>
  </nav>
</template>

<script>

import { useMainStore } from "@/store/useMainStore";
import { storeToRefs } from "pinia";

export default {
  name: "Navbar",
  components: {
  },
  props: {
    minNav: {
      type: Function,
      default: () => {},
    },
    color: {
      type: String,
      default: "text-body",
    },
  },
  setup() {
    const store = useMainStore();
    const { userdata } = storeToRefs(store);
    return { store, userdata };
  },
  data() {
    return {
      showMenu: false,
    };
  },
  methods: {
    toggleSidebar() {
      this.store.navbarMinimize();
    },
  },
};
</script>