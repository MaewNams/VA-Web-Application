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
        public string address { get; set; }
        public string phonenumber { get; set; }
        public string openingDetail { get; set; }
    }
}