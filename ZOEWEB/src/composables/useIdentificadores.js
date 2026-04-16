import { computed, watch } from 'vue';

// Regex oficiales para CURP y RFC (persona física o moral)
const CURP_REGEX = /^[A-Z]{1}[AEIOUX]{1}[A-Z]{2}\d{2}(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01])[HM](AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[RO]|S[PLR]|T[CSL]|VZ|YN|ZS)([B-DF-HJ-NP-TV-Z]{3})[A-Z0-9]{1}\d{1}$/;
const RFC_REGEX  = /^([A-ZÑ&]{3}|[A-ZÑ&]{4})\d{6}[A-Z0-9]{3}$/;

/**
 * Centraliza validación y normalización de CURP / RFC.
 * @param {Ref|ComputedRef} curpRef referencia (get/set) al valor de CURP
 * @param {Ref|ComputedRef} rfcRef referencia (get/set) al valor de RFC
 * @param {{requireBoth?: boolean}} options requireBoth: si ambos deben ser válidos para canGuardar
 */
export function useIdentificadores(curpRef, rfcRef, options = {}) {
  const { requireBoth = true } = options;

  const curpValida = computed(() => {
    const v = (curpRef.value || '').toUpperCase().trim();
    if (!v) return false;
    if (v.length !== 18) return false;
    return CURP_REGEX.test(v);
  });

  const rfcValido = computed(() => {
    const v = (rfcRef.value || '').toUpperCase().trim();
    if (!v) return false;
    if (v.length !== 12 && v.length !== 13) return false;
    return RFC_REGEX.test(v);
  });

  // Normalización (uppercase + corte de longitud) usando watchers centralizados
  watch(() => curpRef.value, (v) => {
    if (typeof v !== 'string') return;
    let val = v.toUpperCase();
    if (val.length > 18) val = val.slice(0, 18);
    if (val !== curpRef.value) curpRef.value = val; // evita loop si no cambió
  });

  watch(() => rfcRef.value, (v) => {
    if (typeof v !== 'string') return;
    let val = v.toUpperCase();
    if (val.length > 13) val = val.slice(0, 13);
    if (val !== rfcRef.value) rfcRef.value = val;
  });

  const canGuardar = computed(() => {
    if (requireBoth) return curpValida.value && rfcValido.value;
    return curpValida.value || rfcValido.value; // alternativa si en algún formulario se quiere permitir uno de los dos
  });

  return { curpValida, rfcValido, canGuardar };
}

export default useIdentificadores;