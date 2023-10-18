namespace surgical_reports.interfaces;

    public interface IInstitutionalText
    {
        Task<InstitutionalDTO> getInstitutionalReport(string hospital, string soort, string description);
        Task addRecordInXML(string id);
        string updateInstitutionalReport(InstitutionalDTO rep, int hospitalNo, int soort);
        AdditionalReportDTO getAdditionalReportItems(int hospitalNo,int which);
        int updateAdditionalReportItem(AdditionalReportDTO l, int id, int which);

        Task<IEnumerable<XElement>> getCurrentHospital(string hospitalNo);
    }
