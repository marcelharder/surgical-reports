namespace surgical_reports.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PreViewReportController : ControllerBase
{
    
    private IPreviewReport _repo;
    private IOperativeReport _iorep;
    private IProcedureRepository _proc;
    private IMapper _map;
    private reportMapper _rm;
    

    public PreViewReportController(
        reportMapper rm,
        IOperativeReport iorep,
        IMapper map,
        IPreviewReport repo, 
        IProcedureRepository proc)
    {
        _map = map;
        _repo = repo;
        _proc = proc;
        _rm = rm;
        _iorep = iorep;
    }

    [HttpGet("{id}", Name = "getPreviewReport")]
    public async Task<IActionResult> getReport(int id) { return Ok( await _repo.getPreViewAsync(id)); }
   
    [HttpGet("reset/{id}")]
    public async Task<IActionResult> resetReport(int id){ await _repo.resetPreViewAsync(id); return Ok(); }
    
    
    [HttpPost]
    // this comes from the save and print button and results in a pdf
    public async Task<IActionResult> Post(PreviewForReturnDTO pvfr)
    {
         try
            {  
                Class_privacy_model pm = _map.Map<PreviewForReturnDTO,Class_privacy_model>(pvfr);
                Class_Preview_Operative_report pv = await _repo.getPreViewAsync(pvfr.procedure_id);
                pv = _map.Map<PreviewForReturnDTO, Class_Preview_Operative_report>(pvfr, pv);
                // save the Class_Preview_Operative_report to the database first
                var result = await _repo.updatePVR(pv);
                // generate final operative report Class
                var classFR = await _rm.updateFinalReportAsync(pm, pv.procedure_id);
                // generate PDF and store for 3 days
               try
               {
                     var current_procedure = await _proc.getSpecificProcedure(pvfr.procedure_id);
                     var help = _repo.getReportCode(current_procedure.fdType.ToString());
                     var report_code = Convert.ToInt32(help);
                     await _iorep.getPdf(report_code,classFR);
               }
               catch (Exception a)
               {
                   Console.WriteLine(a.InnerException); 
                   return BadRequest("Error creating the pdf");
               } 
               return Ok(result);
            }
            catch (Exception e) { Console.WriteLine(e.InnerException); }
            return BadRequest("Error saving the preview report");
    }
}
