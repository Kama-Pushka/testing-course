using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace autotests;

public class Tests
{
    private IWebDriver driver;
    private WebDriverWait wait;
    
    [SetUp]
    public void Setup()
    {
        driver = new FirefoxDriver();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        driver.Manage().Window.Maximize();
        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
    }
    
    [TearDown]
    public void TearDown()
    {
        driver.Quit();
        driver.Dispose();
    }

    [Test]
    public void AuthorizationTest() // тест аутентификации
    {
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/");

        var login = driver.FindElement(By.Id("Username"));
        login.SendKeys(Secrets.Username);
        
        var password = driver.FindElement(By.Id("Password"));
        password.SendKeys(Secrets.Password);
        
        var enter = driver.FindElement(By.Name("button"));
        enter.Click();
        
        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='Title']")));
        Assert.That(driver.Title, Is.EqualTo("Новости"), "После аутентификации не произошел переход на страницу Новости.");
    }

    [Test]
    public async Task NavigationMenuElementCommunitiesTest() // тест перехода в меню "Сообщества"
    {
        await Auth();
        
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/news");
        
        var community = driver.FindElement(By.CssSelector("[data-tid=\"Community\"]"));
        community.Click();
        
        wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("[data-tid='Title']")));
        Assert.That(driver.Title, Is.EqualTo("Сообщества"), "После перехода на вкладку сообщества не обнаружен заголовок \"Сообщества\".");
    }
    
    [Test]
    public async Task SearchTest() // тест поисковой строки
    {
        await Auth();
        
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/news");
        
        var search = driver.FindElement(By.CssSelector("[data-tid=\"SearchBar\"]"));
        search.Click(); 
        
        var searchInput = driver.FindElement(By.CssSelector("[placeholder=\"Поиск сотрудника, подразделения, сообщества, мероприятия\"]"));
        searchInput.SendKeys("аптуликсанов руслан германович");
        
        var employee = driver.FindElement(By.XPath("//button[@data-tid='ComboBoxMenu__item'][.//div[@title='Аптуликсанов Руслан Германович']]"));
        employee.Click();

        var employeeName = driver.FindElement(By.CssSelector("[data-tid='EmployeeName']"));
        Assert.That(employeeName.Text, Is.EqualTo("Аптуликсанов Руслан Германович"), "Имя пользователя на странице профиля не соответствует результату поиска.");
    }
    
    [Test]
    public async Task ProfileTest() // тест перехода на страницу "Мой профиль"
    {
        await Auth();
        
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/news");
        
        var profileMenu = driver.FindElement(By.CssSelector("[data-tid=\"ProfileMenu\"]"));
        profileMenu.Click(); 
        
        var profile = driver.FindElement(By.CssSelector("[data-tid=\"Profile\"]"));
        profile.Click();
        
        var employeeName = driver.FindElement(By.CssSelector("[data-tid=\"EmployeeName\"]"));
        Assert.That(employeeName.Text, Is.EqualTo(Secrets.ProfileName), "Отображаемое имя на странице \"Мой профиль\" не совпадает с именем авторизованного пользователя.");
    }
    
    [Test]
    public async Task LogoutTest() // тест выхода из системы
    {
        await Auth();
        
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/news");
        
        var profileMenu = driver.FindElement(By.CssSelector("[data-tid=\"ProfileMenu\"]"));
        profileMenu.Click(); 
        
        var logout = driver.FindElement(By.CssSelector("[data-tid=\"Logout\"]"));
        logout.Click();

        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru");
        wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("Username")));
        Assert.That(driver.Url, Does.Contain("/Account/Login"), "После попытки доступа к закрытому ресурсу не произошло перенаправления на страницу аутентификации.");
    }
    
    // решил использовать апишку для аутентификации на портал
    private static async Task<string> GetJwtToken()
    {
        var url = "https://staff-testing.testkontur.ru/api/v1/auth";
        var body = $"{{\"email\":\"{Secrets.Username}\",\"password\":\"{Secrets.Password}\"}}";
        
        using var client = new HttpClient();
        var content = new StringContent(body, Encoding.UTF8, "application/json");
        
        var response = await client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    private async Task Auth()
    {
        var token = await GetJwtToken();
        driver.Navigate().GoToUrl("https://staff-testing.testkontur.ru/");
        
        var js = (IJavaScriptExecutor)driver;
        js.ExecuteScript($"localStorage.setItem('token', '{token}');"); // не уверен, что это лучший способ инжектить авторизационный токен, но он по крайней мере работает
    }
}