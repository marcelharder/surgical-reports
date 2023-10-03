namespace surgical_reports.interfaces;

    public interface ISuggestion
    {
        Task<List<Class_Item>> GetAllIndividualSuggestions(int userId);

        Task<Class_Suggestion> GetIndividualSuggestion(int soort, int userId);

        Task<Class_Suggestion> updateSuggestion(Class_Suggestion c);

        Task<Class_Suggestion> AddIndividualSuggestion(Class_Suggestion c);

        
    }

