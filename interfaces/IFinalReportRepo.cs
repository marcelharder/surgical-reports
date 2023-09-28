using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using surgical_reports.entities.dtos;
using surgical_reports.Entities;

namespace surgical_reports.interfaces;

    public interface IFinalReportRepo
    {
       public Task<List<Class_Final_operative_report>> getFinalReports(); 
       public Task<Class_Final_operative_report> getSpecificReport(int id); 
       public Task<Class_Final_operative_report> CreateFinalReport(frDto fr);
    }
