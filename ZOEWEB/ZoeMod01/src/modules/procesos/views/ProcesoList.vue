<template>
  <div class="container-fluid py-4">
    <div class="d-sm-flex justify-content-between">
      <div>
        <material-button v-permiso="'Proceso.Agregar'" color="primary" variant="gradient" @click="navigateToCreate">
          Nuevo Proceso
        </material-button>
      </div>
    </div>

     

    <!-- DataTable Component -->
    <DataTable
      title="Proceso"
      description="Lista de Proceso"
      table-id="employee-table"
      :columns="columns"
      :rows="rowsProceso"
      :searchable="true"
      :loadingProgress="loadingProgress"
    >
      <!-- Custom Row Actions -->
      <template #row-actions="{ row }">
        <div>
          
        </div>
        
      

        <material-button
         color="primary" 
         variant="gradient"
          size="sm"
          @click="handleUpdate(row)"
          class="me-3"
          v-permiso="'Proceso.Editar'"
        >
          Actualizar
        </material-button>   
        
       
      </template>
    </DataTable>
  </div>
</template>

<script>
import DataTable from "@/components/widgets/DataTable.vue";
import MaterialButton from "@/components/common/MaterialButton.vue";
import { useProcesoStore } from "@mod1/modules/procesos/store/useProcesoStore";
import { storeToRefs } from "pinia";
import { onBeforeMount } from "vue";
import { useRouter } from "vue-router";



export default {
  name: "ProcesoList",
  components: {
    DataTable,
    MaterialButton,
  },
  setup() {
    const procesoStore = useProcesoStore();
    const { rowsProceso, columns, loadingProgress,proceso } = storeToRefs(procesoStore);
    const router = useRouter();

    const navigateToCreate = () => {
      procesoStore.resetSelected();
      router.push({ name: "ProcesoCreate" });
    };

    const handleUpdate = (row) => {

       const subprocesos = Array.isArray(proceso.value)
    ? proceso.value.find((g) => g.id === row.id)?.subprocesos || []
    : [];

      procesoStore.selectedProceso = {

          id: row.id,
          nombre: row.nombre,
          tipo: row.tipo,
          icono: row.icono,
          activo: row.activo,
          ruta: row.ruta,      
          procesoPadreId: row.procesoPadreId,   
          subprocesos: subprocesos,


        };
      procesoStore.proceso = { ...row };
     
      router.push({ name: "ProcesoCreate" });
    };

    

    onBeforeMount(async () => {
      await procesoStore.fetchProceso();
    });

    return {
      rowsProceso,
      columns,
      loadingProgress,
      navigateToCreate,
      handleUpdate,
      proceso
     
    };
  },
};
</script>