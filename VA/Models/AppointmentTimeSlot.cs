using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VA.Models
{
    public class AppointmentTimeSlot
    {
        [Key]
        public int id { get; set; }
        public int timeId { get; set; }
        public int appointmentID { get; set; }

        [ForeignKey("appointmentID")]
        public virtual Appointment Appointment { get; set; }

        [ForeignKey("timeId")]
        public virtual TimeSlot TimeSlot { get; set; }
    }
}