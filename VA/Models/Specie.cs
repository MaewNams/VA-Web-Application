using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VA.Models
{
    public class Specie
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }

        [InverseProperty("Specie")]
        public virtual ICollection<Pet> Pets { get; set; }
    }
}