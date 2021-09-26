using CovidDashboardTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidDashboardTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private Services.CovidDataService covidDataService = new Services.CovidDataService();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[HttpPost("ExportToCSV")]
        //public async Task<FileResult> ExportToCSV(string startDate, string filterId)
        //{
        //    try
        //    {
        //        RootReport dataResult = await covidDataService.ReportDataAsync(startDate, filterId);
        //        var bulider = new StringBuilder();
        //        var fileName = $"Covid_19_statistics_{filterId}_{startDate.Replace("-", "")}.csv";
        //        var title = filterId.Equals("000") ? "Region" : "Providence";
        //        bulider.AppendLine($"{title}, Confirmed Cases, Active Cases, Deceased Cases, Recovered Cases");
        //        foreach (var item in dataResult.data)
        //        {
        //            if (filterId.Equals("000"))
        //            {
        //                bulider.AppendLine($"{item.region.name},{item.confirmed},{item.active},{item.deaths},{item.recovered}");
        //            }
        //            else
        //            {
        //                if (item.region.province == "" || item.region.province == null)
        //                {
        //                    bulider.AppendLine($"{item.region.name},{item.confirmed},{item.active},{item.deaths},{item.recovered}");
        //                }
        //                else
        //                {
        //                    bulider.AppendLine($"{item.region.province},{item.confirmed},{item.active},{item.deaths},{item.recovered}");
        //                }
        //            }
        //            bulider.Append("\r\n");

        //        }
        //        return File(Encoding.ASCII.GetBytes(bulider.ToString()), "text/csv", fileName);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
           

        //}
    }
}
