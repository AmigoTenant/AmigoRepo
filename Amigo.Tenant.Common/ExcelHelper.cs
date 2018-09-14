using System.Collections.Generic;
using System.Text;

namespace Amigo.Tenant.Common
{
    public class ExcelHelper
    {
        public static string GetHeaderDetail(List<string> headers)
        {
            var textHeaders = string.Empty;
            foreach (var item in headers)
            {
                textHeaders += item + ",";
            }
            return textHeaders;
        }

        public static string StringToCSVCell(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                bool mustQuote = (str.Contains(",") || str.Contains("\"") || str.Contains("\r") || str.Contains("\n"));
                if (mustQuote)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("\"");
                    foreach (char nextChar in str)
                    {
                        sb.Append(nextChar);
                        if (nextChar == '"')
                            sb.Append("\"");
                    }
                    sb.Append("\"");
                    return sb.ToString();
                }
            }
            return str;
        }
    }
}
