using System;

namespace VacanciesIndexing.ConvertersToBackEnd.E1
{
    internal class E1PublishInfo
    {
        public DateTime DateAdded { get; private set; }
        public DateTime DateUpdated { get; private set; }
        public int NumberOfViews { get; private set; }
        public int InnerId { get; private set; }

        private static readonly char[] _st_splitters = { ' ', ',', '\n' };

        public E1PublishInfo(string raVacancyFullInfo)
        {
            var fullInfoParts = raVacancyFullInfo.Split
                (_st_splitters, StringSplitOptions.RemoveEmptyEntries);
                
            InnerId = Int32.Parse(fullInfoParts[3]);
            DateAdded = DateTime.Parse(fullInfoParts[5]);
            DateUpdated = DateTime.Parse(fullInfoParts[7]);
            NumberOfViews = Int32.Parse(fullInfoParts[8]);
        }
    }
}