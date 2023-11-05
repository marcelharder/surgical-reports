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

    [HttpGet("{hospitalNo}/{soort}")]
    public async Task<IActionResult> getInstitutionalReport(string hospitalNo, string soort)
    {
        var result = await _t.getInstitutionalReport(hospitalNo, soort);
        return Ok(result);
    }
 
    [HttpPut("{hospitalNo}/{soort}")]
    public IActionResult updateInstitutionalReport([FromBody] InstitutionalDTO rep, int soort, int hospitalNo)
    {
        var help = _t.updateInstitutionalReport(rep, soort, hospitalNo);
        return Ok(help);
    }

    [HttpGet("AdditionalReportItems/{id}/{which}")]
    public IActionResult getAdditionalReport(int id, int which)
    {
        var help = _t.getAdditionalReportItems(id, which);
        return Ok(help);
    }

    [HttpPut("AdditionalReportItems/{id}/{which}")]
    public IActionResult updateAdditionalReport([FromBody] AdditionalReportDTO l, int id, int which)
    {
        var help = _t.updateAdditionalReportItem(l, id, which);
        return Ok(help);
    }

  }

