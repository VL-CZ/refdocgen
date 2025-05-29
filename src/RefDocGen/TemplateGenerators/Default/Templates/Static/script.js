
function fetchVersions() {
    // Get the original <ul> and its <li> items

    const versionListElement = document.getElementById('version-list');

    if (versionListElement) {

        const versionsJson = versionListElement.innerText;
        const versionItems = JSON.parse(versionsJson).reverse();

        const currentVersion = document.getElementById('current-version').innerText.trim();

        // Create a new <ul> element
        const newUl = document.getElementById('version-selector');

        // Loop through each version and create a new <li> with a <a> inside
        versionItems.forEach(item => {
            const versionText = item.trim(); // Get the version text
            const newLi = document.createElement('li');
            const newA = document.createElement('a');

            // Set the href and class for the <a> tag
            newA.classList.add('dropdown-item');

            const currentUrl = window.location.href;
            const newUrl = currentUrl.replace(currentVersion, versionText);

            newA.href = newUrl;

            newA.textContent = versionText;

            if (versionText === currentVersion) {
                newA.textContent += " (Current)";
            }

            // Append the <a> to the <li> and the <li> to the new <ul>
            newLi.appendChild(newA);
            newUl.appendChild(newLi);
        });
    }
}

function switchTheme() {
    const htmlElement = document.documentElement;

    const currentTheme = htmlElement.getAttribute('data-bs-theme');
    const newTheme = currentTheme === 'light' ? 'dark' : 'light';
    htmlElement.setAttribute('data-bs-theme', newTheme);

    localStorage.setItem('refdocgen-theme', newTheme);
}

/**
 * Sets the language visibility
 * @param {any} selectedLang the selected language identifier
 * @param {any} allLangs identifiers of all languages
 */
function setLanguageVisibility(selectedLang) {

    // get all language identifiers
    const allLangs = Array.from(document.getElementsByClassName('lang-option'))
        .map(option => option.value);

    // invalid language value -> return
    if (!allLangs.includes(selectedLang)) {
        return;
    }

    // Store into localStorage
    localStorage.setItem('selectedLanguage', selectedLang);

    // Hide all language-specific elements
    allLangs.forEach(lang => {
        const elements = document.getElementsByClassName(lang);
        Array.from(elements).forEach(el => {
            el.classList.add('not-visible');
        });
    });

    // Show only selected language elements
    const selectedLangElements = document.getElementsByClassName(selectedLang);
    Array.from(selectedLangElements).forEach(el => {
        el.classList.remove('not-visible');
    });
}

function main() {
    // fetch versions
    fetchVersions();

    // switch theme on click
    const themeSwitcher = document.getElementById('theme-switcher');
    themeSwitcher.addEventListener('click', switchTheme);

    // go to search page on search bar click
    const menuSearchBar = document.getElementById('menu-search-bar');
    menuSearchBar.addEventListener('focus', () => {
        const targetUrl = menuSearchBar.getAttribute('url-target');
        window.location.href = targetUrl;
    });

    // language switching
    const languageSelector = document.getElementById('language-selector');

    // Event listener for dropdown change
    languageSelector.addEventListener('change', function () {
        const selectedLang = this.value;

        // set language visibility
        setLanguageVisibility(selectedLang);
    });
}

window.addEventListener('load', main);

// On page load, restore selected language
window.addEventListener('DOMContentLoaded', () => {
    const savedLang = localStorage.getItem('selectedLanguage');
    const languageSelector = document.getElementById('language-selector');

    if (savedLang) {
        // Set dropdown to saved value
        languageSelector.value = savedLang;

        // Update visibility
        setLanguageVisibility(savedLang);
    }
});
