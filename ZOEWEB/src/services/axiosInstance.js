// src/services/axiosInstance.js
import axios from 'axios'
import { getApiHeaders } from './apiHeaders'
import localStorageService from '@/utils/localStorageService'

const API_BASE_URL = process.env.VUE_APP_API_BASE_URL

// Small helper to decode JWT payload (top-level to satisfy ESLint no-inner-declarations)
function jwtDecode(token) {
  try {
    const parts = token.split('.');
    if (parts.length < 2) return null;
    const payload = parts[1].replace(/-/g, '+').replace(/_/g, '/');
    const padded = payload.padEnd(payload.length + (4 - (payload.length % 4)) % 4, '=');
    const json = atob(padded);
    return JSON.parse(json);
  } catch (e) {
    return null;
  }
}

const axiosInstance = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json'
  }
})

     

// Interceptor para añadir headers dinámicos (como tokens)
axiosInstance.interceptors.request.use(async config => 
  {
    const headers = await getApiHeaders(config.anonimus);
    
     
     
    const merged = { ...config.headers, ...headers };
    if (config.nocontentType) {
      // Remove any preset content-type so the browser can set correct multipart boundaries
      delete merged['Content-Type'];
      delete merged['content-type'];
    }
    config.headers = merged;
  return config
}, error => Promise.reject(error))

// Response interceptor: attempt token refresh on 403 (Forbidden)
axiosInstance.interceptors.response.use(
  response => response,
  async error => {
    try {

      console.log('axiosInstance response interceptor caught error:', error);
      const originalRequest = error.config;
      if (!originalRequest) return Promise.reject(error);

      const status = error.response?.status;
      console.log('axiosInstance response interceptor caught error, status:', status);
      if ((status === 401) && !originalRequest._retry) {
        // Only attempt refresh when the token is actually expired per stored expiration
        const stored = localStorageService.get('externalUser') || {};
        const tokenExpires = stored.tokenExpires || null;

        // If we don't have tokenExpires, do not attempt refresh here (require explicit expiration)
        if (tokenExpires && Date.now() <= tokenExpires) {
          // token still valid according to expiration claim; don't refresh
          return Promise.reject(error);
        }

        console.log('axiosInstance response interceptor attempting token refresh');
        originalRequest._retry = true;
        const refreshToken = stored.refreshToken;
        if (!refreshToken) {
          // No refresh token available, force logout by rejecting
          return Promise.reject(error);
        }

        

        // Call refresh endpoint directly with axios (no interceptors)
        const refreshUrl = `${API_BASE_URL.replace(/\/$/, '')}/auth/refresh-token`;
        try {
          // send both refreshToken and current access token as API expects
          const refreshResp = await axios.post(refreshUrl, { refreshToken, accessToken: stored.token });
          const newToken = refreshResp.data?.token || refreshResp.data?.accessToken || null;
          const newRefresh = refreshResp.data?.refreshToken || null;
          if (newToken) {
            // update stored tokens and expiration
            const updated = { ...(stored || {}), token: newToken };
            if (newRefresh) updated.refreshToken = newRefresh;
            const payload = jwtDecode(newToken);
            if (payload && payload.exp) {
              updated.tokenExpires = payload.exp * 1000;
            }
            localStorageService.set('externalUser', updated);

            // set Authorization header for the original request and retry
            originalRequest.headers = originalRequest.headers || {};
            originalRequest.headers.Authorization = `Bearer ${newToken}`;
            return axiosInstance(originalRequest);
          }
        } catch (refreshError) {
          // Refresh failed - clear stored auth and reject
          localStorageService.remove('externalUser');
          return Promise.reject(refreshError);
        }
      }
    } catch (e) {
      console.error('axiosInstance response interceptor error', e);
    }
    return Promise.reject(error);
  }
);

export default axiosInstance
