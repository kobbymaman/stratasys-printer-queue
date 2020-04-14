using System;

namespace PrinterQueue.Models
{
    public class PrinterJob
    {
        public int Id { get; set; }

        public int Order { get; set; }

        public string Name { get; set; }

        public string Duration { get; set; }

        public string Status { get; set; }
    }
}
