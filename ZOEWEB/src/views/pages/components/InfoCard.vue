<template>
  <div class="card">
    <div class="card-header pb-0">
      <div class="d-flex">
        <p>{{ title }}</p>
        <div :class="store.isRTL ? 'me-auto' : 'ms-auto'">
          <material-badge :color="badge.color">{{ badge.text }}</material-badge>
        </div>
      </div>
    </div>
    <div class="card-body pt-0">
      <p class="mb-0">{{ description }}</p>
    </div>
  </div> 
</template>

<script>
import MaterialBadge from "@/components/common/MaterialBadge.vue";
import { useMainStore } from "@/store/useMainStore";

export default {
  name: "InfoCard",
  components: {
    MaterialBadge,
  },
  props: {
    title: {
      type: String,
      default: "",
    },
    description: {
      type: String,
      default: "",
    },
    badge: {
      type: Object,
      default: () => ({
        color: "success",
        text: "Moderate",
      }),
    },
  },
  setup() {
    const store = useMainStore();
    return { store };
  },
};
</script>