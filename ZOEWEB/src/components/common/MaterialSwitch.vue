<template>
  <div class="form-check form-switch d-flex">
    <input
      :id="id"
      v-permiso="permiso"
      class="form-check-input"
      :class="$attrs.class"
      type="checkbox"
      :name="name"
      :checked="checked"
      @change="handleChange"
    />
    <label class="form-check-label ms-3" :class="labelClass" :for="id">
      <slot />
    </label>
  </div>
</template>

<script>
import { defineComponent } from "vue";

export default defineComponent({
  name: "MaterialSwitch",
  props: {
    name: {
      type: String,
      required: true,
    },
    id: {
      type: String,
      required: true,
    },
    checked: {
      type: Boolean,
      default: false,
    },
    labelClass: {
      type: String,
      default: "",
    },
    permiso: {
      type: String,
      default: "",
    },
  },
  setup(props, { emit }) {
    const handleChange = (event) => {
      const isChecked = event.target.checked;
      emit("update:checked", isChecked); // Emit the updated value
      emit("change", { name: props.name, checked: isChecked }); // Emit a generic change event
    };

    return {
      handleChange,
    };
  },
});
</script>