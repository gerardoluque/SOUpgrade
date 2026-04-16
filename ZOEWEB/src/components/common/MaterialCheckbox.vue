<template>
  <div class="form-check p-0">
    <input
      :id="id"
      class="form-check-input"
      type="checkbox"
      :name="name"
      :checked="checked"
    />
    <label :for="id" class="custom-control-label">
      <slot />
    </label>
  </div>
</template>

<script>
import setMaterialInput from "@/assets/js/material-input.js";

export default {
  name: "MaterialCheckbox",
  props: {
    name: {
      type: String,
      default: "",
    },
    id: {
      type: String,
      default: "",
    },
    modelValue: {
      type: [String, Number],
      default: "",
    },
    value: {
      type: String,
      default: "",
    },
    checked: Boolean,
  },
  emits: ["update:modelValue"],
  mounted() {
    setMaterialInput();
  },
};
</script>
