using System.ServiceProcess;

namespace VacanciesIndexing
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBase.Run(new IndexingService());
        }
    }
}