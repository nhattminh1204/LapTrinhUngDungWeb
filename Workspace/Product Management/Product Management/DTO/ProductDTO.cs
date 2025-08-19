namespace Product_Management.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public DateTime ManuDate { get; set; }
        public int Quantity { get; set; }
        public string image { get; set; }
        public IFormFile formFileImage { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
