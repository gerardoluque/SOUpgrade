<template>
  <div id="sidenav-collapse-main" class="w-auto h-auto collapse navbar-collapse max-height-vh-100 h-100">
    <!-- <div class="user-info px-3 py-4 text-center">
      <div class="avatar-container">
         
      </div>
      <h6 class="mb-1 user-name">{{ user.name }}</h6>
      <p class="text-sm text-secondary user-email">{{ user.email }}</p>      
    </div> -->
    <ul class="navbar-nav">
       <li class="nav-item btn-user-info">
        <a
          class="nav-link font-weight-bold px-3"
          @click.prevent="openUserInfo"
        >
          <div class="d-flex align-items-center">
            <i class="material-icons-round opacity-10 me-2 " data-bs-toggle="tooltip" data-bs-placement="top" title="información del Usuario">person</i>
            <span v-if="!navbarMinimize" class="sidenav-normal">{{ user?.name || 'Información del Usuario' }}</span>
          </div>
        </a>
      </li>

      <li v-for="(item, index) in navItems" :key="index" class="nav-item" :class="isActive(item) ? 'active' : ''">
        <sidenav-collapse :collapse-ref="item.title" :nav-text="item.title" :class="isActive(item) ? 'active' : ''">
          <template #icon>
            <i class="material-icons-round opacity-10">{{ item.icon }}</i>
          </template>
          <template #list>
           <ul class="nav pe-0">
              <sidenav-item
                v-for="(child, childIndex) in item.children"
                :key="childIndex"
                :to="{ name: child.name }"
                :mini-icon="child.miniIcon"
                :text="child.text"
                :class="{ active: currentRoute === child.name }"
              />
            </ul>
          </template>
        </sidenav-collapse>
      </li>
    </ul>

    <br><br>
    <div class="mt-5 px-3">
      <MaterialButton id="logout-button" color="info"   variant="gradient" full-width @click="logout">
        Salir
      </MaterialButton>
    </div>
  </div>
</template>

<script>
import SidenavItem from "./SidenavItem.vue";
import SidenavCollapse from "./SidenavCollapse.vue";
import MaterialButton from "@/components/common/MaterialButton.vue";
import { useMainStore } from "@/store/useMainStore";
import { useRoute } from "vue-router";
import { watch,  computed } from "vue";
import { storeToRefs } from "pinia";
import { useRouter } from "vue-router";

export default {
  name: "SidenavList",
  components: {
    SidenavItem,
    SidenavCollapse,
    MaterialButton,
  },
  setup() {
    const store = useMainStore();
    const { navbarMinimize, currentRoute, navItems,userdata,usercorporations,coporacionSelected } = storeToRefs(store);
    const route = useRoute();
      const router = useRouter();

     // User Information (tolerant to null/async)
     const user = computed(() => ({
      name:  userdata.value?.nombreCompleto || userdata.value?.displayName || 'Usuario',
      email: userdata.value?.email || userdata.value?.mail || '',
      avatar: "https://via.placeholder.com/60",
    }));


   
      const comboOptions = Array.isArray(usercorporations.value)
    ? usercorporations.value.map((x) => ({ label: x }))
    : [];


    watch(
      () => route.name,
      (newRoute) => {
        store.setCurrentRoute(newRoute);
      },
      { immediate: true }
    );


    const isActive = (item) => {      
      return item.children.some((child) => child.name === currentRoute.value);
    };


    const logout = () => {
      store.logout();
    };

   

     const openUserInfo = () => {  
      router.push({ name: "UserInfo" });
    };

    return { navbarMinimize,currentRoute, navItems, isActive, logout
      , user
      , comboOptions
      , selectedOption: null
      ,userdata
      ,coporacionSelected,
      openUserInfo

     };
  },
};
</script>
<style scoped>
</style>

<style scoped>
.btn-user-info .sidenav-normal {
  white-space: normal; /* allow wrapping */
  overflow-wrap: anywhere; /* break long words if needed */
  word-break: break-word;
  display: inline-block;
  max-width: calc(100% - 36px); /* leave space for the icon */
}

.btn-user-info .nav-link {
  padding-right: 12px;
}
</style>
 