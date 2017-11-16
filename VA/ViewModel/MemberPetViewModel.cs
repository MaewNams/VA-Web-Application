using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VA.Models;

namespace VA.ViewModel
{
    public class MemberPetViewModel
    {
        public Member member { get; set; }
        public IEnumerable<Pet> pets { get; set; }
        public IEnumerable<Specie> species { get; set; }
    }
}