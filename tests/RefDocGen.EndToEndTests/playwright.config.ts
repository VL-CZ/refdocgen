import { defineConfig, devices } from "@playwright/test";

export default defineConfig({
    // Look for test files in the "tests" directory, relative to this configuration file.
    testDir: "tests",

    // Run all tests in parallel.
    fullyParallel: true,

    // Fail the build on CI if you accidentally left test.only in the source code.
    forbidOnly: !!process.env.CI,

    // Retry on CI only.
    retries: process.env.CI ? 2 : 0,

    // Opt out of parallel tests on CI.
    workers: process.env.CI ? 1 : undefined,

    // Reporter to use
    reporter: "html",

    use: {
        // Base URL to use in actions like `await page.goto('/')`.
        baseURL:
            process.env.BASE_URL ||
            `file://${process.cwd()}/reference-docs/index.html`,

        // Collect trace when retrying the failed test.
        trace: "on-first-retry",
    },
    // Configure projects for major browsers.
    projects: [
        /* Test against desktop browsers */
        {
            name: "chromium",
            use: { ...devices["Desktop Chrome"] },
        },
        {
            name: "firefox",
            use: { ...devices["Desktop Firefox"] },
        },
        {
            name: "webkit",
            use: { ...devices["Desktop Safari"] },
        },
        // /* Test against mobile viewports. */
        // {
        //     name: "Mobile Chrome",
        //     use: { ...devices["Pixel 5"] },
        // },
        // {
        //     name: "Mobile Safari",
        //     use: { ...devices["iPhone 12"] },
        // },
        // /* Test against branded browsers. */
        // {
        //     name: "Google Chrome",
        //     use: { ...devices["Desktop Chrome"], channel: "chrome" }, // or 'chrome-beta'
        // },
        // {
        //     name: "Microsoft Edge",
        //     use: { ...devices["Desktop Edge"], channel: "msedge" }, // or 'msedge-dev'
        // },
    ],
});
