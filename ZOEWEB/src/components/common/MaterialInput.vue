<template>
  <div
    class="input-group"
    :class="[ 
      `input-group-${variant}`,
      getStatus(error, success),
      { 'is-filled': modelValue || isDateTimeInput || autofilled },
      { 'is-focused': isFocused }
    ]"
  >
    <label :class="variant === 'static' ? '' : 'form-label'">{{ label }}</label>
    <input
      :id="id"
      :type="type"
      class="form-control"
      :class="getClasses(size)"
      :name="name"
      :autocomplete="autocomplete"
      :value="modelValue"
      :placeholder="showError ? localErrorMessage : placeholder"
      :required="isRequired"
      :disabled="disabled"
      :style="{ borderColor: showError ? 'red' : '' }"          
      :maxlength="maxLength || null"
      :minlength="minLength || null"       
      :inputmode="validateType === 'phone' ? 'numeric' : null"
      :pattern="validateType === 'phone' ? '[0-9]*' : null"
      @input="emitValue"
  @change="emitValue"
  @blur="onBlur"
  @focus="onFocus"
  @keypress="onKeyPress"
  @keyup="$emit('keyup', $event)"
    />
  </div>
</template>

<script>
import setMaterialInput from "@/assets/js/material-input.js";
import { useMexValidators } from "@/composables/useValidators.js";
const { isValidCURP, isValidRFC } = useMexValidators();

export default {
  name: "MaterialInput",
  props: {
    variant: { type: String, default: "outline", },
    label: { type: String, default: "", },
    size: { type: String, default: "default", },
    success: { type: Boolean, default: false, },
    error: { type: Boolean, default: false, },
    disabled: { type: Boolean, default: false, },
    name: { type: String, default: "", },
    id: { type: String, required: true, },
    value: { type: String, default: "", },
    placeholder: { type: String, default: "", },
    modelValue: { type: [String, Number], default: "",},
    type: { type: String, default: "text", },
    // native input autocomplete attribute (e.g. 'off', 'new-password')
    autocomplete: { type: String, default: null },
    isRequired: { type: Boolean, default: false, },
    errorMessage: {type: String, default: "Este campo es obligatorio.",},
    minLength: {type: Number,default: null,}, // Minimum characters allowed
    maxLength: {type: Number,default: 8000, }, // Maximum characters allowed  
    validateType: { type: String, default: "" }, // '', 'curp', 'rfc', 'phone', 'email'
    uppercase: { type: Boolean, default: false },  
  },
  emits: ["update:modelValue", "keyup"],
  data() {
    return {
      showError: false,
      localErrorMessage: this.errorMessage, // Local property for error message
      isFocused: true, // Para manejar el estado de focus
      autofilled: false
    };
  },
  computed: {
    // Determina si es un input de fecha o datetime
    isDateTimeInput() {
      return this.type === 'date' || this.type === 'datetime' || this.type === 'datetime-local';
    }
  },
  watch: {
    modelValue(newVal) {
      // if parent sets/clears value, update autofilled flag accordingly
      try {
        this.autofilled = !!(newVal !== null && newVal !== undefined && String(newVal).trim() !== '')
      } catch (e) { /* ignore */ }
    }
  },
  mounted() {
    setMaterialInput();
    // Detect browser autofill: check value after short delay and listen to animationstart
    this.$nextTick(() => {
      try {
        const input = this.$el.querySelector('input')
        if (!input) return
        // Repeated checks because some browsers fill slightly after load.
        // Poll input value for up to ~1s (200ms intervals) and set autofilled when non-empty.
        let checks = 0
        const maxChecks = 5
        const interval = setInterval(() => {
          try {
            if (input.value && String(input.value).trim() !== '') {
              this.autofilled = true
              clearInterval(interval)
              return
            }
            checks++
            if (checks >= maxChecks) clearInterval(interval)
          } catch (e) { clearInterval(interval) }
        }, 200)

        // animationstart detection: define animation name in CSS and listen
        const onAnim = (ev) => {
          try {
            if (ev && ev.animationName && String(ev.animationName).toLowerCase().includes('onautofill')) {
              if (input.value && String(input.value).trim() !== '') this.autofilled = true
            }
          } catch (e) { /* ignore */ }
        }
        input.addEventListener('animationstart', onAnim)
        // store handler reference for cleanup in lifecycle hook
        this._onAutofillAnim = onAnim
      } catch (e) { /* ignore */ }
    })
  },
  beforeUnmount() {
    try {
      const input = this.$el && this.$el.querySelector ? this.$el.querySelector('input') : null
      if (input && this._onAutofillAnim) {
        input.removeEventListener('animationstart', this._onAutofillAnim)
      }
    } catch (e) { /* ignore */ }
  },
  methods: {
    emitValue(e) {
      let val = e.target.value ?? "";
      if (this.uppercase && typeof val === "string") val = val.toUpperCase();
      // Enforce max length proactively
      if (this.maxLength && val.length > this.maxLength) {
        val = val.slice(0, this.maxLength);
      }
      // Opcional: evitar caracteres inválidos básicos para curp/rfc mientras se escribe (solo letras/números y & para RFC)
      if (this.validateType === 'curp') {
        val = val.replace(/[^A-Z0-9]/gi, '').toUpperCase();
      } else if (this.validateType === 'rfc') {
        val = val.replace(/[^A-Z0-9&]/gi, '').toUpperCase();
      } else if (this.validateType === 'phone') {
        // Solo dígitos para teléfonos
        val = val.replace(/\D/g, '');
      }
      // Refleja inmediatamente el valor saneado en el DOM
      e.target.value = val;
      this.$emit("update:modelValue", val);
    },
    onFocus() {
      this.isFocused = true;
    },
    onBlur() {
      this.isFocused = false;
      this.validateInput();
    },
    onKeyPress(e) {
      if (this.validateType === 'phone') {
        const key = e.key;
        if (!/[0-9]/.test(key)) {
          e.preventDefault();
        }
      }
    },
    getClasses: (size) => {
      let sizeValue;

      sizeValue = size ? `form-control-${size}` : null;

      return sizeValue;
    },
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
    validateInput() {
      const val = (this.modelValue ?? "").toString().trim();

      if (this.isRequired && !val) {
      this.showError = true;
      this.localErrorMessage = this.errorMessage || "Este campo es obligatorio.";
      return;
      }
      if (this.minLength && val.length < this.minLength) {
      this.showError = true;
      this.localErrorMessage = `Mínimo ${this.minLength} caracteres.`;
      return;
      }
      if (this.maxLength && val.length > this.maxLength) {
        this.showError = true;
        this.localErrorMessage = `Máximo ${this.maxLength} caracteres.`;
        return;
      }

      if (this.validateType === "curp" && val && !isValidCURP(val)) {
      this.showError = true;
      this.localErrorMessage = "CURP inválida (revisa formato y fecha).";
      return;
      }
      if (this.validateType === "rfc" && val && !isValidRFC(val)) {
      this.showError = true;
      this.localErrorMessage = "RFC inválido (revisa formato y fecha).";
      return;
      }
      if (this.validateType === 'email' && val) {
        const EMAIL_REGEX = /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i;
        if (!EMAIL_REGEX.test(val)) {
          this.showError = true;
          this.localErrorMessage = "Correo electrónico inválido.";
          return;
        }
      }

      this.showError = false;
    },
  },
};
</script>
<style scoped>
@keyframes onAutoFillStart { from { } to { } }
.form-control:-webkit-autofill { animation-name: onAutoFillStart; }
.form-control:-webkit-autofill { animation-duration: 0.01s; }
.input-group .form-control {
  transition: border-color 0.3s ease;
}

