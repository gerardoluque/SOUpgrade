<template>
  <div class="rte-wrapper">
    <div class="rte-toolbar btn-group btn-group-sm mb-1" role="toolbar">
      <button type="button" class="btn btn-light" title="Negritas" @click.prevent="exec('bold')"><strong>B</strong></button>
      <button type="button" class="btn btn-light" title="Itálica" @click.prevent="exec('italic')"><em>I</em></button>
      <button type="button" class="btn btn-light" title="Subrayado" @click.prevent="exec('underline')"><u>U</u></button>
      <button type="button" class="btn btn-light" title="Lista" @click.prevent="exec('insertUnorderedList')"><span>• List</span></button>
      <button type="button" class="btn btn-light" title="Limpiar" @click.prevent="exec('removeFormat')">CLR</button>
    </div>
    <div
      :id="id"
      ref="editorRef"
      class="rte-editor form-control"
      :class="{ 'is-invalid': error, 'is-valid': success }"
      contenteditable="true"
      :placeholder="placeholder"
      @beforeinput="onBeforeInput"
      @paste="onPaste"
      @compositionend="onCompositionEnd"
      @input="onInput"
      @blur="onBlur"
    ></div>
  </div>
</template>
<script>
export default {
  name: 'RichTextEditor',
  props: {
    id: { type: String, required: true },
    modelValue: { type: String, default: '' },
    placeholder: { type: String, default: '' },
    success: { type: Boolean, default: false },
    error: { type: Boolean, default: false },
    disabled: { type: Boolean, default: false },
    maxLength: { type: Number, default: 8000 }
  },
  emits: ['update:modelValue','blur'],
  data() {
    return { internalUpdate: false };
  },
  watch: {
    modelValue(newVal) {
      if (this.internalUpdate) return;
      if (newVal !== this.getHtml()) this.setHtml(newVal || '');
    },
    disabled(val){
      if (this.$refs.editorRef) {
        this.$refs.editorRef.setAttribute('contenteditable', val ? 'false' : 'true');
      }
    }
  },
  mounted() {
    this.setHtml(this.modelValue);
    if (this.disabled && this.$refs.editorRef) {
      this.$refs.editorRef.setAttribute('contenteditable','false');
    }
  },
  methods: {
    exec(cmd) {
      if (this.disabled) return;
      document.execCommand(cmd, false, null);
      this.forceUppercase();
      this.emitCurrentHtml();
    },
    getHtml() { return this.$refs.editorRef?.innerHTML || ''; },
    setHtml(html) {
      if (!this.$refs.editorRef) return;
      const upperHtml = this.toUppercaseHtml(html || '');
      if (this.$refs.editorRef.innerHTML !== upperHtml) {
        this.$refs.editorRef.innerHTML = upperHtml;
      }
    },
    onBeforeInput(event) {
      if (this.disabled) return;
      const data = event?.data;
      if (typeof data === 'string') {
        // Enforce max length based on plain text content
        const el = this.$refs.editorRef;
        const currentText = el ? (el.textContent || '') : '';
        const incoming = data.toString();
        const remaining = Number.isFinite(this.maxLength) ? Math.max(0, this.maxLength - currentText.length) : Infinity;
        const insertText = incoming.toUpperCase().slice(0, remaining);
        event.preventDefault();
        if (insertText.length) {
          document.execCommand('insertText', false, insertText);
        }
      }
    },
    onPaste(event) {
      if (this.disabled) return;
      event.preventDefault();
      const el = this.$refs.editorRef;
      const currentText = el ? (el.textContent || '') : '';
      const text = event.clipboardData?.getData('text/plain') ?? '';
      const remaining = Number.isFinite(this.maxLength) ? Math.max(0, this.maxLength - currentText.length) : Infinity;
      const insertText = text.toUpperCase().slice(0, remaining);
      if (insertText.length) document.execCommand('insertText', false, insertText);
    },
    onCompositionEnd() {
      if (this.disabled) return;
      this.forceUppercase();
      this.emitCurrentHtml();
    },
    onInput() {
      if (this.disabled) return;
      this.forceUppercase();
      // After input, ensure not exceeding maxLength; if exceeds, trim plain text to max and set content
      try {
        if (Number.isFinite(this.maxLength)) {
          const el = this.$refs.editorRef;
          if (el) {
            const plain = (el.textContent || '').replace(/\u00A0/g, ' ');
            if (plain.length > this.maxLength) {
              // Trim to allowed plain text length; this will lose some formatting but ensures limit
              const allowed = plain.slice(0, this.maxLength);
              // Replace editor content with plain allowed text (uppercased by forceUppercase)
              el.innerText = allowed.toUpperCase();
            }
          }
        }
      } catch (e) { console.warn('RichTextEditor enforce maxLength error', e); }
      this.emitCurrentHtml();
    },
    onBlur() { this.$emit('blur'); }
    ,
    forceUppercase() {
      const el = this.$refs.editorRef;
      if (!el) return;
      const walker = document.createTreeWalker(el, NodeFilter.SHOW_TEXT, null);
      while (walker.nextNode()) {
        const node = walker.currentNode;
        if (node?.nodeValue) {
          const upper = node.nodeValue.toUpperCase();
          if (upper !== node.nodeValue) {
            node.nodeValue = upper;
          }
        }
      }
    },
    toUppercaseHtml(html) {
      const tmp = document.createElement('div');
      tmp.innerHTML = html || '';
      const walker = document.createTreeWalker(tmp, NodeFilter.SHOW_TEXT, null);
      while (walker.nextNode()) {
        const node = walker.currentNode;
        if (node?.nodeValue) node.nodeValue = node.nodeValue.toUpperCase();
      }
      return tmp.innerHTML;
    },
    emitCurrentHtml() {
      this.internalUpdate = true;
      this.$emit('update:modelValue', this.getHtml());
      this.$nextTick(() => { this.internalUpdate = false; });
    }
  }
};
</script>
<style scoped>
.rte-editor {
  min-height: 100px;
  overflow-y: auto;
  text-transform: uppercase;
}
.rte-toolbar .btn { font-size: 0.65rem; }
</style>
