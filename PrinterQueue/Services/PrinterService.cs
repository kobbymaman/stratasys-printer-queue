using Microsoft.AspNetCore.SignalR;
using PrinterQueue.Database;
using PrinterQueue.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace PrinterQueue.Services
{
    public class PrinterService
    {
        private readonly IJobsRepository _repository;
        private readonly IHubContext<SignalRHub> _hub;

        private static Task printingLoop = null;
        private static CancellationTokenSource printingJobCTS = new CancellationTokenSource();

        public PrinterService(IJobsRepository repo, IHubContext<SignalRHub> hub)
        {
            _repository = repo;
            _hub = hub;
        }

        public void StartPrinting()
        {
            if (printingLoop != null && !printingLoop.IsCompleted)
            {
                return;
            }

            printingLoop = Task.Factory.StartNew(() => { 
                while (true) {
                    var pj =_repository.GetCurrentPrintingJob();
                    if (pj == null)
                        return;
                    else
                    {
                        TimeSpan jobDuration = TimeSpan.Zero;
                        try { jobDuration = XmlConvert.ToTimeSpan(pj.Duration); } catch { return; }

                        if (jobDuration == TimeSpan.Zero)
                            return;
                        else
                        {
                            try
                            {
                                Task.Delay(jobDuration, printingJobCTS.Token).Wait();
                                _repository.RemovePrintedJob();
                                _hub.Clients.All.SendAsync("printjobcompleted", string.Format("Name: {0}, Duration: {1}", pj.Name, jobDuration.ToString()));
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message.Contains("A task was canceled"))
                                {
                                    printingJobCTS = new CancellationTokenSource();
                                }
                            }
                        }
                    }
                }
            });
        }

        public void CancelPrintingJob()
        {
            printingJobCTS.Cancel();
        }
    }
}
