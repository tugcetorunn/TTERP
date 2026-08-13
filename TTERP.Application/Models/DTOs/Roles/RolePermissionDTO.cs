namespace TTERP.Application.Models.DTOs.Roles
{
    public class RolePermissionDTO
    {
        public int PermissionId { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Module { get; set; } = null!;
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsAssigned { get; set; }
    }
}