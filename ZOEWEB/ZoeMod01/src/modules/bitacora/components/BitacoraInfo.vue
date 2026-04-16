<template>
  <div>
    <!-- <form>
      <MaterialInput
        v-model="bitacora.userName"
        label="Nombre de Usuario"
      />
      <MaterialInput
        v-model="bitacora.eventType"
        label="Tipo de Evento"
      />
      <MaterialInput
        v-model="bitacora.timestamp"
        label="Fecha y Hora"
        type="datetime-local"
      />
    </form> -->
  </div>
</template>

<script>

import { useBitacoraStore } from "@mod1/modules/bitacora/store/useBitacoraStore";   
import { storeToRefs } from "pinia";

export default {
  name: "BitacoraInfo",
  components: {
   
  },
  setup() {
    const bitacoraStore = useBitacoraStore();
    const { bitacora } = storeToRefs(bitacoraStore);

    return {bitacora};
  }
};
</script>