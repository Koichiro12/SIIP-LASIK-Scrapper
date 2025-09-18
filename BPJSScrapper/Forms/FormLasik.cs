using BPJSScrapper.Constant;
using BPJSScrapper.Helpers;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using OpenQA.Selenium.Interactions;
using DocumentFormat.OpenXml.Bibliography;
using System.IO;
using DocumentFormat.OpenXml;

namespace BPJSScrapper.Forms
{
    public partial class FormLasik : Form
    {
        RichTextboxLogger logger;
        TextboxLogger tblogger;
        FileHelper fileHelper;
        ArrayList data;
        SeleniumHelper seleniumHelper;
        bool botIsRunning;
        public FormLasik()
        {
            InitializeComponent();
            fileHelper = new FileHelper();
            logger = new RichTextboxLogger(txt_log);
            tblogger = new TextboxLogger(txt_file);
            data = new ArrayList();
            seleniumHelper = new SeleniumHelper(LinksVal.lasik);
            botIsRunning = false;
        }

        private void btn_browse_Click(object sender, EventArgs e)
        {
            string fpath = fileHelper.GetFilePath("Excel files (*.xlsx)|*.xlsx|All Files (*.*)|*.*)");
            if (fpath == null)
            {
                logger.Process("Excel Tidak Ditemukan, Silahkan Coba Lagi !");
            }
            else
            {
                tblogger.SetTextBox(fpath);
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
                                var stringId = Convert.ToInt32(c.InnerText); // Do some error checking here
                                string text = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText;
                                if(text != "")
                                {
                                    cell.Add(text);
                                }
                               
                            }
                            else
                            {
                                if(c.CellValue != null)
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
            }
        }
        private void btn_start_Click(object sender, EventArgs e)
        {
            Thread tr;
            if(btn_start.Text == "Start")
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
                    tr = new Thread((ThreadStart)delegate
                    {
                        botIsRunning = true;
                        InitiateBot();
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
                seleniumHelper.close();
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
                logger.Process("Bot Di Hentikan !");
            }
            
        }

        private void InitiateBot()
        {
            logger.Process("Checking Data...");
            if(data.Count <= 0)
            {
                logger.In("Data KPJ Kosong, Silahkan Upload File KPJ Terlebih Dahulu !");
                botIsRunning = false;
                btn_start.Invoke((MethodInvoker)delegate
                {
                    btn_start.Enabled = true;
                    btn_start.Text = "Start";
                });
                btn_browse.Invoke((MethodInvoker)delegate
                {
                     btn_browse.Enabled= true;
                });
                return;
            }
            try
            {
                logger.Process("Menjalankan Browser...");
                seleniumHelper.setPageLoadStrategy(PageLoadStrategy.None);
                seleniumHelper.Start();
                seleniumHelper.getDriver().Manage().Window.Maximize();
                
                logger.Process("Berhasil Menjalankan Browser");
                btn_start.Invoke((MethodInvoker)delegate
                {
                    btn_start.Enabled = true;
                    btn_start.Text = "Stop";
                });
                logger.Process("Checking "+data.Count+" Data.....");
                string filepath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+"/hasil_lasik_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xlsx";
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
                        Name = "Hasil Lasik"
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
                    headerRow.Append(cell1, cell2, cell3, cell4, cell5, cell6, cell7);
                    var rowIndex = 2;
                    logger.Process("Starting Auto Checking..");
                    for(int i = 1; i <= data.Count - 1; i++)
                    {

                        if (!botIsRunning)
                        {
                            return;
                        }
                        if(i <= data.Count - 1)
                        {

                          
                            if (((ArrayList)data[i])[0].ToString() != "" && ((ArrayList)data[i])[2].ToString() != "-")
                            {
                               

                                try
                                {
                                    string kpj = ((ArrayList)data[i])[0].ToString();
                                    string nik = ((ArrayList)data[i])[1].ToString();
                                    string nama = ((ArrayList)data[i])[2].ToString();
                                    string tgl_lahir = ((ArrayList)data[i])[3].ToString();
                                    string email = ((ArrayList)data[i])[4].ToString();
                                    string status = "Gagal";
                                    logger.Out("Data Ke - " + i + " : " + kpj);
                                    new WebDriverWait(seleniumHelper.getDriver(), TimeSpan.FromSeconds(600)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//*[@id=\"btn-close-popup-banner\"]")));
                                    if (seleniumHelper.isElementPresent(By.XPath("//*[@id=\"btn-close-popup-banner\"]")))
                                    {
                                        seleniumHelper.getDriver().FindElement(By.XPath("//*[@id=\"btn-close-popup-banner\"]")).Click();
                                        Thread.Sleep(700);

                                        seleniumHelper.getDriver().FindElement(By.XPath("//*[@id=\"regForm\"]/div[2]/div/div/div[1]/input")).Clear();
                                        seleniumHelper.getDriver().FindElement(By.XPath("//*[@id=\"regForm\"]/div[2]/div/div/div[1]/input")).SendKeys(nik);
                                        Thread.Sleep(700);
                                        seleniumHelper.getDriver().FindElement(By.XPath("//*[@id=\"regForm\"]/div[2]/div/div/div[2]/input")).SendKeys(kpj);
                                        Thread.Sleep(700);
                                        seleniumHelper.getDriver().FindElement(By.XPath("//*[@id=\"regForm\"]/div[2]/div/div/div[3]/input")).SendKeys(nama);
                                        seleniumHelper.getDriver().FindElement(By.TagName("body")).Click();
                                        seleniumHelper.getDriver().FindElement(By.TagName("body")).Click();

                                        new WebDriverWait(seleniumHelper.getDriver(), TimeSpan.FromSeconds(600)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("/html/body/div[3]/div/div[3]/button[1]")));
                                        if (seleniumHelper.isElementPresent(By.XPath("/html/body/div[3]/div/div[3]/button[1]")))
                                        {
                                            string hasil = seleniumHelper.getDriver().FindElement(By.XPath("/html/body/div[3]/div/div[3]/button[1]")).Text;
                                            if(hasil == "LANJUTKAN MELALUI JMO")
                                            {
                                                var row = new Row() { RowIndex = (UInt32)rowIndex };
                                                sheetData.Append(row);
                                                
                                                status = "Berhasil";
                                                Cell c1 = new Cell() { CellReference = "A" + rowIndex, DataType = CellValues.String, CellValue = new CellValue(kpj) };
                                                Cell c2 = new Cell() { CellReference = "B" + rowIndex, DataType = CellValues.String, CellValue = new CellValue(nik) };
                                                Cell c3 = new Cell() { CellReference = "C" + rowIndex, DataType = CellValues.String, CellValue = new CellValue(nama) };
                                                Cell c4 = new Cell() { CellReference = "D" + rowIndex, DataType = CellValues.String, CellValue = new CellValue(tgl_lahir) };
                                                Cell c5 = new Cell() { CellReference = "E" + rowIndex, DataType = CellValues.String, CellValue = new CellValue(email) };
                                                Cell c6 = new Cell() { CellReference = "F" + rowIndex, DataType = CellValues.String, CellValue = new CellValue(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) };
                                                Cell c7 = new Cell() { CellReference = "G" + rowIndex, DataType = CellValues.String, CellValue = new CellValue(status) };
                                                row.Append(c1, c2, c3, c4, c5, c6, c7);
                                                rowIndex++;
                                                logger.In(kpj + " : "+status);
                                            }
                                            else
                                            {
                                                logger.In(kpj + " : Gagal");
                                            }

                                        }
                                        Thread.Sleep(3000);
                                        seleniumHelper.getDriver().Navigate().GoToUrl(LinksVal.lasik);
                                    }
                                    
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            else
                            {
                                logger.Process("Skipping Data " + i);
                            }
                        }
                       
                    }
                    logger.Process("Selesai Memproses Data !");
                    botIsRunning = false;
                    seleniumHelper.close();
                    btn_start.Invoke((MethodInvoker)delegate
                    {
                        btn_start.Enabled = true;
                        btn_start.Text = "Start";
                    });
                    btn_browse.Invoke((MethodInvoker)delegate
                    {
                        btn_browse.Enabled = true;
                    });
                }
            }
            catch (Exception ex)
            {
                logger.Out("Error :"+ex.Message);
            }
        }

    }
}
