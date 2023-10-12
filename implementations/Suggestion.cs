namespace surgical_reports.implementations;

public class Suggestion : ISuggestion
{
    private DapperContext _context;
    private IWebHostEnvironment _env;
    public Suggestion(DapperContext context, IWebHostEnvironment env)
    {
        _env = env;
        _context = context;
    }
    public async Task<Class_Suggestion> AddIndividualSuggestion(Class_Suggestion cs)
    {

        var query = "INSERT INTO Suggestions (soort, user, regel_1_a, regel_1_b, regel_1_c, regel_2_a, regel_2_b, regel_2_c, " +
        "regel_3_a, regel_3_b, regel_3_c,regel_4_a, regel_4_b, regel_4_c,regel_5_a, regel_5_b, regel_5_c, " +
        "regel_6_a, regel_6_b, regel_6_c,regel_7_a, regel_7_b, regel_7_c,regel_8_a, regel_8_b, regel_8_c, " +
        "regel_9_a, regel_9_b, regel_9_c,regel_10_a, regel_10_b, regel_10_c,regel_11_a, regel_11_b, regel_11_c, " +
        "regel_12_a, regel_12_b, regel_12_c,regel_13_a, regel_13_b, regel_13_c,regel_14_a, regel_14_b, regel_14_c, " +
        "regel_15, regel_16, regel_17, regel_18, regel_19, regel_20, " +
        "regel_21, regel_22, regel_23, regel_24, regel_25, regel_26, regel_27, " +
        "regel_28, regel_29, regel_30, regel_31, regel_32, regel_33) " +
        "VALUES (@soort, @user, regel_1_a, @regel_1_b, @regel_1_c, @regel_2_a, @regel_2_b, @regel_2_c, " +
        "@regel_3_a, @regel_3_b, @regel_3_c,@regel_4_a, @regel_4_b, @regel_4_c,@regel_5_a, @regel_5_b, @regel_5_c, " +
        "@regel_6_a, @regel_6_b, @regel_6_c,@regel_7_a, @regel_7_b, @regel_7_c,@regel_8_a, @regel_8_b, @regel_8_c, " +
        "@regel_9_a, @regel_9_b, @regel_9_c,@regel_10_a, @regel_10_b, @regel_10_c,@regel_11_a, @regel_11_b, @regel_11_c, " +
        "@regel_12_a, @regel_12_b, @regel_12_c,@regel_13_a, @regel_13_b, @regel_13_c,@regel_14_a, @regel_14_b, @regel_14_c, " +
        "@regel_15, @regel_16, @regel_17, @regel_18, @regel_19, @regel_20, " +
        "@regel_21, @regel_22, @regel_23, @regel_24, @regel_25, @regel_26, @regel_27, " +
        "@regel_28, @regel_29, @regel_30, @regel_31, @regel_32, @regel_33 );" + "SELECT LAST_INSERT_ID();";

        var parameters = new DynamicParameters();

        parameters.Add("soort", cs.soort, DbType.Int32);
        parameters.Add("user", cs.user, DbType.String);
        parameters.Add("regel_1_a", cs.regel_1_a, DbType.String);
        parameters.Add("regel_1_b", cs.regel_1_b, DbType.String);
        parameters.Add("regel_1_c", cs.regel_1_c, DbType.String);
        parameters.Add("regel_2_a", cs.regel_2_a, DbType.String);
        parameters.Add("regel_2_b", cs.regel_2_b, DbType.String);
        parameters.Add("regel_2_c", cs.regel_2_c, DbType.String);
        parameters.Add("regel_3_a", cs.regel_3_a, DbType.String);
        parameters.Add("regel_3_b", cs.regel_3_b, DbType.String);
        parameters.Add("regel_3_c", cs.regel_3_c, DbType.String);
        parameters.Add("regel_4_a", cs.regel_4_a, DbType.String);
        parameters.Add("regel_4_b", cs.regel_4_b, DbType.String);
        parameters.Add("regel_4_c", cs.regel_4_c, DbType.String);
        parameters.Add("regel_5_a", cs.regel_5_a, DbType.String);
        parameters.Add("regel_5_b", cs.regel_5_b, DbType.String);
        parameters.Add("regel_5_c", cs.regel_5_c, DbType.String);
        parameters.Add("regel_6_a", cs.regel_6_a, DbType.String);
        parameters.Add("regel_6_b", cs.regel_6_b, DbType.String);
        parameters.Add("regel_6_c", cs.regel_6_c, DbType.String);
        parameters.Add("regel_7_a", cs.regel_7_a, DbType.String);
        parameters.Add("regel_7_b", cs.regel_7_b, DbType.String);
        parameters.Add("regel_7_c", cs.regel_7_c, DbType.String);
        parameters.Add("regel_8_a", cs.regel_8_a, DbType.String);
        parameters.Add("regel_8_b", cs.regel_8_b, DbType.String);
        parameters.Add("regel_8_c", cs.regel_8_c, DbType.String);
        parameters.Add("regel_9_a", cs.regel_9_a, DbType.String);
        parameters.Add("regel_9_b", cs.regel_9_b, DbType.String);
        parameters.Add("regel_9_c", cs.regel_9_c, DbType.String);
        parameters.Add("regel_10_a", cs.regel_10_a, DbType.String);
        parameters.Add("regel_10_b", cs.regel_10_b, DbType.String);
        parameters.Add("regel_10_c", cs.regel_10_c, DbType.String);

        parameters.Add("regel_11_a", cs.regel_11_a, DbType.String);
        parameters.Add("regel_11_b", cs.regel_11_b, DbType.String);
        parameters.Add("regel_11_c", cs.regel_11_c, DbType.String);
        parameters.Add("regel_12_a", cs.regel_12_a, DbType.String);
        parameters.Add("regel_12_b", cs.regel_12_b, DbType.String);
        parameters.Add("regel_12_c", cs.regel_12_c, DbType.String);
        parameters.Add("regel_13_a", cs.regel_13_a, DbType.String);
        parameters.Add("regel_13_b", cs.regel_13_b, DbType.String);
        parameters.Add("regel_13_c", cs.regel_13_c, DbType.String);
        parameters.Add("regel_14_a", cs.regel_14_a, DbType.String);
        parameters.Add("regel_14_b", cs.regel_14_b, DbType.String);
        parameters.Add("regel_14_c", cs.regel_14_c, DbType.String);
        parameters.Add("regel_15", cs.regel_15, DbType.String);
        parameters.Add("regel_16", cs.regel_16, DbType.String);
        parameters.Add("regel_17", cs.regel_17, DbType.String);
        parameters.Add("regel_18", cs.regel_18, DbType.String);
        parameters.Add("regel_19", cs.regel_19, DbType.String);
        parameters.Add("regel_20", cs.regel_20, DbType.String);

        parameters.Add("regel_21", cs.regel_21, DbType.String);
        parameters.Add("regel_22", cs.regel_22, DbType.String);
        parameters.Add("regel_23", cs.regel_23, DbType.String);
        parameters.Add("regel_24", cs.regel_24, DbType.String);
        parameters.Add("regel_25", cs.regel_25, DbType.String);
        parameters.Add("regel_26", cs.regel_26, DbType.String);
        parameters.Add("regel_27", cs.regel_27, DbType.String);
        parameters.Add("regel_28", cs.regel_28, DbType.String);
        parameters.Add("regel_29", cs.regel_29, DbType.String);
        parameters.Add("regel_30", cs.regel_30, DbType.String);

        parameters.Add("regel_31", cs.regel_31, DbType.String);
        parameters.Add("regel_32", cs.regel_32, DbType.String);
        parameters.Add("regel_33", cs.regel_33, DbType.String);

        using (var connection = _context.CreateConnection())
        {
            var id = await connection.QuerySingleAsync<int>(query, parameters);
            var createdSuggestion = new Class_Suggestion
            {
                Id = id,
                soort = cs.soort,
                user = cs.user,
                regel_1_a = cs.regel_1_a,
                regel_1_b = cs.regel_1_b,
                regel_1_c = cs.regel_1_c,
                regel_2_a = cs.regel_2_a,
                regel_2_b = cs.regel_2_b,
                regel_2_c = cs.regel_2_c,
                regel_3_a = cs.regel_3_a,
                regel_3_b = cs.regel_3_b,
                regel_3_c = cs.regel_3_c,
                regel_4_a = cs.regel_4_a,
                regel_4_b = cs.regel_4_b,
                regel_4_c = cs.regel_4_c,
                regel_5_a = cs.regel_5_a,
                regel_5_b = cs.regel_5_b,
                regel_5_c = cs.regel_5_c,
                regel_6_a = cs.regel_6_a,
                regel_6_b = cs.regel_6_b,
                regel_6_c = cs.regel_6_c,
                regel_7_a = cs.regel_7_a,
                regel_7_b = cs.regel_7_b,
                regel_7_c = cs.regel_7_c,
                regel_8_a = cs.regel_8_a,
                regel_8_b = cs.regel_8_b,
                regel_8_c = cs.regel_8_c,
                regel_9_a = cs.regel_9_a,
                regel_9_b = cs.regel_9_b,
                regel_9_c = cs.regel_9_c,
                regel_10_a = cs.regel_10_a,
                regel_10_b = cs.regel_10_b,
                regel_10_c = cs.regel_10_c,
                regel_11_a = cs.regel_11_a,
                regel_11_b = cs.regel_11_b,
                regel_11_c = cs.regel_11_c,
                regel_12_a = cs.regel_12_a,
                regel_12_b = cs.regel_12_b,
                regel_12_c = cs.regel_12_c,
                regel_13_a = cs.regel_13_a,
                regel_13_b = cs.regel_13_b,
                regel_13_c = cs.regel_13_c,
                regel_14_a = cs.regel_14_a,
                regel_14_b = cs.regel_14_b,
                regel_14_c = cs.regel_14_c,
                regel_15 = cs.regel_15,
                regel_16 = cs.regel_16,
                regel_17 = cs.regel_17,
                regel_18 = cs.regel_18,
                regel_19 = cs.regel_19,
                regel_20 = cs.regel_20,
                regel_21 = cs.regel_21,
                regel_22 = cs.regel_22,
                regel_23 = cs.regel_23,
                regel_24 = cs.regel_24,
                regel_25 = cs.regel_25,
                regel_26 = cs.regel_26,
                regel_27 = cs.regel_27,
                regel_28 = cs.regel_28,
                regel_29 = cs.regel_29,
                regel_30 = cs.regel_30,
                regel_31 = cs.regel_31,
                regel_32 = cs.regel_32,
                regel_33 = cs.regel_33
            };
            return createdSuggestion;
        }



    }
    public async Task<List<Class_Item>> GetAllIndividualSuggestions(string userId)
    {
        var help = new List<Class_Item>();
        var query = "SELECT * FROM Suggestions WHERE user = @userId";

        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QueryAsync<Class_Suggestion>(query, new { userId });
            foreach (Class_Suggestion sug in result) { help.Add(mapSuggestionToClassItem(sug)); }
            return help;
        }
    }
    public async Task<Class_Suggestion> GetIndividualSuggestion(int soort, string userId)
    {
        var query = "SELECT * FROM Suggestions WHERE user = @userId AND soort = @soort";
        using (var connection = _context.CreateConnection())
        {
            var result = await connection.QuerySingleOrDefaultAsync<Class_Suggestion>(query, new { userId, soort });
            if(result == null){
                var sug = new Class_Suggestion();
                sug.user = userId;
                sug.soort = soort;
                return await AddIndividualSuggestion(sug);
            }
            return result;
        }
    }
    public async Task<int> updateSuggestion(Class_Suggestion cs)
    {
        var query = "UPDATE Suggestions SET regel_1_a = @regel_1_a,regel_1_b = @regel_1_b,regel_1_c = @regel_1_c, " +
       "regel_2_a = @regel_2_a,regel_2_b = @regel_2_b,regel_2_c = @regel_2_c, " +
       "regel_3_a = @regel_3_a,regel_3_b = @regel_3_b,regel_3_c = @regel_3_c, " +
       "regel_4_a = @regel_4_a,regel_4_b = @regel_4_b,regel_4_c = @regel_4_c, " +
       "regel_5_a = @regel_5_a,regel_5_b = @regel_5_b,regel_5_c = @regel_5_c, " +
       "regel_6_a = @regel_6_a,regel_6_b = @regel_6_b,regel_6_c = @regel_6_c, " +
       "regel_7_a = @regel_7_a,regel_7_b = @regel_7_b,regel_7_c = @regel_7_c, " +
       "regel_8_a = @regel_8_a,regel_8_b = @regel_8_b,regel_8_c = @regel_8_c, " +
       "regel_9_a = @regel_9_a,regel_9_b = @regel_9_b,regel_9_c = @regel_9_c, " +

       "regel_10_a = @regel_10_a,regel_10_b = @regel_10_b,regel_10_c = @regel_10_c, " +
       "regel_11_a = @regel_11_a,regel_11_b = @regel_11_b,regel_11_c = @regel_11_c, " +
       "regel_12_a = @regel_12_a,regel_12_b = @regel_12_b,regel_12_c = @regel_12_c, " +
       "regel_13_a = @regel_13_a,regel_13_b = @regel_13_b,regel_13_c = @regel_13_c, " +
       "regel_14_a = @regel_14_a,regel_14_b = @regel_14_b,regel_14_c = @regel_14_c, " +

       "regel_15 = @regel_15, regel_16 = @regel_16, regel_17 = @regel_17, " +
       "regel_18 = @regel_18, regel_19 = @regel_19, regel_20 = @regel_20, " +
       "regel_21 = @regel_21, regel_22 = @regel_22, regel_23 = @regel_23, " +
       "regel_24 = @regel_24, regel_25 = @regel_25, regel_26 = @regel_26, " +
       "regel_27 = @regel_27, regel_28 = @regel_28, regel_29 = @regel_29, " +
       "regel_30 = @regel_30, regel_31 = @regel_31, regel_32 = @regel_32, " +
       "regel_33 = @regel_33 WHERE soort = @soort AND user = @user";

        var parameters = new DynamicParameters();
        parameters.Add("soort", cs.soort, DbType.Int32);
        parameters.Add("user", cs.user, DbType.String);
        parameters.Add("regel_1_a", cs.regel_1_a, DbType.String);
        parameters.Add("regel_1_b", cs.regel_1_b, DbType.String);
        parameters.Add("regel_1_c", cs.regel_1_c, DbType.String);
        parameters.Add("regel_2_a", cs.regel_2_a, DbType.String);
        parameters.Add("regel_2_b", cs.regel_2_b, DbType.String);
        parameters.Add("regel_2_c", cs.regel_2_c, DbType.String);
        parameters.Add("regel_3_a", cs.regel_3_a, DbType.String);
        parameters.Add("regel_3_b", cs.regel_3_b, DbType.String);
        parameters.Add("regel_3_c", cs.regel_3_c, DbType.String);
        parameters.Add("regel_4_a", cs.regel_4_a, DbType.String);
        parameters.Add("regel_4_b", cs.regel_4_b, DbType.String);
        parameters.Add("regel_4_c", cs.regel_4_c, DbType.String);
        parameters.Add("regel_5_a", cs.regel_5_a, DbType.String);
        parameters.Add("regel_5_b", cs.regel_5_b, DbType.String);
        parameters.Add("regel_5_c", cs.regel_5_c, DbType.String);
        parameters.Add("regel_6_a", cs.regel_6_a, DbType.String);
        parameters.Add("regel_6_b", cs.regel_6_b, DbType.String);
        parameters.Add("regel_6_c", cs.regel_6_c, DbType.String);
        parameters.Add("regel_7_a", cs.regel_7_a, DbType.String);
        parameters.Add("regel_7_b", cs.regel_7_b, DbType.String);
        parameters.Add("regel_7_c", cs.regel_7_c, DbType.String);
        parameters.Add("regel_8_a", cs.regel_8_a, DbType.String);
        parameters.Add("regel_8_b", cs.regel_8_b, DbType.String);
        parameters.Add("regel_8_c", cs.regel_8_c, DbType.String);
        parameters.Add("regel_9_a", cs.regel_9_a, DbType.String);
        parameters.Add("regel_9_b", cs.regel_9_b, DbType.String);
        parameters.Add("regel_9_c", cs.regel_9_c, DbType.String);
        parameters.Add("regel_10_a", cs.regel_10_a, DbType.String);
        parameters.Add("regel_10_b", cs.regel_10_b, DbType.String);
        parameters.Add("regel_10_c", cs.regel_10_c, DbType.String);

        parameters.Add("regel_11_a", cs.regel_11_a, DbType.String);
        parameters.Add("regel_11_b", cs.regel_11_b, DbType.String);
        parameters.Add("regel_11_c", cs.regel_11_c, DbType.String);
        parameters.Add("regel_12_a", cs.regel_12_a, DbType.String);
        parameters.Add("regel_12_b", cs.regel_12_b, DbType.String);
        parameters.Add("regel_12_c", cs.regel_12_c, DbType.String);
        parameters.Add("regel_13_a", cs.regel_13_a, DbType.String);
        parameters.Add("regel_13_b", cs.regel_13_b, DbType.String);
        parameters.Add("regel_13_c", cs.regel_13_c, DbType.String);
        parameters.Add("regel_14_a", cs.regel_14_a, DbType.String);
        parameters.Add("regel_14_b", cs.regel_14_b, DbType.String);
        parameters.Add("regel_14_c", cs.regel_14_c, DbType.String);
        parameters.Add("regel_15", cs.regel_15, DbType.String);
        parameters.Add("regel_16", cs.regel_16, DbType.String);
        parameters.Add("regel_17", cs.regel_17, DbType.String);
        parameters.Add("regel_18", cs.regel_18, DbType.String);
        parameters.Add("regel_19", cs.regel_19, DbType.String);
        parameters.Add("regel_20", cs.regel_20, DbType.String);

        parameters.Add("regel_21", cs.regel_21, DbType.String);
        parameters.Add("regel_22", cs.regel_22, DbType.String);
        parameters.Add("regel_23", cs.regel_23, DbType.String);
        parameters.Add("regel_24", cs.regel_24, DbType.String);
        parameters.Add("regel_25", cs.regel_25, DbType.String);
        parameters.Add("regel_26", cs.regel_26, DbType.String);
        parameters.Add("regel_27", cs.regel_27, DbType.String);
        parameters.Add("regel_28", cs.regel_28, DbType.String);
        parameters.Add("regel_29", cs.regel_29, DbType.String);
        parameters.Add("regel_30", cs.regel_30, DbType.String);

        parameters.Add("regel_31", cs.regel_31, DbType.String);
        parameters.Add("regel_32", cs.regel_32, DbType.String);
        parameters.Add("regel_33", cs.regel_33, DbType.String);

        try { using (var connection = _context.CreateConnection()) { await connection.ExecuteAsync(query, parameters); } }
        catch (Exception e) { Console.WriteLine(e.InnerException); }

        return 1;
    }
    public async Task<Class_Suggestion> mapToSuggestionFromPreview(Class_Suggestion help, Class_Preview_Operative_report c)
    {
        await Task.Run(() =>
        {

            help.regel_1_a = c.regel_1;
            help.regel_2_a = c.regel_2;
            help.regel_3_a = c.regel_3;
            help.regel_4_a = c.regel_4;
            help.regel_5_a = c.regel_5;
            help.regel_6_a = c.regel_6;
            help.regel_7_a = c.regel_7;
            help.regel_8_a = c.regel_8;
            help.regel_9_a = c.regel_9;
            help.regel_10_a = c.regel_10;
            help.regel_11_a = c.regel_11;
            help.regel_12_a = c.regel_12;
            help.regel_13_a = c.regel_13;
            help.regel_14_a = c.regel_14;
            help.regel_15 = c.regel_15;
            help.regel_16 = c.regel_16;
            help.regel_17 = c.regel_17;
            help.regel_18 = c.regel_18;
            help.regel_19 = c.regel_19;
            help.regel_20 = c.regel_20;
            help.regel_21 = c.regel_21;
            help.regel_22 = c.regel_22;
            help.regel_23 = c.regel_23;
            help.regel_24 = c.regel_24;
            help.regel_25 = c.regel_25;
            help.regel_26 = c.regel_26;
            help.regel_27 = c.regel_27;
            help.regel_28 = c.regel_28;
            help.regel_29 = c.regel_29;
            help.regel_30 = c.regel_30;
            help.regel_31 = c.regel_31;
            help.regel_32 = c.regel_32;
            help.regel_33 = c.regel_33;

            help.regel_1_b = ""; help.regel_1_c = "";
            help.regel_2_b = ""; help.regel_2_c = "";
            help.regel_3_b = ""; help.regel_3_c = "";
            help.regel_4_b = ""; help.regel_4_c = "";
            help.regel_5_b = ""; help.regel_5_c = "";
            help.regel_6_b = ""; help.regel_6_c = "";
            help.regel_7_b = ""; help.regel_7_c = "";
            help.regel_8_b = ""; help.regel_8_c = "";
            help.regel_9_b = ""; help.regel_9_c = "";
            help.regel_10_b = ""; help.regel_10_c = "";
            help.regel_11_b = ""; help.regel_11_c = "";
            help.regel_12_b = ""; help.regel_12_c = "";
            help.regel_13_b = ""; help.regel_13_c = "";
            help.regel_14_b = ""; help.regel_14_c = "";
        });
        return help;
    }
    private Class_Item mapSuggestionToClassItem(Class_Suggestion sug)
    {
        var help = new Class_Item();
        help.value = sug.soort;
        help.description = getProcedureDescription(sug.soort);
        return help;
    }
    private string getProcedureDescription(int soort)
    {
        var result = "";
        var contentRoot = _env.ContentRootPath;
        var filename = Path.Combine(contentRoot, "xml/procedure.xml");
        XDocument order = XDocument.Load(filename);
        IEnumerable<XElement> help = from d in order.Descendants("Code")
                                     where d.Element("ID").Value == soort.ToString()
                                     select d;
        foreach (XElement x in help) { result = x.Element("Description").Value; }
        return result;
    }
}

