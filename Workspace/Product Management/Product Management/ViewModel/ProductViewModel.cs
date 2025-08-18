using Product_Management.DTO;

namespace Product_Management.ViewModel
{
    public class ProductViewModel : Paging
    {
        public List<CategoryDTO> Categories { get; set; }
        public List<ProductDTO> Products { get; set; }

        public ProductDTO Request { get; set; }
        public ProductDTO Response { get; set; }
        
    }
}
