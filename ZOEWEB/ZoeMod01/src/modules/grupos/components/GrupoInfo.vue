<template>
  <div>
    <h4 class="mb-4">Información del Grupo</h4>
    <form>

      <MaterialInput id="grupo-name" v-model="grupo.nombre" label="Nombre"  :isRequired="true"
  
        placeholder="Ingrese una descripción"
      />
      <br />


      <MaterialInput id="grupo-description" v-model="grupo.descr" label="Descripción" 
      type="textarea" :minLength="5"
        :maxLength="200"
        lengthErrorMessage="La descripción debe tener entre 10 y 100 caracteres."
        placeholder="Ingrese una descripción"/>
      <br />

       <MaterialSwitch
        id="grupo-activo"
        name="grupo-activo"
        v-model:checked="grupo.activo"
        @change="handleSwitchChange"
      >
        Activo
      </MaterialSwitch>


      <MaterialTags id="group-users" v-model="grupo.usuarios" 
      label="Usuarios" :options="filteredUsers || []"
      
        seleccionaPlaceholder="Selecciona los usuarios" />

    </form>
  </div>
</template>

<script>
import MaterialInput from "@/components/common/MaterialInput.vue";
import MaterialTags from "@/components/common/MaterialTags.vue";
import MaterialSwitch from "@/components/common/MaterialSwitch.vue";
import { useGruposStore } from "@mod1/modules/grupos/store/useGruposStore";
import { storeToRefs } from "pinia";
import { onMounted } from "vue";

export default {
  name: "GrupoInfo",
  components: {
    MaterialInput,
    MaterialTags,
    MaterialSwitch
  },
  setup() {
    const gruposStore = useGruposStore();
    const { grupo, users, availableUsers } = storeToRefs(gruposStore);    

    
   const filteredUsers = 
    users.value.filter(
    user =>
      user.grupoId === null ||
      user.grupoId === '' ||
      user.grupoId === grupo.value.id
  );

  
    const handleSwitchChange = ({ name, checked }) => {
      if (name === "rol-activo") {
        grupo.IsEnable = checked;
     
      }
    };
    

    onMounted(async () => {

      await gruposStore.fetchUsers();

    });



    return { grupo, users, availableUsers,handleSwitchChange,filteredUsers };
  },
};
</script>
