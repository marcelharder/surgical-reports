
using System.Collections;
using surgical_reports.helpers;

namespace surgical_reports.implementations;

public class PreviewReport : IPreviewReport
{
    private DapperContext _context;
    private IWebHostEnvironment _env;
    private IMapper _map;
    private IInstitutionalText _text;
    private IProcedureRepository _proc;
    private IHospitalRepository _hos;
    private ICPBRepo _icpb;
    private ICABGRepo _cabg;
    private OperatieDrops _drops;

    private string _currentLanguage;
    public PreviewReport(
        OperatieDrops drops,
        IProcedureRepository proc,
        IHospitalRepository hos,
        ICPBRepo icpb,
        ICABGRepo cabg,
        IWebHostEnvironment env,
        DapperContext context,
        IInstitutionalText text,
        IMapper map
       )
    {
        _context = context;
        _map = map;
        _text = text;
        _env = env;
        _proc = proc;
        _icpb = icpb;
        _cabg = cabg;
        _env = env;
        _drops = drops;
        _hos = hos;
        _currentLanguage = "";

    }
    public async Task<Class_Preview_Operative_report> getPreViewAsync(int procedure_id)
    {
        // get the current language
        var currentProcedure = await _proc.getSpecificProcedure(procedure_id);
        var currentHospital = currentProcedure.hospital;
        var hospital = await _hos.GetSpecificHospital(currentHospital.ToString());
        _currentLanguage = hospital.country;

        // check if the procedure exists
        if (await _proc.getSpecificProcedure(procedure_id) != null)
        {
            if (await findPreview(procedure_id)) { return await getPRA(procedure_id); }
            else //add a new preview instance to database
            {   var result = new Class_Preview_Operative_report();
                result.procedure_id = procedure_id;
                var user_id = currentProcedure.SelectedSurgeon;
                if (await UserHasASuggestionForThisProcedure(user_id, currentProcedure.fdType))// look for userspecificreport
                {
                    var usersuggestion = await getUserSpecificSuggestion(user_id, currentProcedure.fdType);
                    result = _map.Map<Class_Suggestion, Class_Preview_Operative_report>(usersuggestion);
                    result.procedure_id = procedure_id;
                    
                    var text = await getStatic_Text_Per_Country(currentProcedure);
                    result.regel_21 = text[0];
                    result.regel_22 = text[1];
                    result.regel_23 = text[2];
                    return await saveNewPreviewReport(result);
                }
                else
                // get the generic preview from the hospital
                {
                    //var report_code = getReportCode(currentProcedure.fdType.ToString());
                    if (currentProcedure.fdType == 6)
                    {
                        result.regel_1 = "This procedure is not (yet) available for reporting";
                        
                        var text = await getStatic_Text_Per_Country(currentProcedure);
                        result.regel_21 = text[0];
                        result.regel_22 = text[1];
                        result.regel_23 = text[2];
                        return await saveNewPreviewReport(result);
                    }
                    else
                    {
                        // check to see if there is an XML file for this hospital, if no record available than add one now.
                        await _text.addRecordInXML(currentProcedure.hospital.ToString());
                        // get the description of this procedure from the 'soort'
                        var description = getDescription(currentProcedure.fdType.ToString());

                        // get the suggestion from the InstitutionalReports.xml
                        var t = await _text.getInstitutionalReport(currentProcedure.hospital.ToString(), currentProcedure.fdType.ToString(), description);
                        var no = _map.Map<InstitutionalDTO, Class_Preview_Operative_report>(t);
                        no.procedure_id = currentProcedure.ProcedureId;
                       
                        var text = await getStatic_Text_Per_Country(currentProcedure);
                        no.regel_21 = text[0];
                        no.regel_22 = text[1];
                        no.regel_23 = text[2];
                        return await saveNewPreviewReport(no);
                    }
                }
            }
        }
        else { return null; }
    }

