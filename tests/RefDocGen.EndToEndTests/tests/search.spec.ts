import { test, expect } from "@playwright/test";

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
