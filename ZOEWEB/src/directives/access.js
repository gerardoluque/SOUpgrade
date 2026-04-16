import { useMainStore } from '../store/useMainStore';

export default {
  mounted(el, binding) {
    const store = useMainStore();


    // Verifica si el store y userPermisos están definidos
    if(binding.value == null || binding.value == '') {
      el.disabled = false;
      return;
    }

    try {

          const modulo = binding.value.split('.')[0].replace(/\s+/g, '').toLowerCase();
          const accion = binding.value.split('.')[1];
          const permisoModulo = store.userPermisos?.find(x => x.nombreModulo.replace(/\s+/g, '').toLowerCase() == modulo);



          const acciones =  permisoModulo?.accion;
          let accionesObj = acciones;
          // Si acciones es un string, conviértelo a objeto
          if (typeof acciones === 'string') {
          try {
          accionesObj = JSON.parse(acciones);
          } catch (e) {
          accionesObj = [];
          }
          }

          // Busca el primer objeto (o recorre todos si necesitas)
          let permiso = false;
          if (Array.isArray(accionesObj) && accionesObj.length > 0) {
          // Toma el primer objeto del arreglo
          const obj = accionesObj[0];
          permiso = obj[accion]; // accede dinámicamente
          }



          if (permisoModulo != null && acciones != null && !permiso) {       
              el.disabled = true;
          } else {
              el.disabled = false;
          }
     
    
    }
    catch (error) {
      console.error('Error al procesar los permisos:', error);
      el.disabled = true; // Deshabilita el elemento si hay un error
      return;
    }

   
    
  }
};
