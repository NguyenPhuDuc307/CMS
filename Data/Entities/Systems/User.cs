using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace CMS.Data.Entities.Systems;

public class User : IdentityUser
{
    public UserSetting? UserSetting { get; set; }
}