    private async Task<List<string>> getStatic_Text_Per_Country(Class_Procedure currentProcedure)
    {
        // this gives a language specific text if the general details are filled
        List<string> static_text = new List<string>();
        List<string> help;
        var iabp_result = "";
        var cpb = await _icpb.getSpecificCPB(currentProcedure.ProcedureId);

        if (cpb != null) { iabp_result = await getIABPStuff(cpb); }
        switch (_currentLanguage)
        {
            case "IT":help = await _drops.getGeneralText("IT");static_text = getStatic(help, currentProcedure, iabp_result);break;
            case "NL":help = await _drops.getGeneralText("NL");static_text = getStatic(help, currentProcedure, iabp_result);break;
            case "GB":help = await _drops.getGeneralText("GB");static_text = getStatic(help, currentProcedure, iabp_result);break;
            case "SA":help = await _drops.getGeneralText("SA");static_text = getStatic(help, currentProcedure, iabp_result);break;
            case "DE":help = await _drops.getGeneralText("DE");static_text = getStatic(help, currentProcedure, iabp_result);break;
            case "US":help = await _drops.getGeneralText("US");static_text = getStatic(help, currentProcedure, iabp_result);break;
            default:  help = await _drops.getGeneralText("GB");static_text = getStatic(help, currentProcedure, iabp_result);break;
         }
          return static_text;
    }

