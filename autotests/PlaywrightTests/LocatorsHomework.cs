using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.Playwright;

namespace PlaywrightTests
{
    public class LocatorsHomework
    {
        [Test]
        public async Task Locators()
        {
            using var playwright = await Playwright.CreateAsync();

            var browser = await playwright.Chromium.LaunchAsync(new() { Headless = false });
            var context = await browser.NewContextAsync();
            var page = await context.NewPageAsync();

            await page.GotoAsync("https://kontur.ru/lp/qa-auto");

            // Локатор кнопки “Попробовать”
            var tryButton = page.Locator("#try-button");

            // Локатор кнопки “Отправить заявку” внизу страницы
            var sendOrderButton = page.Locator("[data-buy-product-button=''][data-long-text='Отправить заявку']");

            await page.GotoAsync("https://kontur.ru/lp/qa-auto-integration");

            // Локатор Виджета Заявки в конце страницы
            var widgetForm = page.Locator("form[data-tid='FWForm']");

            // Поле ввода Фамилии
            var surnameInput = page.Locator("form[data-tid='FWForm'] input[data-field-role='surname']");

            // Поле ввода Имени
            var nameInput = page.Locator("form[data-tid='FWForm'] input[data-field-role='name']");

            // Селект Региона
            var regionSelect = page.Locator("form[data-tid='FWForm'] [data-fw-component-type='region'] select");

            // Поле ввода Email
            var emailInput = page.Locator("form[data-tid='FWForm'] input[type='email']");

            // Локатор ошибки, если ввели неверный Email
            var emailInputValidationError = page.Locator("form[data-tid='FWForm'] [data-fw-component-type='email'] [class='field-validation-error']");

            // Поле ввода Телефона
            var phoneInput = page.Locator("form[data-tid='FWForm'] [data-fw-component-type='phone'] input");

            // Поле ввода Организации
            var companyNameInput = page.Locator("form[data-tid='FWForm'] [data-fw-component-type='company'] input[data-field-role='company-name']");

            // Саджест Организации
            var organizationSuggest = page.Locator("form[data-tid='FWForm'] [data-fw-component-type='company'] [class='organization-suggest-container']");

            // Первый элемент в саджесте Организации
            var organizationSuggestFirstItem = page.Locator("form[data-tid='FWForm'] [data-fw-component-type='company'] [class='organization-suggest-container'] [data-index='0']");

            // Поле загрузки файла Список Контрагентов
            var contragentsFileUploaderInput = page.Locator("form[data-tid='FWForm'] [data-fw-component-type='fileuploader'] input[data-role='fileUploader']");

            // Лейбл с названием загруженного файла
            var fileNameLabel = page.Locator("form[data-tid='FWForm'] [data-fw-component-type='fileuploader'] [class='file-loader__filename-wrap']");

            // Чекбокс Связаться в определённый день
            var customDayCheckbox = page.Locator("form[data-tid='FWForm'] [data-fw-component-type='checkbox'] label");

            // Поле ввода Даты
            var datePicker = page.Locator("form[data-tid='FWForm'] [data-fw-component-type='datepicker'] input[data-role='local-date']");

            // Кнопка “Отправить заявку”
            var submitButton = page.Locator("form[data-tid='FWForm'] button[data-tid='FWSubmitButton']");

            // Заголовок об успешной отправке “Заявка отправлена”
            var successMessageTitle = page.Locator("form[data-tid='FWForm'] [data-tid='FWSuccessBlockTitle']");

            await context.CloseAsync();
            await browser.CloseAsync();
        }
    }
}