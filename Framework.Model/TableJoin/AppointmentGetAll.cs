using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Model.TableJoin
{
    public class AppointmentGetAll : Auditable
    {
        public int Id { get; set; }
        public DateTime DateAppointment { get; set; }
        public int TimeBegin { get; set; }
        public int DoctorId { get; set; }
        public String DoctorName { get; set; }
        public int TimeEnd { get; set; }
        public int SlotsCount { get; set; }
    }
}
