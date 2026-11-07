namespace DBS_MVC_Client.Models
{
    public class DoctorSchedule
    {
        public int DocScheId { get; set; }
        public DateOnly DocSchDate { get; set; }
        public TimeOnly DocSchStime { get; set; }
        public TimeOnly DocSchEtime { get; set; }
        public bool DocSchIsBooked { get; set; }
        public int? DoctorId { get; set; }

    }
}
