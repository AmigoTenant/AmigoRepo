using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Amigo.Tenant.Common
{
    public static class XmlHelper
    {
        public static XmlDocument GenerateXmlServiceRequest<T>(T request1, string xmlServiceName)
        {
            XmlDocument xml = new XmlDocument();
            XmlNode docNode = xml.CreateXmlDeclaration("1.0", "UTF-16", null);
            xml.AppendChild(docNode);

            XmlNode serviceNode = xml.CreateElement(xmlServiceName);
            xml.AppendChild(serviceNode);
            XmlNode parameterNode = xml.CreateElement("Request_Service");
            serviceNode.AppendChild(parameterNode);

            AppendPropertyNodesForRequestOrResponse(xml, parameterNode, request1);

            //XmlWriterSettings settings = new XmlWriterSettings();
            //settings.Indent = true;
            //XmlWriter writer = XmlWriter.Create(@"D:\Descargas\test1.xml", settings);

            //xml.Save(writer);

            return xml;
        }

        public static XmlDocument GenerateXmlServiceRequest<T>(T request1, string xmlServiceName, bool includeDateTime)
        {
            XmlDocument xml = new XmlDocument();
            XmlNode docNode = xml.CreateXmlDeclaration("1.0", "UTF-16", null);
            xml.AppendChild(docNode);

            XmlNode serviceNode = xml.CreateElement(xmlServiceName);
            xml.AppendChild(serviceNode);
            XmlNode parameterNode = xml.CreateElement("Request_Service");
            serviceNode.AppendChild(parameterNode);
            if (includeDateTime)
                AppendPropertyNodesForRequestOrResponse(xml, parameterNode, request1);
            else
                AppendPropertyNodesForRequestWithoutDateTimes(xml, parameterNode, request1);

            return xml;
        }
        private static void AppendPropertyNodesForRequestWithoutDateTimes<T>(XmlDocument xml, XmlNode parameterInNode, T request)
        {
            var excludeProperties = new List<string>();
            AppendPropertyNodesForRequestOrResponse(xml, parameterInNode, request, excludeProperties, true);
        }

        public static XmlDocument GenerateXmlServiceRequest<T, T1>(T request1, T1 request2, string xmlServiceName)
        {
            XmlDocument xml = new XmlDocument();
            XmlNode docNode = xml.CreateXmlDeclaration("1.0", "UTF-16", null);
            xml.AppendChild(docNode);

            XmlNode serviceNode = xml.CreateElement(xmlServiceName);
            xml.AppendChild(serviceNode);
            XmlNode parameterNode1 = xml.CreateElement("Request_Service");
            serviceNode.AppendChild(parameterNode1);

            AppendPropertyNodesForRequestOrResponse(xml, parameterNode1, request1);

            XmlNode parameterNode2 = xml.CreateElement("Request_Service");
            serviceNode.AppendChild(parameterNode2);

            AppendPropertyNodesForRequestOrResponse(xml, parameterNode2, request2);

            return xml;
        }

        public static XmlDocument GenerateXmlServiceResponse<T>(List<T> responses, string xmlServiceName)
        {
            var excludeProperties = new List<string>();
            return GenerateXmlServiceResponse(responses, xmlServiceName, excludeProperties);
        }

        public static XmlDocument GenerateXmlServiceResponse<T>(List<T> responses, string xmlServiceName, List<string> excludeProperties)
        {
            var hasExcludeProperties = excludeProperties.Count > 0;
            XmlDocument xml = new XmlDocument();
            XmlNode docNode = xml.CreateXmlDeclaration("1.0", "UTF-16", null);
            xml.AppendChild(docNode);

            XmlNode serviceNode = xml.CreateElement(xmlServiceName);
            xml.AppendChild(serviceNode);
            XmlNode responseNode = xml.CreateElement("Response_Service");
            serviceNode.AppendChild(responseNode);

            responses.ForEach(response =>
            {
                XmlNode itemNode = xml.CreateElement("Response_Item");
                responseNode.AppendChild(itemNode);

                AppendPropertyNodesForRequestOrResponse(xml, itemNode, response);
            });

            return xml;
        }

        public static XmlDocument GenerateXmlServiceLineResponse(string response, string xmlServiceName)
        {
            XmlDocument xml = new XmlDocument();
            XmlNode docNode = xml.CreateXmlDeclaration("1.0", "UTF-16", null);
            xml.AppendChild(docNode);

            XmlNode serviceNode = xml.CreateElement(xmlServiceName);
            xml.AppendChild(serviceNode);
            XmlNode responseNode = xml.CreateElement("Response_Service");
            serviceNode.AppendChild(responseNode);


            XmlNode itemNode = xml.CreateElement("Response_Item");
            responseNode.AppendChild(itemNode);
            XmlNode parameterNode = xml.CreateElement("Message");
            parameterNode.AppendChild(xml.CreateTextNode(response));
            itemNode.AppendChild(parameterNode);

            return xml;
        }
        
        private static void AppendPropertyNodesForRequestOrResponse(XmlDocument xml, XmlNode parameterInNode, object request)
        {
            var excludeProperties = new List<string>();
            AppendPropertyNodesForRequestOrResponse(xml, parameterInNode, request, excludeProperties, false);
        }

        private static void AppendPropertyNodesForRequestOrResponse(XmlDocument xml, XmlNode parameterInNode, object request, List<string> excludeProperties, bool excludeDates)
        {
            var hasExcludeProperties = excludeProperties.Count > 0;

            foreach (PropertyInfo p in request.GetType().GetProperties())
            {
                if (!hasExcludeProperties || !excludeProperties.Contains(p.Name))
                {
                    XmlNode parameterNode = xml.CreateElement(p.Name);
                    var value = p.GetValue(request, null);
                    if (IsSimpleType(p.PropertyType))
                    {
                        if (!excludeDates || (excludeDates && !IsSimpleDateType(p.PropertyType)))
                        {
                            parameterNode.AppendChild(xml.CreateTextNode(value?.ToString() ?? ""));
                            parameterInNode.AppendChild(parameterNode);
                        }
                        
                    }
                    else if (typeof(IEnumerable).IsAssignableFrom(p.PropertyType))
                    {
                        if (IsSimpleListType(p.PropertyType))
                        {
                            //Console.WriteLine("{0}{1}: {2}", indentString, property.Name, string.Join(",", (string[])propValue));
                            parameterNode.AppendChild(xml.CreateTextNode(ConvertList(p.PropertyType, value)));
                            parameterInNode.AppendChild(parameterNode);
                        }
                        else
                        {
                            XmlNode itemNode = xml.CreateElement(p.Name);
                            parameterInNode.AppendChild(itemNode);

                            IEnumerable enumerable = (IEnumerable)value;
                            foreach (object child in enumerable)
                            {
                                XmlNode itemSubNode = xml.CreateElement("Item");
                                itemNode.AppendChild(itemSubNode);
                                AppendPropertyNodesForRequestOrResponse(xml, itemSubNode, child);
                            }
                        }
                    }
                    else
                    {
                        if (value != null)
                        {
                            XmlNode itemNode = xml.CreateElement(p.Name);
                            parameterInNode.AppendChild(itemNode);
                            AppendPropertyNodesForRequestOrResponse(xml, itemNode, value);
                        }
                        else
                        {
                            parameterNode.AppendChild(xml.CreateTextNode(""));
                            parameterInNode.AppendChild(parameterNode);
                        }
                    }
                }
            }
        }

        public static bool IsSimpleType(Type type)
        {
            return
                type.IsValueType ||
                type.IsPrimitive ||
                new Type[]
                {
                    typeof(double),
                    typeof(double?),
                    typeof(int),
                    typeof(int?),
                    typeof(string),
                    typeof(decimal),
                    typeof(decimal?),
                    typeof(DateTime),
                    typeof(DateTime?),
                    typeof(DateTimeOffset),
                    typeof(DateTimeOffset?),
                    typeof(TimeSpan),
                    typeof(TimeSpan?),
                    typeof(Guid)
                }.Contains(type) ||
                Convert.GetTypeCode(type) != TypeCode.Object;
        }
        public static bool IsSimpleDateType(Type type)
        {
            return
                new Type[]
                {
                    typeof(DateTime),
                    typeof(DateTime?),
                    typeof(DateTimeOffset),
                    typeof(DateTimeOffset?)

                }.Contains(type);
        }

        public static bool IsSimpleListType(Type type)
        {
            return
                new Type[]
                {
                    typeof(List<int>),
                    typeof(List<int?>),
                    typeof(IEnumerable<int>),
                    typeof(IEnumerable<int?>),
                    typeof(List<double>),
                    typeof(List<double?>),
                    typeof(IEnumerable<double>),
                    typeof(IEnumerable<double?>),
                    typeof(List<string>),
                    typeof(IEnumerable<string>),
                    typeof(List<decimal>),
                    typeof(List<decimal?>),
                    typeof(IEnumerable<decimal>),
                    typeof(IEnumerable<decimal?>),
                    typeof(List<DateTime>),
                    typeof(List<DateTime?>),
                    typeof(List<DateTimeOffset>),
                    typeof(List<DateTimeOffset?>),
                    typeof(List<TimeSpan>),
                    typeof(List<TimeSpan?>),
                    typeof(List<Guid>)
                }.Contains(type) ||
                Convert.GetTypeCode(type) != TypeCode.Object;
        }

        public static string ConvertList(Type type,object obj)
        {
            var join = "";

            if (type == typeof(List<string>))
            {
                join = string.Join(",", (List<string>) obj);
            }
            if (type == typeof(IEnumerable<string>))
            {
                join = string.Join(",", (IEnumerable<string>)obj);
            }

            if (type == typeof(List<int>))
            {
                join = string.Join(",", (List<int>)obj);
            }
            if (type == typeof(List<int?>))
            {
                join = string.Join(",", (List<int?>)obj);
            }
            if (type == typeof(IEnumerable<int?>))
            {
                join = string.Join(",", (IEnumerable<int?>)obj);
            }
            if (type == typeof(IEnumerable<int>))
            {
                join = string.Join(",", (IEnumerable<int>)obj);
            }

            if (type == typeof(List<double>))
            {
                join = string.Join(",", (List<double>)obj);
            }
            if (type == typeof(List<double?>))
            {
                join = string.Join(",", (List<double?>)obj);
            }
            if (type == typeof(IEnumerable<double>))
            {
                join = string.Join(",", (IEnumerable<double>)obj);
            }
            if (type == typeof(IEnumerable<double?>))
            {
                join = string.Join(",", (IEnumerable<double?>)obj);
            }

            if (type == typeof(List<decimal>))
            {
                join = string.Join(",", (List<decimal>)obj);
            }
            if (type == typeof(List<decimal?>))
            {
                join = string.Join(",", (List<decimal?>)obj);
            }
            if (type == typeof(IEnumerable<decimal>))
            {
                join = string.Join(",", (IEnumerable<decimal>)obj);
            }
            if (type == typeof(IEnumerable<decimal?>))
            {
                join = string.Join(",", (IEnumerable<decimal?>)obj);
            }
            return join;
            
        }
        
    }
}
