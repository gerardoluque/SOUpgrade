// composables/useCatalogs.js
import { computed } from 'vue'
import { useCatalogStore } from '@/store/useCatalogStore'

export function useCatalogs() {
  const store = useCatalogStore()
  
  const loadCatalog = async (api,catalogName) => {
    try {
      return await store.getCatalog(api,catalogName)
    } catch (error) {
      console.error(`Error loading catalog ${catalogName}:`, error)
      throw error
    }
  }

  return {
    loadCatalog,
    isLoading: computed(() => store.loading),
    catalogError: computed(() => store.error)
  }
}