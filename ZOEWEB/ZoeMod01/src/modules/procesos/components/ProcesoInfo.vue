
<template>
  <div>
    <h4 class="mb-4">Información del Proceso</h4>
    <form>

      <MaterialInput id="proceso-descr" v-model="selectedProceso.nombre" label="Nombre" 
       :disabled="selectedProceso.tipo === 'PROCESO'"
      />
      <br />

      <!-- Sistema -->
      <MaterialComboBox
        id="proceso-sistema"
        label="Sistema"
        v-model="selectedProceso.sistema"
        :options="sistemasOptions"
        seleccionaPlaceholder="Selecciona el sistema"
      />
      <br />


      
      <label for="accountEnabled" class="form-label">Activo</label>
      <material-switch id="accountEnabled" name="accountEnabled" 
       v-model:checked="selectedProceso.activo"
      
      />
                                
       

      <br />

      <MaterialInput id="proceso-icono" v-model="selectedProceso.icono" label="icono" type="textarea" 
      
      />
      <br />
      <MaterialInput id="proceso-ruta" v-model="selectedProceso.ruta" label="ruta" type="textarea"
     
      />
      <br />
 <div class="form-grid">
<div class="form-group">
   <label for="proceso-tipo" class="form-label">Tipo de Proceso</label>
              <select
                id="proceso-tipo"
                class="form-control"                
                v-model="selectedProceso.tipo"
                :disabled="selectedProceso.tipo === 'PROCESO'"
              >
                <option>AGRUPADOR</option>
                <option v-if="selectedProceso.id && selectedProceso.id !== 0">PROCESO</option>
              </select>
    </div>
<div class="form-group">
      <MaterialTags 
       v-if="selectedProceso.tipo != null && selectedProceso.tipo != '' && selectedProceso.tipo === 'AGRUPADOR'"
        id="proceso-users" 
        v-model="selectedSubprocesos" 
        label="Sub Procesos" 
        :options="listaProceso.filter((proc) => proc.tipo === 'PROCESO' && proc.activo === true  ) || []"
        seleccionaPlaceholder="Selecciona los procesos" />

        <label  v-if="selectedProceso.tipo != null && selectedProceso.tipo != '' &&  selectedProceso.tipo != 'AGRUPADOR'" for="proceso-tipo" class="form-label">Proceso Padre</label>
              <select v-if="selectedProceso.tipo != null && selectedProceso.tipo != '' && selectedProceso.tipo != 'AGRUPADOR'"
                id="procesoPadre"
                class="form-control"                
                v-model="selectedProceso.procesoPadreId"
              >
              <option v-for="row in this.listaProceso.filter((proc) => proc.tipo === 'AGRUPADOR' && proc.activo === true)" :key="row.id" :value="row.id">
                {{ row.name }}
              </option>
              </select>
      </div>
    
 
 </div>
    

    </form>
  </div>
</template>

<script>
import MaterialInput from "@/components/common/MaterialInput.vue";
 import MaterialTags from "@/components/common/MaterialTags.vue";
import { useProcesoStore } from "@mod1/modules/procesos/store/useProcesoStore";
import { storeToRefs } from "pinia";
import { ref, computed } from "vue";
import MaterialSwitch from "@/components/common/MaterialSwitch.vue";
import MaterialComboBox from "@/components/common/MaterialComboBox.vue";
import { getSistemas } from "@mod1/services/sistemaDbService.js";


export default {
  name: "ProcesoInfo",
  components: {
    MaterialInput,
    MaterialTags,
    
    MaterialSwitch,
    MaterialComboBox
  },     
  setup() {
    const procesoStore = useProcesoStore();
  const { proceso,selectedProceso,listaProceso } = storeToRefs(procesoStore);
  const sistemasOptions = getSistemas();
    const selectedPermissionType = ref("AGRUPADOR");


 
    const selectedListadoAcciones = computed({
      get() {
        return (selectedProceso.value.acciones || []).map((val) => ({
          id: val.id,
          name: val.descr,
        }));
      },
        set(newValue) {
        selectedProceso.value.acciones = newValue.map((val) => ({
          id: val.id,
          descr: val.name,
        }));
      },
    });

     const selectedSubprocesos = computed({
      get() {
        return (selectedProceso.value.subprocesos || []).map((val) => ({
          id: val.id,
          name: val.descr,
        }));
      },
        set(newValue) {
        selectedProceso.value.subprocesos = newValue.map((val) => ({
          id: val.id,
          descr: val.name,
        }));
      },
    });

    
    const filteredSubprocessOptions = computed(() =>
      listaProceso.value.filter((proc) => proc.tipo === "P" && proc.activo === true)
    );


    



    

    return { 
      proceso,selectedPermissionType,selectedProceso,listaProceso,
 selectedSubprocesos,
      filteredSubprocessOptions,
      selectedListadoAcciones,
      sistemasOptions

    };
  },
};
</script>

<style scoped>
.form-control {
  border: 4px solid #ccc !important;
  border-radius: 4px;
  background: #fff;
}

</style>