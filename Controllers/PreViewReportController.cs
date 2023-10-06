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
                Class_Preview_Operative_report pv  = new Class_Preview_Operative_report();

                if(await _repo.findPreview(pvfr.procedure_id)){pv = await _repo.getSpecificPVR(pvfr.procedure_id);}
                else {return BadRequest("No preview found in the database ...");}
              
                pv = _map.Map<PreviewForReturnDTO, Class_Preview_Operative_report>(pvfr, pv);
                // save the Class_Preview_Operative_report to the database first
                var result = await _repo.updatePVR(pv);

                // generate final operative report Class
                 Class_Procedure cp = await _proc.getSpecificProcedure(pv.procedure_id);
                 var help = _repo.getReportCode(cp.fdType.ToString());
                 var report_code = Convert.ToInt32(help);

                var classFR = await _rm.updateFinalReportAsync(pm,Class_Procedure cp, int report_code);
                // generate PDF and store for 3 days
               try { await _iorep.getPdf(report_code,classFR); }
               catch (Exception a){ Console.WriteLine(a.InnerException); return BadRequest("Error creating the pdf"); } 
               return Ok(result);
            }
            catch (Exception e) { Console.WriteLine(e.InnerException); }
            return BadRequest("Error saving the preview report");
    }
}
