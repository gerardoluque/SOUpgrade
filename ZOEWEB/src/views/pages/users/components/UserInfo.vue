<template>
  <div v-if="user != null" class="container-fluid">
  <div
      class="page-header min-height-300 border-radius-xl mt-4"
      :style="{ backgroundImage: `url(${require('@/assets/img/aux-imgbg.jpg')})`, opacity: 0.7 }"
    > 
    </div>
    <div class="card card-body mx-3 mx-md-4 mt-n6">
      <div class="row">
        <div class="col-12 mt-4">
          <profile-info-card
            title="Información del Usuario"
            description=""
            :info="{
              fullName: user.nombreCompleto || 'N/A',
              mobile: user.telefono || 'No disponible',
              email: user.email || 'No disponible',
              location: coporacionSelected || 'No disponible',
              rol: (userRol != null && userRol.name != null) ? userRol.name : 'No disponible',
            }"
            :action="{
              route: 'javascript:;',
              tooltip: 'Editar Perfil',
            }"
          />
        </div>
      </div>
    </div>
  </div>
  <div v-else>Cargando Información...</div>
</template>

<script>
import ProfileInfoCard from "./ProfileInfoCard.vue";
import setNavPills from "@/assets/js/nav-pills.js";
import setTooltip from "@/assets/js/tooltip.js";
import { useMainStore } from "@/store/useMainStore";
import { storeToRefs } from "pinia";
import { onBeforeMount } from "vue";

export default {
  name: "ProfileOverview",
  components: {
    ProfileInfoCard,
  },
  setup() {
    const store = useMainStore();
    const { userdata: user, userRol, coporacionSelected } = storeToRefs(store);

    onBeforeMount(async () => {
      await store.getUserInfo();
    });

    return { store, user, userRol, onBeforeMount, coporacionSelected };
  },
  data() {
    return {
      showMenu: false,
    };
  },
  persist: {
    enabled: true,
    strategies: [
      {
        key: "mainStore",
        storage: localStorage, // Use localStorage to persist state
        paths: ["user", "userRol"], // Specify which parts of the state to persist
      },
    ],
  },
  mounted() {
    this.store.isAbsolute = true;
    setNavPills();
    setTooltip(this.store.bootstrap);
  },
  beforeUnmount() {
    this.store.isAbsolute = false;
  },
};
</script>