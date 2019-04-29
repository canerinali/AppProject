using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL.Dto
{
   public class CityAndPriceDto
    {
       public int Id { get; set; }
       public int ToCityId { get; set; }
       public string ToCityName { get; set; }
       public int FromCityId { get; set; }
       public string FromCityName { get; set; }
       public double Price { get; set; }
       public int CompanyId { get; set; }
       public string Company { get; set; }
    }
}
