 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VA.Models
{
    public class Appointment
    {
        [Key]
        public int id { get; set; }
        public int memberId { get; set; }
        public int petId { get; set; }
        public int serviceId { get; set; }
        public string detail { get; set; }
        public string suggestion { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string status { get; set; }

        [ForeignKey("memberId")]
        public virtual Member Member { get; set; }

        [ForeignKey("petId")]
        public virtual Pet Pet { get; set; }

        [ForeignKey("serviceId")]
        public virtual VAService VAService { get; set; }

        [InverseProperty("Appointment")]
<<<<<<< HEAD
        public virtual ICollection<AppointmentTimeSlot> AppointmentTimeBlock { get; set; }
=======
        public virtual ICollection<AppointmentTimeBlock> AppointmentTimeSlot { get; set; }
>>>>>>> 77357a911c4c8b10fde568890d9b5e97c99aa41a

    }
}