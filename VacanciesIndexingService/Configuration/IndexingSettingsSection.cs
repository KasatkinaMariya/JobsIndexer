using System.Configuration;
using System.Globalization;
using System.Linq;

namespace VacanciesIndexing.Configuration
{
    public class IndexingSettingsSectionGroup : ConfigurationSectionGroup
    {
        [ConfigurationProperty("general", IsRequired = true)]
        public GeneralSection General
        {
            get { return (GeneralSection)Sections["general"]; }
        }

        [ConfigurationProperty("database", IsRequired = true)]
        public DatabaseSection Database
        {
            get { return (DatabaseSection)Sections["database"]; }
        }

        [ConfigurationProperty("yandexRss", IsRequired = true)]
        public YandexRssSection YandexRss
        {
            get { return (YandexRssSection)Sections["yandexRss"]; }
        }
    }

    public class GeneralSection : ConfigurationSection
    {
        [ConfigurationProperty("hoursNumberBetweenIndexingRuns", IsRequired = true)]
        public ValueElement HoursNumberBetweenIndexingRuns
        {
            get { return (ValueElement)base["hoursNumberBetweenIndexingRuns"]; }
        }
    }

    public class DatabaseSection : ConfigurationSection
    {
        [ConfigurationProperty("pathToCreatingObjectsScript", IsRequired = true)]
        public ValueElement PathToCreatingObjectsScript
        {
            get { return (ValueElement)base["pathToCreatingObjectsScript"]; }
        }

        [ConfigurationProperty("connectionString", IsRequired = true)]
        public ValueElement ConnectionString
        {
            get { return (ValueElement)base["connectionString"]; }
        }
    }

    public class YandexRssSection : ConfigurationSection
    {
        [ConfigurationProperty("urlPattern", IsRequired = true)]
        public ValueElement UrlPattern
        {
            get { return (ValueElement)base["urlPattern"]; }
        }

        [ConfigurationProperty("possibleDatetimeFormats", IsDefaultCollection = true)]
        public DatetimeFormatsCollection PossibleDatetimeFormats
        {
            get { return (DatetimeFormatsCollection)base["possibleDatetimeFormats"]; }
        }
    }

    public class ValueElement : ConfigurationElement
    {
        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return (string)base["value"]; }
            set { base["value"] = value; }
        }
    }

    [ConfigurationCollection(typeof(DatetimeFormatElement), AddItemName = "datetimeFormatInfo")]
    public class DatetimeFormatsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DatetimeFormatElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DatetimeFormatElement)element).FullDatetimePattern;
        }
    }

    public class DatetimeFormatElement : ConfigurationElement
    {
        [ConfigurationProperty("fullDatetimePattern", IsRequired = true, IsKey = true)]
        public string FullDatetimePattern
        {
            get { return (string)this["fullDatetimePattern"]; }
            set { this["fullDatetimePattern"] = value; }
        }

        [ConfigurationProperty("abbreviatedDayNames", IsRequired = true)]
        public string AbbreviatedDayNames
        {
            get { return (string)this["abbreviatedDayNames"]; }
            set { this["abbreviatedDayNames"] = value; }
        }

        [ConfigurationProperty("abbreviatedMonthNames", IsRequired = true)]
        public string AbbreviatedMonthNames
        {
            get { return (string)this["abbreviatedMonthNames"]; }
            set { this["abbreviatedMonthNames"] = value; }
        }

        public DateTimeFormatInfo ConvertToSystemFormatInfo()
        {
            return new DateTimeFormatInfo()
            {
                AbbreviatedDayNames = AbbreviatedDayNames.Split(' '),
                AbbreviatedMonthNames = AbbreviatedMonthNames.Split(' ')
                                                             .Union(new[] {""})
                                                             .ToArray(),
                FullDateTimePattern = FullDatetimePattern
            };
        }
    }
}
