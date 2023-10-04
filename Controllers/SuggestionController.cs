namespace surgical_reports.Controllers;

[ApiController]
[Route("api/[controller]")]

public class SuggestionController : ControllerBase
{
    private ISuggestion _repo;
    private IPreviewReport _previewReport;

   
    public SuggestionController(ISuggestion repo, IPreviewReport previewReport)
    {
        _repo = repo;
        _previewReport = previewReport;
    }

    [HttpGet("{id}")] // get all recorded suggestions for this user as class_items
    public async Task<IActionResult> Get(string id)
    {
        var p = await _repo.GetAllIndividualSuggestions(id);
        return Ok(p);
    }

    [HttpGet("{id}/{soort}", Name = "GetSuggestion")] // gets recorded suggestion for this user by the soort
    public async Task<IActionResult> GetA(string id ,int soort)
    {
        var p = await _repo.GetIndividualSuggestion(soort,id);

        return Ok(p);
    }

    [HttpPut("{userId}/{soort}")]
    public async Task<IActionResult> Put(Class_Preview_Operative_report cp, int soort, string userId)
    {
        // Save the preview report first
        int pvr_result = await _previewReport.updatePVR(cp);

        // get the current suggestion, if not available a new one is generated for this user and soort                     
        var current_suggestion = await _repo.GetIndividualSuggestion(soort,userId);

        Class_Suggestion c = await _repo.mapToSuggestionFromPreview(current_suggestion, cp);

        var result = await _repo.updateSuggestion(c);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Class_Suggestion c)
    {
        var p = await _repo.AddIndividualSuggestion(c);
        if(p != null){return Ok(p);}
        else { throw new Exception($"Adding suggestion {c.user} failed on save"); };
    }

}



