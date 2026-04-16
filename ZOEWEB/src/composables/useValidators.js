// useMexValidators.js
export function useMexValidators() {
  // CURP: 18 caracteres (formato y fecha válidos)
  const CURP_REGEX =
    /^[A-Z][AEIOUX][A-Z]{2}\d{2}(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01])[HM](AS|BC|BS|CC|CL|CM|CS|CH|DF|DG|GT|GR|HG|JC|MC|MN|MS|NT|NL|OC|PL|QT|QR|SP|SL|SR|TC|TS|TL|VZ|YN|ZS|NE)[B-DF-HJ-NP-TV-Z]{3}[A-Z0-9]\d$/;

  // RFC: moral (12) o física (13) — validamos estructura y fecha YYMMDD
  const RFC_REGEX =
    /^[A-ZÑ&]{3,4}\d{2}(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01])[A-Z0-9]{3}$/;

  function isValidCURP(curp) {
    if (!curp) return false;
    curp = String(curp).toUpperCase().trim();
    if (!CURP_REGEX.test(curp)) return false;
    const yy = curp.slice(4, 6);
    const mm = curp.slice(6, 8);
    const dd = curp.slice(8, 10);
    return isValidYYMMDD(yy, mm, dd);
  }

  function isValidRFC(rfc) {
    if (!rfc) return false;
    rfc = String(rfc).toUpperCase().trim();
    if (!RFC_REGEX.test(rfc)) return false;
    const offset = rfc.length === 12 ? 3 : 4; // moral vs física
    const yy = rfc.slice(offset, offset + 2);
    const mm = rfc.slice(offset + 2, offset + 4);
    const dd = rfc.slice(offset + 4, offset + 6);
    return isValidYYMMDD(yy, mm, dd);
  }

  function isValidYYMMDD(yy, mm, dd) {
    const year = Number(yy);
    const month = Number(mm);
    const day = Number(dd);
    const fullYear = year + (year <= 49 ? 2000 : 1900);
    const dt = new Date(fullYear, month - 1, day);
    return (
      dt.getFullYear() === fullYear &&
      dt.getMonth() === month - 1 &&
      dt.getDate() === day
    );
  }

  return { isValidCURP, isValidRFC };
}
