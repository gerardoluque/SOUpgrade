<template>
  <ul>
    <li v-for="item in items" :key="item.id">
      <div>
        <!-- Botón para expandir/colapsar -->
        <button
          v-if="item.tipo === 'A'"
          class="btn btn-sm btn-link p-0 me-2"
          @click="toggleVisibility(item.id)"
        >
          {{ isExpanded(item.id) ? "▼" : "▶" }}
        </button>

        <!-- Nombre del Agrupador o Proceso -->
        <span v-if="item.tipo === 'A'" class="text-primary font-weight-bold">
          {{ item.nombre }} (Agrupador)
        </span>
        <span v-else-if="item.tipo === 'P'" class="text-secondary">
          {{ item.nombre   }} 
        </span>
      </div>

      <!-- Renderiza subprocesos recursivamente -->
      <TreeView
        v-if="item.subprocesos && item.subprocesos.length && isExpanded(item.id)"
        :items="item.subprocesos"
      />
    </li>
  </ul>
</template>

<script>
import { ref } from "vue";

export default {
  name: "TreeView",
  props: {
    items: {
      type: Array,
      required: true,
    },
  },
  setup() {
    const expandedItems = ref(new Set()); // Almacena los IDs de los elementos expandidos

    const toggleVisibility = (id) => {
      if (expandedItems.value.has(id)) {
        expandedItems.value.delete(id); // Colapsa el elemento
      } else {
        expandedItems.value.add(id); // Expande el elemento
      }
    };

    const isExpanded = (id) => {
      return expandedItems.value.has(id); // Verifica si el elemento está expandido
    };

    return {
      toggleVisibility,
      isExpanded,
    };
  },
};
</script>

<style scoped>
.text-primary {
  color: #007bff;
}
.text-secondary {
  color: #6c757d;
}
.btn-link {
  text-decoration: none;
  color: inherit;
}
</style>