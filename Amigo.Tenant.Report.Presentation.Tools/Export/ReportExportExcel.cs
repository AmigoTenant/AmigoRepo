using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using System.Data;
using System.IO;

namespace Report.Presentation.Tools.Export
{
    public class ReportExportExcel : ReportExportBase
    {
        //private ExcelPackage excelPackages = new ExcelPackage();
        private ExcelPackage _pck;
        private ExcelWorksheet _ws;

        public ReportExportExcel()
        {
            _pck = new ExcelPackage();
            _ws = _pck.Workbook.Worksheets.Add("Sheet1");
        }

        public ReportExportExcel(string pathFile)
        {
            var fi = new FileInfo(pathFile);
            _pck = new ExcelPackage(fi);
            _ws = _pck.Workbook.Worksheets[1];
        }

        public override void Export(string name, int initialRow=3)
        {
            Color colorGray = System.Drawing.ColorTranslator.FromHtml("#D9D9D9");
            try
            {
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.Charset = "";
                System.Web.HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
                System.Web.HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + name + ".xlsx");

                if (_data != null)
                {
                    _ws.Cells["A"+initialRow].LoadFromDataTable(_data, PrintHeaders: false);
                    _ws.Cells.AutoFitColumns();
                    //_ws.Cells.Style.WrapText = true;

                    if (_striped)
                    {
                        using (ExcelRange rng = _ws.Cells[initialRow, 1, (_data.Rows.Count == 0 ? 1 : _data.Rows.Count) + 1, _header.Count])
                        {
                            rng.Style.Border.Top.Style = ExcelBorderStyle.Hair;
                            rng.Style.Border.Bottom.Style = ExcelBorderStyle.Hair;
                            rng.Style.Border.Left.Style = ExcelBorderStyle.Hair;
                            rng.Style.Border.Right.Style = ExcelBorderStyle.Hair;

                            rng.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                            rng.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                            rng.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                            rng.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

                            rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            rng.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                        }
                        showStriped(_data);
                    }
                    

                    //if (_striped)
                    //    showStriped(_data);
                }

            }
            catch (Exception e)
            {
                System.Web.HttpContext.Current.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                System.Web.HttpContext.Current.Response.Write(e);
                throw e;
            }
            finally
            {
                System.Web.HttpContext.Current.Response.BinaryWrite(_pck.GetAsByteArray());
                _pck.Dispose();
                System.Web.HttpContext.Current.Response.Flush();
                System.Web.HttpContext.Current.Response.End();
            }
        }

        private void showStriped(DataTable _data)
        {
            bool striped = true;
            Color colorGray = System.Drawing.ColorTranslator.FromHtml("#D9D9D9");
            for (int i = 0; i < _data.Rows.Count; i++)
            {
                if (!striped)
                {
                    using (ExcelRange rng = _ws.Cells[i + 1, 1, i + 1, _header.Count])
                    {
                        rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        rng.Style.Fill.BackgroundColor.SetColor(colorGray);
                    }
                    striped = true;
                }
                else
                    striped = false;

            }
        }

        public override void SetFormat(int postion, string format)
        {
            _ws.Column(postion).Style.Numberformat.Format = format;
        }

        public override void SetHeader(List<ReportHeader> header, int initialRow=2)
        {
            _header = header;

            foreach (var item in header)
            {
                _ws.Cells[initialRow, item.Position].Value = item.Name;
                _ws.Cells[initialRow, item.Position].Style.Fill.PatternType = ExcelFillStyle.Solid;
                _ws.Cells[initialRow, item.Position].Style.Fill.BackgroundColor.SetColor(Color.Black);
                _ws.Cells[initialRow, item.Position].Style.Font.Color.SetColor(Color.White);
            }
            _ws.View.FreezePanes(initialRow +1, 1);

            //using (ExcelRange rng = _ws.Cells[2, 1, 2, header.Count])
            //{
            //    rng.AutoFilter = true;
            //}
        }

        public override void SetPreHeader(List<ReportHeader> preHeader)
        {
            foreach (var item in preHeader)
            {
                _ws.Cells[item.Position, item.PositionY].Value = item.Name;
            }
            
        }
    }
}
