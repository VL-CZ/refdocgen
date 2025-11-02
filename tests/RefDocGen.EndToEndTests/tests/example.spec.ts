import { test, expect } from "@playwright/test";

test("API pages navigation", async ({ page }) => {
    await page.goto("index.html");

    await page
        .getByRole("link", { name: "assembly RefDocGen.ExampleLibrary" })
        .click();

    await page
        .getByRole("link", {
            name: "namespace RefDocGen.ExampleLibrary",
            exact: true,
        })
        .click();

    await page.getByRole("link", { name: "class User" }).click();

    await expect(page).toHaveURL(/RefDocGen\.ExampleLibrary\.User\.html/);
});

test("Search for class", async ({ page }) => {
    await page.goto("index.html");

    await page.getByRole("searchbox", { name: "Search" }).click();

    const searchBox = page.getByRole("textbox", { name: "Search input" });

    await searchBox.click();
    await searchBox.fill("animal");
    await searchBox.dispatchEvent("input");

    await page
        .getByRole("link", { name: "RefDocGen.ExampleLibrary.Animal class" })
        .click();

    await expect(page).toHaveURL(
        /api\/RefDocGen\.ExampleLibrary\.Animal\.html/,
    );
});

test("Search for member", async ({ page }) => {
    await page.goto("index.html");

    await page.getByRole("searchbox", { name: "Search" }).click();

    const searchBox = page.getByRole("textbox", { name: "Search input" });

    await searchBox.click();
    await searchBox.fill("firstname");
    await searchBox.dispatchEvent("input");

    await page
        .getByRole("link", {
            name: "RefDocGen.ExampleLibrary.User.FirstName property",
        })
        .click();

    await expect(page).toHaveURL(
        /api\/RefDocGen\.ExampleLibrary\.User\.html.*#FirstName/,
    );
});

test("Go to static page", async ({ page }) => {
    await page.goto("index.html");

    await page.getByRole("link", { name: "HtmlPage" }).click();
    await expect(page).toHaveURL(/htmlPage.html/);
});

test("Go to static page in a subfolder", async ({ page }) => {
    await page.goto("index.html");

    await page.getByRole("button", { name: "Folder" }).click();
    await page.getByRole("link", { name: "AnotherPage" }).click();

    await expect(page).toHaveURL(/folder\/anotherPage.html/);
});

test("Switch documentation version", async ({ page }) => {
    await page.goto("index.html");

    await page.getByRole("link", { name: "class User" }).click();
    await page.getByRole("button", { name: "v-privatexxx" }).click();
    await page.getByRole("link", { name: "v-public" }).click();

    await expect(page).toHaveURL(
        /v-public\/api\/RefDocGen\.ExampleLibrary\.User\.html/,
    );
});
