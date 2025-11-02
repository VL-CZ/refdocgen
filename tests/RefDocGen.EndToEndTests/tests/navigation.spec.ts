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

