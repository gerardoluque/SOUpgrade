<template>
  <div class="container-fluid py-4">
    <div class="d-sm-flex justify-content-between">
      <div>
        <material-button v-permiso="'Usuarios.Agregar'" color="primary" variant="gradient" @click="navigateToCreate">
          Nuevo Usuario
        </material-button>
      </div>
    </div>

    
      <DataTable 
      title="Usuarios" 
      description="Lista de Usuarios" 
      table-id="employee-table" 
      :columns="columns" 
      :rows="rowUsers"
      :searchable="true" 
      :loadingProgress="loadingProgress" 
      >  
       
      <!-- Custom Row Actions -->
      <template #row-actions="{ row }">

      
        <material-button
        color="primary" 
        variant="gradient"
          size="sm"
          @click="handleUpdate(row)"
          class="me-2"
          v-permiso="'Usuarios.Editar'"
        >
          Actualizar
        </material-button>   
        
        

      </template>
      
     </DataTable>

 
  
   
  </div>
</template>

<script>

 import MaterialButton from "@/components/common/MaterialButton.vue";
import { useUsuarioStore } from "@mod1/modules/usuarios/store/useUsuarioStore";
import { storeToRefs } from "pinia";
import { onBeforeMount } from "vue";
import DataTable from "@/components/widgets/DataTable.vue";
import { useRouter } from "vue-router";


export default {
  name: "UsuarioList",
  components: {
    DataTable,
    MaterialButton, 
  },
  setup() {


    const usuarioStore = useUsuarioStore();
    const { rowUsers, columns, loadingProgress } = storeToRefs(usuarioStore);
    const router = useRouter();

    const navigateToCreate = () => {
      usuarioStore.resetSelected();
      router.push({ name: "UsuarioCreate" });
      };

      onBeforeMount(async () => {
        await usuarioStore.fetchUsers();
      });

      const handleUpdate = (row) => {
        usuarioStore.selecteduser  = { ...row };
        usuarioStore.getUserById(row.id);
        
      router.push({ name: "UsuarioCreate" });
    };

   
    return {
      rowUsers,
      columns,
      loadingProgress,
      navigateToCreate,
      handleUpdate,
     
    };
  },
};
</script>