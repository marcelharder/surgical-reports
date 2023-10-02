namespace surgical_reports.entities;

public class Class_TimingReports
    {
        public int Id { get; set; }
        public string location {get; set;}
        public DateTime created {get; set;} 
        public Class_TimingReports(DateTime cr)
        {
            created = cr;
        }
    }
