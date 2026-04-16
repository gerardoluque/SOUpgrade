import { Tooltip } from "bootstrap";

export default function setTooltip() {
  if (!Tooltip) {
    console.warn("Bootstrap Tooltip is not available.");
    return;
  }

  const tooltipTriggerList = [].slice.call(
    document.querySelectorAll('[data-bs-toggle="tooltip"]')
  );
  tooltipTriggerList.forEach((tooltipTriggerEl) => {
    new Tooltip(tooltipTriggerEl);
  });
}