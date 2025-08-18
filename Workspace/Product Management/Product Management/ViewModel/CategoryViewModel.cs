using Product_Management.DTO;

namespace Product_Management.ViewModel
{
    public class CategoryViewModel : Paging
    {
        public CategoryDTO Request { get; set; }
        public CategoryDTO Response { get; set; }

        public List<CategoryDTO> Categories { get; set; }
    }
}
