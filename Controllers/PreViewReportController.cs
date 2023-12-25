using Microsoft.AspNetCore.Http.HttpResults;

namespace surgical_reports.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PreViewReportController : ControllerBase
{

    private IPreviewReport _repo;
    private IOperativeReport _iorep;
    private IProcedureRepository _proc;

    private IManageFinalReport _final;
    private IMapper _map;
    private reportMapper _rm;


    public PreViewReportController(IManageFinalReport final,
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
        _final = final;
    }

    [HttpGet("{id}", Name = "getPreviewReport")]
    public async Task<IActionResult> getReport(int id) { return Ok(await _repo.getPreViewAsync(id)); }
   
    [HttpGet("reset/{id}")]
    public async Task<IActionResult> resetReport(int id) { var help = await _repo.resetPreViewAsync(id); return Ok(help); }

     [HttpGet("getReportHeader/{procedure_id}")]
    public async Task<IActionResult> reportHeader(int procedure_id) { var help = await _repo.getReportHeaderAsync(procedure_id); return Ok(help); }
  
    [HttpPost]
    // this comes from the save and print button and results in a pdf
    public async Task<IActionResult> Post(PreviewForReturnDTO pvfr)
    {
        try
        {
            Class_privacy_model pm = _map.Map<PreviewForReturnDTO, Class_privacy_model>(pvfr);
            Class_Preview_Operative_report pv = new Class_Preview_Operative_report();

            if (await _repo.findPreview(pvfr.procedure_id)) {
                 pv = await _repo.getSpecificPVR(pvfr.procedure_id); }
            else { return BadRequest("No preview found in the database ..."); }

            pv = _map.Map<PreviewForReturnDTO, Class_Preview_Operative_report>(pvfr, pv);
            // save the Class_Preview_Operative_report to the database first
            var result = await _repo.updatePVR(pv);

            // generate final operative report
            Class_Procedure cp = await _proc.getSpecificProcedure(pv.procedure_id);
            var type_operatie = cp.fdType;
            var help = _repo.getReportCode(cp.fdType.ToString());
            var report_code = Convert.ToInt32(help);
            Class_Final_operative_report classFR = await _rm.updateFinalReportAsync(pm, cp, report_code);


            try
            {
                // generate PDF and store for 3 days
                await _iorep.getPdf(report_code, type_operatie, classFR);
                // add this pdf file in to the list of reportTimings

                ReportTiming rt = new ReportTiming();
                rt.publishTime = DateTime.UtcNow;
                rt.id = pv.procedure_id;
                rt.fileLocation = "";

                // save this to the xml file timingsRefReport.xml
                _final.AddToExpiredReports(rt);
                
            }
            catch (Exception a) { Console.WriteLine(a.InnerException); return BadRequest("Error creating the pdf"); }
            return Ok(result);
        }
        catch (Exception e) { Console.WriteLine(e.InnerException); }
        return BadRequest("Error saving the preview report");
    }







}
