<template>
  <div class="container-fluid py-4">
    <div class="d-sm-flex justify-content-between">
      <div>
        <material-button  v-permiso="'Grupos.Agregar'" color="primary" variant="gradient" @click="navigateToCreate">
          Nuevo Grupo
        </material-button>
      </div>
    </div>

    <!-- DataTable Component -->
    <DataTable
      title="Grupos"
      description="Lista de Grupos"
      table-id="employee-table"
      :columns="columns"
      :rows="rowsGrupos"
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
          class="me-2"
          v-permiso="'Grupos.Editar'"
        >
          Actualizar
        </material-button>
        <!-- <material-button
          color="danger"
          variant="gradient"
          size="sm"
          @click="handleDelete(row)"
        >
          Eliminar
        </material-button> -->
      </template>
    </DataTable>
  </div>
</template>

<script>
import DataTable from "@/components/widgets/DataTable.vue";
import MaterialButton from "@/components/common/MaterialButton.vue";
import { useGruposStore } from "@mod1/modules/grupos/store/useGruposStore";
import { storeToRefs } from "pinia";
import { onBeforeMount } from "vue";
import { useRouter } from "vue-router";

export default {
  name: "GruposList",
  components: {
    DataTable,
    MaterialButton,
  },
  setup() {
    const gruposStore = useGruposStore();
    const { rowsGrupos, columns, loadingProgress } = storeToRefs(gruposStore);
    const router = useRouter();

    const navigateToCreate = () => {
      gruposStore.resetSelectedGrupo();
      router.push({ name: "GruposCreate" });
    };

    const handleUpdate = async (row) => {
      gruposStore.grupo = { ...row };
      await gruposStore.fetchGrupoByID();
      router.push({ name: "GruposCreate" });
    };

    const handleDelete = (row) => {
      if (confirm(`¿Estás seguro de que deseas eliminar el grupo "${row.nombre}"?`)) {
        gruposStore.deleteGrupo(row.id);
      }
    };

    onBeforeMount(async () => {
      
      await gruposStore.fetchGrupos();
    });

    return {
      rowsGrupos,
      columns,
      loadingProgress,
      navigateToCreate,
      handleUpdate,
      handleDelete,
    };
  },
};
</script>