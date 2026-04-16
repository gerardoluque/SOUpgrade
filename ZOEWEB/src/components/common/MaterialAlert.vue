<template>
  <div v-if="alert.visible" class="alert text-white" role="alert" :class="getClasses(alert.color, dismissible)">
    <span class="alert-icon">
      <i :class="getIcon(alert.icon)" />
    </span>
    <span class="alert-text">
      {{ alert.message }}
    </span>
    <button
v-if="dismissible" type="button" class="btn-close d-flex justify-content-center align-items-center"
      aria-label="Close" @click="clearAlert">
      <span aria-hidden="true" class="text-lg font-weight-bold">&times;</span>
    </button>
  </div>
</template>

<script>
import { useMainStore } from "@/store/useMainStore";
import { computed } from "vue";

export default {
  name: "MaterialAlert",
  props: {
    dismissible: {
      type: Boolean,
      default: true,
    },
  },
  setup() {
    const store = useMainStore();

    // Computed property to access the alert state
    const alert = computed(() => store.alert);

    const clearAlert = () => {
      store.clearAlert();
    };

    return {
      alert, // Computed alert state
      clearAlert,
    };
  },
  methods: {
    getClasses: (color, dismissible) => {
      const colorValue = color ? `alert-${color}` : null;
      const dismissibleValue = dismissible ? "alert-dismissible fade show" : null;
      return `${colorValue} ${dismissibleValue}`;
    },
    getIcon: (icon) => (icon ? `material-icons-round ${icon}` : null),
  },
};
</script>

<style scoped>
.alert-icon {
  margin-right: 10px;
}
</style>