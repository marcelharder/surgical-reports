namespace surgical_reports.interfaces;

public interface IPreviewReport
    {
        Task<Class_Preview_Operative_report> getPreViewAsync(int procedure_id);
        Task<Class_Preview_Operative_report> getSpecificPVR(int procedure_id);
        Task<Class_Preview_Operative_report> resetPreViewAsync(int procedure_id);
        Task<int> updatePVR(Class_Preview_Operative_report pv);
        Task<bool> findPreview(int procedure_id);
        string getReportCode(string fdType);
    }
