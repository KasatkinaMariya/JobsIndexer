using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace VacanciesIndexing.LinksCollector
{
    class YandexRssCollector : LinksCollectorBase
    {
        private readonly string _rssFeedUrlPattern;
        private readonly IEnumerable<DateTimeFormatInfo> _possibleDatetimeFormatInfs;

        protected override IEnumerable<HtmlNode> LoadAllVacancyNodes()
        {
            IEnumerable<HtmlNode> allVacancyNodes = new List<HtmlNode>();

            var webClient = new WebClient() { Encoding = Encoding.UTF8 };

            for (int curPageNumber = 1; ; curPageNumber++)
            {
                string rssFeedUrl = string.Format(_rssFeedUrlPattern, curPageNumber);
                string rssFeedHtml = webClient.DownloadString(rssFeedUrl);

                var rssFeedDoc = new HtmlDocument();
                rssFeedDoc.LoadHtml(rssFeedHtml);

                var curVacanciesPortion = rssFeedDoc.DocumentNode.SelectNodes("rss/channel/item");
                if (curVacanciesPortion == null)
                    break;
                allVacancyNodes = curVacanciesPortion.Union(allVacancyNodes);
            }

            return allVacancyNodes;
        }

        protected override bool IsVacancyNodeNew(HtmlNode vacancyNode, DateTime lastUpdateDatetime)
        {
            string datetimeStr = vacancyNode.SelectSingleNode("pubdate").InnerHtml.Split(',')[1];

            foreach (var formatInfo in _possibleDatetimeFormatInfs)
            {
                try
                {
                    var vacancyDatetime = DateTime.ParseExact(datetimeStr,
                                                              formatInfo.FullDateTimePattern,
                                                              formatInfo);
                    return vacancyDatetime > lastUpdateDatetime;
                }
                catch (FormatException)
                {
                    IndexingService.Log.Debug(string.Format("Failed to parse '{0}' with '{1}'",
                                                             datetimeStr, formatInfo.FullDateTimePattern));
                }
            }

            IndexingService.Log.Debug(string.Format("Failed to parse '{0}'", datetimeStr));

            return true;
        }

        protected override string RetrieveUrlFromVacancyNode(HtmlNode vacancyNode)
        {
            return vacancyNode.SelectSingleNode("guid").InnerHtml;
        }

        public YandexRssCollector()
        {
            _rssFeedUrlPattern = IndexingService.ActualConfiguration.RssFeedUrlPattern;
            _possibleDatetimeFormatInfs = IndexingService.ActualConfiguration.PossibleDatetimeFormatInfs;
        }
    }
}
