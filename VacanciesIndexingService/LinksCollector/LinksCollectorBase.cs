using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace VacanciesIndexing.LinksCollector
{
    abstract class LinksCollectorBase
    {
        protected abstract IEnumerable<HtmlNode> LoadAllVacancyNodes();
        protected abstract bool IsVacancyNodeNew(HtmlNode vacancyNode, DateTime lastUpdateDatetime);
        protected abstract string RetrieveUrlFromVacancyNode(HtmlNode vacancyNode);

        public IEnumerable<string> CollectInterestingUrls(DateTime lastUpdateDatetime)
        {
            IndexingService.Log.Debug("Collecting links started");

            var allVacancyNodes = LoadAllVacancyNodes();

            var interestingUrls = allVacancyNodes
                                  .Where(vacancy => IsVacancyNodeNew(vacancy, lastUpdateDatetime))
                                  .Select(RetrieveUrlFromVacancyNode);

            return interestingUrls;
        }
    }
}