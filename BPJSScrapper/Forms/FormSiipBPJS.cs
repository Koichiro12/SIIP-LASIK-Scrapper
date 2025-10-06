using BPJSScrapper.Constant;
using BPJSScrapper.Helpers;
using DocumentFormat.OpenXml;
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

            if(botIsRunning)
            {
                botIsRunning = false;
                Thread.Sleep(2000);
            }
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
                                if(text != "")
                                {
                                    cell.Add(text);
                                }

                            }
                            else
                            {
                                if (c.CellValue != null)
                                {
                                    cell.Add(c.CellValue.Text);
                                }

                            }
                        }
                        if(cell.Count > 0)
                        {
                            data.Add(cell);
                        }
                      
                    }
                }
                logger.Process("File KPJ Ready");
                logger.Out(data.Count.ToString());
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
                    btn_start.Enabled = false;
                    btn_start.Text = "Start";
                });
                btn_browse.Invoke((MethodInvoker)delegate
                {
                    btn_browse.Enabled = true;
                });
                btn_browser.Invoke((MethodInvoker)delegate
                {
                    btn_browser.Enabled = false;
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
                        logger.Process("Silahkan Login !");
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
            
            string filepath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/hasil_SIIP_BPJS_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
            using(var workbook = SpreadsheetDocument.Create(filepath, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook,true))
            {
                var workbookPart = workbook.AddWorkbookPart();
                workbook.WorkbookPart.Workbook = new Workbook();
                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());
                var sheets = workbook.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
                var sheet = new Sheet()
                {
                    Id = workbook.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Hasil SIIP BPJS"
                };
                sheets.Append(sheet);
                var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
                var headerRow = new Row() { RowIndex = 1 };
                sheetData.Append(headerRow);

                Cell cell1 = new Cell() { CellReference = "A1", DataType = CellValues.String, CellValue = new CellValue("No KPJ") };
                Cell cell2 = new Cell() { CellReference = "B1", DataType = CellValues.String, CellValue = new CellValue("NIK") };
                Cell cell3 = new Cell() { CellReference = "C1", DataType = CellValues.String, CellValue = new CellValue("Nama") };
                Cell cell4 = new Cell() { CellReference = "D1", DataType = CellValues.String, CellValue = new CellValue("Tgl Lahir") };
                Cell cell5 = new Cell() { CellReference = "E1", DataType = CellValues.String, CellValue = new CellValue("Email") };
                Cell cell6 = new Cell() { CellReference = "F1", DataType = CellValues.String, CellValue = new CellValue("Tanggal & Waktu") };
                Cell cell7 = new Cell() { CellReference = "G1", DataType = CellValues.String, CellValue = new CellValue("Status") };
                Cell cell8 = new Cell() { CellReference = "H1", DataType = CellValues.String, CellValue = new CellValue("Ket") };
                headerRow.Append(cell1,cell2,cell3,cell4,cell5,cell6,cell7,cell8);
                var rowIndex = 2;
                for (int i = 0; i <= data.Count - 1; i++)
                {
                    if (!botIsRunning)
                    {
                        break;
                    }
                    if (i <= data.Count - 1)
                    {
                        if (((ArrayList)data[i])[0].ToString() == "")
                        {
                            return;
                        }
                        try
                        {
                            var kpj =  ((ArrayList)data[i])[0].ToString();
                            var nik = "-";
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

                                        var row = new Row() { RowIndex = (UInt32)rowIndex };
                                        sheetData.Append(row);
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
                                                if (!email.ToLower().Trim().Contains("@gmail"))
                                                {
                                                    logger.In(kpj + ": " + nik + " " + nama + " " + tgl_lahir + " " + email);
                                                    Cell c1 = new Cell() { CellReference = "A" + rowIndex, DataType = CellValues.String, CellValue = new CellValue(kpj) };
                                                    Cell c2 = new Cell() { CellReference = "B" + rowIndex, DataType = CellValues.String, CellValue = new CellValue(nik) };
                                                    Cell c3 = new Cell() { CellReference = "C" + rowIndex, DataType = CellValues.String, CellValue = new CellValue(nama) };
                                                    Cell c4 = new Cell() { CellReference = "D" + rowIndex, DataType = CellValues.String, CellValue = new CellValue(tgl_lahir) };
                                                    Cell c5 = new Cell() { CellReference = "E" + rowIndex, DataType = CellValues.String, CellValue = new CellValue(email) };
                                                    Cell c6 = new Cell() { CellReference = "F" + rowIndex, DataType = CellValues.String, CellValue = new CellValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) };
                                                    Cell c7 = new Cell() { CellReference = "G" + rowIndex, DataType = CellValues.String, CellValue = new CellValue(sts) };
                                                    Cell c8 = new Cell() { CellReference = "H" + rowIndex, DataType = CellValues.String, CellValue = new CellValue(ket) };
                                                    row.Append(c1, c2, c3, c4, c5, c6, c7, c8);
                                                    rowIndex++;
                                                }
                                                else
                                                {
                                                    logger.In(kpj + ": Gagal");
                                                }
                                              

                                            }

                                        }
                                        else
                                        {
                                            status = "Gagal";
                                            logger.In(rawName);
                                            ket = rawName;
                                        }
                                        //Save Excels
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
                    Thread.Sleep(1000);
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
