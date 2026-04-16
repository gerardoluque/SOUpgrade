// filepath: c:\Users\DELL\source\repos\BaseWebApp\ZOE-WEB\src\services\apiHeaders.js
import { msalInstance } from "@/authConfig";
import localStorageService from "@/utils/localStorageService";

/**
 * Obtiene el token de acceso desde Azure AD.
 */
async function getAccessToken() {
   
  if(localStorageService.get("isSystemUser"))
  {
       return localStorageService.get("externalUser").token;  
  }
  else
  {
      const account = msalInstance.getActiveAccount();
      if (!account) {
        // No active account: return null instead of throwing so callers can decide
        // This prevents errors when the user logs out and background requests try to run.
        return null;
      }

      const tokenResponse = await msalInstance.acquireTokenSilent({
        scopes: [process.env.VUE_APP_SCOPE_API],
        account: account,
      });

      return tokenResponse.accessToken;

  }



}

/**
 * Genera los encabezados para las solicitudes API.
 */
export async function getApiHeaders(anonimus = false) {

if(!anonimus) {

  const accessToken = await getAccessToken();
  // Try to include selected SistemaId if present (set on route selection)
  const sistemaId = localStorageService.get("sistemaSelectedId") ?? 1;
  const headers = {
    "Content-Type": "application/json",
    "Ocp-Apim-Subscription-Key": process.env.VUE_APP_OCP_KEY,
    ...(sistemaId !== null && sistemaId !== undefined && sistemaId !== "" ? { "SistemaId": sistemaId } : {}),
  };
  if (accessToken) {
    headers.Authorization = `Bearer ${accessToken}`;
  }
  return headers;

  }

  else
  {
return {
    "Content-Type": "application/json",
  };

}
}
export async function getApiHeadersWithCorporation() {
  const accessToken = await getAccessToken();

  // Prefer the stored selected ID when present
  let corpId = localStorageService.get("coporacionSelectedId") || null;
  if (!corpId) {
    const coporacionSelected = localStorageService.get("coporacionSelected");
    const corporaciones = localStorageService.get("corporaciones") || [];
    const match = Array.isArray(corporaciones)
      ? corporaciones.find((g) => g && g.nombre === coporacionSelected)
      : null;
    corpId = match && match.id ? match.id : null;
  }

  const headers = {
    "Content-Type": "application/json",
    "Ocp-Apim-Subscription-Key": process.env.VUE_APP_OCP_KEY,
  };
  if (accessToken) {
    headers.Authorization = `Bearer ${accessToken}`;
  }
  if (corpId !== null && corpId !== undefined && corpId !== "") {
    headers["CorporacionId"] = corpId;
  }
  // Append SistemaId if set by router on process selection
  const sistemaId = localStorageService.get("sistemaSelectedId");
  if (sistemaId !== null && sistemaId !== undefined && sistemaId !== "") {
    headers["SistemaId"] = sistemaId;
  }
  return headers;

}