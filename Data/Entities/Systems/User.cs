using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CMS.Data.Entities.Systems;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateOnly Dob { get; set; }
    public string? Avatar { get; set; }
    public string? Background { get; set; }
    public string? Introduction { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal AccountBalance { get; set; }
    public int NumberOfPosts { get; set; }
    public int NumberOfFollowers { get; set; }
    public int NumberOfFollowing { get; set; }
    public string? WebsiteUrl { get; set; }
    public string? Job { get; set; }
    public string? Company { get; set; }
    public string? Address { get; set; }
    public UserSetting? UserSetting { get; set; }
}