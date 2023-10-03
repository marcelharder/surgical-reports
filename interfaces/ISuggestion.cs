namespace surgical_reports.interfaces;

    public interface ISuggestion
    {
        Task<List<Class_Item>> GetAllIndividualSuggestions();

        Task<Class_Suggestion> GetIndividualSuggestion(int id);

        Task<Class_Suggestion> updateSuggestion(Class_Suggestion c);

        Task<Class_Suggestion> AddIndividualSuggestion(Class_Suggestion c);

        
    }

