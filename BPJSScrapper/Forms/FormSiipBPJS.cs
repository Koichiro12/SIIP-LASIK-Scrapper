using BPJSScrapper.Constant;
using BPJSScrapper.Helpers;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                logger.Process("Excel Tidak Ditemukan, Silahkan Coba Lagi !");
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
                                var stringId = Convert.ToInt32(c.InnerText); // Do some error checking here
                                string text = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(stringId).InnerText;
                                cell.Add(text);
                            }
                        }
                        data.Add(cell);
                    }
                }
                logger.Process("File KPJ Ready");
            }
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            InitialzeBot();
        }

        private void InitialzeBot()
        {
            logger.Process("Menjalankan Prosedur");
            logger.Process("Cek Data KPJ excel");
            if (data.Count > 0)
            {
                logger.Process("Menjalankan Browser...");
                try
                {
                    seleniumHelper.Start();
                    logger.Process("Berhasil Menjalankan Browser");
                    logger.Process("Checking.....");
                    new WebDriverWait(seleniumHelper.getDriver(), TimeSpan.FromSeconds(600)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("form-login")));
                    if (seleniumHelper.isElementPresent(By.Id("form-login")))
                    {
                        logger.Process("Silahkan Login dan Masuk Ke Form SIIP yang di tentukan !");
                        new WebDriverWait(seleniumHelper.getDriver(), TimeSpan.FromSeconds(600)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//*[@id=\"accordion-test\"]/div/div[1]/h4/a")));
                        if (seleniumHelper.isElementPresent(By.XPath("//*[@id=\"accordion-test\"]/div/div[1]/h4/a")))
                        {
                            logger.Process("Seluruh Prosedur Telah Terpenuhi, Silahkan Menjalankan BOT dengan klik start");
                        }

                    }
                    else
                    {
                        new WebDriverWait(seleniumHelper.getDriver(), TimeSpan.FromSeconds(600)).Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//*[@id=\"accordion-test\"]/div/div[1]/h4/a")));
                        if (seleniumHelper.isElementPresent(By.XPath("//*[@id=\"accordion-test\"]/div/div[1]/h4/a")))
                        {
                            logger.Process("Seluruh Prosedur Telah Terpenuhi, Silahkan Menjalankan BOT dengan klik start");
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Process("Gagal Menjalankan Browser Error : " + ex.Message);
                }
            }
            else
            {
                logger.Out("Data KPJ (Excel) Kosong / Belum Di set");
            }
        }
    }
}
