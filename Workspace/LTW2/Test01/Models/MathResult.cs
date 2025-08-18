namespace Test01.Models
{
    public class MathResult
    {
        public double a { get; set; }
        public double b { get; set; }
        public double c { get; set; }
        public string op { get; set; }
        public bool isValid { get; set; } = true;
        public string ErrorMessage { get; set; }
        public string message => isValid ? $"{a} {op} {b} = {c}" : ErrorMessage;
    }

}
