using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VA.Models
{
    public class VAService
    {
        [Key]
        public int id { get; set; }
        public string description { get; set; }

        [InverseProperty("VAService")]
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}