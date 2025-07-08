
const isLocalFile = location.protocol === "file:";

/**
 * Loads the version list from the 'version-list' element, and populates the 'version-selector' dropdown with their data.
 */
function loadVersions() {
    const versionListElement = document.getElementById("version-list");

    if (versionListElement) {

        const versionsJson = versionListElement.innerText;
        const versionItems = JSON.parse(versionsJson).reverse();

        const currentVersion = document.getElementById("current-version").innerText.trim();

        // Get the 'version-selector'
        const versionSelector = document.getElementById("version-selector");

        // Loop through each version and create a new <li> with a <a> inside
        versionItems.forEach(item => {
            const versionText = item.trim(); // Get the version text
            const newLi = document.createElement("li");
            const newA = document.createElement("a");

            // Set the href and class for the <a> tag
            newA.classList.add("dropdown-item");

            const currentUrl = window.location.href;
            const newUrl = currentUrl.replace(currentVersion, versionText);

            newA.href = newUrl;
            newA.textContent = versionText;

            if (versionText === currentVersion) {
                newA.textContent += " (Current)";
            }

            // Append the <a> to the <li> and the <li> to the new <ul>
            newLi.appendChild(newA);
            versionSelector.appendChild(newLi);
        });
    }
}

/**
 * Gets the other theme that passed in the parameter.
 */
function getOtherTheme(theme) {
    return theme === "light" ? "dark" : "light";
}

/**
 * Switches the theme from light to dark or vice versa.
 */
function switchTheme() {
    if (isLocalFile) { // set the 'theme' URL param, and redirect
        const url = new URL(window.location.href);
        const currentTheme = url.searchParams.get("theme");
        const newTheme = getOtherTheme(currentTheme);

        url.searchParams.set("theme", newTheme);
        window.location.href = url.toString();
    }
    else { // set the 'data-bs-theme' attribute + store into local storage
        const htmlElement = document.documentElement;
        const currentTheme = htmlElement.getAttribute("data-bs-theme");
        const newTheme = getOtherTheme(currentTheme);

        htmlElement.setAttribute("data-bs-theme", newTheme);
        localStorage.setItem("refdocgen-theme", newTheme);
    }
}

/**
 * Sets the visibility of language specific elements.
 * @param {any} selectedLang the selected language identifier.
 */
function setLanguageVisibility(selectedLang) {

    // get all language identifiers
    const allLangs = Array.from(document.getElementsByClassName("lang-option"))
        .map(option => option.value);

    // invalid language value -> do nothing
    if (!allLangs.includes(selectedLang)) {
        return;
    }

    if (isLocalFile) {
        const url = new URL(window.location.href);

        if (url.searchParams.get("lang") !== selectedLang) { // add the 'lang' URL param, and redirect
            url.searchParams.set("lang", selectedLang);
            window.location.href = url.toString();
        }
    }
    else { // set 'data-language' attribute + store into local storage
        document.documentElement.setAttribute("data-language", selectedLang);
        localStorage.setItem("refdocgen-language", selectedLang);
    }
}

/**
 * Tests if the URL is absolute.
 */
function isAbsoluteUrl(url) {
    return /^(https?:)?\/\//.test(url); // check for 'http' or 'https' or '//', which is enough in our case
                                        // https://stackoverflow.com/questions/10687099/how-to-test-if-a-url-string-is-absolute-or-relative
}

/**
 * Updates all relative links at the page to include the same URL parameters as this page has.
 */
function updateRelativeLinks() {
    const urlParams = new URLSearchParams(window.location.search);
    if (!urlParams.get("theme")) {
        urlParams.set("theme", "dark");
    }

    if (!urlParams.get("lang")) {
        urlParams.set("lang", "csharp-lang");
    }

    let links = document.querySelectorAll("a[href]");

    for (const link of links) {
        const href = link.getAttribute("href");

        // Skip if it's an absolute URL (e.g., starts with http or //)
        if (isAbsoluteUrl(href)) {
            continue;
        }

        // Create a URL object relative to the current location
        const url = new URL(href, window.location.href);

        // Preserve the existing URL params
        url.search = urlParams.toString();

        // Set it back as a relative URL (without origin)
        link.setAttribute("href", url.toString());
    }
}

function main() {
    const languageSelector = document.getElementById("language-selector");
    const savedLang = document.documentElement.getAttribute("data-language");

    // set the saved language
    if (savedLang) {
        languageSelector.value = savedLang;
    }

    // load versions, and create version selector dropdown
    loadVersions();

    // switch theme on click
    const themeSwitcher = document.getElementById("theme-switcher");
    themeSwitcher.addEventListener("click", switchTheme);

    // go to search page on search bar click
    const menuSearchBar = document.getElementById("menu-search-bar");
    menuSearchBar.addEventListener("focus", () => {
        const targetRelativeUrl = menuSearchBar.getAttribute("data-url-target");

        const currentUrl = new URL(window.location.href);
        const newUrl = new URL(targetRelativeUrl, window.location.href);
        newUrl.search = currentUrl.search;

        window.location.href = newUrl.toString();
    });

    // Event listener for dropdown change
    languageSelector.addEventListener("change", function () {
        const selectedLang = this.value;

        // set language visibility
        setLanguageVisibility(selectedLang);
    });

    // for local files, update all relative links to use the same URL params
    if (isLocalFile) {
        updateRelativeLinks();
    }
}

if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", main);
} else {
    main();
}
