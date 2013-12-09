using System;
using System.ServiceProcess;
using System.Timers;
using VacanciesIndexing.Database;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;

using VacanciesIndexing.Configuration;
using VacanciesIndexing.LinksCollector;

namespace VacanciesIndexing
{
    public partial class IndexingService : ServiceBase
    {
        public static ILog Log;
        public static IndexingActualConfiguration ActualConfiguration;

        private Timer _indexingRunsTimer;

        public IndexingService()
        {
            InitializeComponent();

            Log = InitLog4net();
            ActualConfiguration = new IndexingActualConfiguration();

            DatabaseDeployer.PrepareDatabase();
        }

        protected override void OnStart(string[] args)
        {
            _indexingRunsTimer = new Timer(ActualConfiguration.HoursNumberBetweenIndexingRuns * 60 * 60 * 1000);
            _indexingRunsTimer.Elapsed += (sender, eventArgs) => ExecuteIndexingOnce();
            _indexingRunsTimer.Start();
        }

        private void ExecuteIndexingOnce()
        {
            Log.Info("VacanciesIndexer started");

            try
            {
                var manager = new LoadingAndSavingManager(new YandexRssCollector());
                manager.Run();
            }
            catch (Exception e)
            {
                Log.Fatal(string.Empty, e);
            }
            catch
            {
                Log.Fatal("Unexpected CLR error");
            }
            finally
            {
                Log.Info("VacanciesIndexer finished\r\n");
            }
        }

        private ILog InitLog4net()
        {
            XmlConfigurator.Configure();
            ILog log = LogManager.GetLogger(typeof(Logger));
            return log;
        }
    }
}
