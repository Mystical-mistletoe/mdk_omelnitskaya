using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIApplication.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string UserLogin { get; set; }
        public int ServiceId { get; set; }
        public DateTime AppointmentTime { get; set; }
    }
}
