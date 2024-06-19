//using Dashboard_Service.Models.DTO;
//using Dashboard_Service.Services.IService;
//using FrontEndService.Models.WebDTO.BioDTO;

//using FrontEndService.Services.IServices;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace FrontEndService.Controllers
//{
//    public class PatientDashboardController : Controller
//    {
//        private readonly IPatientDashboardService _PatientDashboardService;
//        public PatientDashboardController(IPatientDashboardService PatientDashboardService)
//        {
//            _PatientDashboardService = PatientDashboardService;
//        }

//        [HttpGet]
//        public async Task<IActionResult> Dashboard()
//        {
//            return View();
//        }

//        [HttpGet]
//        public async Task<IActionResult> LoadPatientInfo()
//        {
//            var patientInfo = await _PatientDashboardService.GetBioByUserIdAsync<PatientBioDTO>();
//            return PartialView("_PatientInfo", patientInfo);
//        }

//        [HttpGet]
//        public async Task<IActionResult> LoadEditPatientInfo()
//        {
//            var patientInfo = await _PatientDashboardService.GetBioByUserIdAsync<PatientBioDTO>();
//            return PartialView("_EditPatientInfo", patientInfo);
//        }

//        [HttpPost]
//        public async Task<IActionResult> EditPatientInfo(PatientBioDTO bioDTO)
//        {
//            if (ModelState.IsValid)
//            {
//                await _PatientDashboardService.UpdateBioAsync<PatientBioDTO>(bioDTO);
//                return RedirectToAction("Dashboard");
//            }
//            return PartialView("_EditPatientInfo", bioDTO);
//        }
//    }
//}
using Dashboard_Service.Models.DTO;
using Dashboard_Service.Services.IService;
using FrontEndService.Models.WebDTO.BioDTO;
using FrontEndService.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FrontEndService.Controllers
{
    public class PatientDashboardController : Controller
    {
        private readonly IPatientDashboardService _PatientDashboardService;
        public PatientDashboardController(IPatientDashboardService PatientDashboardService)
        {
            _PatientDashboardService = PatientDashboardService;
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoadPatientInfo()
        {
            var patientInfo = await _PatientDashboardService.GetBioByUserIdAsync<PatientBioDTO>();
            Console.WriteLine("Patient Info is    " + patientInfo);
            return PartialView("_PatientInfo", patientInfo);
        }

        [HttpGet]
        public async Task<IActionResult> LoadEditPatientInfo()
        {
            var patientInfo = await _PatientDashboardService.GetBioByUserIdAsync<PatientBioDTO>();
            // Mock data for testing
            //var patientInfo = new PatientBioDTO
            //{
            //    FullName = "John Doe",
            //    ContactNumber = "1234567890",
            //    EmailAddress = "johndoe@example.com",
            //    Address = "123 Main St"
            //};

            return PartialView("_EditPatientInfo", patientInfo);
        }

        [HttpPost]
        public async Task<IActionResult> EditPatientInfo(PatientBioDTO bioDTO)
        {
            if (ModelState.IsValid)
            {
                await _PatientDashboardService.UpdateBioAsync<PatientBioDTO>(bioDTO);
                return RedirectToAction("Dashboard");
            }
            return PartialView("_EditPatientInfo", bioDTO);
        }
    }
}
