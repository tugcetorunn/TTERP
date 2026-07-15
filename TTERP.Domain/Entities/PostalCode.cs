using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class PostalCode : BaseEntity<int>
    {
        public string Code { get; set; } = null!;
        public ICollection<Neighborhood>? Neighborhoods { get; set; } = new List<Neighborhood>();
    }
}