namespace surgical_reports.interfaces;

    public interface ICABGRepo
    {
        Task<Class_CABG> getSpecificCABG(int id);
    }
