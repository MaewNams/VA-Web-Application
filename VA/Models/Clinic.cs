using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VA.Models
{
    public class Clinic
    {[Key]
        public int id { get; set; }
        public int maximumCase { get; set; }
    }
}