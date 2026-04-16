 import { defineStore } from 'pinia'
import { ref } from 'vue'
import { fetchCatalog } from '@/services/catalogService'

export const useCatalogStore = defineStore('catalogs', () => {
  const catalogs = ref({})
  const loading = ref(false)
  const error = ref(null)

  const getCatalog = async (api,catalogName) => {
    if (catalogs.value[catalogName]) {
      return catalogs.value[catalogName]
    }

    try {
      loading.value = true
      const data = await fetchCatalog(api,catalogName)
      catalogs.value[catalogName] = data
      return data
    } catch (err) {
      error.value = err
      throw err
    } finally {
      loading.value = false
    }
  }

  return { catalogs, loading, error, getCatalog }
})