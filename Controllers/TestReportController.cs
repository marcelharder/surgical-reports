using surgical_reports.helpers;

namespace surgical_reports.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestReportController : ControllerBase
{
    IInstitutionalText _t;
    public TestReportController(IInstitutionalText t)
    {
        _t = t;
    }

    [HttpGet("{id}/{soort}/{procedure_id}")]
    public async Task<IActionResult> getHospitalSuggestion(string id, string soort, int procedure_id)
    {
        var result = await _t.getText(id, soort, procedure_id);
        return Ok(result);
    }


}

