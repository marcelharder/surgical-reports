
using surgical_reports.helpers;

namespace surgical_reports.implementations;

public class PreviewReport : IPreviewReport
{
    private List<Class_Item> dropLeg = new List<Class_Item>();
    private DapperContext _context;
    private IWebHostEnvironment _env;
    private IMapper _map;
    private IProcedureRepository _repo;
    private IInstitutionalText _text;
    private IProcedureRepository _proc;
    private ICPBRepo _icpb;
    private ICABGRepo _cabg;
    public PreviewReport(
        IProcedureRepository proc,
        ICPBRepo icpb,
        ICABGRepo cabg,
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
        _proc = proc;
        _icpb = icpb;
        _cabg = cabg;
    }
    public async Task<Class_Preview_Operative_report> getPreViewAsync(int procedure_id)
    {
        if (await findPreview(procedure_id)) {return await getPRA(procedure_id);}
        else
        {
            //add a new preview instance to database
            var result = new Class_Preview_Operative_report();
            result.procedure_id = procedure_id;

            var currentProcedure = await _repo.getSpecificProcedure(procedure_id);
            var user_id = currentProcedure.SelectedSurgeon;
            var hospitalNo = currentProcedure.hospital;

            // look for userspecificreport
            if (await UserHasASuggestionForThisProcedure(user_id, currentProcedure.fdType))
            {
                var usersuggestion = await getUserSpecificSuggestion(user_id, currentProcedure.fdType);

                result = _map.Map<Class_Suggestion, Class_Preview_Operative_report>(usersuggestion);
                result.procedure_id = procedure_id;
                return await saveNewPreviewReport(result, hospitalNo);
            }
            else
            // get the generic preview from the hospital
            {
                //var report_code = getReportCode(currentProcedure.fdType.ToString());
                if (currentProcedure.fdType == 6)
                {
                    result.regel_1 = "This procedure is not (yet) available for reporting";
                    return await saveNewPreviewReport(result, hospitalNo);
                }
                else
                {
                    // check to see if there is an XML file for this hospital, if no record available than add one now.
                    await _text.addRecordInXML(currentProcedure.hospital.ToString());
                    // get the description of this procedure from the 'soort'
                    var description = getDescription(currentProcedure.fdType.ToString());
                   
                    // get the suggestion from the InstitutionalReports.xml
                    var text = await _text.getInstitutionalReport(currentProcedure.hospital.ToString(), currentProcedure.fdType.ToString(), description);
                    var no = _map.Map<InstitutionalDTO, Class_Preview_Operative_report>(text);
                    no.procedure_id = currentProcedure.ProcedureId;

                    return await saveNewPreviewReport(no, hospitalNo);
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
    private string getDescription(string test)
    {
        var result = "";
        var contentRoot = _env.ContentRootPath;
        var filename = Path.Combine(contentRoot, "xml/procedure.xml");
        XDocument order = XDocument.Load(filename);
        IEnumerable<XElement> help = from d in order.Descendants("Code")
                                     where d.Element("ID").Value == test
                                     select d;
        foreach (XElement x in help) { result = x.Element("Description").Value; }
        return result;
    }
    private async Task<Class_Suggestion> getUserSpecificSuggestion(int user_id, int soort)
    {
        var query = "SELECT * FROM Suggestions WHERE user = @user_id AND soort = @soort";
        using (var connection = _context.CreateConnection())
        {
            var preview = await connection.QuerySingleOrDefaultAsync<Class_Suggestion>(query, new { user_id, soort });
            return preview;
        }
    }
    public async Task<bool> findPreview(int procedure_id)
    {
        var query = "SELECT * FROM Previews WHERE procedure_id = @procedure_id";
        using (var connection = _context.CreateConnection())
        {
            var preview = await connection.QuerySingleOrDefaultAsync<Class_Preview_Operative_report>(query, new { procedure_id });
            return preview != null;
        }
    }
    private async Task<Class_Preview_Operative_report> getPRA(int procedure_id)
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
        var query = "SELECT * FROM Suggestions WHERE user = @user_id AND soort = @soort";
        using (var connection = _context.CreateConnection())
        {
            var preview = await connection.QuerySingleOrDefaultAsync<Class_Suggestion>(query, new { user_id, soort });
            return preview != null;
        }
    }
    private async Task<Class_Preview_Operative_report> saveNewPreviewReport(Class_Preview_Operative_report cp, int hospitalNo)
    {
       // add the additional things
       var currentFullHospital = await _text.getCurrentHospital(hospitalNo.ToString()); // gives a XElement from the selected hospital
       InstitutionalDTO text = new InstitutionalDTO();
       
       text.Regel1C = await translateHarvestLocationLeg(cp.procedure_id, dropLeg);
       text.Regel2C = await translateHarvestLocationRadial(cp.procedure_id, dropLeg);
       text.Regel6B = text.Regel6B + "" + await getCardioPlegiaTemp(cp.procedure_id) + "" + await getCardioPlegiaRoute(cp.procedure_id) + "" + await getCardioPlegiaType(cp.procedure_id);
                    
       text.Regel21 = await getCirculationSupportAsync(cp.procedure_id, currentFullHospital);
       text.Regel22 = await getIABPUsedAsync(cp.procedure_id, currentFullHospital);
       text.Regel23 = await getPMWiresAsync(cp.procedure_id, currentFullHospital);

       // merge the InstitutionalDTO straight to the Class_Preview_Operative_report
        cp = _map.Map<InstitutionalDTO, Class_Preview_Operative_report>(text,cp);



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
        var query = "UPDATE Previews SET Regel_1 = @Regel_1, Regel_2 = @Regel_2, " +
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

        using (var connection = _context.CreateConnection()) { await connection.ExecuteAsync(query, parameters); }

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
    public async Task<Class_Preview_Operative_report> getSpecificPVR(int procedure_id)
    {
        var query = "SELECT * FROM Previews WHERE procedure_id = @procedure_id";
        using (var connection = _context.CreateConnection())
        {
            var preview = await connection.QuerySingleOrDefaultAsync<Class_Preview_Operative_report>(query, new { procedure_id });
            return preview;
        }
    }

    private async Task<string> translateHarvestLocationLeg(int procedure_id, List<Class_Item> dropLeg)
    {
        var help = "";
        var cabg = await _cabg.getSpecificCABG(procedure_id);

        if (cabg != null && cabg.leg_harvest_location == "")
        {
            var test = Convert.ToInt32(cabg.leg_harvest_location);
            var ci = dropLeg.Single(x => x.value == test);
            help = ci.description;
        }

        return help;
    }
    private async Task<string> translateHarvestLocationRadial(int procedure_id, List<Class_Item> dropRadial)
    {
        var help = "";
        var cabg = await _cabg.getSpecificCABG(procedure_id);
        if (cabg != null && cabg.radial_harvest_location == "")
        {
            var test = Convert.ToInt32(cabg.radial_harvest_location);
            var ci = dropRadial.Single(x => x.value == test);
            help = ci.description;
        }

        return help;
    }
    private async Task<string> getCardioPlegiaTemp(int procedure_id)
    {
        var help = "";
        Class_CPB cpb = await _icpb.getSpecificCPB(procedure_id);
        if (cpb != null) { }
        return help;
    }
    private async Task<string> getCardioPlegiaRoute(int procedure_id)
    {
        var help = "";
        Class_CPB cpb = await _icpb.getSpecificCPB(procedure_id);
        if (cpb != null) { }
        return help;
    }
    private async Task<string> getCardioPlegiaType(int procedure_id)
    {
        var help = "";
        Class_CPB cpb = await _icpb.getSpecificCPB(procedure_id);
        if (cpb != null) { }
        return help;
    }
    private async Task<string> getProcedureDescriptionAsync(int procedure_id)
    {
        var r = await _proc.getSpecificProcedure(procedure_id);
        return r.Description;
    }
    private async Task<string> getCirculationSupportAsync(int procedure_id, IEnumerable<XElement> test)
    {
        var help = "";
        var selectedProcedure = await _proc.getSpecificProcedure(procedure_id);
        if (selectedProcedure != null)
        {
            var t = selectedProcedure.SelectedInotropes; // dit is de gekozen inotropische ondersteuning
            foreach (XElement el in test)// dit is het correcte ziekenhuis, dus ook de juiste taal
            {
                IEnumerable<XElement> te = from tr in test.Descendants("reports").Elements("circulation_support").Elements("items")
                                           where (string)tr.Attribute("id") == t.ToString()
                                           select tr;
                foreach (XElement f in te) { help = f.Element("regel_21").Value; }
            }
        }
        return help;
    }
    private async Task<string> getPMWiresAsync(int procedure_id, IEnumerable<XElement> test)
    {
        var help = "";
        var selectedProcedure = await _proc.getSpecificProcedure(procedure_id);
        if (selectedProcedure != null)
        {
            var t = selectedProcedure.SelectedInotropes; // dit is de gekozen inotropische ondersteuning
            foreach (XElement el in test)// dit is het correcte ziekenhuis, dus ook de juiste taal
            {
                IEnumerable<XElement> te = from tr in test.Descendants("reports").Elements("pmwires").Elements("items")
                                           where (string)tr.Attribute("id") == t.ToString()
                                           select tr;
                foreach (XElement f in te) { help = f.Element("regel_23").Value; }
            }
        }
        return help;
    }
    private async Task<string> getIABPUsedAsync(int procedure_id, IEnumerable<XElement> test)
    {
        var help = "";
        var selectedCPB = await _icpb.getSpecificCPB(procedure_id);
        if (selectedCPB != null)
        {
            var t = selectedCPB.IABP_IND; // dit is de gekozen indicatie voor de IABP
            foreach (XElement el in test)// dit is het correcte ziekenhuis, dus ook de juiste taal
            {
                IEnumerable<XElement> te = from tr in test.Descendants("reports").Elements("iabp").Elements("items")
                                           where (string)tr.Attribute("id") == t.ToString()
                                           select tr;
                foreach (XElement f in te) { help = f.Element("regel_22").Value; }
            }
        }
        return help;
    }
  
}
