
using surgical_reports.helpers;

namespace surgical_reports.implementations;

public class PreviewReport : IPreviewReport
{
    private DapperContext _context;
    private IWebHostEnvironment _env;
    private IMapper _map;
    private IProcedureRepository _repo;
    private IInstitutionalText _text;
    public PreviewReport(
        IWebHostEnvironment env,
        DapperContext context,
        IInstitutionalText text,
        IProcedureRepository repo,
        IMapper map
       )
    {
        _context = context;
        _repo = repo;
        _map = map;
        _text = text;
        _env = env;
    }
    public async Task<Class_Preview_Operative_report> getPreViewAsync(int procedure_id)
    {
        if (await findPreview(procedure_id)) {
            
             return await getPreviewAsync(procedure_id);
             
              }
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
                var report_code = getReportCode(currentProcedure.fdType.ToString());
                if (report_code == "6")
                {
                    result.regel_1 = "Please enter your custom report here and 'Save as suggestion'";
                    return await saveNewPreviewReport(result);
                }
                else
                {
                    // check to see if there is an XML file for this hospital
                    await _text.addRecordInXML(currentProcedure.hospital.ToString());
                        
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

    public async Task<Class_Preview_Operative_report> resetPreViewAsync(int procedure_id)
    {
        var query = "DELETE FROM Previews WHERE procedure_id = @procedure_id";
        using (var connection = _context.CreateConnection()) { await connection.ExecuteAsync(query, new { procedure_id }); }
        return await getPreViewAsync(procedure_id);
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
    private async Task<Class_Preview_Operative_report> saveNewPreviewReport(Class_Preview_Operative_report cp)
    {
         var query = "INSERT INTO Previews (procedure_id,Regel_1, Regel_2, Regel_3, Regel_4, Regel_5, " +
        "Regel_6, Regel_7, Regel_8, Regel_9, Regel_10, " +
        "Regel_11, Regel_12, Regel_13, Regel_14, Regel_15, " +
        "Regel_16, Regel_17, Regel_18, Regel_19, Regel_20, " +
        "Regel_21, Regel_22, Regel_23, Regel_24, Regel_25, " +
        "Regel_26, Regel_27, Regel_28, Regel_29, Regel_30, " +
        "Regel_31, Regel_32, Regel_33 )" +
        "VALUES (@procedure_id, @Regel_1, @Regel_2, @Regel_3, @Regel_4, @Regel_5,  " +
        "@Regel_6, @Regel_7, @Regel_8, @Regel_9, @Regel_10, " +
        "@Regel_11, @Regel_12, @Regel_13, @Regel_14, @Regel_15, " +
        "@Regel_16, @Regel_17, @Regel_18, @Regel_19, @Regel_20, " +
        "@Regel_21, @Regel_22, @Regel_23, @Regel_24, @Regel_25, " +
        "@Regel_26, @Regel_27, @Regel_28, @Regel_29, @Regel_30, " +
        "@Regel_31, @Regel_32, @Regel_33);" + "SELECT LAST_INSERT_ID();";

        var parameters = new DynamicParameters();

        parameters.Add("procedure_id", cp.procedure_id, DbType.Int32);
        parameters.Add("Regel_1", cp.regel_1, DbType.String);
        parameters.Add("Regel_2", cp.regel_2, DbType.String);
        parameters.Add("Regel_3", cp.regel_3, DbType.String);
        parameters.Add("Regel_4", cp.regel_4, DbType.String);
        parameters.Add("Regel_5", cp.regel_5, DbType.String);
        parameters.Add("Regel_6", cp.regel_6, DbType.String);
        parameters.Add("Regel_7", cp.regel_7, DbType.String);
        parameters.Add("Regel_8", cp.regel_8, DbType.String);
        parameters.Add("Regel_9", cp.regel_9, DbType.String);
        parameters.Add("Regel_10", cp.regel_10, DbType.String);
       
        parameters.Add("Regel_11", cp.regel_11, DbType.String);
        parameters.Add("Regel_12", cp.regel_12, DbType.String);
        parameters.Add("Regel_13", cp.regel_13, DbType.String);
        parameters.Add("Regel_14", cp.regel_14, DbType.String);
        parameters.Add("Regel_15", cp.regel_15, DbType.String);
        parameters.Add("Regel_16", cp.regel_16, DbType.String);
        parameters.Add("Regel_17", cp.regel_17, DbType.String);
        parameters.Add("Regel_18", cp.regel_18, DbType.String);
        parameters.Add("Regel_19", cp.regel_19, DbType.String);
        parameters.Add("Regel_20", cp.regel_20, DbType.String);
       
        parameters.Add("Regel_21", cp.regel_21, DbType.String);
        parameters.Add("Regel_22", cp.regel_22, DbType.String);
        parameters.Add("Regel_23", cp.regel_23, DbType.String);
        parameters.Add("Regel_24", cp.regel_24, DbType.String);
        parameters.Add("Regel_25", cp.regel_25, DbType.String);
        parameters.Add("Regel_26", cp.regel_26, DbType.String);
        parameters.Add("Regel_27", cp.regel_27, DbType.String);
        parameters.Add("Regel_28", cp.regel_28, DbType.String);
        parameters.Add("Regel_29", cp.regel_29, DbType.String);
        parameters.Add("Regel_30", cp.regel_30, DbType.String);
      
        parameters.Add("Regel_31", cp.regel_31, DbType.String);
        parameters.Add("Regel_32", cp.regel_32, DbType.String);
        parameters.Add("Regel_33", cp.regel_33, DbType.String);

         using (var connection = _context.CreateConnection())
        {
            var id = await connection.QuerySingleAsync<int>(query, parameters);
            var createdPreviewReport = new Class_Preview_Operative_report
            {
                Id = id,
                procedure_id = cp.procedure_id,
                regel_1 = cp.regel_1,
                regel_2 = cp.regel_2,
                regel_3 = cp.regel_3,
                regel_4 = cp.regel_4,
                regel_5 = cp.regel_5,
                regel_6 = cp.regel_6,
                regel_7 = cp.regel_7,
                regel_8 = cp.regel_8,
                regel_9 = cp.regel_9,
                regel_10 = cp.regel_10,
                regel_11 = cp.regel_11,
                regel_12 = cp.regel_12,
                regel_13 = cp.regel_13,
                regel_14 = cp.regel_14,
                regel_15 = cp.regel_15,
                regel_16 = cp.regel_16,
                regel_17 = cp.regel_17,
                regel_18 = cp.regel_18,
                regel_19 = cp.regel_19,
                regel_20 = cp.regel_20,
                regel_21 = cp.regel_21,
                regel_22 = cp.regel_22,
                regel_23 = cp.regel_23,
                regel_24 = cp.regel_24,
                regel_25 = cp.regel_25,
                regel_26 = cp.regel_26,
                regel_27 = cp.regel_27,
                regel_28 = cp.regel_28,
                regel_29 = cp.regel_29,
                regel_30 = cp.regel_30,
                regel_31 = cp.regel_31,
                regel_32 = cp.regel_32,
                regel_33 = cp.regel_33
              
                
            };
            return createdPreviewReport;
        }
    
    }
    public async Task<int> updatePVR(Class_Preview_Operative_report pv)
    {
        var query = "UPDATE Previews SET Regel_1 = @Regel_1, Regel_2 = @Regel_2 " + 
        "Regel_3 = @Regel_3, Regel_4 = @Regel_4, Regel_5 = @Regel_5, " +
        "Regel_6 = @Regel_6, Regel_7 = @Regel_7, Regel_8 = @Regel_8, " +
        "Regel_9 = @Regel_9, Regel_10 = @Regel_10, Regel_11 = @Regel_11, " +
        "Regel_12 = @Regel_12, Regel_13 = @Regel_13, Regel_14 = @Regel_14, " +
        "Regel_15 = @Regel_15, Regel_16 = @Regel_16, Regel_17 = @Regel_17, " +
        "Regel_18 = @Regel_18, Regel_19 = @Regel_19, Regel_20 = @Regel_20, " +
        "Regel_21 = @Regel_21, Regel_22 = @Regel_22, Regel_23 = @Regel_23, " +
        "Regel_24 = @Regel_24, Regel_25 = @Regel_25, Regel_26 = @Regel_26, " +
        "Regel_27 = @Regel_27, Regel_28 = @Regel_28, Regel_29 = @Regel_29, " +
        "Regel_30 = @Regel_30, Regel_31 = @Regel_31, Regel_32 = @Regel_32, " +
        "Regel_33 = @Regel_33 WHERE procedure_id = @procedure_id";
        var parameters = new DynamicParameters();
        parameters.Add("procedure_id", pv.procedure_id, DbType.Int32);
        parameters.Add("Regel_1", pv.regel_1, DbType.String);
        parameters.Add("Regel_2", pv.regel_2, DbType.String);
        parameters.Add("Regel_3", pv.regel_3, DbType.String);
        parameters.Add("Regel_4", pv.regel_4, DbType.String);
        parameters.Add("Regel_5", pv.regel_5, DbType.String);
        parameters.Add("Regel_6", pv.regel_6, DbType.String);  
        parameters.Add("Regel_7", pv.regel_7, DbType.String);
        parameters.Add("Regel_8", pv.regel_8, DbType.String);
        parameters.Add("Regel_9", pv.regel_9, DbType.String);
        parameters.Add("Regel_10", pv.regel_10, DbType.String);  
        parameters.Add("Regel_11", pv.regel_11, DbType.String);
        parameters.Add("Regel_12", pv.regel_12, DbType.String);
        parameters.Add("Regel_13", pv.regel_13, DbType.String);
        parameters.Add("Regel_14", pv.regel_14, DbType.String);
        parameters.Add("Regel_15", pv.regel_15, DbType.String);
        parameters.Add("Regel_16", pv.regel_16, DbType.String);  
        parameters.Add("Regel_17", pv.regel_17, DbType.String);
        parameters.Add("Regel_18", pv.regel_18, DbType.String);
        parameters.Add("Regel_19", pv.regel_19, DbType.String);
        parameters.Add("Regel_20", pv.regel_20, DbType.String);  
        parameters.Add("Regel_21", pv.regel_21, DbType.String);
        parameters.Add("Regel_22", pv.regel_22, DbType.String);
        parameters.Add("Regel_23", pv.regel_23, DbType.String);
        parameters.Add("Regel_24", pv.regel_24, DbType.String);
        parameters.Add("Regel_25", pv.regel_25, DbType.String);
        parameters.Add("Regel_26", pv.regel_26, DbType.String);  
        parameters.Add("Regel_27", pv.regel_27, DbType.String);
        parameters.Add("Regel_28", pv.regel_28, DbType.String);
        parameters.Add("Regel_29", pv.regel_29, DbType.String);
        parameters.Add("Regel_30", pv.regel_30, DbType.String);  
        parameters.Add("Regel_31", pv.regel_31, DbType.String);
        parameters.Add("Regel_32", pv.regel_32, DbType.String);
        parameters.Add("Regel_33", pv.regel_33, DbType.String);
       
        using(var connection = _context.CreateConnection()) { await connection.ExecuteAsync(query, parameters); }

        return 1;
    }
   
    public string getReportCode(string fdType)
    {
            var result = "";
            var contentRoot = _env.ContentRootPath;
            var filename = Path.Combine(contentRoot, "xml/procedure.xml");
            XDocument order = XDocument.Load(filename);
            IEnumerable<XElement> help = from d in order.Descendants("Code")
                                         where d.Element("ID").Value == fdType.ToString()
                                         select d;
            foreach (XElement x in help) { result = x.Element("report_code").Value; }
            return result;
        }
}
