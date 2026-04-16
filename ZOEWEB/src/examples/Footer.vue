<template>
  <footer class="py-3 footer">
    <div class="container-fluid">
      <div class="row align-items-center justify-content-lg-between">
        <div class="mb-4 col-lg-6 mb-lg-0">
          <div class="text-sm text-center copyright text-muted" :class="store.isRTL ? 'text-lg-end' : 'text-lg-start'">

          </div>
        </div>
        <div class="col-lg-6">

        </div>
      </div>
    </div>
  </footer>
</template>

<script>
import { useMainStore } from "@/store/useMainStore";

export default {
  name: "AppFooter",
  setup() {
    const store = useMainStore();
    return { store };
  },
};
</script>