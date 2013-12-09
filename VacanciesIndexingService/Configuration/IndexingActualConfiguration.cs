using System.Collections.Generic;
using System.Configuration;
using System.Globalization;

namespace VacanciesIndexing.Configuration
{
    public class IndexingActualConfiguration
    {
        public double HoursNumberBetweenIndexingRuns
        {
            get
            {
                if (_hoursNumberBetweenIndexingRuns == null)
                    _hoursNumberBetweenIndexingRuns = double.Parse((_indexingGroup.Sections["general"] as GeneralSection)
                                                                    .HoursNumberBetweenIndexingRuns.Value);
                return _hoursNumberBetweenIndexingRuns.Value;
            }
        }
        private double? _hoursNumberBetweenIndexingRuns;

        #region Database
        public string PathToCreatingObjectsScript
        {
            get
            {
                if (_pathToCreatingObjectsScript == null)
                    _pathToCreatingObjectsScript = (_indexingGroup.Sections["database"] as DatabaseSection)
                                                   .PathToCreatingObjectsScript.Value;
                return _pathToCreatingObjectsScript;
            }
        }
        private string _pathToCreatingObjectsScript;

        public string ConnectionString
        {
            get
            {
                return _connectionString ??
                       (_connectionString = (_indexingGroup.Sections["database"] as DatabaseSection)
                                            .ConnectionString.Value);
            }
        }
        private string _connectionString;
        #endregion Database

        #region YandexRss
        public string RssFeedUrlPattern
        {
            get
            {
                if (_rssFeedUrl == null)
                    _rssFeedUrl = (_indexingGroup.Sections["yandexRss"] as YandexRssSection)
                                  .UrlPattern.Value;
                return _rssFeedUrl;
            }
        }
        private string _rssFeedUrl;

        public IEnumerable<DateTimeFormatInfo> PossibleDatetimeFormatInfs
        {
            get
            {
                if (_possibleDatetimeFormatInfs == null)
                {
                    var formatsFromConfig = (_indexingGroup.Sections["yandexRss"] as YandexRssSection)
                                            .PossibleDatetimeFormats;

                    _possibleDatetimeFormatInfs = new List<DateTimeFormatInfo>();
                    foreach (DatetimeFormatElement e in formatsFromConfig)
                        _possibleDatetimeFormatInfs.Add(e.ConvertToSystemFormatInfo());
                }
                return _possibleDatetimeFormatInfs;
            }
        }
        private ICollection<DateTimeFormatInfo> _possibleDatetimeFormatInfs;
        #endregion YandexRss

        private readonly ConfigurationSectionGroup _indexingGroup;

        public IndexingActualConfiguration()
        {
            _indexingGroup = ConfigurationManager
                             .OpenExeConfiguration(ConfigurationUserLevel.None)
                             .SectionGroups["indexingSettings"];
        }
    }
}