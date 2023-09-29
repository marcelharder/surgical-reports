using surgical_reports.helpers;

namespace surgical_reports.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PreViewReportController : ControllerBase
{
    private readonly reportMapper _map;
    private IPreviewReport _repo;
    

    public PreViewReportController(reportMapper map, IPreviewReport repo)
    {
        _map = map;
        _repo = repo;
    }

    [HttpGet("{id}", Name = "getPreviewReport")]
    public async Task<IActionResult> getReport(int id) { return Ok( await _repo.getPreViewAsync(id)); }
   
    [HttpGet("{reset/id}", Name = "getPreviewReport")]
    public async Task<IActionResult> resetReport(int id){ await _repo.resetPreViewAsync(id); return Ok(); }
    
    
    [HttpPost]
    // this comes from the save and print button and results in a pdf
    public async Task<IActionResult> Post(PreviewForReturnDTO pvfr)
    {
         try
            {  
                Class_privacy_model pm = _map.mapToClassPrivacyModel(pvfr);
                Class_Preview_Operative_report pv = await _repo.getPreViewAsync(pvfr.procedure_id);
                pv = _map.mapToClassPreviewOperativeReport(pvfr, pv);

                // save the Class_Preview_Operative_report to the database first
                var result = await _repo.updatePVR(pv);

                // generate final operative report Class
                var classFR = await _map.updateFinalReportAsync(pm, pv.procedure_id);

               
               return Ok(result);
            }
            catch (Exception e) { Console.WriteLine(e.InnerException); }
            return BadRequest("Error saving the preview report");
    }


}
