namespace SchoolManagementSystem.Domain.Entities;

public class Role : TenantEntity
{
    public string RoleName { get; set; }
    public string Description { get; set; }
    public int RoleType { get; set; }
    public DateTime? DeleteRequestedOn { get; set; }
    public virtual Tenant Tenant { get; set; }
    public virtual ICollection<UserRole> UserRoleList { get; set; }
    public virtual ICollection<RoleMenu> RoleMenuList { get; set; }
}