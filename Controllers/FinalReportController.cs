

namespace surgical_reports.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class FinalReportController : ControllerBase
    {
       
private readonly IWebHostEnvironment _env;
        private IProcedureRepository _proc;
        private IManageFinalReport _impdf;


        public FinalReportController(
            IWebHostEnvironment env,
            IManageFinalReport impdf,

            IProcedureRepository proc)
        {
            _env = env;
            _impdf = impdf;
            _proc = proc;


        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
           return File(this.GetStream(id.ToString()), "application/pdf", $"{id}.pdf");
        }
        [AllowAnonymous]
        [HttpGet("getRefReport/{hash}")]
        public async Task<IActionResult> getPdfForRefPhys(string hash)
        {
            _impdf.DeleteExpiredReports(); // delete expired reports 
            var id = await _proc.getProcedureIdFromHash(hash);
            if (id == 0 || await _impdf.PdfDoesNotExists(id.ToString()))
            {
                return BadRequest("Your operative report is not found or expired ...");
            }
            return File(this.GetStream(id.ToString()), "application/pdf", $"{id}.pdf");
        }

        [AllowAnonymous]
        [HttpGet("deleteExpiredReports")]
        public IActionResult deleteExpiredReports()
        {
            var help = 0;
            help = _impdf.DeleteExpiredReports();
            if (help == 2) { return BadRequest(new { message = "Something went wrong in removing the expired reports" }); }
            return Ok("Success");
        }

        private Stream GetStream(string id_string)
        {
            var pathToFile = _env.ContentRootPath + "/assets/pdf/";
            var file_name = pathToFile + id_string + ".pdf";
            var stream = new FileStream(file_name, FileMode.Open, FileAccess.Read);
            stream.Position = 0;
            return stream;
        }
       

       
    }
