using System;

namespace Commerce.Amazon.Domain.Config
{
	public class Configuration
	{
		
		public static string ApiUri { get { return "ApiUrl"; } }
		public static string LogDirectory { get { return @"C:\Logs"; } }
		public static string LogFile { get { return $"LogFile_{DateTime.Now.ToString("yyyyMMdd")}.log"; } }
		public static string LogWatch { get { return "C:\\Logs\\WatcherWeb.txt"; } }
        
	}
}
