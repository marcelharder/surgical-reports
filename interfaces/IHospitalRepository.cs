using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace surgical_reports.interfaces
{
    public interface IHospitalRepository
    {
         Task<HospitalForReturnDTO> GetSpecificHospital(string hospitalId);
    }
}