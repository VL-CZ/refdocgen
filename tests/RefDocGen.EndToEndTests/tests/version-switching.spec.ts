import { test, expect } from "@playwright/test";

test("Switch documentation version", async ({ page }) => {
    await page.goto("index.html");

    await page.getByRole("link", { name: "class User" }).click();
    await page.getByRole("button", { name: "v-privatexxx" }).click();
    await page.getByRole("link", { name: "v-public" }).click();

    await expect(page).toHaveURL(
        /v-public\/api\/RefDocGen\.ExampleLibrary\.User\.html/,
    );
});
