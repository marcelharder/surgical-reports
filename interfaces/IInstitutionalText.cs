using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace surgical_reports.interfaces
{
    public interface IInstitutionalText
    {
        Task<List<string>> getText(string hospital, string soort, int procedure_id);
    }
}