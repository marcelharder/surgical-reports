namespace surgical_reports.interfaces;

    public interface ICPBRepo
    {
        Task<Class_CPB> getSpecificCPB(int id);
    }
