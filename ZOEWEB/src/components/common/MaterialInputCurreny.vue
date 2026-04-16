<template>
  <div
    class="input-group"
    :class="[ 
      `input-group-${variant}`,
      getStatus(error, success),
      { 'is-filled': internalRawValue && internalRawValue !== '', 'is-focused': isFocused }
    ]"
  >
    <label :class="variant === 'static' ? '' : 'form-label'">{{ label }}</label>
    <input
      :id="id"
      type="text"
      class="form-control"
      :class="getClasses(size)"
      :name="name"
      :value="displayValue"
      :placeholder="placeholder"
      :required="isRequired"
      :disabled="disabled"
      :maxlength="maxLength || null"
      inputmode="decimal"
      @input="onInput"
      @blur="onBlur"
      @focus="onFocus"
      @keypress="onKeyPress"
      @keyup="$emit('keyup', $event)"
    />
  </div>
</template>

<script>
import setMaterialInput from "@/assets/js/material-input.js";

/**
 * MaterialInputCurreny
 * - Especializado para entradas numéricas decimales
 * - Muestra formato de moneda para México (MXN) usando Intl
 * - Emite `update:modelValue` con Number (o empty string si vacío)
 * - Evita código espagueti encapsulando la lógica en una clase pequeña
 */
export default {
  name: "MaterialInputCurreny",
  props: {
    variant: { type: String, default: "outline" },
    label: { type: String, default: "" },
    size: { type: String, default: "default" },
    success: { type: Boolean, default: false },
    error: { type: Boolean, default: false },
    disabled: { type: Boolean, default: false },
    name: { type: String, default: "" },
    id: { type: String, required: true },
    modelValue: { type: [String, Number], default: "" },
    placeholder: { type: String, default: "" },
    isRequired: { type: Boolean, default: false },
    maxLength: { type: Number, default: 20 },
    minLength: { type: Number, default: null },
    errorMessage: { type: String, default: "Este campo es obligatorio." },
  },
  emits: ["update:modelValue", "keyup"],
  data() {
    return {
      // internalRawValue guarda la representación sin formato (ej: "1234.56")
      internalRawValue: this.normalizeInitial(this.modelValue),
      isFocused: false,
      showError: false,
      localErrorMessage: this.errorMessage,
    };
  },
  computed: {
    // displayValue: si está enfocado muestra el raw para editar;
    // si no, muestra la versión formateada a moneda MXN.
    displayValue() {
      if (this.isFocused) return this.internalRawValue;
      if (this.internalRawValue === null || this.internalRawValue === "") return "";
      const n = Number(this.internalRawValue);
      if (Number.isNaN(n)) return this.internalRawValue;
      return CurrencyFormatter.format(n);
    },
  },
  watch: {
    // Si el padre cambia modelValue, actualizamos la representación interna
    modelValue(newVal) {
      this.internalRawValue = this.normalizeInitial(newVal);
    }
  },
  mounted() {
    setMaterialInput();
  },
  methods: {
    // Normaliza valores iniciales a string sin formato (por ejemplo: 123 -> "123.00")
    normalizeInitial(val) {
      if (val === null || val === undefined || val === "") return "";
      const num = Number(val);
      if (Number.isNaN(num)) return String(val);
      // Keep decimal precision as entered by parent if any, otherwise default to 2 decimals when formatting
      return String(num);
    },
    // Handler para eventos input: valida y mantiene sólo caracteres válidos
    onInput(e) {
      const raw = e.target.value;
      // sanitize mantiene sólo dígitos y separador decimal (. o ,) y evita múltiples separadores
      const sanitized = CurrencyFormatter.sanitizeForEditing(raw);
      this.internalRawValue = sanitized;
      // Emitimos el valor numérico (si se puede) para que el padre reciba Number
      const parsed = CurrencyFormatter.parseNumber(sanitized);
      this.$emit("update:modelValue", parsed === null ? "" : parsed);
      // visual: actualizamos el DOM para reflejar el sanitized (por si el browser no lo reemplaza)
      e.target.value = this.displayValue;
    },
    onFocus() {
      this.isFocused = true;
      // show raw without formatting to allow user editing
      this.$nextTick(() => {
        const input = this.$el && this.$el.querySelector ? this.$el.querySelector('input') : null;
        if (input) input.value = this.internalRawValue;
      });
    },
    onBlur() {
      this.isFocused = false;
      this.validateInput();
      // force formatting on blur (displayValue computed handles it)
      this.$nextTick(() => {
        const input = this.$el && this.$el.querySelector ? this.$el.querySelector('input') : null;
        if (input) input.value = this.displayValue;
      });
    },
    onKeyPress(e) {
      const key = e.key;
      // permitir control keys
      if (key.length > 1) return;
      // permitir dígitos, punto y coma decimal (coma), y evitar múltiples decimales
      const allowed = /[0-9.,]/;
      if (!allowed.test(key)) {
        e.preventDefault();
        return;
      }
      // evitar ingreso de más de un separador decimal
      if ((key === '.' || key === ',') && (this.internalRawValue.includes('.') || this.internalRawValue.includes(','))) {
        e.preventDefault();
      }
    },
    getClasses: (size) => {
      return size ? `form-control-${size}` : null;
    },
    getStatus: (error, success) => {
      if (success) return "is-valid";
      if (error) return "is-invalid";
      return null;
    },
    validateInput() {
      const val = (this.internalRawValue ?? "").toString().trim();
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
      this.showError = false;
    }
  }
};

