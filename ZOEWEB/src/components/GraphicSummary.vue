<template>
  <div class="card h-100">
    <div class="card-header pb-0">
      <h6 class="mb-0">{{ title }}</h6>
      <small v-if="subtitle" class="text-muted">{{ subtitle }}</small>
    </div>
    <div class="card-body">
      <div v-if="type === 'donut'">
        <div class="chart-wrap">
          <canvas ref="canvasRef" :height="180"></canvas>
        </div>
        <ul v-if="legend && legend.length" class="legend mt-3">
          <li v-for="(l,i) in legend" :key="i">
            <span class="dot" :style="{ background: l.color }"></span>
            <span class="label">{{ l.label }}</span>
            <span class="value">{{ formatPercent(l.value) }}</span>
          </li>
        </ul>
      </div>

      <div v-else-if="type === 'gauge'" class="gauge-wrap">
        <div class="gauge-chart">
          <canvas ref="canvasRef" :height="200"></canvas>
          <div class="center-text">
                  <div class="amount">{{ formatCurrency(gasto) }}</div>
                  <div class="percent">{{ formatPercent(porcentaje) }}</div>
                  <div class="caption text-muted">GASTO</div>
          </div>
        </div>
        <div class="gauge-legend mt-3">
          <div class="row">
            <div class="col-6 d-flex justify-content-between">
              <span class="text-uppercase small">PRESUPUESTO</span>
              <span class="fw-semibold">{{ formatCurrency(presupuesto) }}</span>
            </div>
            <div class="col-6 d-flex justify-content-between">
              <span class="text-uppercase small">GASTO</span>
              <span class="fw-semibold">{{ formatCurrency(gasto) }}</span>
            </div>
          </div>
        </div>
      </div>

      <div v-else class="text-muted">Tipo de gráfica no soportado.</div>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref, watch, computed } from 'vue'
import Chart from 'chart.js/auto'

const props = defineProps({
  title: { type: String, default: '' },
  subtitle: { type: String, default: '' },
  type: { type: String, default: 'donut' }, // 'donut' | 'gauge'
  data: { type: Object, default: () => ({}) }
})

const canvasRef = ref(null)
let chart

const presupuesto = computed(() => (props.data && props.data.presupuesto) ?? 0)
const gasto = computed(() => (props.data && props.data.gasto) ?? 0)
const porcentaje = computed(() => {
  const d = props.data || {}
  if (typeof d.porcentaje === 'number' && !Number.isNaN(d.porcentaje)) {
    // If backend returns a decimal (0.34) convert to percent, if >1 assume already percent
    if (d.porcentaje > 0 && d.porcentaje <= 1) return Math.round(d.porcentaje * 100)
    return Math.round(d.porcentaje)
  }
  // fallback compute
  const p = Number(d.presupuesto || 0)
  const g = Number(d.gasto || 0)
  if (p <= 0) return 0
  return Math.round((g / p) * 100)
})

const legend = computed(() => {
  if (props.type !== 'donut') return []
  const d = props.data || {}
  const labels = d.labels || []
  const values = d.values || []
  const colors = d.colors || []
  const total = values.reduce((a,b)=>a+b,0) || 1
  return labels.map((l, i) => ({
    label: l,
    value: Math.round((values[i] || 0) * 100 / total),
    color: colors[i] || '#999'
  }))
})

function render() {
  if (!canvasRef.value) return
  const ctx = canvasRef.value.getContext('2d')
  if (chart) { chart.destroy() }

  if (props.type === 'donut') {
    const d = props.data || {}
    chart = new Chart(ctx, {
      type: 'doughnut',
      data: {
        labels: d.labels || [],
        datasets: [{
          data: d.values || [],
          backgroundColor: d.colors || ['#ef476f','#118ab2','#06d6a0','#ffd166','#8338ec'],
          borderWidth: 0,
          hoverOffset: 4,
          cutout: '60%'
        }]
      },
      options: {
        plugins: { legend: { display: false } },
        responsive: true,
        maintainAspectRatio: false
      }
    })
  } else if (props.type === 'gauge') {
    const d = props.data || {}
    const restante = Math.max(0, (d.presupuesto || 0) - (d.gasto || 0))
    chart = new Chart(ctx, {
      type: 'doughnut',
      data: {
        labels: ['Gasto','Restante'],
        datasets: [{
          data: [d.gasto || 0, restante],
          backgroundColor: ['#C9A227', '#e5e7eb'],
          borderWidth: 0,
          cutout: '70%'
        }]
      },
      options: {
        plugins: { legend: { display: false } },
        rotation: -90,
        circumference: 360,
        responsive: true,
        maintainAspectRatio: false
      }
    })
  }
}

onMounted(render)
watch(() => props.data, render, { deep: true })
watch(() => props.type, render)

function formatPercent(v){ return `${v}%` }
function formatCurrency(n){
  try {
    return new Intl.NumberFormat('es-MX',{ style:'currency', currency:'MXN', maximumFractionDigits:0 }).format(Number(n||0))
  } catch { return `$${Number(n||0).toLocaleString()}` }
}
</script>

<style scoped>
.chart-wrap { position: relative; height: 260px; }
.legend { list-style: none; padding: 0; margin: 0; }
.legend li { display: flex; align-items: center; justify-content: space-between; gap: .5rem; }
.legend .dot { width: 10px; height: 10px; border-radius: 50%; display: inline-block; }
.legend .label { flex: 1; }
.gauge-wrap { position: relative; min-height: 230px; }
.gauge-chart { position: relative; height: 210px; }
.center-text { position: absolute; inset: 0; display:flex; align-items:center; justify-content:center; flex-direction:column; pointer-events:none; }
.center-text .amount { font-size: 1.25rem; font-weight: 700; }
.center-text .percent { font-size: 1.15rem; font-weight: 800; color: #C9A227; margin-top: 0.2rem; }
.center-text .caption { margin-top: 0.2rem; }
</style>
