﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace VA.ViewModel
{
    public class AppointmentViewModel
    {
        public int appointmentId { get; set; }
        public DateTime date { get; set; }
        public DateTime? start { get; set; }
        public DateTime? end { get; set; }
        public string petName { get; set; }
        public string service { get; set; }
        public string suggestion { get; set; }
    }
}