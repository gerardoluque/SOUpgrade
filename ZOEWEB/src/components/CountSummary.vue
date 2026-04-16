<template>
  <div class="count-summary-list">
    <div
      v-for="(item, idx) in counts"
      :key="idx"
      class="count-summary-item"
      role="button"
      tabindex="0"
      @click="$emit('item-click', item, idx)"
      @keydown.enter="$emit('item-click', item, idx)"
    >
      <div class="count-title">{{ formatValue(item.value, item.mainIsCurrency) }}<span v-if="item.suffix" class="count-suffix">{{ item.suffix }}</span></div>
      <div class="count-label">{{ item.label }}</div>
      <div v-if="hasAmount(item)" class="count-amount">{{ formatCurrency(item.amount) }}</div>
      <div v-else-if="item.meta" class="count-meta">{{ item.meta }}</div>
    </div>
  </div>
</template>

<script setup>
const {counts} = defineProps({
  counts: {
    type: Array,
    required: true,
    // [{ label: 'Filtrado', value: 10, amount: 123.45 }, ...]
  }
});

defineEmits(['item-click'])

function hasAmount(item) {
  return item != null && typeof item.amount !== 'undefined' && item.amount !== null;
}

function formatCurrency(value) {
  try {
    const num = Number(value || 0);
    return new Intl.NumberFormat('es-MX', { style: 'currency', currency: 'MXN' }).format(num);
  } catch (e) {
    return String(value);
  }
}

function formatValue(value, asCurrency = false) {
  try {
    if (value == null) return '';
    const num = Number(value);
    if (!Number.isFinite(num)) return String(value);
    if (asCurrency) {
      return new Intl.NumberFormat('es-MX', { style: 'currency', currency: 'MXN', maximumFractionDigits: 0 }).format(num);
    }
    // show integer without decimals by default
    return new Intl.NumberFormat('es-MX', { maximumFractionDigits: 0 }).format(num);
  } catch (e) {
    return String(value);
  }
}
</script>


<style scoped>
.count-summary-list {
  display: flex;
  flex-wrap: wrap;
  gap: 1rem 1.5rem;
  justify-content: flex-start;
  margin-bottom: 1rem;
}
.count-summary-item {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  width: 260px;
  padding: 1.25rem 1.5rem;
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 2px 12px rgba(16,24,40,0.06), 0 1px 4px rgba(0,0,0,0.04);
  border: 1px solid #e9edf3;
  transition: box-shadow 0.18s, transform 0.12s;
  text-align: center;
}
.count-summary-item:hover {
  box-shadow: 0 6px 28px rgba(16,24,40,0.12);
  transform: translateY(-3px);
}
.count-title {
  color: #C9A227; /* primary system yellow */
  font-size: 2.25rem;
  font-weight: 800;
  text-align: center;
}
.count-label {
  color: #23425a;
  font-size: 0.95rem;
  text-align: center;
  margin-top: 0.5rem;
  font-weight: 700;
  letter-spacing: 0.4px;
  text-transform: uppercase;
}
.count-amount {
  color: #6b7280;
  font-size: 0.95rem;
  margin-top: 0.5rem;
}
.count-meta {
  color: #94a3b8;
  font-size: 0.82rem;
  margin-top: 0.35rem;
  text-transform: uppercase;
  font-weight: 700;
}
.count-suffix {
  font-size: 0.7rem;
  margin-left: 6px;
  color: #334155;
  font-weight: 700;
}
@media (max-width: 900px) {
  .count-summary-item { width: 48%; }
}
@media (max-width: 600px) {
  .count-summary-list { gap: 0.6rem; }
  .count-summary-item { width: 100%; padding: 0.9rem; }
  .count-title { font-size: 1.6rem; }
  .count-label { font-size: 0.95rem; }
}
</style>