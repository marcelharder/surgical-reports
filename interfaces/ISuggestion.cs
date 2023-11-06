namespace surgical_reports.interfaces;

    public interface ISuggestion
    {
        Task<List<Class_Item>> GetAllIndividualSuggestions(string userId);

        Task<Class_Suggestion> GetIndividualSuggestion(int soort, string userId);

        Task<int> updateSuggestion(Class_Suggestion c);

        Task<Class_Suggestion> AddIndividualSuggestion(Class_Suggestion c);

      
        
    }

