using TTERP.Domain.Entities.Common;

namespace TTERP.Domain.Entities
{
    public class Neighborhood : BaseEntity<int>
    {
        public int DistrictId { get; set; }
        public int? PostalCodeId { get; set; }
        public string Name { get; set; } = null!;
        public District? District { get; set; } = null!;
        public PostalCode? PostalCode { get; set; }
    }
}