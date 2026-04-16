
<template>
    <div class="material-tag">
        <label :for="id" class="form-label">{{ label }}</label>
        <div class="tags-container">         
            <select :id="id" v-model="newTag" class="form-control-select" @change="addTag">
                <option disabled value="">{{ seleccionaPlaceholder }}</option>
                <option v-for="(option, index) in availableOptions" :key="index" :value="option">
                    {{ option.name }}
                </option>
            </select>
            <ul class="selected-tags">
                <li v-for="(tag, index) in selectedTags" :key="index" class="tag-item">
                    {{ tag.name }}
                    <button type="button" class="remove-tag" @click="removeTag(index)">
                        &times;
                    </button>
                </li>
            </ul>
        </div>
    </div>
</template>

<script>
import { defineComponent, ref, computed } from "vue";

export default defineComponent({
    name: "MaterialTags",
    props: {
        id: {
            type: String,
            required: true,
        },
        label: {
            type: String,
            default: "",
        },
        seleccionaPlaceholder: {
            type: String,
            default: "Selecciona una opción",
        },
        modelValue: {
            type: Array,
            default: () => [],
        },
        options: {
            type: Array,
            default: () => [],
        },
    },
    emits: ["update:modelValue"],
    setup(props, { emit }) {
        const newTag = ref("");

        const selectedTags = computed(() => props.modelValue || []);

        const availableOptions = computed(() =>
            props.options.filter((option) => {
                // If option has id, compare by id; otherwise compare by name (case-insensitive)
                return !selectedTags.value.some((tag) => {
                    if (!tag) return false;
                    if (tag.id != null && option.id != null) return tag.id === option.id;
                    const tagName = (tag.name || tag).toString().toLowerCase();
                    const optName = (option.name || option).toString().toLowerCase();
                    return tagName === optName;
                });
            })
        );

        const addTag = () => {
            if (!newTag.value) return;
            // normalize new value (could be object or string)
            const candidate = newTag.value;
            const exists = selectedTags.value.some((tag) => {
                if (!tag) return false;
                if (tag.id != null && candidate && candidate.id != null) return tag.id === candidate.id;
                const tagName = (tag.name || tag).toString().toLowerCase();
                const candName = (candidate.name || candidate).toString().toLowerCase();
                return tagName === candName;
            });
            if (!exists) {
                emit("update:modelValue", [...selectedTags.value, candidate]);
            }
            newTag.value = "";
        };

        const removeTag = (index) => {
            const updatedTags = [...selectedTags.value];
            updatedTags.splice(index, 1);
            emit("update:modelValue", updatedTags);
        };

        return {
            newTag,
            selectedTags,
            availableOptions,
            addTag,
            removeTag,
        };
    },
});
</script>

<style scoped>
.form-control-select {
    display: block;
    width: 100%;
    padding: 0.625rem 0.75rem;
    font-size: 14px;
    font-weight: 400;
    line-height: 1.5;
    color: #c2c3c4;
    background-color: #fff;
    background-clip: padding-box;
    border: 4px solid #ccc !important;
    border-radius: 0.375rem;
    transition: border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
    
}
/* placeholder color */
.form-control-select::placeholder {
    color: #6c757d;
    opacity: 1; /* Firefox */
}

.form-control {
  border: 4px solid #ccc !important;
  border-radius: 4px;
  background: #fff;
}

.form-control-select:focus {
    color: #495057;
    background-color: #fff;
    border-color: #80bdff;
    outline: 0;
    box-shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
}

.selected-tags {
    list-style: none;
    padding: 0;
    margin: 0;
    display: flex;
    flex-wrap: wrap;
}

.tag-item {
    background-color: #525151;
    border-radius: 4px;
    padding: 5px 10px;
    margin: 5px;
    display: flex;
    align-items: center;
    color: #fff;
    font-size: 14px;
}

.remove-tag {
    background: none;
    border: none;
    color: #ff0000;
    cursor: pointer;
    margin-left: 5px;
}

.remove-tag:hover {
    color: #a71d2a;
}
</style>