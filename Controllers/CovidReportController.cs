using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;
using System.Linq;
using CovidDashboardTest.Models;
using System.Collections.Generic;
using System.Text;

namespace CovidDashboardTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidReportController : ControllerBase
    {
        private Services.CovidDataService covidDataService = new Services.CovidDataService();
        [HttpGet("getreport/{startDate}/{filterId}")]
        public async Task<IActionResult> GetReport(string startDate,string filterId)
        {
            try
            {
                List<ReportResultModel> dataReport = new List<ReportResultModel>();
                dataReport = await covidDataService.ReportDataAsync(startDate, filterId);
                return Ok(dataReport);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
           
        }
        [HttpGet("getProvidence")]
        public async Task<IActionResult> GetProvidenceAsync()
        {
            try
            {
                return Ok(await covidDataService.ProvidenceListAsync());
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("exportcsv/{startDate}/{filterId}")]
        public async Task<IActionResult> ExportToCSV(string startDate, string filterId)
        {
            List<ReportResultModel> data = (List<ReportResultModel>)await GetReport(startDate, filterId);
            var bulider = new StringBuilder();
            var fileName = $"Covid_19_statistics_{filterId}_{startDate.Replace("-", "")}.csv";
            var title = filterId.Equals("000") ? "Region" : "Providence";
            bulider.AppendLine($"{title}, Confirmed Cases, Active Cases, Deceased Cases, Recovered Cases");
            foreach (var item in data)
            {
                bulider.AppendLine($"{item.City},{item.ConfirmedCases},{item.ActiveCases},{item.DeceasedCases},{item.RecoveredCases}");
            }
            return File(Encoding.UTF8.GetBytes(bulider.ToString()),"text/csv",fileName);
        }
    }
}



