using Microsoft.AspNetCore.Mvc;
using Test01.Models;

namespace Test01.Controllers
{
    public class MathController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Calculate(double a, double b, string op)
        {
            var result = BuildResult(a, b, op);
            return View("Result", result);
        }

        private MathResult BuildResult(double a, double b, string op)
        {
            double c = 0;
            string symbol = "";
            bool isValid = true;
            string errorMessage = "";

            switch (op)
            {
                case "add":
                    c = a + b;
                    symbol = "+";
                    break;

                case "sub":
                    c = a - b;
                    symbol = "−";
                    break;

                case "mul":
                    c = a * b;
                    symbol = "×";
                    break;

                case "div":
                    symbol = "÷";
                    if (b == 0)
                    {
                        isValid = false;
                        errorMessage = "Không thể chia cho 0!";
                    }
                    else
                    {
                        c = a / b;
                    }
                    break;

                default:
                    isValid = false;
                    errorMessage = "Phép toán không hợp lệ!";
                    break;
            }

            return new MathResult
            {
                a = a,
                b = b,
                c = c,
                op = symbol,
                isValid = isValid,
                ErrorMessage = errorMessage
            };
        }

    }
}
