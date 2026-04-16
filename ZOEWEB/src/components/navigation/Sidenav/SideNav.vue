<template>
  <div>
    <div class="sidenav bg-dark" :class="{ 'sidenav-mini': store.isSidenavMini, 'sidenav-open': isSidenavOpen }">
      <button class="btn btn-sm btn-light sidenav-toggle" @click="toggleSidenav">
        {{ isSidenavOpen ? "Close" : "Open" }}
      </button>
      <ul class="navbar-nav">
        <SidenavItem
v-for="(item, index) in navItems" :key="index" :to="item.to" :mini-icon="item.miniIcon" :sistema-id="item.sistemaId"
          :text="item.text" />
      </ul>
    </div>

    <!-- Main Content -->
    <div class="main-content" :class="{ 'content-pushed': isSidenavOpen }">
      <slot />
    </div>
  </div>
</template>

<script>
import SidenavItem from "@/components/navigation/Sidenav/SidenavItem.vue";
import { ref } from "vue";

export default {
  name: "Sidenav",
  components: {
    SidenavItem,
  },
  setup() {
    const isSidenavOpen = ref(false);

    const toggleSidenav = () => 
    {
        console.log("Toggle Sidenav clicked");
        isSidenavOpen.value = !isSidenavOpen.value;
      
    };

     

    return {
      isSidenavOpen,
      toggleSidenav,
       
    };
  },
};
</script>

<style scoped>
.sidenav.sidenav-mini {
  width: 80px; /* Adjust the width for mini mode */
  overflow: hidden;
  transition: width 0.3s ease;
}

.sidenav.sidenav-mini .navbar-nav .nav-item .nav-link {
  text-align: center;
  padding: 0.5rem 0;
}

.sidenav.sidenav-mini .navbar-nav .nav-item .sidenav-normal {
  display: none; /* Hide the full text in mini mode */
}

.sidenav.sidenav-mini .navbar-nav .nav-item .sidenav-mini-icon {
  display: inline-block;
}
</style>