using Newtonsoft.Json;
using NorthWind.Sales.BusinessObjects.DTOs.CreateOrder;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace NorthWind.E2ETest.E2E
{
    public class SwaggerSendRequestPostOrderPageTest : E2EChromeDriverBase
    {
        public SwaggerSendRequestPostOrderPageTest() : base()
        {
        }


        /*
         * Para esta prueba si es necesario que el sitio web (front-end) se encuentre ejecutandose para proporcionar la url
         */
        [Fact]
        public async Task SuccessReponse_When_SendRequest_PostOrder()
        {

            var orderDetails = new List<CreateOrderDetailDTO>
            {
                new CreateOrderDetailDTO { ProductId = 1, UnitPrice = 10.99M, Quantity = 2 },
                new CreateOrderDetailDTO { ProductId = 2, UnitPrice = 5.99M, Quantity = 3 }
            };

            var order = new CreateOrderDTO
            {
                CustomerId = new Random().Next(0, 99999).ToString(),
                ShipAddress = "1234 Main St",
                ShipCity = "City",
                ShipCountry = "Country",
                ShipPostalCode = "44444",
                OrderDetails = orderDetails
            };

            string json = JsonConvert.SerializeObject(order);

            //await NavigateAndFillOutRegistrationForm(string.Empty, "info@dangl-it.com", "LongEnoughPassword");
            await NavigateAndFillOutRequestForm(json);
            var statusCodeResult = ResponseCodeTextIsSuccess();
            Assert.True(statusCodeResult);
        }

        private async Task NavigateAndFillOutRequestForm(string json)
        {
            ChromeDriver.Url = "https://localhost:7115/swagger/index.html";

            //var registerButton = _chromeDriver.FindElement(By.XPath("//a[.='Register']"));
            //registerButton.Click();

            //var usernameFormField = _chromeDriver.FindElement(By.XPath("//input[@name='username']"));
            //var emailFormField = _chromeDriver.FindElement(By.XPath("//input[@name='email']"));
            //var passwordFormField = _chromeDriver.FindElement(By.XPath("//input[@name='password']"));

            //usernameFormField.SendKeys(username);
            //emailFormField.SendKeys(email);
            //passwordFormField.SendKeys(password);

            //var acceptTosCheckbox = _chromeDriver.FindElement(By.ClassName("mat-checkbox-inner-container"));
            //acceptTosCheckbox.Click();

            // Tiempo mientras la web renderiza sus componentes
            await Task.Delay(100);

            var expanderButton = ChromeDriver.FindElement(By.Id("operations-Order-post_api_Order")).FindElement(By.ClassName("opblock-summary-control"));
            expanderButton.Click();

            // Tiempo mientras la web renderiza los componentes que se habilitaran en la seccion correspondiente
            await Task.Delay(100);

            var tryButton = ChromeDriver.FindElement(By.Id("operations-Order-post_api_Order")).FindElement(By.ClassName("try-out__btn"));
            tryButton.Click();

            await Task.Delay(100);

            var bodyFormField = ChromeDriver.FindElement(By.Id("operations-Order-post_api_Order")).FindElement(By.XPath("//textarea[@class='body-param__text']"));
            bodyFormField.Clear();
            bodyFormField.SendKeys(json);

            var executeButton = ChromeDriver.FindElement(By.Id("operations-Order-post_api_Order")).FindElement(By.XPath("//button[@class='btn execute opblock-control__btn']"));
            executeButton.Click();

            // Tiempo mientras se resuelve la solicitd
            await Task.Delay(1000);
        }

        private bool ResponseCodeTextIsSuccess()
        {
            var columnCode = ChromeDriver.FindElement(By.Id("operations-Order-post_api_Order")).FindElement(By.XPath("//td[@class='response-col_status']"));
            return columnCode.Text.Equals("200");
        }
    }
}
