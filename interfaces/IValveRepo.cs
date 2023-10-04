namespace surgical_reports.interfaces;

    public interface IValveRepo
    {
        Task<Class_Valve> getValveBySerial(string serial);
         Task<Class_Valve> getValve(string implantPosition, int procedure_id);
      
    }
