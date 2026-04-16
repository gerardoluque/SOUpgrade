<template>
  <div v-if="visible" class="ppm-backdrop" @click.self="close">
    <div class="ppm-modal" role="dialog" aria-modal="true">
      <!-- Header -->
      <div class="ppm-header">
        <button v-if="step==='camera'" class="ppm-icon-btn" aria-label="Volver" @click="backToMenu">⟵</button>
        <h5 class="ppm-title">{{ title }}</h5>
        <button class="ppm-icon-btn" aria-label="Cerrar" @click="close">&times;</button>
      </div>

      <!-- Body -->
      <div class="ppm-body">
        <!-- STEP: MENU -->
        <div v-if="step==='menu'" class="ppm-menu">
          <button class="ppm-action" type="button" @click="triggerFile">
            📁 Subir foto
          </button>
          <button class="ppm-action" type="button" @click="openCamera">
            📷 Tomar foto
          </button>

          <p class="ppm-hint">
            Puedes subir una imagen desde tu dispositivo o tomar una foto con la cámara.
          </p>
        </div>

        <!-- STEP: CAMERA -->
        <div v-else-if="step==='camera'">
          <div class="ppm-video-wrap">
            <video ref="videoEl" autoplay playsinline></video>
            <div v-if="showGrid" class="ppm-grid"></div>
          </div>
          <div class="ppm-controls">
            <button class="ppm-btn" :disabled="!canSwitch" @click="switchCamera">🔄 Cambiar cámara</button>
            <button class="ppm-btn ppm-primary" :disabled="!streamActive" @click="capture">📸 Capturar</button>
          </div>
        </div>
      </div>

      <!-- Footer -->
      <div class="ppm-footer">
        <button class="ppm-btn" @click="close">Cancelar</button>
      </div>
    </div>

    <!-- input file oculto -->
    <input
      ref="fileInput"
      type="file"
      accept="image/*"
      class="ppm-hidden"
      @change="onFileSelected"
    />
  </div>
</template>

<script setup>
/**
 * PhotoPickerModal.vue
 * Modal reutilizable para subir o tomar foto.
 *
 * Props:
 *  - visible (v-model:visible)
 *  - title: título del modal
 *  - modelValue: valor actual (opcional)
 *  - output: "file" | "dataUrl" | "both"  (default "both")
 *  - facingMode: "environment" | "user" (default "environment")
 *  - showGrid: líneas guía (default false)
 *  - maxWidth, maxHeight: redimensión (default 1600x1600, no amplía)
 *  - imageType: "image/jpeg" | "image/png" | "image/webp" (default jpeg)
 *  - imageQuality: 0..1 (default 0.9)
 *  - autoCloseOnSelect: cierra el modal al seleccionar/capturar (default true)
 *
 * Emits:
 *  - update:visible (Boolean)
 *  - update:modelValue (payload)
 *  - select (payload)
 *  - error (Error)
 */

import { ref, computed, watch, onBeforeUnmount } from "vue";

const props = defineProps({
  visible: { type: Boolean, default: false },
  title: { type: String, default: "Seleccionar foto" },
  modelValue: { type: Object, default: null },
  output: { type: String, default: "both" }, // "file" | "dataUrl" | "both"
  facingMode: { type: String, default: "environment" },
  showGrid: { type: Boolean, default: false },
  maxWidth: { type: Number, default: 1600 },
  maxHeight: { type: Number, default: 1600 },
  imageType: { type: String, default: "image/jpeg" },
  imageQuality: { type: Number, default: 0.9 },
  autoCloseOnSelect: { type: Boolean, default: true },
});

const emit = defineEmits(["update:visible", "update:modelValue", "select", "error"]);

const step = ref("menu"); // 'menu' | 'camera'
const fileInput = ref(null);
const videoEl = ref(null);
const stream = ref(null);
const currentFacing = ref(props.facingMode);
const canSwitch = ref(true);

const streamActive = computed(() => !!stream.value);

watch(() => props.visible, (v) => {
  if (!v) {
    stopStream();
    step.value = "menu";
  }
});

function close() {
  emit("update:visible", false);
}

function backToMenu() {
  stopStream();
  step.value = "menu";
}

function triggerFile() {
  fileInput.value && (fileInput.value.value = "");
  fileInput.value?.click();
}

function onFileSelected(e) {
  const file = e.target.files?.[0];
  if (!file) return;
  fileToOutput(file, props.maxWidth, props.maxHeight, props.imageType, props.imageQuality, props.output)
    .then(payload => handleSelect(payload))
    .catch(err => emit("error", normalizeErr(err)));
}

async function openCamera() {
  try {
    await startStream(currentFacing.value);
    step.value = "camera";
  } catch (err) {
    emit("error", normalizeErr(err));
  }
}

async function startStream(facing) {
  stopStream();
  if (!navigator.mediaDevices?.getUserMedia) {
    throw new Error("La cámara no está disponible en este navegador/entorno.");
  }
  const constraints = {
    video: {
      facingMode: { ideal: facing },
      width: { ideal: 1280 }, height: { ideal: 720 },
    },
    audio: false,
  };
  const s = await navigator.mediaDevices.getUserMedia(constraints);
  stream.value = s;
  if (videoEl.value) {
    videoEl.value.srcObject = s;
    try { await videoEl.value.play(); } catch { /* noop */ }
  }
  const devices = await navigator.mediaDevices.enumerateDevices();
  canSwitch.value = devices.filter(d => d.kind === "videoinput").length > 1;
}

