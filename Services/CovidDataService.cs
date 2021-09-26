using CovidDashboardTest.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace CovidDashboardTest.Services
{
    public class CovidDataService
    {
        public async Task<List<ReportResultModel>> ReportDataAsync(string startDate, string filterId)
        {
            try
            {
                List<ReportResultModel> dataReport = new List<ReportResultModel>();
                string BaseUrl = $"https://covid-19-statistics.p.rapidapi.com/reports?date={startDate}";
                if (filterId != "000")
                    BaseUrl = BaseUrl + $"&iso={filterId}";
                var client = new RestClient(BaseUrl);
                var request = new RestRequest(Method.GET);
                request.AddHeader("x-rapidapi-host", "covid-19-statistics.p.rapidapi.com");
                request.AddHeader("x-rapidapi-key", "5cc7171b8bmsh01c77501925a72cp1aab57jsn94e1b77d4cd7");
                IRestResponse response = await client.ExecuteAsync(request);

                Models.RootReport rootReport = new Models.RootReport();

                rootReport = JsonConvert.DeserializeObject<Models.RootReport>(response.Content);

                if (filterId.Equals("000"))
                {
                    dataReport = (from detail in rootReport.data
                                  orderby detail.confirmed descending
                                  select new ReportResultModel
                                  {
                                      City = detail.region.name,
                                      ConfirmedCases = detail.confirmed,
                                      ActiveCases = detail.active,
                                      DeceasedCases = detail.deaths,
                                      RecoveredCases = detail.recovered
                                  }).Take(10).ToList();
                }
                else
                {
                    dataReport = (from detail in rootReport.data
                                  orderby detail.confirmed descending
                                  select new ReportResultModel
                                  {
                                      City = detail.region.province == "" ? detail.region.name : detail.region.province,
                                      ConfirmedCases = detail.confirmed,
                                      ActiveCases = detail.active,
                                      DeceasedCases = detail.deaths,
                                      RecoveredCases = detail.recovered
                                  }).Take(10).ToList();
                }


                return dataReport;

            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        public async Task<List<ProvidenceItem>> ProvidenceListAsync()
        {
            try
            {
                Models.ProvidenceRoot myList = new();
                var client = new RestClient("https://covid-19-statistics.p.rapidapi.com/regions");
                var request = new RestRequest(Method.GET);
                request.AddHeader("x-rapidapi-host", "covid-19-statistics.p.rapidapi.com");
                request.AddHeader("x-rapidapi-key", "5cc7171b8bmsh01c77501925a72cp1aab57jsn94e1b77d4cd7");
                IRestResponse response = await client.ExecuteAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    myList.Data.Add(new Models.ProvidenceItem() { iso = "000", name = "Worldwide" });
                    Models.ProvidenceRoot myDeserializedClass = JsonConvert.DeserializeObject<Models.ProvidenceRoot>(response.Content);
                    myList.Data.AddRange(myDeserializedClass.Data);
                    return myList.Data;
                }
                else
                {
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