.input-group .form-control.is-invalid {
  border-color: red !important;
}

.input-group .form-control::placeholder {
  color: red;
  opacity: 1;
  font-size:16px;
  text-align: end;
}
.input-group .form-control.is-valid {
  border-color: #C9A432 !important;
}

.form-control {
  border: 2px solid #ccc !important;
  border-radius: 5px;
  background: #fff;
}

.input-group .form-control:focus,
.input-group .form-control:hover {
  border-color: #C9A432 !important;
  box-shadow: 0 0 0 2px rgba(0,123,255,0.15); /* opcional para efecto */
}

/* Right-align numeric input values */
.input-group input[type="number"] {
  text-align: right;
}

/* Estilos para inputs de fecha/datetime - label siempre visible como ComboBox */
.input-group input[type="date"] + .form-label,
.input-group input[type="datetime"] + .form-label,
.input-group input[type="datetime-local"] + .form-label {
  font-size: 14px !important;
  font-weight: 600 !important;  
  color: #344563 !important;
}

.input-group.is-filled input[type="date"] ~ .form-label,
.input-group.is-filled input[type="datetime"] ~ .form-label,
.input-group.is-filled input[type="datetime-local"] ~ .form-label,
.input-group.is-focused input[type="date"] ~ .form-label,
.input-group.is-focused input[type="datetime"] ~ .form-label,
.input-group.is-focused input[type="datetime-local"] ~ .form-label {
  font-size: 14px !important;
  font-weight: 600 !important;
  color: #344563 !important;
}

/* Estilos para label visible y accesible - homogéneo con MaterialInput fecha/datetime */
.input-group.is-filled .form-label,
.input-group.is-focused .form-label {
    font-size: 14px !important;
    font-weight: 600 !important;
    color: #4a9d5f !important;
}

</style>