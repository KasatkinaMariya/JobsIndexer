using System;

namespace VacanciesIndexing.BackEndFormat
{
    class BackEndVacancyFormat
    {
        public string VacancyTitle { get; set; }
        public string Company { get; set; }
        public string City { get; set; }

        public int? SalaryMin { get; set; }
        public int? SalaryMax { get; set; }
        public string SalaryCurrency { get; set; }

        public string Education { get; set; }
        public double? ExperienceMin { get; set; }
        public double? ExperienceMax { get; set; }

        public string EmploymentType { get; set; }
        public string Skills { get; set; }
        public string Description { get; set; }

        public string ContactPerson { get; set; }
        public string ContactCompany { get; set; }
        public string TelephoneNumber { get; set; }
        public string Email { get; set; }
        public string Site { get; set; }

        public DateTime DateAdded { get; set; }
        public DateTime DateUpdated { get; set; }
        public int NumberOfViews { get; set; }

        public VacancyProvider SourceName { get; set; }
        public long InnerId { get; set; }
        public Uri Link { get; set; }
    }
}