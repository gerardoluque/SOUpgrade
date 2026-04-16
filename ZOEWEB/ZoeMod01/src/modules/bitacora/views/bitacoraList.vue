<template>
  <div class=" container-fluid">
    <div class=" row">
      <div class="col-12">
        <div class="card">
          <!-- Card Header: Title Section -->
          <div class="card-header title-section">
            <div>
              <h5 class="mb-0">Reporte de Bitácora</h5>
              <p class="mb-0 text-muted text-sm">Consulta los eventos registrados en el sistema</p>
            </div>
          </div>

          <!-- Card Header: Filters Section -->
          <div class="card-header filters-section">
            <!-- Start Date -->
            <div>
              <div class="filter-item">
                <label for="startDate" class="form-label">Fecha de Inicio</label>
                <MaterialInput id="startDate" type="date" v-model="bitacora.StartDate" @blur="formatStartDate" />
              </div>

              <!-- Users -->
              <div class="filter-item">
                <MaterialTags id="group-users" v-model="bitacora.UserIds" label="Usuarios" :options="users || []"
                  seleccionaPlaceholder="Selecciona los usuarios" />
              </div>

              <!-- End Date -->

            </div>
            <div class="filter-item">
              <label for="endDate" class="form-label">Fecha de Fin</label>
              <MaterialInput id="endDate" type="date" v-model="bitacora.EndDate" @blur="formatEndDate" />
            </div>

            <!-- Search Button -->
            <div class="filter-item button-container flex flex-row">
              <MaterialButton class="m-1" color="primary" size="md" variant="gradient"
               @click="fetchFilteredBitacora"
               v-permiso="'bitacoradeacceso.Consultar'"
               >
                Buscar
              </MaterialButton>

              <MaterialButton class="m-1" color="secondary" size="md" variant="gradient" @click="cleanBitacora">
                Limpiar Campos
              </MaterialButton>

            </div>

          </div>
        </div>
      </div>

      <!-- Data Table -->
      <DataTable title="Bitácora" description="Lista de Eventos" table-id="bitacora-table" :columns="columns"
        :rows="rowsBitacora" :searchable="true" :loadingProgress="loadingProgress" :exportar="true" 
        permisoExportar="bitacoradeacceso.Exportar" />
    </div>
  </div>
</template>

<script>
import DataTable from "@/components/widgets/DataTable.vue";
import MaterialInput from "@/components/common/MaterialInput.vue";
import MaterialButton from "@/components/common/MaterialButton.vue";
import MaterialTags from "@/components/common/MaterialTags.vue";
import { useBitacoraStore } from "@mod1/modules/bitacora/store/useBitacoraStore";
import { storeToRefs } from "pinia";
import { onMounted, ref } from "vue";

export default {
  name: "BitacoraList",
  components: {
    DataTable,
    MaterialInput,
    MaterialButton,
    MaterialTags,
  },
  setup() {
    const bitacoraStore = useBitacoraStore();
    const { rowsBitacora, columns, loadingProgress, users, bitacora } = storeToRefs(bitacoraStore);

    const formattedStartDate = ref("");
    const formattedEndDate = ref("");

    const formatStartDate = () => {
      if (formattedStartDate.value) {
        const [year, month, day] = formattedStartDate.value.split("-");
        formattedStartDate.value = `${day}/${month}/${year}`;
      }
    };

    const formatEndDate = () => {
      if (formattedEndDate.value) {
        const [year, month, day] = formattedEndDate.value.split("-");
        formattedEndDate.value = `${day}/${month}/${year}`;
      }
    };

    const fetchFilteredBitacora = async () => {
      await bitacoraStore.fetchBitacora();
    };
    const cleanBitacora = () => {
      bitacoraStore.cleanBitacora();
    };

    onMounted(async () => {
      await bitacoraStore.fetchUsers();
    });

    return {
      rowsBitacora,
      columns,
      loadingProgress,
      formattedStartDate,
      formattedEndDate,
      formatStartDate,
      formatEndDate,
      fetchFilteredBitacora,
      users,
      bitacora,
      cleanBitacora
    };
  },
};
</script>

<style scoped>
.card {
  border-radius: 10px;
  background-color: #ffffff;
  border: 1px solid #e3e6f0;
}

.card-header {
  padding: 1rem;
  border-bottom: 1px solid #e3e6f0;
}

.title-section {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.filters-section {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 1rem;
  padding: 1rem;
}

.filter-item {
  display: flex;
  flex-direction: column;
}

.button-container {
  display: flex;
  justify-content: flex-end;
  align-items: flex-end;
}
</style>