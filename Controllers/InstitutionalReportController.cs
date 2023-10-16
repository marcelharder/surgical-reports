using surgical_reports.helpers;

namespace surgical_reports.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InstitutionalReportController : ControllerBase
{
    IInstitutionalText _t;
    public InstitutionalReportController(IInstitutionalText t)
    {
        _t = t;
    }

    [HttpGet("{id}/{soort}/{procedure_id}")]
    public async Task<IActionResult> getInstitutionalReport(string id, string soort, int procedure_id)
    {
        var result = await _t.getText(id, soort, procedure_id);
        return Ok(result);
    }

 #region <!-- InstitutionalReports stuff -->
        
        [HttpPut("InstitutionalReport/{id}/{soort}")]
        public IActionResult updateIRep([FromBody] InstitutionalDTO rep, string id, int soort){
             var help = _t.updateInstitutionalReport(rep,soort,Convert.ToInt32(id));
          return Ok(help);
        }
        [HttpPost("InstitutionalReport/{id}")]
        public IActionResult createIRep(string id){
             var help = _t.addRecordInXML(id);
          return Ok(help);
        }

        [HttpGet("AdditionalReportItems/{id}/{which}")]
        public IActionResult getARI(string id, int which){
          var help = _t.getAdditionalReportItems(Convert.ToInt32(id),which);
          return Ok(help);
        }

        [HttpPut("UpdateAdditionalReportItems/{id}/{which}")]
        public IActionResult getARk([FromBody] AdditionalReportDTO l,string id,int which){
          var help = _t.updateAdditionalReportItem(l,Convert.ToInt32(id),which);
          return Ok(help);
        }

        #endregion




}

