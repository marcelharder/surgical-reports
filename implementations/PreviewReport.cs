
using surgical_reports.helpers;

namespace surgical_reports.implementations;

public class PreviewReport : IPreviewReport
{
    private DapperContext _context;
    private reportMapper _reportM;
    private IMapper _map;
    private IProcedureRepository _repo;
    private IInstitutionalText _text;
    public PreviewReport(
        DapperContext context,
        IInstitutionalText text,
        IProcedureRepository repo,
        IMapper map,
        reportMapper reportM)
    {
        _context = context;
        _repo = repo;
        _map = map;
        _reportM = reportM;
        _text = text;
    }
    public async Task<Class_Preview_Operative_report> getPreViewAsync(int procedure_id)
    {
        if (await findPreview(procedure_id))
        {
            return await getPreViewAsync(procedure_id);
        }
        else
        {
            //add preview to database
            var result = new Class_Preview_Operative_report();
            result.procedure_id = procedure_id;
            var currentProcedure = await _repo.getSpecificProcedure(procedure_id);
            var user_id = currentProcedure.SelectedSurgeon;

            // look for userspecificreport
            if (await UserHasASuggestionForThisProcedure(user_id, currentProcedure.fdType))
            {
                var usersuggestion = await getUserSpecificSuggestion(user_id, currentProcedure.fdType);
                result = _map.Map<Class_Suggestion, Class_Preview_Operative_report>(usersuggestion);
                result.procedure_id = procedure_id;
                return await saveNewPreviewReport(result);
            }
            else
            // get the generic preview from the hospital
            {
                var report_code = _reportM.getReportCode(currentProcedure.fdType);
                if (report_code == "6")
                {
                    result.regel_1 = "Please enter your custom report here and 'Save as suggestion'";
                    return await saveNewPreviewReport(result);
                }
                else
                {
                    // get the suggestion from the InstitutionalReports.xml
                    var text = await _text.getText(currentProcedure.hospital, currentProcedure.fdType);
                }

            }
        }
    }

    public Task<Class_Preview_Operative_report> resetPreViewAsync(int procedure_id)
    {
        throw new NotImplementedException();
    }

    public Task<int> updatePVR(Class_Preview_Operative_report cp)
    {
        throw new NotImplementedException();
    }

    private async Task<Class_Suggestion> getUserSpecificSuggestion(int user_id, int soort)
    {
        var query = "SELECT * FROM Suggestions WHERE user = @user_id AND soort = @soort";
        using (var connection = _context.CreateConnection())
        {
            var preview = await connection.QuerySingleOrDefaultAsync<Class_Suggestion>(query, new { user_id });
            return preview;
        }
    }
    private Task<Class_Preview_Operative_report> getHospitalSuggestion(int procedure_id)
    {
        throw new NotImplementedException();
    }
    private async Task<bool> findPreview(int procedure_id)
    {
        var query = "SELECT * FROM Previews WHERE procedure_id = @procedure_id";
        using (var connection = _context.CreateConnection())
        {
            var preview = await connection.QuerySingleOrDefaultAsync<Class_Preview_Operative_report>(query, new { procedure_id });
            return preview != null;
        }
    }
    private async Task<Class_Preview_Operative_report> getPreviewAsync(int procedure_id)
    {
        var query = "SELECT * FROM Previews WHERE procedure_id = @procedure_id";
        using (var connection = _context.CreateConnection())
        {
            var preview = await connection.QuerySingleOrDefaultAsync<Class_Preview_Operative_report>(query, new { procedure_id });
            return preview;
        }
    }
    private async Task<bool> UserHasASuggestionForThisProcedure(int user_id, int soort)
    {
        var query = "SELECT * FROM Suggestions WHERE user = @user_id AND soort = @fdType";
        using (var connection = _context.CreateConnection())
        {
            var preview = await connection.QuerySingleOrDefaultAsync<Class_Suggestion>(query, new { user_id });
            return preview != null;
        }
    }
    private Task<Class_Preview_Operative_report> saveNewPreviewReport(Class_Preview_Operative_report cp)
    {
        throw new NotImplementedException();
    }
}
