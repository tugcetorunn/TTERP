using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class Town : BaseEntity<int>
    {
        public int CityId { get; set; }
        public string Name { get; set; } = null!;
        public City? City { get; set; } = null!;
        public ICollection<District>? Districts { get; set; } = new List<District>();
    }
}