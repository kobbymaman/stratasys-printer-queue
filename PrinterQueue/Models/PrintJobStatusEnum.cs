
using System.ComponentModel;

namespace PrinterQueue.Models
{
    public enum PrintJobStatusEnum
    {
        [Description("Printing")]
        Printing,

        [Description("Queued")]
        Queued
    }
}
