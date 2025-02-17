using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public class WebScraperService
{
	private readonly HttpClient _httpClient;

	public WebScraperService(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<HashSet<string>> ExploreSiteAsync(string startUrl, HashSet<string> visitedUrls = null)
	{
		if (visitedUrls == null) visitedUrls = new HashSet<string>();

		if (visitedUrls.Contains(startUrl))
		{
			return visitedUrls;
		}

		visitedUrls.Add(startUrl);

		var links = await ScrapeSiteAsync(startUrl);

		foreach (var link in links)
		{
			if (IsInternalLink(link, startUrl))
			{
				await ExploreSiteAsync(link, visitedUrls);
			}
		}

		return visitedUrls;
	}

	private async Task<List<string>> ScrapeSiteAsync(string url)
	{
		var links = new List<string>();

		try
		{
			var response = await _httpClient.GetStringAsync(url);
			var htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(response);

			var linkNodes = htmlDocument.DocumentNode.SelectNodes("//a[@href]");
			if (linkNodes != null)
			{
				foreach (var node in linkNodes)
				{
					var href = node.GetAttributeValue("href", string.Empty);
					if (Uri.IsWellFormedUriString(href, UriKind.Absolute))
					{
						links.Add(href);
					}
					else if (Uri.IsWellFormedUriString(url + href, UriKind.Absolute))
					{
						links.Add(url + href);
					}
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Erreur lors du scraping de {url}: {ex.Message}");
		}

		return links;
	}

	private bool IsInternalLink(string link, string baseUrl)
	{
		Uri baseUri = new Uri(baseUrl);
		Uri targetUri;

		if (Uri.TryCreate(link, UriKind.Absolute, out targetUri))
		{
			return baseUri.Host.Equals(targetUri.Host, StringComparison.OrdinalIgnoreCase);
		}

		return true;
	}
}