/**
 * CurrencyFormatter
 * - Clase liviana para parsear/formatar y sanear entradas de usuario
 * - Encapsula el uso de Intl.NumberFormat para `es-MX`/`MXN`
 */
class CurrencyFormatter {
  static formatter = new Intl.NumberFormat('es-MX', {
    style: 'currency',
    currency: 'MXN',
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  });

  // Formatea Number -> string con símbolo y separadores según locale
  static format(value) {
    try {
      return this.formatter.format(Number(value));
    } catch (e) {
      return String(value);
    }
  }

  // Parsea string a Number. Retorna null si vacío o no parseable.
  static parseNumber(text) {
    if (text === null || text === undefined) return null;
    let t = String(text).trim();
    if (t === '') return null;
    // Remueve cualquier carácter que no sea dígito o . o , o -
    t = t.replace(/[^0-9.,-]/g, '');
    // Si existe coma como separador decimal y punto como miles, normalizar: quitar puntos y cambiar coma por punto
    const hasComma = t.indexOf(',') !== -1;
    const hasDot = t.indexOf('.') !== -1;
    if (hasComma && hasDot) {
      // Caso: "1.234,56" => quitar puntos (miles) y reemplazar coma por punto
      t = t.replace(/\./g, '').replace(/,/g, '.');
    } else if (hasComma && !hasDot) {
      // Caso: "1234,56" => convertir coma decimal a punto
      t = t.replace(/,/g, '.');
    } else {
      // Solo puntos o solo dígitos: dejar como está (punto es decimal)
      // También remueve cualquier otro punto extra que pueda actuar como thousand separator
      const parts = t.split('.');
      if (parts.length > 2) {
        // Asumir últimos dos son decimals
        const decimals = parts.pop();
        t = parts.join('') + '.' + decimals;
      }
    }
    const n = Number(t);
    if (Number.isNaN(n)) return null;
    return n;
  }

  // Sanitize input while editing: permite sólo una coma/punto decimal y dígitos
  static sanitizeForEditing(raw) {
    if (raw === null || raw === undefined) return '';
    let s = String(raw);
    // Remueve caracteres inválidos (permite dígitos, punto y coma)
    s = s.replace(/[^0-9.,-]/g, '');
    // Si hay más de un separador (punto o coma), eliminar los extras dejando la primera ocurrencia
    // Normal simpler: reemplazar múltiples comas/puntos por una sola
    s = s.replace(/,+/g, ',');
    s = s.replace(/\.+/g, '.');
    // Si existen ambos, keep as-is (parse logic manejará), pero evita tener múltiples de cada.
    // Evita cosas como ",." al inicio
    s = s.replace(/^[.,]+/, '');
    return s;
  }
}

</script>

<style scoped>
/* Reutiliza estilos base del MaterialInput para consistencia visual */
@keyframes onAutoFillStart { from { } to { } }
.form-control:-webkit-autofill { animation-name: onAutoFillStart; }
.form-control:-webkit-autofill { animation-duration: 0.01s; }
.input-group .form-control { transition: border-color 0.3s ease; }
.input-group .form-control.is-invalid { border-color: red !important; }
.input-group .form-control::placeholder { color: red; opacity: 1; font-size:16px; text-align: end; }
.input-group .form-control.is-valid { border-color: #C9A432 !important; }
.form-control { border: 2px solid #ccc !important; border-radius: 5px; background: #fff; }
.input-group .form-control:focus, .input-group .form-control:hover { border-color: #C9A432 !important; box-shadow: 0 0 0 2px rgba(0,123,255,0.15); }
/* Alinear a la derecha visualmente para valores numéricos (comodidad) */
.input-group input[type="text"] { text-align: right; }

</style>
