using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace surgical_reports.entities
{
    public class Class_Employee
    {
         public int Id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string profession { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public string liscense_to_kill { get; set; }
        public string selected_hospital_id { get; set; }
        public bool active { get; set; }
    }
}