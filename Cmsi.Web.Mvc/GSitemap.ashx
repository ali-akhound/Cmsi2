<%@ WebHandler Language="C#" Class="GSitemap" %>
using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;

public class GSitemap : IHttpHandler
{
    private const int MaxDepth = 6;

    public void ProcessRequest(HttpContext context)
    {
        HttpResponse response = context.Response;
        response.ContentType = "text/xml";

        using (XmlTextWriter writer = new XmlTextWriter(response.OutputStream, Encoding.UTF8))
        {
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteStartElement("urlset");
            writer.WriteAttributeString("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
            SiteMapNode root = SiteMap.RootNode;
            WriteNode(root, writer, 0);
            writer.WriteEndElement(); // urlset
            writer.WriteEndDocument();
        }
    }

    public bool IsReusable
    {
        get
        {
            return true;
        }
    }

    private void WriteNode(SiteMapNode node, XmlTextWriter writer, int depth)
    {
        if (depth > MaxDepth) return;

        writer.WriteStartElement("url");

        NameValueCollection vars = HttpContext.Current.Request.ServerVariables;
        string port = vars["SERVER_PORT"];
        port = (port == "80") ? String.Empty : ':' + port;
        string protocol = (vars["SERVER_PORT_SECURE"] == "1") ? "https://" : "http://";
        if (!node.Url.StartsWith("http://"))
        {
            writer.WriteElementString("loc", protocol + vars["SERVER_NAME"] + port + node.Url);
        }
        else
        {
            writer.WriteElementString("loc", node.Url);
        }
        string lastMod = node["lastModified"];
        if (String.IsNullOrEmpty(lastMod))
        {
            try
            {
                string physicalPath = HttpContext.Current.Request.MapPath(node.Url);
                if (File.Exists(physicalPath))
                {
                    DateTime lastModified = File.GetLastWriteTimeUtc(physicalPath);
                    lastMod = lastModified.ToString("yyyy-MM-ddThh:mm:ss.fffZ");
                }
            }
            catch { }
        }
        if (!String.IsNullOrEmpty(lastMod))
        {
            writer.WriteElementString("lastmod", lastMod);
        }

        string changeFreq = node["changeFrequency"];
        if (!String.IsNullOrEmpty(changeFreq))
        {
            writer.WriteElementString("changefreq", changeFreq);
        }

        string priority = node["priority"];
        if (!String.IsNullOrEmpty(priority))
        {
            writer.WriteElementString("priority", priority);
        }

        //string title = node["title"];
        //writer.WriteElementString("title", node.Title);

        //string description = node["description"];
        //writer.WriteElementString("description", node.Description);

        writer.WriteElementString("changefreq", "monthly");
        writer.WriteElementString("priority", "0.5");

        writer.WriteEndElement(); // url

        int subNodeDepth = depth + 1;
        foreach (SiteMapNode subNode in node.ChildNodes)
        {
            WriteNode(subNode, writer, subNodeDepth);
        }

    }
}