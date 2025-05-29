
/**
 * Returns a search result card corresponding to the given item
 * @param {any} item the item whose card is returned
 * @param {any} language the language in which the results are displayed
 */
function getSearchResultCard(item, language) {
    const searchResultTemplate = document.getElementById('search-result-template').firstElementChild;
    const template = searchResultTemplate.cloneNode(true);

    const cardTitleLink = template.querySelector('.search-result-title-link');
    const cardBody = template.querySelector('.search-result-body');

    cardTitleLink.textContent = item.name.data[language];
    cardTitleLink.href = item.url;

    cardBody.innerHTML = item.docComment;

    return template;
}

/**
 * Gets search result
 * @param {any} fuse Fuse.js configuration
 * @param {any} text the text to search for
 * @param {any} language the language in which the results are displayed
 */
function getSearchResults(fuse, text, language) {
    const resultsList = document.getElementById('search-results');

    const query = text.trim();
    const results = fuse.search(query);

    resultsList.innerHTML = '';

    results.forEach(result => {
        const item = getSearchResultCard(result.item, language);
        resultsList.appendChild(item);
    });

    if (results.length === 0 && query !== '') {
        resultsList.innerHTML = 'No results found';
    }
}

window.addEventListener('load', () => {
    const searchBox = document.getElementById('search-box');
    searchBox.focus();

    const serializedSearchData = document.getElementById('search-json-data').innerHTML.trim();

    // Parse the JSON string into a JS object
    const jsonSearchData = JSON.parse(serializedSearchData);

    const language = document.getElementById('language-selector').value;

    const fuse = new Fuse(jsonSearchData, {
        keys: [`name.data.${language}`],
        threshold: 0.5, // Adjust for strict/fuzzy matching, see https://www.fusejs.io/api/options.html#threshold
        distance: 500
    });

    // search when the user types a character
    searchBox.addEventListener('input', () => getSearchResults(fuse, searchBox.value, language));
});
