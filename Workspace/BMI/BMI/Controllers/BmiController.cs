using BMI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BMI.Controllers
{
    public class BmiController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(double height, double weight)
        {
            if (height <= 0 || weight <= 0)
            {
                ViewBag.Error = "Chiều cao và cân nặng phải lớn hơn 0!";
                return View();
            }

            var result = CalculateBmi(height, weight);
            return View(result);
        }
        public BmiResult CalculateBmi(double height, double weight)
        {
            double bmi = Math.Round(weight / (height * height), 2);
            string status;

            if (bmi < 18.5)
                status = "Gầy";
            else if (bmi < 24.9)
                status = "Bình thường";
            else if (bmi < 29.9)
                status = "Thừa cân";
            else
                status = "Béo phì";

            return new BmiResult()
            {
                Height = height,
                Weight = weight,
                Bmi = bmi,
                Status = status
            };
        }
    }
}
