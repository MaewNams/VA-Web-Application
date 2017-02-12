using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VA.Models
{
    public class Member
    {
        [Key]
        public int id { get; set; }
        public string codeId { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string address { get; set; }
        public string phonenumber { get; set; }

        [InverseProperty("Member")]
        public virtual ICollection<Pet> Pets { get; set; }
        [InverseProperty("Member")]
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}