namespace surgical_reports.interfaces;

    public interface IOperativeReport
    {
         Task<int> getPdf(int report_code, int soort_operatie, Class_Final_operative_report fr);
    }
