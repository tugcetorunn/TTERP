using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class Category : BaseEntity<int>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}