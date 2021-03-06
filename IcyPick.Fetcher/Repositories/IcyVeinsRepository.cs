using HtmlAgilityPack;
using IcyPick.Fetcher.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace IcyPick.Fetcher.Repositories
{
    public class IcyVeinsRepository : IHeroesRepository
    {
        readonly IHttpClientFactory clientFactory;
        readonly IOptionsMonitor<IcyVeinsRepositoryOptions> options;

        public IcyVeinsRepository(IHttpClientFactory clientFactory, IOptionsMonitor<IcyVeinsRepositoryOptions> options)
        {
            this.clientFactory = clientFactory;
            this.options = options;
        }

        async Task<HtmlDocument> GetHtmlDocumentAsync(Uri uri, CancellationToken cancellationToken)
        {
            using var client = clientFactory.CreateClient();
            using var response = await client.GetAsync(uri, cancellationToken);
            response.EnsureSuccessStatusCode();

            var document = new HtmlDocument();
            document.Load(await response.Content.ReadAsStreamAsync(cancellationToken));

            return document;
        }

        static readonly Regex idFinder = new Regex("/([a-zA-Z_-]*)-build-guide$");
        public async Task<IReadOnlyList<Hero>> GetHeroesAsync(CancellationToken cancellationToken)
        {
            var uri = new Uri(options.CurrentValue.BaseUrl!);

            var document = await GetHtmlDocumentAsync(uri, cancellationToken);

            return document.DocumentNode.SelectNodes("//*[@class='nav_content_block_entry_heroes_hero']")
                .Select(node => ParseHeroNode(node, uri))
                .ToList();

            static Hero ParseHeroNode(HtmlNode node, Uri baseUri)
            {
                var guideUrl = node.SelectSingleNode("a").GetAttributeValue("href", null)
                    ?? throw new InvalidOperationException($"Parsing failed. Expected ${node.OuterHtml} to contain an a href");
                var iconUrl = node.SelectSingleNode("a/img").GetAttributeValue("src", null)
                    ?? throw new InvalidOperationException($"Parsing failed. Expected ${node.OuterHtml} to contain an a/img src");
                var role = node.SelectSingleNode("ancestor::*[@class='nav_entry nav_entry_with_content']/*[@class='header']").InnerText.ToLower();

                return new Hero(
                    idFinder.Match(guideUrl).Groups[1].Value,
                    node.InnerText.Trim(),
                    NormalizeRole(role),
                    new Uri(baseUri, guideUrl),
                    new Uri(baseUri, iconUrl));
            }

            static string NormalizeRole(string role)
                => role switch
                {
                    "bruisers" => "bruiser",
                    "healers" => "healer",
                    "melee assassins" => "melee assassin",
                    "ranged assassins" => "ranged assassin",
                    "supports" => "support",
                    "tanks" => "tank",
                    _ => throw new InvalidOperationException($"Role ${role} is unknown")
                };
        }

        public async Task<HeroGuide> GetHeroGuideAsync(Hero hero, CancellationToken cancellationToken)
        {
            var document = await GetHtmlDocumentAsync(hero.GuideUri, cancellationToken);

            return new HeroGuide(
                hero,
                ParseMapPreference(document, hero.GuideUri),
                ParseSynergiesAndCounter(document));

            static HeroMapPreference ParseMapPreference(HtmlDocument document, Uri baseUri)
            {
                var strongerNode = document.DocumentNode.SelectSingleNode("//*[@class='heroes_maps_stronger']");
                var averageNode = document.DocumentNode.SelectSingleNode("//*[@class='heroes_maps_average']");
                var weakerNode = document.DocumentNode.SelectSingleNode("//*[@class='heroes_maps_weaker']");
                var strategyNode = document.DocumentNode.SelectSingleNode("//*[@class='heroes_maps_text']");

                return new HeroMapPreference(
                    FindTooltipNodes(strongerNode).Select(ParseMap).WhereNotNull().ToList(),
                    FindTooltipNodes(averageNode).Select(ParseMap).WhereNotNull().ToList(),
                    FindTooltipNodes(weakerNode).Select(ParseMap).WhereNotNull().ToList(),
                    strategyNode.InnerText.Trim());

                Map? ParseMap(HtmlNode node)
                {
                    var id = ExtractTooltipId(node, "map");
                    if (id == null)
                        return null;
                
                    var imageNode = node.SelectSingleNode(".//img")
                        ?? throw new InvalidOperationException($"Parsing failed. Expected ${node.OuterHtml} to contain an img");

                    var imagePath = imageNode.GetAttributeValue("src", null)
                        ?? throw new InvalidOperationException($"Parsing failed. Expected ${node.OuterHtml} to contain an img src");

                    var imageTitle = imageNode.GetAttributeValue("title", null)
                        ?? throw new InvalidOperationException($"Parsing failed. Expected ${node.OuterHtml} to contain an img title");

                    return new Map(id, imageTitle, new Uri(baseUri, imagePath));
                }
            }

            static HeroSynergiesAndCounter ParseSynergiesAndCounter(HtmlDocument document)
            {
                var synergyNode = document.DocumentNode.SelectSingleNode("//*[@class='heroes_synergies']//*[@class='heroes_synergies_counters_content']");
                var counterNode = document.DocumentNode.SelectSingleNode("//*[@class='heroes_counters']//*[@class='heroes_synergies_counters_content']");

                return new HeroSynergiesAndCounter(
                    FindTooltipIds(synergyNode, "hero"),
                    synergyNode.InnerText.Trim(),

                    FindTooltipIds(counterNode, "hero"),
                    counterNode.InnerText.Trim());

                static IReadOnlyList<string> FindTooltipIds(HtmlNode node, string prefix)
                    => FindTooltipNodes(node)
                        .Select(node => ExtractTooltipId(node, prefix))
                        .WhereNotNull()
                        .ToList();
            }

            static IEnumerable<HtmlNode> FindTooltipNodes(HtmlNode node)
                => node.SelectNodes(".//*[@data-heroes-tooltip]") ?? Enumerable.Empty<HtmlNode>();

            static string? ExtractTooltipId(HtmlNode node, string prefix)
            {
                var id = node.GetAttributeValue("data-heroes-tooltip", null);
                return id != null && id.StartsWith(prefix + "-", StringComparison.OrdinalIgnoreCase)
                    ? id[(prefix.Length + 1)..]
                    : null;
            }
        }

        public async Task DownloadImageAsync(IEntityWithImage entity, Func<Task<Stream>> outputStreamAsyncFactory, CancellationToken cancellationToken)
        {
            using var client = clientFactory.CreateClient();
            using var response = await client.GetAsync(entity.IconUri, cancellationToken);

            if (response.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException($"Could not download image {entity.IconUri} for {entity.Id} ({entity.GetType().Name}");

            using var inputStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var outputStream = await outputStreamAsyncFactory();

            await inputStream.CopyToAsync(outputStream, cancellationToken);
        }

        public async Task<IReadOnlyList<Tier>> GetHeroTiersAsync(string list, CancellationToken cancellationToken)
        {
            var uri = new Uri(new Uri(options.CurrentValue.BaseUrl!), $"heroes-of-the-storm-{list}-tier-list");

            var document = await GetHtmlDocumentAsync(uri, cancellationToken);

            return document.DocumentNode.SelectNodes("//*[starts-with(@id, 'ranking-')]")
                .Select(ParseHeroTierNode)
                .GroupBy(pair => pair.tierName, pair => pair.hero)
                .Select(group => new Tier(group.Key, group.ToList()))
                .ToList();

            static (string tierName, Tier.Hero hero) ParseHeroTierNode(HtmlNode node)
            {
                var prefixedId = node.GetAttributeValue("id", null)
                    ?? throw new InvalidOperationException($"Parsing failed. Expected ${node.OuterHtml} to contain an an id");
                var originalId = prefixedId["ranking-".Length..];
                var isBanRecommended = node.SelectNodes(".//*[contains(@class, 'htl_ban_true')]")?.Any() == true;

                var (id, condition) = ExtractHeroIdAndCondition(originalId);
                var hero = new Tier.Hero(id, isBanRecommended, condition);

                var tierNode = node.SelectSingleNode("(ancestor::*[@class='htl']/preceding-sibling::*[@class='heading_container heading_number_2'])[last()]")
                    ?? throw new InvalidOperationException($"Parsing failed. Expected ${node.OuterHtml} parent htl's to be preceded by a heading_container");

                var tierName = tierNode.SelectSingleNode(".//h2").GetAttributeValue("id", null)
                    ?? throw new InvalidOperationException($"Parsing failed. Expected ${tierNode.OuterHtml} to contain an an id");

                return (tierName, hero);
            }

            static (string id, string? condition) ExtractHeroIdAndCondition(string heroId)
            {
                return heroId switch
                {
                    "varian-taunt" => ("varian", "taunt"),
                    "varian-twin" => ("varian", "twin"),
                    "varian-colossus" => ("varian", "colossus"),
                    _ => (heroId, null)
                };
            }
        }
    }
}