function stopStream() {
  if (stream.value) {
    stream.value.getTracks().forEach(t => t.stop());
    stream.value = null;
  }
}

async function switchCamera() {
  currentFacing.value = currentFacing.value === "environment" ? "user" : "environment";
  try {
    await startStream(currentFacing.value);
  } catch (err) {
    emit("error", normalizeErr(err));
  }
}

async function capture() {
  if (!videoEl.value) return;
  const video = videoEl.value;
  const vw = video.videoWidth || 1280;
  const vh = video.videoHeight || 720;

  const { canvas, outW, outH } = renderToCanvas(video, vw, vh, props.maxWidth, props.maxHeight);
  const payload = await canvasToOutput(canvas, props.imageType, props.imageQuality, props.output);
  payload.width = outW;
  payload.height = outH;
  handleSelect(payload);
}

function handleSelect(payload) {
  emit("update:modelValue", payload);
  emit("select", payload);
  if (props.autoCloseOnSelect) close();
}

/* ---------- Utilidades ---------- */
function normalizeErr(err) {
  return err instanceof Error ? err : new Error(String(err));
}

function renderToCanvas(source, srcW, srcH, maxW, maxH) {
  const ratio = Math.min(maxW / srcW, maxH / srcH, 1);
  const outW = Math.round(srcW * ratio);
  const outH = Math.round(srcH * ratio);
  const canvas = document.createElement("canvas");
  canvas.width = outW; canvas.height = outH;
  const ctx = canvas.getContext("2d");
  ctx.drawImage(source, 0, 0, outW, outH);
  return { canvas, outW, outH };
}

function fileToOutput(file, maxW, maxH, type, quality, output) {
  return new Promise((resolve, reject) => {
    const fr = new FileReader();
    fr.onload = () => {
      const img = new Image();
      img.onload = async () => {
        const { canvas, outW, outH } = renderToCanvas(img, img.naturalWidth, img.naturalHeight, maxW, maxH);
        const payload = await canvasToOutput(canvas, type, quality, output, file.name);
        payload.width = outW; payload.height = outH;
        resolve(payload);
      };
      img.onerror = reject;
      img.src = fr.result;
    };
    fr.onerror = reject;
    fr.readAsDataURL(file);
  });
}

function canvasToOutput(canvas, type, quality, output, filenameHint = "photo.jpg") {
  return new Promise((resolve) => {
    if (output === "dataUrl") {
      const dataUrl = canvas.toDataURL(type, quality);
      resolve({ dataUrl });
      return;
    }
    canvas.toBlob((blob) => {
      const file = new File([blob], ensureExt(filenameHint, type), { type });
      if (output === "file") {
        resolve({ file });
      } else {
        const dataUrl = canvas.toDataURL(type, quality);
        resolve({ file, dataUrl });
      }
    }, type, quality);
  });
}

function ensureExt(name, mime) {
  const ext = mime === "image/png" ? ".png" : mime === "image/webp" ? ".webp" : ".jpg";
  return /\.\w+$/.test(name) ? name : `photo${ext}`;
}

onBeforeUnmount(() => stopStream());
</script>

<style scoped>
.ppm-backdrop {
  position: fixed; inset: 0; background: rgba(0,0,0,.36);
  display: flex; align-items: flex-start; justify-content: center;
  z-index: 2000;
}
.ppm-modal {
  width: min(96vw, 760px);
  margin-top: 6vh;
  background: #fff; border-radius: 14px;
  box-shadow: 0 12px 36px rgba(0,0,0,.25);
  display: flex; flex-direction: column;
  animation: ppm-in .15s ease-out;
}
@keyframes ppm-in { from { transform: translateY(-16px) } to { transform: translateY(0) } }

.ppm-header, .ppm-footer {
  display: flex; align-items: center; justify-content: space-between;
  padding: .9rem 1rem; border-bottom: 1px solid #eee;
}
.ppm-footer { border-top: 1px solid #eee; border-bottom: none; justify-content: flex-end; }

.ppm-title { margin: 0; font-size: 16px; }
.ppm-icon-btn { background: none; border: none; font-size: 1.4rem; cursor: pointer; line-height: 1; }

.ppm-body { padding: 1rem; }

.ppm-menu { display: grid; gap: .8rem; }
.ppm-action {
  background: #f9fafb; border: 1px solid #e5e7eb; border-radius: 10px;
  padding: .9rem; text-align: left; cursor: pointer; font-size: 15px;
}
.ppm-action:hover { background: #eef2ff; border-color: #c7d2fe; }
.ppm-hint { color: #6b7280; font-size: 12px; margin: .2rem 0 0; }

.ppm-video-wrap { position: relative; width: 100%; background: #000; border-radius: 10px; overflow: hidden; }
video { display: block; width: 100%; height: auto; }
.ppm-grid::before, .ppm-grid::after {
  content: ""; position: absolute; left: 0; width: 100%; height: 1px; background: rgba(255,255,255,.28);
}
.ppm-grid::before { top: 33.333%; }
.ppm-grid::after  { top: 66.666%; }

.ppm-controls { display: flex; gap: .6rem; margin-top: .8rem; }
.ppm-btn {
  background: #f3f4f6; border: 1px solid #e5e7eb; color: #111827;
  padding: .5rem .9rem; border-radius: 10px; cursor: pointer;
}
.ppm-btn:hover { background: #e5e7eb; }
.ppm-primary { background: #2563eb; color: #fff; border-color: #1d4ed8; }
.ppm-primary:hover { background: #1d4ed8; }

.ppm-hidden { display: none; }
</style>
