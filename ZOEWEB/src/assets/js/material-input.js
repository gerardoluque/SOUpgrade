/* eslint-disable */

// when input is focused add focused class for style
function focused(el) {
  if (el.parentElement.classList.contains("input-group")) {
    el.parentElement.classList.add("focused");
  }
}

// when input is focused remove focused class for style
function defocused(el) {
  if (el.parentElement.classList.contains("input-group")) {
    el.parentElement.classList.remove("focused");
  }
}

// helper for adding on all elements multiple attributes
function setAttributes(el, options) {
  Object.keys(options).forEach(function (attr) {
    el.setAttribute(attr, options[attr]);
  });
}

// adding on inputs attributes for calling the focused and defocused functions
if (document.querySelectorAll(".input-group").length != 0) {
  var allInputs = document.querySelectorAll("input.form-control");
  allInputs.forEach((el) =>
    setAttributes(el, {
      onfocus: "focused(this)",
      onfocusout: "defocused(this)",
    }),
  );
}

export default function setMaterialInput() {
  // Material Design Input function
  var inputs = document.querySelectorAll("input");

  for (var i = 0; i < inputs.length; i++) {
    // If the input has a value (including browser autofill setting the value property), mark as filled
    try {
      if (inputs[i].hasAttribute("value") || (inputs[i].value && String(inputs[i].value).trim() !== "")) {
        // dispatch an input event so Vue component handlers pick up the value
        try {
          var evInit = { bubbles: true };
          var inputEvent = new Event('input', evInit);
          inputs[i].dispatchEvent(inputEvent);
        } catch (ee) {
          inputs[i].parentElement.classList.add("is-filled");
        }
      }
    } catch (e) {
      // ignore readonly or unusual inputs
    }
    inputs[i].addEventListener(
      "focus",
      function (e) {
        this.parentElement.classList.add("is-focused");
      },
      false,
    );

    inputs[i].onkeyup = function (e) {
      if (this.value != "") {
        this.parentElement.classList.add("is-filled");
      } else {
        this.parentElement.classList.remove("is-filled");
      }
    };

    inputs[i].addEventListener(
      "focusout",
      function (e) {
        if (this.value != "") {
          this.parentElement.classList.add("is-filled");
        }
        this.parentElement.classList.remove("is-focused");
      },
      false,
    );
  }

  // Some browsers autofill after DOMContentLoaded; re-check inputs at several intervals
  var recheckDelays = [150, 500, 1200, 2500];
  recheckDelays.forEach(function (delay) {
    setTimeout(function () {
      var allInputs2 = document.querySelectorAll("input.form-control");
      allInputs2.forEach(function (inp) {
        try {
          if (inp.value && String(inp.value).trim() !== "") {
            // dispatch input so Vue updates v-model and component classes
            try {
              var ie = new Event('input', { bubbles: true });
              inp.dispatchEvent(ie);
            } catch (ex) {
              inp.parentElement.classList.add('is-filled');
            }
          }
        } catch (e) { }
      });
    }, delay);
  });

  // Listen for autofill CSS animation start (Chrome/Edge/WebKit) to detect autofill immediately
  function onAnimationStart(e) {
    try {
      var el = e.target;
      if (el && el.tagName === 'INPUT') {
        if (el.parentElement && String(el.value || '').trim() !== '') {
          try {
            var ie2 = new Event('input', { bubbles: true });
            el.dispatchEvent(ie2);
          } catch (ex2) {
            el.parentElement.classList.add('is-filled');
          }
        }
      }
    } catch (err) { /* ignore */ }
  }

  document.addEventListener('animationstart', onAnimationStart, true);
  document.addEventListener('webkitAnimationStart', onAnimationStart, true);
}
