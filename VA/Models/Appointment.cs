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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
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
        public virtual ICollection<AppointmentTimeBlock> AppointmentTimeSlot { get; set; }

    }
}