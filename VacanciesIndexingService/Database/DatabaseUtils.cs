using System;
using System.Data;
using System.Data.SqlClient;

using VacanciesIndexing.BackEndFormat;

namespace VacanciesIndexing.Database
{
    static class DatabaseUtils
    {
        public static DateTime LoadLastUpdateDatetime(SqlConnection activeConnection)
        {
            var selectText = "SELECT MAX(DatetimeMark) FROM dbo.LastUpdateDatetime";
            var selectingCommand = new SqlCommand(selectText, activeConnection);

            var lastUpdateDatetime = selectingCommand.ExecuteScalar() as DateTime?;
            return lastUpdateDatetime ?? DateTime.MinValue;
        }

        public static void SetLastUpdateDatetime(DateTime datetimeToSave,
                                                 SqlConnection activeConnection)
        {
            var settingCommand = new SqlCommand("dbo.SetLastUpdateDatetime", activeConnection)
            {
                CommandType = CommandType.StoredProcedure,
                Parameters =
                {
                    new SqlParameter("@DatetimeToSave", datetimeToSave),
                }
            };

            settingCommand.ExecuteNonQuery();
        }

        public static void SaveVacancy(BackEndVacancyFormat vacancyToSave,
                                       SqlConnection activeConnection)
        {
            var insertCommand = new SqlCommand("InsertVacancyDetails", activeConnection)
            {
                CommandType = CommandType.StoredProcedure,
                Parameters =
                {
                    new SqlParameter("@VacancyTitle", vacancyToSave.VacancyTitle),
                    new SqlParameter("@CompanyName", vacancyToSave.Company),
                    new SqlParameter("@City", vacancyToSave.City),

                    new SqlParameter("@SalaryMin", vacancyToSave.SalaryMin),
                    new SqlParameter("@SalaryMax", vacancyToSave.SalaryMax),
                    new SqlParameter("@SalaryCurrency", vacancyToSave.SalaryCurrency),

                    new SqlParameter("@Education", vacancyToSave.Education),
                    new SqlParameter("@ExperienceMin", vacancyToSave.ExperienceMin),
                    new SqlParameter("@ExperienceMax", vacancyToSave.ExperienceMax),
                    new SqlParameter("@EmploymentType", vacancyToSave.EmploymentType),
                    new SqlParameter("@Skills", vacancyToSave.Skills),
                    new SqlParameter("@VacancyText", vacancyToSave.Description),

                    new SqlParameter("@ContactPerson", vacancyToSave.ContactPerson),
                    new SqlParameter("@ContactCompany", vacancyToSave.ContactCompany),
                    new SqlParameter("@ContactPhone", vacancyToSave.TelephoneNumber),
                    new SqlParameter("@ContactEmail", vacancyToSave.Email),
                    new SqlParameter("@ContactSite", vacancyToSave.Site),

                    new SqlParameter("@DateAdded", vacancyToSave.DateAdded),
                    new SqlParameter("@DateUpdated", vacancyToSave.DateUpdated),
                    new SqlParameter("@NumberOfViews", vacancyToSave.NumberOfViews),
                    new SqlParameter("@SourceName", vacancyToSave.SourceName.ToString()),
                    new SqlParameter("@SourceInnerId", vacancyToSave.InnerId),
                    new SqlParameter("@SourceLink", vacancyToSave.Link.AbsoluteUri),
                }
            };

            insertCommand.ExecuteNonQuery();
        }
    }
}
