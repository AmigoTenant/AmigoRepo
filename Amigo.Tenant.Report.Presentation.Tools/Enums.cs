using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report.Presentation.Tools
{
    public class Enums
    {
        public enum EmailType
        {
            EIR,
            EDC,//DataCorrection,
            APT,
            APTStock,
            COM,
            REES//MR ESTIMATE REPAIR
        }
        public enum ReportExport
        {
            EXCEL,
            PDF
        }


    }
    public enum EquipmentSizeValue
    {
        Fourty = 40,
        Twenty = 20
    }
}
