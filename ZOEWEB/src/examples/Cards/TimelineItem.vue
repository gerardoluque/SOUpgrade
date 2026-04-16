<template>
  <div class="card" :class="store.isDarkMode ? 'bg-gradient-dark' : ''">
    <div class="pb-0 card-header" :class="store.isDarkMode ? 'bg-transparent' : ''">
      <h6 :class="store.isDarkMode ? 'text-white' : ''">{{ title }}</h6>
      <!--  eslint-disable-next-line vue/no-v-html -->
      <p class="text-sm" v-html="description"></p>
    </div>
    <div class="p-3 card-body">
      <div
        class="timeline timeline-one-side"
        :data-timeline-axis-style="store.isDarkMode ? 'dashed' : 'dotted'"
      >
        <slot />
      </div>
    </div>
  </div>
</template>

<script>
import { useMainStore } from "@/store/useMainStore";

export default {
  name: "TimelineList",
  props: {
    title: {
      type: String,
      default: "",
    },
    description: {
      type: String,
      default: "",
    },
  },
  setup() {
    const store = useMainStore();
    return { store };
  },
};
</script>