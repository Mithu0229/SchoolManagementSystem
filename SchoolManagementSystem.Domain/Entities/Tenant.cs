using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Domain.Entities;
public class Tenant : AuditableEntity
{
    public required string TenantName { get; set; }
    public string? BinNo { get; set; }
    public required string TenantEmail { get; set; }
    public required string PhoneNumber { get; set; }
    public string? Domain { get; set; }

    public string? Street { get; set; }
    public string? City { get; set; }
    public string? Province { get; set; }
    public string? PostCode { get; set; }
    public string? Reason { get; set; }
    public virtual ICollection<User> TenantUserList { get; set; } = new List<User>();
    public virtual ICollection<Role> TenantRoleList { get; set; } = new List<Role>();
}

public class IclockTransaction
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("emp_code")]
    [MaxLength(20)]
    public string EmpCode { get; set; } = string.Empty;

    [Required]
    [Column("punch_time")]
    public DateTime PunchTime { get; set; }

    [Required]
    [Column("punch_state")]
    [MaxLength(5)]
    public string PunchState { get; set; } = string.Empty;

    [Required]
    [Column("verify_type")]
    public int VerifyType { get; set; }

    [Column("work_code")]
    [MaxLength(20)]
    public string? WorkCode { get; set; }

    [Column("terminal_sn")]
    [MaxLength(50)]
    public string? TerminalSn { get; set; }

    [Column("terminal_alias")]
    [MaxLength(50)]
    public string? TerminalAlias { get; set; }

    [Column("area_alias")]
    [MaxLength(100)]
    public string? AreaAlias { get; set; }

    [Column("longitude")]
    public double? Longitude { get; set; }

    [Column("latitude")]
    public double? Latitude { get; set; }

    [Column("gps_location")]
    public string? GpsLocation { get; set; }

    [Column("mobile")]
    [MaxLength(50)]
    public string? Mobile { get; set; }

    [Column("source")]
    public short? Source { get; set; }

    [Column("purpose")]
    public short? Purpose { get; set; }

    [Column("crc")]
    [MaxLength(100)]
    public string? Crc { get; set; }

    [Column("is_attendance")]
    public short? IsAttendance { get; set; }

    [Column("reserved")]
    [MaxLength(100)]
    public string? Reserved { get; set; }

    [Column("upload_time")]
    public DateTime? UploadTime { get; set; }

    [Column("sync_status")]
    public short? SyncStatus { get; set; }

    [Column("sync_time")]
    public DateTime? SyncTime { get; set; }

    [Column("is_mask")]
    public short? IsMask { get; set; }

    [Column("temperature", TypeName = "numeric(4,1)")]
    public decimal? Temperature { get; set; }

    [Column("emp_id")]
    public int? EmpId { get; set; }

    [Column("terminal_id")]
    public int? TerminalId { get; set; }

    [Column("company_code")]
    [MaxLength(50)]
    public string? CompanyCode { get; set; }
}