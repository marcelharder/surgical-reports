namespace surgical_reports.interfaces;

public interface IPreviewReport
    {
        Task<Class_Preview_Operative_report> getPreViewAsync(int procedure_id);
        Task<Class_Preview_Operative_report> resetPreViewAsync(int procedure_id);
        Task updatePVR(Class_Preview_Operative_report pv);
    }
