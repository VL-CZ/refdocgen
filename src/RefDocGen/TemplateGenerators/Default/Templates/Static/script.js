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
    const currentTheme = document.getElementById('theme-switcher');
    const htmlElement = document.body;

    currentTheme.addEventListener('click', function () {
        const currentTheme = htmlElement.getAttribute('data-bs-theme');
        const newTheme = currentTheme === 'light' ? 'dark' : 'light';
        htmlElement.setAttribute('data-bs-theme', newTheme);

        localStorage.setItem('theme', newTheme);
    });

    const savedTheme = localStorage.getItem('theme');

    if (savedTheme) {
        htmlElement.setAttribute('data-bs-theme', savedTheme);
    }

}

window.addEventListener('load', main);
