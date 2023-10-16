namespace surgical_reports.interfaces;

    public interface IInstitutionalText
    {
        Task<InstitutionalDTO> getText(string hospital, string soort, int procedure_id);
        Task addRecordInXML(string id);
        string updateInstitutionalReport(InstitutionalDTO rep, int hospitalNo, int soort);
        AdditionalReportDTO getAdditionalReportItems(int hospitalNo,int which);
        int updateAdditionalReportItem(AdditionalReportDTO l, int id, int which);
    }
