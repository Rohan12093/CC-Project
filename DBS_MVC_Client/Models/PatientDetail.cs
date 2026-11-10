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

        //--Kailash 8/11/2026 --//
        //Calcluating Patient age from PatientDob
        public int PatientAge
        {
            get
            {
                var today = DateOnly.FromDateTime(DateTime.Today);

                int age = today.Year - PatientDob.Year;

                if (today < PatientDob.AddYears(age))
                {
                    age--;
                }

                return age;
            }
        }


    }
}
