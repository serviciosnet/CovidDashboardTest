using ClosedXML.Excel;
using CovidDashboardTest.Models;
using iTextSharp.text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CovidDashboardTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportDataController : ControllerBase
    {
        private Services.CovidDataService covidDataService = new Services.CovidDataService();
       
        [HttpPost("exportdatatofile")]
        public async Task<IActionResult> ExporDataToFile([FromBody] ExportOptions exportOptions)
        {
            //var dictioneryexportType = Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString());
            //var exportType = dictioneryexportType["Export"];
            List<ReportResultModel> dataResult = await covidDataService.ReportDataAsync(exportOptions.startDate, exportOptions.filterId);
            switch (exportOptions.exportType.ToUpper())
            {
                case "CSV":
                    ExportToCsv(Helpers.Convert.ConvertToDataTable(dataResult));
                    break;
                case "JSON":
                    ExportToJson(dataResult);
                    break;
                case "XML":
                    ExportToXML(dataResult);
                    break;

            }
            return null;
        }
       
        private void ExportToCsv(DataTable details)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = details.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in details.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field =>
                  string.Concat("\"", field.ToString().Replace("\"", "\"\""), "\""));
                sb.AppendLine(string.Join(",", fields));
            }
            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(sb.ToString());
            Response.Clear();
            Response.Headers.Add("content-disposition", "attachment;filename=ProductDetails.csv");
            Response.ContentType = "application/text";
            Response.Body.WriteAsync(byteArray);
            Response.Body.Flush();
        }

        private void ExportToJson(List<ReportResultModel> details)
        {
            string jsonProductList = new JavaScriptSerializer().Serialize(details);
            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(jsonProductList);

            Response.Clear();
            Response.Headers.Clear();
            Response.ContentType = "application/json";
            Response.Headers.Add("Content-Length", jsonProductList.Length.ToString());
            Response.Headers.Add("Content-Disposition", "attachment; filename=ProductDetails.json;");
            Response.Body.WriteAsync(byteArray);
            Response.Body.FlushAsync();
        }
        private void ExportToXML(List<ReportResultModel> details)
        {
            
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("Products");
            xml.AppendChild(root);
            foreach (var item in details)
            {
                XmlElement child = xml.CreateElement("record");
                child.SetAttribute("City", item.City.ToString().Trim());
                child.SetAttribute("ConfirmedCases", item.ConfirmedCases.ToString());
                child.SetAttribute("ActiveCases", item.ActiveCases.ToString());
                child.SetAttribute("DeceasedCases", item.DeceasedCases.ToString());
                child.SetAttribute("RecoveredCases", item.RecoveredCases.ToString());
                root.AppendChild(child);
            }
            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(xml.OuterXml.ToString());

            Response.Clear();
            Response.Headers.Clear();
            Response.ContentType = "application/xml";
            Response.Headers.Add("Content-Disposition", "attachment; filename=ProductDetails.xml;");
            Response.Body.WriteAsync(byteArray);
            Response.Body.Flush();
        }
 
    }
}
