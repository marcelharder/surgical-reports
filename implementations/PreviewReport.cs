
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
        if (await findPreview(procedure_id)) {return await getPreViewAsync(procedure_id);}
        else
        {
            //add a new preview instance to database
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
                    var text = await _text.getText(currentProcedure.hospital.ToString(), currentProcedure.fdType.ToString(), currentProcedure.ProcedureId);
                    result.regel_1 = text[0]; result.regel_2 = text[1]; result.regel_3 = text[2]; result.regel_4 = text[3]; result.regel_5 = text[4];
                    result.regel_6 = text[5]; result.regel_7 = text[6]; result.regel_8 = text[7]; result.regel_9 = text[8]; result.regel_10 = text[9];
                    result.regel_11 = text[10]; result.regel_12 = text[11]; result.regel_13 = text[12]; result.regel_14 = text[13]; result.regel_15 = text[14];
                    result.regel_16 = text[15]; result.regel_17 = text[16]; result.regel_18 = text[17]; result.regel_19 = text[18]; result.regel_20 = text[19];
                    result.regel_21 = text[20]; result.regel_22 = text[21]; result.regel_23 = text[22]; result.regel_24 = text[23]; result.regel_25 = text[24];
                    result.regel_26 = text[25]; result.regel_27 = text[26]; result.regel_28 = text[27]; result.regel_29 = text[28]; result.regel_30 = text[29];
                    result.regel_31 = text[30]; result.regel_32 = text[31]; result.regel_33 = text[32];
                    return await saveNewPreviewReport(result);
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
