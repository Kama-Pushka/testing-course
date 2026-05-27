using System.Threading.Tasks;
using Microsoft.Playwright;
using NUnit.Framework;

namespace PlaywrightTests;

public class ActionsHomework
{
    [Test]
    public async Task Diadoc_OrderFromWidget_Success()
    {
        using var playwright = await Playwright.CreateAsync();

        var browser = await playwright.Chromium.LaunchAsync(new()
        {
            Headless = false,
            Args = new[] { "--start-maximized" }
        });

        var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();

        // 1. Перейти на страницу https://kontur.ru/lp/qa-auto
        await page.GotoAsync("https://kontur.ru/lp/qa-auto");
        
        var tryButton = page.Locator("#try-button");
        var sendOrderButton = page.Locator("[data-buy-product-button=''][data-long-text='Отправить заявку']");
        
        // 2. Кликнуть по кнопке Попробовать в обложке
        await tryButton.ClickAsync();
        
        // 3. Дождаться скролла вниз и нажать кнопку Отправить заявку
        await sendOrderButton.WaitForAsync();
        await sendOrderButton.ClickAsync();
        
        var widgetForm = page.Locator("form[data-tid='FWForm']");
        var surnameInput = page.Locator("form[data-tid='FWForm'] input[data-field-role='surname']");
        var nameInput = page.Locator("form[data-tid='FWForm'] input[data-field-role='name']");
        var regionSelect = page.Locator("form[data-tid='FWForm'] [data-fw-component-type='region'] select");
        var emailInput = page.Locator("form[data-tid='FWForm'] input[type='email']");
        var emailInputValidationError = page.Locator("form[data-tid='FWForm'] [data-fw-component-type='email'] [class='field-validation-error']");
        var phoneInput = page.Locator("form[data-tid='FWForm'] [data-fw-component-type='phone'] input");
        var companyNameInput = page.Locator("form[data-tid='FWForm'] [data-fw-component-type='company'] input[data-field-role='company-name']");
        var organizationSuggest = page.Locator("form[data-tid='FWForm'] [data-fw-component-type='company'] [class='organization-suggest-container']");
        var organizationSuggestFirstItem = page.Locator("form[data-tid='FWForm'] [data-fw-component-type='company'] [class='organization-suggest-container'] [data-index='0']");
        var contragentsFileUploaderInput = page.Locator("form[data-tid='FWForm'] [data-fw-component-type='fileuploader'] input[data-role='fileUploader']");
        var fileNameLabel = page.Locator("form[data-tid='FWForm'] [data-fw-component-type='fileuploader'] [class='file-loader__filename-wrap']");
        var customDayCheckbox = page.Locator("form[data-tid='FWForm'] [data-fw-component-type='checkbox'] label");
        var datePicker = page.Locator("form[data-tid='FWForm'] [data-fw-component-type='datepicker'] input[data-role='local-date']");
        var submitButton = page.Locator("form[data-tid='FWForm'] button[data-tid='FWSubmitButton']");
        var successMessageTitle = page.Locator("form[data-tid='FWForm'] [data-tid='FWSuccessBlockTitle']");
        
        // 4. Прокрутить страницу до Виджета заявки
        await widgetForm.ScrollIntoViewIfNeededAsync();
        
        // 5. Дождаться появления Виджета заявки
        await widgetForm.WaitForAsync();
        
        // 6. Заполнить поля Фамилия и Имя
        await surnameInput.FillAsync("Фамилия");
        await nameInput.FillAsync("Имя");
        
        // 7. В селекте Регион выбрать Свердловская область
        await regionSelect.SelectOptionAsync("Свердловская область");
        
        // 8. В поле Электронная почта для связи ввести - невалидный email
        await emailInput.FillAsync("notvalid");
        
        // 9. Снять фокус (например, нажать Tab)
        await emailInput.PressAsync("Tab");
        
        // 10. Проверить, что появилась ошибка Некорректный адрес электронной почты
        var emailInputValidationErrorText = (await emailInputValidationError.TextContentAsync())?.Trim();
        Assert.That(emailInputValidationErrorText, 
             Is.EqualTo("Некорректный адрес электронной почты"),
             "При вводе невалидного email должен появится текст ошибки \"Некорректный адрес электронной почты\".");
        
        // 11. Очистить поле, ввести - валидный email
        await emailInput.FillAsync("valid@mail.com");
        
        // 12. Заполнить поле Телефон для связи
        await phoneInput.FillAsync("1234567890");

        // 13. В поле Ваша организация ввести Контур
        await companyNameInput.FillAsync("Контур");
        
        // 14. Дождаться появления Саджеста организаций
        await organizationSuggest.WaitForAsync();
        
        // 15. Выбрать первый элемент из выпавшего списка
        await organizationSuggestFirstItem.ClickAsync();
        
        // 16. В поле Список контрагентов загрузить .docx файл
        await contragentsFileUploaderInput.SetInputFilesAsync("../../../test.docx");
        
        // 17. Дождаться появления лейбла с названием файла
        await fileNameLabel.WaitForAsync();
        
        // 18. Поставить галочку в поле Связаться в определённый день
        await customDayCheckbox.SetCheckedAsync(true);
        
        // 19. Дождаться появления поля Дата
        await datePicker.WaitForAsync();
        
        // 20. Установить дату доставки = сегодня + 8 дней
        var formattedDate = 8;
        await page.EvaluateAsync($@"$(""[data-role='datepicker'] input"").datepicker(""setDate"",""{formattedDate}"");");
        
        // 21. Дождаться появления сообщение об успешно отправленной заявке
        await submitButton.ClickAsync();
        
        // 22. Дождаться появления Виджета заявки
        await successMessageTitle.WaitForAsync();
        
        var successMessageTitleText = (await successMessageTitle.TextContentAsync())?.Trim();
        Assert.That(successMessageTitleText, 
            Is.EqualTo("Заявка отправлена!"),
            "Сообщение об успешно отправленной заявке содержит текст \"Заявка отправлена!\".");
        
        await context.CloseAsync();
        await browser.CloseAsync();
    }
}