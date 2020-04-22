using PrinterQueue.Models;
using System.Collections.Generic;

namespace PrinterQueue.Database
{
    public interface IJobsRepository
    {
        IEnumerable<PrinterJob> GetAll();
        bool AddNewJob(PrinterJob pj);
        void MoveUp(int id);
        void MoveDown(int id);
        void CancelPrintingJob();
        void DeleteById(int id);
        PrinterJob GetCurrentPrintingJob();
        void RemovePrintedJob();
    }
}
