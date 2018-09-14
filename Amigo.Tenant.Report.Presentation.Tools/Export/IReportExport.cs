using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Presentation.Tools.Export
{
    public interface IReportExport
    {
        void SetPreHeader(List<ReportHeader> preHeader);
        void SetHeader(List<ReportHeader> header, int initialRow);
        void SetBody(IEnumerable<dynamic> data);
        void SetFormat(int postion, string format);
        void SetStriped(bool _striped);
        void Export(string name, int initialRow);

    }
}
