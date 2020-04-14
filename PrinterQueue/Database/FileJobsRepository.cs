using Newtonsoft.Json;
using PrinterQueue.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PrinterQueue.Database
{
    public class FileJobsRepository : IJobsRepository
    {
        private readonly string jobsDatabaseFilePath = @"Database\jobs.json";
        public IEnumerable<PrinterJob> GetAll()
        {
            JobsDB jobsDB = GetTheJobsDB();
            return jobsDB.Jobs.OrderBy(j => j.Order);
        }

        public void AddNewJob(PrinterJob pj)
        {
            if (string.IsNullOrWhiteSpace(pj.Name))
                throw new ArgumentException("Job name is empty");

            if (string.IsNullOrWhiteSpace(pj.Duration))
                throw new ArgumentException("Duration is corrupt or zero");

            JobsDB jobsDB = GetTheJobsDB();

            pj.Id = jobsDB.NextId;
            jobsDB.NextId++;

            if (jobsDB.Jobs.Count == 0)
            {
                pj.Status = PrintJobStatusEnum.Printing.ToDescription();
                pj.Order = 1;
            }
            else
            {
                pj.Status = PrintJobStatusEnum.Queued.ToDescription();
                pj.Order = jobsDB.Jobs.Max(j => j.Order) + 1;
            }

            jobsDB.Jobs.Add(pj);

            SetTheJobsDB(jobsDB);
        }

        public void MoveUp(int id)
        {
            JobsDB jobsDB = GetTheJobsDB();
            PrinterJob pj = jobsDB.Jobs.SingleOrDefault(j => j.Id == id);

            if (pj == null)
                throw new ArgumentException("Can't find relevant printing job");

            else if (pj.Status == PrintJobStatusEnum.Printing.ToDescription())
                throw new ArgumentException("Can't move up the printing job");

            else if (pj.Order == 2)
                throw new ArgumentException("Can't move up over the printing job");

            else
            {
                PrinterJob topPG = jobsDB.Jobs.SingleOrDefault(j => j.Order == pj.Order - 1);
                if (topPG != null)
                    topPG.Order++;

                pj.Order--;

                SetTheJobsDB(jobsDB);
            }
        }

        public void MoveDown(int id)
        {
            JobsDB jobsDB = GetTheJobsDB();
            PrinterJob pj = jobsDB.Jobs.SingleOrDefault(j => j.Id == id);

            if (pj == null)
                throw new ArgumentException("Can't find relevant printing job");

            else if (pj.Status == PrintJobStatusEnum.Printing.ToDescription())
                throw new ArgumentException("Can't move down the printing job");

            else if (pj.Order == jobsDB.Jobs.Count)
                throw new ArgumentException("Can't move down, it's already the last printing job");

            else
            {
                PrinterJob bottomPG = jobsDB.Jobs.SingleOrDefault(j => j.Order == pj.Order + 1);
                if (bottomPG != null)
                    bottomPG.Order--;

                pj.Order++;

                SetTheJobsDB(jobsDB);
            }
        }

        public void CancelPrintingJob()
        {
            JobsDB jobsDB = GetTheJobsDB();
            PrinterJob pj = jobsDB.Jobs.SingleOrDefault(j => j.Status == PrintJobStatusEnum.Printing.ToDescription());

            if (pj == null)
                throw new ArgumentException("Can't find relevant printing job");

            else
            {
                PrinterJob bottomPG = jobsDB.Jobs.SingleOrDefault(j => j.Order == pj.Order + 1);
                if (bottomPG != null)
                {
                    bottomPG.Status = PrintJobStatusEnum.Printing.ToDescription();
                    bottomPG.Order--;
                }

                pj.Status = PrintJobStatusEnum.Queued.ToDescription();
                pj.Order++;

                SetTheJobsDB(jobsDB);
            }
        }

        public void DeleteById(int id)
        {
            JobsDB jobsDB = GetTheJobsDB();
            PrinterJob pj = jobsDB.Jobs.SingleOrDefault(j => j.Id == id);

            if (pj == null)
                throw new ArgumentException("Can't find relevant printing job");

            else if (pj.Status == PrintJobStatusEnum.Printing.ToDescription())
                throw new ArgumentException("Can't delete the printing job");

            else
            {
                jobsDB.Jobs.Where(j => j.Order > pj.Order).ToList().ForEach(j => j.Order--);
                jobsDB.Jobs.Remove(pj);
                SetTheJobsDB(jobsDB);
            }
        }

        private JobsDB GetTheJobsDB()
        {
            JobsDB jobsDB = null;
            using (StreamReader r = new StreamReader(jobsDatabaseFilePath))
            {
                string json = r.ReadToEnd();
                jobsDB = JsonConvert.DeserializeObject<JobsDB>(json);
            }
            return jobsDB;
        }

        private void SetTheJobsDB(JobsDB jobsDB)
        {
            using (StreamWriter file = File.CreateText(jobsDatabaseFilePath))
            {
                string json = JsonConvert.SerializeObject(jobsDB, Formatting.Indented);
                file.Write(json);
            }
        }
    }
}
