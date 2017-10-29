using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Model.TableJoin
{
    public class AppointmentDetailGetAll : Auditable
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public String PatientName { get; set; }
        public String DoctorName { get; set; }
        public DateTime DateAppointment { get; set; }
        public int TimeBegin { get; set; }
        public int TimeEnd { get; set; }
    }
}
