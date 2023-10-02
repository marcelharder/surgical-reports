using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace surgical_reports.interfaces
{
    public interface IProcedureRepository
    {
        Task<Class_Procedure> getSpecificProcedure(int id);
        Task<int> getProcedureIdFromHash(string hash);
    }
}