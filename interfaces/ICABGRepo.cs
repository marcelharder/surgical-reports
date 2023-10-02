using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace surgical_reports.interfaces
{
    public interface ICABGRepo
    {
        Task<Class_CABG> getSpecificCABG(int id);
    }
}