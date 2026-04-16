<template>
  <a
    
    :href="collapse ? `#${collapseRef}` : collapseRef"
    :aria-controls="collapseRef"
    :aria-expanded="isExpanded"    
    class="nav-link"
    v-bind="$attrs"
    @click.prevent="toggleCollapse"
  >
    <div
      class="text-center d-flex align-items-center justify-content-center"
      :class="store.isRTL ? ' ms-2' : 'me-2'"
    >
      <slot name="icon"></slot>
    </div>
    <span class="nav-link-text" :class="store.isRTL ? ' me-1' : 'ms-1'">{{
      navText
    }}</span>
  </a>
  <div :id="collapseRef" :class="['collapse', { show: isExpanded }]">
    <slot name="list"></slot>
  </div>
</template>

<script>
import { useMainStore } from "@/store/useMainStore";

export default {
  name: "SidenavCollapse",
  props: {
    collapseRef: {
      type: String,
      required: true,
    },
    navText: {
      type: String,
      required: true,
    },
    collapse: {
      type: Boolean,
      default: true,
    },
  },
  setup() {
    const store = useMainStore();
    return { store };
  },
  data() {
    return {
      isExpanded: false,
    };
  },
   methods: {
    toggleCollapse() {
     
      this.isExpanded = !this.isExpanded; // Alterna el estado
    },
  },
};
</script>