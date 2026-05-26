using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.Playwright;

namespace PlaywrightTests
{
    public class LocatorsPractice
    {
        [Test]
        public async Task Locators()
        {
            using var playwright = await Playwright.CreateAsync();

            var browser = await playwright.Chromium.LaunchAsync(new() { Headless = false });
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            await page.GotoAsync("https://kontur.ru/lp/qa-auto-ref");

            // Локатор блока “Зарабатывайте на рекомендациях”
            var orderSection = page.Locator("#order");

            // Локатор кнопки “Стать партнером” в блоке в конце страницы 
            var becomePartnerButton = page.Locator("[value='Стать партнером']");

            // Локатор лайтбокса “Заявка на партнерство”
            var becomePartnerLightbox = page.Locator(".lightbox-window__content");

            // Локатор поля “Фамилия”
            var surnameInput = page.Locator(".lightbox-window__content input[data-field-role='surname']");

            // Локатор поля “Имя”
            var nameInput = page.Locator(".lightbox-window__content input[data-field-role='name']");

            // Локатор поля “Электронная почта”
            var emailInput = page.Locator(".lightbox-window__content input[type='email']");

            // Локатор кнопки “Отправить”
            var submitButton = page.Locator(".lightbox-window__content button[type='submit'][value='Отправить']");

            // Локатор лайтбокса “Заявка отправлена”
            var successLightbox = page.Locator(".form-message_success");

            // Локатор текста в лайтбоксе об успешной отправке 
            var successMessageText = page.Locator(".form-message_success [data-role='success-message-text']");

            await context.CloseAsync();
            await browser.CloseAsync();
        }
    }
}