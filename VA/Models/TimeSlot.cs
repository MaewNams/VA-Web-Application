﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace VA.Models
{
    public class TimeSlot
    {
        [Key]
        public int id { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public int numberofCase { get; set; }
        public string status { get; set; }

        [InverseProperty("TimeSlot")]
        public virtual ICollection<AppointmentTimeSlot> AppointmentTimeBlock { get; set; }
    }
}