    private List<string> getStatic(List<string> help, Class_Procedure currentProcedure, string iabp_result){
        var r = new List<string>();
         if (currentProcedure.SelectedInotropes != 0) { r.Add(help[0]); } else r.Add("");
         if (iabp_result != "")                       { r.Add(help[1]); } else r.Add("");
         if (currentProcedure.SelectedPacemaker != 0) { r.Add(help[2]); } else r.Add("");
     return r;
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
    private async Task<Class_Preview_Operative_report> addProcedureDetails(Class_Preview_Operative_report cp)
    {
        InstitutionalDTO text = new InstitutionalDTO();
        var currentProcedure = await _proc.getSpecificProcedure(cp.procedure_id);
        var currentHospital = currentProcedure.hospital.ToString().makeSureTwoChar();
        var hos = await _hos.GetSpecificHospital(currentHospital);
        var currentlanguage = hos.country;
        var currentSoort = currentProcedure.fdType.ToString();




        if (currentProcedure.fdType == 1) // doe dit eerst voor de CABG Lima/VSM
        {
            text = await _text.getInstitutionalReport(currentHospital, currentSoort, "");
            cp.regel_21 = cp.regel_21 + await getCirculationSupportAsync(currentProcedure);
            cp.regel_23 = cp.regel_23 + await getPMWiresAsync(currentProcedure);

            var cabg = await _cabg.getSpecificCABG(cp.procedure_id);
            if (cabg != null)
            {
                cp.regel_1 = text.Regel1A + text.Regel1B + " " + await translateHarvestLocationLeg(cabg) + " " + text.Regel1C;
                // cp.regel_2 = text.Regel2A + text.Regel2B + " " + await translateHarvestLocationRadial(cabg) + " " + text.Regel2C;
            }
            var cpb = await _icpb.getSpecificCPB(cp.procedure_id);
            if (cpb != null)
            {
                cp.regel_5 = text.Regel5A + " " + cpb.LOWEST_CORE_TEMP + " " + text.Regel5C;
                cp.regel_22 = cp.regel_22 + await getIABPStuff(cpb);

                cp.regel_6 = await getCardioPlegiaTemp(cpb) + " " + await getCardioPlegiaRoute(cpb) + " " + await getCardioPlegiaType(cpb) + " with " + text.Regel6A + " of " + cpb.INFUSION_DOSE_INT + " ml";
            }
        }
        else
        {
            cp.regel_21 = cp.regel_21 + await getCirculationSupportAsync(currentProcedure);
            cp.regel_23 = cp.regel_23 + await getPMWiresAsync(currentProcedure);
            var cpb1 = await _icpb.getSpecificCPB(cp.procedure_id);
            if (cpb1 != null)
            {
                // now get the details of the IABP
                cp.regel_22 = cp.regel_22 + await getIABPStuff(cpb1);
            }
        }
        return cp;
    }
    private async Task<Class_Preview_Operative_report> saveNewPreviewReport(Class_Preview_Operative_report cp)
    {
        cp = await addProcedureDetails(cp);
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
    private async Task<string> translateHarvestLocationLeg(Class_CABG cabg)
    {
        var help = "";
        List<Class_Item> dropLeg = new List<Class_Item>();
        dropLeg = await _drops.getCABGLeg(_currentLanguage);

        if (cabg.leg_harvest_location != null)
        {
            var test = Convert.ToInt32(cabg.leg_harvest_location);
            var ci = dropLeg.Single(x => x.value == test);
            help = ci.description;
        }

        return help;
    }
    private async Task<string> translateHarvestLocationRadial(Class_CABG cabg)
    {
        var help = "";
        List<Class_Item> dropRadial = new List<Class_Item>();
        dropRadial = await _drops.getCABGRadial(_currentLanguage);

        if (cabg.radial_harvest_location != null)
        {
            var test = Convert.ToInt32(cabg.radial_harvest_location);
            var ci = dropRadial.Single(x => x.value == test);
            help = ci.description;
        }

        return help;
    }
    private async Task<string> getCardioPlegiaTemp(Class_CPB cpb)
    {
        //temp
        var help = "";
        List<Class_Item> dropTemp = new List<Class_Item>();
        dropTemp = await _drops.getCPB_temp(_currentLanguage);
        if (cpb.cardiopl_temp != null)
        {
            var test = Convert.ToInt32(cpb.cardiopl_temp);
            var ci = dropTemp.Single(x => x.value == test);
            help = ci.description;
        }

        return help;
    }
    private async Task<string> getCardioPlegiaRoute(Class_CPB cpb)
    {
        // delivery
        var help = "";
        List<Class_Item> dropDelivery = new List<Class_Item>();
        dropDelivery = await _drops.getCPB_delivery(_currentLanguage);
        if (cpb.INFUSION_MODE_ANTE != null)
        {
            var test = Convert.ToInt32(cpb.INFUSION_MODE_ANTE);
            var ci = dropDelivery.Single(x => x.value == test);
            help = ci.description;
        }
        return help;
    }
    private async Task<string> getCardioPlegiaType(Class_CPB cpb)
    {
        // typeCardiopleg
        var help = "";
        List<Class_Item> dropType = new List<Class_Item>();
        dropType = await _drops.getTypeCardiopleg(_currentLanguage);
        if (cpb.CARDIOPLEGIA_TYPE != null)
        {
            var test = Convert.ToInt32(cpb.CARDIOPLEGIA_TYPE);
            var ci = dropType.Single(x => x.value == test);
            help = ci.description.ToLower();
        }
        return help;
    }
    private async Task<string> getIABPStuff(Class_CPB cpb)
    {
        string help = "";
        if (cpb.IABP_OPTIONS != null && cpb.IABP_OPTIONS != "0")
        {
            help = await getIABPWhenInserted(cpb);
            help = help + " inserted ";
            if (cpb.IABP_IND != null && cpb.IABP_IND != "0")
            {
                help = help + "for " + await getIABPUsedAsync(cpb);
            }
            else { help = ""; }
        }
        else { help = ""; }
        return help;
    }
    private async Task<string> getIABPWhenInserted(Class_CPB cpb)
    {
        // IABP when inserted, bv preoperatively etc
        var help = "";
        List<Class_Item> drop = new List<Class_Item>();
        drop = await _drops.getCPB_iabp_timing(_currentLanguage);
        try
        {
            var t = Convert.ToInt32(cpb.IABP_OPTIONS);
            var ci = drop.Single(x => x.value == t);
            help = ci.description;
        }
        catch (Exception e) { Console.Write(e.InnerException); }
        return help;
    }
    private async Task<string> getIABPUsedAsync(Class_CPB cpb)
    {
        // IABP indication
        var help = "";
        List<Class_Item> drop = new List<Class_Item>();
        drop = await _drops.getCPB_iabp_ind(_currentLanguage);
        try
        {
            var t = Convert.ToInt32(cpb.IABP_IND);
            var ci = drop.Single(x => x.value == t);
            help = ci.description;
        }
        catch (Exception e) { Console.Write(e.InnerException); }
        return help;
    }
    private async Task<string> getCirculationSupportAsync(Class_Procedure currentProcedure)
    {
        var help = "";
        List<Class_Item> dropCirc = new List<Class_Item>();
        dropCirc = await _drops.getInotropeOptionsAsync(_currentLanguage);

        if (currentProcedure.SelectedInotropes != 0)
        {
            var t = currentProcedure.SelectedInotropes;
            var ci = dropCirc.Single(x => x.value == t);
            help = ci.description;
        }
        return help;
    }
    private async Task<string> getPMWiresAsync(Class_Procedure currentProcedure)
    {
        var help = "";
        List<Class_Item> dropCirc = new List<Class_Item>();
        dropCirc = await _drops.getPacemakerOptionsAsync(_currentLanguage);

        if (currentProcedure.SelectedPacemaker != 0)
        {
            var t = currentProcedure.SelectedPacemaker;
            var ci = dropCirc.Single(x => x.value == t);
            help = ci.description;
        }
        return help;
    }

}
