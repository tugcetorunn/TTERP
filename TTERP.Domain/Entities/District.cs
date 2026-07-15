using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class District : BaseEntity<int>
    {
        public int TownId { get; set; }
        public string Name { get; set; } = null!;
        public Town? Town { get; set; } = null!;
        public ICollection<Neighborhood>? Neighborhoods { get; set; } = new List<Neighborhood>();
    }
}