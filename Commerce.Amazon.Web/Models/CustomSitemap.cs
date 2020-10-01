using System.Collections.Generic;

namespace Commerce.Amazon.Web.Models
{
	public class CustomSitemap
	{
		public Sitemap SiteMap { get; set; }
	}

	public class Sitemap
	{
		public List<Sitemapnode> SiteMapNodes { get; set; }
	}

	public class Sitemapnode
	{
		public string title { get; set; }
		public string controller { get; set; }
		public string action { get; set; }
		public bool clickable { get; set; }
		public bool haschildren { get; set; }
		public string querykey { get; set; }
		public List<Sitemapnode> SiteMapNodes { get; set; }
	}

}