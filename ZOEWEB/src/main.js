import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import { createPinia } from "pinia";

import "./assets/css/nucleo-icons.css";
import "./assets/css/nucleo-svg.css";
import "./assets/css/nucleo-variables.css";
import "./assets/css/monitor-events.css";
import VueSweetalert2 from "vue-sweetalert2";
import MaterialDashboard from "./material-dashboard";
import { initializeMsal } from "./authConfig";
import permisoDirective from './directives/access';
import uppercase from './directives/uppercase';

import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min.js";

const appInstance = createApp(App);
const pinia = createPinia();

// Inicializa MSAL antes de montar la aplicación
initializeMsal()
  .then(() => {
    console.log("MSAL initialized successfully.");
  })
  .catch(() => {
    console.warn("MSAL initialization failed. Proceeding without authentication.");
  })
  .finally(() => {
    // Utilities and Spanish defaults: formatters and messages
    const spanishUtils = {
      // Accepts Date, timestamp, or ISO string. Returns 'dd/MM/yyyy'
      formatDate(value) {
        if (!value) return '';
        const d = (value instanceof Date) ? value : new Date(value);
        if (Number.isNaN(d.getTime())) return '';
        return new Intl.DateTimeFormat('es-ES', { day: '2-digit', month: '2-digit', year: 'numeric' }).format(d);
      },
      // Returns 'dd/MM/yyyy HH:mm'
      formatDateTime(value) {
        if (!value) return '';

        let input = value;
        if (typeof value === 'string') {
          let raw = value.trim();

          // .NET style: /Date(1735689600000)/
          const dotNetMatch = /^\/Date\((\d+)([+-]\d{4})?\)\/$/.exec(raw);
          if (dotNetMatch) {
            input = Number(dotNetMatch[1]);
          } else if (/^\d{10}$/.test(raw)) {
            // Epoch seconds as string
            input = Number(raw) * 1000;
          } else if (/^\d{13}$/.test(raw)) {
            // Epoch millis as string
            input = Number(raw);
          } else {
            // Exact backend case without timezone, e.g. "2026-03-12T02:39:40.8851322"
            const utcNoTzMatch = /^(\d{4})-(\d{2})-(\d{2})[T\s](\d{2}):(\d{2})(?::(\d{2})(?:\.(\d{1,7}))?)?$/.exec(raw);
            if (utcNoTzMatch && !/(Z|[+-]\d{2}:?\d{2})$/i.test(raw)) {
              const year = Number(utcNoTzMatch[1]);
              const month = Number(utcNoTzMatch[2]);
              const day = Number(utcNoTzMatch[3]);
              const hour = Number(utcNoTzMatch[4]);
              const minute = Number(utcNoTzMatch[5]);
              const second = Number(utcNoTzMatch[6] || 0);
              const fraction = String(utcNoTzMatch[7] || '').padEnd(3, '0').slice(0, 3);
              const millisecond = Number(fraction || 0);
              input = new Date(Date.UTC(year, month - 1, day, hour, minute, second, millisecond));
            } else {
            // Normalize common backend datetime variants
            // Example: "2026-03-11 18:20:30.0000000 +00:00" -> "2026-03-11T18:20:30.000+00:00"
            raw = raw.replace(/\s+(?=[+-]\d{2}:?\d{2}$)/, '');

            const hasTimezone = /(Z|[+-]\d{2}:?\d{2})$/i.test(raw);
            const isoDateTimeNoTz = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}(:\d{2}(\.\d{1,7})?)?$/i.test(raw);
            const sqlDateTimeNoTz = /^\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}(:\d{2}(\.\d{1,7})?)?$/i.test(raw);

            // Backend can return UTC without timezone suffix; force UTC in that case.
            const trimFraction = (text) =>
              text.replace(/\.(\d{3})\d+(?=(Z|[+-]\d{2}:?\d{2})?$)/i, '.$1');

            const normalized = raw.replace(' ', 'T');
            if (!hasTimezone && (isoDateTimeNoTz || sqlDateTimeNoTz)) {
              input = trimFraction(`${normalized}Z`);
            } else {
              input = trimFraction(normalized);
            }
            }
          }
        }

        const d = (input instanceof Date) ? input : new Date(input);
        if (Number.isNaN(d.getTime())) return '';
        const date = new Intl.DateTimeFormat('es-ES', { day: '2-digit', month: '2-digit', year: 'numeric' }).format(d);
        const time = new Intl.DateTimeFormat('es-ES', { hour: '2-digit', minute: '2-digit', hour12: false }).format(d);
        return `${date} ${time}`;
      },
      // Parse an ISO-like date (YYYY-MM-DD or ISO) and return Date object or null
      parseIso(value) {
        if (!value) return null;
        try {
          const d = new Date(value);
          return Number.isNaN(d.getTime()) ? null : d;
        } catch (e) {
          return null;
        }
      }
    };

    const spanishMessages = {
      ok: 'Aceptar',
      cancel: 'Cancelar',
      required: 'Este campo es obligatorio.',
      invalid_email: 'Correo electrónico inválido.',
      invalid_curp: 'CURP inválida.',
      invalid_rfc: 'RFC inválido.'
    };

    // Register globals for easy use in components: $fmt and $msg
    appInstance.config.globalProperties.$fmt = spanishUtils;
    appInstance.config.globalProperties.$msg = spanishMessages;

    appInstance.use(pinia);
    appInstance.use(router);
    // Provide Spanish defaults to SweetAlert2
    appInstance.use(VueSweetalert2, { confirmButtonText: spanishMessages.ok, cancelButtonText: spanishMessages.cancel });
    appInstance.use(MaterialDashboard);
    appInstance.directive("permiso",permisoDirective);
    appInstance.directive('uppercase', uppercase);
    appInstance.mount("#app");
  });