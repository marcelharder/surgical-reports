namespace surgical_reports.interfaces;

    public interface IValveRepo
    {
        Task<Class_Valve> getValveBySerial(string serial);
    }
