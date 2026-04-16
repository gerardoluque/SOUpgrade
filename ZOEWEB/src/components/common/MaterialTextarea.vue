<template>
  <div
    class="input-group"
    :class="`input-group-${variant} ${getStatus(error, success)}`"
  >
    <textarea
      :id="id"
      class="form-control"
      :rows="rows"
      :name="name"
      :value="modelValueComputed"
      :placeholder="placeholder"
      :isRequired="isRequired"
      :disabled="disabled"
      @input="$emit('update:modelValue', $event.target.value); $emit('input', $event.target.value)"
    ></textarea>
  </div>
</template>

<script>
import setMaterialInput from "@/assets/js/material-input.js";

export default {
  name: "MaterialTextarea",
  emits: ['update:modelValue','input'],
  props: {
    variant: {
      type: String,
      default: "outline",
    },
    id: {
      type: String,
      required: true,
    },
    name: {
      type: String,
      default: "",
    },
    // v-model support
    modelValue: {
      type: String,
      default: undefined,
    },
    // backward compatible `value` prop
    value: {
      type: String,
      default: "",
    },
    placeholder: {
      type: String,
      default: "...",
    },
    isRequired: Boolean,
    disabled: {
      type: Boolean,
      default: false,
    },
    rows: {
      type: Number,
      default: 5,
    },
    success: {
      type: Boolean,
      default: false,
    },
    error: {
      type: Boolean,
      default: false,
    },
  },
  computed: {
    modelValueComputed() {
      return this.modelValue !== undefined ? this.modelValue : this.value
    }
  },
  mounted() {
    setMaterialInput();
  },
  methods: {
    getStatus: (error, success) => {
      let isValidValue;

      if (success) {
        isValidValue = "is-valid";
      } else if (error) {
        isValidValue = "is-invalid";
      } else {
        isValidValue = null;
      }

      return isValidValue;
    },
  },
};
</script>
