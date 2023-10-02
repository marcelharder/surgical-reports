using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace surgical_reports.interfaces
{
    public interface IValveRepo
    {
        Task<Class_Valve> getValveBySerial(string serial);
    }
}