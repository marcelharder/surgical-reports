namespace surgical_reports.implementations;

public class Suggestion : ISuggestion
{

    private DapperContext _context;

    public Suggestion(DapperContext context)
    {

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
        "regel_21, regel_22, regel_23, regel_24, regel_25 regel_27, " +
        "regel_28, regel_29, regel_30, regel_31, regel_32, regel_33 )" +
        "VALUES (@soort, @user, regel_1_a, @regel_1_b, @regel_1_c, @regel_2_a, @regel_2_b, @regel_2_c, " +
        "@regel_3_a, @regel_3_b, @regel_3_c,@regel_4_a, @regel_4_b, @regel_4_c,@regel_5_a, @regel_5_b, @regel_5_c, " +
        "@regel_6_a, @regel_6_b, @regel_6_c,@regel_7_a, @regel_7_b, @regel_7_c,@regel_8_a, @regel_8_b, @regel_8_c, " +
        "@regel_9_a, @regel_9_b, @regel_9_c,@regel_10_a, @regel_10_b, @regel_10_c,@regel_11_a, @regel_11_b, @regel_11_c, " +
        "@regel_12_a, @regel_12_b, @regel_12_c,@regel_13_a, @regel_13_b, @regel_13_c,@regel_14_a, @regel_14_b, @regel_14_c, " +
        "@regel_15, @regel_16, @regel_17, @regel_18, @regel_19, @regel_20, " +
        "@regel_21, @regel_22, @regel_23, @regel_24, @regel_25 @regel_27, " +
        "@regel_28, @regel_29, @regel_30, @regel_31, @regel_32, @regel_33 )" + "SELECT LAST_INSERT_ID();";

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
    public async Task<List<Class_Item>> GetAllIndividualSuggestions(int userId)
    {

        var query = "SELECT * FROM Suggestions WHERE user = @userId";

        
         
       /*  var currentUserId = _sp.getCurrentUserId();
        var help = new List<Class_Item>();
        var result = await _context.Suggestions.Where(x => x.user == currentUserId.ToString()).ToListAsync();
        foreach (Class_Suggestion sug in result) { help.Add(_special.mapSuggestionToClassItem(sug)); }
        return help; */

    }
    public async Task<Class_Suggestion> GetIndividualSuggestion(int soort, int userId)
    {

        var result = new Class_Suggestion();
        var check = await _context.Suggestions
            .Where(x => x.user == _sp.getCurrentUserId().ToString())
            .Where(x => x.soort == id).AnyAsync();
        if (check)
        {
            result = await _context.Suggestions
                 .Where(x => x.user == _sp.getCurrentUserId().ToString())
                 .Where(x => x.soort == id).FirstOrDefaultAsync();
        }
        else
        {
            result.user = _sp.getCurrentUserId().ToString();
            result.soort = id;
        }
        return result;
    }
    public async Task<Class_Suggestion> updateSuggestion(Class_Suggestion cs)
    {
        var query = "";


    }
}

