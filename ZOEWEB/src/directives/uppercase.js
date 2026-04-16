export default {
  mounted(el) {
    const cleanups = [];

    const attachToInput = (inputEl) => {
      if (!inputEl) return;
      const toUpperAndRestoreCaret = () => {
        const prev = inputEl.value ?? '';
        const upper = prev.toUpperCase();
        if (prev === upper) return false; // no change
        const start = inputEl.selectionStart;
        const end = inputEl.selectionEnd;
        inputEl.value = upper;
        try { inputEl.setSelectionRange(start, end); } catch (e) { console.log("Error setting selection range:", e); }
        return true;
      };
      const syncModel = () => {
        if (inputEl._uc_syncing) return;
        inputEl._uc_syncing = true;
        inputEl.dispatchEvent(new Event('input', { bubbles: true }));
        inputEl._uc_syncing = false;
      };
      const process = () => { if (toUpperAndRestoreCaret()) syncModel(); };
      const onInput = () => process();
      const onChange = () => process();
      const onBlur = () => process();
      const onPaste = () => setTimeout(process, 0);
      inputEl.addEventListener('input', onInput);
      inputEl.addEventListener('change', onChange);
      inputEl.addEventListener('blur', onBlur);
      inputEl.addEventListener('paste', onPaste);
      // Inicial
      process();
      inputEl.style.textTransform = 'uppercase';
      // cleanup for this input
      const remove = () => {
        inputEl.removeEventListener('input', onInput);
        inputEl.removeEventListener('change', onChange);
        inputEl.removeEventListener('blur', onBlur);
        inputEl.removeEventListener('paste', onPaste);
      };
      cleanups.push(remove);
    };

    const isInput = (n) => n && (n.tagName === 'INPUT' || n.tagName === 'TEXTAREA');
    if (isInput(el)) {
      attachToInput(el);
    } else {
      const inputs = el.querySelectorAll('input, textarea');
      inputs.forEach(attachToInput);
    }

    // Observe dynamically added inputs within this subtree
    const observer = new MutationObserver((mutations) => {
      mutations.forEach((m) => {
        (m.addedNodes || []).forEach((node) => {
          if (node.nodeType !== 1) return; // element only
          if (isInput(node)) attachToInput(node);
          else {
            const nested = node.querySelectorAll?.('input, textarea');
            nested && nested.forEach(attachToInput);
          }
        });
      });
    });
    observer.observe(el, { childList: true, subtree: true });

    el._uppercaseCleanup = () => {
      try { observer.disconnect(); } catch (e) { console.log("Error disconnecting observer:", e); }
      cleanups.forEach(fn => {
        try { fn(); } catch (e) { console.log("Error during cleanup:", e); }
      });
    };
  },
  unmounted(el) { el._uppercaseCleanup && el._uppercaseCleanup(); }
};
