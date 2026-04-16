<template>
  <div class="container-fluid py-4">
    <div class="d-sm-flex justify-content-between">
      <div>
        <material-button v-permiso="'Roles.Agregar'" color="primary" variant="gradient" @click="navigateToCreate">
          Nuevo Rol
        </material-button>
      </div>
    </div>

    <!-- DataTable Component -->
    <DataTable
      title="Roles"
      description="Lista de Roles"
      table-id="employee-table"
      :columns="columns"
      :rows="rowsRoles"
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
          v-permiso="'Roles.Editar'"
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
import { useRolesStore } from "@mod1/modules/roles/store/useRolesStore";
import { storeToRefs } from "pinia";
import { onBeforeMount } from "vue";
import { useRouter } from "vue-router";

export default {
  name: "RolesList",
  components: {
    DataTable,
    MaterialButton,
  },
  setup() {
    const rolesStore = useRolesStore();
    const { rowsRoles, columns, loadingProgress } = storeToRefs(rolesStore);
    const router = useRouter();

    const navigateToCreate = () => {
      rolesStore.resetSelectedRol();
      router.push({ name: "RolesCreate" });
    };

    const handleUpdate = async (row) => {
      rolesStore.rol = { ...row };
      await rolesStore.fetchRolByID();
     
      router.push({ name: "RolesCreate" });
    };

    const handleDelete = (row) => {
      if (confirm(`¿Estás seguro de que deseas eliminar el role "${row.nombre}"?`)) {
        rolesStore.deleteGrupo(row.id);
      }
    };

    onBeforeMount(async () => {
      await rolesStore.fetchRoles();
    });

    return {
      rowsRoles,
      columns,
      loadingProgress,
      navigateToCreate,
      handleUpdate,
      handleDelete,
    };
  },
};
</script>