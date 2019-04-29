using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Dto
{
   public class PassengerAndTicketDto
    {
        public int Id { get; set; }
        public int ToCityId { get; set; }
        public string ToCity { get; set; }
        public int FromCityId { get; set; }
        public string FromCity { get; set; }
        public string Passenger { get; set; }
      
        public int UserId { get; set; }
        public int TotalCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<PassengerDto> PassengerDtos { get; set; }
        public int CompanyId { get; set; }
        public string Company { get; set; }
    }
}
