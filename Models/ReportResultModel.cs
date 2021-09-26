using System;

namespace CovidDashboardTest.Models
{
    public class ReportResultModel
    {
        public string City { get; set; }
        public Int64 ConfirmedCases {  get; set; }
        public Int64 ActiveCases {  get; set; }
        public Int64 DeceasedCases {  get; set; }
        public Int64 RecoveredCases {  get; set;}
    }
}
