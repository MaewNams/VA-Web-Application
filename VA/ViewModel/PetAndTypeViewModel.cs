using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VA.Models;

namespace VA.ViewModel
{
    public class PetAndTypeViewModel
    {
        public Pet pet { get; set; }
        public IEnumerable<PetType> pettypes { get; set; }
    }
}