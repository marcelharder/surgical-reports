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

        var query = "INSERT INTO Suggestions";



        _context.Suggestions.Add(cs);
        await _context.SaveChangesAsync();
        return 1;
    }
    public async Task<List<Class_Item>> GetAllIndividualSuggestions()
    {
        var currentUserId = _sp.getCurrentUserId();
        var help = new List<Class_Item>();
        var result = await _context.Suggestions.Where(x => x.user == currentUserId.ToString()).ToListAsync();
        foreach (Class_Suggestion sug in result) { help.Add(_special.mapSuggestionToClassItem(sug)); }
        return help;

    }
    public async Task<Class_Suggestion> GetIndividualSuggestion(int id)
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

