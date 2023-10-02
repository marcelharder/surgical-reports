namespace surgical_reports.interfaces;

public interface IComposeFinalReport
{
   public Task<List<Class_Final_operative_report>> getFinalReports();
   public Task<Class_Final_operative_report> getSpecificReport(int id);
   public Task<Class_Final_operative_report> CreateFinalReport(frDto fr);

   public Task composeAsync(int procedure_id);
   public int deletePDF(int id);
   public Task<int> getReportCode(int procedure_id);
   public int deleteExpiredReports();
   public int addToExpiredReports(ReportTiming rt);
   public Task<bool> isReportExpired(int id);

}
