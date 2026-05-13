using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using FluentAssertions;

namespace DatesAndStuff.Web.Tests;

[TestFixture]
public class BlazeDemoPageTests
{
    private IWebDriver driver;
    private const string BaseURL = "https://blazedemo.com";

    [SetUp]
    public void SetupTest()
    {
        driver = new ChromeDriver();
    }

    [TearDown]
    public void TeardownTest()
    {
        try
        {
            driver.Quit();
            driver.Dispose();
        }
        catch (Exception)
        {
        }
    }

    [Test]
    public void MexicoCityToDublin_ShouldHaveAtLeastThreeFlights()
    {
        // Arrange
        driver.Navigate().GoToUrl(BaseURL);
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        var fromPortSelect = wait.Until(ExpectedConditions.ElementExists(By.Name("fromPort")));
        var fromPortSelectElement = new SelectElement(fromPortSelect);
        fromPortSelectElement.SelectByText("Mexico City");

        var toPortSelect = wait.Until(ExpectedConditions.ElementExists(By.Name("toPort")));
        var toPortSelectElement = new SelectElement(toPortSelect);
        toPortSelectElement.SelectByText("Dublin");

        // Act
        var findFlightsButton = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("input[type='submit']")));
        findFlightsButton.Click();

        // Assert
        wait.Until(ExpectedConditions.UrlContains("reserve.php"));
        
        var flightRows = driver.FindElements(By.CssSelector("table tbody tr"));
        var flightCount = flightRows.Count - 1;
        
        flightCount.Should().BeGreaterThanOrEqualTo(3, because: "there should be at least 3 flights between Mexico City and Dublin");
    }
}
