function main() {
    // Get the original <ul> and its <li> items
    const versionList = document.getElementById('version-list');
    const versionItems = versionList.querySelectorAll('li');

    // Create a new <ul> element
    const newUl = document.getElementById('version-selector');

    const currentVersion = versionItems[versionItems.length - 1].textContent;

    // Loop through each version and create a new <li> with a <a> inside
    versionItems.forEach(item => {
        const versionText = item.textContent.trim(); // Get the version text
        const newLi = document.createElement('li');
        const newA = document.createElement('a');

        // Set the href and class for the <a> tag
        newA.classList.add('dropdown-item');

        const currentUrl = window.location.href;
        const newUrl = currentUrl.replace(currentVersion, versionText);

        newA.href = newUrl;
        newA.textContent = versionText;

        // Append the <a> to the <li> and the <li> to the new <ul>
        newLi.appendChild(newA);
        newUl.appendChild(newLi);
    });

}

window.onload = main
