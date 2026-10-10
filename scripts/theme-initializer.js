"use strict";
const theme = localStorage.getItem("theme") || "theme-system-default";
document.body.classList.add(theme);
