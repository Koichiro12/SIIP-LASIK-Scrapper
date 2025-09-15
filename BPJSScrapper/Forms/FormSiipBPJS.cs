using BPJSScrapper.Constant;
using BPJSScrapper.Helpers;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace BPJSScrapper.Forms
{
    public partial class FormSiipBPJS : Form
    {

        SeleniumHelper seleniumHelper;
        RichTextboxLogger logger;
        TextboxLogger tbLogger;
        FileHelper fileHelper;
        ArrayList data;
        bool botIsRunning = false;

        public FormSiipBPJS()
        {
            InitializeComponent();
            logger = new RichTextboxLogger(txt_log);
            tbLogger = new TextboxLogger(txt_file);
            seleniumHelper = new SeleniumHelper(LinksVal.bpjs_url);
            fileHelper = new FileHelper();
            data = new ArrayList();
        }

        private void FormSiipBPJS_Load(object sender, EventArgs e)
        {

        }

        private void FormSiipBPJS_FormClosed(object sender, FormClosedEventArgs e)
        {

            seleniumHelper.close();
        }

        private void btn_browse_Click(object sender, EventArgs e)
        {
            string fpath = fileHelper.GetFilePath("Excel files (*.xlsx)|*.xlsx|All Files (*.*)|*.*)");
            if (fpath == null)
            {
                if(txt_file.Text.Length <= 0)
                {
                    logger.Process("Excel Tidak Ditemukan, Silahkan Coba Lagi !");
                }
            }
            else
            {
                tbLogger.SetTextBox(fpath);
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(fpath, false))
                {
                    data.Clear();
                    WorkbookPart workbookPart = doc.WorkbookPart;
                    WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                    SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                    foreach (Row r in sheetData.Elements<Row>())
                    {
                        ArrayList cell = new ArrayList();
                        foreach (Cell c in r.Elements<Cell>())
                        {
                            if (c.DataType != null && c.DataType == CellValues.SharedString)
                            {
                                var stringId = Convert.ToInt32(c.InnerText); 
                                string text = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText;
                                cell.Add(text);
                            }
                        }
                        data.Add(cell);
                    }
                }
                logger.Process("File KPJ Ready");
                logger.Process(data.Count.ToString());
            }
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            Thread tr;
            if (btn_start.Text.Equals("Start"))
            {
                try
                {
                    btn_start.Invoke((MethodInvoker)delegate
                    {
                        btn_start.Enabled = false;
                        btn_start.Text = "Running...";
                    });
                    btn_browse.Invoke((MethodInvoker)delegate
                     {
                         btn_browse.Enabled = false;
                     });
                    btn_browser.Invoke((MethodInvoker)delegate
                    {
                        btn_browser.Enabled = false;
                    });
                    tr = new Thread((ThreadStart)delegate
                    {
                        botIsRunning = true;
                        StartBot();
                    });
                    tr.IsBackground = true;
                    tr.Start();
                    btn_start.Invoke((MethodInvoker)delegate
                    {
                        btn_start.Enabled = true;
                        btn_start.Text = "Stop";
                    });
                }
                catch (Exception ex)
                {
                    logger.In("Error : " + ex.Message);
                }
            }
            else
            {
                botIsRunning = false;
                btn_start.Invoke((MethodInvoker)delegate
                {
                    btn_start.Enabled = true;
                    btn_start.Text = "Start";
                });
                btn_browse.Invoke((MethodInvoker)delegate
                {
                    btn_browse.Enabled = true;
                });
                btn_browser.Invoke((MethodInvoker)delegate
                {
                    btn_browser.Enabled = true;
                });
                logger.Process("Bot Di Hentikan !");
            }
        }  
        private void btn_browser_Click(object sender, EventArgs e)
        {
            Thread tr;
            if (btn_browser.Text.Equals("Open Browser"))
            {
                try
                {
                    btn_browser.Invoke((MethodInvoker)delegate
                    {
                        btn_browser.Enabled = false;
                    });
                    tr = new Thread((ThreadStart)delegate
                    {
                        InitialzeBot();
                    });
                    tr.IsBackground = true;
                    tr.Start();

                }
                catch (Exception ex)
                {
                    logger.In("Error : " + ex.Message);
                }
            }
            else
            {
                try
                {
                    seleniumHelper.getDriver().Quit();
                    btn_browser.Invoke((MethodInvoker)delegate
                    {
                        btn_browser.Text = "Open Browser";
                    });
                    btn_start.Invoke((MethodInvoker)delegate
                    {
                        btn_start.Enabled = false;
                        btn_start.Text = "Start";
                    });
                    logger.Process("Browser Ditutup");
                }
                catch (Exception ex)
                {
                    logger.In("Error : " + ex.Message);
                }
            }
        }

        private void InitialzeBot()
        {
                logger.Process("Menjalankan Browser...");
                try
                {
                    seleniumHelper.Start();
                    seleniumHelper.getDriver().Manage().Window.Maximize();
                    logger.Process("Berhasil Menjalankan Browser");
                    btn_browser.Invoke((MethodInvoker)delegate
                    {
                        btn_browser.Enabled = true;
                        btn_browser.Text = "Close Browser";
                    });
                    logger.Process("Checking.....");
                    new WebDriverWait(seleniumHelper.getDriver(), TimeSpan.FromSeconds(600)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("form-login")));
                    if (seleniumHelper.isElementPresent(By.Id("form-login")))
                    {
                        logger.Process("Silahkan Login dan Masuk Ke Form SIIP yang di tentukan !");
                        new WebDriverWait(seleniumHelper.getDriver(), TimeSpan.FromSeconds(6000)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//*[@id=\"wrapper\"]/div[1]/div[2]/div/ul[1]/li/button")));
                        if (seleniumHelper.isElementPresent(By.XPath("//*[@id=\"wrapper\"]/div[1]/div[2]/div/ul[1]/li/button")))
                        {
                            seleniumHelper.getDriver().Navigate().GoToUrl(LinksVal.form_url);
                            new WebDriverWait(seleniumHelper.getDriver(), TimeSpan.FromSeconds(6000)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//*[@id=\"accordion-test\"]/div/div[1]/h4/a")));
                            if (seleniumHelper.isElementPresent(By.XPath("//*[@id=\"accordion-test\"]/div/div[1]/h4/a")))
                            {
                                logger.Process("Browser Siap !");
                                btn_start.Invoke((MethodInvoker)delegate
                                {
                                    btn_start.Enabled = true;
                                    btn_start.Text = "Start";
                                });
                            }
                        }

                    }
                    else
                    {
                        new WebDriverWait(seleniumHelper.getDriver(), TimeSpan.FromSeconds(6000)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//*[@id=\"accordion-test\"]/div/div[1]/h4/a")));
                        if (seleniumHelper.isElementPresent(By.XPath("//*[@id=\"accordion-test\"]/div/div[1]/h4/a")))
                        {
                            logger.Process("Browser Siap !");
                            btn_start.Invoke((MethodInvoker)delegate
                            {
                                btn_start.Enabled = true;
                                btn_start.Text = "Start";
                            });
                        }
                    }
                }catch(WebDriverTimeoutException ex)
                {
                    logger.Out("Waktu Tunggu Menuju Form Habis / Timeout . Silahkan Membuka Browser Kembali");
                    seleniumHelper.getDriver().Quit();
                    btn_browser.Invoke((MethodInvoker)delegate
                    {
                        btn_browser.Enabled = true;
                        btn_browser.Text = "Open Browser";
                    });
                }
                catch(Exception ex)
                {

                }
            
        }

        private void StartBot()
        {
            logger.Process("Memulai Bot....");
            if(data.Count <= 0)
            {
                logger.Out("Data KPJ Kosong, Silahkan Upload File KPJ Terlebih Dahulu !");
                botIsRunning = false;
                btn_start.Invoke((MethodInvoker)delegate
                {
                    btn_start.Enabled = true;
                    btn_start.Text = "Start";
                });
                btn_browse.Invoke((MethodInvoker)delegate
                {
                    btn_browse.Enabled = true;
                });
                btn_browser.Invoke((MethodInvoker)delegate
                {
                    btn_browser.Enabled = true;
                });
                return;
            }
          
            for (int i = 0; i <= data.Count - 1; i++)
            {
                if (!botIsRunning)
                {
                    break;
                }
                if (i <= data.Count)
                {
                    if (((ArrayList)data[i])[0].ToString() == "")
                    {
                        return;
                    }
                    try
                    {
                        string kpj = ((ArrayList)data[i])[0].ToString();
                        string nik = "-";
                        string nama = "-";
                        string tgl_lahir = "-";
                        string email = "-";
                        string status = "Gagal";
                        string ket = "-";
                        logger.Out("Data Ke - " + (i + 1) + " : " + kpj);
                        if (seleniumHelper.isElementPresent(By.XPath("//*[@id=\"accordion-test\"]/div/div[1]/h4/a")))
                        {

                            new WebDriverWait(seleniumHelper.getDriver(), TimeSpan.FromSeconds(600)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"collapseOne\"]/div/div/div/button[1]")));
                            seleniumHelper.getDriver().FindElement(By.XPath("//*[@id=\"collapseOne\"]/div/div/div/button[1]")).Click();
                            Thread.Sleep(700);
                            seleniumHelper.getDriver().FindElement(By.Id("kpj")).Clear();
                            seleniumHelper.getDriver().FindElement(By.Id("kpj")).SendKeys(kpj);
                            seleniumHelper.getDriver().FindElement(By.XPath("//*[@id=\"collapseTwo\"]/div/div/div/div[2]/a/button")).Click();

                            new WebDriverWait(seleniumHelper.getDriver(), TimeSpan.FromSeconds(600)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("/html/body/div[3]")));
                            if (seleniumHelper.isElementPresent(By.XPath("/html/body/div[3]")))
                            {
                                Thread.Sleep(100);
                                new WebDriverWait(seleniumHelper.getDriver(), TimeSpan.FromSeconds(600)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("/html/body/div[3]/div/h2")));
                                if (seleniumHelper.isElementPresent(By.XPath("/html/body/div[3]/div/h2")) && seleniumHelper.isElementPresent(By.XPath("/html/body/div[3]/div/div[6]")) && seleniumHelper.isElementPresent(By.XPath("/html/body/div[3]/div/button[1]")))
                                {
                                    string sts = seleniumHelper.getDriver().FindElement(By.XPath("/html/body/div[3]/div/h2")).Text;
                                    string rawName = seleniumHelper.getDriver().FindElement(By.XPath("/html/body/div[3]/div/div[6]")).Text;

                                    if (sts.Contains("Berhasil!"))
                                    {
                                        nama = fileHelper.StringCatcher(rawName, "Tenaga Kerja atas nama ", " terdaftar sebagai peserta BPJS Ketenagakerjaan.");
                                        seleniumHelper.getDriver().FindElement(By.XPath("/html/body/div[3]/div/button[1]")).Click();
                                        new WebDriverWait(seleniumHelper.getDriver(), TimeSpan.FromSeconds(600)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("no_identitas")));
                                        if (seleniumHelper.isElementPresent(By.Id("no_identitas")) && seleniumHelper.isElementPresent(By.Id("tgl_lahir")) && seleniumHelper.isElementPresent(By.Id("email")))
                                        {
                                            //Set Data
                                            nik = seleniumHelper.getDriver().FindElement(By.Id("no_identitas")).GetAttribute("value");
                                            tgl_lahir = seleniumHelper.getDriver().FindElement(By.Id("tgl_lahir")).GetAttribute("value");
                                            email = seleniumHelper.getDriver().FindElement(By.Id("email")).GetAttribute("value");
                                            logger.In(kpj + ": " + nik + " " + nama + " " + tgl_lahir + " " + email);

                                            //Save Excels
                                        }

                                    }
                                    else
                                    {
                                        status = "Gagal";
                                        logger.In(rawName);
                                        ket = rawName;
                                    }
                                    seleniumHelper.getDriver().Navigate().GoToUrl(LinksVal.form_url);
                                    new WebDriverWait(seleniumHelper.getDriver(), TimeSpan.FromSeconds(600)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"accordion-test\"]/div/div[1]/h4/a")));
                                }
                            }
                            else
                            {
                                Thread.Sleep(100);
                                //Save Excel
                                logger.In("Data Tidak Di temukan, Karena Kendala. Memulai Ulang Data !");
                                //Kembali Dan Reset Form
                                seleniumHelper.getDriver().Navigate().Refresh();
                                new WebDriverWait(seleniumHelper.getDriver(), TimeSpan.FromSeconds(600)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"accordion-test\"]/div/div[1]/h4/a")));
                            }
                        }
                    }
                    catch (WebDriverTimeoutException ex)
                    {
                        logger.Out("TimeOut,Retrying...");
                        i -= 1;
                    }
                    catch (Exception ex)
                    {
                        logger.Out("Error :" + ex.Message);
                        logger.Out("Retrying...");
                        i -= 1;
                        seleniumHelper.getDriver().Navigate().GoToUrl(LinksVal.form_url);
                        new WebDriverWait(seleniumHelper.getDriver(), TimeSpan.FromSeconds(600)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//*[@id=\"accordion-test\"]/div/div[1]/h4/a")));
                    }
                }
                
                    
            }
            logger.Process("Selesai Memproses Data !");
            botIsRunning = false;
            btn_start.Invoke((MethodInvoker)delegate
            {
                btn_start.Enabled = true;
                btn_start.Text = "Start";
            });
            btn_browse.Invoke((MethodInvoker)delegate
            {
                btn_browse.Enabled = true;
            });
            btn_browser.Invoke((MethodInvoker)delegate
            {
                btn_browser.Enabled = true;
            });
        }
       
    }
}
