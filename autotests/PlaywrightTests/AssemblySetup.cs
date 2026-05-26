using Microsoft.Playwright;

namespace PlaywrightTests;

[SetUpFixture]
public class AssemblySetUp
{
    [OneTimeSetUp]
    public void InstallDrivers()
    {
        var exitCode = Program.Main(new[] { "install", "--with-deps", "chromium", "firefox" });
        if (exitCode != 0)
            throw new Exception($"Playwright exited with code {exitCode}");
    }
}