using Commerce.Amazon.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Commerce.Amazon.Web.Modules
{
	public class CustomSiteMapModule
	{
		public CustomSitemap CustomSitemap { get; set; }
		List<Sitemapnode> SiteMapNodes;
		private readonly IHostingEnvironment _hostingEnvironment;

		public CustomSiteMapModule(IHostingEnvironment hostingEnvironment)
		{
			SiteMapNodes = new List<Sitemapnode>();
			_hostingEnvironment = hostingEnvironment;
			CustomSitemap = JsonConvert.DeserializeObject<CustomSitemap>(File.ReadAllText(Path.Combine(_hostingEnvironment.ContentRootPath, "sitemap.json"), Encoding.GetEncoding(1252)));
		}
		public List<Sitemapnode> GetNodesBy(string controller, string action, string[] querykeys = null)
		{
			SiteMapNodes = new List<Sitemapnode>();
			Sitemapnode firstNode = CustomSitemap.SiteMap.SiteMapNodes.FirstOrDefault();
			if (querykeys != null)
			{
				HasControllerAndAction(firstNode, controller, action, querykeys);
			}
			else
			{
				HasControllerAndAction(firstNode, controller, action);
			}
			SiteMapNodes.Add(firstNode);
			SiteMapNodes.Reverse();
			return SiteMapNodes;
		}

		bool HasControllerAndAction(Sitemapnode node, string controller, string action, string[] querykeys)
		{
			if (controller == node.controller && action == node.action && querykeys.Contains(node.querykey, StringComparer.OrdinalIgnoreCase))
			{
				SiteMapNodes.Add(node);
				return true;
			}
			else if (node.haschildren)
			{
				foreach (var item in node.SiteMapNodes)
				{
					if (HasControllerAndAction(item, controller, action, querykeys))
					{
						if (!SiteMapNodes.Exists(w=>w.title == item.title))
						{
							SiteMapNodes.Add(item);
						}
						return true;
					}
				}
				return false;
			}
			return false;
		}
		bool HasControllerAndAction(Sitemapnode node, string controller, string action)
		{
			if (controller == node.controller && action == node.action)
			{
				SiteMapNodes.Add(node);
				return true;
			}
			else if (node.haschildren)
			{
				foreach (var item in node.SiteMapNodes)
				{
					if (HasControllerAndAction(item, controller, action))
					{
						if (!SiteMapNodes.Exists(w=>w.title == item.title))
						{
							SiteMapNodes.Add(item);
						}
						return true;
					}
				}
				return false;
			}
			return false;
		}
	}
}