using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Amigo.Tenant.IdentityServer.Infrastructure.Extensions;

namespace Amigo.Tenant.IdentityServer.Infrastructure.ViewService
{
    internal class AssetManager
    {
        public const string HttpAssetsNamespace = "Amigo.Tenant.IdentityServer.Infrastructure.ViewService.HttpAssets";
        public const string FontAssetsNamespace = HttpAssetsNamespace + ".libs.bootstrap.fonts";

        public const string PageAssetsNamespace = "Amigo.Tenant.IdentityServer.Infrastructure.ViewService.PageAssets";
        const string PagesPrefix = PageAssetsNamespace + ".";
        const string Layout = PagesPrefix + "layout.html";
        const string FormPostResponse = PagesPrefix + "FormPostResponse.html";
        const string CheckSession = PagesPrefix + "checksession.html";
        const string SignoutFrame = PagesPrefix + "SignoutFrame.html";
        const string Welcome = PagesPrefix + "welcome.html";

        static readonly ResourceCache cache = new ResourceCache();

        const string PageNameTemplate = PagesPrefix + "{0}" + ".html";
        public static string LoadPage(string pageName)
        {
            pageName = String.Format(PageNameTemplate, pageName);
            return LoadResourceString(pageName);
        }

        public static string ApplyContentToLayout(string layout, string content)
        {
            return Format(layout, new { pageContent = content });
        }
        
        public static string LoadLayoutWithContent(string content)
        {
            if (content == null) return null;

            var layout = LoadResourceString(Layout);
            return ApplyContentToLayout(layout, content);
        }

        public static string LoadLayoutWithPage(string pageName)
        {
            var pageContent = LoadPage(pageName);
            return LoadLayoutWithContent(pageContent);
        }

        public static string LoadFormPost(string rootUrl, string redirectUri, string fields)
        {
            return LoadResourceString(FormPostResponse,
                new
                {
                    rootUrl,
                    redirect_uri = redirectUri,
                    fields
                }
            );
        }

        public static string LoadCheckSession(string rootUrl, string cookieName)
        {
            return LoadResourceString(CheckSession, new
            {
                rootUrl,
                cookieName
            });
        }

        public static string LoadSignoutFrame(IEnumerable<string> frameUrls)
        {
            string frames = null;
            if (frameUrls != null && frameUrls.Any())
            {
                frameUrls = frameUrls.Select(x => String.Format("<iframe src='{0}'></iframe>", x));
                frames = frameUrls.Aggregate((x, y) => x + Environment.NewLine + y);
            }

            return LoadResourceString(SignoutFrame, new
            {
                frames
            });
        }

        internal static string LoadWelcomePage(string applicationPath, string version)
        {
            applicationPath = applicationPath.RemoveTrailingSlash();
            return LoadResourceString(Welcome, new
            {
                applicationPath,
                version
            });
        }
        
        static string LoadResourceString(string name)
        {
            string value = cache.Read(name);
            if (value == null)
            {
                var assembly = typeof(AssetManager).Assembly;
                var s = assembly.GetManifestResourceStream(name);
                if (s != null)
                {
                    using (var sr = new StreamReader(s))
                    {
                        value = sr.ReadToEnd();
                        cache.Write(name, value);
                    }
                }
            }
            return value;
        }

        static string LoadResourceString(string name, object data)
        {
            string value = LoadResourceString(name);
            if (value == null) return null;

            value = Format(value, data);
            return value;
        }

        static string Format(string value, IDictionary<string, object> data)
        {
            if (value == null) return null;

            foreach (var key in data.Keys)
            {
                var val = data[key];
                val = val ?? String.Empty;
                value = value.Replace("{" + key + "}", val.ToString());
            }
            return value;
        }

        public static string Format(string value, object data)
        {
            return Format(value, Map(data));
        }
        
        static IDictionary<string, object> Map(object values)
        {
            var dictionary = values as IDictionary<string, object>;
            
            if (dictionary == null) 
            {
                dictionary = new Dictionary<string, object>();
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(values))
                {
                    dictionary.Add(descriptor.Name, descriptor.GetValue(values));
                }
            }

            return dictionary;
        }
    }
}

