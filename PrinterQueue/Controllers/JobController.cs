using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PrinterQueue.Database;
using PrinterQueue.Models;
using PrinterQueue.Services;

namespace PrinterQueue.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobController : ControllerBase
    {
        private readonly IJobsRepository _repository;
        private readonly IHubContext<SignalRHub> _hub;
        private static PrinterService _printerService;
        public JobController(IJobsRepository repo, IHubContext<SignalRHub> hub)
        {
            _repository = repo;
            _hub = hub;
            _printerService = new PrinterService(_repository, _hub);
        }

        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<PrinterJob> result = null;
            try
            {
                result = _repository.GetAll();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
                        
            return Ok(result);
        }

        [HttpPost]
        [Route("InitPrinting")]
        public IActionResult InitPrinting()
        {
            try
            {
                _printerService.StartPrinting();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPost]
        [Route("NewJob")]
        public IActionResult NewJob(PrinterJob pj)
        {
            try
            {
                bool isItJobToPrintNow = _repository.AddNewJob(pj);
                if (isItJobToPrintNow)
                {
                    _printerService.StartPrinting();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPost]
        [Route("MoveUp")]
        public IActionResult MoveUp(int id)
        {
            try
            {
                _repository.MoveUp(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPost]
        [Route("MoveDown")]
        public IActionResult MoveDown(int id)
        {
            try
            {
                _repository.MoveDown(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPost]
        [Route("CancelPrintingJob")]
        public IActionResult CancelPrintingJob()
        {
            try
            {
                _repository.CancelPrintingJob();
                _printerService.CancelPrintingJob();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                _repository.DeleteById(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
