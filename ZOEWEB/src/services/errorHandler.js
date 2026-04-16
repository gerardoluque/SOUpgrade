/**
 * Maneja errores de API y otros errores.
 * @param {Function} apiCall - La función que realiza la llamada a la API.
 * @returns {Promise<any>} - La respuesta de la API o un error manejado.
 */
export async function handleApiError(apiCall) {
    try {
      return await apiCall();
    } catch (error) {
      console.error("Error capturado:", error);
  
      // Personaliza el mensaje de error según el tipo de error
      if (error.response) {
        // Errores de respuesta del servidor
        throw new Error(`Error del servidor: ${error.response.statusText}`);
      } else if (error.request) {
        // Errores de red
        throw new Error("Error de red: No se pudo conectar con el servidor.");
      } else {
        // Otros errores
        throw new Error(error.message || "Ocurrió un error inesperado.");
      }
    }
  }