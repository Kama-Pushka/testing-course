using NUnit.Framework;
using Microsoft.Playwright;
using System.Threading.Tasks;

namespace PlaywrightTests
{
    public class WikipediaSearchShould
    {
        [Test]
        public async Task BeReturnPlaywrightPage_WhenFindPlaywright()
        {
            // --- Запустить браузер
            using var playwright = await Playwright.CreateAsync();

            var launchOptions = new BrowserTypeLaunchOptions
            {
                Headless = false
            };
            var browser = await playwright.Chromium.LaunchAsync(launchOptions);
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            // --- Выполнить тест

            // 1. Открыть сайт https://en.wikipedia.org/
            await page.GotoAsync("https://en.wikipedia.org/");

            // 2. Ввести в поле поиска текст: Playwright (software)
            await page.Locator("#searchform input[type='search']").FillAsync("Playwright (software)");

            // 3. Нажать кнопку поиска
            await page.Locator("#searchform button").First.ClickAsync();

            // --- Проверить результат

            // Ожидаемый результат: в инфобоксе указано название "Playwright"
            var infoboxHeading = await page.Locator(".infobox .infobox-above.summary").TextContentAsync();
            Assert.That(infoboxHeading, Is.EqualTo("Playwright"), "В инфобоксе статьи название не совпадает с ожидаемым.");

            // --- Закрыть браузер
            await context.CloseAsync();
            await browser.CloseAsync();
        }
    }
}