using System;
using System.Text.RegularExpressions;

namespace VacanciesIndexing.ConvertersToBackEnd.E1
{
    internal class E1SalaryInfo
    {
        public int? SalaryMin { get; private set; }
        public int? SalaryMax { get; private set; }
        public string SalaryCurrency { get; private set; }

        public E1SalaryInfo(string raVacancyFullSalary)
        {
            if (raVacancyFullSalary.Contains("не указана"))
                return;

            var digitGroupsOccurences = Regex.Matches(raVacancyFullSalary, @"\d+\s+\d+");
            Func<int, int> retrievePayByOccurenceIndex = (occurenceIndex)
                                                         => Int32.Parse(digitGroupsOccurences[occurenceIndex].ToString().Replace(" ", String.Empty));

            switch (digitGroupsOccurences.Count)
            {
                case 1:
                    int pay = retrievePayByOccurenceIndex(0);
                    SalaryMin = pay;
                    SalaryMax = pay;
                    break;
                case 2:
                    SalaryMin = retrievePayByOccurenceIndex(0);
                    SalaryMax = retrievePayByOccurenceIndex(1);
                    break;
                default:
                    Console.WriteLine("Parsing salary '{0}' failed", raVacancyFullSalary);
                    break;
            }

            SalaryCurrency = Regex.Match(raVacancyFullSalary, @"\S+$")
                                  .ToString().Replace(".",String.Empty);
        }
    }
}