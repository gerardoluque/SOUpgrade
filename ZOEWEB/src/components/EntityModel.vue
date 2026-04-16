<template>
  <div v-if="visible" class="entity-modal-backdrop" @click.self="handleCancel">
    <div ref="modalRef" class="entity-modal" role="dialog" aria-modal="true">
      <!-- Header -->
      <div class="entity-modal-header">
        <h5>{{ title }}</h5>
        <button class="close-btn" aria-label="Cerrar" @click="handleCancel">&times;</button>
      </div>

      <!-- Body: tu formulario va aquí vía slot -->
      <div class="entity-modal-body">
        <!-- Si el padre no pasa slot, mostramos un debug mínimo -->
        <slot name="form" :entity="entity" :set="setField">
          <pre style="font-size:12px; color:#555; background:#fafafa; padding:8px; border-radius:6px;">
{{ entity }}
          </pre>
        </slot>
      </div>

      <!-- Footer por defecto (puedes sobreescribir con slot "footer") -->
      <div class="entity-modal-footer">
        <slot name="footer" :on-save="handleSave" :on-cancel="handleCancel" :entity="entity" :set="setField">
          <button class="btn btn-secondary" type="button" @click="handleCancel">Cancelar</button>
          <button class="btn btn-primary" type="button" @click="handleSave">{{ submitText }}</button>
        </slot>
      </div>
    </div>
  </div>
</template>

<script setup>
import { reactive, watch,  ref, onMounted, onBeforeUnmount } from "vue";

// Props & Emits
const props = defineProps({
  // Control de visibilidad (soporta v-model:visible en el padre)
  visible: { type: Boolean, default: false },
  // Entidad inicial a editar (soporta v-model:modelValue)
  modelValue: { type: Object, default: () => ({}) },
  // Título y textos
  title: { type: String, default: "Editar registro" },
  submitText: { type: String, default: "Guardar" },
  // Cierra automáticamente al guardar
  autoCloseOnSave: { type: Boolean, default: true },
});

const emit = defineEmits([
  "update:visible",
  "update:modelValue",
  "save",     // payload: entidad
  "cancel",   // payload: entidad actual
]);

// ======== Estado local ========
// Clon simple (si tu entidad es muy compleja, cambia a structuredClone)
const clone = (obj) => JSON.parse(JSON.stringify(obj || {}));
const entity = reactive(clone(props.modelValue));

// ======== Integraciones UX ========
const modalRef = ref(null);

const lockScroll = (locked) => {
  if (typeof document !== "undefined") {
    document.body.style.overflow = locked ? "hidden" : "";
  }
};

const onEsc = (e) => {
  if (e.key === "Escape" && props.visible) handleCancel();
};

onMounted(() => document.addEventListener("keydown", onEsc));
onBeforeUnmount(() => {
  document.removeEventListener("keydown", onEsc);
  lockScroll(false);
});

// Cuando se abre, resetea el formulario local y bloquea scroll del body
watch(() => props.visible, (v) => {
  lockScroll(v);
  if (v) {
    Object.assign(entity, clone(props.modelValue));
    // foco al modal
    setTimeout(() => modalRef.value?.focus?.(), 0);
  }
});

// Si el padre cambia la entidad mientras está cerrado, sincroniza
watch(() => props.modelValue, (val) => {
  if (!props.visible) Object.assign(entity, clone(val));
}, { deep: true });

// ======== Helpers ========
function setField(path, value) {
  // Soporta 'a.b.c' como ruta
  const parts = path.split(".");
  let cur = entity;
  while (parts.length > 1) {
    const k = parts.shift();
    if (!(k in cur) || typeof cur[k] !== "object") cur[k] = {};
    cur = cur[k];
  }
  cur[parts[0]] = value;
}

function handleSave() {
  const payload = clone(entity);
  emit("update:modelValue", payload);
  emit("save", payload);
  if (props.autoCloseOnSave) emit("update:visible", false);
}

function handleCancel() {
  emit("cancel", clone(entity));
  emit("update:visible", false);
}
</script>

<style scoped>
.entity-modal-backdrop {
  position: fixed;
  inset: 0;
  background: rgba(0,0,0,0.35);
  z-index: 2000;
  display: flex;
  align-items: flex-start;
  justify-content: center;
}

.entity-modal {
  background: #fff;
  min-width: 60%;
  max-width: 96vw;
  max-height: 90vh;
  margin-top: 5vh;
  border-radius: 12px;
  box-shadow: 0 8px 32px rgba(0,0,0,0.18);
  display: flex;
  flex-direction: column;
  outline: none;
  animation: slideIn .18s ease-out;
}
@keyframes slideIn { from { transform: translateY(-24px) } to { transform: translateY(0) } }

.entity-modal-header {
  display: flex; align-items: center; justify-content: space-between;
  padding: 1rem 1.25rem; border-bottom: 1px solid #eee;
}
.entity-modal-body {
  padding: 1rem 1.25rem; overflow: auto;
}
.entity-modal-footer {
  padding: 0.8rem 1.25rem; border-top: 1px solid #eee;
  display: flex; gap: 0.75rem; justify-content: flex-end;
}
.close-btn { background: none; border: none; font-size: 1.6rem; line-height: 1; cursor: pointer; }

.btn { padding: .5rem .9rem; border-radius: 8px; border: 1px solid transparent; cursor: pointer; }
.btn-secondary { background: #f3f4f6; color: #111827; }
.btn-secondary:hover { background: #e5e7eb; }
.btn-primary { background: #2563eb; color: #fff; }
.btn-primary:hover { background: #1d4ed8; }
</style>
