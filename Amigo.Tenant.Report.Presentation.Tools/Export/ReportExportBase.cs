using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Report.Presentation.Tools.Export
{
    public abstract class ReportExportBase : IReportExport
    {
        protected DataTable _data;
        protected List<ReportHeader> _header;

        protected bool _striped = false;

        public ReportExportBase()
        {
            _data = new DataTable();
            _header = new List<ReportHeader>();
        }
        public abstract void Export(string name, int initialRow);
        public abstract void SetPreHeader(List<ReportHeader> preHeader);
        public abstract void SetHeader(List<ReportHeader> header, int initialRow);
        public abstract void SetFormat(int postion, string format);

        public void SetBody(IEnumerable<dynamic> body)
        {
            _data = LinqQueryToDataTable(body);
        }
        public void SetStriped(bool striped)
        {
            //TRUE=MOSTRAR ZEBRA; FALSE=OCULTA ZEBRA
            _striped = striped;
        }

        protected static DataTable LinqQueryToDataTable(IEnumerable<dynamic> v)
        {

            var firstRecord = v.FirstOrDefault();
            if (firstRecord == null)
                return null;

            PropertyInfo[] infos = firstRecord.GetType().GetProperties();
            DataTable table = new DataTable();

            foreach (var info in infos)
            {

                Type propType = info.PropertyType;

                if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    table.Columns.Add(info.Name, Nullable.GetUnderlyingType(propType));
                else
                    table.Columns.Add(info.Name, info.PropertyType);
            }


            DataRow row;
            foreach (var record in v)
            {
                row = table.NewRow();
                for (int i = 0; i < table.Columns.Count; i++)
                    row[i] = infos[i].GetValue(record) != null ? infos[i].GetValue(record) : DBNull.Value;

                table.Rows.Add(row);
            }


            table.AcceptChanges();
            return table;
        }

    }
}
