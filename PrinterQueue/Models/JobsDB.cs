using System.Collections.Generic;

namespace PrinterQueue.Models
{
    public class JobsDB
    {
        public int NextId { get; set; }

        public List<PrinterJob> Jobs { get; set; }
    }
}
