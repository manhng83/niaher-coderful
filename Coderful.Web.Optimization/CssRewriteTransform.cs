namespace Coderful.Web.Optimization
{
	using System;
	using System.Text.RegularExpressions;
	using System.Web;
	using System.Web.Optimization;

	/// <summary>
	/// Wraps standard CssRewriteUrlTransform to properly handle scenarios where
	/// application's virtual path is not under the hostname, but rather in the subdirectory,
	/// e.g. - www.example.com/apps/myapp. This implementation also contains the fix for properly  
	/// handling embedded data URIs (https://aspnetoptimization.codeplex.com/workitem/88).
	/// </summary>
	public class CssRewriteUrlTransform : IItemTransform
	{
		internal static string RebaseUrlToAbsolute(string baseUrl, string url)
		{
			if (string.IsNullOrWhiteSpace(url) || 
				string.IsNullOrWhiteSpace(baseUrl) ||
				url.StartsWith("/", StringComparison.OrdinalIgnoreCase) || 
				url.StartsWith("data:") || 
				url.StartsWith("http://") || 
				url.StartsWith("https://"))
			{
				return url;
			}

			if (!baseUrl.EndsWith("/", StringComparison.OrdinalIgnoreCase))
			{
				baseUrl = baseUrl + "/";
			}

			return VirtualPathUtility.ToAbsolute(baseUrl + url);
		}

		internal static string ConvertUrlsToAbsolute(string baseUrl, string content)
		{
			if (string.IsNullOrWhiteSpace(content))
			{
				return content;
			}
			
			return new Regex("url\\(['\"]?(?<url>[^)]+?)['\"]?\\)").Replace(content, match => "url(" + RebaseUrlToAbsolute(baseUrl, match.Groups["url"].Value) + ")");
		}

		public string Process(string includedVirtualPath, string input)
		{
			string absoluteVirtualPath = "/" + VirtualPathUtility.ToAbsolute(includedVirtualPath);
			
			return ConvertUrlsToAbsolute(VirtualPathUtility.GetDirectory(absoluteVirtualPath.Substring(1)), input);
		}
	}
}