using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Dto
{
   public class PassengerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public DateTime BirthDate { get; set; }
        public string Tc { get; set; }
        public bool IsTc { get; set; }
        public string PassportNo { get; set; }
        public int PassengerAndTicketId { get; set; }
    }
}
