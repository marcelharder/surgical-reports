namespace surgical_reports.interfaces;

    public interface IHospitalRepository
    {
         Task<HospitalForReturnDTO> GetSpecificHospital(string hospitalId);
    }
