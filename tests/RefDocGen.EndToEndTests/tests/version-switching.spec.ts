import { test, expect } from "@playwright/test";

test("Switch documentation version", async ({ page }) => {
    await page.goto("index.html");

    await page.getByRole("link", { name: "class User" }).click();
    await page.getByRole("button", { name: "v1.1" }).click();
    await page.getByRole("link", { name: "v1.0" }).click();

    await expect(page).toHaveURL(
        /v1.0\/api\/RefDocGen\.ExampleLibrary\.User\.html/,
    );
});
