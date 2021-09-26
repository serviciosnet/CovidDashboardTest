using System.Collections.Generic;

namespace CovidDashboardTest.Models
{
    public class ProvidenceRoot
    {
        public List<ProvidenceItem> Data {  get; set; }
        public ProvidenceRoot()
        {
            Data = new List<ProvidenceItem>();  
        }
    }
}
