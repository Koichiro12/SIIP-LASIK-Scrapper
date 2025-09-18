using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPJSScrapper.Helpers
{


    public class SeleniumHelper
    {

        IWebDriver driver;
        ChromeDriverService cds;
        public string Url { get; set; }

        PageLoadStrategy pgs;

        public SeleniumHelper(string url)
        {
            Url = url;
            cds = ChromeDriverService.CreateDefaultService();
            cds.HideCommandPromptWindow = true;
            pgs = PageLoadStrategy.Default;
        }

        public IWebDriver getDriver()
        {
            return this.driver;
        }

        public bool isElementPresent(By by)
        {
            try
            {
                this.driver.FindElement(by);
                return true;
            }catch { 
                return false; 
            }

        }

        public void setPageLoadStrategy(PageLoadStrategy pgs)
        {
            this.pgs = pgs;
        }

        public void Start()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("no-sandbox");
            options.PageLoadStrategy = pgs;
            driver = new ChromeDriver(cds,options);
            driver.Navigate().GoToUrl(Url);
        }
        public void close()
        {
            if(driver != null)
            {
                driver.Quit();
            }
        }
    }
}
