﻿using HtmlAgilityPack;
using IcyPick.Fetcher.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace IcyPick.Fetcher.Repositories
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

        private async Task<HtmlDocument> GetHtmlDocumentAsync(Uri uri, CancellationToken cancellationToken)
        {
            using var client = clientFactory.CreateClient();
            using var response = await client.GetAsync(uri, cancellationToken);

            var document = new HtmlDocument();
            document.Load(await response.Content.ReadAsStreamAsync(cancellationToken));

            return document;
        }

        public async Task<IReadOnlyList<Hero>> GetHeroesAsync(CancellationToken cancellationToken)
        {
            var uri = new Uri(options.CurrentValue.BaseUrl!);

            var document = await GetHtmlDocumentAsync(uri, cancellationToken);

            return document.DocumentNode.SelectNodes("//*[@class='nav_content_block_entry_heroes_hero']")
                .Select(node => ParseHeroNode(node, uri))
                .ToList();
        }

        private static readonly Regex idFinder = new Regex("/([a-zA-Z_-]*)-build-guide$");
        private static Hero ParseHeroNode(HtmlNode node, Uri baseUri)
        {
            var guideUrl = node.SelectSingleNode("a").GetAttributeValue("href", null)
                ?? throw new InvalidOperationException($"Parsing failed. Expected ${node.OuterHtml} to contain an a href");
            var iconUrl = node.SelectSingleNode("a/img").GetAttributeValue("src", null)
                ?? throw new InvalidOperationException($"Parsing failed. Expected ${node.OuterHtml} to contain an a/img src");
            var category = node.SelectSingleNode("ancestor::*[@class='nav_entry nav_entry_with_content']/*[@class='header']").InnerText.ToLower();

            return new Hero(
                idFinder.Match(guideUrl).Groups[1].Value,
                node.InnerText.Trim(),
                category,
                new Uri(baseUri, guideUrl),
                new Uri(baseUri, iconUrl));
        }

        public async Task<HeroGuide> GetHeroGuideAsync(Hero hero, CancellationToken cancellationToken)
        {
            var document = await GetHtmlDocumentAsync(hero.GuideUri, cancellationToken);

            return new HeroGuide(
                hero,
                ParseMapPreference(document),
                ParseSynergiesAndCounter(document));

        }

        private static HeroMapPreference ParseMapPreference(HtmlDocument document)
        {
            var strongerNode = document.DocumentNode.SelectSingleNode("//*[@class='heroes_maps_stronger']");
            var averageNode = document.DocumentNode.SelectSingleNode("//*[@class='heroes_maps_average']");
            var weakerNode = document.DocumentNode.SelectSingleNode("//*[@class='heroes_maps_weaker']");
            var strategyNode = document.DocumentNode.SelectSingleNode("//*[@class='heroes_maps_text']");

            return new HeroMapPreference(
                FindTooltips(strongerNode, "map"),
                FindTooltips(averageNode, "map"),
                FindTooltips(weakerNode, "map"),
                strategyNode.InnerText);
        }

        private static HeroSynergiesAndCounter ParseSynergiesAndCounter(HtmlDocument document)
        {
            var synergyNode = document.DocumentNode.SelectSingleNode("//*[@class='heroes_synergies']//*[@class='heroes_synergies_counters_content']");
            var counterNode = document.DocumentNode.SelectSingleNode("//*[@class='heroes_counters']//*[@class='heroes_synergies_counters_content']");

            return new HeroSynergiesAndCounter(
                FindTooltips(synergyNode, "hero"),
                synergyNode.InnerText.Trim(),

                FindTooltips(counterNode, "hero"),
                counterNode.InnerText.Trim());
        }
            
        private static IReadOnlyList<string> FindTooltips(HtmlNode node, string prefix)
            => (node.SelectNodes(".//*[@data-heroes-tooltip]") ?? Enumerable.Empty<HtmlNode>())
                .Select(node => node.GetAttributeValue("data-heroes-tooltip", null))
                .Where(value => value != null && value.StartsWith(prefix + "-", StringComparison.OrdinalIgnoreCase))
                .Select(value => value[(prefix.Length + 1)..])
                .ToList();
    }
}