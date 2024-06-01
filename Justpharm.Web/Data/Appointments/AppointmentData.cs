namespace Justpharm.Web.Data.Appointments
{
    public class AppointmentData
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Location { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public bool IsAllDay { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }
        public Nullable<int> RecurrenceID { get; set; }
        public string Color { get; set; }
        public Guid? UidTomas { get; set; }
    }
}
