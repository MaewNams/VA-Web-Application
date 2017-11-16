using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VA.Models
{
    public class Pet
    {
        [Key]
        public int id { get; set; }
        public int memberId { get; set; }
        public string name { get; set; }
        public int specieId { get; set; }

        [ForeignKey("memberId")]
        public virtual Member Member { get; set; }

        [ForeignKey("specieId")]
        public virtual Specie Specie { get; set; }

        [InverseProperty("Pet")]
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}