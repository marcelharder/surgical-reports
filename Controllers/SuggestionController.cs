using System.Runtime.CompilerServices;

namespace surgical_reports.Controllers;

[ApiController]
[Route("api/[controller]")]

public class SuggestionController : ControllerBase
{
    private ISuggestion _repo;
    private IMapper _map;
    private IPreviewReport _previewReport;
    private IProcedureRepository _proc;


    public SuggestionController(ISuggestion repo, IPreviewReport previewReport, IProcedureRepository proc, IMapper map)
    {
        _repo = repo;
        _previewReport = previewReport;
        _proc = proc;
        _map = map;
    }

    [HttpGet("{id}")] // get all recorded suggestions for this user as class_items
    public async Task<IActionResult> Get(string id)
    {
        var p = await _repo.GetAllIndividualSuggestions(id);
        return Ok(p);
    }

    [HttpGet("{id}/{soort}", Name = "GetSuggestion")] // gets recorded suggestion for this user by the soort
    public async Task<IActionResult> GetA(string id, int soort)
    {
        var p = await _repo.GetIndividualSuggestion(soort, id);
        return Ok(p);
    }

    [HttpPut]
    public async Task<IActionResult> Put(Class_Preview_Operative_report cp)
    {
        // Save the preview report first
        int pvr_result = await _previewReport.updatePVR(cp);


        var currentProcedure = await _proc.getSpecificProcedure(cp.procedure_id);
        // get the currentuser from cp
        var currentUser = currentProcedure.SelectedSurgeon;
        // get the soort from fdType from cp
        var soort = currentProcedure.fdType;
        // get the current suggestion, if not available a new one is generated for this user and soort                     
        var current_suggestion = await _repo.GetIndividualSuggestion(soort, currentUser.ToString());

        // Class_Suggestion c = await _repo.mapToSuggestionFromPreview(current_suggestion, cp);

        Class_Suggestion c = _map.Map<Class_Preview_Operative_report, Class_Suggestion>(cp, current_suggestion);
        c.soort = soort;
        c.user = currentUser.ToString();


        var result = await _repo.updateSuggestion(c);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Class_Suggestion c)
    {
        var p = new Class_Suggestion();
        if (await _repo.GetIndividualSuggestion(c.soort, c.user) == null)
        {
            p = await _repo.AddIndividualSuggestion(c);
        }

        return Ok(p);
    }

}



