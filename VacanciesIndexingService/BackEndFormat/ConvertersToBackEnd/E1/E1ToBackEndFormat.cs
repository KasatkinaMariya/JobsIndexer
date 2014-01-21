using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

using VacanciesIndexing.BackEndFormat;

namespace VacanciesIndexing.ConvertersToBackEnd.E1
{
    class E1ToBackEndFormat : ConverterToBackEndFormat
    {
        protected override BackEndVacancyFormat BuildBackEnd(HtmlDocument vacancyDocument)
        {
            var allNodes = vacancyDocument.DocumentNode.SelectNodes("//*");

            var experienceInfo = new E1ExperienceInfo (SelectNodeValueByAttributeValue(allNodes, "ra-vacancy-full-requirements-list-item"));
            var salaryInfo = new E1SalaryInfo (SelectNodeValueByAttributeValue(allNodes, "ra-vacancy-full-salary"));
            var publishInfo = new E1PublishInfo(SelectNodeValueByAttributeValue(allNodes, "ra-vacancy-full-info"));

            return new BackEndVacancyFormat()
                {
                    SourceName = VacancyProvider.E1,
                    Link = new Uri(_linkToVacancyDetails),

                    VacancyTitle = SelectNodeValueByAttributeValue(allNodes, "ra-vacancy-full-title"),
                    Company = SelectNodeValueByAttributeValue(allNodes, "ra-vacancy-full-flying-box-title"),
                    City = SelectNodeValueByAttributeValue(allNodes, "ra-vacancy-full-location"),

                    SalaryMin = salaryInfo.SalaryMin,
                    SalaryMax = salaryInfo.SalaryMax,
                    SalaryCurrency = salaryInfo.SalaryCurrency,

                    Education = experienceInfo.Education,
                    ExperienceMin = experienceInfo.ExperienceMin,
                    ExperienceMax = experienceInfo.ExperienceMax,

                    EmploymentType = SelectNodeValueByAttributeValue(allNodes, "employmentType"),
                    Skills = SelectNodeValueByAttributeValue(allNodes, "ra-vacancy-full-skills"),
                    Description = SelectNodeValueByAttributeValue(allNodes, "ra-vacancy-full-description"),

                    ContactPerson = SelectNodeValueByAttributeValue(allNodes, "ra-vacancy-full-contact-info-person"),
                    ContactCompany = SelectNodeValueByAttributeValue(allNodes, "ra-vacancy-full-contact-info-company"),
                    TelephoneNumber = SelectNodeValueByAttributeValue(allNodes, "ra-vacancy-full-contact-info-phone"),
                    Email = "",
                    Site = SelectNodeValueByAttributeValue(allNodes, "ra-vacancy-full-contact-info-site"),

                    DateAdded = publishInfo.DateAdded,
                    DateUpdated = publishInfo.DateUpdated,
                    NumberOfViews = publishInfo.NumberOfViews,
                    InnerId = publishInfo.InnerId
                };
        }

        public E1ToBackEndFormat(string linkToVacancyDetails)
            : base (linkToVacancyDetails)
        {
        }

        private static string SelectNodeValueByAttributeValue
            (IEnumerable<HtmlNode> nodes, string attributeValue)
        {
            try
            {
                var relevantNode = nodes.First(node => node.Attributes
                                                       .FirstOrDefault(a => a.Value == attributeValue) != null);
                return relevantNode.InnerText.Trim();
            }
            catch (Exception e)
            {
                IndexingService.Log.Debug(string.Format("Not found attribute '{0}'", attributeValue));
                return "";
            }
        }
    }
}