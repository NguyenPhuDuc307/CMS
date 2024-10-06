using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CMS.Data.Entities.Systems;

public class Role : IdentityRole
{
    public ICollection<Permission>? Permissions { get; set; }
}