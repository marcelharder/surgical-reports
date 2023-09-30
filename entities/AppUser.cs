using Microsoft.AspNetCore.Identity;

namespace surgical_reports.entities;

    public class AppUser
    {
        public int hospital_id { get; set; }
        public string worked_in { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Gender { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public DateTime PaidTill { get; set; }
        public string KnownAs { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string IBAN { get; set; }
        public string Mobile { get; set; }
        public string Country { get; set; }
        public bool active { get; set; }
        public bool ltk { get; set; }
       
    }
