using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.Playwright;

namespace PlaywrightTests
{
    public class ActionsPractice
    {
        [Test]
        public async Task Diadoc_BecomingPartner_SuccessMessageIsShown()
        {
            using var playwright = await Playwright.CreateAsync();

            var browser = await playwright.Chromium.LaunchAsync(new() { Headless = false });
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            // 1. Перейти на страницу с тестовым лендингом
            await page.GotoAsync("https://kontur.ru/lp/qa-auto-ref");

            // Локаторы
            var orderSection = page.Locator("#order");
            var becomePartnerButton = page.Locator("[value='Стать партнером']");
            var becomePartnerLightbox = page.Locator(".lightbox-window__content");
            var surnameInput = page.Locator(".lightbox-window__content input[data-field-role='surname']");
            var nameInput = page.Locator(".lightbox-window__content input[data-field-role='name']");
            var emailInput = page.Locator(".lightbox-window__content input[type='email']");
            var submitButton = page.Locator(".lightbox-window__content button[type='submit'][value='Отправить']");
            var successLightbox = page.Locator(".form-message_success");
            var successMessageTitle = page.Locator(".form-message_success [data-role='success-message-title']");

            // 2. Проскроллить до блока “Зарабатывайте на рекомендациях”
            await orderSection.ScrollIntoViewIfNeededAsync();

            // 3. Кликнуть по кнопке “Стать партнером”
            await becomePartnerButton.ClickAsync();

            // 4. Дождаться появления лайтбокса
            await becomePartnerLightbox.WaitForAsync();

            // 5. Заполнить поле “Фамилия” корректными данными
            await surnameInput.FillAsync("Митягин");

            // 6. Заполнить поле “Имя” корректными данными
            await nameInput.FillAsync("Владимир");

            // 7. Заполнить поле “Электронная почта” корректными данными
            await emailInput.FillAsync("mityagin@skbkontur.ru");

            // 8. Нажать кнопку “Отправить”
            await submitButton.ClickAsync();

            // 9. Дождаться появления сообщения об успешной отправке
            await successLightbox.WaitForAsync();

            // 10. Проверить текст успешного сообщения
            var titleText = (await successMessageTitle.TextContentAsync())?.Trim();

            Assert.That(titleText, 
                Is.EqualTo("Заявка отправлена!"),
                "Неверный текст заголовка успешного сообщения об отправке заявки на партнерство");

            await context.CloseAsync();
            await browser.CloseAsync();
        }
    }
}