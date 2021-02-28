using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IcyPick.Fetcher
{
    public class IcyVeinsRepository : IHeroesRepository
    {
        private readonly IHttpClientFactory clientFactory;
        private readonly IOptionsMonitor<IcyVeinsRepositoryOptions> options;

        public IcyVeinsRepository(IHttpClientFactory clientFactory, IOptionsMonitor<IcyVeinsRepositoryOptions> options)
        {
            this.clientFactory = clientFactory;
            this.options = options;
        }

        private HttpClient CreateClient()
        {
            var client = clientFactory.CreateClient();
            client.BaseAddress = new Uri(options.CurrentValue.BaseUrl!); // Option validation ensures not null

            return client;
        }

        public async Task<IReadOnlyList<Heroe>> GetHeroesAsync(CancellationToken cancellationToken)
        {
            using var client = CreateClient();
            var document = await GetHtmlDocumentAsync(client, "", cancellationToken);
            return document.DocumentNode.SelectNodes("//*[@class='nav_content_block_entry_heroes_hero']")
                .Select(node => ParseHeroNode(node, client.BaseAddress!))
                .ToList();
        }

        private static Heroe ParseHeroNode(HtmlNode node, Uri baseUri)
        {
            var guideUrl = node.SelectSingleNode("a").GetAttributeValue("href", null)
                ?? throw new InvalidOperationException($"Parsing failed. Expected ${node.OuterHtml} to contain an a href");
            var iconUrl = node.SelectSingleNode("a/img").GetAttributeValue("src", null)
                ?? throw new InvalidOperationException($"Parsing failed. Expected ${node.OuterHtml} to contain an a/img src");
            var category = node.SelectSingleNode("ancestor::*[@class='nav_entry nav_entry_with_content']/*[@class='header']").InnerText.ToLower();

            return new Heroe(
                node.InnerText.Trim(),
                category,
                new Uri(baseUri, guideUrl),
                new Uri(baseUri, iconUrl));
        }

        private static async Task<HtmlDocument> GetHtmlDocumentAsync(HttpClient client, string relativeUrl, CancellationToken cancellationToken)
        {
            using var response = await client.GetAsync(relativeUrl, cancellationToken);

            var document = new HtmlDocument();
            document.Load(await response.Content.ReadAsStreamAsync(cancellationToken));
            return document;
        }
    }
}
