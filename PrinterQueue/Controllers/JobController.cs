using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using PrinterQueue.Database;
using PrinterQueue.Models;

namespace PrinterQueue.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobController : ControllerBase
    {
        private readonly IJobsRepository _repository;
        public JobController(IJobsRepository repo)
        {
            _repository = repo;
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
        [Route("NewJob")]
        public IActionResult NewJob(PrinterJob pj)
        {
            try
            {
                _repository.AddNewJob(pj);
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
