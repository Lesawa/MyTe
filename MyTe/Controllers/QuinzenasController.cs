//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using MyTe.Models.Entities;
//using MyTe.Services;
//using System;

//namespace MyTe.Controllers
//{
//    public class QuinzenasController : Controller
//    {
//        public IActionResult Index(DateTime currentDate, int fortnight)
//        {
//            if (currentDate == default)
//            {
//                currentDate = DateTime.Today;
//            }

//            var fortnightsViewModel = GetFortnightViewModel(currentDate.Year, currentDate.Month);

//            if (fortnight == 1 || fortnight == 2)
//            {
//                return View(fortnightsViewModel[fortnight - 1]);
//            }
//            else
//            {
//                return RedirectToAction("Index", new { currentDate });
//            }
//        }

//        private List<FortnightViewModel> GetFortnightViewModel(int year, int month)
//        {
//            var startDate = new DateTime(year, month, 1);
//            var endDate = startDate.AddMonths(1).AddDays(-1);

//            var firstFortnightEnd = startDate.AddDays(14);
//            var secondFortnightEnd = endDate;

//            var fortnightViewModel = new List<FortnightViewModel>
//            {
//                new FortnightViewModel
//                {
//                    StartDate = startDate,
//                    EndDate = firstFortnightEnd,
//                    Title = "First Fortnight"
//                },
//                new FortnightViewModel
//                {
//                    StartDate = firstFortnightEnd.AddDays(1),
//                    EndDate = secondFortnightEnd,
//                    Title = "Second Fortnight"
//                }
//            };

//            return fortnightViewModel;
//        }
//    }
//}