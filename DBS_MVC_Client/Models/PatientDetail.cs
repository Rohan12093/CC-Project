namespace DBS_MVC_Client.Models
{
    public class PatientDetail
    {
        public int PatientId { get; set; }
        public string PatientFname { get; set; }
        public string PatientLname { get; set; }
        public DateOnly PatientDob { get; set; }
        public string PatientStreetAddress { get; set; }
        public string PatientSuburb { get; set; }
        public string PatientCity { get; set; }
        public int PatientPostalCode { get; set; }
        public string PatientEmail { get; set; }
        public string PatientPhone { get; set; }
        public bool PatientEnrolStat { get; set; }

    }
}
