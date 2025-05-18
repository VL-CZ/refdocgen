function getSearchResultCard(item) {
    const searchResultTemplate = document.getElementById('search-result-template').firstElementChild;
    const template = searchResultTemplate.cloneNode(true);

    const cardTitleLink = template.querySelector('.search-result-title-link');
    const cardBody = template.querySelector('.search-result-body');

    cardTitleLink.textContent = item.Name;
    cardTitleLink.href = item.Url;

    cardBody.innerHTML = item.DocComment;

    return template;
}

document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('search-box').focus();

    const serializedSearchData = document.getElementById('search-json-data').innerHTML.trim();

    // Parse the JSON string into a JS object
    const jsonSearchData = JSON.parse(serializedSearchData);

    const fuse = new Fuse(jsonSearchData, {
        keys: ['Name'],
        threshold: 0.5 // Adjust for strict/fuzzy matching
    });

    const searchBox = document.getElementById('search-box');
    const resultsList = document.getElementById('search-results');

    searchBox.addEventListener('input', () => {
        const query = searchBox.value.trim();
        const results = fuse.search(query);

        resultsList.innerHTML = '';

        results.forEach(result => {
            const item = getSearchResultCard(result.item);
            resultsList.appendChild(item);
        });

        if (results.length === 0 && query !== '') {
            resultsList.innerHTML = 'No results found';
        }
    });
});
