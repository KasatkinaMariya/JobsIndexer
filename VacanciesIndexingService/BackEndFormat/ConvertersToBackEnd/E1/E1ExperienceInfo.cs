using System;
using System.Linq;

namespace VacanciesIndexing.ConvertersToBackEnd.E1
{
    internal class E1ExperienceInfo
    {
        public string Education { get; private set; }
        public double? ExperienceMin { get; private set; }
        public double? ExperienceMax { get; private set; }

        private readonly static string[] _st_experienceAndEducationSplitters = {" и "};

        public E1ExperienceInfo(string raVacancyFullRequirements)
        {
            string educationStr = null;
            string experienceStr = null;

            var parts = raVacancyFullRequirements.Split(_st_experienceAndEducationSplitters,
                                                        StringSplitOptions.None);
            if (parts.Count() == 2)
            {
                educationStr = parts[0];
                experienceStr = parts[1];
            }
            else
                experienceStr = parts[0];

            if (educationStr != null)
                Education = educationStr.Trim();
            if (experienceStr != null)
            {
                if (experienceStr.Contains("ез опыта"))
                {
                    ExperienceMax = 0;
                }
                else if (experienceStr.Contains("до 1 года"))
                {
                    ExperienceMin = 0;
                    ExperienceMax = 1;
                }
                else if (experienceStr.Contains("1-3 года"))
                {
                    ExperienceMin = 1;
                    ExperienceMax = 3;
                }
                else if (experienceStr.Contains("3-5 лет"))
                {
                    ExperienceMin = 3;
                    ExperienceMax = 5;
                }
                else if (experienceStr.Contains("более 5 лет"))
                {
                    ExperienceMin = 5;
                }
                else
                {
                    IndexingService.Log.Warn(String.Format("Parsing experience '{0}' failed", experienceStr));
                }
            }
        }
    }
}