namespace surgical_reports.interfaces;

    public interface IProcedureRepository
    {
        Task<Class_Procedure> getSpecificProcedure(int id);
        Task<int> getProcedureIdFromHash(string hash);
    }
