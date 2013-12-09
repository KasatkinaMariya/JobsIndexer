using System;
using System.Net;
using System.Text;
using HtmlAgilityPack;

using VacanciesIndexing.BackEndFormat;
using VacanciesIndexing.ConvertersToBackEnd.E1;

namespace VacanciesIndexing.ConvertersToBackEnd
{
    abstract class ConverterToBackEndFormat
    {
        protected readonly string _linkToVacancyDetails;

        protected abstract BackEndVacancyFormat BuildBackEnd(HtmlDocument vacancyDocument);

        public BackEndVacancyFormat ParseContentToBackEndFormat()
        {
            var webClient = new WebClient() { Encoding = Encoding.UTF8 };
            string vacancyHtmlContent = webClient.DownloadString(_linkToVacancyDetails);

            var vacancyHtmlDocument = new HtmlDocument();
            vacancyHtmlDocument.LoadHtml(vacancyHtmlContent);

            return BuildBackEnd(vacancyHtmlDocument);
        }

        public static ConverterToBackEndFormat FabricCreate(string linkToVacancyDetails)
        {
            if (linkToVacancyDetails.StartsWith(@"http://rabota.e1.ru/"))
            {
                return new E1ToBackEndFormat(linkToVacancyDetails);
            }

            var message = string.Format("Unknown vacancy provider. Have no converter to backendformat for {0}.",
                                        linkToVacancyDetails);
            throw new Exception(message);
        }

        protected ConverterToBackEndFormat(string linkToVacancyDetails)
        {
            _linkToVacancyDetails = linkToVacancyDetails;
        }
    }
}