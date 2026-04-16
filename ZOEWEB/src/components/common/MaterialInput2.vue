<template>
  <div class="material-input-container">
    <label class="material-label">{{ label }}</label>
    <input
      :id="id"
      :type="type"
      class="material-input"
      :class="[
        getClasses(size),
        { 'has-error': showError },
        { 'is-valid': success },
        { 'is-filled': modelValue }
      ]"
      :name="name"
      :value="modelValue"
      :placeholder="showError ? localErrorMessage : placeholder"
      :required="isRequired"
      :disabled="disabled"
      :maxlength="maxLength || null"
      :minlength="minLength || null"       
      :inputmode="validateType === 'phone' ? 'numeric' : null"
      :pattern="validateType === 'phone' ? '[0-9]*' : null"
      @input="emitValue"
      @change="emitValue"
      @blur="validateInput"
      @keypress="onKeyPress"
    />
    <div v-if="showError" class="error-message">
      {{ localErrorMessage }}
    </div>
  </div>
</template>

<script>
import { useMexValidators } from "@/composables/useValidators.js";
const { isValidCURP, isValidRFC } = useMexValidators();

export default {
  name: "MaterialInput2",
  props: {
    variant: { type: String, default: "outline" },
    label: { type: String, default: "" },
    size: { type: String, default: "default" },
    success: { type: Boolean, default: false },
    error: { type: Boolean, default: false },
    disabled: { type: Boolean, default: false },
    name: { type: String, default: "" },
    id: { type: String, required: true },
    value: { type: String, default: "" },
    placeholder: { type: String, default: "" },
    modelValue: { type: [String, Number], default: "" },
    type: { type: String, default: "text" },
    isRequired: { type: Boolean, default: false },
    errorMessage: { type: String, default: "Este campo es obligatorio." },
    minLength: { type: Number, default: null },
    maxLength: { type: Number, default: null },
    validateType: { type: String, default: "" }, // '', 'curp', 'rfc', 'phone', 'email'
    uppercase: { type: Boolean, default: false },  
  },
  emits: ["update:modelValue"],
  data() {
    return {
      showError: false,
      localErrorMessage: this.errorMessage,
    };
  },
  methods: {
    emitValue(e) {
      let val = e.target.value ?? "";
      if (this.uppercase && typeof val === "string") val = val.toUpperCase();
      
      // Enforce max length proactively
      if (this.maxLength && val.length > this.maxLength) {
        val = val.slice(0, this.maxLength);
      }
      
      // Sanitize input based on validation type
      if (this.validateType === 'curp') {
        val = val.replace(/[^A-Z0-9]/gi, '').toUpperCase();
      } else if (this.validateType === 'rfc') {
        val = val.replace(/[^A-Z0-9&]/gi, '').toUpperCase();
      } else if (this.validateType === 'phone') {
        val = val.replace(/\D/g, '');
      }
      
      // Reflect sanitized value in DOM
      e.target.value = val;
      this.$emit("update:modelValue", val);
    },
    
    onKeyPress(e) {
      if (this.validateType === 'phone') {
        const key = e.key;
        if (!/[0-9]/.test(key)) {
          e.preventDefault();
        }
      }
    },
    
    getClasses(size) {
      let sizeValue = size ? `input-${size}` : null;
      return sizeValue;
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
/* Contenedor principal */
.material-input-container {
  margin-bottom: 24px;
  width: 100%;
}

/* Label mejorado para vista cansada */
.material-label {
  display: block;
  font-size: 18px !important;
  font-weight: 700 !important;
  color: #1a202c !important;
  margin-bottom: 12px !important;
  letter-spacing: 0.5px !important;
  line-height: 1.3 !important;
  text-transform: none !important;
  font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial, sans-serif !important;
}

/* Input mejorado */
.material-input {
  width: 100%;
  padding: 16px 20px !important;
  font-size: 16px !important;
  font-weight: 500 !important;
  line-height: 1.5 !important;
  color: #2d3748 !important;
  background-color: #ffffff !important;
  border: 3px solid #cbd5e0 !important;
  border-radius: 8px !important;
  transition: all 0.2s ease-in-out !important;
  box-sizing: border-box !important;
}

/* Estados del input */
.material-input:focus {
  outline: none !important;
  border-color: #3182ce !important;
  box-shadow: 0 0 0 3px rgba(49, 130, 206, 0.1) !important;
  background-color: #ffffff !important;
}

.material-input:hover:not(:focus):not(:disabled) {
  border-color: #a0aec0 !important;
}

.material-input:disabled {
  background-color: #f7fafc !important;
  color: #a0aec0 !important;
  cursor: not-allowed !important;
  border-color: #e2e8f0 !important;
}

/* Estado con contenido */
.material-input.is-filled {
  background-color: #ffffff !important;
}

/* Estado de error */
.material-input.has-error {
  border-color: #e53e3e !important;
  background-color: #fed7d7 !important;
}

.material-input.has-error:focus {
  border-color: #c53030 !important;
  box-shadow: 0 0 0 3px rgba(229, 62, 62, 0.1) !important;
}

/* Estado válido */
.material-input.is-valid {
  border-color: #38a169 !important;
  background-color: #f0fff4 !important;
}

.material-input.is-valid:focus {
  border-color: #2f855a !important;
  box-shadow: 0 0 0 3px rgba(56, 161, 105, 0.1) !important;
}

/* Mensaje de error */
.error-message {
  margin-top: 8px !important;
  font-size: 14px !important;
  font-weight: 600 !important;
  color: #e53e3e !important;
  line-height: 1.4 !important;
}

/* Placeholder mejorado */
.material-input::placeholder {
  color: #a0aec0 !important;
  opacity: 1 !important;
  font-weight: 400 !important;
}

.material-input.has-error::placeholder {
  color: #c53030 !important;
  font-weight: 600 !important;
}

/* Tamaños diferentes */
.material-input.input-sm {
  padding: 12px 16px !important;
  font-size: 15px !important;
}

.material-input.input-lg {
  padding: 20px 24px !important;
  font-size: 18px !important;
}

/* Alineación para números */
.material-input[type="number"] {
  text-align: right;
}

/* Responsivo para móviles */
@media (max-width: 768px) {
  .material-label {
    font-size: 19px !important;
    font-weight: 700 !important;
    margin-bottom: 14px !important;
  }
  
  .material-input {
    padding: 18px 20px !important;
    font-size: 17px !important;
    border-width: 3px !important;
  }
  
  .error-message {
    font-size: 15px !important;
    font-weight: 600 !important;
  }
}

/* Alto contraste para accesibilidad */
@media (prefers-contrast: high) {
  .material-label {
    color: #000000 !important;
    font-weight: 800 !important;
    text-shadow: none !important;
  }
  
  .material-input {
    border-color: #000000 !important;
    border-width: 4px !important;
    color: #000000 !important;
  }
  
  .material-input:focus {
    border-color: #000000 !important;
    box-shadow: 0 0 0 4px rgba(0, 0, 0, 0.2) !important;
  }
}

/* Soporte para modo oscuro */
@media (prefers-color-scheme: dark) {
  .material-label {
    color: #f7fafc !important;
  }
  
  .material-input {
    background-color: #2d3748 !important;
    border-color: #4a5568 !important;
    color: #f7fafc !important;
  }
  
  .material-input:focus {
    border-color: #63b3ed !important;
    background-color: #2d3748 !important;
  }
  
  .material-input::placeholder {
    color: #a0aec0 !important;
  }
}
</style>