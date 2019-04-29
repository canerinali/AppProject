using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Service;
using DAL.Dto;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            LoginControl();
            var service = new CommonService();
            return View(service.GetDashboard());

        }

        public ActionResult CityForPrice()
        {
            LoginControl();

            var service = new CommonService();
            var result = service.GetCityAndPric();
            return View(result);
        }

        [HttpGet]
        public ActionResult NewCityForPrice(int? id)
        {
            LoginControl();
            var service = new CommonService();
            ViewBag.fromCity = service.GetCity();
            ViewBag.toCity = service.GetCity();
            ViewBag.company = service.GetCompany();
            if (id != null)
            {
                return View(service.GetCityPriceById(id.Value));
            }
            else
            {
                return View(new CityAndPriceDto());
            }
           
        }
        public ActionResult DeleteCityPrice(int id)
        {
            LoginControl();
            var service = new CommonService();
            service.DeleteCityAndPric(id);
            return RedirectToAction("CityForPrice");
        }
        [HttpPost]
        public ActionResult NewCityForPrice(CityAndPriceDto model)
        {
            LoginControl();
            var service = new CommonService();

            var result = service.InsertCityAndPrice(model);
            ViewBag.fromCity = service.GetCity();
            ViewBag.toCity = service.GetCity();
            ViewBag.company = service.GetCompany();
            return RedirectToAction("CityForPrice");
        }

        public ActionResult Tickets()
        {
            LoginControl();
            var service = new CommonService();
            return View(service.GetCityPassengerAndTicket());
        }

        public ActionResult NewTickets()
        {
            LoginControl();
            var service = new CommonService();
            ViewBag.fromCity = service.GetCity();
            ViewBag.toCity = service.GetCity();
            ViewBag.company = service.GetCompany();
            return View();
        }
        [HttpPost]
        public ActionResult NewTickets(IEnumerable<PassengerAndTicketSaveDto> list)
        {
            LoginControl();
            var service = new CommonService();
            var login = Session["Login"] as LoginDto;
            
            var result = service.InsertPassengerAndTicket(new PassengerAndTicketDto()
            {
                FromCityId = list.ToList()[0].FromCityId,
                ToCityId = list.ToList()[0].ToCityId,
                TotalCount = list.Count(),
                UserId = login.Id,
                CompanyId = list.ToList()[0].CompanyId
                
            });
            service.InsertPassengerMultiple(result,list != null ? list.ToList() : new List<PassengerAndTicketSaveDto>());
            return RedirectToAction("Tickets");
        }

        public ActionResult GetPrice(int toCityId, int fromCityId)
        {
            var service = new CommonService();
            var result = service.GetPrice(toCityId, fromCityId);
            return Json(new
            {
                price = result != null ? result.Price : 0
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CityIndex()
        {
            var service = new CommonService();
            return View(service.GetCity());
        }
        public ActionResult NewCity(int? id)
        {
            var service = new CommonService();
            if (id!=null)
            {
                return View(service.GetByCityId(id.Value));
            }
            else
            {
                return View(new CityDto());
            }
           
        } 
        [HttpPost]
        public ActionResult NewCity(CityDto model)
        {
            var service = new CommonService();
            service.InsertCity(model);
            return RedirectToAction("CityIndex");
        }
        
        public ActionResult DeleteCity(int id)
        {
            var service = new CommonService();
            service.DeleteCity(id);
            return RedirectToAction("CityIndex");
        }

        #region Company operation
          public ActionResult CompanyIndex()
        {
            var service = new CommonService();
            return View(service.GetCompany());
        }
        public ActionResult NewCompany(int? id)
        {
            var service = new CommonService();
            if (id!=null)
            {
                return View(service.GetByCompanyId(id.Value));
            }
            else
            {
                return View(new CompanyDto());
            }
           
        } 
        [HttpPost]
        public ActionResult NewCompany(CompanyDto model)
        {
            var service = new CommonService();
            service.InsertCompany(model);
            return RedirectToAction("CompanyIndex");
        }
        
        public ActionResult DeleteCompany(int id)
        {
            var service = new CommonService();
            service.DeleteCompany(id);
            return RedirectToAction("CompanyIndex");
        }
        #endregion

        public ActionResult GetFromAndToCity(int companyId)
        {
            var service = new CommonService();
            var result = service.GetCityForCompanyId(companyId);
            return Json(new
            {
                price = result
            }, JsonRequestBehavior.AllowGet);
        }

        public bool LoginControl()
        {
            try
            {
                var login = Session["Login"] as LoginDto;
                if (login != null)
                {
                    return true;
                }
                else
                {
                    Response.Redirect("/Account/Login");
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}
