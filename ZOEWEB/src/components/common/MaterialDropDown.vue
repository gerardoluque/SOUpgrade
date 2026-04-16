<template>
  <div
    class="input-group"
    :class="[
      `input-group-${variant}`,
      getStatus(error, success),
      'is-filled',
      { 'is-focused': isFocused }
    ]"
  >
    <label v-if="!hideLabel && label" :class="variant === 'static' ? '' : 'form-label'">{{ label }}</label>
    <select
  :id="id"
  v-model="proxyValue"
  class="form-control"
  required
  :disabled="disabled"
  @focus="onFocus"
  @blur="onBlur"
>
  <option disabled value="">{{ seleccionaPlaceholder || 'Selecciona una opción' }}</option>
  <option
    v-for="(option, index) in options"
    :key="index"
    :value="normalizeValue(option)"
  >
    {{ option.name }}
  </option>
</select>
  </div>
</template>

<script>
import { defineComponent, computed, ref } from "vue";

export default defineComponent({
  name: "MaterialDropDown",
  props: {
    id: { type: String, required: true },
    label: { type: String, default: "" },
  seleccionaPlaceholder: { type: String, default: "Selecciona una opción" },
  hideLabel: { type: Boolean, default: false },
    modelValue: { type: [String, Number, null], default: "" },
    options: { type: Array, default: () => [] },
    variant: { type: String, default: "outline" },
    success: { type: Boolean, default: false },
    error: { type: Boolean, default: false },
    // Si tus IDs son numéricos y quieres guardar número en el v-model, pon esto en true
    numericValue: { type: Boolean, default: false },
    disabled: { type: Boolean, default: false, },
  },
  emits: ["update:modelValue"],
  setup(props, { emit }) {
    const isStrictNumeric = (val) => {
      const s = String(val)
      return /^-?\d+(?:\.\d+)?$/.test(s)
    }

    const proxyValue = computed({
      get: () => {
        const mv = props.modelValue
        return mv == null ? "" : String(mv)
      },
      set: (val) => {
        const out = props.numericValue && isStrictNumeric(val) ? Number(val) : val
        emit("update:modelValue", out)
      },
    });

    const hasValue = computed(() => proxyValue.value !== "");

    const normalizeValue = (option) => {
      const raw = option?.id !== undefined ? option.id : option?.name ?? "";
      // Siempre renderiza como string en el DOM; la coerción (si aplica) se hace en el setter
      return String(raw);
    };

    const getStatus = (error, success) => {
      if (success) return "is-valid";
      if (error) return "is-invalid";
      return null;
    };

    const isFocused = ref(false);

    const onFocus = () => {
      isFocused.value = true;
    };

    const onBlur = () => {
      isFocused.value = false;
    };

    return { proxyValue, hasValue, normalizeValue, getStatus, onFocus, onBlur, isFocused };
  },
});
</script>

<style scoped>
.form-control {
  border: 2px solid #ccc !important;
  border-radius: 5px;
  background: #fff;
}

.input-group .form-control:focus,
.input-group .form-control:hover {
  border-color: #C9A432 !important;
  box-shadow: 0 0 0 2px rgba(0,123,255,0.15);
}

.input-group.is-filled .form-label,
.input-group.is-focused .form-label {
  font-size: 14px !important;
  font-weight: 600 !important;
  color: #344563 !important;
}
</style>
