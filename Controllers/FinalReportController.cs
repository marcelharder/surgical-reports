using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using surgical_reports.entities.dtos;
using surgical_reports.Entities;
using surgical_reports.implementations;

namespace surgical_reports.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinalReportController : ControllerBase
    {
        private readonly IComposeFinalReport _final;

        public FinalReportController(IComposeFinalReport final)
        {
            _final = final;
        }

        [HttpGet]
        public async Task<IActionResult> getReports()
        {
            var result = await _final.getFinalReports();
            return Ok(result);
        }

        [HttpGet("{id}", Name = "getFinalReport")]
        public async Task<IActionResult> getReports(int id)
        {
            var result = await _final.getSpecificReport(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> createFinalReport(frDto final)
        {
            try
            {
                var createdFinalReport = await _final.CreateFinalReport(final);
                return CreatedAtRoute("getFinalReport", new { id = createdFinalReport.Id }, createdFinalReport);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
    }
}