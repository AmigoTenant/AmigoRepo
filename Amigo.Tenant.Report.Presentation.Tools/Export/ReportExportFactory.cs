using System;

namespace Report.Presentation.Tools.Export
{
    public static class ReportExportFactory
    {
        public static IReportExport Create(string status)
        {
            Enums.ReportExport renum;
            Enum.TryParse<Enums.ReportExport>(status, out renum);

            return SelectFactory(renum, String.Empty);
        }

        public static IReportExport Create(string status, string pathFile)
        {
            Enums.ReportExport renum;
            Enum.TryParse<Enums.ReportExport>(status, out renum);

            return SelectFactory(renum, pathFile);
        }

        private static IReportExport SelectFactory(Enums.ReportExport renum, string pathFile)
        {
            switch (renum)
            {
                case Enums.ReportExport.EXCEL:
                    if(String.IsNullOrEmpty(pathFile))
                        return new ReportExportExcel();
                    else
                        return new ReportExportExcel(pathFile);
                case Enums.ReportExport.PDF:
                    return new ReportExportPdf();
                default:
                    return null;
            }
        }
    }
}
