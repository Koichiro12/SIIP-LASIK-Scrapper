using BPJSScrapper.Helpers;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
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

namespace BPJSScrapper.Forms
{
    public partial class FormLasik : Form
    {
        RichTextboxLogger logger;
        TextboxLogger tblogger;
        FileHelper fileHelper;
        ArrayList data;

        public FormLasik()
        {
            InitializeComponent();
            fileHelper = new FileHelper();
            logger = new RichTextboxLogger(txt_log);
            tblogger = new TextboxLogger(txt_file);
            data = new ArrayList();
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

        }
    }
}
