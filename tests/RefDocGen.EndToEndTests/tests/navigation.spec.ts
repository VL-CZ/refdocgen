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

    await page.getByRole("link", { name: "Contact" }).click();
    await expect(page).toHaveURL(/contact.html/);
});

test("Go to static page in a subfolder", async ({ page }) => {
    await page.goto("index.html");

    await page.getByRole("button", { name: "Tutorial" }).click();
    await page.getByRole("link", { name: "Installation" }).click();

    await expect(page).toHaveURL(/Tutorial\/installation.html/);
});
