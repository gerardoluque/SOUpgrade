<template>
  <div class="card blur shadow-blur">
    <div class="card-body">
      <div class="d-flex">
        <p class="mb-0" :class="store.isRTL ? 'ms-5' : 'me-5'">
          {{ controllerIs }}
        </p>
        <material-switch :id="id" :name="image.label" checked />
      </div>
      <img class="img-fluid pt-3 pb-2" :src="image.url" :alt="image.alt" />
      <p class="mb-0">{{ image.label }}</p>
    </div>
  </div>
</template>

<script>
import MaterialSwitch from "@/components/common/MaterialSwitch.vue";
import { useMainStore } from "@/store/useMainStore";

export default {
  name: "ControllerCard",
  components: {
    MaterialSwitch,
  },
  props: {
    id: {
      type: String,
      default: "",
    },
    image: {
      type: Object,
      default: () => ({
        url: "",
        alt: "",
        label: "",
      }),
    },
    controllerIs: {
      type: String,
      default: "On",
    },
  },
  setup() {
    const store = useMainStore();
    return { store };
  },
};
</script>