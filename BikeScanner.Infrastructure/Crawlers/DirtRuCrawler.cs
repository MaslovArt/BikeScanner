using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BikeScanner.Application.Interfaces;
using BikeScanner.Application.Models;
using BikeScanner.Domain.Extensions;
using BikeScanner.Infrastructure.Configs.DirtRu;
using BikeScanner.Infrastructure.Models.DirtRu;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BikeScanner.Infrastructure.Crawlers
{
    /// <summary>
    /// Download ads from dirt.ru market
    /// </summary>
    public class DirtRuCrawler : ICrawler
	{
        private readonly ILogger<DirtRuCrawler> _logger;
        private readonly DirtRuSourceConfig     _sourceConfig;

		public DirtRuCrawler(
            ILogger<DirtRuCrawler> logger,
            IOptions<DirtRuSourceConfig> sourceOptions)
		{
            _logger = logger;
            _sourceConfig = sourceOptions.Value;
		}

        public async Task<AdItem[]> Get(DateTime since)
        {
            var pageForums = await GetForumsInfo();
            _logger.LogInformation($"Get {pageForums.Length} forums from page ({string.Join(',', pageForums.Select(f => f.ForumId))})");

            var source = _sourceConfig.Forums
                .Where(s => pageForums.Any(f => f.ForumId == s.ForumId))
                .ToArray();
            if (source.Length != _sourceConfig.Forums.Length)
            {
                var notFoundForums = _sourceConfig.Forums.Except(source);
                _logger.LogWarning($"Forums not found: {string.Join(',', notFoundForums.Select(f => f.ForumId))}");
            }

            source = source
                .Where(s => pageForums.First(f => f.ForumId == s.ForumId).ForumUpdate > since)
                .ToArray();
            _logger.LogInformation($"Check {source.Length} forums updates ({string.Join(',', source.Select(f => f.ForumId))})");

            var getTasks = source.Select(s => GetForumItems(since, s));
            var items = await Task.WhenAll(getTasks);  

            return items
                .SelectMany(i => i)
                .Select(i => new AdItem()
                {
                    Url = i.Url,
                    Text = $"{i.Prefix} {i.Text}",
                    Published = i.Published,
                    SourceType = CrawlerType.DirtRu.ToString(),
                })
                .ToArray();
        }

        private async Task<HtmlNode> DownloadPage(string url)
        {
            var response = await new HttpClient().GetAsync(url);
            var pageContents = await response.Content.ReadAsStringAsync();

            var document = new HtmlDocument();
            document.LoadHtml(pageContents);

            return document.DocumentNode;
        }

        private async Task<ForumItem[]> GetForumItems(DateTime since, DirtRuForumConfig source)
        {
            var items = new List<ForumItem>();

            var currentPage = 1;
            while (currentPage < _sourceConfig.MaximumParsePages)
            {
                var specificForumUrl = $"https://forum.dirt.ru/forumdisplay.php?f={source.ForumId}&order=desc&page={currentPage++}";
                var document = await DownloadPage(specificForumUrl);

                var forumTableId = $"#threadbits_forum_{source.ForumId}";
                var forumItemsRows = document.QuerySelectorAll($"{forumTableId} tr");

                var forumItems = forumItemsRows
                    .Select(TryParseForumItemRow)
                    .Where(i => i != null)
                    .Where(i => !_sourceConfig.ExcludeKeys
                        .Any(k => i.Text.ToUpper().Contains(k.ToUpper())))
                    .ToArray();

                var newItems = forumItems
                    .Where(i => i.Published > since)
                    .ToArray();
                items.AddRange(newItems);

                if (newItems.Length != forumItems.Length) break;
            }

            _logger.LogInformation($"[{source.ForumName}:{source.ForumId}] get {items.Count} items from {currentPage - 1} pages");

            return items.ToArray();
        }

        private async Task<ForumInfo[]> GetForumsInfo()
        {
            var marketUrl = $"https://forum.dirt.ru/forumdisplay.php?f=7";
            var document = await DownloadPage(marketUrl);

            var forumTables = new List<HtmlNode>();
            forumTables.AddRange(document.QuerySelectorAll("#collapseobj_forumbit_22"));
            forumTables.AddRange(document.QuerySelectorAll("#collapseobj_forumbit_23"));
            forumTables.AddRange(document.QuerySelectorAll("#collapseobj_forumbit_23 ~ tbody"));

            if (forumTables.Count == 0)
                _logger.LogWarning("Parse market page: No forums found!");

            return forumTables
                .SelectMany(table => table.GetChildElements())
                .Select(TryParseForumInfoRow)
                .Where(f => f != null)
                .ToArray();
        }

        private ForumItem TryParseForumItemRow(HtmlNode node)
        {
            try
            {
                var columns = node.GetChildElements().ToArray();
                var itemId = columns[2]
                    .Attributes["id"].Value
                    .Replace("td_title_", "");
                var prefix = columns[1].InnerText;
                var desctiption = columns[2]
                    .QuerySelector($"#thread_title_{itemId}")
                    .InnerText;
                var published = columns[3].InnerText
                    .ReplaceAll(new string[] { "\r", "\n", "\t" }, "")
                    .Split("от")[0];

                return new ForumItem()
                {
                    Url = $"https://forum.dirt.ru/showthread.php?t={itemId}",
                    Text = desctiption,
                    Prefix = prefix,
                    Published = DateTime.ParseExact(published, "dd.MM.yyyy HH:mm", null)
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Can't parse forum item row [html: {node.InnerHtml}]");
                return null;
            }
        }

        private ForumInfo TryParseForumInfoRow(HtmlNode node)
        {
            try
            {
                var forumIdStr = node
                    .QuerySelector(".alt1Active a")
                    .Attributes["href"]
                    .Value.Split('=').Last();
                var updateStr = node
                    .QuerySelector(".alt2 .smallfont div:nth-child(3)")
                    .InnerText
                    .ReplaceAll(new string[] { "\r", "\n", "\t" }, "");

                return new ForumInfo()
                {
                    ForumId = int.Parse(forumIdStr),
                    ForumUpdate = DateTime.ParseExact(updateStr, "dd.MM.yyyy HH:mm", null)
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Can't parse forum info row [html: {node.InnerHtml}]");
                return null;
            }
        }
    }
}

