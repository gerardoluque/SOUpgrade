
<template>
    <div 
        class="input-group" 
        :class="[
            `input-group-${variant}`,
            'is-filled',
            { 'is-focused': isFocused }
        ]"
    >
        <label v-if="!hideLabel && label" :class="variant === 'static' ? '' : 'form-label'">{{ label }}</label>
        <select
            :id="id"
            class="form-control"
            :disabled="disabled"
            :value="selectedValue"
            @change="onChange"
            @focus="onFocus"
            @blur="onBlur"
        >
            <option disabled value="">{{ seleccionaPlaceholder }}</option>
            <option
                v-for="(option, index) in options"
                :key="index"
                :value="String(option?.value ?? option?.id ?? option?.name ?? '')"
            >
                {{ option.name ?? option?.label ?? option?.value ?? option?.id }}
            </option>
        </select>
    </div>
</template>

<script>
import { defineComponent, computed, ref } from "vue";

export default defineComponent({
    name: "MaterialComboBox",
    props: {
        id: { type: String, required: true },
        label: { type: String, default: "" },
        hideLabel: { type: Boolean, default: false },
        variant: { type: String, default: "outline" },
        seleccionaPlaceholder: { type: String, default: "Selecciona una opción" },
        modelValue: { type: [String, Number, Boolean, Object, null], default: null },
        options: { type: Array, default: () => [] },
        disabled: { type: Boolean, default: false },
        // Controla el tipo de dato que regresa: false -> string, true -> number
        numericValue: { type: Boolean, default: false },
    },
    emits: ["update:modelValue"],
    setup(props, { emit }) {
        const isFocused = ref(false);
        
        const selectedValue = computed(() => {
            const mv = props.modelValue
            if (mv === undefined || mv === null) return ''
            return String(mv)
        })

        const onChange = (e) => {
            const raw = e.target.value
            if (props.numericValue) {
                const n = Number(raw)
                emit('update:modelValue', Number.isNaN(n) ? raw : n)
            } else {
                emit('update:modelValue', raw)
            }
        }

        const onFocus = () => {
            isFocused.value = true;
        }

        const onBlur = () => {
            isFocused.value = false;
        }

        return { 
            selectedValue, 
            onChange, 
            onFocus, 
            onBlur, 
            isFocused 
        }
    }
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

/* Estilos para label visible y accesible - homogéneo con MaterialInput fecha/datetime */
.input-group.is-filled .form-label,
.input-group.is-focused .form-label {
    font-size: 14px !important;
    font-weight: 600 !important;
    color: #4a9d5f !important;
}
</style>