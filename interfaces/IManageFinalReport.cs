namespace surgical_reports.interfaces;

public interface IManageFinalReport
    {
        int DeletePDF(int id);
        int DeleteExpiredReports();
        int AddToExpiredReports(ReportTiming rt);
        Task<bool> IsReportExpired(int id);
        Task<bool> PdfDoesNotExists(string id_string);
    }
