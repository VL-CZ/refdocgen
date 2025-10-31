import { test, expect } from '@playwright/test';

test('has title', async ({ page }) => {
  await page.goto('https://playwright.dev/');

  // Expect a title "to contain" a substring.
  await expect(page).toHaveTitle(/Playwright/);
});

test('get started link', async ({ page }) => {
  await page.goto('https://vl-cz.github.io/refdocgen-demo-example-library/api/index.html');

  // Click the get started link.
  await page.getByRole('link', { name: 'assembly RefDocGen.ExampleLibrary' }).click();

  // Expects page to have a heading with the name of Installation.
  await expect(page.getByRole('heading', { name: 'assembly RefDocGen.ExampleLibrary' })).toBeVisible();
});
