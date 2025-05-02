function main() {
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

    // TODO
    const themeSwitcher = document.getElementById('theme-switcher');

    themeSwitcher.addEventListener('click', function () {
        const htmlElement = document.documentElement;

        const currentTheme = htmlElement.getAttribute('data-bs-theme');
        const newTheme = currentTheme === 'light' ? 'dark' : 'light';
        htmlElement.setAttribute('data-bs-theme', newTheme);

        localStorage.setItem('refdocgen-theme', newTheme);
    });
}

window.addEventListener('load', main);
