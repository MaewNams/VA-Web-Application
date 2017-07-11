using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VA.ViewModel
{
    public class TimetableViewModel
    {
        public int timetableID { get; set; }
        public DateTime date { get; set; }
        public DateTime? start { get; set; }
        public DateTime? end { get; set; }
        public string status { get; set; }
    }
}