<template>
  <nav aria-label="breadcrumb">
    <ol class="breadcrumb">
      <li v-for="(crumb, index) in breadcrumbs" :key="index" class="breadcrumb-item" :class="{ active: index === breadcrumbs.length - 1 }">
        <router-link v-if="index !== breadcrumbs.length - 1" :to="crumb.path">{{ crumb.name }}</router-link>
        <span v-else>{{ crumb.name }}</span>
      </li>
    </ol>
  </nav>
</template>

<script>
import { useRoute } from "vue-router";

export default {
  name: "Breadcrumbs",
  setup() {
    const route = useRoute();


    // Generate breadcrumbs based on the current route
    const breadcrumbs = route.matched.map((match) => ({
      name: match.meta.breadcrumb || match.name,
      path: match.path,
    }));

    return { breadcrumbs };
  },
};
</script>

<style scoped>
.breadcrumb {
  background-color: transparent;
  margin-bottom: 0;
  padding: 0;
}

.breadcrumb-item.active {
  font-weight: bold;
  color: #6c757d;
}
</style>