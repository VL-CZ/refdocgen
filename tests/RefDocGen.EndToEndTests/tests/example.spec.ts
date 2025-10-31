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

test('test', async ({ page }) => {
  await page.goto('https://vl-cz.github.io/refdocgen-demo-example-library/api/index.html');
  await expect(page.locator('body')).toMatchAriaSnapshot(`- heading "RefDocGen API" [level=1]`);
  await page.getByRole('link', { name: 'assembly RefDocGen.ExampleLibrary' }).click();
  await expect(page.locator('body')).toMatchAriaSnapshot(`
    - heading "assembly RefDocGen.ExampleLibrary" [level=1]
    - button:
      - img
    `);
  await page.getByRole('link', { name: 'namespace RefDocGen.ExampleLibrary', exact: true }).click();
  await expect(page.locator('h1')).toMatchAriaSnapshot(`- heading "namespace RefDocGen.ExampleLibrary" [level=1]`);
  await page.getByRole('link', { name: 'class User' }).click();
  await expect(page.locator('body')).toMatchAriaSnapshot(`
    - heading "public class User" [level=1]
    - button:
      - img
    `);
  await page.getByRole('link', { name: 'API' }).click();
  await expect(page.locator('body')).toMatchAriaSnapshot(`
    - heading "RefDocGen API" [level=1]
    - button:
      - img
    `);
});

test('test', async ({ page }) => {
  await page.goto('https://vl-cz.github.io/refdocgen-demo-example-library/api/index.html');
  await page.getByRole('searchbox', { name: 'Search' }).click();
  await page.getByRole('textbox', { name: 'Search input' }).fill('user');
  await page.getByRole('link', { name: 'RefDocGen.ExampleLibrary.User class' }).click();
  await expect(page.locator('body')).toMatchAriaSnapshot(`
    - heading "public class User" [level=1]
    - button:
      - img
    `);
});
