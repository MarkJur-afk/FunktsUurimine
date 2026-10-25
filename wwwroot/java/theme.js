document.addEventListener("DOMContentLoaded", function () {

    const button = document.getElementById("themeToggle");

    let theme = localStorage.getItem("theme");

    if (!theme) {
        theme = "light";
    }

    applyTheme(theme);

    if (button) {
        button.addEventListener("click", function () {

            const currentTheme =
                document.documentElement.getAttribute("data-theme");

            if (currentTheme === "dark") {
                applyTheme("light");
            } else {
                applyTheme("dark");
            }

        });
    }
});


function applyTheme(theme) {

    document.documentElement.setAttribute("data-theme", theme);

    localStorage.setItem("theme", theme);

    const button = document.getElementById("themeToggle");

    if (!button) {
        return;
    }

    const russian = localStorage.getItem("language") === "ru";
    button.textContent = theme === "dark"
        ? (russian ? "☀️ Светлая тема" : "☀️ Hele teema")
        : (russian ? "🌙 Тёмная тема" : "🌙 Tume teema");
}

window.addEventListener("languagechange", () => applyTheme(document.documentElement.getAttribute("data-theme") || "light"));
