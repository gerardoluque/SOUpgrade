<template>
  <div>
    <h4 class="mb-4">Información del Rol</h4>
    <form>
      <!-- Nombre del Rol -->
      <MaterialInput
        id="rol-name"
        label="Nombre del Rol"
        v-model="rol.name"
        :isRequired="true"
        placeholder="Ingrese el nombre del rol"
         @input="filterNumbers"
      />
      <br />

      <!-- Tipo Rol -->
      <MaterialComboBox
        id="rol-tipo"
        label="Tipo Rol"
        v-model="rol.tipoRol"
        :options="tipoRolesOptions"
        seleccionaPlaceholder="Selecciona el tipo de rol"
      />
      <br />

      <!-- Descripción -->
      <MaterialInput
        id="rol-description"
        label="Descripción"
        v-model="rol.descripcion"
        type="textarea"
      />
      <br />

    
      <MaterialSwitch
        id="rol-activo"
        name="rol-activo"
        v-model:checked="rol.activo"
        @change="handleSwitchChange"
      >
        Activo
      </MaterialSwitch>

    
      <MaterialTags id="rol-procesos" v-model="rol.procesos" label="Procesos" :options="procesos || []"
      seleccionaPlaceholder="Selecciona los procesos" />
    </form>
  </div>
</template>

<script>
import MaterialInput from "@/components/common/MaterialInput.vue";
import MaterialSwitch from "@/components/common/MaterialSwitch.vue";
import MaterialTags from "@/components/common/MaterialTags.vue";
import MaterialComboBox from "@/components/common/MaterialComboBox.vue";
import { useRolesStore } from "@mod1/modules/roles/store/useRolesStore";
import { storeToRefs } from "pinia";
import { onMounted } from "vue";
import { TipoRolesOptions } from '@/enums/enums.js'

export default {
  name: "RolInfo",
  components: {
    MaterialInput,
    MaterialTags,
    MaterialSwitch,
    MaterialComboBox
  },
  setup() {
    const rolesStore = useRolesStore();
  const { rol, procesos } = storeToRefs(rolesStore);
  const tipoRolesOptions = TipoRolesOptions;
   
  
    const handleSwitchChange = ({ name, checked }) => {
      if (name === "rol-activo") {
        rol.value.activo = checked; // Update the rol object directly
     
      }
    };

    // Fetch procesos when the component is mounted
    onMounted(async () => {
      await rolesStore.fetchProcesos();
    });

    const filterNumbers = (event) => {
      const input = event.target.value;
       // Remove all numbers and characters except letters
       rol.value.name = input.replace(/[^a-zA-Z\s]/g, ""); // Remove all numbers and special characters

    };

    return { rol, procesos, tipoRolesOptions, handleSwitchChange, filterNumbers };
  },
};
</script>