using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CMS.Data.Entities.Systems;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateOnly Dob { get; set; }
    public string? Avatar { get; set; }
    public string? Introduction { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal AccountBalance { get; set; }
    public UserSetting? UserSetting { get; set; }
}