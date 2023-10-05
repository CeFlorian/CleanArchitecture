using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using System.Reflection;

namespace NorthWind.E2ETest
{
    public class E2EChromeDriverBase : IDisposable
    {
        protected readonly ChromeDriver ChromeDriver;

        public E2EChromeDriverBase()
        {
            var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\ChromeDriverResources";
            var chromeOptions = new ChromeOptions();

            chromeOptions.AddArgument("--lang=en");

            if (!Debugger.IsAttached)
            {
                chromeOptions.AddArgument("--headless");
                chromeOptions.AddArgument("--window-size=1920,1080");
            }

            // Para encontrar la version de chromedriver.exe adecuada para la edicion y version del chrome instalado que se utilizara
            // https://googlechromelabs.github.io/chrome-for-testing/
            ChromeDriver = new ChromeDriver(assemblyLocation, chromeOptions);
        }

        public void Dispose()
        {
            ChromeDriver.Dispose();
        }
    }
}
