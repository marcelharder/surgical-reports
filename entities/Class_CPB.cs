using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace surgical_reports.entities
{
    public class Class_CPB
    {
        public int Id { get; set; }
        public int PROCEDURE_ID { get; set; }
        public int CROSS_CLAMP_TIME { get; set; }
        public int PERFUSION_TIME { get; set; }
        public  int LOWEST_CORE_TEMP { get; set; }
        public string CARDIOPLEGIA { get; set; }
        public string CARDIOPLEGIA_TYPE { get; set; }
        public string INFUSION_MODE_ANTE { get; set; }
        public  int INFUSION_MODE_RETRO { get; set; }
        public  int INFUSION_DOSE_INT { get; set; }
        public  int INFUSION_DOSE_CONT { get; set; }
        public  int CARDIOPLEGIA_TEMP_WARM { get; set; }
        public  int CARDIOPLEGIA_TEMP_COLD { get; set; }
        public string IABP { get; set; }
        public string IABP_OPTIONS { get; set; }
        public string IABP_IND { get; set; }
        public  int PACING_HARV { get; set; }
        public  int PACING_ATRIAL { get; set; }
        public  int PACING_VENTRICULAR { get; set; }
        public  int CARDIOVERSION { get; set; }
        public string VAD { get; set; }
        public  int LVAD { get; set; }
        public  int RVAD { get; set; }
        public string BVAD { get; set; }
        public string TAH { get; set; }
        public  int INOTROPES { get; set; }
        public  int Antiarrhythmics { get; set; }
        public  int SKIN_INCISION_START_TIME { get; set; }
        public  int SKIN_INCISION_STOP_TIME { get; set; }
        public string opcab_attempt { get; set; }
        public string cpb_used { get; set; }
        public string a1 { get; set; }
        public string a2 { get; set; }
        public string a3 { get; set; }
        public string a4 { get; set; }
        public string v1 { get; set; }
        public string v2 { get; set; }
        public string v3 { get; set; }
        public string v4 { get; set; }
        public string aoOCCL { get; set; }
        public  int long_isch { get; set; }
        public string cardiopl_timing { get; set; }
        public string cardiopl_temp { get; set; }
        public string cns_protect { get; set; }
        public  int cns_time_1 { get; set; }
        public  int cns_time_2 { get; set; }
        public  int cns_time_3 { get; set; }
        public string deep_hypo { get; set; }
        public string deep_hypo_rcp { get; set; }
        public string acp_circ { get; set; }
        public string other_cns_protect { get; set; }
        public string nonCMProtect { get; set; }
        public Nullable<short> nonCMProtect_type { get; set; }
        public Nullable<System.DateTime> IABP_DATE { get; set; }
        public string myoplasty { get; set; }
        public  int cpb_start_hr { get; set; }
        public  int cpb_start_min { get; set; }
        public  int cpb_stop_hr { get; set; }
        public  int cpb_stop_min { get; set; }
        public  int clamp_start_hr { get; set; }
        public  int clamp_start_min { get; set; }
        public  int clamp_stop_hr { get; set; }
        public  int clamp_stop_min { get; set; }
        public string other_cardiac_support { get; set; }
        public string cardiac_support { get; set; }
    }
}