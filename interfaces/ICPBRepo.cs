using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace surgical_reports.interfaces
{
    public interface ICPBRepo
    {
        Task<Class_CPB> getSpecificCPB(int id);
    }
}