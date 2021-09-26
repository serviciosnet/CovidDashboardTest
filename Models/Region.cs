using System.Collections.Generic;

namespace CovidDashboardTest.Models
{
    public class Region
    {
        public string iso { get; set; }
        public string name { get; set; }
        public string province { get; set; }
        public string lat { get; set; }
        public string @long { get; set; }
        public List<object> cities { get; set; }
    }
}
