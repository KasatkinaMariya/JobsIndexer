using System;
using System.Data.SqlClient;
using System.Linq;

using VacanciesIndexing.ConvertersToBackEnd;
using VacanciesIndexing.Database;
using VacanciesIndexing.LinksCollector;

namespace VacanciesIndexing
{
    class LoadingAndSavingManager
    {
        private readonly LinksCollectorBase _linksCollector;
        private readonly string _connectionString;

        public void Run()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                DateTime lastUpdateDatetime = DatabaseUtils.LoadLastUpdateDatetime(connection);
                IndexingService.Log.Info(string.Format("LastUpdateDatetime is '{0}'", lastUpdateDatetime));
                
                var links = _linksCollector.CollectInterestingUrls(lastUpdateDatetime);
                IndexingService.Log.Info(string.Format("{0} interesting links to vacancies was collected", links.Count()));
                
                foreach (string link in links)
                {
                    try
                    {
                        IndexingService.Log.Debug(string.Format("Start processing '{0}'", link));

                        var parsedVacancy = ConverterToBackEndFormat.FabricCreate(link)
                                            .ParseContentToBackEndFormat();
                        IndexingService.Log.Debug(string.Format("'{0}' parsed", link));

                        DatabaseUtils.SaveVacancy(parsedVacancy,connection);
                        IndexingService.Log.Info(string.Format("'{0}' saved to database", link));
                    }
                    catch (Exception e)
                    {
                        IndexingService.Log.Error(string.Format("Exception while processing '{0}'", link), e);
                    }
                }

                DatabaseUtils.SetLastUpdateDatetime(DateTime.Now,connection);
                IndexingService.Log.Info("LastUpdateDatetime was updated in database");
            }
        }

        public LoadingAndSavingManager(LinksCollectorBase linksCollector)
        {
            _linksCollector = linksCollector;
            _connectionString = IndexingService.ActualConfiguration.ConnectionString;

            IndexingService.Log.Info(string.Format("LoadingAndSavingManager uses '{0}' to collect links", linksCollector.GetType()));
            IndexingService.Log.Info(string.Format("LoadingAndSavingManager uses '{0}' to connect to database", _connectionString));
        }
    }
}
