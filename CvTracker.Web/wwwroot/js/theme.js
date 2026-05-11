(function () {

  var storageKey = "adas-connect-theme";



  function current() {

    var v = document.documentElement.getAttribute("data-theme");

    return v === "dark" ? "dark" : "light";

  }



  function set(theme) {

    if (theme !== "light" && theme !== "dark") {

      theme = "light";

    }

    document.documentElement.setAttribute("data-theme", theme);

    try {

      localStorage.setItem(storageKey, theme);

    } catch (e) {

      /* ignore */

    }

    var meta = document.getElementById("theme-color-meta");

    if (meta) {

      meta.setAttribute("content", theme === "dark" ? "#0d3d28" : "#00A651");

    }

    var btn = document.getElementById("themeToggle");

    if (!btn) {

      return;

    }

    btn.setAttribute("aria-pressed", theme === "dark" ? "true" : "false");

    var icon = btn.querySelector(".theme-icon");

    var label = btn.querySelector(".theme-toggle-text");

    if (theme === "dark") {

      if (icon) {

        icon.textContent = "\u2600";

      }

      if (label) {

        label.textContent = "Light";

      }

      btn.setAttribute("title", "Switch to light mode");

      btn.setAttribute("aria-label", "Switch to light mode");

    } else {

      if (icon) {

        icon.textContent = "\u263D";

      }

      if (label) {

        label.textContent = "Dark";

      }

      btn.setAttribute("title", "Switch to dark mode");

      btn.setAttribute("aria-label", "Switch to dark mode");

    }

  }



  function ensureInitialAttribute() {

    if (document.documentElement.getAttribute("data-theme")) {

      return;

    }

    try {

      var stored = localStorage.getItem(storageKey);

      if (stored === "light" || stored === "dark") {

        document.documentElement.setAttribute("data-theme", stored);

        return;

      }

      stored = localStorage.getItem("cvtracker-theme");

      if (stored === "light" || stored === "dark") {

        document.documentElement.setAttribute("data-theme", stored);

        return;

      }

    } catch (e) {

      /* ignore */

    }

    if (window.matchMedia && window.matchMedia("(prefers-color-scheme: dark)").matches) {

      document.documentElement.setAttribute("data-theme", "dark");

    } else {

      document.documentElement.setAttribute("data-theme", "light");

    }

  }



  function onReady() {

    ensureInitialAttribute();

    set(current());

    var btn = document.getElementById("themeToggle");

    if (btn) {

      btn.addEventListener("click", function () {

        set(current() === "dark" ? "light" : "dark");

      });

    }

  }



  if (document.readyState === "loading") {

    document.addEventListener("DOMContentLoaded", onReady);

  } else {

    onReady();

  }

})();

