using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Presentation.Tools.Export
{
    public class ReportExportPdf : ReportExportBase
    {
        public override void Export(string name, int initialRow)
        {
            throw new NotImplementedException();
        }

        public override void SetFormat(int postion, string format)
        {
            throw new NotImplementedException();
        }

        public override void SetHeader(List<ReportHeader> header, int initialRow)
        {
            throw new NotImplementedException();
        }
        public override void SetPreHeader(List<ReportHeader> header)
        {
            throw new NotImplementedException();
        }
    }
}
