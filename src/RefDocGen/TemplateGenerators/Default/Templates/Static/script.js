
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
}

window.addEventListener('load', main);
