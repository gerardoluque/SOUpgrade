import { useMainStore } from '../store/useMainStore';



export   function getAccess(permiso) {
  
    const store = useMainStore();

   // let permiso = false;

    try {
            const modulo = permiso.value.split('.')[0].replace(/\s+/g, '').toLowerCase();
            const accion = permiso.value.split('.')[1];
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

            if (Array.isArray(accionesObj) && accionesObj.length > 0) {
            // Toma el primer objeto del arreglo
            const obj = accionesObj[0];
            permiso = obj[accion]; // accede dinámicamente
            }
    }
    catch (error) {
      console.error('Error al procesar los permisos:', error);
      return false; // Retorna false si hay un error
    }
    



  return permiso;


}