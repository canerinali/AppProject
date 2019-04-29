using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL.Dto;

namespace Web.Models
{
    public class TicketsViewModel
    {
        public List<PassengerAndTicketDto> PassengerAndTicketDtos { get; set; }
        public List<PassengerDto> PassengerDtos { get; set; }
    }
}